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
            this.lblSearchAccId = new DevExpress.XtraEditors.LabelControl();
            this.cboSearchAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
        this.lblSearchTitle = new DevExpress.XtraEditors.LabelControl();
        this.txtTitle = new WYNLAB.Base.Controls.TextEditWyn();
            this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn2 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panFileSide = new WYNLAB.Base.Controls.PanelWyn();
            this.panFileHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn5 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panFileBar = new WYNLAB.Base.Controls.PanelWyn();
            this.btnFileAttach = new WYNLAB.Base.Controls.ButtonWyn();
            this.grdFile = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvwFile = new WYNLAB.Base.Controls.GridViewWyn();
            this.colFSerl = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFFileNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFFileSize = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFRemark = new DevExpress.XtraGrid.Columns.GridColumn();
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
            ((System.ComponentModel.ISupportInitialize)(this.panFileSide)).BeginInit();
            this.panFileSide.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panFileHeader)).BeginInit();
            this.panFileHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panFileBar)).BeginInit();
            this.panFileBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).BeginInit();
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
            // 아래쪽 상세 영역 - 왼쪽 panelWyn5(상세 입력)가 Fill, 오른쪽에 첨부파일 패널(panFileSide)을
            // 스플리터와 함께 둔다. Dock=Right 스택은 나중에 추가한 컨트롤이 바깥쪽을 먼저 차지하므로
            // Fill -> 스플리터 -> 첨부 패널 순으로 추가한다.
            this.panelWyn4.Controls.Add(this.panelWyn5);
            this.panelWyn4.Controls.Add(this.splitterWyn2);
            this.panelWyn4.Controls.Add(this.panFileSide);
            this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn4.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn4.Location = new System.Drawing.Point(0, 206);
            this.panelWyn4.Name = "panelWyn4";
            this.panelWyn4.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.panelWyn4.Size = new System.Drawing.Size(1235, 287);
            this.panelWyn4.TabIndex = 7;
            //
            // panelWyn5
            //
            this.panelWyn5.Controls.Add(this.panData);
            this.panelWyn5.Controls.Add(this.panelWyn6);
            this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn5.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn5.Location = new System.Drawing.Point(0, 3);
            this.panelWyn5.Name = "panelWyn5";
            this.panelWyn5.Size = new System.Drawing.Size(849, 284);
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
        // 목록이 위쪽 가로 전체로 올라가면서 상세 영역이 낮아졌다 - 하단이 panData 밖으로 삐져나가지
        // 않게 높이를 줄이고, 오른쪽 닻을 풀어서 폭이 늘어도 우측 필드(x=640)를 침범하지 않게 한다.
        this.txtDetailContent.Size = new System.Drawing.Size(500, 80);
        this.txtDetailContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left)));
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
            this.panelWyn6.Size = new System.Drawing.Size(849, 27);
            this.panelWyn6.TabIndex = 7;
            //
            // splitterWyn1
            //
            this.splitterWyn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitterWyn1.Location = new System.Drawing.Point(0, 200);
            this.splitterWyn1.Name = "splitterWyn1";
            this.splitterWyn1.Size = new System.Drawing.Size(1235, 6);
            this.splitterWyn1.TabIndex = 9;
            this.splitterWyn1.TabStop = false;
            //
            // panelWyn8
            //
            this.panelWyn8.Controls.Add(this.grd1);
            this.panelWyn8.Controls.Add(this.panelWyn2);
            this.panelWyn8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn8.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn8.Location = new System.Drawing.Point(0, 0);
            this.panelWyn8.Name = "panelWyn8";
            this.panelWyn8.Size = new System.Drawing.Size(1235, 200);
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
            this.grd1.Size = new System.Drawing.Size(1235, 173);
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
        this.colMTitle.Width = 420;
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
            this.panelWyn2.Size = new System.Drawing.Size(1235, 27);
            this.panelWyn2.TabIndex = 11;
            // 
            // panHeader
            // 
            this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.lblSearchAccId);
            this.panHeader.Controls.Add(this.cboSearchAccId);
            this.panHeader.Controls.Add(this.labelControl1);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(5, 33);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1235, 49);
            this.panHeader.TabIndex = 8;
            // 
            // lblSearchAccId (조회조건 첫 번째 - 사업장 표준, 2026-10-03)
            // 
            this.lblSearchAccId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSearchAccId.Appearance.Options.UseFont = true;
            this.lblSearchAccId.Location = new System.Drawing.Point(16, 24);
            this.lblSearchAccId.Name = "lblSearchAccId";
            this.lblSearchAccId.Size = new System.Drawing.Size(36, 15);
            this.lblSearchAccId.TabIndex = 0;
            this.lblSearchAccId.Text = "사업장";
            // 
            // cboSearchAccId (사업장 LookUp, Required - 기본값은 화면 생성자에서 로그인 사업장)
            // 
            this.cboSearchAccId.EditValue = "";
            this.cboSearchAccId.Location = new System.Drawing.Point(60, 21);
            this.cboSearchAccId.LookupKey = "L_ACC";
            this.cboSearchAccId.Name = "cboSearchAccId";
            this.cboSearchAccId.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.cboSearchAccId.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboSearchAccId.Properties.Appearance.Options.UseBackColor = true;
            this.cboSearchAccId.Properties.Appearance.Options.UseForeColor = true;
            this.cboSearchAccId.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboSearchAccId.Properties.NullText = "";
            this.cboSearchAccId.Required = true;
            this.cboSearchAccId.Size = new System.Drawing.Size(131, 20);
            this.cboSearchAccId.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(224, 18);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 15);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "검색조건";
            // 
            // txtSearchQ
            //
        this.lblSearchTitle.Location = new System.Drawing.Point(215, 24);
        this.lblSearchTitle.Name = "lblSearchTitle";
        this.lblSearchTitle.Text = "제목";
        this.panHeader.Controls.Add(this.lblSearchTitle);
        this.txtTitle.Location = new System.Drawing.Point(295, 20);
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
            // splitterWyn2
            //
            this.splitterWyn2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn2.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitterWyn2.Location = new System.Drawing.Point(849, 3);
            this.splitterWyn2.Name = "splitterWyn2";
            this.splitterWyn2.Size = new System.Drawing.Size(6, 284);
            this.splitterWyn2.TabIndex = 10;
            this.splitterWyn2.TabStop = false;
            //
            // panFileSide
            //
            this.panFileSide.Controls.Add(this.grdFile);
            this.panFileSide.Controls.Add(this.panFileBar);
            this.panFileSide.Controls.Add(this.panFileHeader);
            this.panFileSide.Dock = System.Windows.Forms.DockStyle.Right;
            this.panFileSide.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panFileSide.Location = new System.Drawing.Point(855, 3);
            this.panFileSide.Name = "panFileSide";
            this.panFileSide.Size = new System.Drawing.Size(380, 284);
            this.panFileSide.TabIndex = 11;
            //
            // panFileHeader
            //
            this.panFileHeader.Controls.Add(this.sectionHeaderWyn5);
            this.panFileHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panFileHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panFileHeader.Location = new System.Drawing.Point(0, 0);
            this.panFileHeader.Name = "panFileHeader";
            this.panFileHeader.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panFileHeader.Size = new System.Drawing.Size(380, 27);
            this.panFileHeader.TabIndex = 12;
            //
            // sectionHeaderWyn5
            //
            this.sectionHeaderWyn5.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn5.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn5.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn5.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn5.Name = "sectionHeaderWyn5";
            this.sectionHeaderWyn5.Size = new System.Drawing.Size(375, 25);
            this.sectionHeaderWyn5.TabIndex = 9;
            this.sectionHeaderWyn5.Text = "첨부파일";
            //
            // panFileBar
            //
            this.panFileBar.Appearance.BackColor = System.Drawing.Color.White;
            this.panFileBar.Appearance.Options.UseBackColor = true;
            this.panFileBar.Controls.Add(this.btnFileAttach);
            this.panFileBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panFileBar.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panFileBar.Location = new System.Drawing.Point(0, 27);
            this.panFileBar.Name = "panFileBar";
            this.panFileBar.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panFileBar.Size = new System.Drawing.Size(380, 30);
            this.panFileBar.TabIndex = 11;
            //
            // btnFileAttach
            //
            this.btnFileAttach.BackColor = System.Drawing.Color.Transparent;
            this.btnFileAttach.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnFileAttach.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFileAttach.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnFileAttach.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnFileAttach.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnFileAttach.Image = null;
            this.btnFileAttach.Location = new System.Drawing.Point(5, 3);
            this.btnFileAttach.Name = "btnFileAttach";
            this.btnFileAttach.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnFileAttach.Size = new System.Drawing.Size(95, 24);
            this.btnFileAttach.TabIndex = 0;
            this.btnFileAttach.Text = "FILE첨부";
            this.btnFileAttach.ToolTip = null;
            //
            // grdFile
            //
            this.grdFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdFile.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdFile.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdFile.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdFile.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdFile.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdFile.Location = new System.Drawing.Point(0, 57);
            this.grdFile.MainView = this.gvwFile;
            this.grdFile.Name = "grdFile";
            this.grdFile.Size = new System.Drawing.Size(380, 227);
            this.grdFile.TabIndex = 1;
            this.grdFile.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvwFile});
            //
            // gvwFile
            //
            this.gvwFile.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colFSerl,
            this.colFFileNm,
            this.colFFileSize,
            this.colFRemark});
            this.gvwFile.GridControl = this.grdFile;
            this.gvwFile.HighlightFocusedRow = true;
            this.gvwFile.Name = "gvwFile";
            this.gvwFile.OptionsBehavior.Editable = false;
            this.gvwFile.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvwFile.OptionsView.ColumnAutoWidth = false;
            this.gvwFile.OptionsView.ShowGroupPanel = false;
            //
            // colFSerl
            //
            this.colFSerl.Caption = "순번";
            this.colFSerl.FieldName = "Serl";
            this.colFSerl.Name = "colFSerl";
            this.colFSerl.Visible = true;
            this.colFSerl.VisibleIndex = 0;
            this.colFSerl.Width = 44;
            //
            // colFFileNm
            //
            this.colFFileNm.Caption = "FILE NAME";
            this.colFFileNm.FieldName = "FileNm";
            this.colFFileNm.Name = "colFFileNm";
            this.colFFileNm.Visible = true;
            this.colFFileNm.VisibleIndex = 1;
            this.colFFileNm.Width = 170;
            //
            // colFFileSize
            //
            this.colFFileSize.Caption = "FileSize";
            this.colFFileSize.FieldName = "FileSize";
            this.colFFileSize.Name = "colFFileSize";
            this.colFFileSize.Visible = true;
            this.colFFileSize.VisibleIndex = 2;
            this.colFFileSize.Width = 70;
            //
            // colFRemark
            //
            this.colFRemark.Caption = "비고";
            this.colFRemark.FieldName = "Remark";
            this.colFRemark.Name = "colFRemark";
            this.colFRemark.Visible = true;
            this.colFRemark.VisibleIndex = 3;
            this.colFRemark.Width = 90;
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
            ((System.ComponentModel.ISupportInitialize)(this.panFileSide)).EndInit();
            this.panFileSide.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panFileHeader)).EndInit();
            this.panFileHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panFileBar)).EndInit();
            this.panFileBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).EndInit();
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
    private SplitterWyn splitterWyn2;
    private PanelWyn panFileSide;
    private PanelWyn panFileHeader;
    private SectionHeaderWyn sectionHeaderWyn5;
    private PanelWyn panFileBar;
    private ButtonWyn btnFileAttach;
    private GridControlWyn grdFile;
    private GridViewWyn gvwFile;
    private DevExpress.XtraGrid.Columns.GridColumn colFSerl;
    private DevExpress.XtraGrid.Columns.GridColumn colFFileNm;
    private DevExpress.XtraGrid.Columns.GridColumn colFFileSize;
    private DevExpress.XtraGrid.Columns.GridColumn colFRemark;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private WYNLAB.Base.Controls.LookUpEditWyn cboSearchAccId;
}
