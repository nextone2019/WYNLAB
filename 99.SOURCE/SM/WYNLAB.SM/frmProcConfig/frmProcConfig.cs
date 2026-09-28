using System.Data;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Grid;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.SM;

/// <summary>
/// 프로세스 설정 - 업무 처리 방식/허용 기준처럼 프로세스 동작을 바꾸는 값(TSMPROCCONFIG)을 관리하는 공통 화면.
/// 회사정보/메일/비밀번호 정책을 다루는 frmSiteConfig와는 별개다. 설정 "정의"(어떤 설정이 있는지, 타입/범위/
/// 기본값/설명)는 개발자가 마이그레이션으로 넣고, 여기서는 "값"만 바꾼다 - 그래서 행 추가/삭제가 없고, 새 설정이
/// 생기면 마이그레이션에 행 하나만 넣으면 이 화면에 그대로 나타난다.
///
/// 값 칸의 편집기는 행의 data_type마다 다르다(CustomRowCellEdit): ENUM=공통코드 콤보(사용 Y 항목만),
/// YN=체크, INT/DEC=숫자 마스크, TEXT=텍스트. 저장할 때 서버(USP_SM_PROCCONFIG_S)가 타입/범위/선택지를 다시
/// 검증하고, 값이 실제로 바뀐 것만 변경 이력(TSMPROCCONFIGHIST)에 남긴다. 아래 grd2는 선택한 설정의 그 이력이다.
/// </summary>
public partial class frmProcConfig : BaseForm
{
    private static readonly Dictionary<string, string> ModuleCodes = new()
    {
        ["구매"] = "MA", ["판매"] = "SA", ["생산"] = "PR", ["공통"] = "SM",
    };

    private DataTable _list = new();
    private DataTable _options = new();
    private readonly Dictionary<string, RepositoryItemLookUpEdit> _enumRepos = new();
    private readonly RepositoryItemCheckEdit _ynRepo = new() { ValueChecked = "Y", ValueUnchecked = "N", AutoHeight = false };
    private readonly RepositoryItemTextEdit _textRepo = new();
    private readonly RepositoryItemTextEdit _intRepo = new();
    private readonly RepositoryItemTextEdit _decRepo = new();

    public frmProcConfig()
    {
        InitializeComponent();

        Text = "프로세스설정";

        Controls.Add(BuildScreenHeader());

        cboSearchModule.SelectedIndex = 0;

        // 숫자 값은 문자열 컬럼(cur_value)에 그대로 들어가므로 마스크로 형식만 막고 범위는 서버가 검증한다.
        _intRepo.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
        _intRepo.Mask.EditMask = @"\d{0,9}";
        _intRepo.Mask.UseMaskAsDisplayFormat = false;
        _decRepo.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
        _decRepo.Mask.EditMask = @"\d{0,9}(\.\d{0,4})?";
        _decRepo.Mask.UseMaskAsDisplayFormat = false;
        foreach (var repo in new RepositoryItem[] { _ynRepo, _textRepo, _intRepo, _decRepo })
            grd1.RepositoryItems.Add(repo);

        gvw1.Role = GridRoleWyn.Edit; // 값 칸만 편집 가능(디자이너에서 나머지 컬럼 AllowEdit=false, 행 추가/삭제 불가)
        gvw1.HighlightFocusedRow = true;
        gvw1.CustomRowCellEdit += Gvw1_CustomRowCellEdit;
        gvw1.CellValueChanged += (s, e) => { if (e.Column == colCurValue && gvw1.GetDataRow(e.RowHandle) is { } row) UpdateRowState(row); };
        gvw1.FocusedRowObjectChanged += async (s, e) => await SafeExecuteAsync(LoadHistoryAsync, "변경 이력 조회");
        gvw1.PopupMenuShowing += Gvw1_PopupMenuShowing;

        gvw2.Role = GridRoleWyn.Query;
        gvw2.HighlightFocusedRow = true;

        cboSearchModule.EditValueChanged += async (s, e) => await SafeExecuteAsync(QueryClick, "조회");
        txtSearchKeyword.KeyDown += async (s, e) => { if (e.KeyCode == Keys.Enter) await SafeExecuteAsync(QueryClick, "조회"); };

        Load += async (s, e) => await QueryClick();
    }

    public override async Task QueryClick() => await QueryCore(keepKey: null);

    private async Task QueryCore(string? keepKey)
    {
        var moduleName = cboSearchModule.EditValue?.ToString();
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_module_cd"] = moduleName != null && ModuleCodes.TryGetValue(moduleName, out var cd) ? cd : null,
            ["p_keyword"] = txtSearchKeyword.Text,
        };
        _list = await QueryAsync("USP_SM_PROCCONFIG_Q", p);
        _options = await QueryAsync("USP_SM_PROCCONFIG_Q", new Dictionary<string, string?> { ["p_work_type"] = "Q2" });

        // 선택지가 바뀌었을 수 있으니(예: 자동 입고 옵션이 열림) 콤보 편집기를 새로 만든다.
        foreach (var repo in _enumRepos.Values) grd1.RepositoryItems.Remove(repo);
        _enumRepos.Clear();

        SuppressDirtyTracking(() =>
        {
            _list.Columns.Add("default_nm", typeof(string));
            _list.Columns.Add("state_nm", typeof(string));
            foreach (DataRow row in _list.Rows)
            {
                row["default_nm"] = DisplayOf(row["data_type"]?.ToString(), row["enum_major_cd"]?.ToString(), row["default_value"]?.ToString());
                row["state_nm"] = row["default_yn"]?.ToString() == "Y" ? "기본값" : "변경됨";
            }
            _list.AcceptChanges();
        });
        TrackDirty(_list);

        grd1.DataSource = _list;

        if (keepKey != null)
        {
            for (var i = 0; i < gvw1.RowCount; i++)
            {
                if (gvw1.GetDataRow(i)?["config_key"]?.ToString() != keepKey) continue;
                gvw1.FocusedRowHandle = i;
                break;
            }
        }
        await LoadHistoryAsync();
    }

    private void Gvw1_CustomRowCellEdit(object? sender, CustomRowCellEditEventArgs e)
    {
        if (e.Column != colCurValue) return;
        if (gvw1.GetRow(e.RowHandle) is not DataRowView view) return;

        e.RepositoryItem = view["data_type"]?.ToString() switch
        {
            "ENUM" => EnumRepo(view["enum_major_cd"]?.ToString() ?? string.Empty),
            "YN" => _ynRepo,
            "INT" => _intRepo,
            "DEC" => _decRepo,
            _ => _textRepo,
        };
    }

    private RepositoryItemLookUpEdit EnumRepo(string majorCd)
    {
        if (_enumRepos.TryGetValue(majorCd, out var repo)) return repo;

        var view = new DataView(_options) { RowFilter = $"major_cd = '{majorCd.Replace("'", "''")}'" };
        repo = new RepositoryItemLookUpEdit
        {
            DataSource = view,
            DisplayMember = "minor_nm",
            ValueMember = "minor_cd",
            NullText = string.Empty,
            ShowHeader = false,
            ShowFooter = false,
            TextEditStyle = TextEditStyles.DisableTextEditor,
        };
        repo.Columns.Add(new LookUpColumnInfo("minor_nm"));
        grd1.RepositoryItems.Add(repo);
        _enumRepos[majorCd] = repo;
        return repo;
    }

    /// <summary>화면에 보여줄 값 표시 - ENUM은 공통코드 이름, YN은 예/아니오, 나머지는 그대로.</summary>
    private string DisplayOf(string? dataType, string? enumMajorCd, string? value)
    {
        if (value == null) return string.Empty;
        if (dataType == "YN") return value == "Y" ? "예" : "아니오";
        if (dataType == "ENUM" && enumMajorCd != null)
        {
            var match = _options.Rows.Cast<DataRow>().FirstOrDefault(r =>
                r["major_cd"]?.ToString() == enumMajorCd && r["minor_cd"]?.ToString() == value);
            if (match != null) return match["minor_nm"]?.ToString() ?? value;
        }
        return value;
    }

    private void UpdateRowState(DataRow row)
    {
        row["state_nm"] = row["cur_value"]?.ToString() == row["default_value"]?.ToString() ? "기본값" : "변경됨";
    }

    private void Gvw1_PopupMenuShowing(object? sender, PopupMenuShowingEventArgs e)
    {
        if (e.MenuType != GridMenuType.Row || e.HitInfo == null || e.HitInfo.RowHandle < 0) return;

        var handle = e.HitInfo.RowHandle;
        e.Menu ??= new GridViewMenu(gvw1);
        e.Menu.Items.Add(new DXMenuItem("기본값으로 되돌리기", (s, a) =>
        {
            if (gvw1.GetDataRow(handle) is not { } row) return;
            row["cur_value"] = row["default_value"];
            UpdateRowState(row);
        }));
    }

    /// <summary>MAIN/SUB 그리드 관례 - 선택한 설정이 바뀌면 이력을 다시 읽고, 선택한 행이 없으면 비운다.</summary>
    private async Task LoadHistoryAsync()
    {
        if (gvw1.GetFocusedRow() is not DataRowView view) { grd2.DataSource = null; return; }

        var dataType = view["data_type"]?.ToString();
        var enumMajor = view["enum_major_cd"]?.ToString();
        var hist = await QueryAsync("USP_SM_PROCCONFIG_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q1",
            ["p_config_key"] = view["config_key"]?.ToString(),
        });

        hist.Columns.Add("old_nm", typeof(string));
        hist.Columns.Add("new_nm", typeof(string));
        foreach (DataRow r in hist.Rows)
        {
            r["old_nm"] = DisplayOf(dataType, enumMajor, r["old_value"]?.ToString());
            r["new_nm"] = DisplayOf(dataType, enumMajor, r["new_value"]?.ToString());
        }
        grd2.DataSource = hist;
    }

    public override async Task SaveClick()
    {
        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        var changed = _list.Rows.Cast<DataRow>()
            .Where(r => r.RowState == DataRowState.Modified
                        && !Equals(r["cur_value", DataRowVersion.Original]?.ToString(), r["cur_value", DataRowVersion.Current]?.ToString()))
            .ToList();
        if (changed.Count == 0)
        {
            Toast.Show("변경된 설정이 없습니다.");
            return;
        }

        // 프로세스 처리 경로가 바뀌는 값이라 저장 전에 이전/이후와 적용 시점(설명)을 한 번 더 보여준다.
        var lines = changed.Select(r =>
        {
            var type = r["data_type"]?.ToString();
            var major = r["enum_major_cd"]?.ToString();
            var before = DisplayOf(type, major, r["cur_value", DataRowVersion.Original]?.ToString());
            var after = DisplayOf(type, major, r["cur_value", DataRowVersion.Current]?.ToString());
            return $"■ {r["config_nm"]}: {before} → {after}\n   {r["description"]}";
        });
        var confirm = AppMessageBox.Show(
            "다음 프로세스 설정을 저장합니다.\n\n" + string.Join("\n\n", lines) + "\n\n저장하시겠습니까?",
            "프로세스 설정 저장", MessageBoxButtons.YesNo);
        if (confirm != DialogResult.Yes) return;

        foreach (var row in changed)
        {
            var result = await SaveAsync("USP_SM_PROCCONFIG_S", new Dictionary<string, string?>
            {
                ["p_work_type"] = "U",
                ["p_config_key"] = row["config_key"]?.ToString(),
                ["p_config_value"] = row["cur_value", DataRowVersion.Current]?.ToString(),
            });
            if (result == null || !result.Success)
            {
                AppMessageBox.Show(result?.Message ?? "저장에 실패했습니다.", "저장 실패");
                await QueryCore(keepKey: row["config_key"]?.ToString());
                return;
            }
        }

        Toast.Show("저장되었습니다.");
        await QueryCore(keepKey: (gvw1.GetFocusedRow() as DataRowView)?["config_key"]?.ToString());
    }

    // 설정은 개발자가 마이그레이션으로 정의한다 - 이 화면에서 추가/삭제하지 않는다.
    public override Task NewClick() => Task.CompletedTask;
    public override Task DeleteClick() => Task.CompletedTask;
    public override Task NewRowClick() => Task.CompletedTask;
    public override Task DeleteRowClick() => Task.CompletedTask;
}
