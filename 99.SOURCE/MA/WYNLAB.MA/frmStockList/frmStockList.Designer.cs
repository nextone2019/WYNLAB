// 재고현황(frmStockList) - 현재고(TMASTOCK) 조회 전용 화면(2026-09-25). 품목/창고/위치/LOT별 현재고.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

public partial class frmStockList
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStockList));
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelTop = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItemSpec = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWhNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLocNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLotNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnitCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStockQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTransId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUptDt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.lblSearchKeyword = new DevExpress.XtraEditors.LabelControl();
            this.txtSearchKeyword = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblSearchWh = new DevExpress.XtraEditors.LabelControl();
            this.txtSearchWh = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblSearchLot = new DevExpress.XtraEditors.LabelControl();
            this.txtSearchLot = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblSearchZero = new DevExpress.XtraEditors.LabelControl();
            this.cboSearchZero = new DevExpress.XtraEditors.ComboBoxEdit();
            this.paTitleH = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.paTitle1 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
            this.panBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelTop)).BeginInit();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchWh.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchLot.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchZero.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).BeginInit();
            this.paTitleH.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle1)).BeginInit();
            this.paTitle1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panBase
            // 
            this.panBase.Appearance.BackColor = System.Drawing.Color.White;
            this.panBase.Appearance.Options.UseBackColor = true;
            this.panBase.Controls.Add(this.panelTop);
            this.panBase.Controls.Add(this.panHeader);
            this.panBase.Controls.Add(this.paTitleH);
            this.panBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBase.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panBase.Location = new System.Drawing.Point(0, 0);
            this.panBase.Name = "panBase";
            this.panBase.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.panBase.Size = new System.Drawing.Size(1680, 760);
            this.panBase.TabIndex = 6;
            // 
            // panelTop
            // 
            this.panelTop.Appearance.BackColor = System.Drawing.Color.White;
            this.panelTop.Appearance.Options.UseBackColor = true;
            this.panelTop.Controls.Add(this.grd1);
            this.panelTop.Controls.Add(this.paTitle1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTop.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelTop.Location = new System.Drawing.Point(5, 74);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelTop.Size = new System.Drawing.Size(1670, 681);
            this.panelTop.TabIndex = 0;
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
            this.grd1.Size = new System.Drawing.Size(1670, 646);
            this.grd1.TabIndex = 1;
            this.grd1.UseEmbeddedNavigator = false;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            // 
            // gvw1
            // 
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colItemNo,
            this.colItemNm,
            this.colItemSpec,
            this.colWhNm,
            this.colLocNm,
            this.colLotNo,
            this.colUnitCd,
            this.colStockQty,
            this.colTransId,
            this.colUptDt});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.HighlightFocusedRow = true;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsBehavior.Editable = false;
            this.gvw1.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            // 
            // colItemNo
            // 
            this.colItemNo.Caption = "품번";
            this.colItemNo.FieldName = "item_no";
            this.colItemNo.Name = "colItemNo";
            this.colItemNo.OptionsColumn.AllowEdit = false;
            this.colItemNo.Visible = true;
            this.colItemNo.VisibleIndex = 0;
            this.colItemNo.Width = 110;
            // 
            // colItemNm
            // 
            this.colItemNm.Caption = "품명";
            this.colItemNm.FieldName = "item_nm";
            this.colItemNm.Name = "colItemNm";
            this.colItemNm.OptionsColumn.AllowEdit = false;
            this.colItemNm.Visible = true;
            this.colItemNm.VisibleIndex = 1;
            this.colItemNm.Width = 160;
            // 
            // colItemSpec
            // 
            this.colItemSpec.Caption = "규격";
            this.colItemSpec.FieldName = "item_spec";
            this.colItemSpec.Name = "colItemSpec";
            this.colItemSpec.OptionsColumn.AllowEdit = false;
            this.colItemSpec.Visible = true;
            this.colItemSpec.VisibleIndex = 2;
            this.colItemSpec.Width = 120;
            // 
            // colWhNm
            // 
            this.colWhNm.Caption = "창고";
            this.colWhNm.FieldName = "wh_nm";
            this.colWhNm.Name = "colWhNm";
            this.colWhNm.OptionsColumn.AllowEdit = false;
            this.colWhNm.Visible = true;
            this.colWhNm.VisibleIndex = 3;
            this.colWhNm.Width = 110;
            // 
            // colLocNm
            // 
            this.colLocNm.Caption = "위치";
            this.colLocNm.FieldName = "loc_nm";
            this.colLocNm.Name = "colLocNm";
            this.colLocNm.OptionsColumn.AllowEdit = false;
            this.colLocNm.Visible = true;
            this.colLocNm.VisibleIndex = 4;
            this.colLocNm.Width = 100;
            // 
            // colLotNo
            // 
            this.colLotNo.Caption = "LOT";
            this.colLotNo.FieldName = "lot_no";
            this.colLotNo.Name = "colLotNo";
            this.colLotNo.OptionsColumn.AllowEdit = false;
            this.colLotNo.Visible = true;
            this.colLotNo.VisibleIndex = 5;
            this.colLotNo.Width = 100;
            // 
            // colUnitCd
            // 
            this.colUnitCd.Caption = "단위";
            this.colUnitCd.FieldName = "unit_cd";
            this.colUnitCd.Name = "colUnitCd";
            this.colUnitCd.OptionsColumn.AllowEdit = false;
            this.colUnitCd.Visible = true;
            this.colUnitCd.VisibleIndex = 6;
            this.colUnitCd.Width = 50;
            // 
            // colStockQty
            // 
            this.colStockQty.Caption = "현재고";
            this.colStockQty.DisplayFormat.FormatString = "#,##0.####";
            this.colStockQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colStockQty.FieldName = "stock_qty";
            this.colStockQty.Name = "colStockQty";
            this.colStockQty.OptionsColumn.AllowEdit = false;
            this.colStockQty.Visible = true;
            this.colStockQty.VisibleIndex = 7;
            this.colStockQty.Width = 100;
            // 
            // colTransId
            // 
            this.colTransId.Caption = "최종수불";
            this.colTransId.FieldName = "trans_id";
            this.colTransId.Name = "colTransId";
            this.colTransId.OptionsColumn.AllowEdit = false;
            this.colTransId.Visible = true;
            this.colTransId.VisibleIndex = 8;
            this.colTransId.Width = 80;
            // 
            // colUptDt
            // 
            this.colUptDt.Caption = "최종변경";
            this.colUptDt.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.colUptDt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colUptDt.FieldName = "upt_dt";
            this.colUptDt.Name = "colUptDt";
            this.colUptDt.OptionsColumn.AllowEdit = false;
            this.colUptDt.Visible = true;
            this.colUptDt.VisibleIndex = 9;
            this.colUptDt.Width = 120;
            // 
            // panHeader
            // 
            this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.lblSearchKeyword);
            this.panHeader.Controls.Add(this.txtSearchKeyword);
            this.panHeader.Controls.Add(this.lblSearchWh);
            this.panHeader.Controls.Add(this.txtSearchWh);
            this.panHeader.Controls.Add(this.lblSearchLot);
            this.panHeader.Controls.Add(this.txtSearchLot);
            this.panHeader.Controls.Add(this.lblSearchZero);
            this.panHeader.Controls.Add(this.cboSearchZero);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(5, 25);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1670, 49);
            this.panHeader.TabIndex = 8;
            // 
            // lblSearchKeyword
            // 
            this.lblSearchKeyword.Location = new System.Drawing.Point(25, 18);
            this.lblSearchKeyword.Name = "lblSearchKeyword";
            this.lblSearchKeyword.Size = new System.Drawing.Size(70, 14);
            this.lblSearchKeyword.TabIndex = 0;
            this.lblSearchKeyword.Text = "품번/품명/규격";
            // 
            // txtSearchKeyword
            // 
            this.txtSearchKeyword.Location = new System.Drawing.Point(129, 15);
            this.txtSearchKeyword.Name = "txtSearchKeyword";
            this.txtSearchKeyword.Size = new System.Drawing.Size(150, 20);
            this.txtSearchKeyword.TabIndex = 1;
            // 
            // lblSearchWh
            // 
            this.lblSearchWh.Location = new System.Drawing.Point(301, 18);
            this.lblSearchWh.Name = "lblSearchWh";
            this.lblSearchWh.Size = new System.Drawing.Size(20, 14);
            this.lblSearchWh.TabIndex = 2;
            this.lblSearchWh.Text = "창고";
            // 
            // txtSearchWh
            // 
            this.txtSearchWh.Location = new System.Drawing.Point(333, 15);
            this.txtSearchWh.Name = "txtSearchWh";
            this.txtSearchWh.Size = new System.Drawing.Size(110, 20);
            this.txtSearchWh.TabIndex = 3;
            // 
            // lblSearchLot
            // 
            this.lblSearchLot.Location = new System.Drawing.Point(465, 18);
            this.lblSearchLot.Name = "lblSearchLot";
            this.lblSearchLot.Size = new System.Drawing.Size(23, 14);
            this.lblSearchLot.TabIndex = 4;
            this.lblSearchLot.Text = "LOT";
            // 
            // txtSearchLot
            // 
            this.txtSearchLot.Location = new System.Drawing.Point(509, 15);
            this.txtSearchLot.Name = "txtSearchLot";
            this.txtSearchLot.Size = new System.Drawing.Size(110, 20);
            this.txtSearchLot.TabIndex = 5;
            // 
            // lblSearchZero
            // 
            this.lblSearchZero.Location = new System.Drawing.Point(641, 18);
            this.lblSearchZero.Name = "lblSearchZero";
            this.lblSearchZero.Size = new System.Drawing.Size(31, 14);
            this.lblSearchZero.TabIndex = 6;
            this.lblSearchZero.Text = "재고 0";
            // 
            // cboSearchZero
            // 
            this.cboSearchZero.Location = new System.Drawing.Point(697, 15);
            this.cboSearchZero.Name = "cboSearchZero";
            this.cboSearchZero.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboSearchZero.Properties.Items.AddRange(new object[] {
            "제외",
            "포함"});
            this.cboSearchZero.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboSearchZero.Size = new System.Drawing.Size(90, 20);
            this.cboSearchZero.TabIndex = 7;
            // 
            // paTitleH
            // 
            this.paTitleH.Appearance.BackColor = System.Drawing.Color.White;
            this.paTitleH.Appearance.Options.UseBackColor = true;
            this.paTitleH.Controls.Add(this.sectionHeaderWyn1);
            this.paTitleH.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitleH.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitleH.Location = new System.Drawing.Point(5, 0);
            this.paTitleH.Name = "paTitleH";
            this.paTitleH.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.paTitleH.Size = new System.Drawing.Size(1670, 25);
            this.paTitleH.TabIndex = 9;
            // 
            // sectionHeaderWyn1
            // 
            this.sectionHeaderWyn1.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn1.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(1665, 25);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 11;
            this.sectionHeaderWyn1.Text = "창고별재고현황 [frmStockList]";
            // 
            // paTitle1
            // 
            this.paTitle1.Appearance.BackColor = System.Drawing.Color.White;
            this.paTitle1.Appearance.Options.UseBackColor = true;
            this.paTitle1.Controls.Add(this.sectionHeaderWyn4);
            this.paTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitle1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitle1.Location = new System.Drawing.Point(0, 8);
            this.paTitle1.Name = "paTitle1";
            this.paTitle1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.paTitle1.Size = new System.Drawing.Size(1670, 27);
            this.paTitle1.TabIndex = 13;
            // 
            // sectionHeaderWyn4
            // 
            this.sectionHeaderWyn4.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn4.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn4.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn4.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn4.Name = "sectionHeaderWyn4";
            this.sectionHeaderWyn4.Size = new System.Drawing.Size(1665, 25);
            this.sectionHeaderWyn4.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn4.SvgIcon")));
            this.sectionHeaderWyn4.TabIndex = 8;
            this.sectionHeaderWyn4.Text = "창고별 재고현황 조회";
            // 
            // frmStockList
            // 
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1680, 760);
            this.Controls.Add(this.panBase);
            this.Name = "frmStockList";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelTop)).EndInit();
            this.panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchWh.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchLot.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchZero.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitleH)).EndInit();
            this.paTitleH.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.paTitle1)).EndInit();
            this.paTitle1.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    private PanelWyn panBase;
    private PanelWyn panelTop;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNo;
    private DevExpress.XtraGrid.Columns.GridColumn colItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn colItemSpec;
    private DevExpress.XtraGrid.Columns.GridColumn colWhNm;
    private DevExpress.XtraGrid.Columns.GridColumn colLocNm;
    private DevExpress.XtraGrid.Columns.GridColumn colLotNo;
    private DevExpress.XtraGrid.Columns.GridColumn colUnitCd;
    private DevExpress.XtraGrid.Columns.GridColumn colStockQty;
    private DevExpress.XtraGrid.Columns.GridColumn colTransId;
    private DevExpress.XtraGrid.Columns.GridColumn colUptDt;
    private DevExpress.XtraEditors.LabelControl lblSearchKeyword;
    private TextEditWyn txtSearchKeyword;
    private DevExpress.XtraEditors.LabelControl lblSearchWh;
    private TextEditWyn txtSearchWh;
    private DevExpress.XtraEditors.LabelControl lblSearchLot;
    private TextEditWyn txtSearchLot;
    private DevExpress.XtraEditors.LabelControl lblSearchZero;
    private DevExpress.XtraEditors.ComboBoxEdit cboSearchZero;
    private PanelWyn panHeader;
    private PanelWyn paTitle1;
    private SectionHeaderWyn sectionHeaderWyn4;
    private PanelWyn paTitleH;
    private SectionHeaderWyn sectionHeaderWyn1;
}
