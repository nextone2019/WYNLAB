// AI Builder가 마스터-폼-서브그리드 템플릿을 복제해서 자동 생성 - 2026-09-17.
// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.SM;

public partial class frmBoard
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBoard));
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
        this.lblDetailBoardId = new DevExpress.XtraEditors.LabelControl();
        this.txtDetailBoardId = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDetailAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboDetailAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblDetailTitle = new DevExpress.XtraEditors.LabelControl();
        this.txtDetailTitle = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDetailContent = new DevExpress.XtraEditors.LabelControl();
        this.txtDetailContent = new WYNLAB.Base.Controls.MemoEditWyn();
        this.lblDetailEmpNm = new DevExpress.XtraEditors.LabelControl();
        this.txtDetailEmpNm = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblDetailImportantYn = new DevExpress.XtraEditors.LabelControl();
        this.chkDetailImportantYn = new WYNLAB.Base.Controls.CheckBoxWyn();
        this.lblDetailUseYn = new DevExpress.XtraEditors.LabelControl();
        this.chkDetailUseYn = new WYNLAB.Base.Controls.CheckBoxWyn();
        this.lblDetailRegDt = new DevExpress.XtraEditors.LabelControl();
        this.txtDetailRegDt = new WYNLAB.Base.Controls.TextEditWyn();
            this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn8 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.colMBoardId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMAccId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMTitle = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMEmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMImportantYn = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMUseYn = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colMRegDt = new DevExpress.XtraGrid.Columns.GridColumn();
        this.chkEditcolM = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
        this.lookUpcolMAccId = new WYNLAB.Base.Controls.LookUpColumnEdit();
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
        this.lblDetailBoardId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailBoardId.Appearance.Options.UseFont = true;
        this.lblDetailBoardId.Location = new System.Drawing.Point(16, 19);
        this.lblDetailBoardId.Name = "lblDetailBoardId";
        this.lblDetailBoardId.Text = "게시물ID";
        this.txtDetailBoardId.Location = new System.Drawing.Point(120, 16);
        this.txtDetailBoardId.Name = "txtDetailBoardId";
        this.txtDetailBoardId.Size = new System.Drawing.Size(220, 20);
        this.txtDetailBoardId.Properties.ReadOnly = true;
        this.panData.Controls.Add(this.lblDetailBoardId);
        this.panData.Controls.Add(this.txtDetailBoardId);
        this.lblDetailAccId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailAccId.Appearance.Options.UseFont = true;
        this.lblDetailAccId.Location = new System.Drawing.Point(16, 47);
        this.lblDetailAccId.Name = "lblDetailAccId";
        this.lblDetailAccId.Text = "사업장";
        this.cboDetailAccId.Location = new System.Drawing.Point(120, 44);
        this.cboDetailAccId.Name = "cboDetailAccId";
        this.cboDetailAccId.Size = new System.Drawing.Size(220, 20);
        this.cboDetailAccId.LookupKey = "L_ACC";
        this.cboDetailAccId.Required = true;
        this.panData.Controls.Add(this.lblDetailAccId);
        this.panData.Controls.Add(this.cboDetailAccId);
        this.lblDetailTitle.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailTitle.Appearance.Options.UseFont = true;
        this.lblDetailTitle.Location = new System.Drawing.Point(16, 75);
        this.lblDetailTitle.Name = "lblDetailTitle";
        this.lblDetailTitle.Text = "제목";
        this.txtDetailTitle.Location = new System.Drawing.Point(120, 72);
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
        this.txtDetailContent.Location = new System.Drawing.Point(120, 100);
        this.txtDetailContent.Name = "txtDetailContent";
        this.txtDetailContent.Size = new System.Drawing.Size(500, 130);
        this.txtDetailContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.panData.Controls.Add(this.lblDetailContent);
        this.panData.Controls.Add(this.txtDetailContent);
        this.lblDetailEmpNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailEmpNm.Appearance.Options.UseFont = true;
        this.lblDetailEmpNm.Location = new System.Drawing.Point(640, 19);
        this.lblDetailEmpNm.Name = "lblDetailEmpNm";
        this.lblDetailEmpNm.Text = "작성자";
        this.txtDetailEmpNm.Location = new System.Drawing.Point(700, 16);
        this.txtDetailEmpNm.Name = "txtDetailEmpNm";
        this.txtDetailEmpNm.Size = new System.Drawing.Size(120, 20);
        this.txtDetailEmpNm.Properties.ReadOnly = true;
        this.panData.Controls.Add(this.lblDetailEmpNm);
        this.panData.Controls.Add(this.txtDetailEmpNm);
        this.lblDetailImportantYn.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailImportantYn.Appearance.Options.UseFont = true;
        this.lblDetailImportantYn.Location = new System.Drawing.Point(640, 47);
        this.lblDetailImportantYn.Name = "lblDetailImportantYn";
        this.lblDetailImportantYn.Text = "중요공지";
        this.chkDetailImportantYn.Location = new System.Drawing.Point(700, 44);
        this.chkDetailImportantYn.Name = "chkDetailImportantYn";
        this.panData.Controls.Add(this.lblDetailImportantYn);
        this.panData.Controls.Add(this.chkDetailImportantYn);
        this.lblDetailUseYn.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailUseYn.Appearance.Options.UseFont = true;
        this.lblDetailUseYn.Location = new System.Drawing.Point(640, 75);
        this.lblDetailUseYn.Name = "lblDetailUseYn";
        this.lblDetailUseYn.Text = "게시여부";
        this.chkDetailUseYn.Location = new System.Drawing.Point(700, 72);
        this.chkDetailUseYn.Name = "chkDetailUseYn";
        this.panData.Controls.Add(this.lblDetailUseYn);
        this.panData.Controls.Add(this.chkDetailUseYn);
        this.lblDetailRegDt.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblDetailRegDt.Appearance.Options.UseFont = true;
        this.lblDetailRegDt.Location = new System.Drawing.Point(640, 103);
        this.lblDetailRegDt.Name = "lblDetailRegDt";
        this.lblDetailRegDt.Text = "등록일시";
        this.txtDetailRegDt.Location = new System.Drawing.Point(700, 100);
        this.txtDetailRegDt.Name = "txtDetailRegDt";
        this.txtDetailRegDt.Size = new System.Drawing.Size(120, 20);
        this.txtDetailRegDt.Properties.ReadOnly = true;
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
        // colMBoardId
        //
        this.colMBoardId.Caption = "게시물ID";
        this.colMBoardId.FieldName = "board_id";
        this.colMBoardId.Name = "colMBoardId";
        this.colMBoardId.OptionsColumn.AllowEdit = false;
        this.colMBoardId.Visible = true;
        this.colMBoardId.VisibleIndex = 0;
        this.colMBoardId.Width = 100;
        //
        // colMAccId
        //
        this.colMAccId.Caption = "사업장";
        this.colMAccId.FieldName = "acc_id";
        this.colMAccId.Name = "colMAccId";
        this.colMAccId.ColumnEdit = this.lookUpcolMAccId;
        this.colMAccId.Visible = true;
        this.colMAccId.VisibleIndex = 1;
        this.colMAccId.Width = 100;
        //
        // colMTitle
        //
        this.colMTitle.Caption = "제목";
        this.colMTitle.FieldName = "title";
        this.colMTitle.Name = "colMTitle";
        this.colMTitle.Visible = true;
        this.colMTitle.VisibleIndex = 2;
        this.colMTitle.Width = 100;
        //
        // colMEmpNm
        //
        this.colMEmpNm.Caption = "작성자";
        this.colMEmpNm.FieldName = "emp_nm";
        this.colMEmpNm.Name = "colMEmpNm";
        this.colMEmpNm.Visible = true;
        this.colMEmpNm.VisibleIndex = 3;
        this.colMEmpNm.Width = 100;
        //
        // colMImportantYn
        //
        this.colMImportantYn.Caption = "중요공지";
        this.colMImportantYn.FieldName = "important_yn";
        this.colMImportantYn.Name = "colMImportantYn";
        this.colMImportantYn.ColumnEdit = this.chkEditcolM;
        this.colMImportantYn.Visible = true;
        this.colMImportantYn.VisibleIndex = 4;
        this.colMImportantYn.Width = 100;
        //
        // colMUseYn
        //
        this.colMUseYn.Caption = "게시여부";
        this.colMUseYn.FieldName = "use_yn";
        this.colMUseYn.Name = "colMUseYn";
        this.colMUseYn.ColumnEdit = this.chkEditcolM;
        this.colMUseYn.Visible = true;
        this.colMUseYn.VisibleIndex = 5;
        this.colMUseYn.Width = 100;
        //
        // colMRegDt
        //
        this.colMRegDt.Caption = "등록일시";
        this.colMRegDt.FieldName = "reg_dt";
        this.colMRegDt.Name = "colMRegDt";
        this.colMRegDt.Visible = true;
        this.colMRegDt.VisibleIndex = 6;
        this.colMRegDt.Width = 100;
        //
        // chkEditcolM
        //
        this.chkEditcolM.Name = "chkEditcolM";
        //
        // lookUpcolMAccId
        //
        this.lookUpcolMAccId.LookupKey = "L_ACC";
        this.lookUpcolMAccId.Name = "lookUpcolMAccId";
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colMBoardId,
            this.colMAccId,
            this.colMTitle,
            this.colMEmpNm,
            this.colMImportantYn,
            this.colMUseYn,
            this.colMRegDt});
        this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { this.chkEditcolM, this.lookUpcolMAccId});
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
            this.sectionHeaderWyn1.Text = "공지사항등록 [frmBoard]";
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
            // frmBoard
            //
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1245, 580);
            this.Controls.Add(this.panBase);
            this.Name = "frmBoard";
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
    private DevExpress.XtraEditors.LabelControl lblDetailBoardId;
    private TextEditWyn txtDetailBoardId;
    private DevExpress.XtraEditors.LabelControl lblDetailAccId;
    private LookUpEditWyn cboDetailAccId;
    private DevExpress.XtraEditors.LabelControl lblDetailTitle;
    private TextEditWyn txtDetailTitle;
    private DevExpress.XtraEditors.LabelControl lblDetailContent;
    private MemoEditWyn txtDetailContent;
    private DevExpress.XtraEditors.LabelControl lblDetailEmpNm;
    private TextEditWyn txtDetailEmpNm;
    private DevExpress.XtraEditors.LabelControl lblDetailImportantYn;
    private CheckBoxWyn chkDetailImportantYn;
    private DevExpress.XtraEditors.LabelControl lblDetailUseYn;
    private CheckBoxWyn chkDetailUseYn;
    private DevExpress.XtraEditors.LabelControl lblDetailRegDt;
    private TextEditWyn txtDetailRegDt;
    private PanelWyn panelWyn6;
    private SplitterWyn splitterWyn1;
    private PanelWyn panelWyn8;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colMBoardId;
    private DevExpress.XtraGrid.Columns.GridColumn colMAccId;
    private DevExpress.XtraGrid.Columns.GridColumn colMTitle;
    private DevExpress.XtraGrid.Columns.GridColumn colMEmpNm;
    private DevExpress.XtraGrid.Columns.GridColumn colMImportantYn;
    private DevExpress.XtraGrid.Columns.GridColumn colMUseYn;
    private DevExpress.XtraGrid.Columns.GridColumn colMRegDt;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkEditcolM;
    private LookUpColumnEdit lookUpcolMAccId;
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
