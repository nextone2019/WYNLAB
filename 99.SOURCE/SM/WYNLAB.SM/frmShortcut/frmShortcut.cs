using System.Data;
using DevExpress.XtraEditors;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.SM.SHORTCUT;

/// <summary>
/// 단축키설정 화면(TSMSHORTCUTDEFAULT 기본값 + TSMUSERSHORTCUT 사용자 재정의).
///
/// 대상은 화면(메뉴)이 아니라 Shell 상단 공통 툴바의 7개 고정 액션(조회/입력/삭제/행추가/
/// 행삭제/저장/출력) - ShellForm.BuildToolbar/ProcessCmdKey가 부르는 BaseForm 표준 액션과
/// 1:1이다. 한 사용자가 조회에 Ctrl+Q를 지정하면 어느 화면에서든 그 키로 조회된다.
///
/// 다른 화면(frmMinorCode 등)과 달리 그리드+저장 버튼 흐름이 아니라, 각 행의 키 입력칸에
/// 포커스를 두고 원하는 키 조합을 누르는 즉시 저장한다(OS 단축키 설정 화면과 같은 방식) -
/// 액션이 7개뿐이고 서로 독립적이라 그리드로 묶어 일괄 저장할 이유가 없다. 그래서 SaveClick은
/// 오버라이드하지 않는다(Shell 툴바의 저장/Ctrl+S를 눌러도 이 화면에서는 할 일이 없다).
/// </summary>
public class frmShortcut : BaseForm
{
    private readonly Dictionary<string, RowControls> _rows = new();

    public frmShortcut()
    {
        Text = "단축키설정";

        var body = BuildBody();
        Controls.Add(body);
        Controls.Add(BuildScreenHeader()); // BuildScreenHeader 주석 참고 - 반드시 나중에 Add해야 맨 위를 차지한다.

        Load += async (s, e) => await SafeExecuteAsync(QueryClick, "조회");
    }

    /// <summary>화면 갱신 + 세션 캐시(SessionManager.Current.Shortcuts) 갱신을 한 번에 한다 - 이
    /// 화면을 벗어나지 않고 바로 다른 화면에서 새 단축키를 쓰려면(재로그인 없이) ShellForm.
    /// ProcessCmdKey가 참조하는 세션 캐시도 매번 최신 상태여야 한다.</summary>
    public override async Task QueryClick()
    {
        // p_user_id를 빼먹으면 프로시저의 기본값(NULL)으로 실행되어 TSMUSERSHORTCUT LEFT JOIN이
        // 절대 매치되지 않는다 - 그러면 실제로 재정의한 게 있어도 전부 "기본값"으로만 보이고
        // 초기화 버튼도 항상 비활성 상태로 남는다(2026-09-02 실제 발견 - 이 사용자의 조회
        // 단축키가 실제로는 Ctrl+R로 재정의돼 있었는데 이 화면은 계속 Ctrl+Q/기본값으로만
        // 보여줘서 아무도 그 사실을 몰랐다). api/data 범용 통로는 조회(Query) 요청에는 로그인
        // 사용자 id를 서버가 자동으로 안 채워준다(저장/삭제만 그렇게 한다) - 그래서 조회는
        // 화면이 직접 넘겨야 한다.
        var table = await QueryAsync("USP_SM_SHORTCUT_Q", new { p_work_type = "Q", p_user_id = Session.UserId });
        var shortcuts = new List<ShortcutDto>();

        foreach (DataRow row in table.Rows)
        {
            var actionCd = Str(row, "action_cd");
            var isCustom = Str(row, "custom_yn") == "Y";

            shortcuts.Add(new ShortcutDto
            {
                ActionCd = actionCd,
                ActionNm = Str(row, "action_nm"),
                KeyCombo = Str(row, "key_combo"),
                CustomYn = isCustom,
                SortOrder = int.TryParse(Str(row, "sort_order"), out var sort) ? sort : 0
            });

            if (!_rows.TryGetValue(actionCd, out var controls)) continue;
            controls.TxtCombo.Text = Str(row, "key_combo");
            controls.BtnReset.Enabled = isCustom;
            controls.LblStatus.Text = isCustom ? "변경됨" : "기본값";
        }

        SessionManager.Current.ReplaceShortcuts(shortcuts);
    }

    private PanelWyn BuildBody()
    {
        var body = new PanelWyn { Dock = DockStyle.Fill, BackColor = Color.White };

        var card = new PanelWyn
        {
            Style = PanelWynStyle.Card,
            Location = new Point(24, 50),
            Size = new Size(560, 40 + RowDefinitions.Length * RowHeight + 16)
        };

        var lblGuide = new LabelControl
        {
            Text = "입력칸을 클릭한 뒤 원하는 키 조합을 누르면 즉시 저장됩니다. 다른 동작이 이미 쓰고 있는 키는 지정할 수 없습니다.",
            Location = new Point(16, 12),
            AutoSizeMode = LabelAutoSizeMode.Default
        };
        lblGuide.Appearance.Font = AppFonts.Caption;
        lblGuide.Appearance.ForeColor = Color.FromArgb(130, 132, 138);
        card.Controls.Add(lblGuide);

        var y = 40;
        foreach (var (actionCd, actionNm) in RowDefinitions)
        {
            card.Controls.Add(BuildRow(actionCd, actionNm, y));
            y += RowHeight;
        }

        body.Controls.Add(card);
        return body;
    }

    /// <summary>서버 응답(action_nm)이 오기 전까지 화면에 뭐라도 보여줘야 하므로, 시드 데이터
    /// (014_User_Shortcut.sql)와 같은 이름을 기본으로 미리 넣어둔다 - QueryClick이 끝나면
    /// TxtCombo/상태 라벨만 서버 값으로 갱신되고 이 이름표는 그대로 재사용된다.</summary>
    private static readonly (string ActionCd, string ActionNm)[] RowDefinitions =
    {
        ("QUERY", "조회"),
        ("NEW", "입력"),
        ("DELETE", "삭제"),
        ("ROWADD", "행추가"),
        ("ROWDELETE", "행삭제"),
        ("SAVE", "저장"),
        ("PRINT", "출력"),
    };

    private const int RowHeight = 40;

    private Panel BuildRow(string actionCd, string actionNm, int y)
    {
        var row = new Panel { Location = new Point(0, y), Size = new Size(544, RowHeight) };

        var lblAction = new LabelWyn { Text = actionNm, Location = new Point(16, 10), Size = new Size(80, 20) };
        lblAction.Appearance.Font = AppFonts.Body;

        // ReadOnly는 일부러 안 쓴다 - DevExpress TextEdit은 ReadOnly일 때 KeyDown 자체가 안 올라올
        // 수 있어서(포커스/키보드 입력을 더 낮은 단계에서 막음), 대신 KeyDown에서 매번
        // e.SuppressKeyPress로 실제 타이핑만 막는다(TxtCombo_KeyDown 참고) - 키 캡처 입력칸의
        // 표준적인 구현 방식.
        var txtCombo = new TextEditWyn
        {
            Location = new Point(104, 6),
            Size = new Size(160, 26)
        };
        txtCombo.Properties.NullText = "지정 안 됨";
        txtCombo.Tag = actionCd;
        txtCombo.KeyDown += TxtCombo_KeyDown;

        var lblStatus = new LabelWyn { Text = string.Empty, Location = new Point(280, 10), Size = new Size(60, 20) };
        lblStatus.Appearance.Font = AppFonts.Caption;
        lblStatus.Appearance.ForeColor = Color.FromArgb(150, 150, 150);

        var btnReset = new SimpleButton { Text = "초기화", Location = new Point(350, 5), Size = new Size(70, 28), Enabled = false };
        btnReset.Tag = actionCd;
        btnReset.Click += BtnReset_Click;

        row.Controls.Add(lblAction);
        row.Controls.Add(txtCombo);
        row.Controls.Add(lblStatus);
        row.Controls.Add(btnReset);

        _rows[actionCd] = new RowControls(txtCombo, lblStatus, btnReset);
        return row;
    }

    /// <summary>키 입력칸에서 실제로 조합키를 누른 즉시 저장한다.</summary>
    private async void TxtCombo_KeyDown(object? sender, KeyEventArgs e)
    {
        e.SuppressKeyPress = true; // 글자가 입력칸에 그대로 타이핑되는 것만 막는다
        if (sender is not TextEditWyn txt || txt.Tag is not string actionCd) return;
        if (ShortcutKeys.IsModifierOnly(e.KeyData)) return; // Ctrl/Shift만 눌린 상태 - 본 키를 더 기다린다

        var comboText = ShortcutKeys.ToText(e.KeyData);
        if (comboText == txt.Text) return; // 지금 값 그대로 다시 누른 경우 - 할 일 없음

        if (ShortcutKeys.ReservedCombos.Contains(comboText))
        {
            AppMessageBox.Show($"[{comboText}]는 시스템이 이미 사용 중인 예약된 키 조합입니다.", "지정 불가");
            return;
        }

        var conflict = _rows.FirstOrDefault(kv => kv.Key != actionCd && kv.Value.TxtCombo.Text == comboText);
        if (conflict.Key != null)
        {
            var conflictNm = RowDefinitions.First(r => r.ActionCd == conflict.Key).ActionNm;
            AppMessageBox.Show($"[{comboText}]는 이미 [{conflictNm}] 동작에 지정되어 있습니다.", "지정 불가");
            return;
        }

        var result = await SaveAsync("USP_SM_SHORTCUT_S", new
        {
            p_work_type = "U",
            p_action_cd = actionCd,
            p_key_combo = comboText
        });

        if (!result.Success)
        {
            AppMessageBox.Show(result.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        await QueryClick(); // 화면/세션 캐시 갱신을 한 번에 처리(QueryClick 주석 참고)
        Toast.Show("단축키가 저장되었습니다.");
    }

    private async void BtnReset_Click(object? sender, EventArgs e)
    {
        if (sender is not SimpleButton btn || btn.Tag is not string actionCd) return;

        var result = await SaveAsync("USP_SM_SHORTCUT_S", new { p_work_type = "D", p_action_cd = actionCd });
        if (!result.Success)
        {
            AppMessageBox.Show(result.Message ?? "초기화에 실패했습니다.", "초기화 실패");
            return;
        }

        await QueryClick();
        Toast.Show("기본값으로 되돌렸습니다.");
    }

    /// <summary>DataRow에서 문자열 컬럼을 안전하게 읽는다 - 없는 컬럼이거나 NULL이면 빈 문자열
    /// (frmMinorCode.Str과 같은 이유 - ProcData.ToDataTable이 만드는 컬럼은 항상 object 타입).</summary>
    private static string Str(DataRow row, string columnName) =>
        row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value
            ? Convert.ToString(row[columnName]) ?? string.Empty
            : string.Empty;

    /// <summary>C# record는 net48에서 IsExternalInit 폴리필 없이는 컴파일 안 되므로(CS0518) 그냥
    /// 평범한 클래스로 둔다.</summary>
    private sealed class RowControls
    {
        public RowControls(TextEditWyn txtCombo, LabelWyn lblStatus, SimpleButton btnReset)
        {
            TxtCombo = txtCombo;
            LblStatus = lblStatus;
            BtnReset = btnReset;
        }

        public TextEditWyn TxtCombo { get; }
        public LabelWyn LblStatus { get; }
        public SimpleButton BtnReset { get; }
    }
}
