// VS 디자이너가 자동 생성하는 필드 선언에는 = null!을 안 붙여서 nullable 경고(CS8618)가
// 계속 나기 때문에, 이 파일(디자이너 전용)만 nullable 검사를 끈다 - 흔한 관례. 이 프로젝트를
// 복사해서 새 화면을 만들 때도 그 화면의 Designer.cs 맨 위에 이 줄을 그대로 유지할 것.
#nullable disable
using System.Drawing;

namespace WYNLAB.SM.TEMPLATE;

public partial class TemplateForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.SuspendLayout();
        //
        // TemplateForm
        //
        this.ClientSize = new Size(800, 500);
        this.Name = "TemplateForm";
        // 여기서부터는 디자이너 캔버스에 컨트롤을 끌어다 놓으면 VS가 자동으로 채워준다.
        this.ResumeLayout(false);
    }
}
