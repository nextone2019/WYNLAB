// 프로세스 설정 화면 - 업무 처리 방식/허용 기준 같은 프로세스 정책 값을 관리한다(Master-SubGrid 배치:
// 위 grd1=설정 목록(값만 편집), 아래 grd2=선택한 설정의 변경 이력). 상단 타이틀 영역은 이 파일에 두지 않는다
// (BaseForm.BuildScreenHeader가 그리고, 배치는 사용자가 직접 한다 - 다른 화면과 같은 규약).
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.SM;

public partial class frmProcConfig
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.panBase = new WYNLAB.Base.Controls.PanelWyn();
        this.panelSplit = new WYNLAB.Base.Controls.PanelWyn();
        this.panelTop = new WYNLAB.Base.Controls.PanelWyn();
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.colModuleNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colGroupNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colConfigNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colCurValue = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDefaultNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colStateNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colDescription = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colUptUserId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colUptDt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.sectionHeaderMaster = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
        this.panelBottom = new WYNLAB.Base.Controls.PanelWyn();
        this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
        this.colHistDt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colHistOld = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colHistNew = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colHistUser = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colHistPc = new DevExpress.XtraGrid.Columns.GridColumn();
        this.sectionHeaderSub = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.lblSearchModule = new DevExpress.XtraEditors.LabelControl();
        this.cboSearchModule = new DevExpress.XtraEditors.ComboBoxEdit();
        this.lblSearchKeyword = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchKeyword = new WYNLAB.Base.Controls.TextEditWyn();
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        this.panBase.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panelSplit)).BeginInit();
        this.panelSplit.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panelTop)).BeginInit();
        this.panelTop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelBottom)).BeginInit();
        this.panelBottom.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        this.panHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchModule.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).BeginInit();
        this.SuspendLayout();
        //
        // panBase
        //
        this.panBase.Appearance.BackColor = System.Drawing.Color.White;
        this.panBase.Appearance.Options.UseBackColor = true;
        this.panBase.Controls.Add(this.panelSplit);
        this.panBase.Controls.Add(this.panHeader);
        this.panBase.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panBase.Location = new System.Drawing.Point(0, 0);
        this.panBase.Name = "panBase";
        this.panBase.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
        this.panBase.Size = new System.Drawing.Size(1680, 760);
        this.panBase.TabIndex = 6;
        //
        // panelSplit
        //
        this.panelSplit.Controls.Add(this.panelBottom);
        this.panelSplit.Controls.Add(this.splitterWyn1);
        this.panelSplit.Controls.Add(this.panelTop);
        this.panelSplit.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelSplit.Location = new System.Drawing.Point(5, 49);
        this.panelSplit.Name = "panelSplit";
        this.panelSplit.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.panelSplit.Size = new System.Drawing.Size(1670, 706);
        this.panelSplit.TabIndex = 7;
        //
        // panelTop
        //
        this.panelTop.Controls.Add(this.grd1);
        this.panelTop.Controls.Add(this.sectionHeaderMaster);
        this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelTop.Location = new System.Drawing.Point(3, 0);
        this.panelTop.Name = "panelTop";
        this.panelTop.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
        this.panelTop.Size = new System.Drawing.Size(1664, 420);
        this.panelTop.TabIndex = 0;
        this.panelTop.Height = 420;
        //
        // grd1
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd1.EmbeddedNavigator.Buttons.Append.Visible = false;
        this.grd1.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
        this.grd1.EmbeddedNavigator.Buttons.Edit.Visible = false;
        this.grd1.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
        this.grd1.EmbeddedNavigator.Buttons.Remove.Visible = false;
        this.grd1.Location = new System.Drawing.Point(0, 36);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.Size = new System.Drawing.Size(1664, 384);
        this.grd1.TabIndex = 1;
        this.grd1.UseEmbeddedNavigator = false;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw1});
        //
        // gvw1
        //
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colModuleNm,
        this.colGroupNm,
        this.colConfigNm,
        this.colCurValue,
        this.colDefaultNm,
        this.colStateNm,
        this.colDescription,
        this.colUptUserId,
        this.colUptDt});
        this.gvw1.GridControl = this.grd1;
        this.gvw1.HighlightFocusedRow = true;
        this.gvw1.Name = "gvw1";
        this.gvw1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
        this.gvw1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
        this.gvw1.OptionsView.ColumnAutoWidth = false;
        this.gvw1.OptionsView.ShowGroupPanel = false;
        //
        // colModuleNm
        //
        this.colModuleNm.Caption = "구분";
        this.colModuleNm.FieldName = "module_nm";
        this.colModuleNm.Name = "colModuleNm";
        this.colModuleNm.OptionsColumn.AllowEdit = false;
        this.colModuleNm.Visible = true;
        this.colModuleNm.VisibleIndex = 0;
        this.colModuleNm.Width = 60;
        //
        // colGroupNm
        //
        this.colGroupNm.Caption = "묶음";
        this.colGroupNm.FieldName = "group_nm";
        this.colGroupNm.Name = "colGroupNm";
        this.colGroupNm.OptionsColumn.AllowEdit = false;
        this.colGroupNm.Visible = true;
        this.colGroupNm.VisibleIndex = 1;
        this.colGroupNm.Width = 100;
        //
        // colConfigNm
        //
        this.colConfigNm.Caption = "설정명";
        this.colConfigNm.FieldName = "config_nm";
        this.colConfigNm.Name = "colConfigNm";
        this.colConfigNm.OptionsColumn.AllowEdit = false;
        this.colConfigNm.Visible = true;
        this.colConfigNm.VisibleIndex = 2;
        this.colConfigNm.Width = 190;
        //
        // colCurValue
        //
        this.colCurValue.Caption = "값";
        this.colCurValue.FieldName = "cur_value";
        this.colCurValue.Name = "colCurValue";
        this.colCurValue.Visible = true;
        this.colCurValue.VisibleIndex = 3;
        this.colCurValue.Width = 130;
        //
        // colDefaultNm
        //
        this.colDefaultNm.Caption = "기본값";
        this.colDefaultNm.FieldName = "default_nm";
        this.colDefaultNm.Name = "colDefaultNm";
        this.colDefaultNm.OptionsColumn.AllowEdit = false;
        this.colDefaultNm.Visible = true;
        this.colDefaultNm.VisibleIndex = 4;
        this.colDefaultNm.Width = 90;
        //
        // colStateNm
        //
        this.colStateNm.Caption = "상태";
        this.colStateNm.FieldName = "state_nm";
        this.colStateNm.Name = "colStateNm";
        this.colStateNm.OptionsColumn.AllowEdit = false;
        this.colStateNm.Visible = true;
        this.colStateNm.VisibleIndex = 5;
        this.colStateNm.Width = 70;
        //
        // colDescription
        //
        this.colDescription.Caption = "설명 (바꾸면 무엇이 달라지는가)";
        this.colDescription.FieldName = "description";
        this.colDescription.Name = "colDescription";
        this.colDescription.OptionsColumn.AllowEdit = false;
        this.colDescription.Visible = true;
        this.colDescription.VisibleIndex = 6;
        this.colDescription.Width = 620;
        //
        // colUptUserId
        //
        this.colUptUserId.Caption = "수정자";
        this.colUptUserId.FieldName = "upt_user_id";
        this.colUptUserId.Name = "colUptUserId";
        this.colUptUserId.OptionsColumn.AllowEdit = false;
        this.colUptUserId.Visible = true;
        this.colUptUserId.VisibleIndex = 7;
        this.colUptUserId.Width = 80;
        //
        // colUptDt
        //
        this.colUptDt.Caption = "수정일시";
        this.colUptDt.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
        this.colUptDt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
        this.colUptDt.FieldName = "upt_dt";
        this.colUptDt.Name = "colUptDt";
        this.colUptDt.OptionsColumn.AllowEdit = false;
        this.colUptDt.Visible = true;
        this.colUptDt.VisibleIndex = 8;
        this.colUptDt.Width = 120;
        //
        // sectionHeaderMaster
        //
        this.sectionHeaderMaster.BackColor = System.Drawing.Color.White;
        this.sectionHeaderMaster.Dock = System.Windows.Forms.DockStyle.Top;
        this.sectionHeaderMaster.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.sectionHeaderMaster.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
        this.sectionHeaderMaster.Location = new System.Drawing.Point(0, 8);
        this.sectionHeaderMaster.Name = "sectionHeaderMaster";
        this.sectionHeaderMaster.Size = new System.Drawing.Size(1664, 28);
        this.sectionHeaderMaster.TabIndex = 0;
        this.sectionHeaderMaster.Text = "프로세스 설정 항목 (값 칸만 수정할 수 있습니다)";
        //
        // splitterWyn1
        //
        this.splitterWyn1.Dock = System.Windows.Forms.DockStyle.Top;
        this.splitterWyn1.Location = new System.Drawing.Point(3, 420);
        this.splitterWyn1.Name = "splitterWyn1";
        this.splitterWyn1.Size = new System.Drawing.Size(1664, 10);
        this.splitterWyn1.TabIndex = 1;
        this.splitterWyn1.TabStop = false;
        //
        // panelBottom
        //
        this.panelBottom.Controls.Add(this.grd2);
        this.panelBottom.Controls.Add(this.sectionHeaderSub);
        this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelBottom.Location = new System.Drawing.Point(3, 430);
        this.panelBottom.Name = "panelBottom";
        this.panelBottom.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
        this.panelBottom.Size = new System.Drawing.Size(1664, 276);
        this.panelBottom.TabIndex = 2;
        //
        // grd2
        //
        this.grd2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd2.EmbeddedNavigator.Buttons.Append.Visible = false;
        this.grd2.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
        this.grd2.EmbeddedNavigator.Buttons.Edit.Visible = false;
        this.grd2.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
        this.grd2.EmbeddedNavigator.Buttons.Remove.Visible = false;
        this.grd2.Location = new System.Drawing.Point(0, 36);
        this.grd2.MainView = this.gvw2;
        this.grd2.Name = "grd2";
        this.grd2.Size = new System.Drawing.Size(1664, 240);
        this.grd2.TabIndex = 1;
        this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw2});
        //
        // gvw2
        //
        this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colHistDt,
        this.colHistOld,
        this.colHistNew,
        this.colHistUser,
        this.colHistPc});
        this.gvw2.GridControl = this.grd2;
        this.gvw2.HighlightFocusedRow = true;
        this.gvw2.Name = "gvw2";
        this.gvw2.OptionsBehavior.Editable = false;
        this.gvw2.OptionsView.ColumnAutoWidth = false;
        this.gvw2.OptionsView.ShowGroupPanel = false;
        //
        // colHistDt
        //
        this.colHistDt.Caption = "변경일시";
        this.colHistDt.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
        this.colHistDt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
        this.colHistDt.FieldName = "chg_dt";
        this.colHistDt.Name = "colHistDt";
        this.colHistDt.Visible = true;
        this.colHistDt.VisibleIndex = 0;
        this.colHistDt.Width = 150;
        //
        // colHistOld
        //
        this.colHistOld.Caption = "변경 전";
        this.colHistOld.FieldName = "old_nm";
        this.colHistOld.Name = "colHistOld";
        this.colHistOld.Visible = true;
        this.colHistOld.VisibleIndex = 1;
        this.colHistOld.Width = 140;
        //
        // colHistNew
        //
        this.colHistNew.Caption = "변경 후";
        this.colHistNew.FieldName = "new_nm";
        this.colHistNew.Name = "colHistNew";
        this.colHistNew.Visible = true;
        this.colHistNew.VisibleIndex = 2;
        this.colHistNew.Width = 140;
        //
        // colHistUser
        //
        this.colHistUser.Caption = "변경자";
        this.colHistUser.FieldName = "chg_user_id";
        this.colHistUser.Name = "colHistUser";
        this.colHistUser.Visible = true;
        this.colHistUser.VisibleIndex = 3;
        this.colHistUser.Width = 100;
        //
        // colHistPc
        //
        this.colHistPc.Caption = "PC";
        this.colHistPc.FieldName = "chg_pc";
        this.colHistPc.Name = "colHistPc";
        this.colHistPc.Visible = true;
        this.colHistPc.VisibleIndex = 4;
        this.colHistPc.Width = 160;
        //
        // sectionHeaderSub
        //
        this.sectionHeaderSub.BackColor = System.Drawing.Color.White;
        this.sectionHeaderSub.Dock = System.Windows.Forms.DockStyle.Top;
        this.sectionHeaderSub.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.sectionHeaderSub.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
        this.sectionHeaderSub.Location = new System.Drawing.Point(0, 8);
        this.sectionHeaderSub.Name = "sectionHeaderSub";
        this.sectionHeaderSub.Size = new System.Drawing.Size(1664, 28);
        this.sectionHeaderSub.TabIndex = 0;
        this.sectionHeaderSub.Text = "선택한 설정의 변경 이력";
        //
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
        this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        this.panHeader.Controls.Add(this.lblSearchModule);
        this.panHeader.Controls.Add(this.cboSearchModule);
        this.panHeader.Controls.Add(this.lblSearchKeyword);
        this.panHeader.Controls.Add(this.txtSearchKeyword);
        this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.panHeader.Location = new System.Drawing.Point(5, 0);
        this.panHeader.Name = "panHeader";
        this.panHeader.Size = new System.Drawing.Size(1670, 49);
        this.panHeader.TabIndex = 8;
        //
        // lblSearchModule
        //
        this.lblSearchModule.Location = new System.Drawing.Point(25, 18);
        this.lblSearchModule.Name = "lblSearchModule";
        this.lblSearchModule.Size = new System.Drawing.Size(24, 15);
        this.lblSearchModule.TabIndex = 0;
        this.lblSearchModule.Text = "구분";
        //
        // cboSearchModule
        //
        this.cboSearchModule.Location = new System.Drawing.Point(60, 15);
        this.cboSearchModule.Name = "cboSearchModule";
        this.cboSearchModule.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.cboSearchModule.Properties.Items.AddRange(new object[] {
        "전체",
        "구매",
        "판매",
        "생산",
        "공통"});
        this.cboSearchModule.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
        this.cboSearchModule.Size = new System.Drawing.Size(110, 20);
        this.cboSearchModule.TabIndex = 1;
        //
        // lblSearchKeyword
        //
        this.lblSearchKeyword.Location = new System.Drawing.Point(200, 18);
        this.lblSearchKeyword.Name = "lblSearchKeyword";
        this.lblSearchKeyword.Size = new System.Drawing.Size(60, 15);
        this.lblSearchKeyword.TabIndex = 2;
        this.lblSearchKeyword.Text = "설정명/키";
        //
        // txtSearchKeyword
        //
        this.txtSearchKeyword.Location = new System.Drawing.Point(270, 15);
        this.txtSearchKeyword.Name = "txtSearchKeyword";
        this.txtSearchKeyword.Size = new System.Drawing.Size(200, 20);
        this.txtSearchKeyword.TabIndex = 3;
        //
        // frmProcConfig
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1680, 760);
        this.Controls.Add(this.panBase);
        this.Name = "frmProcConfig";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        this.panBase.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panelSplit)).EndInit();
        this.panelSplit.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panelTop)).EndInit();
        this.panelTop.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelBottom)).EndInit();
        this.panelBottom.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        this.panHeader.ResumeLayout(false);
        this.panHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchModule.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panelSplit;
    private PanelWyn panelTop;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colModuleNm;
    private DevExpress.XtraGrid.Columns.GridColumn colGroupNm;
    private DevExpress.XtraGrid.Columns.GridColumn colConfigNm;
    private DevExpress.XtraGrid.Columns.GridColumn colCurValue;
    private DevExpress.XtraGrid.Columns.GridColumn colDefaultNm;
    private DevExpress.XtraGrid.Columns.GridColumn colStateNm;
    private DevExpress.XtraGrid.Columns.GridColumn colDescription;
    private DevExpress.XtraGrid.Columns.GridColumn colUptUserId;
    private DevExpress.XtraGrid.Columns.GridColumn colUptDt;
    private SectionHeaderWyn sectionHeaderMaster;
    private SplitterWyn splitterWyn1;
    private PanelWyn panelBottom;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private DevExpress.XtraGrid.Columns.GridColumn colHistDt;
    private DevExpress.XtraGrid.Columns.GridColumn colHistOld;
    private DevExpress.XtraGrid.Columns.GridColumn colHistNew;
    private DevExpress.XtraGrid.Columns.GridColumn colHistUser;
    private DevExpress.XtraGrid.Columns.GridColumn colHistPc;
    private SectionHeaderWyn sectionHeaderSub;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchModule;
    private DevExpress.XtraEditors.ComboBoxEdit cboSearchModule;
    private DevExpress.XtraEditors.LabelControl lblSearchKeyword;
    private TextEditWyn txtSearchKeyword;
}
