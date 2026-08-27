using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace WYNLAB.Base;

/// <summary>
/// XtraGrid 기반 목록화면의 공통 베이스. 개별 화면에 버튼을 두지 않고,
/// Shell 상단의 공통 툴바(조회/입력/삭제/행추가/행삭제/저장/출력)가 호출하는 QueryClick/NewClick/
/// DeleteClick/NewRowClick/DeleteRowClick/SaveClick/PrintClick(BaseForm에서 상속)를 override 해서 동작을 채운다.
/// </summary>
public class BaseGridForm : BaseForm
{
    protected GridControl MainGrid { get; } = new GridControl();
    protected GridView MainGridView { get; } = new GridView();

    public BaseGridForm()
    {
        InitializeGridDefaults();
    }

    private void InitializeGridDefaults()
    {
        MainGrid.MainView = MainGridView;
        MainGrid.Dock = DockStyle.Fill;

        // BAROCRM 화면 기준 표준 옵션 - 우클릭 필터, 그룹핑, 자동필터행 전부 사용
        MainGridView.OptionsView.ShowAutoFilterRow = true;
        MainGridView.OptionsView.ShowGroupPanel = true;
        MainGridView.OptionsBehavior.AllowIncrementalSearch = true;
        MainGridView.OptionsSelection.MultiSelect = true;

        this.Controls.Add(MainGrid);
    }

    /// <summary>Shell의 "출력" 버튼 기본 동작 - 엑셀로 내보내기. 필요시 하위 화면에서 재정의 가능.</summary>
    public override Task PrintClick()
    {
        SafeExecute(() =>
        {
            using var dlg = new SaveFileDialog { Filter = "Excel (*.xlsx)|*.xlsx", FileName = $"{Text}_{DateTime.Now:yyyyMMdd}.xlsx" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                MainGridView.ExportToXlsx(dlg.FileName);
            }
        }, "출력");
        return Task.CompletedTask;
    }
}
