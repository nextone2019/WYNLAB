using System;
using System.Collections;
using System.ComponentModel.Design;
using DevExpress.XtraGrid.Design;
using WYNLAB.Base.Controls;

namespace WYNLAB.Controls.Design;

/// <summary>
/// GridControlWyn을 도구상자에서 캔버스에 처음 놓을 때(또는 "Run Designer" -> Views 추가 시)
/// DevExpress가 기본으로 만들어주는 View를 그냥 GridView가 아니라 GridViewWyn으로 바꿔치기한다.
///
/// 왜 필요한가: VS 디자이너는 "정식 컴포넌트 생성 경로"(IDesignerHost.CreateComponent)를 통해
/// 만들어진 컴포넌트만 제대로 된 필드/Site를 가진 걸로 인식해서 컬럼 등 속성 변경을 저장한다.
/// 예전에 GridControlWyn 생성자 안에서 그냥 new GridViewWyn()으로 만들었더니 이 경로를 안 타서
/// 디자이너에서 추가한 컬럼이 재빌드하면 사라지는 문제가 있었다 - 이 디자이너 클래스가 그
/// "정식 경로"를 통해 GridViewWyn을 만들어줌으로써 그 문제를 근본적으로 해결한다.
///
/// DevExpress 자신이 GridControl에 커스텀 디자이너(GridControlDesigner)를 붙여서 기본 View를
/// 자동 생성해주는 것과 완전히 같은 방식 - 거기서 만드는 View 타입만 GridViewWyn으로 바꾼 것.
///
/// 주의: View 교체는 InitializeNewComponent가 실행되는 "그 순간"에 하면 안 된다 - 이 시점은
/// 아직 툴박스 드롭 트랜잭션(ParentControlDesigner가 "방금 만든 컴포넌트를 캔버스에 붙이는" 처리)이
/// 진행 중이라, 여기서 host.CreateComponent로 컴포넌트를 하나 더 만들면 그 드롭 처리가 방금 추가된
/// View 쪽을 "방금 만든 컴포넌트"로 착각해서 정작 그리드 본체는 캔버스에 안 붙고 트레이에만 남는
/// 문제가 있었다(실제로 겪음). 그래서 드롭 트랜잭션이 완전히 끝난 다음 메시지 루프 틱으로
/// BeginInvoke를 통해 미뤄서 처리한다 - 디자인 서페이스도 실제 WinForms 메시지 펌프가 돌고 있어서
/// 이 방식이 통한다.
/// </summary>
public class GridControlWynDesigner : GridControlDesigner
{
    public override void InitializeNewComponent(IDictionary? defaultValues)
    {
        base.InitializeNewComponent(defaultValues);

        if (Component is not GridControlWyn grid) return;
        if (grid.MainView is GridViewWyn) return; // 이미 GridViewWyn이면 손 안 댐(복사-붙여넣기 등)

        if (GetService(typeof(IDesignerHost)) is not IDesignerHost host) return;

        grid.BeginInvoke(new Action(() => ReplaceView(grid, host)));
    }

    private static void ReplaceView(GridControlWyn grid, IDesignerHost host)
    {
        if (grid.MainView is GridViewWyn) return;

        try
        {
            var oldView = grid.MainView;

            var newView = (GridViewWyn)host.CreateComponent(typeof(GridViewWyn));
            grid.MainView = newView; // MainView setter가 ViewCollection 등록까지 같이 처리함 - 여기서 또 Add하면 중복 추가로 예외 발생

            if (oldView != null)
            {
                grid.ViewCollection.Remove(oldView);
                if (oldView.Site != null)
                    host.DestroyComponent(oldView); // 디자이너 컴포넌트로 정식 등록된 경우만 이 경로로 제거
                else
                    oldView.Dispose(); // base가 그냥 new로 만든 것이면 컨테이너에 없어서 DestroyComponent가 예외를 던짐
            }
        }
        catch
        {
            // 부가 작업 실패 - 기본 GridView가 그대로 남을 뿐, 그리드 자체는 정상적으로 캔버스에
            // 배치된다. 필요하면 스마트태그의 Run Designer -> Views에서 수동으로 GridViewWyn을
            // 추가하면 된다.
        }
    }
}
