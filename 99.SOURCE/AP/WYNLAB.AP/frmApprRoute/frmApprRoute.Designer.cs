// 결재경로관리 - 결재문서현황과 같은 구조(2026-10-04): 검색조건(사업장/결재경로명) / 왼쪽 결재경로 리스트 / 오른쪽 결재경로상세(경로명·코드 + 조직도 + 승인부·수신부 구성).
// VS 디자이너로 자유롭게 편집 가능 - 화면 로직은 frmApprRoute.cs.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.AP;

public partial class frmApprRoute
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
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.lblSearchAccId = new DevExpress.XtraEditors.LabelControl();
        this.cboSearchAccId = new WYNLAB.Base.Controls.LookUpEditWyn();
        this.lblSearchRouteNm = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchRouteNm = new WYNLAB.Base.Controls.TextEditWyn();
        this.panelSplit = new WYNLAB.Base.Controls.PanelWyn();
        this.panelLeft = new WYNLAB.Base.Controls.PanelWyn();
        this.sectionHeaderMaster = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.colRouteId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colRouteNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
        this.panelRight = new WYNLAB.Base.Controls.PanelWyn();
        this.sectionHeaderDetail = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panData = new WYNLAB.Base.Controls.PanelWyn();
        this.lblRouteNm = new DevExpress.XtraEditors.LabelControl();
        this.txtRouteNm = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblRouteId = new DevExpress.XtraEditors.LabelControl();
        this.txtRouteId = new WYNLAB.Base.Controls.TextEditWyn();
        this.panelEdit = new WYNLAB.Base.Controls.PanelWyn();
        this.panelTree = new WYNLAB.Base.Controls.PanelWyn();
        this.sectionHeaderTree = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.treeEmp = new WYNLAB.Base.Controls.TreeListWyn();
        this.colTreeNm = new DevExpress.XtraTreeList.Columns.TreeListColumn();
        this.colTreeEmpNo = new DevExpress.XtraTreeList.Columns.TreeListColumn();
        this.colTreeJobGrade = new DevExpress.XtraTreeList.Columns.TreeListColumn();
        this.panelArrows = new WYNLAB.Base.Controls.PanelWyn();
        this.btnToLine = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnToRecv = new WYNLAB.Base.Controls.ButtonWyn();
        this.panelLists = new WYNLAB.Base.Controls.PanelWyn();
        this.panelDetailLine = new WYNLAB.Base.Controls.PanelWyn();
        this.panLineHead = new WYNLAB.Base.Controls.PanelWyn();
        this.sectionHeaderLine = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.btnAddLine = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnRemoveLine = new WYNLAB.Base.Controls.ButtonWyn();
        this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
        this.colLineSort = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLineEmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colLineEmpNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.splitterWyn3 = new WYNLAB.Base.Controls.SplitterWyn();
        this.panelDetailRecv = new WYNLAB.Base.Controls.PanelWyn();
        this.panRecvHead = new WYNLAB.Base.Controls.PanelWyn();
        this.sectionHeaderRecv = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.btnAddRecv = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnRemoveRecv = new WYNLAB.Base.Controls.ButtonWyn();
        this.grd3 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw3 = new WYNLAB.Base.Controls.GridViewWyn();
        this.colRecvSort = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colRecvEmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colRecvEmpNo = new DevExpress.XtraGrid.Columns.GridColumn();

        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchRouteNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelSplit)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelLeft)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelRight)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtRouteNm.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtRouteId.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelEdit)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelTree)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.treeEmp)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelArrows)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelLists)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelDetailLine)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panLineHead)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelDetailRecv)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panRecvHead)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd3)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw3)).BeginInit();
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
        this.panBase.Size = new System.Drawing.Size(1245, 570);
        this.panBase.TabIndex = 0;
        //
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
        this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        this.panHeader.Controls.Add(this.lblSearchAccId);
        this.panHeader.Controls.Add(this.cboSearchAccId);
        this.panHeader.Controls.Add(this.lblSearchRouteNm);
        this.panHeader.Controls.Add(this.txtSearchRouteNm);
        this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.panHeader.Location = new System.Drawing.Point(5, 0);
        this.panHeader.Name = "panHeader";
        this.panHeader.Size = new System.Drawing.Size(1235, 49);
        this.panHeader.TabIndex = 1;
        //
        // lblSearchAccId (조회조건 첫 번째 - 사업장 표준)
        //
        this.lblSearchAccId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchAccId.Appearance.Options.UseFont = true;
        this.lblSearchAccId.Location = new System.Drawing.Point(25, 18);
        this.lblSearchAccId.Name = "lblSearchAccId";
        this.lblSearchAccId.Size = new System.Drawing.Size(36, 15);
        this.lblSearchAccId.TabIndex = 0;
        this.lblSearchAccId.Text = "사업장";
        //
        // cboSearchAccId (사업장 LookUp, Required - 기본값은 화면 생성자에서 로그인 사업장)
        //
        this.cboSearchAccId.EditValue = "";
        this.cboSearchAccId.Location = new System.Drawing.Point(69, 15);
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
        // lblSearchRouteNm
        //
        this.lblSearchRouteNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblSearchRouteNm.Appearance.Options.UseFont = true;
        this.lblSearchRouteNm.Location = new System.Drawing.Point(224, 18);
        this.lblSearchRouteNm.Name = "lblSearchRouteNm";
        this.lblSearchRouteNm.Size = new System.Drawing.Size(60, 15);
        this.lblSearchRouteNm.TabIndex = 2;
        this.lblSearchRouteNm.Text = "결재경로명";
        //
        // txtSearchRouteNm
        //
        this.txtSearchRouteNm.Location = new System.Drawing.Point(292, 15);
        this.txtSearchRouteNm.Name = "txtSearchRouteNm";
        this.txtSearchRouteNm.Size = new System.Drawing.Size(200, 20);
        this.txtSearchRouteNm.TabIndex = 3;
        //
        // panelSplit
        //
        this.panelSplit.Controls.Add(this.panelRight);
        this.panelSplit.Controls.Add(this.splitterWyn1);
        this.panelSplit.Controls.Add(this.panelLeft);
        this.panelSplit.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelSplit.Location = new System.Drawing.Point(5, 49);
        this.panelSplit.Name = "panelSplit";
        this.panelSplit.Size = new System.Drawing.Size(1235, 516);
        this.panelSplit.TabIndex = 0;
        //
        // panelLeft
        //
        this.panelLeft.Controls.Add(this.grd1);
        this.panelLeft.Controls.Add(this.sectionHeaderMaster);
        this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
        this.panelLeft.Location = new System.Drawing.Point(0, 0);
        this.panelLeft.Name = "panelLeft";
        this.panelLeft.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
        this.panelLeft.Size = new System.Drawing.Size(270, 516);
        this.panelLeft.TabIndex = 0;
        //
        // sectionHeaderMaster
        //
        this.sectionHeaderMaster.Dock = System.Windows.Forms.DockStyle.Top;
        this.sectionHeaderMaster.Height = 28;
        this.sectionHeaderMaster.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Grid;
        this.sectionHeaderMaster.Location = new System.Drawing.Point(0, 8);
        this.sectionHeaderMaster.Name = "sectionHeaderMaster";
        this.sectionHeaderMaster.Text = "결재경로 리스트";
        //
        // grd1
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd1.Location = new System.Drawing.Point(0, 36);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.Size = new System.Drawing.Size(270, 480);
        this.grd1.TabIndex = 1;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { this.gvw1 });
        //
        // gvw1
        //
        this.colRouteId.Caption = "결재경로코드";
        this.colRouteId.FieldName = "route_id";
        this.colRouteId.Name = "colRouteId";
        this.colRouteId.OptionsColumn.AllowEdit = false;
        this.colRouteId.Visible = true;
        this.colRouteId.VisibleIndex = 0;
        this.colRouteId.Width = 90;
        this.colRouteNm.Caption = "결재경로명";
        this.colRouteNm.FieldName = "route_nm";
        this.colRouteNm.Name = "colRouteNm";
        this.colRouteNm.OptionsColumn.AllowEdit = false;
        this.colRouteNm.Visible = true;
        this.colRouteNm.VisibleIndex = 1;
        this.colRouteNm.Width = 160;
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colRouteId,
        this.colRouteNm});
        this.gvw1.GridControl = this.grd1;
        this.gvw1.Name = "gvw1";
        this.gvw1.OptionsView.ColumnAutoWidth = false;
        //
        // splitterWyn1
        //
        this.splitterWyn1.Dock = System.Windows.Forms.DockStyle.Left;
        this.splitterWyn1.Location = new System.Drawing.Point(270, 0);
        this.splitterWyn1.Name = "splitterWyn1";
        this.splitterWyn1.Size = new System.Drawing.Size(6, 516);
        this.splitterWyn1.TabIndex = 1;
        this.splitterWyn1.TabStop = false;
        //
        // panelRight
        //
        this.panelRight.Controls.Add(this.panelEdit);
        this.panelRight.Controls.Add(this.panData);
        this.panelRight.Controls.Add(this.sectionHeaderDetail);
        this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelRight.Location = new System.Drawing.Point(276, 0);
        this.panelRight.Name = "panelRight";
        this.panelRight.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
        this.panelRight.Size = new System.Drawing.Size(959, 516);
        this.panelRight.TabIndex = 2;
        //
        // sectionHeaderDetail
        //
        this.sectionHeaderDetail.Dock = System.Windows.Forms.DockStyle.Top;
        this.sectionHeaderDetail.Height = 28;
        this.sectionHeaderDetail.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Grid;
        this.sectionHeaderDetail.Location = new System.Drawing.Point(0, 8);
        this.sectionHeaderDetail.Name = "sectionHeaderDetail";
        this.sectionHeaderDetail.Text = "결재경로상세";
        //
        // panData
        //
        this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        this.panData.Controls.Add(this.lblRouteNm);
        this.panData.Controls.Add(this.txtRouteNm);
        this.panData.Controls.Add(this.lblRouteId);
        this.panData.Controls.Add(this.txtRouteId);
        this.panData.Dock = System.Windows.Forms.DockStyle.Top;
        this.panData.Location = new System.Drawing.Point(0, 36);
        this.panData.Name = "panData";
        this.panData.Size = new System.Drawing.Size(959, 50);
        this.panData.TabIndex = 1;
        //
        // lblRouteNm
        //
        this.lblRouteNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblRouteNm.Appearance.Options.UseFont = true;
        this.lblRouteNm.Location = new System.Drawing.Point(20, 18);
        this.lblRouteNm.Name = "lblRouteNm";
        this.lblRouteNm.Size = new System.Drawing.Size(60, 15);
        this.lblRouteNm.TabIndex = 0;
        this.lblRouteNm.Text = "결재경로명";
        //
        // txtRouteNm (Required)
        //
        this.txtRouteNm.Location = new System.Drawing.Point(90, 15);
        this.txtRouteNm.Name = "txtRouteNm";
        this.txtRouteNm.Required = true;
        this.txtRouteNm.Size = new System.Drawing.Size(240, 20);
        this.txtRouteNm.TabIndex = 1;
        //
        // lblRouteId
        //
        this.lblRouteId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.lblRouteId.Appearance.Options.UseFont = true;
        this.lblRouteId.Location = new System.Drawing.Point(360, 18);
        this.lblRouteId.Name = "lblRouteId";
        this.lblRouteId.Size = new System.Drawing.Size(72, 15);
        this.lblRouteId.TabIndex = 2;
        this.lblRouteId.Text = "결재경로코드";
        //
        // txtRouteId
        //
        this.txtRouteId.Location = new System.Drawing.Point(442, 15);
        this.txtRouteId.Name = "txtRouteId";
        this.txtRouteId.Properties.ReadOnly = true;
        this.txtRouteId.Size = new System.Drawing.Size(100, 20);
        this.txtRouteId.TabIndex = 3;
        //
        // panelEdit
        //
        this.panelEdit.Controls.Add(this.panelLists);
        this.panelEdit.Controls.Add(this.panelArrows);
        this.panelEdit.Controls.Add(this.panelTree);
        this.panelEdit.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelEdit.Location = new System.Drawing.Point(0, 86);
        this.panelEdit.Name = "panelEdit";
        this.panelEdit.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
        this.panelEdit.Size = new System.Drawing.Size(959, 430);
        this.panelEdit.TabIndex = 2;
        //
        // panelTree
        //
        this.panelTree.Controls.Add(this.treeEmp);
        this.panelTree.Controls.Add(this.sectionHeaderTree);
        this.panelTree.Dock = System.Windows.Forms.DockStyle.Left;
        this.panelTree.Location = new System.Drawing.Point(0, 8);
        this.panelTree.Name = "panelTree";
        this.panelTree.Size = new System.Drawing.Size(330, 422);
        this.panelTree.TabIndex = 0;
        //
        // sectionHeaderTree
        //
        this.sectionHeaderTree.Dock = System.Windows.Forms.DockStyle.Top;
        this.sectionHeaderTree.Height = 28;
        this.sectionHeaderTree.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
        this.sectionHeaderTree.Location = new System.Drawing.Point(0, 0);
        this.sectionHeaderTree.Name = "sectionHeaderTree";
        this.sectionHeaderTree.Text = "조직도";
        //
        // treeEmp
        //
        this.treeEmp.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
        this.colTreeNm,
        this.colTreeEmpNo,
        this.colTreeJobGrade});
        this.treeEmp.Dock = System.Windows.Forms.DockStyle.Fill;
        this.treeEmp.ImageIndexFieldName = "ImgIdx";
        this.treeEmp.KeyFieldName = "NodeKey";
        this.treeEmp.Location = new System.Drawing.Point(0, 28);
        this.treeEmp.Name = "treeEmp";
        this.treeEmp.OptionsBehavior.Editable = false;
        this.treeEmp.OptionsView.ShowColumns = false;
        this.treeEmp.ParentFieldName = "ParentKey";
        this.treeEmp.RowHeight = 26;
        this.treeEmp.Size = new System.Drawing.Size(330, 394);
        this.treeEmp.TabIndex = 1;
        //
        // colTreeNm
        //
        this.colTreeNm.Caption = "부서/사원";
        this.colTreeNm.FieldName = "Nm";
        this.colTreeNm.Name = "colTreeNm";
        this.colTreeNm.Visible = true;
        this.colTreeNm.VisibleIndex = 0;
        this.colTreeNm.Width = 169;
        //
        // colTreeEmpNo
        //
        this.colTreeEmpNo.Caption = "사번";
        this.colTreeEmpNo.FieldName = "EmpNo";
        this.colTreeEmpNo.Name = "colTreeEmpNo";
        this.colTreeEmpNo.Width = 95;
        //
        // colTreeJobGrade
        //
        this.colTreeJobGrade.Caption = "직위";
        this.colTreeJobGrade.FieldName = "JobGrade";
        this.colTreeJobGrade.Name = "colTreeJobGrade";
        this.colTreeJobGrade.Width = 85;
        //
        // panelArrows (조직도에서 고른 사원을 승인부/수신부로 보내는 화살표)
        //
        this.panelArrows.Controls.Add(this.btnToLine);
        this.panelArrows.Controls.Add(this.btnToRecv);
        this.panelArrows.Dock = System.Windows.Forms.DockStyle.Left;
        this.panelArrows.Location = new System.Drawing.Point(330, 8);
        this.panelArrows.Name = "panelArrows";
        this.panelArrows.Size = new System.Drawing.Size(46, 422);
        this.panelArrows.TabIndex = 1;
        //
        // btnToLine
        //
        this.btnToLine.Location = new System.Drawing.Point(6, 150);
        this.btnToLine.Name = "btnToLine";
        this.btnToLine.Size = new System.Drawing.Size(34, 34);
        this.btnToLine.TabIndex = 0;
        this.btnToLine.Text = "▲";
        this.btnToLine.ToolTip = "선택한 사원을 승인부(결재라인)에 추가합니다.";
        //
        // btnToRecv
        //
        this.btnToRecv.Location = new System.Drawing.Point(6, 190);
        this.btnToRecv.Name = "btnToRecv";
        this.btnToRecv.Size = new System.Drawing.Size(34, 34);
        this.btnToRecv.TabIndex = 1;
        this.btnToRecv.Text = "▼";
        this.btnToRecv.ToolTip = "선택한 사원을 수신부(수신라인)에 추가합니다.";
        //
        // panelLists
        //
        this.panelLists.Controls.Add(this.panelDetailRecv);
        this.panelLists.Controls.Add(this.splitterWyn3);
        this.panelLists.Controls.Add(this.panelDetailLine);
        this.panelLists.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelLists.Location = new System.Drawing.Point(376, 8);
        this.panelLists.Name = "panelLists";
        this.panelLists.Size = new System.Drawing.Size(583, 422);
        this.panelLists.TabIndex = 2;
        //
        // panelDetailLine
        //
        this.panelDetailLine.Controls.Add(this.grd2);
        this.panelDetailLine.Controls.Add(this.panLineHead);
        this.panelDetailLine.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelDetailLine.Height = 200;
        this.panelDetailLine.Location = new System.Drawing.Point(0, 0);
        this.panelDetailLine.Name = "panelDetailLine";
        this.panelDetailLine.Size = new System.Drawing.Size(583, 200);
        this.panelDetailLine.TabIndex = 1;
        //
        // panLineHead (제목 + 추가/삭제 버튼)
        //
        this.panLineHead.Controls.Add(this.sectionHeaderLine);
        this.panLineHead.Controls.Add(this.btnAddLine);
        this.panLineHead.Controls.Add(this.btnRemoveLine);
        this.panLineHead.Dock = System.Windows.Forms.DockStyle.Top;
        this.panLineHead.Height = 28;
        this.panLineHead.Location = new System.Drawing.Point(0, 0);
        this.panLineHead.Name = "panLineHead";
        this.panLineHead.Size = new System.Drawing.Size(583, 28);
        this.panLineHead.TabIndex = 0;
        //
        // sectionHeaderLine
        //
        this.sectionHeaderLine.Dock = System.Windows.Forms.DockStyle.Fill;
        this.sectionHeaderLine.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Grid;
        this.sectionHeaderLine.Location = new System.Drawing.Point(0, 0);
        this.sectionHeaderLine.Name = "sectionHeaderLine";
        this.sectionHeaderLine.Text = "승인부 결재경로 구성";
        //
        // btnAddLine
        //
        this.btnAddLine.Dock = System.Windows.Forms.DockStyle.Right;
        this.btnAddLine.Location = new System.Drawing.Point(457, 0);
        this.btnAddLine.Name = "btnAddLine";
        this.btnAddLine.Size = new System.Drawing.Size(64, 28);
        this.btnAddLine.TabIndex = 1;
        this.btnAddLine.Text = "추가";
        //
        // btnRemoveLine
        //
        this.btnRemoveLine.Dock = System.Windows.Forms.DockStyle.Right;
        this.btnRemoveLine.Location = new System.Drawing.Point(521, 0);
        this.btnRemoveLine.Name = "btnRemoveLine";
        this.btnRemoveLine.Size = new System.Drawing.Size(62, 28);
        this.btnRemoveLine.TabIndex = 2;
        this.btnRemoveLine.Text = "삭제";
        //
        // gvw2
        //
        this.colLineSort.Caption = "순번";
        this.colLineSort.FieldName = "Sort";
        this.colLineSort.Name = "colLineSort";
        this.colLineSort.Visible = true;
        this.colLineSort.VisibleIndex = 0;
        this.colLineSort.Width = 50;
        this.colLineEmpNm.Caption = "결재자";
        this.colLineEmpNm.FieldName = "EmpNm";
        this.colLineEmpNm.Name = "colLineEmpNm";
        this.colLineEmpNm.Visible = true;
        this.colLineEmpNm.VisibleIndex = 1;
        this.colLineEmpNm.Width = 120;
        this.colLineEmpNo.Caption = "사번";
        this.colLineEmpNo.FieldName = "EmpNo";
        this.colLineEmpNo.Name = "colLineEmpNo";
        this.colLineEmpNo.Visible = true;
        this.colLineEmpNo.VisibleIndex = 2;
        this.colLineEmpNo.Width = 100;
        this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colLineSort,
        this.colLineEmpNm,
        this.colLineEmpNo});
        this.gvw2.GridControl = this.grd2;
        this.gvw2.Name = "gvw2";
        this.gvw2.OptionsBehavior.Editable = false;
        this.gvw2.OptionsView.ColumnAutoWidth = false;
        //
        // grd2
        //
        this.grd2.AllowDrop = true;
        this.grd2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd2.Location = new System.Drawing.Point(0, 28);
        this.grd2.MainView = this.gvw2;
        this.grd2.Name = "grd2";
        this.grd2.Size = new System.Drawing.Size(583, 172);
        this.grd2.TabIndex = 1;
        this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { this.gvw2 });
        //
        // splitterWyn3
        //
        this.splitterWyn3.Dock = System.Windows.Forms.DockStyle.Top;
        this.splitterWyn3.Location = new System.Drawing.Point(0, 200);
        this.splitterWyn3.Name = "splitterWyn3";
        this.splitterWyn3.Size = new System.Drawing.Size(583, 6);
        this.splitterWyn3.TabIndex = 2;
        this.splitterWyn3.TabStop = false;
        //
        // panelDetailRecv
        //
        this.panelDetailRecv.Controls.Add(this.grd3);
        this.panelDetailRecv.Controls.Add(this.panRecvHead);
        this.panelDetailRecv.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelDetailRecv.Location = new System.Drawing.Point(0, 206);
        this.panelDetailRecv.Name = "panelDetailRecv";
        this.panelDetailRecv.Size = new System.Drawing.Size(583, 216);
        this.panelDetailRecv.TabIndex = 3;
        //
        // panRecvHead (제목 + 추가/삭제 버튼)
        //
        this.panRecvHead.Controls.Add(this.sectionHeaderRecv);
        this.panRecvHead.Controls.Add(this.btnAddRecv);
        this.panRecvHead.Controls.Add(this.btnRemoveRecv);
        this.panRecvHead.Dock = System.Windows.Forms.DockStyle.Top;
        this.panRecvHead.Height = 28;
        this.panRecvHead.Location = new System.Drawing.Point(0, 0);
        this.panRecvHead.Name = "panRecvHead";
        this.panRecvHead.Size = new System.Drawing.Size(583, 28);
        this.panRecvHead.TabIndex = 0;
        //
        // sectionHeaderRecv
        //
        this.sectionHeaderRecv.Dock = System.Windows.Forms.DockStyle.Fill;
        this.sectionHeaderRecv.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Grid;
        this.sectionHeaderRecv.Location = new System.Drawing.Point(0, 0);
        this.sectionHeaderRecv.Name = "sectionHeaderRecv";
        this.sectionHeaderRecv.Text = "수신부 결재경로 구성";
        //
        // btnAddRecv
        //
        this.btnAddRecv.Dock = System.Windows.Forms.DockStyle.Right;
        this.btnAddRecv.Location = new System.Drawing.Point(457, 0);
        this.btnAddRecv.Name = "btnAddRecv";
        this.btnAddRecv.Size = new System.Drawing.Size(64, 28);
        this.btnAddRecv.TabIndex = 1;
        this.btnAddRecv.Text = "추가";
        //
        // btnRemoveRecv
        //
        this.btnRemoveRecv.Dock = System.Windows.Forms.DockStyle.Right;
        this.btnRemoveRecv.Location = new System.Drawing.Point(521, 0);
        this.btnRemoveRecv.Name = "btnRemoveRecv";
        this.btnRemoveRecv.Size = new System.Drawing.Size(62, 28);
        this.btnRemoveRecv.TabIndex = 2;
        this.btnRemoveRecv.Text = "삭제";
        //
        // gvw3
        //
        this.colRecvSort.Caption = "순번";
        this.colRecvSort.FieldName = "Sort";
        this.colRecvSort.Name = "colRecvSort";
        this.colRecvSort.Visible = true;
        this.colRecvSort.VisibleIndex = 0;
        this.colRecvSort.Width = 50;
        this.colRecvEmpNm.Caption = "수신자";
        this.colRecvEmpNm.FieldName = "EmpNm";
        this.colRecvEmpNm.Name = "colRecvEmpNm";
        this.colRecvEmpNm.Visible = true;
        this.colRecvEmpNm.VisibleIndex = 1;
        this.colRecvEmpNm.Width = 120;
        this.colRecvEmpNo.Caption = "사번";
        this.colRecvEmpNo.FieldName = "EmpNo";
        this.colRecvEmpNo.Name = "colRecvEmpNo";
        this.colRecvEmpNo.Visible = true;
        this.colRecvEmpNo.VisibleIndex = 2;
        this.colRecvEmpNo.Width = 100;
        this.gvw3.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colRecvSort,
        this.colRecvEmpNm,
        this.colRecvEmpNo});
        this.gvw3.GridControl = this.grd3;
        this.gvw3.Name = "gvw3";
        this.gvw3.OptionsBehavior.Editable = false;
        this.gvw3.OptionsView.ColumnAutoWidth = false;
        //
        // grd3
        //
        this.grd3.AllowDrop = true;
        this.grd3.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd3.Location = new System.Drawing.Point(0, 28);
        this.grd3.MainView = this.gvw3;
        this.grd3.Name = "grd3";
        this.grd3.Size = new System.Drawing.Size(583, 188);
        this.grd3.TabIndex = 1;
        this.grd3.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { this.gvw3 });
        //
        // frmApprRoute
        //
        this.ClientSize = new System.Drawing.Size(1245, 570);
        this.Controls.Add(this.panBase);
        this.Name = "frmApprRoute";

        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cboSearchAccId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchRouteNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelSplit)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelLeft)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelRight)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtRouteNm.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtRouteId.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelEdit)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelTree)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.treeEmp)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelArrows)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelLists)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelDetailLine)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panLineHead)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panelDetailRecv)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panRecvHead)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw3)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchAccId;
    private LookUpEditWyn cboSearchAccId;
    private DevExpress.XtraEditors.LabelControl lblSearchRouteNm;
    private TextEditWyn txtSearchRouteNm;
    private PanelWyn panelSplit;
    private PanelWyn panelLeft;
    private SectionHeaderWyn sectionHeaderMaster;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colRouteId;
    private DevExpress.XtraGrid.Columns.GridColumn colRouteNm;
    private SplitterWyn splitterWyn1;
    private PanelWyn panelRight;
    private SectionHeaderWyn sectionHeaderDetail;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblRouteNm;
    private TextEditWyn txtRouteNm;
    private DevExpress.XtraEditors.LabelControl lblRouteId;
    private TextEditWyn txtRouteId;
    private PanelWyn panelEdit;
    private PanelWyn panelTree;
    private SectionHeaderWyn sectionHeaderTree;
    private TreeListWyn treeEmp;
    private DevExpress.XtraTreeList.Columns.TreeListColumn colTreeNm;
    private DevExpress.XtraTreeList.Columns.TreeListColumn colTreeEmpNo;
    private DevExpress.XtraTreeList.Columns.TreeListColumn colTreeJobGrade;
    private PanelWyn panelArrows;
    private ButtonWyn btnToLine;
    private ButtonWyn btnToRecv;
    private PanelWyn panelLists;
    private PanelWyn panelDetailLine;
    private PanelWyn panLineHead;
    private SectionHeaderWyn sectionHeaderLine;
    private ButtonWyn btnAddLine;
    private ButtonWyn btnRemoveLine;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private DevExpress.XtraGrid.Columns.GridColumn colLineSort;
    private DevExpress.XtraGrid.Columns.GridColumn colLineEmpNm;
    private DevExpress.XtraGrid.Columns.GridColumn colLineEmpNo;
    private SplitterWyn splitterWyn3;
    private PanelWyn panelDetailRecv;
    private PanelWyn panRecvHead;
    private SectionHeaderWyn sectionHeaderRecv;
    private ButtonWyn btnAddRecv;
    private ButtonWyn btnRemoveRecv;
    private GridControlWyn grd3;
    private GridViewWyn gvw3;
    private DevExpress.XtraGrid.Columns.GridColumn colRecvSort;
    private DevExpress.XtraGrid.Columns.GridColumn colRecvEmpNm;
    private DevExpress.XtraGrid.Columns.GridColumn colRecvEmpNo;
}
