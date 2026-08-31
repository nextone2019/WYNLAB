using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace WYNLAB.Base.Controls;

/// <summary>
/// PictureEdit 기반 - 사진 등록/표시용(예: 사원등록의 사원사진). 우클릭 메뉴(불러오기/지우기/
/// 복사/붙여넣기)는 DevExpress가 기본 제공하지만, 더블클릭으로 파일 선택창 열기와 탐색기에서
/// 이미지 파일을 끌어다 놓는 것(드래그 앤 드롭)은 PictureEdit이 자동으로 지원하지 않는다 -
/// 둘 다 흔히 기대하는 동작이라 직접 붙였다(실제로 사용자가 둘 다 안 된다고 겪은 뒤 추가함,
/// 2026-08-31). 화면 크기에 맞춰 비율을 유지하며 채우는 SizeMode 기본값과, 서버와 저장/조회할
/// 때 바이트 배열로 바로 주고받을 수 있는 ImageBytes 프로퍼티도 같이 제공한다 - JPEG로
/// 인코딩한다(사진 용도라 PNG 무손실까지는 필요 없고, 같은 화질이면 파일 크기가 작아 범용
/// 데이터 통로처럼 Base64 문자열로 실어 보내는 경로에 유리하다).
/// </summary>
[ToolboxItem(true)]
public class PictureEditWyn : PictureEdit
{
    public PictureEditWyn()
    {
        Properties.SizeMode = PictureSizeMode.Squeeze;
        Properties.ShowMenu = true; // 우클릭 메뉴(불러오기/지우기/복사/붙여넣기) - DevExpress 기본 제공

        AllowDrop = true;
        DoubleClick += (s, e) => LoadFromDialog();
        DragEnter += OnDragEnter;
        DragDrop += OnDragDrop;
    }

    private void OnDragEnter(object? sender, DragEventArgs e)
    {
        e.Effect = e.Data?.GetDataPresent(DataFormats.FileDrop) == true ? DragDropEffects.Copy : DragDropEffects.None;
    }

    private void OnDragDrop(object? sender, DragEventArgs e)
    {
        if (e.Data?.GetData(DataFormats.FileDrop) is string[] { Length: > 0 } files)
            LoadFromFile(files[0]);
    }

    private void LoadFromDialog()
    {
        using var dialog = new OpenFileDialog { Filter = "이미지 파일|*.jpg;*.jpeg;*.png;*.bmp;*.gif|모든 파일|*.*" };
        if (dialog.ShowDialog() == DialogResult.OK) LoadFromFile(dialog.FileName);
    }

    /// <summary>바이트로 읽어서 ImageBytes 세터(MemoryStream 경유)로 넣는다 - Image.FromFile을
    /// 직접 쓰면 그 Image 객체가 살아있는 동안 원본 파일을 잠가버려서(GDI+가 파일 스트림을 계속
    /// 물고 있음), 방금 넣은 파일을 옮기거나 지울 수 없게 되는 문제가 있다. 깨졌거나 이미지가
    /// 아닌 파일을 골라도(우클릭 불러오기와 달리 확장자로 미리 거르지 않으므로) 조용히
    /// 무시한다 - 사진 하나 잘못 골랐다고 화면이 죽으면 안 된다.</summary>
    private void LoadFromFile(string path)
    {
        try
        {
            ImageBytes = File.ReadAllBytes(path);
        }
        catch
        {
        }
    }

    /// <summary>현재 표시된 이미지를 JPEG 바이트로 읽거나(저장용) 바이트에서 이미지를 채운다
    /// (조회용) - null/빈 배열을 넣으면 이미지 없음(플레이스홀더만 표시) 상태가 된다.</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public byte[]? ImageBytes
    {
        get
        {
            if (EditValue is not Image img) return null;
            using var stream = new MemoryStream();
            img.Save(stream, ImageFormat.Jpeg);
            return stream.ToArray();
        }
        set
        {
            if (value == null || value.Length == 0)
            {
                EditValue = null;
                return;
            }
            using var stream = new MemoryStream(value);
            EditValue = Image.FromStream(stream);
        }
    }
}
