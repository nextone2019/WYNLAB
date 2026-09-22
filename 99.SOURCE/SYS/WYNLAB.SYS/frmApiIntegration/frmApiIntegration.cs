using System.Drawing;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.SYS;

/// <summary>
/// 외부 API 연동 정의/실행/이력 관리(2026-09-15, Developer Tool 메뉴). TSMAPIDEF에 등록된
/// 연동(현재는 EXIM_FX 하나) 목록을 왼쪽 그리드로 보여주고, 오른쪽에서 URL/인증키/실행시각을
/// 편집 + "지금 실행" + 최근 실행이력을 확인한다.
///
/// 연동 종류 자체(신규 연동 추가)는 이 화면에서 만들지 않는다 - 새 연동은 서버에
/// IExternalApiIntegration 구현체를 하나 추가하고 마이그레이션으로 TSMAPIDEF에 한 줄 넣는
/// 것으로 완성되므로(ApiIntegrationController.cs 설명 참고), 이 화면은 이미 등록된 연동의
/// "설정값 편집 + 수동실행 + 이력조회"만 담당한다 - 그래서 다른 화면들과 달리 신규/삭제
/// 버튼이 없다(frmSiteConfig와 같은 이유 - 목록의 행 자체를 이 화면에서 늘리고 줄이지 않음).
///
/// 저장된 인증키는 화면에 절대 다시 보여주지 않는다(서버가 애초에 돌려주지 않음 - HasAuthKey
/// 불리언만 옴). "새 인증키" 입력칸은 비워두면 기존 값 유지, 채우면 그 값으로 교체한다.
/// </summary>
public class frmApiIntegration : BaseForm
{
    private readonly GridControlWyn grdList = new();
    private readonly GridViewWyn gvwList = new();
    private readonly GridControlWyn grdLog = new();
    private readonly GridViewWyn gvwLog = new();

    private readonly Panel detailPanel = new() { Dock = DockStyle.Top, Height = 280, Padding = new Padding(16) };

    private readonly LabelControl lblApiCd = new() { AutoSize = true };
    private readonly TextEdit txtApiNm = new();
    private readonly TextEdit txtBaseUrl = new();
    private readonly LabelControl lblAuthKeyStatus = new() { AutoSize = true };
    private readonly TextEdit txtNewAuthKey = new();
    private readonly TextEdit txtRunTime = new();
    private readonly CheckEdit chkEnabled = new() { Text = "사용" };
    private readonly MemoEditWyn txtDescription = new();
    private readonly ButtonWyn btnSave = new() { Text = "저장" };
    private readonly ButtonWyn btnRunNow = new() { Text = "지금 실행" };

    private List<ApiIntegrationDto> _items = new();
    private string? _selectedApiCd;

    public frmApiIntegration()
    {
        Text = "API 연동관리";

        BuildListGrid();
        BuildDetailPanel();
        BuildLogGrid();

        var rightContainer = new Panel { Dock = DockStyle.Fill };
        rightContainer.Controls.Add(grdLog);
        rightContainer.Controls.Add(new SplitterWyn { Dock = DockStyle.Top });
        rightContainer.Controls.Add(detailPanel);

        var leftPanel = new Panel { Dock = DockStyle.Left, Width = 360 };
        leftPanel.Controls.Add(grdList);

        var mainContainer = new Panel { Dock = DockStyle.Fill };
        mainContainer.Controls.Add(rightContainer);
        mainContainer.Controls.Add(new SplitterWyn { Dock = DockStyle.Left });
        mainContainer.Controls.Add(leftPanel);

        Controls.Add(mainContainer);
        Controls.Add(BuildScreenHeader());

        txtNewAuthKey.Properties.UseSystemPasswordChar = true;

        btnSave.Click += async (s, e) => await SaveClick();
        btnRunNow.Click += async (s, e) => await RunNowClick();
        gvwList.FocusedRowChanged += (s, e) => LoadSelectedIntoDetail();

        TrackDirty(detailPanel);
        SetDetailEnabled(false);

        Load += async (s, e) => await QueryClick();
    }

    private void BuildListGrid()
    {
        grdList.Dock = DockStyle.Fill;
        grdList.MainView = gvwList;
        grdList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gvwList });

        gvwList.GridControl = grdList;
        gvwList.OptionsBehavior.Editable = false;
        gvwList.OptionsView.ColumnAutoWidth = false;
        gvwList.OptionsView.ShowGroupPanel = false;
        gvwList.EmptyText = "등록된 연동이 없습니다.";

        gvwList.Columns.AddRange(new[]
        {
            new GridColumn { FieldName = nameof(ApiIntegrationDto.ApiCd), Caption = "연동코드", VisibleIndex = 0, Width = 90 },
            new GridColumn { FieldName = nameof(ApiIntegrationDto.ApiNm), Caption = "연동명", VisibleIndex = 1, Width = 130 },
            new GridColumn { FieldName = nameof(ApiIntegrationDto.EnabledYn), Caption = "사용", VisibleIndex = 2, Width = 40 },
            new GridColumn
            {
                FieldName = nameof(ApiIntegrationDto.LastRunDt), Caption = "마지막 실행", VisibleIndex = 3, Width = 150,
                DisplayFormat = { FormatType = DevExpress.Utils.FormatType.DateTime, FormatString = "yyyy-MM-dd HH:mm:ss" },
            },
            new GridColumn { FieldName = nameof(ApiIntegrationDto.LastRunResultCd), Caption = "결과", VisibleIndex = 4, Width = 50 },
        });
        foreach (GridColumn col in gvwList.Columns) col.Visible = true;
    }

    private void BuildLogGrid()
    {
        grdLog.Dock = DockStyle.Fill;
        grdLog.MainView = gvwLog;
        grdLog.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gvwLog });

        gvwLog.GridControl = grdLog;
        gvwLog.OptionsBehavior.Editable = false;
        gvwLog.OptionsView.ColumnAutoWidth = false;
        gvwLog.OptionsView.ShowGroupPanel = false;
        gvwLog.EmptyText = "실행 이력이 없습니다.";

        gvwLog.Columns.AddRange(new[]
        {
            new GridColumn
            {
                FieldName = nameof(ApiIntegrationLogDto.RunDt), Caption = "실행시각", VisibleIndex = 0, Width = 150,
                DisplayFormat = { FormatType = DevExpress.Utils.FormatType.DateTime, FormatString = "yyyy-MM-dd HH:mm:ss" },
            },
            new GridColumn { FieldName = nameof(ApiIntegrationLogDto.ResultCd), Caption = "결과", VisibleIndex = 1, Width = 50 },
            new GridColumn { FieldName = nameof(ApiIntegrationLogDto.RowCnt), Caption = "건수", VisibleIndex = 2, Width = 50 },
            new GridColumn { FieldName = nameof(ApiIntegrationLogDto.ElapsedMs), Caption = "소요(ms)", VisibleIndex = 3, Width = 70 },
            new GridColumn { FieldName = nameof(ApiIntegrationLogDto.Message), Caption = "메시지", VisibleIndex = 4, Width = 260 },
        });
        foreach (GridColumn col in gvwLog.Columns) col.Visible = true;
    }

    // Panel.Padding은 Dock/Anchor 레이아웃 대상에만 적용되고, Location을 직접 지정하는 자식에는
    // 적용되지 않는다(frmSiteConfig도 같은 함정을 AutoScroll=true로 피해갔음) - 그래서 여기서는
    // Padding 대신 여백을 좌표에 직접 반영한다. 이게 없으면 첫 줄(연동코드/연동명)이 패널 맨
    // 위/왼쪽 끝에 그대로 붙어서, 화면 타이틀 바 바로 아래에 여백 없이 붙어 보인다(2026-09-15
    // 사용자 피드백 - "화면이 제목에 가려지는 부분들").
    private const int MarginX = 16;
    private const int MarginY = 12;

    private void BuildDetailPanel()
    {
        var y = MarginY;
        detailPanel.Controls.Add(lblApiCd);
        lblApiCd.Location = new Point(MarginX, y);
        lblApiCd.Font = new Font(lblApiCd.Font, FontStyle.Bold);
        y += 26;

        AddRow(detailPanel, "연동명", txtApiNm, ref y, 240, MarginX);
        AddRow(detailPanel, "URL", txtBaseUrl, ref y, 420, MarginX);

        detailPanel.Controls.Add(MakeLabel("인증키", new Point(MarginX, y + 4)));
        lblAuthKeyStatus.Location = new Point(MarginX + 160, y + 4);
        detailPanel.Controls.Add(lblAuthKeyStatus);
        y += 24;
        AddRow(detailPanel, "새 인증키(변경시에만 입력)", txtNewAuthKey, ref y, 240, MarginX);

        AddRow(detailPanel, "실행시각(HH:mm, 비우면 수동실행만)", txtRunTime, ref y, 80, MarginX);

        chkEnabled.Location = new Point(MarginX + 160, y);
        chkEnabled.AutoSize = true;
        detailPanel.Controls.Add(chkEnabled);
        y += 30;

        detailPanel.Controls.Add(MakeLabel("설명", new Point(MarginX, y + 4)));
        txtDescription.Location = new Point(MarginX + 160, y);
        txtDescription.Size = new Size(420, 50);
        detailPanel.Controls.Add(txtDescription);
        y += 60;

        btnSave.Location = new Point(MarginX + 160, y);
        btnSave.Size = new Size(80, 28);
        detailPanel.Controls.Add(btnSave);

        btnRunNow.Location = new Point(MarginX + 250, y);
        btnRunNow.Size = new Size(90, 28);
        detailPanel.Controls.Add(btnRunNow);
    }

    private static LabelControl MakeLabel(string text, Point location) =>
        new() { Text = text, Location = location, AutoSize = true };

    private static void AddRow(Control parent, string label, BaseEdit edit, ref int y, int width, int x = 0)
    {
        parent.Controls.Add(MakeLabel(label, new Point(x, y + 4)));
        edit.Location = new Point(x + 160, y);
        edit.Size = new Size(width, 20);
        parent.Controls.Add(edit);
        y += 28;
    }

    private void SetDetailEnabled(bool enabled)
    {
        foreach (var c in new Control[] { txtApiNm, txtBaseUrl, txtNewAuthKey, txtRunTime, chkEnabled, txtDescription, btnSave, btnRunNow })
            c.Enabled = enabled;
    }

    public override async Task QueryClick()
    {
        try
        {
            _items = await ApiClient.GetAsync<List<ApiIntegrationDto>>("api/system/api-integrations") ?? new();
            grdList.DataSource = null;
            grdList.DataSource = _items;
            LoadSelectedIntoDetail();
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"연동 목록 조회 중 오류가 발생했습니다.\n{ex.Message}", "오류");
        }
    }

    private void LoadSelectedIntoDetail()
    {
        var row = gvwList.GetFocusedRow() as ApiIntegrationDto;
        _selectedApiCd = row?.ApiCd;

        if (row == null)
        {
            SetDetailEnabled(false);
            lblApiCd.Text = "";
            grdLog.DataSource = null;
            return;
        }

        SuppressDirtyTracking(() =>
        {
            lblApiCd.Text = row.ApiCd;
            txtApiNm.Text = row.ApiNm;
            txtBaseUrl.Text = row.BaseUrl;
            lblAuthKeyStatus.Text = row.HasAuthKey ? "등록됨" : "미등록";
            lblAuthKeyStatus.ForeColor = row.HasAuthKey ? Color.FromArgb(30, 140, 60) : Color.FromArgb(200, 60, 40);
            txtNewAuthKey.Text = "";
            txtRunTime.Text = row.RunTime ?? "";
            chkEnabled.Checked = row.EnabledYn == "Y";
            txtDescription.Text = row.Description ?? "";
        });
        SetDetailEnabled(true);

        _ = LoadLogsAsync(row.ApiCd);
    }

    private async Task LoadLogsAsync(string apiCd)
    {
        try
        {
            var logs = await ApiClient.GetAsync<List<ApiIntegrationLogDto>>($"api/system/api-integrations/{apiCd}/logs") ?? new();
            grdLog.DataSource = null;
            grdLog.DataSource = logs;
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"실행 이력 조회 중 오류가 발생했습니다.\n{ex.Message}", "오류");
        }
    }

    public override async Task SaveClick()
    {
        if (_selectedApiCd == null) return;

        var runTime = txtRunTime.Text.Trim();
        if (runTime.Length > 0 && !System.Text.RegularExpressions.Regex.IsMatch(runTime, @"^([01]\d|2[0-3]):[0-5]\d$"))
        {
            AppMessageBox.Show("실행시각은 HH:mm 형식(예: 09:30)으로 입력하거나 비워두세요.", "확인");
            return;
        }

        var request = new SaveApiIntegrationRequest
        {
            ApiNm = txtApiNm.Text,
            BaseUrl = txtBaseUrl.Text,
            NewAuthKey = string.IsNullOrEmpty(txtNewAuthKey.Text) ? null : txtNewAuthKey.Text,
            RunTime = runTime.Length == 0 ? null : runTime,
            EnabledYn = chkEnabled.Checked ? "Y" : "N",
            Description = txtDescription.Text,
        };

        var result = await ApiClient.PutAsync<SaveApiIntegrationRequest, ApiResult>($"api/system/api-integrations/{_selectedApiCd}", request);
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        Toast.Show("저장되었습니다.");
        await QueryClick();
    }

    private async Task RunNowClick()
    {
        if (_selectedApiCd == null) return;

        btnRunNow.Enabled = false;
        try
        {
            var result = await ApiClient.PostAsync<object, ApiResult>($"api/system/api-integrations/{_selectedApiCd}/run", new { });
            AppMessageBox.Show(result?.Message ?? "실행 요청을 처리했습니다.", result?.Success == true ? "실행 완료" : "실행 실패");
            await QueryClick();
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"실행 요청 중 오류가 발생했습니다.\n{ex.Message}", "오류");
        }
        finally
        {
            btnRunNow.Enabled = true;
        }
    }
}
