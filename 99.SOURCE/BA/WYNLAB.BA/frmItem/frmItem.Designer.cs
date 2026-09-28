// AI Builder媛 留덉뒪??????렇由щ뱶 ?쒗뵆由우쓣 蹂듭젣?댁꽌 ?먮룞 ?앹꽦 - 2026-09-08.
// ?붿옄???쒕ぉ?곸뿭/?щ갚/?됱긽)??諛붽씀?ㅻ㈃ ???뚯씪???꾨땲???먮낯 ?쒗뵆由?99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)??怨좎튂?몄슂.
#nullable disable
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

public partial class frmItem
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmItem));
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions2 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject7 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject8 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions3 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject9 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject10 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject11 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject12 = new DevExpress.Utils.SerializableAppearanceObject();
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn8 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colMAccCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpcolMAccCd = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colMItemId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.spinEditcolM = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colMItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMItemSpec = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpcolMUnitCd = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colMPoUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpcolMPoUnitCd = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colMWhId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMLocId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMSafeQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMDeptId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMDeptNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMEmpId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMEmpNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMEmpNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMCustId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMCustNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMAssetType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpcolMAssetType = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colMOutType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMPoQcYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkEditcolM = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colMProdQcYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMLotYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMStockYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMStatCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpcolMStatCd = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colMGrp1Id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMGrp2Id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMGrp3Id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMGrp4Id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMRemark = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colD1FrUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpcolD1FrUnitCd = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colD1FrQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.spinEditcolD1 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colD1ToUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpcolD1ToUnitCd = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.colD1ToQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD1Remark = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnDeletRow2 = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnAddRow2 = new WYNLAB.Base.Controls.ButtonWyn();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
            this.grpItemProdQc = new WYNLAB.Base.Controls.GroupBoxWyn();
            this.lblDetailPoQcYn = new DevExpress.XtraEditors.LabelControl();
            this.chkDetailPoQcYn = new WYNLAB.Base.Controls.CheckBoxWyn();
            this.lblDetailProdQcYn = new DevExpress.XtraEditors.LabelControl();
            this.chkDetailProdQcYn = new WYNLAB.Base.Controls.CheckBoxWyn();
            this.lblDetailLotYn = new DevExpress.XtraEditors.LabelControl();
            this.chkDetailLotYn = new WYNLAB.Base.Controls.CheckBoxWyn();
            this.lblDetailStockYn = new DevExpress.XtraEditors.LabelControl();
            this.chkDetailStockYn = new WYNLAB.Base.Controls.CheckBoxWyn();
            this.panelWyn9 = new WYNLAB.Base.Controls.PanelWyn();
            this.txtDetailEmpId = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailEmpId = new DevExpress.XtraEditors.LabelControl();
            this.lblDetailDeptId = new DevExpress.XtraEditors.LabelControl();
            this.lblDetailItemId = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailOutType = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailOutType = new DevExpress.XtraEditors.LabelControl();
            this.grpItemBasic = new WYNLAB.Base.Controls.GroupBoxWyn();
            this.txtDetailItemId = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailAssetType = new DevExpress.XtraEditors.LabelControl();
            this.lblDetailAccCd = new DevExpress.XtraEditors.LabelControl();
            this.cboDetailAssetType = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.cboDetailAccCd = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblDetailStatCd = new DevExpress.XtraEditors.LabelControl();
            this.cboDetailStatCd = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblDetailItemNo = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailItemNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailItemNm = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailItemNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailItemSpec = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailItemSpec = new WYNLAB.Base.Controls.TextEditWyn();
            this.grpItemStock = new WYNLAB.Base.Controls.GroupBoxWyn();
            this.lblDetailWhId = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailWhId = new WYNLAB.Base.Controls.TextEditWyn();
            this.popDetailWhNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
            this.lblDetailLocId = new DevExpress.XtraEditors.LabelControl();
            this.txtDetailLocId = new WYNLAB.Base.Controls.TextEditWyn();
            this.popDetailLocNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
            this.lblDetailSafeQty = new DevExpress.XtraEditors.LabelControl();
            this.numDetailSafeQty = new WYNLAB.Base.Controls.SpinEditWyn();
            this.cboDetailUnitCd = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblDetailUnitCd = new DevExpress.XtraEditors.LabelControl();
            this.cboDetailPoUnitCd = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblDetailPoUnitCd = new DevExpress.XtraEditors.LabelControl();
            this.grpItemClassify = new WYNLAB.Base.Controls.GroupBoxWyn();
            this.lblDetailGrp1Id = new DevExpress.XtraEditors.LabelControl();
            this.cboDetailGrp1Id = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblDetailGrp2Id = new DevExpress.XtraEditors.LabelControl();
            this.cboDetailGrp2Id = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblDetailGrp3Id = new DevExpress.XtraEditors.LabelControl();
            this.cboDetailGrp3Id = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblDetailGrp4Id = new DevExpress.XtraEditors.LabelControl();
            this.cboDetailGrp4Id = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.groupBoxWyn1 = new WYNLAB.Base.Controls.GroupBoxWyn();
            this.txtDetailRemark = new WYNLAB.Base.Controls.MemoEditWyn();
            this.grpItemPersonnel = new WYNLAB.Base.Controls.GroupBoxWyn();
            this.txtDetailEmpNo = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailEmpNm = new DevExpress.XtraEditors.LabelControl();
            this.popDetailEmpNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
            this.txtDetailDeptId = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailDeptNm = new DevExpress.XtraEditors.LabelControl();
            this.popDetailDeptNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
            this.txtDetailCustId = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDetailCustNm = new DevExpress.XtraEditors.LabelControl();
            this.popDetailCustNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
            this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtItemNo_Q = new WYNLAB.Base.Controls.TextEditWyn();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtItemId = new WYNLAB.Base.Controls.TextEditWyn();
            this.cboDetailAccCd_Q = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
            this.panBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
            this.panelWyn3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).BeginInit();
            this.panelWyn8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcolMAccCd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditcolM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcolMUnitCd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcolMPoUnitCd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcolMAssetType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEditcolM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcolMStatCd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).BeginInit();
            this.panelWyn2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).BeginInit();
            this.panelWyn4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcolD1FrUnitCd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditcolD1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcolD1ToUnitCd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).BeginInit();
            this.panelWyn7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
            this.panData.SuspendLayout();
            this.grpItemProdQc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkDetailPoQcYn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDetailProdQcYn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDetailLotYn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDetailStockYn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn9)).BeginInit();
            this.panelWyn9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailEmpId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailOutType.Properties)).BeginInit();
            this.grpItemBasic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailItemId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailAssetType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailAccCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailStatCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailItemNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailItemNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailItemSpec.Properties)).BeginInit();
            this.grpItemStock.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailWhId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popDetailWhNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailLocId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popDetailLocNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDetailSafeQty.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailUnitCd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailPoUnitCd.Properties)).BeginInit();
            this.grpItemClassify.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailGrp1Id.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailGrp2Id.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailGrp3Id.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailGrp4Id.Properties)).BeginInit();
            this.groupBoxWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailRemark.Properties)).BeginInit();
            this.grpItemPersonnel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailEmpNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popDetailEmpNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailDeptId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popDetailDeptNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailCustId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popDetailCustNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).BeginInit();
            this.panelWyn6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemNo_Q.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailAccCd_Q.Properties)).BeginInit();
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
            this.panBase.Size = new System.Drawing.Size(2126, 957);
            this.panBase.TabIndex = 6;
            // 
            // panelWyn3
            // 
            this.panelWyn3.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn3.Appearance.Options.UseBackColor = true;
            this.panelWyn3.Controls.Add(this.panelWyn8);
            this.panelWyn3.Controls.Add(this.splitterWyn1);
            this.panelWyn3.Controls.Add(this.panelWyn4);
            this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn3.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn3.Location = new System.Drawing.Point(5, 82);
            this.panelWyn3.Name = "panelWyn3";
            this.panelWyn3.Size = new System.Drawing.Size(2116, 870);
            this.panelWyn3.TabIndex = 7;
            // 
            // panelWyn8
            // 
            this.panelWyn8.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn8.Appearance.Options.UseBackColor = true;
            this.panelWyn8.Controls.Add(this.grd1);
            this.panelWyn8.Controls.Add(this.panelWyn2);
            this.panelWyn8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn8.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn8.Location = new System.Drawing.Point(0, 0);
            this.panelWyn8.Name = "panelWyn8";
            this.panelWyn8.Size = new System.Drawing.Size(1350, 870);
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
            this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.chkEditcolM,
            this.spinEditcolM,
            this.lookUpcolMAccCd,
            this.lookUpcolMUnitCd,
            this.lookUpcolMPoUnitCd,
            this.lookUpcolMAssetType,
            this.lookUpcolMStatCd});
            this.grd1.Size = new System.Drawing.Size(1350, 843);
            this.grd1.TabIndex = 10;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            // 
            // gvw1
            // 
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colMAccCd,
            this.colMItemId,
            this.colMItemNo,
            this.colMItemNm,
            this.colMItemSpec,
            this.colMUnitCd,
            this.colMPoUnitCd,
            this.colMWhId,
            this.colMLocId,
            this.colMSafeQty,
            this.colMDeptId,
            this.colMDeptNm,
            this.colMEmpId,
            this.colMEmpNo,
            this.colMEmpNm,
            this.colMCustId,
            this.colMCustNm,
            this.colMAssetType,
            this.colMOutType,
            this.colMPoQcYn,
            this.colMProdQcYn,
            this.colMLotYn,
            this.colMStockYn,
            this.colMStatCd,
            this.colMGrp1Id,
            this.colMGrp2Id,
            this.colMGrp3Id,
            this.colMGrp4Id,
            this.colMRemark});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.HighlightFocusedRow = true;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsBehavior.Editable = false;
            this.gvw1.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw1.OptionsSelection.InvertSelection = true;
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            // 
            // colMAccCd
            // 
            this.colMAccCd.Caption = "사업장";
            this.colMAccCd.ColumnEdit = this.lookUpcolMAccCd;
            this.colMAccCd.FieldName = "acc_id";
            this.colMAccCd.Name = "colMAccCd";
            this.colMAccCd.Width = 100;
            // 
            // lookUpcolMAccCd
            // 
            this.lookUpcolMAccCd.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpcolMAccCd.LookupKey = "L_ACC";
            this.lookUpcolMAccCd.Name = "lookUpcolMAccCd";
            this.lookUpcolMAccCd.NullText = "";
            this.lookUpcolMAccCd.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // colMItemId
            // 
            this.colMItemId.Caption = "품목ID";
            this.colMItemId.ColumnEdit = this.spinEditcolM;
            this.colMItemId.FieldName = "item_id";
            this.colMItemId.Name = "colMItemId";
            this.colMItemId.Width = 100;
            // 
            // spinEditcolM
            // 
            this.spinEditcolM.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEditcolM.Name = "spinEditcolM";
            // 
            // colMItemNo
            // 
            this.colMItemNo.Caption = "품번";
            this.colMItemNo.FieldName = "item_no";
            this.colMItemNo.Name = "colMItemNo";
            this.colMItemNo.Visible = true;
            this.colMItemNo.VisibleIndex = 1;
            this.colMItemNo.Width = 115;
            // 
            // colMItemNm
            // 
            this.colMItemNm.Caption = "품명";
            this.colMItemNm.FieldName = "item_nm";
            this.colMItemNm.Name = "colMItemNm";
            this.colMItemNm.Visible = true;
            this.colMItemNm.VisibleIndex = 2;
            this.colMItemNm.Width = 196;
            // 
            // colMItemSpec
            // 
            this.colMItemSpec.Caption = "洹쒓꺽";
            this.colMItemSpec.FieldName = "item_spec";
            this.colMItemSpec.Name = "colMItemSpec";
            this.colMItemSpec.Visible = true;
            this.colMItemSpec.VisibleIndex = 3;
            this.colMItemSpec.Width = 100;
            // 
            // colMUnitCd
            // 
            this.colMUnitCd.Caption = "재고단위";
            this.colMUnitCd.ColumnEdit = this.lookUpcolMUnitCd;
            this.colMUnitCd.FieldName = "unit_cd";
            this.colMUnitCd.Name = "colMUnitCd";
            this.colMUnitCd.Visible = true;
            this.colMUnitCd.VisibleIndex = 4;
            this.colMUnitCd.Width = 70;
            // 
            // lookUpcolMUnitCd
            // 
            this.lookUpcolMUnitCd.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpcolMUnitCd.LookupKey = "L_CM0001";
            this.lookUpcolMUnitCd.Name = "lookUpcolMUnitCd";
            this.lookUpcolMUnitCd.NullText = "";
            this.lookUpcolMUnitCd.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // colMPoUnitCd
            // 
            this.colMPoUnitCd.Caption = "구매단위";
            this.colMPoUnitCd.ColumnEdit = this.lookUpcolMPoUnitCd;
            this.colMPoUnitCd.FieldName = "po_unit_cd";
            this.colMPoUnitCd.Name = "colMPoUnitCd";
            this.colMPoUnitCd.Visible = true;
            this.colMPoUnitCd.VisibleIndex = 5;
            this.colMPoUnitCd.Width = 71;
            // 
            // lookUpcolMPoUnitCd
            // 
            this.lookUpcolMPoUnitCd.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpcolMPoUnitCd.LookupKey = "L_CM0001";
            this.lookUpcolMPoUnitCd.Name = "lookUpcolMPoUnitCd";
            this.lookUpcolMPoUnitCd.NullText = "";
            this.lookUpcolMPoUnitCd.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // colMWhId
            // 
            this.colMWhId.Caption = "李쎄퀬";
            this.colMWhId.ColumnEdit = this.spinEditcolM;
            this.colMWhId.FieldName = "wh_id";
            this.colMWhId.Name = "colMWhId";
            this.colMWhId.Visible = true;
            this.colMWhId.VisibleIndex = 6;
            this.colMWhId.Width = 96;
            // 
            // colMLocId
            // 
            this.colMLocId.Caption = "LOCATION";
            this.colMLocId.ColumnEdit = this.spinEditcolM;
            this.colMLocId.FieldName = "loc_id";
            this.colMLocId.Name = "colMLocId";
            this.colMLocId.Visible = true;
            this.colMLocId.VisibleIndex = 7;
            this.colMLocId.Width = 89;
            // 
            // colMSafeQty
            // 
            this.colMSafeQty.Caption = "안전재고수량";
            this.colMSafeQty.ColumnEdit = this.spinEditcolM;
            this.colMSafeQty.FieldName = "safe_qty";
            this.colMSafeQty.Name = "colMSafeQty";
            this.colMSafeQty.Visible = true;
            this.colMSafeQty.VisibleIndex = 8;
            this.colMSafeQty.Width = 100;
            // 
            // colMDeptId
            // 
            this.colMDeptId.Caption = "부서ID";
            this.colMDeptId.ColumnEdit = this.spinEditcolM;
            this.colMDeptId.FieldName = "dept_id";
            this.colMDeptId.Name = "colMDeptId";
            this.colMDeptId.Width = 100;
            // 
            // colMDeptNm
            // 
            this.colMDeptNm.Caption = "부서";
            this.colMDeptNm.FieldName = "dept_nm";
            this.colMDeptNm.Name = "colMDeptNm";
            this.colMDeptNm.Visible = true;
            this.colMDeptNm.VisibleIndex = 9;
            this.colMDeptNm.Width = 100;
            // 
            // colMEmpId
            // 
            this.colMEmpId.Caption = "사원ID";
            this.colMEmpId.ColumnEdit = this.spinEditcolM;
            this.colMEmpId.FieldName = "emp_id";
            this.colMEmpId.Name = "colMEmpId";
            this.colMEmpId.Width = 100;
            // 
            // colMEmpNo
            // 
            this.colMEmpNo.Caption = "사원번호";
            this.colMEmpNo.FieldName = "emp_no";
            this.colMEmpNo.Name = "colMEmpNo";
            this.colMEmpNo.Visible = true;
            this.colMEmpNo.VisibleIndex = 10;
            this.colMEmpNo.Width = 100;
            // 
            // colMEmpNm
            // 
            this.colMEmpNm.Caption = "사원명";
            this.colMEmpNm.FieldName = "emp_nm";
            this.colMEmpNm.Name = "colMEmpNm";
            this.colMEmpNm.Visible = true;
            this.colMEmpNm.VisibleIndex = 11;
            this.colMEmpNm.Width = 100;
            // 
            // colMCustId
            // 
            this.colMCustId.Caption = "援щℓ泥쁈D";
            this.colMCustId.ColumnEdit = this.spinEditcolM;
            this.colMCustId.FieldName = "cust_id";
            this.colMCustId.Name = "colMCustId";
            this.colMCustId.Width = 100;
            // 
            // colMCustNm
            // 
            this.colMCustNm.Caption = "구매처";
            this.colMCustNm.FieldName = "cust_nm";
            this.colMCustNm.Name = "colMCustNm";
            this.colMCustNm.Visible = true;
            this.colMCustNm.VisibleIndex = 12;
            this.colMCustNm.Width = 100;
            // 
            // colMAssetType
            // 
            this.colMAssetType.Caption = "자산구분";
            this.colMAssetType.ColumnEdit = this.lookUpcolMAssetType;
            this.colMAssetType.FieldName = "asset_type";
            this.colMAssetType.Name = "colMAssetType";
            this.colMAssetType.Visible = true;
            this.colMAssetType.VisibleIndex = 0;
            this.colMAssetType.Width = 79;
            // 
            // lookUpcolMAssetType
            // 
            this.lookUpcolMAssetType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpcolMAssetType.LookupKey = "L_CM0002";
            this.lookUpcolMAssetType.Name = "lookUpcolMAssetType";
            this.lookUpcolMAssetType.NullText = "";
            this.lookUpcolMAssetType.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // colMOutType
            // 
            this.colMOutType.Caption = "외주구분";
            this.colMOutType.FieldName = "out_type";
            this.colMOutType.Name = "colMOutType";
            this.colMOutType.Visible = true;
            this.colMOutType.VisibleIndex = 13;
            this.colMOutType.Width = 100;
            // 
            // colMPoQcYn
            // 
            this.colMPoQcYn.Caption = "수입검사여부";
            this.colMPoQcYn.ColumnEdit = this.chkEditcolM;
            this.colMPoQcYn.FieldName = "po_qc_yn";
            this.colMPoQcYn.Name = "colMPoQcYn";
            this.colMPoQcYn.Visible = true;
            this.colMPoQcYn.VisibleIndex = 14;
            this.colMPoQcYn.Width = 100;
            // 
            // chkEditcolM
            // 
            this.chkEditcolM.Name = "chkEditcolM";
            this.chkEditcolM.ValueChecked = "Y";
            this.chkEditcolM.ValueUnchecked = "N";
            // 
            // colMProdQcYn
            // 
            this.colMProdQcYn.Caption = "공정검사여부";
            this.colMProdQcYn.ColumnEdit = this.chkEditcolM;
            this.colMProdQcYn.FieldName = "prod_qc_yn";
            this.colMProdQcYn.Name = "colMProdQcYn";
            this.colMProdQcYn.Visible = true;
            this.colMProdQcYn.VisibleIndex = 15;
            this.colMProdQcYn.Width = 100;
            // 
            // colMLotYn
            // 
            this.colMLotYn.Caption = "LOT愿由ъ뿬遺";
            this.colMLotYn.ColumnEdit = this.chkEditcolM;
            this.colMLotYn.FieldName = "lot_yn";
            this.colMLotYn.Name = "colMLotYn";
            this.colMLotYn.Visible = true;
            this.colMLotYn.VisibleIndex = 16;
            this.colMLotYn.Width = 100;
            // 
            // colMStockYn
            // 
            this.colMStockYn.Caption = "재고관리여부";
            this.colMStockYn.ColumnEdit = this.chkEditcolM;
            this.colMStockYn.FieldName = "stock_yn";
            this.colMStockYn.Name = "colMStockYn";
            this.colMStockYn.Visible = true;
            this.colMStockYn.VisibleIndex = 17;
            this.colMStockYn.Width = 100;
            // 
            // colMStatCd
            // 
            this.colMStatCd.Caption = "품목상태";
            this.colMStatCd.ColumnEdit = this.lookUpcolMStatCd;
            this.colMStatCd.FieldName = "stat_cd";
            this.colMStatCd.Name = "colMStatCd";
            this.colMStatCd.Visible = true;
            this.colMStatCd.VisibleIndex = 18;
            this.colMStatCd.Width = 100;
            // 
            // lookUpcolMStatCd
            // 
            this.lookUpcolMStatCd.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpcolMStatCd.LookupKey = "L_BA0002";
            this.lookUpcolMStatCd.Name = "lookUpcolMStatCd";
            this.lookUpcolMStatCd.NullText = "";
            this.lookUpcolMStatCd.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // colMGrp1Id
            // 
            this.colMGrp1Id.Caption = "품목그룹1";
            this.colMGrp1Id.ColumnEdit = this.spinEditcolM;
            this.colMGrp1Id.FieldName = "grp1_id";
            this.colMGrp1Id.Name = "colMGrp1Id";
            this.colMGrp1Id.Visible = true;
            this.colMGrp1Id.VisibleIndex = 19;
            this.colMGrp1Id.Width = 100;
            // 
            // colMGrp2Id
            // 
            this.colMGrp2Id.Caption = "품목그룹2";
            this.colMGrp2Id.ColumnEdit = this.spinEditcolM;
            this.colMGrp2Id.FieldName = "grp2_id";
            this.colMGrp2Id.Name = "colMGrp2Id";
            this.colMGrp2Id.Visible = true;
            this.colMGrp2Id.VisibleIndex = 20;
            this.colMGrp2Id.Width = 100;
            // 
            // colMGrp3Id
            // 
            this.colMGrp3Id.Caption = "품목그룹3";
            this.colMGrp3Id.ColumnEdit = this.spinEditcolM;
            this.colMGrp3Id.FieldName = "grp3_id";
            this.colMGrp3Id.Name = "colMGrp3Id";
            this.colMGrp3Id.Visible = true;
            this.colMGrp3Id.VisibleIndex = 21;
            this.colMGrp3Id.Width = 100;
            // 
            // colMGrp4Id
            // 
            this.colMGrp4Id.Caption = "품목그룹4";
            this.colMGrp4Id.ColumnEdit = this.spinEditcolM;
            this.colMGrp4Id.FieldName = "grp4_id";
            this.colMGrp4Id.Name = "colMGrp4Id";
            this.colMGrp4Id.Visible = true;
            this.colMGrp4Id.VisibleIndex = 22;
            this.colMGrp4Id.Width = 100;
            // 
            // colMRemark
            // 
            this.colMRemark.Caption = "鍮꾧퀬";
            this.colMRemark.FieldName = "remark";
            this.colMRemark.Name = "colMRemark";
            this.colMRemark.Visible = true;
            this.colMRemark.VisibleIndex = 23;
            this.colMRemark.Width = 100;
            // 
            // panelWyn2
            // 
            this.panelWyn2.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn2.Appearance.Options.UseBackColor = true;
            this.panelWyn2.Controls.Add(this.sectionHeaderWyn4);
            this.panelWyn2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn2.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn2.Location = new System.Drawing.Point(0, 0);
            this.panelWyn2.Name = "panelWyn2";
            this.panelWyn2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn2.Size = new System.Drawing.Size(1350, 27);
            this.panelWyn2.TabIndex = 11;
            // 
            // sectionHeaderWyn4
            // 
            this.sectionHeaderWyn4.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn4.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn4.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn4.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn4.Name = "sectionHeaderWyn4";
            this.sectionHeaderWyn4.Size = new System.Drawing.Size(1345, 25);
            this.sectionHeaderWyn4.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn4.SvgIcon")));
            this.sectionHeaderWyn4.TabIndex = 9;
            this.sectionHeaderWyn4.Text = "품목LIST";
            // 
            // splitterWyn1
            // 
            this.splitterWyn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitterWyn1.Location = new System.Drawing.Point(1350, 0);
            this.splitterWyn1.Name = "splitterWyn1";
            this.splitterWyn1.Size = new System.Drawing.Size(10, 870);
            this.splitterWyn1.TabIndex = 9;
            this.splitterWyn1.TabStop = false;
            // 
            // panelWyn4
            // 
            this.panelWyn4.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn4.Appearance.Options.UseBackColor = true;
            this.panelWyn4.Controls.Add(this.grd2);
            this.panelWyn4.Controls.Add(this.panelWyn7);
            this.panelWyn4.Controls.Add(this.panelWyn1);
            this.panelWyn4.Controls.Add(this.panelWyn5);
            this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelWyn4.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn4.Location = new System.Drawing.Point(1360, 0);
            this.panelWyn4.Name = "panelWyn4";
            this.panelWyn4.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.panelWyn4.Size = new System.Drawing.Size(756, 870);
            this.panelWyn4.TabIndex = 7;
            // 
            // grd2
            // 
            this.grd2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd2.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grd2.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grd2.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grd2.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grd2.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grd2.Location = new System.Drawing.Point(3, 635);
            this.grd2.MainView = this.gvw2;
            this.grd2.Name = "grd2";
            this.grd2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.spinEditcolD1,
            this.lookUpcolD1FrUnitCd,
            this.lookUpcolD1ToUnitCd});
            this.grd2.Size = new System.Drawing.Size(753, 235);
            this.grd2.TabIndex = 0;
            this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw2});
            // 
            // gvw2
            // 
            this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colD1FrUnitCd,
            this.colD1FrQty,
            this.colD1ToUnitCd,
            this.colD1ToQty,
            this.colD1Remark});
            this.gvw2.GridControl = this.grd2;
            this.gvw2.HighlightFocusedRow = true;
            this.gvw2.Name = "gvw2";
            this.gvw2.OptionsBehavior.Editable = false;
            this.gvw2.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw2.OptionsView.ColumnAutoWidth = false;
            this.gvw2.OptionsView.ShowGroupPanel = false;
            // 
            // colD1FrUnitCd
            // 
            this.colD1FrUnitCd.Caption = "기준단위";
            this.colD1FrUnitCd.ColumnEdit = this.lookUpcolD1FrUnitCd;
            this.colD1FrUnitCd.FieldName = "fr_unit_cd";
            this.colD1FrUnitCd.Name = "colD1FrUnitCd";
            this.colD1FrUnitCd.Visible = true;
            this.colD1FrUnitCd.VisibleIndex = 0;
            this.colD1FrUnitCd.Width = 100;
            // 
            // lookUpcolD1FrUnitCd
            // 
            this.lookUpcolD1FrUnitCd.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpcolD1FrUnitCd.LookupKey = "L_CM0001";
            this.lookUpcolD1FrUnitCd.Name = "lookUpcolD1FrUnitCd";
            this.lookUpcolD1FrUnitCd.NullText = "";
            this.lookUpcolD1FrUnitCd.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // colD1FrQty
            // 
            this.colD1FrQty.Caption = "기준수량";
            this.colD1FrQty.ColumnEdit = this.spinEditcolD1;
            this.colD1FrQty.FieldName = "fr_qty";
            this.colD1FrQty.Name = "colD1FrQty";
            this.colD1FrQty.Visible = true;
            this.colD1FrQty.VisibleIndex = 1;
            this.colD1FrQty.Width = 100;
            // 
            // spinEditcolD1
            // 
            this.spinEditcolD1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEditcolD1.Name = "spinEditcolD1";
            // 
            // colD1ToUnitCd
            // 
            this.colD1ToUnitCd.Caption = "환산단위";
            this.colD1ToUnitCd.ColumnEdit = this.lookUpcolD1ToUnitCd;
            this.colD1ToUnitCd.FieldName = "to_unit_cd";
            this.colD1ToUnitCd.Name = "colD1ToUnitCd";
            this.colD1ToUnitCd.Visible = true;
            this.colD1ToUnitCd.VisibleIndex = 2;
            this.colD1ToUnitCd.Width = 100;
            // 
            // lookUpcolD1ToUnitCd
            // 
            this.lookUpcolD1ToUnitCd.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpcolD1ToUnitCd.LookupKey = "L_CM0001";
            this.lookUpcolD1ToUnitCd.Name = "lookUpcolD1ToUnitCd";
            this.lookUpcolD1ToUnitCd.NullText = "";
            this.lookUpcolD1ToUnitCd.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // colD1ToQty
            // 
            this.colD1ToQty.Caption = "환산수량";
            this.colD1ToQty.ColumnEdit = this.spinEditcolD1;
            this.colD1ToQty.FieldName = "to_qty";
            this.colD1ToQty.Name = "colD1ToQty";
            this.colD1ToQty.Visible = true;
            this.colD1ToQty.VisibleIndex = 3;
            this.colD1ToQty.Width = 100;
            // 
            // colD1Remark
            // 
            this.colD1Remark.Caption = "鍮꾧퀬";
            this.colD1Remark.FieldName = "remark";
            this.colD1Remark.Name = "colD1Remark";
            this.colD1Remark.Visible = true;
            this.colD1Remark.VisibleIndex = 4;
            this.colD1Remark.Width = 100;
            // 
            // panelWyn7
            // 
            this.panelWyn7.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn7.Appearance.Options.UseBackColor = true;
            this.panelWyn7.Controls.Add(this.btnDeletRow2);
            this.panelWyn7.Controls.Add(this.btnAddRow2);
            this.panelWyn7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn7.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn7.Location = new System.Drawing.Point(3, 605);
            this.panelWyn7.Name = "panelWyn7";
            this.panelWyn7.Padding = new System.Windows.Forms.Padding(5, 3, 0, 2);
            this.panelWyn7.Size = new System.Drawing.Size(753, 30);
            this.panelWyn7.TabIndex = 9;
            // 
            // btnDeletRow2
            // 
            this.btnDeletRow2.BackColor = System.Drawing.Color.Transparent;
            this.btnDeletRow2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(198)))), ((int)(((byte)(193)))));
            this.btnDeletRow2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeletRow2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(236)))), ((int)(((byte)(234)))));
            this.btnDeletRow2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDeletRow2.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnDeletRow2.Image = null;
            this.btnDeletRow2.Location = new System.Drawing.Point(65, 3);
            this.btnDeletRow2.Name = "btnDeletRow2";
            this.btnDeletRow2.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDeletRow2.Size = new System.Drawing.Size(58, 24);
            this.btnDeletRow2.TabIndex = 0;
            this.btnDeletRow2.Text = "?됱궘??";
            this.btnDeletRow2.ToolTip = "행삭제(현재 탭)";
            // 
            // btnAddRow2
            // 
            this.btnAddRow2.BackColor = System.Drawing.Color.Transparent;
            this.btnAddRow2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(231)))), ((int)(((byte)(203)))));
            this.btnAddRow2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddRow2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(245)))), ((int)(((byte)(231)))));
            this.btnAddRow2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnAddRow2.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnAddRow2.Image = null;
            this.btnAddRow2.Location = new System.Drawing.Point(3, 3);
            this.btnAddRow2.Name = "btnAddRow2";
            this.btnAddRow2.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnAddRow2.Size = new System.Drawing.Size(58, 24);
            this.btnAddRow2.TabIndex = 0;
            this.btnAddRow2.Text = "?됱텛媛";
            this.btnAddRow2.ToolTip = "행추가(현재 탭)";
            // 
            // panelWyn1
            // 
            this.panelWyn1.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn1.Appearance.Options.UseBackColor = true;
            this.panelWyn1.Controls.Add(this.sectionHeaderWyn2);
            this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn1.Location = new System.Drawing.Point(3, 578);
            this.panelWyn1.Name = "panelWyn1";
            this.panelWyn1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn1.Size = new System.Drawing.Size(753, 27);
            this.panelWyn1.TabIndex = 8;
            // 
            // sectionHeaderWyn2
            // 
            this.sectionHeaderWyn2.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn2.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn2.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn2.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn2.Name = "sectionHeaderWyn2";
            this.sectionHeaderWyn2.Size = new System.Drawing.Size(748, 25);
            this.sectionHeaderWyn2.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn2.SvgIcon")));
            this.sectionHeaderWyn2.TabIndex = 10;
            this.sectionHeaderWyn2.Text = "하위 목록";
            // 
            // panelWyn5
            // 
            this.panelWyn5.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn5.Appearance.Options.UseBackColor = true;
            this.panelWyn5.Controls.Add(this.panData);
            this.panelWyn5.Controls.Add(this.panelWyn6);
            this.panelWyn5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn5.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn5.Location = new System.Drawing.Point(3, 0);
            this.panelWyn5.Name = "panelWyn5";
            this.panelWyn5.Size = new System.Drawing.Size(753, 578);
            this.panelWyn5.TabIndex = 6;
            // 
            // panData
            // 
            this.panData.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panData.Appearance.Options.UseBackColor = true;
            this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panData.Controls.Add(this.grpItemProdQc);
            this.panData.Controls.Add(this.panelWyn9);
            this.panData.Controls.Add(this.grpItemBasic);
            this.panData.Controls.Add(this.grpItemStock);
            this.panData.Controls.Add(this.grpItemClassify);
            this.panData.Controls.Add(this.groupBoxWyn1);
            this.panData.Controls.Add(this.grpItemPersonnel);
            this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(0, 27);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(753, 551);
            this.panData.TabIndex = 8;
            // 
            // grpItemProdQc
            // 
            this.grpItemProdQc.BackColor = System.Drawing.Color.Transparent;
            this.grpItemProdQc.Controls.Add(this.lblDetailPoQcYn);
            this.grpItemProdQc.Controls.Add(this.chkDetailPoQcYn);
            this.grpItemProdQc.Controls.Add(this.lblDetailProdQcYn);
            this.grpItemProdQc.Controls.Add(this.chkDetailProdQcYn);
            this.grpItemProdQc.Controls.Add(this.lblDetailLotYn);
            this.grpItemProdQc.Controls.Add(this.chkDetailLotYn);
            this.grpItemProdQc.Controls.Add(this.lblDetailStockYn);
            this.grpItemProdQc.Controls.Add(this.chkDetailStockYn);
            this.grpItemProdQc.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.grpItemProdQc.Location = new System.Drawing.Point(11, 350);
            this.grpItemProdQc.Name = "grpItemProdQc";
            this.grpItemProdQc.Size = new System.Drawing.Size(696, 47);
            this.grpItemProdQc.TabIndex = 103;
            this.grpItemProdQc.TabStop = false;
            this.grpItemProdQc.Text = "품목 Option";
            // 
            // lblDetailPoQcYn
            // 
            this.lblDetailPoQcYn.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailPoQcYn.Appearance.Options.UseFont = true;
            this.lblDetailPoQcYn.Location = new System.Drawing.Point(12, 21);
            this.lblDetailPoQcYn.Name = "lblDetailPoQcYn";
            this.lblDetailPoQcYn.Size = new System.Drawing.Size(72, 15);
            this.lblDetailPoQcYn.TabIndex = 42;
            this.lblDetailPoQcYn.Text = "수입검사여부";
            // 
            // chkDetailPoQcYn
            // 
            this.chkDetailPoQcYn.Location = new System.Drawing.Point(87, 18);
            this.chkDetailPoQcYn.Name = "chkDetailPoQcYn";
            this.chkDetailPoQcYn.Properties.Caption = "";
            this.chkDetailPoQcYn.Size = new System.Drawing.Size(20, 20);
            this.chkDetailPoQcYn.TabIndex = 43;
            // 
            // lblDetailProdQcYn
            // 
            this.lblDetailProdQcYn.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailProdQcYn.Appearance.Options.UseFont = true;
            this.lblDetailProdQcYn.Location = new System.Drawing.Point(174, 21);
            this.lblDetailProdQcYn.Name = "lblDetailProdQcYn";
            this.lblDetailProdQcYn.Size = new System.Drawing.Size(72, 15);
            this.lblDetailProdQcYn.TabIndex = 44;
            this.lblDetailProdQcYn.Text = "공정검사여부";
            // 
            // chkDetailProdQcYn
            // 
            this.chkDetailProdQcYn.Location = new System.Drawing.Point(249, 18);
            this.chkDetailProdQcYn.Name = "chkDetailProdQcYn";
            this.chkDetailProdQcYn.Properties.Caption = "";
            this.chkDetailProdQcYn.Size = new System.Drawing.Size(20, 20);
            this.chkDetailProdQcYn.TabIndex = 45;
            // 
            // lblDetailLotYn
            // 
            this.lblDetailLotYn.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailLotYn.Appearance.Options.UseFont = true;
            this.lblDetailLotYn.Location = new System.Drawing.Point(494, 21);
            this.lblDetailLotYn.Name = "lblDetailLotYn";
            this.lblDetailLotYn.Size = new System.Drawing.Size(69, 15);
            this.lblDetailLotYn.TabIndex = 46;
            this.lblDetailLotYn.Text = "LOT愿由ъ뿬遺";
            // 
            // chkDetailLotYn
            // 
            this.chkDetailLotYn.Location = new System.Drawing.Point(569, 18);
            this.chkDetailLotYn.Name = "chkDetailLotYn";
            this.chkDetailLotYn.Properties.Caption = "";
            this.chkDetailLotYn.Size = new System.Drawing.Size(20, 20);
            this.chkDetailLotYn.TabIndex = 47;
            // 
            // lblDetailStockYn
            // 
            this.lblDetailStockYn.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailStockYn.Appearance.Options.UseFont = true;
            this.lblDetailStockYn.Location = new System.Drawing.Point(338, 21);
            this.lblDetailStockYn.Name = "lblDetailStockYn";
            this.lblDetailStockYn.Size = new System.Drawing.Size(72, 15);
            this.lblDetailStockYn.TabIndex = 48;
            this.lblDetailStockYn.Text = "재고관리여부";
            // 
            // chkDetailStockYn
            // 
            this.chkDetailStockYn.Location = new System.Drawing.Point(413, 18);
            this.chkDetailStockYn.Name = "chkDetailStockYn";
            this.chkDetailStockYn.Properties.Caption = "";
            this.chkDetailStockYn.Size = new System.Drawing.Size(20, 20);
            this.chkDetailStockYn.TabIndex = 49;
            // 
            // panelWyn9
            // 
            this.panelWyn9.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panelWyn9.Appearance.Options.UseBackColor = true;
            this.panelWyn9.Controls.Add(this.txtDetailEmpId);
            this.panelWyn9.Controls.Add(this.lblDetailEmpId);
            this.panelWyn9.Controls.Add(this.lblDetailDeptId);
            this.panelWyn9.Controls.Add(this.lblDetailItemId);
            this.panelWyn9.Controls.Add(this.txtDetailOutType);
            this.panelWyn9.Controls.Add(this.lblDetailOutType);
            this.panelWyn9.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn9.Location = new System.Drawing.Point(713, 334);
            this.panelWyn9.Name = "panelWyn9";
            this.panelWyn9.Size = new System.Drawing.Size(333, 74);
            this.panelWyn9.TabIndex = 74;
            this.panelWyn9.Visible = false;
            // 
            // txtDetailEmpId
            // 
            this.txtDetailEmpId.Location = new System.Drawing.Point(94, 22);
            this.txtDetailEmpId.Name = "txtDetailEmpId";
            this.txtDetailEmpId.Size = new System.Drawing.Size(72, 20);
            this.txtDetailEmpId.TabIndex = 29;
            // 
            // lblDetailEmpId
            // 
            this.lblDetailEmpId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailEmpId.Appearance.Options.UseFont = true;
            this.lblDetailEmpId.Location = new System.Drawing.Point(45, 25);
            this.lblDetailEmpId.Name = "lblDetailEmpId";
            this.lblDetailEmpId.Size = new System.Drawing.Size(36, 15);
            this.lblDetailEmpId.TabIndex = 26;
            this.lblDetailEmpId.Text = "사원ID";
            // 
            // lblDetailDeptId
            // 
            this.lblDetailDeptId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailDeptId.Appearance.Options.UseFont = true;
            this.lblDetailDeptId.Location = new System.Drawing.Point(45, 48);
            this.lblDetailDeptId.Name = "lblDetailDeptId";
            this.lblDetailDeptId.Size = new System.Drawing.Size(36, 15);
            this.lblDetailDeptId.TabIndex = 22;
            this.lblDetailDeptId.Text = "부서ID";
            // 
            // lblDetailItemId
            // 
            this.lblDetailItemId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailItemId.Appearance.Options.UseFont = true;
            this.lblDetailItemId.Location = new System.Drawing.Point(45, 6);
            this.lblDetailItemId.Name = "lblDetailItemId";
            this.lblDetailItemId.Size = new System.Drawing.Size(36, 15);
            this.lblDetailItemId.TabIndex = 2;
            this.lblDetailItemId.Text = "품목ID";
            // 
            // txtDetailOutType
            // 
            this.txtDetailOutType.Location = new System.Drawing.Point(177, 51);
            this.txtDetailOutType.Name = "txtDetailOutType";
            this.txtDetailOutType.Size = new System.Drawing.Size(220, 20);
            this.txtDetailOutType.TabIndex = 41;
            // 
            // lblDetailOutType
            // 
            this.lblDetailOutType.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailOutType.Appearance.Options.UseFont = true;
            this.lblDetailOutType.Location = new System.Drawing.Point(83, 54);
            this.lblDetailOutType.Name = "lblDetailOutType";
            this.lblDetailOutType.Size = new System.Drawing.Size(48, 15);
            this.lblDetailOutType.TabIndex = 40;
            this.lblDetailOutType.Text = "외주구분";
            // 
            // grpItemBasic
            // 
            this.grpItemBasic.BackColor = System.Drawing.Color.Transparent;
            this.grpItemBasic.Controls.Add(this.txtDetailItemId);
            this.grpItemBasic.Controls.Add(this.lblDetailAssetType);
            this.grpItemBasic.Controls.Add(this.lblDetailAccCd);
            this.grpItemBasic.Controls.Add(this.cboDetailAssetType);
            this.grpItemBasic.Controls.Add(this.cboDetailAccCd);
            this.grpItemBasic.Controls.Add(this.lblDetailStatCd);
            this.grpItemBasic.Controls.Add(this.cboDetailStatCd);
            this.grpItemBasic.Controls.Add(this.lblDetailItemNo);
            this.grpItemBasic.Controls.Add(this.txtDetailItemNo);
            this.grpItemBasic.Controls.Add(this.lblDetailItemNm);
            this.grpItemBasic.Controls.Add(this.txtDetailItemNm);
            this.grpItemBasic.Controls.Add(this.lblDetailItemSpec);
            this.grpItemBasic.Controls.Add(this.txtDetailItemSpec);
            this.grpItemBasic.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.grpItemBasic.Location = new System.Drawing.Point(11, 17);
            this.grpItemBasic.Name = "grpItemBasic";
            this.grpItemBasic.Size = new System.Drawing.Size(368, 175);
            this.grpItemBasic.TabIndex = 100;
            this.grpItemBasic.TabStop = false;
            this.grpItemBasic.Text = "기본정보";
            // 
            // txtDetailItemId
            // 
            this.txtDetailItemId.Location = new System.Drawing.Point(296, 55);
            this.txtDetailItemId.Name = "txtDetailItemId";
            this.txtDetailItemId.Properties.ReadOnly = true;
            this.txtDetailItemId.Size = new System.Drawing.Size(61, 20);
            this.txtDetailItemId.TabIndex = 29;
            // 
            // lblDetailAssetType
            // 
            this.lblDetailAssetType.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailAssetType.Appearance.Options.UseFont = true;
            this.lblDetailAssetType.Location = new System.Drawing.Point(17, 140);
            this.lblDetailAssetType.Name = "lblDetailAssetType";
            this.lblDetailAssetType.Size = new System.Drawing.Size(48, 15);
            this.lblDetailAssetType.TabIndex = 38;
            this.lblDetailAssetType.Text = "자산구분";
            // 
            // lblDetailAccCd
            // 
            this.lblDetailAccCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailAccCd.Appearance.Options.UseFont = true;
            this.lblDetailAccCd.Location = new System.Drawing.Point(29, 30);
            this.lblDetailAccCd.Name = "lblDetailAccCd";
            this.lblDetailAccCd.Size = new System.Drawing.Size(36, 15);
            this.lblDetailAccCd.TabIndex = 0;
            this.lblDetailAccCd.Text = "사업장";
            // 
            // cboDetailAssetType
            // 
            this.cboDetailAssetType.EditValue = "";
            this.cboDetailAssetType.Location = new System.Drawing.Point(73, 137);
            this.cboDetailAssetType.LookupKey = "L_CM0002";
            this.cboDetailAssetType.Name = "cboDetailAssetType";
            this.cboDetailAssetType.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.cboDetailAssetType.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboDetailAssetType.Properties.Appearance.Options.UseBackColor = true;
            this.cboDetailAssetType.Properties.Appearance.Options.UseForeColor = true;
            this.cboDetailAssetType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDetailAssetType.Properties.NullText = "";
            this.cboDetailAssetType.Required = true;
            this.cboDetailAssetType.Size = new System.Drawing.Size(131, 20);
            this.cboDetailAssetType.TabIndex = 39;
            // 
            // cboDetailAccCd
            // 
            this.cboDetailAccCd.EditValue = "";
            this.cboDetailAccCd.Location = new System.Drawing.Point(73, 27);
            this.cboDetailAccCd.LookupKey = "L_ACC";
            this.cboDetailAccCd.Name = "cboDetailAccCd";
            this.cboDetailAccCd.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.cboDetailAccCd.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboDetailAccCd.Properties.Appearance.Options.UseBackColor = true;
            this.cboDetailAccCd.Properties.Appearance.Options.UseForeColor = true;
            this.cboDetailAccCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDetailAccCd.Properties.NullText = "";
            this.cboDetailAccCd.Required = true;
            this.cboDetailAccCd.Size = new System.Drawing.Size(131, 20);
            this.cboDetailAccCd.TabIndex = 1;
            // 
            // lblDetailStatCd
            // 
            this.lblDetailStatCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailStatCd.Appearance.Options.UseFont = true;
            this.lblDetailStatCd.Location = new System.Drawing.Point(222, 28);
            this.lblDetailStatCd.Name = "lblDetailStatCd";
            this.lblDetailStatCd.Size = new System.Drawing.Size(48, 15);
            this.lblDetailStatCd.TabIndex = 58;
            this.lblDetailStatCd.Text = "품목상태";
            // 
            // cboDetailStatCd
            // 
            this.cboDetailStatCd.EditValue = "";
            this.cboDetailStatCd.Location = new System.Drawing.Point(276, 27);
            this.cboDetailStatCd.LookupKey = "L_BA0002";
            this.cboDetailStatCd.Name = "cboDetailStatCd";
            this.cboDetailStatCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDetailStatCd.Properties.NullText = "";
            this.cboDetailStatCd.Size = new System.Drawing.Size(81, 20);
            this.cboDetailStatCd.TabIndex = 59;
            // 
            // lblDetailItemNo
            // 
            this.lblDetailItemNo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailItemNo.Appearance.Options.UseFont = true;
            this.lblDetailItemNo.Location = new System.Drawing.Point(41, 58);
            this.lblDetailItemNo.Name = "lblDetailItemNo";
            this.lblDetailItemNo.Size = new System.Drawing.Size(24, 15);
            this.lblDetailItemNo.TabIndex = 6;
            this.lblDetailItemNo.Text = "품번";
            // 
            // txtDetailItemNo
            // 
            this.txtDetailItemNo.Location = new System.Drawing.Point(73, 55);
            this.txtDetailItemNo.Name = "txtDetailItemNo";
            this.txtDetailItemNo.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.txtDetailItemNo.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDetailItemNo.Properties.Appearance.Options.UseBackColor = true;
            this.txtDetailItemNo.Properties.Appearance.Options.UseForeColor = true;
            this.txtDetailItemNo.Required = true;
            this.txtDetailItemNo.Size = new System.Drawing.Size(221, 20);
            this.txtDetailItemNo.TabIndex = 7;
            // 
            // lblDetailItemNm
            // 
            this.lblDetailItemNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailItemNm.Appearance.Options.UseFont = true;
            this.lblDetailItemNm.Location = new System.Drawing.Point(41, 86);
            this.lblDetailItemNm.Name = "lblDetailItemNm";
            this.lblDetailItemNm.Size = new System.Drawing.Size(24, 15);
            this.lblDetailItemNm.TabIndex = 8;
            this.lblDetailItemNm.Text = "품명";
            // 
            // txtDetailItemNm
            // 
            this.txtDetailItemNm.Location = new System.Drawing.Point(73, 83);
            this.txtDetailItemNm.Name = "txtDetailItemNm";
            this.txtDetailItemNm.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.txtDetailItemNm.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDetailItemNm.Properties.Appearance.Options.UseBackColor = true;
            this.txtDetailItemNm.Properties.Appearance.Options.UseForeColor = true;
            this.txtDetailItemNm.Required = true;
            this.txtDetailItemNm.Size = new System.Drawing.Size(284, 20);
            this.txtDetailItemNm.TabIndex = 9;
            // 
            // lblDetailItemSpec
            // 
            this.lblDetailItemSpec.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailItemSpec.Appearance.Options.UseFont = true;
            this.lblDetailItemSpec.Location = new System.Drawing.Point(41, 114);
            this.lblDetailItemSpec.Name = "lblDetailItemSpec";
            this.lblDetailItemSpec.Size = new System.Drawing.Size(24, 15);
            this.lblDetailItemSpec.TabIndex = 10;
            this.lblDetailItemSpec.Text = "洹쒓꺽";
            // 
            // txtDetailItemSpec
            // 
            this.txtDetailItemSpec.Location = new System.Drawing.Point(73, 111);
            this.txtDetailItemSpec.Name = "txtDetailItemSpec";
            this.txtDetailItemSpec.Size = new System.Drawing.Size(284, 20);
            this.txtDetailItemSpec.TabIndex = 11;
            // 
            // grpItemStock
            // 
            this.grpItemStock.BackColor = System.Drawing.Color.Transparent;
            this.grpItemStock.Controls.Add(this.lblDetailWhId);
            this.grpItemStock.Controls.Add(this.txtDetailWhId);
            this.grpItemStock.Controls.Add(this.popDetailWhNm);
            this.grpItemStock.Controls.Add(this.lblDetailLocId);
            this.grpItemStock.Controls.Add(this.txtDetailLocId);
            this.grpItemStock.Controls.Add(this.popDetailLocNm);
            this.grpItemStock.Controls.Add(this.lblDetailSafeQty);
            this.grpItemStock.Controls.Add(this.numDetailSafeQty);
            this.grpItemStock.Controls.Add(this.cboDetailUnitCd);
            this.grpItemStock.Controls.Add(this.lblDetailUnitCd);
            this.grpItemStock.Controls.Add(this.cboDetailPoUnitCd);
            this.grpItemStock.Controls.Add(this.lblDetailPoUnitCd);
            this.grpItemStock.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.grpItemStock.Location = new System.Drawing.Point(385, 17);
            this.grpItemStock.Name = "grpItemStock";
            this.grpItemStock.Size = new System.Drawing.Size(322, 175);
            this.grpItemStock.TabIndex = 101;
            this.grpItemStock.TabStop = false;
            this.grpItemStock.Text = "재고정보";
            // 
            // lblDetailWhId
            // 
            this.lblDetailWhId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailWhId.Appearance.Options.UseFont = true;
            this.lblDetailWhId.Location = new System.Drawing.Point(53, 30);
            this.lblDetailWhId.Name = "lblDetailWhId";
            this.lblDetailWhId.Size = new System.Drawing.Size(24, 15);
            this.lblDetailWhId.TabIndex = 16;
            this.lblDetailWhId.Text = "李쎄퀬";
            // 
            // txtDetailWhId
            // 
            this.txtDetailWhId.Location = new System.Drawing.Point(241, 27);
            this.txtDetailWhId.Name = "txtDetailWhId";
            this.txtDetailWhId.Properties.ReadOnly = true;
            this.txtDetailWhId.Size = new System.Drawing.Size(60, 20);
            this.txtDetailWhId.TabIndex = 17;
            // 
            // popDetailWhNm
            // 
            this.popDetailWhNm.Location = new System.Drawing.Point(83, 27);
            this.popDetailWhNm.LookupKey = "P_WH";
            this.popDetailWhNm.Name = "popDetailWhNm";
            this.popDetailWhNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.popDetailWhNm.Size = new System.Drawing.Size(155, 20);
            this.popDetailWhNm.TabIndex = 18;
            this.popDetailWhNm.ToolTip = null;
            // 
            // lblDetailLocId
            // 
            this.lblDetailLocId.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailLocId.Appearance.Options.UseFont = true;
            this.lblDetailLocId.Location = new System.Drawing.Point(21, 58);
            this.lblDetailLocId.Name = "lblDetailLocId";
            this.lblDetailLocId.Size = new System.Drawing.Size(58, 15);
            this.lblDetailLocId.TabIndex = 19;
            this.lblDetailLocId.Text = "LOCATION";
            // 
            // txtDetailLocId
            // 
            this.txtDetailLocId.Location = new System.Drawing.Point(241, 55);
            this.txtDetailLocId.Name = "txtDetailLocId";
            this.txtDetailLocId.Properties.ReadOnly = true;
            this.txtDetailLocId.Size = new System.Drawing.Size(60, 20);
            this.txtDetailLocId.TabIndex = 20;
            // 
            // popDetailLocNm
            // 
            this.popDetailLocNm.Location = new System.Drawing.Point(83, 55);
            this.popDetailLocNm.LookupKey = "P_LOC";
            this.popDetailLocNm.Name = "popDetailLocNm";
            this.popDetailLocNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.popDetailLocNm.Size = new System.Drawing.Size(155, 20);
            this.popDetailLocNm.TabIndex = 21;
            this.popDetailLocNm.ToolTip = null;
            // 
            // lblDetailSafeQty
            // 
            this.lblDetailSafeQty.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailSafeQty.Appearance.Options.UseFont = true;
            this.lblDetailSafeQty.Location = new System.Drawing.Point(7, 86);
            this.lblDetailSafeQty.Name = "lblDetailSafeQty";
            this.lblDetailSafeQty.Size = new System.Drawing.Size(72, 15);
            this.lblDetailSafeQty.TabIndex = 20;
            this.lblDetailSafeQty.Text = "안전재고수량";
            // 
            // numDetailSafeQty
            // 
            this.numDetailSafeQty.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numDetailSafeQty.Location = new System.Drawing.Point(83, 83);
            this.numDetailSafeQty.Name = "numDetailSafeQty";
            this.numDetailSafeQty.Properties.MaxValue = new decimal(new int[] {
            -727379969,
            232,
            0,
            0});
            this.numDetailSafeQty.Size = new System.Drawing.Size(106, 20);
            this.numDetailSafeQty.TabIndex = 21;
            // 
            // cboDetailUnitCd
            // 
            this.cboDetailUnitCd.EditValue = "";
            this.cboDetailUnitCd.Location = new System.Drawing.Point(83, 111);
            this.cboDetailUnitCd.LookupKey = "L_CM0001";
            this.cboDetailUnitCd.Name = "cboDetailUnitCd";
            this.cboDetailUnitCd.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.cboDetailUnitCd.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboDetailUnitCd.Properties.Appearance.Options.UseBackColor = true;
            this.cboDetailUnitCd.Properties.Appearance.Options.UseForeColor = true;
            this.cboDetailUnitCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDetailUnitCd.Properties.NullText = "";
            this.cboDetailUnitCd.Required = true;
            this.cboDetailUnitCd.Size = new System.Drawing.Size(106, 20);
            this.cboDetailUnitCd.TabIndex = 13;
            // 
            // lblDetailUnitCd
            // 
            this.lblDetailUnitCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailUnitCd.Appearance.Options.UseFont = true;
            this.lblDetailUnitCd.Location = new System.Drawing.Point(31, 114);
            this.lblDetailUnitCd.Name = "lblDetailUnitCd";
            this.lblDetailUnitCd.Size = new System.Drawing.Size(48, 15);
            this.lblDetailUnitCd.TabIndex = 12;
            this.lblDetailUnitCd.Text = "재고단위";
            // 
            // cboDetailPoUnitCd
            // 
            this.cboDetailPoUnitCd.EditValue = "";
            this.cboDetailPoUnitCd.Location = new System.Drawing.Point(83, 137);
            this.cboDetailPoUnitCd.LookupKey = "L_CM0001";
            this.cboDetailPoUnitCd.Name = "cboDetailPoUnitCd";
            this.cboDetailPoUnitCd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDetailPoUnitCd.Properties.NullText = "";
            this.cboDetailPoUnitCd.Size = new System.Drawing.Size(106, 20);
            this.cboDetailPoUnitCd.TabIndex = 15;
            // 
            // lblDetailPoUnitCd
            // 
            this.lblDetailPoUnitCd.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailPoUnitCd.Appearance.Options.UseFont = true;
            this.lblDetailPoUnitCd.Location = new System.Drawing.Point(31, 140);
            this.lblDetailPoUnitCd.Name = "lblDetailPoUnitCd";
            this.lblDetailPoUnitCd.Size = new System.Drawing.Size(48, 15);
            this.lblDetailPoUnitCd.TabIndex = 14;
            this.lblDetailPoUnitCd.Text = "구매단위";
            // 
            // grpItemClassify
            // 
            this.grpItemClassify.BackColor = System.Drawing.Color.Transparent;
            this.grpItemClassify.Controls.Add(this.lblDetailGrp1Id);
            this.grpItemClassify.Controls.Add(this.cboDetailGrp1Id);
            this.grpItemClassify.Controls.Add(this.lblDetailGrp2Id);
            this.grpItemClassify.Controls.Add(this.cboDetailGrp2Id);
            this.grpItemClassify.Controls.Add(this.lblDetailGrp3Id);
            this.grpItemClassify.Controls.Add(this.cboDetailGrp3Id);
            this.grpItemClassify.Controls.Add(this.lblDetailGrp4Id);
            this.grpItemClassify.Controls.Add(this.cboDetailGrp4Id);
            this.grpItemClassify.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.grpItemClassify.Location = new System.Drawing.Point(11, 198);
            this.grpItemClassify.Name = "grpItemClassify";
            this.grpItemClassify.Size = new System.Drawing.Size(368, 146);
            this.grpItemClassify.TabIndex = 104;
            this.grpItemClassify.TabStop = false;
            this.grpItemClassify.Text = "품목그룹 정보";
            // 
            // lblDetailGrp1Id
            // 
            this.lblDetailGrp1Id.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailGrp1Id.Appearance.Options.UseFont = true;
            this.lblDetailGrp1Id.Location = new System.Drawing.Point(10, 31);
            this.lblDetailGrp1Id.Name = "lblDetailGrp1Id";
            this.lblDetailGrp1Id.Size = new System.Drawing.Size(55, 15);
            this.lblDetailGrp1Id.TabIndex = 60;
            this.lblDetailGrp1Id.Text = "품목그룹1";
            // 
            // cboDetailGrp1Id
            // 
            this.cboDetailGrp1Id.EditValue = "";
            this.cboDetailGrp1Id.Location = new System.Drawing.Point(73, 28);
            this.cboDetailGrp1Id.Name = "cboDetailGrp1Id";
            this.cboDetailGrp1Id.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDetailGrp1Id.Properties.NullText = "";
            this.cboDetailGrp1Id.Size = new System.Drawing.Size(220, 20);
            this.cboDetailGrp1Id.TabIndex = 61;
            // 
            // lblDetailGrp2Id
            // 
            this.lblDetailGrp2Id.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailGrp2Id.Appearance.Options.UseFont = true;
            this.lblDetailGrp2Id.Location = new System.Drawing.Point(10, 59);
            this.lblDetailGrp2Id.Name = "lblDetailGrp2Id";
            this.lblDetailGrp2Id.Size = new System.Drawing.Size(55, 15);
            this.lblDetailGrp2Id.TabIndex = 62;
            this.lblDetailGrp2Id.Text = "품목그룹2";
            // 
            // cboDetailGrp2Id
            // 
            this.cboDetailGrp2Id.EditValue = "";
            this.cboDetailGrp2Id.Location = new System.Drawing.Point(73, 56);
            this.cboDetailGrp2Id.Name = "cboDetailGrp2Id";
            this.cboDetailGrp2Id.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDetailGrp2Id.Properties.NullText = "";
            this.cboDetailGrp2Id.Size = new System.Drawing.Size(220, 20);
            this.cboDetailGrp2Id.TabIndex = 63;
            // 
            // lblDetailGrp3Id
            // 
            this.lblDetailGrp3Id.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailGrp3Id.Appearance.Options.UseFont = true;
            this.lblDetailGrp3Id.Location = new System.Drawing.Point(10, 87);
            this.lblDetailGrp3Id.Name = "lblDetailGrp3Id";
            this.lblDetailGrp3Id.Size = new System.Drawing.Size(55, 15);
            this.lblDetailGrp3Id.TabIndex = 64;
            this.lblDetailGrp3Id.Text = "품목그룹3";
            // 
            // cboDetailGrp3Id
            // 
            this.cboDetailGrp3Id.EditValue = "";
            this.cboDetailGrp3Id.Location = new System.Drawing.Point(73, 84);
            this.cboDetailGrp3Id.Name = "cboDetailGrp3Id";
            this.cboDetailGrp3Id.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDetailGrp3Id.Properties.NullText = "";
            this.cboDetailGrp3Id.Size = new System.Drawing.Size(220, 20);
            this.cboDetailGrp3Id.TabIndex = 65;
            // 
            // lblDetailGrp4Id
            // 
            this.lblDetailGrp4Id.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailGrp4Id.Appearance.Options.UseFont = true;
            this.lblDetailGrp4Id.Location = new System.Drawing.Point(10, 115);
            this.lblDetailGrp4Id.Name = "lblDetailGrp4Id";
            this.lblDetailGrp4Id.Size = new System.Drawing.Size(55, 15);
            this.lblDetailGrp4Id.TabIndex = 66;
            this.lblDetailGrp4Id.Text = "품목그룹4";
            // 
            // cboDetailGrp4Id
            // 
            this.cboDetailGrp4Id.EditValue = "";
            this.cboDetailGrp4Id.Location = new System.Drawing.Point(73, 112);
            this.cboDetailGrp4Id.Name = "cboDetailGrp4Id";
            this.cboDetailGrp4Id.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDetailGrp4Id.Properties.NullText = "";
            this.cboDetailGrp4Id.Size = new System.Drawing.Size(220, 20);
            this.cboDetailGrp4Id.TabIndex = 67;
            // 
            // groupBoxWyn1
            // 
            this.groupBoxWyn1.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxWyn1.Controls.Add(this.txtDetailRemark);
            this.groupBoxWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.groupBoxWyn1.Location = new System.Drawing.Point(11, 406);
            this.groupBoxWyn1.Name = "groupBoxWyn1";
            this.groupBoxWyn1.Size = new System.Drawing.Size(696, 130);
            this.groupBoxWyn1.TabIndex = 102;
            this.groupBoxWyn1.TabStop = false;
            this.groupBoxWyn1.Text = "기타정보";
            // 
            // txtDetailRemark
            // 
            this.txtDetailRemark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDetailRemark.Location = new System.Drawing.Point(3, 19);
            this.txtDetailRemark.Name = "txtDetailRemark";
            this.txtDetailRemark.Size = new System.Drawing.Size(690, 108);
            this.txtDetailRemark.TabIndex = 0;
            // 
            // grpItemPersonnel
            // 
            this.grpItemPersonnel.BackColor = System.Drawing.Color.Transparent;
            this.grpItemPersonnel.Controls.Add(this.txtDetailEmpNo);
            this.grpItemPersonnel.Controls.Add(this.lblDetailEmpNm);
            this.grpItemPersonnel.Controls.Add(this.popDetailEmpNm);
            this.grpItemPersonnel.Controls.Add(this.txtDetailDeptId);
            this.grpItemPersonnel.Controls.Add(this.lblDetailDeptNm);
            this.grpItemPersonnel.Controls.Add(this.popDetailDeptNm);
            this.grpItemPersonnel.Controls.Add(this.txtDetailCustId);
            this.grpItemPersonnel.Controls.Add(this.lblDetailCustNm);
            this.grpItemPersonnel.Controls.Add(this.popDetailCustNm);
            this.grpItemPersonnel.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.grpItemPersonnel.Location = new System.Drawing.Point(385, 200);
            this.grpItemPersonnel.Name = "grpItemPersonnel";
            this.grpItemPersonnel.Size = new System.Drawing.Size(322, 144);
            this.grpItemPersonnel.TabIndex = 102;
            this.grpItemPersonnel.TabStop = false;
            this.grpItemPersonnel.Text = "담당자/구매정보";
            // 
            // txtDetailEmpNo
            // 
            this.txtDetailEmpNo.Location = new System.Drawing.Point(191, 28);
            this.txtDetailEmpNo.Name = "txtDetailEmpNo";
            this.txtDetailEmpNo.Properties.ReadOnly = true;
            this.txtDetailEmpNo.Size = new System.Drawing.Size(112, 20);
            this.txtDetailEmpNo.TabIndex = 29;
            // 
            // lblDetailEmpNm
            // 
            this.lblDetailEmpNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailEmpNm.Appearance.Options.UseFont = true;
            this.lblDetailEmpNm.Location = new System.Drawing.Point(42, 31);
            this.lblDetailEmpNm.Name = "lblDetailEmpNm";
            this.lblDetailEmpNm.Size = new System.Drawing.Size(36, 15);
            this.lblDetailEmpNm.TabIndex = 30;
            this.lblDetailEmpNm.Text = "사원명";
            // 
            // popDetailEmpNm
            // 
            this.popDetailEmpNm.Location = new System.Drawing.Point(83, 28);
            this.popDetailEmpNm.LookupKey = "P_EMP";
            this.popDetailEmpNm.Name = "popDetailEmpNm";
            this.popDetailEmpNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", 24, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.popDetailEmpNm.Size = new System.Drawing.Size(106, 20);
            this.popDetailEmpNm.TabIndex = 31;
            this.popDetailEmpNm.ToolTip = null;
            // 
            // txtDetailDeptId
            // 
            this.txtDetailDeptId.Location = new System.Drawing.Point(241, 56);
            this.txtDetailDeptId.Name = "txtDetailDeptId";
            this.txtDetailDeptId.Properties.ReadOnly = true;
            this.txtDetailDeptId.Size = new System.Drawing.Size(62, 20);
            this.txtDetailDeptId.TabIndex = 17;
            // 
            // lblDetailDeptNm
            // 
            this.lblDetailDeptNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailDeptNm.Appearance.Options.UseFont = true;
            this.lblDetailDeptNm.Location = new System.Drawing.Point(30, 59);
            this.lblDetailDeptNm.Name = "lblDetailDeptNm";
            this.lblDetailDeptNm.Size = new System.Drawing.Size(48, 15);
            this.lblDetailDeptNm.TabIndex = 24;
            this.lblDetailDeptNm.Text = "담당부서";
            // 
            // popDetailDeptNm
            // 
            this.popDetailDeptNm.Location = new System.Drawing.Point(83, 56);
            this.popDetailDeptNm.LookupKey = "P_DEPT";
            this.popDetailDeptNm.Name = "popDetailDeptNm";
            this.popDetailDeptNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", 24, true, true, false, editorButtonImageOptions2, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, serializableAppearanceObject6, serializableAppearanceObject7, serializableAppearanceObject8, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.popDetailDeptNm.Size = new System.Drawing.Size(155, 20);
            this.popDetailDeptNm.TabIndex = 25;
            this.popDetailDeptNm.ToolTip = null;
            // 
            // txtDetailCustId
            // 
            this.txtDetailCustId.Location = new System.Drawing.Point(241, 84);
            this.txtDetailCustId.Name = "txtDetailCustId";
            this.txtDetailCustId.Properties.ReadOnly = true;
            this.txtDetailCustId.Size = new System.Drawing.Size(62, 20);
            this.txtDetailCustId.TabIndex = 29;
            // 
            // lblDetailCustNm
            // 
            this.lblDetailCustNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailCustNm.Appearance.Options.UseFont = true;
            this.lblDetailCustNm.Location = new System.Drawing.Point(42, 87);
            this.lblDetailCustNm.Name = "lblDetailCustNm";
            this.lblDetailCustNm.Size = new System.Drawing.Size(36, 15);
            this.lblDetailCustNm.TabIndex = 36;
            this.lblDetailCustNm.Text = "구매처";
            // 
            // popDetailCustNm
            // 
            this.popDetailCustNm.Location = new System.Drawing.Point(83, 84);
            this.popDetailCustNm.LookupKey = "P_CUST";
            this.popDetailCustNm.Name = "popDetailCustNm";
            this.popDetailCustNm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", 24, true, true, false, editorButtonImageOptions3, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject9, serializableAppearanceObject10, serializableAppearanceObject11, serializableAppearanceObject12, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.popDetailCustNm.Size = new System.Drawing.Size(155, 20);
            this.popDetailCustNm.TabIndex = 37;
            this.popDetailCustNm.ToolTip = null;
            // 
            // panelWyn6
            // 
            this.panelWyn6.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn6.Appearance.Options.UseBackColor = true;
            this.panelWyn6.Controls.Add(this.sectionHeaderWyn3);
            this.panelWyn6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn6.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn6.Location = new System.Drawing.Point(0, 0);
            this.panelWyn6.Name = "panelWyn6";
            this.panelWyn6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn6.Size = new System.Drawing.Size(753, 27);
            this.panelWyn6.TabIndex = 7;
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
            this.sectionHeaderWyn3.Size = new System.Drawing.Size(748, 25);
            this.sectionHeaderWyn3.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn3.SvgIcon")));
            this.sectionHeaderWyn3.TabIndex = 9;
            this.sectionHeaderWyn3.Text = "품목정보 등록";
            // 
            // panHeader
            // 
            this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.labelControl1);
            this.panHeader.Controls.Add(this.txtItemNo_Q);
            this.panHeader.Controls.Add(this.labelControl2);
            this.panHeader.Controls.Add(this.txtItemId);
            this.panHeader.Controls.Add(this.cboDetailAccCd_Q);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(5, 33);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(2116, 49);
            this.panHeader.TabIndex = 8;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(214, 19);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(53, 15);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "품번/품명";
            // 
            // txtItemNo_Q
            // 
            this.txtItemNo_Q.Location = new System.Drawing.Point(275, 16);
            this.txtItemNo_Q.Name = "txtItemNo_Q";
            this.txtItemNo_Q.Size = new System.Drawing.Size(204, 20);
            this.txtItemNo_Q.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(14, 19);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(36, 15);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "?ъ뾽??";
            // 
            // txtItemId
            // 
            this.txtItemId.Location = new System.Drawing.Point(1068, 13);
            this.txtItemId.Name = "txtItemId";
            this.txtItemId.Size = new System.Drawing.Size(150, 20);
            this.txtItemId.TabIndex = 1;
            this.txtItemId.Visible = false;
            // 
            // cboDetailAccCd_Q
            // 
            this.cboDetailAccCd_Q.EditValue = "";
            this.cboDetailAccCd_Q.Location = new System.Drawing.Point(58, 16);
            this.cboDetailAccCd_Q.LookupKey = "L_ACC";
            this.cboDetailAccCd_Q.Name = "cboDetailAccCd_Q";
            this.cboDetailAccCd_Q.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(219)))));
            this.cboDetailAccCd_Q.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboDetailAccCd_Q.Properties.Appearance.Options.UseBackColor = true;
            this.cboDetailAccCd_Q.Properties.Appearance.Options.UseForeColor = true;
            this.cboDetailAccCd_Q.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDetailAccCd_Q.Properties.NullText = "";
            this.cboDetailAccCd_Q.Required = true;
            this.cboDetailAccCd_Q.Size = new System.Drawing.Size(131, 20);
            this.cboDetailAccCd_Q.TabIndex = 1;
            // 
            // paTitle
            // 
            this.paTitle.Appearance.BackColor = System.Drawing.Color.White;
            this.paTitle.Appearance.Options.UseBackColor = true;
            this.paTitle.Controls.Add(this.sectionHeaderWyn1);
            this.paTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitle.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitle.Location = new System.Drawing.Point(5, 0);
            this.paTitle.Name = "paTitle";
            this.paTitle.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.paTitle.Size = new System.Drawing.Size(2116, 33);
            this.paTitle.TabIndex = 5;
            // 
            // sectionHeaderWyn1
            // 
            this.sectionHeaderWyn1.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn1.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(2111, 33);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 10;
            this.sectionHeaderWyn1.Text = "품목정보등록 [frmItem]";
            // 
            // frmItem
            // 
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(2126, 957);
            this.Controls.Add(this.panBase);
            this.Name = "frmItem";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
            this.panelWyn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn8)).EndInit();
            this.panelWyn8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcolMAccCd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditcolM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcolMUnitCd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcolMPoUnitCd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcolMAssetType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEditcolM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcolMStatCd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn2)).EndInit();
            this.panelWyn2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
            this.panelWyn4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcolD1FrUnitCd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditcolD1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcolD1ToUnitCd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).EndInit();
            this.panelWyn7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
            this.panelWyn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
            this.panelWyn5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
            this.panData.ResumeLayout(false);
            this.grpItemProdQc.ResumeLayout(false);
            this.grpItemProdQc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkDetailPoQcYn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDetailProdQcYn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDetailLotYn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDetailStockYn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn9)).EndInit();
            this.panelWyn9.ResumeLayout(false);
            this.panelWyn9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailEmpId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailOutType.Properties)).EndInit();
            this.grpItemBasic.ResumeLayout(false);
            this.grpItemBasic.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailItemId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailAssetType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailAccCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailStatCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailItemNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailItemNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailItemSpec.Properties)).EndInit();
            this.grpItemStock.ResumeLayout(false);
            this.grpItemStock.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailWhId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popDetailWhNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailLocId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popDetailLocNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDetailSafeQty.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailUnitCd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailPoUnitCd.Properties)).EndInit();
            this.grpItemClassify.ResumeLayout(false);
            this.grpItemClassify.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailGrp1Id.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailGrp2Id.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailGrp3Id.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailGrp4Id.Properties)).EndInit();
            this.groupBoxWyn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailRemark.Properties)).EndInit();
            this.grpItemPersonnel.ResumeLayout(false);
            this.grpItemPersonnel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailEmpNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popDetailEmpNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailDeptId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popDetailDeptNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetailCustId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popDetailCustNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn6)).EndInit();
            this.panelWyn6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemNo_Q.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDetailAccCd_Q.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).EndInit();
            this.paTitle.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    private PanelWyn panBase;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn4;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private DevExpress.XtraGrid.Columns.GridColumn colD1FrUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colD1FrQty;
    private DevExpress.XtraGrid.Columns.GridColumn colD1ToUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colD1ToQty;
    private DevExpress.XtraGrid.Columns.GridColumn colD1Remark;
    private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spinEditcolD1;
    private LookUpColumnEdit lookUpcolD1FrUnitCd;
    private LookUpColumnEdit lookUpcolD1ToUnitCd;
    private PanelWyn panelWyn1;
    private PanelWyn panelWyn7;
    private ButtonWyn btnDeletRow2;
    private ButtonWyn btnAddRow2;
    private PanelWyn panelWyn5;
    private PanelWyn panData;
    private GroupBoxWyn grpItemBasic;
    private GroupBoxWyn grpItemStock;
    private GroupBoxWyn grpItemPersonnel;
    private GroupBoxWyn grpItemProdQc;
    private GroupBoxWyn grpItemClassify;
    private DevExpress.XtraEditors.LabelControl lblDetailAccCd;
    private LookUpEditWyn cboDetailAccCd;
    private DevExpress.XtraEditors.LabelControl lblDetailItemId;
    private DevExpress.XtraEditors.LabelControl lblDetailItemNo;
    private TextEditWyn txtDetailItemNo;
    private DevExpress.XtraEditors.LabelControl lblDetailItemNm;
    private TextEditWyn txtDetailItemNm;
    private DevExpress.XtraEditors.LabelControl lblDetailItemSpec;
    private TextEditWyn txtDetailItemSpec;
    private DevExpress.XtraEditors.LabelControl lblDetailUnitCd;
    private LookUpEditWyn cboDetailUnitCd;
    private DevExpress.XtraEditors.LabelControl lblDetailPoUnitCd;
    private LookUpEditWyn cboDetailPoUnitCd;
    private DevExpress.XtraEditors.LabelControl lblDetailWhId;
    private TextEditWyn txtDetailWhId;
    private PopupLookupEditWyn popDetailWhNm;
    private DevExpress.XtraEditors.LabelControl lblDetailLocId;
    private TextEditWyn txtDetailLocId;
    private PopupLookupEditWyn popDetailLocNm;
    private DevExpress.XtraEditors.LabelControl lblDetailSafeQty;
    private SpinEditWyn numDetailSafeQty;
    private DevExpress.XtraEditors.LabelControl lblDetailDeptId;
    private DevExpress.XtraEditors.LabelControl lblDetailDeptNm;
    private PopupLookupEditWyn popDetailDeptNm;
    private DevExpress.XtraEditors.LabelControl lblDetailEmpId;
    private TextEditWyn txtDetailEmpNo;
    private TextEditWyn txtDetailEmpId;
    private DevExpress.XtraEditors.LabelControl lblDetailEmpNm;
    private PopupLookupEditWyn popDetailEmpNm;
    private DevExpress.XtraEditors.LabelControl lblDetailCustNm;
    private PopupLookupEditWyn popDetailCustNm;
    private DevExpress.XtraEditors.LabelControl lblDetailAssetType;
    private LookUpEditWyn cboDetailAssetType;
    private DevExpress.XtraEditors.LabelControl lblDetailOutType;
    private TextEditWyn txtDetailOutType;
    private DevExpress.XtraEditors.LabelControl lblDetailPoQcYn;
    private CheckBoxWyn chkDetailPoQcYn;
    private DevExpress.XtraEditors.LabelControl lblDetailProdQcYn;
    private CheckBoxWyn chkDetailProdQcYn;
    private DevExpress.XtraEditors.LabelControl lblDetailLotYn;
    private CheckBoxWyn chkDetailLotYn;
    private DevExpress.XtraEditors.LabelControl lblDetailStockYn;
    private CheckBoxWyn chkDetailStockYn;
    private DevExpress.XtraEditors.LabelControl lblDetailStatCd;
    private LookUpEditWyn cboDetailStatCd;
    private DevExpress.XtraEditors.LabelControl lblDetailGrp1Id;
    private LookUpEditWyn cboDetailGrp1Id;
    private DevExpress.XtraEditors.LabelControl lblDetailGrp2Id;
    private LookUpEditWyn cboDetailGrp2Id;
    private DevExpress.XtraEditors.LabelControl lblDetailGrp3Id;
    private LookUpEditWyn cboDetailGrp3Id;
    private DevExpress.XtraEditors.LabelControl lblDetailGrp4Id;
    private LookUpEditWyn cboDetailGrp4Id;
    private PanelWyn panelWyn6;
    private SplitterWyn splitterWyn1;
    private PanelWyn panelWyn8;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colMAccCd;
    private DevExpress.XtraGrid.Columns.GridColumn colMItemId;
    private DevExpress.XtraGrid.Columns.GridColumn colMItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colMItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colMItemSpec;
    private DevExpress.XtraGrid.Columns.GridColumn colMUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colMPoUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colMWhId;
    private DevExpress.XtraGrid.Columns.GridColumn colMLocId;
    private DevExpress.XtraGrid.Columns.GridColumn colMSafeQty;
    private DevExpress.XtraGrid.Columns.GridColumn colMDeptId;
    private DevExpress.XtraGrid.Columns.GridColumn colMDeptNm;
    private DevExpress.XtraGrid.Columns.GridColumn colMEmpId;
    private DevExpress.XtraGrid.Columns.GridColumn colMEmpNo;
    private DevExpress.XtraGrid.Columns.GridColumn colMEmpNm;
    private DevExpress.XtraGrid.Columns.GridColumn colMCustId;
    private DevExpress.XtraGrid.Columns.GridColumn colMCustNm;
    private DevExpress.XtraGrid.Columns.GridColumn colMAssetType;
    private DevExpress.XtraGrid.Columns.GridColumn colMOutType;
    private DevExpress.XtraGrid.Columns.GridColumn colMPoQcYn;
    private DevExpress.XtraGrid.Columns.GridColumn colMProdQcYn;
    private DevExpress.XtraGrid.Columns.GridColumn colMLotYn;
    private DevExpress.XtraGrid.Columns.GridColumn colMStockYn;
    private DevExpress.XtraGrid.Columns.GridColumn colMStatCd;
    private DevExpress.XtraGrid.Columns.GridColumn colMGrp1Id;
    private DevExpress.XtraGrid.Columns.GridColumn colMGrp2Id;
    private DevExpress.XtraGrid.Columns.GridColumn colMGrp3Id;
    private DevExpress.XtraGrid.Columns.GridColumn colMGrp4Id;
    private DevExpress.XtraGrid.Columns.GridColumn colMRemark;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkEditcolM;
    private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spinEditcolM;
    private LookUpColumnEdit lookUpcolMAccCd;
    private LookUpColumnEdit lookUpcolMUnitCd;
    private LookUpColumnEdit lookUpcolMPoUnitCd;
    private LookUpColumnEdit lookUpcolMAssetType;
    private LookUpColumnEdit lookUpcolMStatCd;
    private PanelWyn panelWyn2;
    private PanelWyn panHeader;
    private TextEditWyn txtItemId;
    private DevExpress.XtraEditors.LabelControl labelControl1;
    private PanelWyn paTitle;
    private SectionHeaderWyn sectionHeaderWyn2;
    private SectionHeaderWyn sectionHeaderWyn3;
    private SectionHeaderWyn sectionHeaderWyn4;
    private TextEditWyn txtDetailDeptId;
    private TextEditWyn txtDetailCustId;
    private TextEditWyn txtItemNo_Q;
    private SectionHeaderWyn sectionHeaderWyn1;
    private PanelWyn panelWyn9;
    private TextEditWyn txtDetailItemId;
    private GroupBoxWyn groupBoxWyn1;
    private MemoEditWyn txtDetailRemark;
    private DevExpress.XtraEditors.LabelControl labelControl2;
    private LookUpEditWyn cboDetailAccCd_Q;
}
