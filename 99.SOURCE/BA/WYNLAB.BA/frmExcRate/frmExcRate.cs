using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.BA;

/// <summary>
/// 환율조회 화면(2026-09-15) - TBAEXCRATE 최신 고시일자 스냅샷을 보여주는 조회전용 화면.
/// "환율정보수신" 버튼은 api/system/api-integrations/EXIM_FX/run(수출입은행 환율 연동, 이미
/// API 연동관리 화면에서 등록/자동실행되고 있는 것과 같은 엔드포인트)을 그대로 호출해서
/// 수동으로 즉시 갱신한다 - 이 화면은 그 연동의 정의(URL/인증키/스케줄)를 몰라도 되고,
/// 그냥 "지금 받아와줘"만 요청한다.
/// </summary>
public partial class frmExcRate : BaseForm
{
    private DataTable _list = new();

    public frmExcRate()
    {
        InitializeComponent();

        Text = "환율조회";

        Controls.Add(BuildScreenHeader());

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;

        ymdFrDate.MarkRequired();
        ymdToDate.MarkRequired();

        var today = DateTime.Now.ToString("yyyyMMdd");
        ymdFrDate.YyyyMmDd = today;
        ymdToDate.YyyyMmDd = today;

        btnReceive.Click += async (s, e) => await ReceiveClick();

        Load += async (s, e) => await QueryClick();
    }

    public override async Task QueryClick()
    {
        if (string.IsNullOrEmpty(ymdFrDate.YyyyMmDd) || string.IsNullOrEmpty(ymdToDate.YyyyMmDd))
        {
            AppMessageBox.Show("조회기간(시작일/종료일)은 필수입니다.", "확인");
            return;
        }

        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_base_date_from"] = ymdFrDate.YyyyMmDd,
            ["p_base_date_to"] = ymdToDate.YyyyMmDd,
        };
        _list = await QueryAsync("USP_BA_EXCRATE_Q", p);
        grd1.DataSource = _list;
    }

    private async Task ReceiveClick()
    {
        var confirm = AppMessageBox.Show(
            "이미 등록 된 환율정보가 있다면 초기화 후 재 수신됩니다. 환율정보수신 작업을 진행 하시겠습니까?",
            "환율정보수신", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        btnReceive.Enabled = false;
        try
        {
            var result = await ApiClient.PostAsync<object, ApiResult>("api/system/api-integrations/EXIM_FX/run", new { });
            if (result == null || !result.Success)
            {
                AppMessageBox.Show(result?.Message ?? "환율정보 수신에 실패했습니다.", "수신 실패");
                return;
            }

            Toast.Show(result.Message ?? "환율정보를 수신했습니다.");
            await QueryClick();
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"환율정보 수신 중 오류가 발생했습니다.\n{ex.Message}", "오류");
        }
        finally
        {
            btnReceive.Enabled = true;
        }
    }
}
