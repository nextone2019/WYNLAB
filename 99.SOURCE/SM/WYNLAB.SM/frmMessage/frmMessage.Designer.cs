// AI Builder가 마스터-폼-서브그리드 템플릿을 복제해서 자동 생성 - 2026-09-17.
// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.SM;

public partial class frmMessage
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMessage));
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
        this.lblDetailMsgId = new DevExpress.XtraEditors.LabelControl();
        this.txtDetailMsgId = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDetailBoxType = new DevExpress.XtraEditors.LabelControl();
        this.txtDetailBoxType = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDetailFromEmpNm = new DevExpress.XtraEditors.LabelControl();
        this.txtDetailFromEmpNm = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDetailToEmpNo = new DevExpress.XtraEditors.LabelControl();
        this.txtDetailToEmpNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDetailToEmpNm = new DevExpress.XtraEditors.LabelControl();
        this.txtDetailToEmpNm = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDetailTitle = new DevExpress.XtraEditors.LabelControl();
        this.txtDetailTitle = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDetailContent = new DevExpress.XtraEditors.LabelControl();
        this.memDetailContent = new WYNLAB.Base.Controls.MemoEditWyn();
        this.lblDetailReadYn = new DevExpress.XtraEditors.LabelControl();
        this.chkDetailReadYn = new WYNLAB.Base.Controls.CheckBoxWyn();
        this.lblDetailRegDt = new DevExpress.XtraEditors.LabelControl();
        this.txtDetailRegDt = new WYNLAB.Base.Controls.TextEditWyn();
            this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn8 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.colMMsgId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMBoxType = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMFromEmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMToEmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMTitle = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMReadYn = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMRegDt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.chkEditcolM = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
        this.lblSearchTitle = new DevExpress.XtraEditors.LabelControl();
        this.txtTitle = new WYNLAB.Base.Controls.TextEditWyn();
            this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
            this.panBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
            this.panelWyn3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).BeginInit();
            this.panelWyn4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
            this.panData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).BeginInit();
            this.panelWyn6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).BeginInit();
            this.panelWyn8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).BeginInit();
            this.panelWyn2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).BeginInit();
            this.paTitle.SuspendLayout();
            this.SuspendLayout();
            //
            // panBase
            //
            this.panBase.Appearance.BackColor = System.Drawing.Color.White;
            this.panBase.Appearance.Options.UseBackColor = true;
            this.panBase.Controls.Add(this.panelWyn3);
            this.panBase.Controls.Add(this.panHeader);
            this.panBase.Controls.Add(this.paTitle);
            this.panBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBase.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panBase.Location = new System.Drawing.Point(0, 0);
            this.panBase.Name = "panBase";
            this.panBase.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.panBase.Size = new System.Drawing.Size(1245, 580);
            this.panBase.TabIndex = 6;
            //
            // panelWyn3
            //
            this.panelWyn3.Controls.Add(this.panelWyn4);
            this.panelWyn3.Controls.Add(this.splitterWyn1);
            this.panelWyn3.Controls.Add(this.panelWyn8);
            this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn3.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn3.Location = new System.Drawing.Point(5, 82);
            this.panelWyn3.Name = "panelWyn3";
            this.panelWyn3.Size = new System.Drawing.Size(1235, 493);
            this.panelWyn3.TabIndex = 7;
            //
            // panelWyn4
            //
            this.panelWyn4.Controls.Add(this.panelWyn5);
            this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn4.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn4.Location = new System.Drawing.Point(412, 0);
            this.panelWyn4.Name = "panelWyn4";
            this.panelWyn4.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.panelWyn4.Size = new System.Drawing.Size(823, 493);
            this.panelWyn4.TabIndex = 7;
            //
            // panelWyn5
            //
            this.panelWyn5.Controls.Add(this.panData);
            this.panelWyn5.Controls.Add(this.panelWyn6);
            this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn5.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn5.Location = new System.Drawing.Point(3, 0);
            this.panelWyn5.Name = "panelWyn5";
            this.panelWyn5.Size = new System.Drawing.Size(820, 493);
            this.panelWyn5.TabIndex = 6;
            //
            // panData
            //
            this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(0, 27);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(820, 191);
            this.panData.TabIndex = 8;
            //
            // lblDetailSample1
            //
        this.lblDetailMsgId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailMsgId.Appearance.Options.UseFont = true;
        this.lblDetailMsgId.Location = new System.Drawing.Point(16, 19);
        this.lblDetailMsgId.Name = "lblDetailMsgId";
        this.lblDetailMsgId.Text = "쪽지ID";
        this.txtDetailMsgId.Location = new System.Drawing.Point(120, 16);
        this.txtDetailMsgId.Name = "txtDetailMsgId";
        this.txtDetailMsgId.Size = new System.Drawing.Size(150, 20);
        this.txtDetailMsgId.Properties.ReadOnly = true;
        this.panData.Controls.Add(this.lblDetailMsgId);
        this.panData.Controls.Add(this.txtDetailMsgId);
        this.lblDetailBoxType.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailBoxType.Appearance.Options.UseFont = true;
        this.lblDetailBoxType.Location = new System.Drawing.Point(300, 19);
        this.lblDetailBoxType.Name = "lblDetailBoxType";
        this.lblDetailBoxType.Text = "구분";
        this.txtDetailBoxType.Location = new System.Drawing.Point(400, 16);
        this.txtDetailBoxType.Name = "txtDetailBoxType";
        this.txtDetailBoxType.Size = new System.Drawing.Size(150, 20);
        this.panData.Controls.Add(this.lblDetailBoxType);
        this.panData.Controls.Add(this.txtDetailBoxType);
        this.lblDetailFromEmpNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailFromEmpNm.Appearance.Options.UseFont = true;
        this.lblDetailFromEmpNm.Location = new System.Drawing.Point(16, 47);
        this.lblDetailFromEmpNm.Name = "lblDetailFromEmpNm";
        this.lblDetailFromEmpNm.Text = "보낸사람";
        this.txtDetailFromEmpNm.Location = new System.Drawing.Point(120, 44);
        this.txtDetailFromEmpNm.Name = "txtDetailFromEmpNm";
        this.txtDetailFromEmpNm.Size = new System.Drawing.Size(150, 20);
        this.panData.Controls.Add(this.lblDetailFromEmpNm);
        this.panData.Controls.Add(this.txtDetailFromEmpNm);
        this.lblDetailToEmpNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailToEmpNo.Appearance.Options.UseFont = true;
        this.lblDetailToEmpNo.Location = new System.Drawing.Point(300, 47);
        this.lblDetailToEmpNo.Name = "lblDetailToEmpNo";
        this.lblDetailToEmpNo.Text = "받는사람(사번)";
        this.txtDetailToEmpNo.Location = new System.Drawing.Point(400, 44);
        this.txtDetailToEmpNo.Name = "txtDetailToEmpNo";
        this.txtDetailToEmpNo.Size = new System.Drawing.Size(150, 20);
        this.txtDetailToEmpNo.Required = true;
        this.panData.Controls.Add(this.lblDetailToEmpNo);
        this.panData.Controls.Add(this.txtDetailToEmpNo);
        this.lblDetailToEmpNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailToEmpNm.Appearance.Options.UseFont = true;
        this.lblDetailToEmpNm.Location = new System.Drawing.Point(16, 75);
        this.lblDetailToEmpNm.Name = "lblDetailToEmpNm";
        this.lblDetailToEmpNm.Text = "받는사람";
        this.txtDetailToEmpNm.Location = new System.Drawing.Point(120, 72);
        this.txtDetailToEmpNm.Name = "txtDetailToEmpNm";
        this.txtDetailToEmpNm.Size = new System.Drawing.Size(150, 20);
        this.panData.Controls.Add(this.lblDetailToEmpNm);
        this.panData.Controls.Add(this.txtDetailToEmpNm);
        this.lblDetailTitle.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailTitle.Appearance.Options.UseFont = true;
        this.lblDetailTitle.Location = new System.Drawing.Point(300, 75);
        this.lblDetailTitle.Name = "lblDetailTitle";
        this.lblDetailTitle.Text = "제목";
        this.txtDetailTitle.Location = new System.Drawing.Point(400, 72);
        this.txtDetailTitle.Name = "txtDetailTitle";
        this.txtDetailTitle.Size = new System.Drawing.Size(220, 20);
        this.txtDetailTitle.Required = true;
        this.panData.Controls.Add(this.lblDetailTitle);
        this.panData.Controls.Add(this.txtDetailTitle);
        this.lblDetailContent.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailContent.Appearance.Options.UseFont = true;
        this.lblDetailContent.Location = new System.Drawing.Point(16, 103);
        this.lblDetailContent.Name = "lblDetailContent";
        this.lblDetailContent.Text = "내용";
        this.memDetailContent.Location = new System.Drawing.Point(120, 100);
        this.memDetailContent.Name = "memDetailContent";
        this.memDetailContent.Size = new System.Drawing.Size(500, 130);
        this.memDetailContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.panData.Controls.Add(this.lblDetailContent);
        this.panData.Controls.Add(this.memDetailContent);
        this.lblDetailReadYn.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailReadYn.Appearance.Options.UseFont = true;
        this.lblDetailReadYn.Location = new System.Drawing.Point(640, 103);
        this.lblDetailReadYn.Name = "lblDetailReadYn";
        this.lblDetailReadYn.Text = "읽음";
        this.chkDetailReadYn.Location = new System.Drawing.Point(700, 100);
        this.chkDetailReadYn.Name = "chkDetailReadYn";
        this.panData.Controls.Add(this.lblDetailReadYn);
        this.panData.Controls.Add(this.chkDetailReadYn);
        this.lblDetailRegDt.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailRegDt.Appearance.Options.UseFont = true;
        this.lblDetailRegDt.Location = new System.Drawing.Point(640, 131);
        this.lblDetailRegDt.Name = "lblDetailRegDt";
        this.lblDetailRegDt.Text = "보낸일시";
        this.txtDetailRegDt.Location = new System.Drawing.Point(700, 128);
        this.txtDetailRegDt.Name = "txtDetailRegDt";
        this.txtDetailRegDt.Size = new System.Drawing.Size(120, 20);
        this.panData.Controls.Add(this.lblDetailRegDt);
        this.panData.Controls.Add(this.txtDetailRegDt);
            //
            // panelWyn6
            //
            this.panelWyn6.Controls.Add(this.sectionHeaderWyn3);
            this.panelWyn6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn6.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn6.Location = new System.Drawing.Point(0, 0);
            this.panelWyn6.Name = "panelWyn6";
            this.panelWyn6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn6.Size = new System.Drawing.Size(820, 27);
            this.panelWyn6.TabIndex = 7;
            //
            // splitterWyn1
            //
            this.splitterWyn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn1.Location = new System.Drawing.Point(402, 0);
            this.splitterWyn1.Name = "splitterWyn1";
            this.splitterWyn1.Size = new System.Drawing.Size(10, 493);
            this.splitterWyn1.TabIndex = 9;
            this.splitterWyn1.TabStop = false;
            //
            // panelWyn8
            //
            this.panelWyn8.Controls.Add(this.grd1);
            this.panelWyn8.Controls.Add(this.panelWyn2);
            this.panelWyn8.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelWyn8.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn8.Location = new System.Drawing.Point(0, 0);
            this.panelWyn8.Name = "panelWyn8";
            this.panelWyn8.Size = new System.Drawing.Size(402, 493);
            this.panelWyn8.TabIndex = 12;
            //
            // grd1
            //
            this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd1.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grd1.Location = new System.Drawing.Point(0, 27);
            this.grd1.MainView = this.gvw1;
            this.grd1.Name = "grd1";
            this.grd1.Size = new System.Drawing.Size(402, 466);
            this.grd1.TabIndex = 10;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            //
            // gvw1
            //
            this.gvw1.GridControl = this.grd1;
            this.gvw1.HighlightFocusedRow = true;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsBehavior.Editable = false;
            this.gvw1.OptionsSelection.InvertSelection = true;
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            //
            // colM1
            //
        //
        // colMMsgId
        //
        this.colMMsgId.Caption = "쪽지ID";
        this.colMMsgId.FieldName = "msg_id";
        this.colMMsgId.Name = "colMMsgId";
        this.colMMsgId.OptionsColumn.AllowEdit = false;
        this.colMMsgId.Visible = true;
        this.colMMsgId.VisibleIndex = 0;
        this.colMMsgId.Width = 60;
        //
        // colMBoxType
        //
        this.colMBoxType.Caption = "구분";
        this.colMBoxType.FieldName = "box_type";
        this.colMBoxType.Name = "colMBoxType";
        this.colMBoxType.Visible = true;
        this.colMBoxType.VisibleIndex = 1;
        this.colMBoxType.Width = 50;
        //
        // colMFromEmpNm
        //
        this.colMFromEmpNm.Caption = "보낸사람";
        this.colMFromEmpNm.FieldName = "from_emp_nm";
        this.colMFromEmpNm.Name = "colMFromEmpNm";
        this.colMFromEmpNm.Visible = true;
        this.colMFromEmpNm.VisibleIndex = 2;
        this.colMFromEmpNm.Width = 70;
        //
        // colMToEmpNm
        //
        this.colMToEmpNm.Caption = "받는사람";
        this.colMToEmpNm.FieldName = "to_emp_nm";
        this.colMToEmpNm.Name = "colMToEmpNm";
        this.colMToEmpNm.Visible = true;
        this.colMToEmpNm.VisibleIndex = 3;
        this.colMToEmpNm.Width = 70;
        //
        // colMTitle
        //
        this.colMTitle.Caption = "제목";
        this.colMTitle.FieldName = "title";
        this.colMTitle.Name = "colMTitle";
        this.colMTitle.Visible = true;
        this.colMTitle.VisibleIndex = 4;
        this.colMTitle.Width = 120;
        //
        // colMReadYn
        //
        this.colMReadYn.Caption = "읽음";
        this.colMReadYn.FieldName = "read_yn";
        this.colMReadYn.Name = "colMReadYn";
        this.colMReadYn.ColumnEdit = this.chkEditcolM;
        this.colMReadYn.Visible = true;
        this.colMReadYn.VisibleIndex = 5;
        this.colMReadYn.Width = 50;
        //
        // colMRegDt
        //
        this.colMRegDt.Caption = "보낸일시";
        this.colMRegDt.FieldName = "reg_dt";
        this.colMRegDt.Name = "colMRegDt";
        this.colMRegDt.Visible = true;
        this.colMRegDt.VisibleIndex = 6;
        this.colMRegDt.Width = 100;
        //
        // chkEditcolM
        //
        this.chkEditcolM.Name = "chkEditcolM";
        this.chkEditcolM.ValueChecked = "Y";
        this.chkEditcolM.ValueUnchecked = "N";
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colMMsgId,
            this.colMBoxType,
            this.colMFromEmpNm,
            this.colMToEmpNm,
            this.colMTitle,
            this.colMReadYn,
            this.colMRegDt});
        this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { this.chkEditcolM});
            //
            // panelWyn2
            //
            this.panelWyn2.Controls.Add(this.sectionHeaderWyn4);
            this.panelWyn2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn2.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn2.Location = new System.Drawing.Point(0, 0);
            this.panelWyn2.Name = "panelWyn2";
            this.panelWyn2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn2.Size = new System.Drawing.Size(402, 27);
            this.panelWyn2.TabIndex = 11;
            //
            // panHeader
            //
            this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.labelControl1);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(5, 33);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1235, 49);
            this.panHeader.TabIndex = 8;
            //
            // labelControl1
            //
            this.labelControl1.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(25, 18);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 15);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "검색조건";
            //
            // txtSearchQ
            //
        this.lblSearchTitle.Location = new System.Drawing.Point(16, 24);
        this.lblSearchTitle.Name = "lblSearchTitle";
        this.lblSearchTitle.Text = "제목";
        this.panHeader.Controls.Add(this.lblSearchTitle);
        this.txtTitle.Location = new System.Drawing.Point(96, 20);
        this.txtTitle.Name = "txtTitle";
        this.txtTitle.Size = new System.Drawing.Size(150, 20);
        this.panHeader.Controls.Add(this.txtTitle);
            //
            // paTitle
            //
            this.paTitle.Controls.Add(this.sectionHeaderWyn1);
            this.paTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitle.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitle.Location = new System.Drawing.Point(5, 0);
            this.paTitle.Name = "paTitle";
            this.paTitle.Size = new System.Drawing.Size(1235, 33);
            this.paTitle.TabIndex = 5;
            //
            // sectionHeaderWyn1
            //
            this.sectionHeaderWyn1.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn1.Location = new System.Drawing.Point(0, 0);
            this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(1235, 33);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 9;
            this.sectionHeaderWyn1.Text = "쪽지함 [frmMessage]";
            //
            // sectionHeaderWyn4
            //
            this.sectionHeaderWyn4.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn4.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn4.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn4.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn4.Name = "sectionHeaderWyn4";
            this.sectionHeaderWyn4.Size = new System.Drawing.Size(397, 25);
            this.sectionHeaderWyn4.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn4.SvgIcon")));
            this.sectionHeaderWyn4.TabIndex = 9;
            this.sectionHeaderWyn4.Text = "목록";
            //
            // sectionHeaderWyn3
            //
            this.sectionHeaderWyn3.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn3.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn3.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn3.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn3.Name = "sectionHeaderWyn3";
            this.sectionHeaderWyn3.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.sectionHeaderWyn3.Size = new System.Drawing.Size(815, 25);
            this.sectionHeaderWyn3.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn3.SvgIcon")));
            this.sectionHeaderWyn3.TabIndex = 9;
            this.sectionHeaderWyn3.Text = "상세 등록";
            //
            // frmMessage
            //
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1245, 580);
            this.Controls.Add(this.panBase);
            this.Name = "frmMessage";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
            this.panelWyn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
            this.panelWyn4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
            this.panelWyn5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
            this.panData.ResumeLayout(false);
            this.panData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).EndInit();
            this.panelWyn6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).EndInit();
            this.panelWyn8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).EndInit();
            this.panelWyn2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).EndInit();
            this.paTitle.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    private PanelWyn panBase;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn4;
    private PanelWyn panelWyn5;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblDetailMsgId;
    private TextEditWyn txtDetailMsgId;
    private DevExpress.XtraEditors.LabelControl lblDetailBoxType;
    private TextEditWyn txtDetailBoxType;
    private DevExpress.XtraEditors.LabelControl lblDetailFromEmpNm;
    private TextEditWyn txtDetailFromEmpNm;
    private DevExpress.XtraEditors.LabelControl lblDetailToEmpNo;
    private TextEditWyn txtDetailToEmpNo;
    private DevExpress.XtraEditors.LabelControl lblDetailToEmpNm;
    private TextEditWyn txtDetailToEmpNm;
    private DevExpress.XtraEditors.LabelControl lblDetailTitle;
    private TextEditWyn txtDetailTitle;
    private DevExpress.XtraEditors.LabelControl lblDetailContent;
    private MemoEditWyn memDetailContent;
    private DevExpress.XtraEditors.LabelControl lblDetailReadYn;
    private CheckBoxWyn chkDetailReadYn;
    private DevExpress.XtraEditors.LabelControl lblDetailRegDt;
    private TextEditWyn txtDetailRegDt;
    private PanelWyn panelWyn6;
    private SplitterWyn splitterWyn1;
    private PanelWyn panelWyn8;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colMMsgId;
    private DevExpress.XtraGrid.Columns.GridColumn colMBoxType;
    private DevExpress.XtraGrid.Columns.GridColumn colMFromEmpNm;
    private DevExpress.XtraGrid.Columns.GridColumn colMToEmpNm;
    private DevExpress.XtraGrid.Columns.GridColumn colMTitle;
    private DevExpress.XtraGrid.Columns.GridColumn colMReadYn;
    private DevExpress.XtraGrid.Columns.GridColumn colMRegDt;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkEditcolM;
    private PanelWyn panelWyn2;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchTitle;
    private TextEditWyn txtTitle;
    private DevExpress.XtraEditors.LabelControl labelControl1;
    private PanelWyn paTitle;
    private SectionHeaderWyn sectionHeaderWyn3;
    private SectionHeaderWyn sectionHeaderWyn4;
    private SectionHeaderWyn sectionHeaderWyn1;
}
