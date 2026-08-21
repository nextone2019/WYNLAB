using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using WYNLAB.Base;
using System.Drawing;

namespace WYNLAB.SM.USERGROUP;

partial class UserGroupListForm
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

    private Panel searchPanel;
    private LabelControl lblSearchGrpNm;
    private TextEdit txtSearchGrpNm;
    private SimpleButton btnSearch;

    private void InitializeComponent()
    {
        this.searchPanel = new Panel();
        var searchPanelBottomBorder = new Panel();
        this.lblSearchGrpNm = new LabelControl();
        this.txtSearchGrpNm = new TextEdit();
        this.btnSearch = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)this.txtSearchGrpNm.Properties).BeginInit();
        this.SuspendLayout();
        //
        // searchPanelBottomBorder
        //
        searchPanelBottomBorder.Dock = DockStyle.Bottom;
        searchPanelBottomBorder.Height = 1;
        searchPanelBottomBorder.BackColor = Color.FromArgb(228, 229, 232);
        //
        // lblSearchGrpNm
        //
        this.lblSearchGrpNm.Location = new Point(16, 15);
        this.lblSearchGrpNm.Size = new Size(44, 18);
        this.lblSearchGrpNm.AutoSizeMode = LabelAutoSizeMode.None;
        this.lblSearchGrpNm.Text = "그룹명";
        this.lblSearchGrpNm.Appearance.ForeColor = Color.FromArgb(100, 100, 100);
        this.lblSearchGrpNm.Appearance.Font = AppFonts.Caption;
        //
        // txtSearchGrpNm
        //
        this.txtSearchGrpNm.Location = new Point(62, 11);
        this.txtSearchGrpNm.Size = new Size(160, 24);
        this.txtSearchGrpNm.Properties.Appearance.Font = AppFonts.Body;
        this.txtSearchGrpNm.KeyDown += this.txtSearchGrpNm_KeyDown;
        //
        // btnSearch
        //
        this.btnSearch.Text = "검색";
        this.btnSearch.Location = new Point(232, 10);
        this.btnSearch.Size = new Size(72, 26);
        this.btnSearch.Appearance.Font = AppFonts.Body;
        this.btnSearch.Click += this.btnSearch_Click;
        //
        // searchPanel
        //
        this.searchPanel.Dock = DockStyle.Top;
        this.searchPanel.Height = 46;
        this.searchPanel.BackColor = Color.FromArgb(250, 250, 251);
        this.searchPanel.Controls.Add(this.btnSearch);
        this.searchPanel.Controls.Add(this.txtSearchGrpNm);
        this.searchPanel.Controls.Add(this.lblSearchGrpNm);
        this.searchPanel.Controls.Add(searchPanelBottomBorder);
        //
        // MainGridView - BaseGridForm이 만든 그리드(Dock=Fill)에 이 화면 전용 컬럼을 채운다.
        // AutoPopulateColumns를 꺼서 DTO 리플렉션 자동생성 대신 아래 컬럼 목록을 그대로 쓴다 -
        // 컬럼을 추가/삭제하고 싶으면 이 목록만 고치면 된다(디자이너로도 편집 가능).
        //
        this.MainGridView.OptionsBehavior.Editable = false;
        this.MainGridView.OptionsBehavior.AutoPopulateColumns = false;
        this.MainGridView.DoubleClick += this.MainGridView_DoubleClick;
        this.MainGridView.Columns.AddRange(new GridColumn[]
        {
            new GridColumn { FieldName = "UserGrpCd", Caption = "그룹코드", Visible = true, VisibleIndex = 0, Width = 100 },
            new GridColumn { FieldName = "UserGrpNm", Caption = "그룹명", Visible = true, VisibleIndex = 1, Width = 150 },
            new GridColumn { FieldName = "Description", Caption = "설명", Visible = true, VisibleIndex = 2, Width = 240 },
            new GridColumn { FieldName = "SortOrder", Caption = "정렬순서", Visible = true, VisibleIndex = 3, Width = 70 },
            new GridColumn { FieldName = "UseYn", Caption = "사용", Visible = true, VisibleIndex = 4, Width = 60 },
            new GridColumn { FieldName = "MemberCount", Caption = "소속인원", Visible = true, VisibleIndex = 5, Width = 80 },
        });
        //
        // UserGroupListForm
        //
        this.Text = "사용자그룹관리";
        this.Controls.Add(this.searchPanel);
        this.Load += this.UserGroupListForm_Load;
        ((System.ComponentModel.ISupportInitialize)this.txtSearchGrpNm.Properties).EndInit();
        this.ResumeLayout(false);
    }
}
