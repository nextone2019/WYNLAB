using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using WYNLAB.Base;
using System.Drawing;

namespace WYNLAB.SM.USERGROUP;

partial class UserGroupEditForm
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

    private TextEdit txtUserGrpCd;
    private TextEdit txtUserGrpNm;
    private MemoEdit txtDescription;
    private SpinEdit spnSortOrder;
    private CheckEdit chkUseYn;
    private LayoutControl layoutControl;
    private Panel headerPanel;
    private LabelControl titleLabel;
    private Panel footerPanel;
    private SimpleButton btnSave;
    private SimpleButton btnCancel;
    private Panel memberGroupPanel;
    private GridControl memberGrid;
    private GridView memberGridView;

    private void InitializeComponent()
    {
        this.txtUserGrpCd = new TextEdit();
        this.txtUserGrpNm = new TextEdit();
        this.txtDescription = new MemoEdit();
        this.spnSortOrder = new SpinEdit();
        this.chkUseYn = new CheckEdit();
        this.layoutControl = new LayoutControl();
        this.headerPanel = new Panel();
        this.titleLabel = new LabelControl();
        this.footerPanel = new Panel();
        this.btnSave = new SimpleButton();
        this.btnCancel = new SimpleButton();
        this.memberGroupPanel = new Panel();
        var memberCaptionLabel = new LabelControl();
        this.memberGrid = new GridControl();
        this.memberGridView = new GridView();

        ((System.ComponentModel.ISupportInitialize)this.txtUserGrpCd.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.txtUserGrpNm.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.txtDescription.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.spnSortOrder.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.chkUseYn.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.layoutControl).BeginInit();
        this.SuspendLayout();
        //
        // UserGroupEditForm
        //
        this.StartPosition = FormStartPosition.CenterParent;
        this.Width = 560;
        this.Height = 420;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        //
        // spnSortOrder / chkUseYn
        //
        this.spnSortOrder.Properties.MinValue = 0;
        this.spnSortOrder.Properties.MaxValue = 9999;
        this.chkUseYn.Text = "사용";
        //
        // layoutControl (기본정보/상태 입력 영역)
        //
        this.layoutControl.Dock = DockStyle.Fill;
        this.layoutControl.Padding = new Padding(12);
        this.layoutControl.BeginUpdate();
        var root = this.layoutControl.Root;
        root.TextVisible = false;
        root.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);

        var groupBasic = root.AddGroup("기본정보").StyleAsSection();
        groupBasic.AddItem("그룹코드", this.txtUserGrpCd);
        groupBasic.AddItem("그룹명", this.txtUserGrpNm).MarkRequired();
        var descItem = groupBasic.AddItem("설명", this.txtDescription);
        descItem.Control.Height = 50;
        groupBasic.AddItem("정렬순서", this.spnSortOrder);

        var groupStatus = root.AddGroup("상태").StyleAsSection();
        var itemUseYn = groupStatus.AddItem(string.Empty, this.chkUseYn);
        itemUseYn.TextVisible = false;

        this.layoutControl.EndUpdate();
        //
        // headerPanel / titleLabel
        //
        this.headerPanel.Dock = DockStyle.Top;
        this.headerPanel.Height = 46;
        this.headerPanel.BackColor = Color.FromArgb(245, 246, 248);
        this.titleLabel.Location = new Point(16, 12);
        this.titleLabel.AutoSizeMode = LabelAutoSizeMode.None;
        this.titleLabel.Size = new Size(340, 22);
        this.titleLabel.Appearance.Font = AppFonts.SubHeading;
        this.headerPanel.Controls.Add(this.titleLabel);
        //
        // memberGroupPanel / memberCaptionLabel / memberGrid / memberGridView
        // 신규등록 모드에서는 .cs 쪽에서 Visible=false로 숨긴다(컨트롤 자체는 항상 생성).
        //
        this.memberGroupPanel.Dock = DockStyle.Bottom;
        this.memberGroupPanel.Height = 260;

        memberCaptionLabel.Text = "소속 사용자 배정 (체크 후 저장)";
        memberCaptionLabel.Dock = DockStyle.Top;
        memberCaptionLabel.Height = 24;
        memberCaptionLabel.Padding = new Padding(12, 6, 0, 0);
        memberCaptionLabel.Appearance.Font = AppFonts.BodyBold;
        memberCaptionLabel.Appearance.ForeColor = Color.FromArgb(70, 70, 70);

        this.memberGrid.MainView = this.memberGridView;
        this.memberGrid.Dock = DockStyle.Fill;
        this.memberGridView.OptionsView.ShowGroupPanel = false;
        this.memberGridView.OptionsView.ShowAutoFilterRow = true;
        this.memberGridView.OptionsBehavior.Editable = true;
        this.memberGridView.OptionsBehavior.AutoPopulateColumns = false;
        this.memberGridView.Columns.AddRange(new GridColumn[]
        {
            new GridColumn { FieldName = "UserId", Caption = "아이디", Visible = true, VisibleIndex = 0, OptionsColumn = { AllowEdit = false } },
            new GridColumn { FieldName = "UserNm", Caption = "이름", Visible = true, VisibleIndex = 1, OptionsColumn = { AllowEdit = false } },
            new GridColumn { FieldName = "DeptNm", Caption = "부서", Visible = true, VisibleIndex = 2, OptionsColumn = { AllowEdit = false } },
            new GridColumn { FieldName = "IsMember", Caption = "소속", Visible = true, VisibleIndex = 3, Width = 50 },
        });

        this.memberGroupPanel.Controls.Add(this.memberGrid);
        this.memberGroupPanel.Controls.Add(memberCaptionLabel);
        //
        // footerPanel / btnSave / btnCancel
        //
        this.footerPanel.Dock = DockStyle.Bottom;
        this.footerPanel.Height = 56;
        this.footerPanel.BackColor = Color.FromArgb(245, 246, 248);

        this.btnSave.Text = "저장";
        this.btnSave.Size = new Size(90, 32);
        this.btnSave.Appearance.BackColor = Color.FromArgb(37, 122, 201);
        this.btnSave.Appearance.ForeColor = Color.White;
        this.btnSave.Appearance.Options.UseBackColor = true;
        this.btnSave.Appearance.Options.UseForeColor = true;
        this.btnSave.Click += this.btnSave_Click;

        this.btnCancel.Text = "취소";
        this.btnCancel.DialogResult = DialogResult.Cancel;
        this.btnCancel.Size = new Size(90, 32);

        this.footerPanel.Controls.Add(this.btnSave);
        this.footerPanel.Controls.Add(this.btnCancel);
        this.footerPanel.Resize += this.footerPanel_Resize;
        //
        // UserGroupEditForm (Dock 순서 중요: Fill 먼저, 그다음 Bottom 두 개는 원래 순서(memberGroupPanel -> footerPanel) 유지)
        //
        this.Controls.Add(this.layoutControl);
        this.Controls.Add(this.memberGroupPanel);
        this.Controls.Add(this.footerPanel);
        this.Controls.Add(this.headerPanel);

        this.CancelButton = this.btnCancel;
        this.Load += this.UserGroupEditForm_Load;

        ((System.ComponentModel.ISupportInitialize)this.layoutControl).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.txtUserGrpCd.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.txtUserGrpNm.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.txtDescription.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.spnSortOrder.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.chkUseYn.Properties).EndInit();
        this.ResumeLayout(false);
    }
}
