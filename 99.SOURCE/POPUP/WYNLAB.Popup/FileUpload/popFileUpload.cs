using DevExpress.XtraEditors;
using WYNLAB.Base;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Popup;

/// <summary>
/// 파일업로드 공통 팝업(TSMFILE 기반) - 다른 화면들의 MDI 업무화면(BaseForm)과 달리, 여러
/// 화면에서 모달로 띄워 쓰는 독립 팝업이라 popPopUp과 같은 이유로 BaseForm이 아니라
/// XtraForm을 직접 상속한다. 호출측은 이 클래스 내부를 몰라도 되도록 ShowAsync 하나만 부른다
/// (popPopUp.ShowAsync와 같은 모양).
///
/// 서버 통로는 api/data/query|save(GenericDataRepository)가 아니라 전용 api/files/*
/// (FilesController)다 - 이 팝업은 거래처/품목 등 어느 화면에서나 열리는 공용 기능이라
/// 특정 메뉴의 PROC_PREFIX/권한에 묶이면 안 되기 때문(LookupsController와 같은 이유,
/// project_wynlab_popup_lookup_framework 참고).
///
/// 업로드는 파일을 통째로 한 번에 안 보내고 청크로 나눠 보낸다(init -> chunk*N -> complete,
/// 2026-09-06 지시 - "청크로 쪼개면 용량 제한 없지 않아?"). IIS(requestFiltering)와
/// ASP.NET Core(IISServerOptions.MaxRequestBodySize) 기본 요청 크기 제한이 약 28.6MB~30MB라
/// 파일 하나를 통째로 보내면 그 크기부터 막힌다 - web.config를 안 건드리고 청크 하나를 그
/// 제한보다 훨씬 작게(FilesController.ChunkSizeBytes, 4MB) 잡아서 우회한다.
/// </summary>
public partial class popFileUpload : XtraForm
{
    private const string DefaultFileType = "GENERAL";

    /// <summary>FilesController.ChunkSizeBytes와 반드시 같은 값이어야 한다 - 재시도(기존
    /// file_id로 이어서 청크를 보내는 경로)는 init을 다시 안 부르므로 서버가 ChunkSize를
    /// 알려줄 기회가 없다(새 업로드는 init 응답의 ChunkSize를 그대로 쓰므로 이 상수에
    /// 의존하지 않는다).</summary>
    private const int RetryChunkSizeBytes = 4 * 1024 * 1024;

    private readonly string _docType;
    private readonly long _docId;
    private readonly string _docNo;
    private readonly int _docSerl;
    private readonly string? _formId;
    private List<FileListItemDto> _items = new();

    private popFileUpload(string docType, long docId, string docNo, int docSerl, string? formId)
    {
        InitializeComponent();
        WYNLAB.Base.Controls.GridSortSupport.Enable(gvw1); // 헤더 클릭 정렬 - 모든 그리드 공통(GridViewWyn이 아닌 기본 GridView)

        _docType = docType;
        _docId = docId;
        _docNo = docNo;
        _docSerl = docSerl;
        _formId = formId;

        lblTarget.Text = $"대상 : {docNo}";

        btnSelectFile.Click += async (s, e) => await SelectAndUploadAsync();
        btnDownload.Click += async (s, e) => await DownloadSelectedAsync();
        btnDelete.Click += async (s, e) => await DeleteSelectedAsync();
        btnRetry.Click += async (s, e) => await RetrySelectedAsync();
        btnClose.Click += (s, e) => Close();

        // 체크박스 컬럼(gridColumn19) 헤더에 전체선택용 체크박스를 직접 그린다 - DevExpress
        // v21.2 GridColumn에는 헤더 체크박스 내장 기능이 없어서(리플렉션으로 확인함) 수동으로
        // 그리고 클릭을 직접 잡는다(2026-09-06 요청 - "헤더에 체크박스 그려서... 일괄선택").
        gvw1.CustomDrawColumnHeader += (s, e) =>
        {
            if (e.Column != gridColumn19) return;
            e.Painter.DrawObject(e.Info);
            e.Handled = true;
            DrawHeaderCheckBox(e.Graphics, e.Bounds);
        };
        gvw1.MouseUp += (s, e) =>
        {
            var hit = gvw1.CalcHitInfo(new Point(e.X, e.Y));
            if (hit.InColumn && hit.Column == gridColumn19) ToggleSelectAll();
        };
        // 행 하나씩 체크할 때도 헤더 체크박스 상태(전체 체크 여부)를 바로 갱신한다.
        gvw1.CellValueChanged += (s, e) =>
        {
            if (e.Column == gridColumn19) gvw1.InvalidateColumnHeader(gridColumn19);
        };

        // FILE SIZE를 바이트 그대로 안 보여주고 KB/MB 단위로 바꿔 보여준다(2026-09-06 요청).
        gvw1.CustomColumnDisplayText += (s, e) =>
        {
            if (e.Column == gridColumn9 && e.Value is long bytes)
                e.DisplayText = FileSizeFormatter.Format(bytes);
        };

        // 드래그앤드롭 - 별도 영역(panDrop) 없이 grd1에 직접 끌어다 놔도 업로드되게 한다
        // (2026-09-06 요청 - "Grd1에 바로 드래그앤드랍하면 안돼?"). GridControlWyn도 평범한
        // Control이라 다른 컨트롤과 똑같이 AllowDrop/DragEnter/DragDrop이 그대로 동작한다.
        EnableFileDrop(grd1);

        // panDrop을 없앤 대신, 그리드의 빈 공간(행이 없는 영역)에 안내 문구를 직접 그린다 -
        // DevExpress GridView 전용 훅(CustomDrawEmptyForeground)으로, 데이터가 있는 행 부분은
        // 안 건드리고 빈 여백에만 그려진다.
        gvw1.CustomDrawEmptyForeground += (s, e) =>
        {
            const string hint = "파일을 여기로 끌어다 놓거나 위쪽 '파일 선택' 버튼으로 추가하세요.";
            using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near };
            e.Graphics.DrawString(hint, gvw1.Appearance.Row.Font, Brushes.Gray,
                new RectangleF(e.Bounds.Left, e.Bounds.Top + 16, e.Bounds.Width, 40), format);
        };

        Load += async (s, e) => await QueryAsync();
    }

    /// <summary>doc_type/doc_id(+doc_no 표시용/doc_serl)로 이 문서의 첨부파일 팝업을 모달로 띄운다.
    /// 닫히면(성공/취소 구분 없이) 호출측이 자기 화면의 파일목록(grdFile 등)을 새로 조회하면 된다 -
    /// 팝업 안에서 업로드/삭제/재시도가 몇 번이든 일어날 수 있어 "뭐가 바뀌었는지"를 일일이
    /// 돌려주기보다 호출측이 그냥 한 번 더 조회하는 편이 단순하다.</summary>
    public static void ShowAsync(string docType, long docId, string docNo, int docSerl, Control owner)
    {
        var ownerForm = owner.FindForm();
        // 어느 화면에서 업로드했는지(TSMFILE.form_id)를 호출측이 매번 문자열로 넘길 필요 없이
        // owner의 실제 폼 타입 이름으로 자동으로 채운다(2026-09-06 요청 - "어느 화면에서
        // 업로드 했는지 form_id에 남겨줘").
        var formId = ownerForm?.GetType().Name;

        using var form = new popFileUpload(docType, docId, docNo, docSerl, formId);
        if (ownerForm != null) form.ShowDialog(ownerForm);
        else form.ShowDialog();
    }

    private void EnableFileDrop(Control control)
    {
        control.AllowDrop = true;
        control.DragEnter += (s, e) =>
        {
            e.Effect = e.Data?.GetDataPresent(DataFormats.FileDrop) == true
                ? DragDropEffects.Copy
                : DragDropEffects.None;
        };
        control.DragDrop += async (s, e) =>
        {
            if (e.Data?.GetData(DataFormats.FileDrop) is string[] paths)
                await UploadNewFilesAsync(paths);
        };
    }

    /// <summary>삭제/다운로드 등 "선택된 항목 일괄 처리"의 대상 목록을 정한다 - 체크박스로 표시한
    /// 행이 있으면 그것들, 하나도 체크 안 했으면 지금 포커스된 행 하나를 자동으로 체크해서
    /// 그 행만 처리한다(2026-09-06 요청). CloseEditor/UpdateCurrentRow로 편집 중인 체크박스
    /// 값을 먼저 커밋해야 방금 클릭한 체크가 _items에 반영된다.</summary>
    private List<FileListItemDto> GetTargetRows()
    {
        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        var targets = _items.Where(i => i.IsChecked).ToList();
        if (targets.Count == 0 && gvw1.GetFocusedRow() is FileListItemDto focused)
        {
            focused.IsChecked = true;
            gvw1.RefreshData();
            gvw1.InvalidateColumnHeader(gridColumn19);
            targets.Add(focused);
        }

        return targets;
    }

    /// <summary>헤더 체크박스 클릭 - 하나라도 안 체크된 행이 있으면 전부 체크하고, 이미 전부
    /// 체크되어 있으면 전부 해제한다(2026-09-06 요청 - "헤더에 체크박스 그려서... 일괄선택").</summary>
    private void ToggleSelectAll()
    {
        if (_items.Count == 0) return;

        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        var selectAll = _items.Any(i => !i.IsChecked);
        foreach (var item in _items) item.IsChecked = selectAll;
        gvw1.RefreshData();
        gvw1.InvalidateColumnHeader(gridColumn19);
    }

    /// <summary>gridColumn19(체크박스) 헤더 한가운데에 작은 체크박스를 직접 그린다 - 전부
    /// 체크됐으면 체크 표시까지 그린다.</summary>
    private void DrawHeaderCheckBox(Graphics graphics, Rectangle headerBounds)
    {
        const int boxSize = 14;
        var rect = new Rectangle(
            headerBounds.Left + (headerBounds.Width - boxSize) / 2,
            headerBounds.Top + (headerBounds.Height - boxSize) / 2,
            boxSize, boxSize);

        graphics.FillRectangle(Brushes.White, rect);
        graphics.DrawRectangle(Pens.Gray, rect);

        var allChecked = _items.Count > 0 && _items.All(i => i.IsChecked);
        if (allChecked)
        {
            using var pen = new Pen(Color.SeaGreen, 2);
            graphics.DrawLine(pen, rect.Left + 3, rect.Top + 7, rect.Left + 6, rect.Bottom - 3);
            graphics.DrawLine(pen, rect.Left + 6, rect.Bottom - 3, rect.Right - 2, rect.Top + 3);
        }
    }

    private async Task QueryAsync()
    {
        try
        {
            var url = $"api/files?docType={Uri.EscapeDataString(_docType)}&docId={_docId}&docSerl={_docSerl}";
            _items = await ApiClient.GetAsync<List<FileListItemDto>>(url) ?? new List<FileListItemDto>();
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"목록 조회 중 오류가 발생했습니다.\n{ex.Message}", "오류");
            _items = new List<FileListItemDto>();
        }

        grd1.DataSource = null;
        grd1.DataSource = _items;
        gvw1.InvalidateColumnHeader(gridColumn19);

        var failCount = _items.Count(i => i.FailYn == "Y");
        lblSummary.Text = failCount > 0 ? $"총 {_items.Count}건 (실패 {failCount}건)" : $"총 {_items.Count}건";
    }

    private async Task SelectAndUploadAsync()
    {
        using var dialog = new OpenFileDialog { Multiselect = true, Title = "첨부할 파일 선택" };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;

        await UploadNewFilesAsync(dialog.FileNames);
    }

    private async Task UploadNewFilesAsync(IEnumerable<string> filePaths)
    {
        foreach (var filePath in filePaths)
        {
            var fileInfo = new FileInfo(filePath);

            FileInitUploadResponse? initResp;
            try
            {
                initResp = await ApiClient.PostAsync<FileInitUploadRequest, FileInitUploadResponse>("api/files/init", new FileInitUploadRequest
                {
                    DocType = _docType,
                    DocId = _docId,
                    DocNo = _docNo,
                    DocSerl = _docSerl,
                    FileType = DefaultFileType,
                    FileNm = fileInfo.Name,
                    FileSize = fileInfo.Length,
                    MimeType = "application/octet-stream",
                    FormId = _formId,
                });
            }
            catch (Exception ex)
            {
                AppMessageBox.Show($"'{fileInfo.Name}' 업로드 시작 중 오류가 발생했습니다.\n{ex.Message}", "오류");
                continue;
            }

            if (initResp == null || !initResp.Success)
            {
                AppMessageBox.Show($"'{fileInfo.Name}' 업로드 실패\n{initResp?.Message}", "업로드 실패");
                continue;
            }

            await UploadChunkedAsync(initResp.FileId, filePath, initResp.ChunkSize);
        }

        await QueryAsync();
    }

    /// <summary>지정된 로컬 파일을 fileId(이미 api/files/init으로 등록된 TSMFILE 행)에
    /// chunkSize만큼씩 나눠 올리고 마지막에 완료 처리한다. 청크 전송 전에 서버가 지금까지 받은
    /// 바이트 수(staged-bytes)를 먼저 확인해서, 이미 받은 만큼은 다시 안 보낸다(이어올리기) -
    /// 새 업로드는 항상 0부터지만, 재시도(같은 fileId로 다시 호출)는 중간에 끊긴 지점부터
    /// 이어간다.</summary>
    private async Task<bool> UploadChunkedAsync(long fileId, string localFilePath, int chunkSize)
    {
        long resumeFrom = 0;
        try
        {
            var staged = await ApiClient.GetAsync<StagedBytesResponse>($"api/files/{fileId}/staged-bytes");
            resumeFrom = staged?.Bytes ?? 0;
        }
        catch { /* 조회 실패해도 처음부터 다시 보내면 되므로 무시 */ }

        using var stream = File.OpenRead(localFilePath);
        var totalChunks = Math.Max(1, (int)Math.Ceiling((double)stream.Length / chunkSize));
        var doneChunks = 0;

        if (resumeFrom > 0 && resumeFrom <= stream.Length)
        {
            stream.Seek(resumeFrom, SeekOrigin.Begin);
            doneChunks = (int)(resumeFrom / chunkSize);
        }

        progressUpload.Properties.Minimum = 0;
        progressUpload.Properties.Maximum = totalChunks;
        progressUpload.Position = doneChunks;
        progressUpload.Visible = true;

        try
        {
            var buffer = new byte[chunkSize];
            int bytesRead;
            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                doneChunks++;
                progressUpload.Position = doneChunks;
                lblSummary.Text = $"업로드 중... {doneChunks}/{totalChunks} 청크 ({Path.GetFileName(localFilePath)})";

                var sent = false;
                Exception? lastEx = null;
                for (var attempt = 1; attempt <= 3 && !sent; attempt++)
                {
                    try
                    {
                        await ApiClient.UploadChunkAsync($"api/files/{fileId}/chunk", buffer, bytesRead);
                        sent = true;
                    }
                    catch (Exception ex)
                    {
                        lastEx = ex;
                        if (attempt < 3) await Task.Delay(500 * attempt);
                    }
                }

                if (!sent)
                {
                    AppMessageBox.Show(
                        $"청크 업로드에 실패했습니다({doneChunks}/{totalChunks}).\n{lastEx?.Message}\n목록에서 재시도할 수 있습니다.",
                        "업로드 실패");
                    return false;
                }
            }

            try
            {
                var completeResult = await ApiClient.PostAsync<object?, FileUploadResponse>($"api/files/{fileId}/complete", null);
                if (completeResult == null || !completeResult.Success)
                {
                    AppMessageBox.Show(completeResult?.Message ?? "업로드 완료 처리에 실패했습니다.", "업로드 실패");
                    return false;
                }
            }
            catch (Exception ex)
            {
                AppMessageBox.Show($"업로드 완료 처리 중 오류가 발생했습니다.\n{ex.Message}", "오류");
                return false;
            }

            return true;
        }
        finally
        {
            progressUpload.Visible = false;
        }
    }

    private async Task RetrySelectedAsync()
    {
        if (gvw1.GetFocusedRow() is not FileListItemDto row)
        {
            AppMessageBox.Show("재시도할 행을 먼저 선택하세요.", "확인");
            return;
        }
        if (row.FailYn != "Y")
        {
            AppMessageBox.Show("실패한 파일만 재시도할 수 있습니다.", "확인");
            return;
        }

        using var dialog = new OpenFileDialog { Title = $"'{row.FileNm}' 다시 선택", Multiselect = false };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;

        try
        {
            await ApiClient.PostAsync<object?, ApiResult>($"api/files/{row.FileId}/mark-retry", null);
        }
        catch { /* 재시도 횟수 기록은 부가 정보일 뿐 - 실패해도 실제 재업로드는 계속 진행한다 */ }

        await UploadChunkedAsync(row.FileId, dialog.FileName, RetryChunkSizeBytes);
        await QueryAsync();
    }

    /// <summary>다운로드 전용 임시 폴더 - 인터넷 브라우저의 임시 인터넷파일 폴더와 같은 원리로,
    /// 다운로드할 때마다 이 폴더를 비우고 새로 받는다(2026-09-06 요청) - 어디에 저장할지 매번
    /// 사용자에게 묻지 않는다.</summary>
    private static string DownloadTempDir => Path.Combine(Path.GetTempPath(), "WYNLAB_FileDownload");

    private async Task DownloadSelectedAsync()
    {
        var targets = GetTargetRows();
        if (targets.Count == 0)
        {
            AppMessageBox.Show("다운로드할 항목을 먼저 선택하세요.", "확인");
            return;
        }

        var failed = targets.Where(r => r.FailYn == "Y").ToList();
        if (failed.Count > 0)
        {
            AppMessageBox.Show($"업로드에 실패한 파일은 다운로드할 수 없어 제외합니다({failed.Count}건). 먼저 재시도해주세요.", "확인");
            targets = targets.Except(failed).ToList();
        }
        if (targets.Count == 0) return;

        try
        {
            if (Directory.Exists(DownloadTempDir)) Directory.Delete(DownloadTempDir, true);
            Directory.CreateDirectory(DownloadTempDir);
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"임시 폴더를 준비하는 중 오류가 발생했습니다.\n{ex.Message}", "오류");
            return;
        }

        var usedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var failCount = 0;
        foreach (var row in targets)
        {
            try
            {
                var (bytes, _) = await ApiClient.DownloadAsync($"api/files/{row.FileId}/download");

                var fileNm = row.FileNm ?? $"file_{row.FileId}";
                // 같은 배치 안에서 파일명이 겹치면(같은 문서에 동명 파일 두 개 첨부된 경우)
                // FileId를 앞에 붙여 구분한다 - 매번 비우는 임시 폴더라 평소엔 그냥 원래 이름 그대로.
                if (!usedNames.Add(fileNm)) fileNm = $"{row.FileId}_{fileNm}";

                File.WriteAllBytes(Path.Combine(DownloadTempDir, fileNm), bytes);
            }
            catch (Exception ex)
            {
                failCount++;
                AppMessageBox.Show($"'{row.FileNm}' 다운로드 중 오류가 발생했습니다.\n{ex.Message}", "오류");
            }
        }

        if (failCount < targets.Count)
        {
            Toast.Show($"{targets.Count - failCount}건 다운로드되었습니다.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("explorer.exe", $"\"{DownloadTempDir}\"") { UseShellExecute = true });
        }
    }

    private async Task DeleteSelectedAsync()
    {
        var targets = GetTargetRows();
        if (targets.Count == 0)
        {
            AppMessageBox.Show("삭제할 항목을 먼저 선택하세요.", "확인");
            return;
        }

        var confirmMessage = targets.Count == 1
            ? $"'{targets[0].FileNm}' 파일을 삭제하시겠습니까?"
            : $"선택한 {targets.Count}건을 삭제하시겠습니까?";
        if (AppMessageBox.Show(confirmMessage, "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        var failCount = 0;
        foreach (var row in targets)
        {
            try
            {
                var result = await ApiClient.DeleteAsync<ApiResult>($"api/files/{row.FileId}");
                if (result == null || !result.Success)
                {
                    failCount++;
                    AppMessageBox.Show($"'{row.FileNm}' 삭제 실패\n{result?.Message}", "삭제 실패");
                }
            }
            catch (Exception ex)
            {
                failCount++;
                AppMessageBox.Show($"'{row.FileNm}' 삭제 중 오류가 발생했습니다.\n{ex.Message}", "오류");
            }
        }

        if (failCount < targets.Count)
            Toast.Show($"{targets.Count - failCount}건 삭제되었습니다.");

        await QueryAsync();
    }
}
