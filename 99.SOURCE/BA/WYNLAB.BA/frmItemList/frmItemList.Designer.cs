// AI Builder가 싱글그리드 템플릿을 복제해서 자동 생성 - 2026-09-16.
// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

public partial class frmItemList
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmItemList));
            this.panBody = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.col1AssetType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpColumnEdit2 = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.col1ItemId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.spinEditcol1 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.col1ItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1ItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1ItemSpec = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1UnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpcol1UnitCd = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.col1WhId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1WhNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1LocId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1LocNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1SafeQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1DeptId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1DeptNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1EmpId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1EmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1StockYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.col1StatCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpColumnEdit1 = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.col1Remark = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpcol1StatCd = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.paTitle1 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.cboAssetType_Q = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.cboStatCd_Q = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblSearchItemNo = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtItemNo_Q = new WYNLAB.Base.Controls.TextEditWyn();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lblSearchItemNm = new DevExpress.XtraEditors.LabelControl();
            this.txtItemSpec_Q = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtItemNm_Q = new WYNLAB.Base.Controls.TextEditWyn();
            this.paTitleH = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBody)).BeginInit();
            this.panBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpColumnEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditcol1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcol1UnitCd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpColumnEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcol1StatCd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle1)).BeginInit();
            this.paTitle1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboAssetType_Q.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboStatCd_Q.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemNo_Q.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemSpec_Q.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemNm_Q.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).BeginInit();
            this.paTitleH.SuspendLayout();
            this.SuspendLayout();
            // 
            // panBody
            // 
            this.panBody.Controls.Add(this.grd1);
            this.panBody.Controls.Add(this.paTitle1);
            this.panBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBody.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panBody.Location = new System.Drawing.Point(5, 90);
            this.panBody.Name = "panBody";
            this.panBody.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panBody.Size = new System.Drawing.Size(2208, 505);
            this.panBody.TabIndex = 1;
            // 
            // grd1
            // 
            this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd1.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grd1.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grd1.Location = new System.Drawing.Point(0, 35);
            this.grd1.MainView = this.gvw1;
            this.grd1.Name = "grd1";
            this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.chkEdit1,
            this.spinEditcol1,
            this.lookUpcol1UnitCd,
            this.lookUpcol1StatCd,
            this.lookUpColumnEdit1,
            this.lookUpColumnEdit2});
            this.grd1.Size = new System.Drawing.Size(2208, 470);
            this.grd1.TabIndex = 1;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            // 
            // gvw1
            // 
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col1AssetType,
            this.col1ItemId,
            this.col1ItemNo,
            this.col1ItemNm,
            this.col1ItemSpec,
            this.col1UnitCd,
            this.col1WhId,
            this.col1WhNm,
            this.col1LocId,
            this.col1LocNm,
            this.col1SafeQty,
            this.col1DeptId,
            this.col1DeptNm,
            this.col1EmpId,
            this.col1EmpNm,
            this.col1StockYn,
            this.col1StatCd,
            this.col1Remark});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsBehavior.Editable = false;
            this.gvw1.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            // 
            // col1AssetType
            // 
            this.col1AssetType.Caption = "자산구분";
            this.col1AssetType.ColumnEdit = this.lookUpColumnEdit2;
            this.col1AssetType.FieldName = "asset_type";
            this.col1AssetType.Name = "col1AssetType";
            this.col1AssetType.OptionsColumn.AllowEdit = false;
            this.col1AssetType.Visible = true;
            this.col1AssetType.VisibleIndex = 4;
            this.col1AssetType.Width = 88;
            // 
            // lookUpColumnEdit2
            // 
            this.lookUpColumnEdit2.AutoHeight = false;
            this.lookUpColumnEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpColumnEdit2.LookupKey = "L_CM0002";
            this.lookUpColumnEdit2.Name = "lookUpColumnEdit2";
            this.lookUpColumnEdit2.NullText = "";
            this.lookUpColumnEdit2.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // col1ItemId
            // 
            this.col1ItemId.Caption = "품목ID";
            this.col1ItemId.ColumnEdit = this.spinEditcol1;
            this.col1ItemId.FieldName = "item_id";
            this.col1ItemId.Name = "col1ItemId";
            this.col1ItemId.OptionsColumn.AllowEdit = false;
            this.col1ItemId.Visible = true;
            this.col1ItemId.VisibleIndex = 0;
            this.col1ItemId.Width = 80;
            // 
            // spinEditcol1
            // 
            this.spinEditcol1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEditcol1.Name = "spinEditcol1";
            // 
            // col1ItemNo
            // 
            this.col1ItemNo.Caption = "품번";
            this.col1ItemNo.FieldName = "item_no";
            this.col1ItemNo.Name = "col1ItemNo";
            this.col1ItemNo.OptionsColumn.AllowEdit = false;
            this.col1ItemNo.Visible = true;
            this.col1ItemNo.VisibleIndex = 1;
            this.col1ItemNo.Width = 152;
            // 
            // col1ItemNm
            // 
            this.col1ItemNm.Caption = "품명";
            this.col1ItemNm.FieldName = "item_nm";
            this.col1ItemNm.Name = "col1ItemNm";
            this.col1ItemNm.OptionsColumn.AllowEdit = false;
            this.col1ItemNm.Visible = true;
            this.col1ItemNm.VisibleIndex = 2;
            this.col1ItemNm.Width = 248;
            // 
            // col1ItemSpec
            // 
            this.col1ItemSpec.Caption = "규격";
            this.col1ItemSpec.FieldName = "item_spec";
            this.col1ItemSpec.Name = "col1ItemSpec";
            this.col1ItemSpec.OptionsColumn.AllowEdit = false;
            this.col1ItemSpec.Visible = true;
            this.col1ItemSpec.VisibleIndex = 3;
            this.col1ItemSpec.Width = 114;
            // 
            // col1UnitCd
            // 
            this.col1UnitCd.Caption = "단위";
            this.col1UnitCd.ColumnEdit = this.lookUpcol1UnitCd;
            this.col1UnitCd.FieldName = "unit_cd";
            this.col1UnitCd.Name = "col1UnitCd";
            this.col1UnitCd.OptionsColumn.AllowEdit = false;
            this.col1UnitCd.Visible = true;
            this.col1UnitCd.VisibleIndex = 5;
            this.col1UnitCd.Width = 41;
            // 
            // lookUpcol1UnitCd
            // 
            this.lookUpcol1UnitCd.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpcol1UnitCd.LookupKey = "L_CM0001";
            this.lookUpcol1UnitCd.Name = "lookUpcol1UnitCd";
            this.lookUpcol1UnitCd.NullText = "";
            this.lookUpcol1UnitCd.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // col1WhId
            // 
            this.col1WhId.Caption = "창고ID";
            this.col1WhId.FieldName = "wh_id";
            this.col1WhId.Name = "col1WhId";
            this.col1WhId.OptionsColumn.AllowEdit = false;
            this.col1WhId.Width = 52;
            // 
            // col1WhNm
            // 
            this.col1WhNm.Caption = "창고명";
            this.col1WhNm.FieldName = "wh_nm";
            this.col1WhNm.Name = "col1WhNm";
            this.col1WhNm.OptionsColumn.AllowEdit = false;
            this.col1WhNm.Visible = true;
            this.col1WhNm.VisibleIndex = 6;
            this.col1WhNm.Width = 108;
            // 
            // col1LocId
            // 
            this.col1LocId.Caption = "Location ID";
            this.col1LocId.FieldName = "loc_id";
            this.col1LocId.Name = "col1LocId";
            this.col1LocId.OptionsColumn.AllowEdit = false;
            this.col1LocId.Width = 98;
            // 
            // col1LocNm
            // 
            this.col1LocNm.Caption = "Location";
            this.col1LocNm.FieldName = "loc_nm";
            this.col1LocNm.Name = "col1LocNm";
            this.col1LocNm.OptionsColumn.AllowEdit = false;
            this.col1LocNm.Visible = true;
            this.col1LocNm.VisibleIndex = 7;
            this.col1LocNm.Width = 111;
            // 
            // col1SafeQty
            // 
            this.col1SafeQty.Caption = "안전재고수량";
            this.col1SafeQty.ColumnEdit = this.spinEditcol1;
            this.col1SafeQty.FieldName = "safe_qty";
            this.col1SafeQty.Name = "col1SafeQty";
            this.col1SafeQty.OptionsColumn.AllowEdit = false;
            this.col1SafeQty.Visible = true;
            this.col1SafeQty.VisibleIndex = 8;
            this.col1SafeQty.Width = 93;
            // 
            // col1DeptId
            // 
            this.col1DeptId.Caption = "부서ID";
            this.col1DeptId.FieldName = "DEPT_ID";
            this.col1DeptId.Name = "col1DeptId";
            this.col1DeptId.OptionsColumn.AllowEdit = false;
            this.col1DeptId.Width = 66;
            // 
            // col1DeptNm
            // 
            this.col1DeptNm.Caption = "부서";
            this.col1DeptNm.FieldName = "dept_nm";
            this.col1DeptNm.Name = "col1DeptNm";
            this.col1DeptNm.OptionsColumn.AllowEdit = false;
            this.col1DeptNm.Visible = true;
            this.col1DeptNm.VisibleIndex = 9;
            this.col1DeptNm.Width = 111;
            // 
            // col1EmpId
            // 
            this.col1EmpId.Caption = "담당자ID";
            this.col1EmpId.FieldName = "EMP_ID";
            this.col1EmpId.Name = "col1EmpId";
            this.col1EmpId.OptionsColumn.AllowEdit = false;
            this.col1EmpId.Visible = true;
            this.col1EmpId.VisibleIndex = 10;
            this.col1EmpId.Width = 76;
            // 
            // col1EmpNm
            // 
            this.col1EmpNm.Caption = "담당자";
            this.col1EmpNm.FieldName = "emp_nm";
            this.col1EmpNm.Name = "col1EmpNm";
            this.col1EmpNm.OptionsColumn.AllowEdit = false;
            this.col1EmpNm.Visible = true;
            this.col1EmpNm.VisibleIndex = 11;
            this.col1EmpNm.Width = 65;
            // 
            // col1StockYn
            // 
            this.col1StockYn.Caption = "재고관리대상";
            this.col1StockYn.ColumnEdit = this.chkEdit1;
            this.col1StockYn.FieldName = "stock_yn";
            this.col1StockYn.Name = "col1StockYn";
            this.col1StockYn.OptionsColumn.AllowEdit = false;
            this.col1StockYn.Visible = true;
            this.col1StockYn.VisibleIndex = 12;
            this.col1StockYn.Width = 95;
            // 
            // chkEdit1
            // 
            this.chkEdit1.Name = "chkEdit1";
            // 
            // col1StatCd
            // 
            this.col1StatCd.Caption = "품목상태";
            this.col1StatCd.ColumnEdit = this.lookUpColumnEdit1;
            this.col1StatCd.FieldName = "stat_cd";
            this.col1StatCd.Name = "col1StatCd";
            this.col1StatCd.OptionsColumn.AllowEdit = false;
            this.col1StatCd.Visible = true;
            this.col1StatCd.VisibleIndex = 13;
            this.col1StatCd.Width = 96;
            // 
            // lookUpColumnEdit1
            // 
            this.lookUpColumnEdit1.AutoHeight = false;
            this.lookUpColumnEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpColumnEdit1.LookupKey = "L_BA0002";
            this.lookUpColumnEdit1.Name = "lookUpColumnEdit1";
            this.lookUpColumnEdit1.NullText = "";
            this.lookUpColumnEdit1.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // col1Remark
            // 
            this.col1Remark.Caption = "비고";
            this.col1Remark.FieldName = "remark";
            this.col1Remark.Name = "col1Remark";
            this.col1Remark.OptionsColumn.AllowEdit = false;
            this.col1Remark.Visible = true;
            this.col1Remark.VisibleIndex = 14;
            this.col1Remark.Width = 370;
            // 
            // lookUpcol1StatCd
            // 
            this.lookUpcol1StatCd.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpcol1StatCd.LookupKey = "L_BA0002";
            this.lookUpcol1StatCd.Name = "lookUpcol1StatCd";
            this.lookUpcol1StatCd.NullText = "";
            this.lookUpcol1StatCd.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // paTitle1
            // 
            this.paTitle1.Controls.Add(this.sectionHeaderWyn4);
            this.paTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitle1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitle1.Location = new System.Drawing.Point(0, 8);
            this.paTitle1.Name = "paTitle1";
            this.paTitle1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.paTitle1.Size = new System.Drawing.Size(2208, 27);
            this.paTitle1.TabIndex = 12;
            // 
            // sectionHeaderWyn4
            // 
            this.sectionHeaderWyn4.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn4.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn4.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn4.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn4.Name = "sectionHeaderWyn4";
            this.sectionHeaderWyn4.Size = new System.Drawing.Size(2203, 25);
            this.sectionHeaderWyn4.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn4.SvgIcon")));
            this.sectionHeaderWyn4.TabIndex = 8;
            this.sectionHeaderWyn4.Text = "LIST";
            // 
            // panHeader
            // 
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.cboAssetType_Q);
            this.panHeader.Controls.Add(this.cboStatCd_Q);
            this.panHeader.Controls.Add(this.lblSearchItemNo);
            this.panHeader.Controls.Add(this.labelControl3);
            this.panHeader.Controls.Add(this.txtItemNo_Q);
            this.panHeader.Controls.Add(this.labelControl2);
            this.panHeader.Controls.Add(this.labelControl1);
            this.panHeader.Controls.Add(this.lblSearchItemNm);
            this.panHeader.Controls.Add(this.txtItemSpec_Q);
            this.panHeader.Controls.Add(this.txtItemNm_Q);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(5, 30);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(2208, 60);
            this.panHeader.TabIndex = 0;
            // 
            // cboAssetType_Q
            // 
            this.cboAssetType_Q.EditValue = "";
            this.cboAssetType_Q.Location = new System.Drawing.Point(845, 21);
            this.cboAssetType_Q.LookupKey = "L_CM0002";
            this.cboAssetType_Q.Name = "cboAssetType_Q";
            this.cboAssetType_Q.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboAssetType_Q.Properties.NullText = "";
            this.cboAssetType_Q.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cboAssetType_Q.Size = new System.Drawing.Size(133, 20);
            this.cboAssetType_Q.TabIndex = 4;
            // 
            // cboStatCd_Q
            // 
            this.cboStatCd_Q.EditValue = "";
            this.cboStatCd_Q.Location = new System.Drawing.Point(642, 20);
            this.cboStatCd_Q.Name = "cboStatCd_Q";
            this.cboStatCd_Q.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboStatCd_Q.Properties.NullText = "";
            this.cboStatCd_Q.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cboStatCd_Q.Size = new System.Drawing.Size(133, 20);
            this.cboStatCd_Q.TabIndex = 4;
            // 
            // lblSearchItemNo
            // 
            this.lblSearchItemNo.Location = new System.Drawing.Point(17, 24);
            this.lblSearchItemNo.Name = "lblSearchItemNo";
            this.lblSearchItemNo.Size = new System.Drawing.Size(20, 14);
            this.lblSearchItemNo.TabIndex = 0;
            this.lblSearchItemNo.Text = "품번";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(799, 24);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(40, 14);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "자산구분";
            // 
            // txtItemNo_Q
            // 
            this.txtItemNo_Q.Location = new System.Drawing.Point(39, 21);
            this.txtItemNo_Q.Name = "txtItemNo_Q";
            this.txtItemNo_Q.Size = new System.Drawing.Size(150, 20);
            this.txtItemNo_Q.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(596, 23);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(40, 14);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "품목상태";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(399, 24);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(20, 14);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "규격";
            // 
            // lblSearchItemNm
            // 
            this.lblSearchItemNm.Location = new System.Drawing.Point(206, 24);
            this.lblSearchItemNm.Name = "lblSearchItemNm";
            this.lblSearchItemNm.Size = new System.Drawing.Size(20, 14);
            this.lblSearchItemNm.TabIndex = 2;
            this.lblSearchItemNm.Text = "품명";
            // 
            // txtItemSpec_Q
            // 
            this.txtItemSpec_Q.Location = new System.Drawing.Point(423, 21);
            this.txtItemSpec_Q.Name = "txtItemSpec_Q";
            this.txtItemSpec_Q.Size = new System.Drawing.Size(150, 20);
            this.txtItemSpec_Q.TabIndex = 3;
            // 
            // txtItemNm_Q
            // 
            this.txtItemNm_Q.Location = new System.Drawing.Point(230, 21);
            this.txtItemNm_Q.Name = "txtItemNm_Q";
            this.txtItemNm_Q.Size = new System.Drawing.Size(150, 20);
            this.txtItemNm_Q.TabIndex = 3;
            // 
            // paTitleH
            // 
            this.paTitleH.Controls.Add(this.sectionHeaderWyn1);
            this.paTitleH.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitleH.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitleH.Location = new System.Drawing.Point(5, 5);
            this.paTitleH.Name = "paTitleH";
            this.paTitleH.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.paTitleH.Size = new System.Drawing.Size(2208, 25);
            this.paTitleH.TabIndex = 6;
            // 
            // sectionHeaderWyn1
            // 
            this.sectionHeaderWyn1.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn1.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(2203, 25);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 11;
            this.sectionHeaderWyn1.Text = "품목현황 [frmItemList]";
            // 
            // frmItemList
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(2218, 600);
            this.Controls.Add(this.panBody);
            this.Controls.Add(this.panHeader);
            this.Controls.Add(this.paTitleH);
            this.Name = "frmItemList";
            this.Padding = new System.Windows.Forms.Padding(5);
            ((System.ComponentModel.ISupportInitialize)(this.panBody)).EndInit();
            this.panBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpColumnEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditcol1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcol1UnitCd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpColumnEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcol1StatCd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle1)).EndInit();
            this.paTitle1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboAssetType_Q.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboStatCd_Q.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemNo_Q.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemSpec_Q.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemNm_Q.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).EndInit();
            this.paTitleH.ResumeLayout(false);
            this.ResumeLayout(false);

    }
    private PanelWyn panHeader;
    private PanelWyn panBody;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private PanelWyn paTitle1;
    private SectionHeaderWyn sectionHeaderWyn4;
    private PanelWyn paTitleH;
    private DevExpress.XtraEditors.LabelControl lblSearchItemNo;
    private TextEditWyn txtItemNo_Q;
    private DevExpress.XtraEditors.LabelControl lblSearchItemNm;
    private TextEditWyn txtItemNm_Q;
    private DevExpress.XtraGrid.Columns.GridColumn col1ItemId;
    private DevExpress.XtraGrid.Columns.GridColumn col1ItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn col1ItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn col1ItemSpec;
    private DevExpress.XtraGrid.Columns.GridColumn col1UnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn col1WhId;
    private DevExpress.XtraGrid.Columns.GridColumn col1WhNm;
    private DevExpress.XtraGrid.Columns.GridColumn col1LocId;
    private DevExpress.XtraGrid.Columns.GridColumn col1LocNm;
    private DevExpress.XtraGrid.Columns.GridColumn col1SafeQty;
    private DevExpress.XtraGrid.Columns.GridColumn col1DeptId;
    private DevExpress.XtraGrid.Columns.GridColumn col1DeptNm;
    private DevExpress.XtraGrid.Columns.GridColumn col1EmpId;
    private DevExpress.XtraGrid.Columns.GridColumn col1EmpNm;
    private DevExpress.XtraGrid.Columns.GridColumn col1StockYn;
    private DevExpress.XtraGrid.Columns.GridColumn col1StatCd;
    private DevExpress.XtraGrid.Columns.GridColumn col1Remark;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkEdit1;
    private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spinEditcol1;
    private LookUpColumnEdit lookUpcol1UnitCd;
    private LookUpColumnEdit lookUpcol1StatCd;
    private DevExpress.XtraEditors.LabelControl labelControl1;
    private TextEditWyn txtItemSpec_Q;
    private SectionHeaderWyn sectionHeaderWyn1;
    private LookUpColumnEdit lookUpColumnEdit1;
    private LookUpEditWyn cboStatCd_Q;
    private DevExpress.XtraEditors.LabelControl labelControl2;
    private DevExpress.XtraGrid.Columns.GridColumn col1AssetType;
    private LookUpColumnEdit lookUpColumnEdit2;
    private LookUpEditWyn cboAssetType_Q;
    private DevExpress.XtraEditors.LabelControl labelControl3;
}
