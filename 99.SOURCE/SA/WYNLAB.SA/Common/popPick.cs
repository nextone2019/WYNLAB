using System.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.SA;

/// <summary>불러오기 팝업에 보여줄 컬럼 한 개(조회 프로시저 결과 컬럼명 + 캡션).</summary>
public sealed class PickColumn
{
    public string Field { get; }
    public string Caption { get; }
    public int Width { get; }
    public bool Numeric { get; }

    // net48 대상이라 record(init 접근자)를 못 쓴다 - 일반 클래스로 둔다.
    public PickColumn(string field, string caption, int width = 90, bool numeric = false)
    {
        Field = field;
        Caption = caption;
        Width = width;
        Numeric = numeric;
    }
}

/// <summary>
/// 영업 모듈 범용 불러오기 팝업 - 출하등록의 수주 불러오기/출하 LOT 불러오기에 쓴다. PR의 popPick과 같은 구조(모듈 간 참조를 피하려고 따로 둔다).
/// 조회 프로시저는 부른 화면 메뉴의 접두사(USP_SA_) 안에 있어야 하고 파라미터 규약은 고정이다:
///   p_work_type='Q', p_acc_id, p_date_from, p_date_to, p_doc_no, p_cust_id, p_keyword
/// validate 콜백을 넘기면 확인 때 체크한 행을 검사해서(예: 같은 작업지시/공정끼리만) 오류 문구가 있으면 닫지 않고 알려준다.
/// 배치는 popPick.Designer.cs(VS 디자이너로 편집 가능).
/// </summary>
public partial class popPick : XtraForm
{
    private readonly long _menuId;
    private readonly string _procName;
    private readonly string? _accId;
    private readonly Func<IReadOnlyList<DataRow>, string?>? _validate;
    private readonly string? _emptyHint;
    private readonly IReadOnlyList<KeyValuePair<string, string>>? _choices; // 조회 구분 선택지(값, 표시) - 있으면 날짜/문서번호 조건 대신 구분 콤보를 보여 준다
    private readonly string? _choiceParam;
    private DevExpress.XtraEditors.ComboBoxEdit? _cboChoice;
    private readonly IDictionary<string, string?>? _extra; // 조회 프로시저에 더 넘길 고정 파라미터(예: p_item_id)
    private DataTable _table = new();

    /// <summary>체크한 행들(조회 결과와 같은 컬럼 구성 + sel). 확인으로 닫았을 때만 채워진다.</summary>
    public DataTable Selected { get; private set; } = new();

    /// <summary>VS 디자이너 전용 생성자 - 실제 코드에서는 호출하지 않는다.</summary>
    private popPick() : this(0, string.Empty, string.Empty, string.Empty, Array.Empty<PickColumn>(), null, null)
    {
    }

    private popPick(long menuId, string title, string procName, string docNoLabel, IReadOnlyList<PickColumn> columns,
        string? accId, Func<IReadOnlyList<DataRow>, string?>? validate, string? emptyHint = null, IDictionary<string, string?>? extra = null,
        IReadOnlyList<KeyValuePair<string, string>>? choices = null, string? choiceParam = null, string? choiceLabel = null, string? choiceDefault = null)
    {
        InitializeComponent();
        _extra = extra;
        _choices = choices;
        _choiceParam = choiceParam;

        _menuId = menuId;
        _procName = procName;
        _accId = accId;
        _validate = validate;
        _emptyHint = emptyHint;

        Text = title;
        lblDocNo.Text = docNoLabel;

        // 조회 결과 컬럼(선택 체크 컬럼 뒤에 이어 붙인다)
        var index = 1;
        foreach (var c in columns)
        {
            var col = new GridColumn
            {
                Caption = c.Caption,
                FieldName = c.Field,
                Name = "col_" + c.Field,
                Width = c.Width,
                Visible = true,
                VisibleIndex = index++,
            };
            col.OptionsColumn.AllowEdit = false;
            if (c.Numeric)
            {
                col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                col.DisplayFormat.FormatString = "#,##0.####";
            }
            gvw1.Columns.Add(col);
        }

        dteFrom.YyyyMmDd = DateTime.Today.AddMonths(-3).ToString("yyyyMMdd");
        dteTo.YyyyMmDd = DateTime.Today.ToString("yyyyMMdd");

        gvw1.Role = GridRoleWyn.Edit; // sel 체크박스만 편집 가능(나머지 컬럼은 AllowEdit=false)
        gvw1.HighlightFocusedRow = true;

        btnSearch.Click += async (s, e) => await SearchAsync();
        btnOk.Click += (s, e) => Confirm();
        btnClose.Click += (s, e) => DialogResult = DialogResult.Cancel;
        chkAll.CheckedChanged += (s, e) => SetAll(chkAll.Checked);
        txtDocNo.KeyDown += async (s, e) => { if (e.KeyCode == Keys.Enter) await SearchAsync(); };
        txtKeyword.KeyDown += async (s, e) => { if (e.KeyCode == Keys.Enter) await SearchAsync(); };

        if (choices != null) BuildChoice(choiceLabel ?? "구분", choiceDefault);

        Shown += async (s, e) => await SearchAsync();
    }

    /// <summary>팝업을 띄우고 체크한 행을 돌려준다(취소하거나 아무것도 안 고르면 null).</summary>
    public static DataTable? Pick(IWin32Window owner, long menuId, string title, string procName, string docNoLabel,
        IReadOnlyList<PickColumn> columns, string? accId, Func<IReadOnlyList<DataRow>, string?>? validate = null, string? emptyHint = null,
        IDictionary<string, string?>? extra = null, IReadOnlyList<KeyValuePair<string, string>>? choices = null, string? choiceParam = null,
        string? choiceLabel = null, string? choiceDefault = null)
    {
        using var dlg = new popPick(menuId, title, procName, docNoLabel, columns, accId, validate, emptyHint, extra, choices, choiceParam, choiceLabel, choiceDefault);
        return dlg.ShowDialog(owner) == DialogResult.OK ? dlg.Selected : null;
    }

    /// <summary>조회 구분 콤보(예: 출하 LOT 선택의 전체/자사창고/외주처)를 검색 패널에 만든다. 날짜/문서번호 조건은 숨기고 그 자리에 놓는다. 구분을 바꾸면 바로 다시 조회한다.</summary>
    private void BuildChoice(string label, string? defaultValue)
    {
        foreach (Control c in new Control[] { lblDate, dteFrom, lblTilde, dteTo, lblDocNo, txtDocNo }) c.Visible = false;

        var lbl = new LabelControl { Text = label, Location = new System.Drawing.Point(28, 14), AutoSize = true };
        _cboChoice = new ComboBoxEdit { Location = new System.Drawing.Point(90, 11), Size = new System.Drawing.Size(140, 20) };
        _cboChoice.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
        foreach (var c in _choices!) _cboChoice.Properties.Items.Add(c.Value);
        var idx = 0;
        for (var i = 0; i < _choices.Count; i++) if (_choices[i].Key == defaultValue) idx = i;
        _cboChoice.SelectedIndex = idx;
        _cboChoice.SelectedIndexChanged += async (s, e) => await SearchAsync();
        panHeader.Controls.Add(lbl);
        panHeader.Controls.Add(_cboChoice);

        lblKeyword.Location = new System.Drawing.Point(260, 14);
        txtKeyword.Location = new System.Drawing.Point(344, 11);
    }

    private async Task SearchAsync()
    {
        try
        {
            var p = new Dictionary<string, string?>
            {
                ["p_work_type"] = "Q",
                ["p_acc_id"] = _accId,
                ["p_date_from"] = dteFrom.YyyyMmDd,
                ["p_date_to"] = dteTo.YyyyMmDd,
                ["p_doc_no"] = txtDocNo.Text,
                ["p_cust_id"] = null,
                ["p_keyword"] = txtKeyword.Text,
            };
            if (_extra != null) foreach (var kv in _extra) p[kv.Key] = kv.Value;
            if (_cboChoice != null && _choiceParam != null && _choices != null)
                p[_choiceParam] = _cboChoice.SelectedIndex >= 0 ? _choices[_cboChoice.SelectedIndex].Key : string.Empty;
            _table = await ProcData.QueryAsync(_menuId, _procName, p);
            if (!_table.Columns.Contains("sel")) _table.Columns.Add("sel", typeof(bool));
            foreach (DataRow r in _table.Rows) r["sel"] = false;

            chkAll.Checked = false;
            grd1.DataSource = _table;

            // 결과가 없으면 왜 비었는지 하단에 알려준다(화면이 조용히 비어 있으면 원인을 알 수 없다).
            var empty = _table.Rows.Count == 0;
            lblHint.Text = empty ? (_emptyHint ?? "조회된 행이 없습니다.") : "체크한 행을 불러옵니다.";
            lblHint.Appearance.ForeColor = empty ? System.Drawing.Color.FromArgb(180, 60, 50) : System.Drawing.Color.Empty;
            lblHint.Appearance.Options.UseForeColor = empty;
        }
        catch (Exception ex)
        {
            AppMessageBox.Show(ex.Message, "조회 실패");
        }
    }

    private void SetAll(bool value)
    {
        gvw1.CloseEditor();
        foreach (DataRow r in _table.Rows) r["sel"] = value;
    }

    private void Confirm()
    {
        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        var picked = _table.Rows.Cast<DataRow>().Where(r => r["sel"] is bool b && b).ToList();
        if (picked.Count == 0)
        {
            AppMessageBox.Show("불러올 행을 체크하세요.", "안내");
            return;
        }

        var error = _validate?.Invoke(picked);
        if (!string.IsNullOrEmpty(error))
        {
            AppMessageBox.Show(error, "안내");
            return;
        }

        Selected = _table.Clone();
        foreach (var r in picked) Selected.ImportRow(r);
        DialogResult = DialogResult.OK;
    }
}
