// LOT계보조회(frmLotTrace) - 검색한 LOT가 속한 계보 전체(grd1, 웨이퍼 입고 LOT부터 산출 LOT까지 트리 모양) + 선택한 LOT의 이력(grd2).
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.PR;

public partial class frmLotTrace
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
        this.panWork = new WYNLAB.Base.Controls.PanelWyn();
        this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
        this.hyperlink2 = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
        this.col2Kind = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col2DocNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col2EvtDate = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col2Descr = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col2Qty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col2StatNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.shSub2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.col1LotDisp = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1ItemNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1Unit = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1InitQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1GenProc = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1RsltNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1WoNo = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1WhNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.col1StockQty = new DevExpress.XtraGrid.Columns.GridColumn();
        this.shList = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.lblSearchLotNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchLotNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblSearchWoNo = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchWoNo = new WYNLAB.Base.Controls.TextEditWyn();
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        this.panBase.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).BeginInit();
        this.panWork.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.hyperlink2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        this.panHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchLotNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchWoNo.Properties)).BeginInit();
        this.SuspendLayout();
        //
        // panBase
        //
        this.panBase.Appearance.BackColor = System.Drawing.Color.White;
        this.panBase.Appearance.Options.UseBackColor = true;
        this.panBase.Controls.Add(this.panWork);
        this.panBase.Controls.Add(this.panHeader);
        this.panBase.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panBase.Location = new System.Drawing.Point(0, 0);
        this.panBase.Name = "panBase";
        this.panBase.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
        this.panBase.Size = new System.Drawing.Size(1680, 800);
        this.panBase.TabIndex = 0;
        //
        // panWork
        //
        this.panWork.Controls.Add(this.grd2);
        this.panWork.Controls.Add(this.shSub2);
        this.panWork.Controls.Add(this.splitterWyn1);
        this.panWork.Controls.Add(this.grd1);
        this.panWork.Controls.Add(this.shList);
        this.panWork.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panWork.Location = new System.Drawing.Point(5, 49);
        this.panWork.Name = "panWork";
        this.panWork.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.panWork.Size = new System.Drawing.Size(1670, 746);
        this.panWork.TabIndex = 1;
        //
        // shList
        //
        this.shList.BackColor = System.Drawing.Color.White;
        this.shList.Dock = System.Windows.Forms.DockStyle.Top;
        this.shList.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.shList.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Grid;
        this.shList.Location = new System.Drawing.Point(3, 0);
        this.shList.Name = "shList";
        this.shList.Size = new System.Drawing.Size(1664, 27);
        this.shList.TabIndex = 0;
        this.shList.Text = "LOT 계보 (검색한 LOT가 굵게 표시됩니다 - 원 LOT부터 산출 LOT까지)";
        //
        // grd1
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Top;
        this.grd1.Location = new System.Drawing.Point(3, 27);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.Size = new System.Drawing.Size(1664, 260);
        this.grd1.TabIndex = 1;
        this.grd1.UseEmbeddedNavigator = false;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw1});
        //
        // splitterWyn1
        //
        this.splitterWyn1.BackColor = System.Drawing.Color.White;
        this.splitterWyn1.Dock = System.Windows.Forms.DockStyle.Top;
        this.splitterWyn1.Location = new System.Drawing.Point(3, 287);
        this.splitterWyn1.Name = "splitterWyn1";
        this.splitterWyn1.Size = new System.Drawing.Size(1664, 10);
        this.splitterWyn1.TabIndex = 2;
        this.splitterWyn1.TabStop = false;
        //
        // gvw1
        //
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.col1LotDisp,
        this.col1ItemNm,
        this.col1Unit,
        this.col1InitQty,
        this.col1GenProc,
        this.col1RsltNo,
        this.col1WoNo,
        this.col1WhNm,
        this.col1StockQty});
        this.gvw1.GridControl = this.grd1;
        this.gvw1.HighlightFocusedRow = true;
        this.gvw1.Name = "gvw1";
        this.gvw1.OptionsBehavior.Editable = false;
        this.gvw1.OptionsView.ColumnAutoWidth = false;
        this.gvw1.OptionsView.ShowGroupPanel = false;
        //
        // col1LotDisp
        //
        this.col1LotDisp.Caption = "LOT";
        this.col1LotDisp.FieldName = "lot_disp";
        this.col1LotDisp.Name = "col1LotDisp";
        this.col1LotDisp.Visible = true;
        this.col1LotDisp.VisibleIndex = 0;
        this.col1LotDisp.Width = 230;
        //
        // col1ItemNm
        //
        this.col1ItemNm.Caption = "품명";
        this.col1ItemNm.FieldName = "item_nm";
        this.col1ItemNm.Name = "col1ItemNm";
        this.col1ItemNm.Visible = true;
        this.col1ItemNm.VisibleIndex = 1;
        this.col1ItemNm.Width = 120;
        //
        // col1Unit
        //
        this.col1Unit.Caption = "단위";
        this.col1Unit.FieldName = "unit_cd";
        this.col1Unit.Name = "col1Unit";
        this.col1Unit.Visible = true;
        this.col1Unit.VisibleIndex = 2;
        this.col1Unit.Width = 50;
        //
        // col1InitQty
        //
        this.col1InitQty.Caption = "생성 수량";
        this.col1InitQty.DisplayFormat.FormatString = "#,##0.####";
        this.col1InitQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.col1InitQty.FieldName = "init_qty";
        this.col1InitQty.Name = "col1InitQty";
        this.col1InitQty.Visible = true;
        this.col1InitQty.VisibleIndex = 3;
        this.col1InitQty.Width = 90;
        //
        // col1GenProc
        //
        this.col1GenProc.Caption = "생성 공정";
        this.col1GenProc.FieldName = "gen_proc_nm";
        this.col1GenProc.Name = "col1GenProc";
        this.col1GenProc.Visible = true;
        this.col1GenProc.VisibleIndex = 4;
        this.col1GenProc.Width = 100;
        //
        // col1RsltNo
        //
        this.col1RsltNo.Caption = "생성 실적";
        this.col1RsltNo.FieldName = "rslt_no";
        this.col1RsltNo.Name = "col1RsltNo";
        this.col1RsltNo.Visible = true;
        this.col1RsltNo.VisibleIndex = 5;
        this.col1RsltNo.Width = 110;
        //
        // col1WoNo
        //
        this.col1WoNo.Caption = "작업지시";
        this.col1WoNo.FieldName = "wo_no";
        this.col1WoNo.Name = "col1WoNo";
        this.col1WoNo.Visible = true;
        this.col1WoNo.VisibleIndex = 6;
        this.col1WoNo.Width = 110;
        //
        // col1WhNm
        //
        this.col1WhNm.Caption = "현재 위치(창고)";
        this.col1WhNm.FieldName = "wh_nm";
        this.col1WhNm.Name = "col1WhNm";
        this.col1WhNm.Visible = true;
        this.col1WhNm.VisibleIndex = 7;
        this.col1WhNm.Width = 130;
        //
        // col1StockQty
        //
        this.col1StockQty.Caption = "현재고";
        this.col1StockQty.DisplayFormat.FormatString = "#,##0.####";
        this.col1StockQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.col1StockQty.FieldName = "stock_qty";
        this.col1StockQty.Name = "col1StockQty";
        this.col1StockQty.Visible = true;
        this.col1StockQty.VisibleIndex = 8;
        this.col1StockQty.Width = 90;
        //
        // shSub2
        //
        this.shSub2.BackColor = System.Drawing.Color.White;
        this.shSub2.Dock = System.Windows.Forms.DockStyle.Top;
        this.shSub2.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        this.shSub2.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Grid;
        this.shSub2.Location = new System.Drawing.Point(3, 297);
        this.shSub2.Name = "shSub2";
        this.shSub2.Size = new System.Drawing.Size(1664, 27);
        this.shSub2.TabIndex = 3;
        this.shSub2.Text = "선택한 LOT 이력 (입고 / 공정 산출 / 공정 투입 / 외주이전 - 문서번호를 더블클릭하면 해당 화면이 열립니다)";
        //
        // grd2
        //
        this.grd2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd2.Location = new System.Drawing.Point(3, 324);
        this.grd2.MainView = this.gvw2;
        this.grd2.Name = "grd2";
        this.grd2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
        this.hyperlink2});
        this.grd2.Size = new System.Drawing.Size(1664, 422);
        this.grd2.TabIndex = 4;
        this.grd2.UseEmbeddedNavigator = false;
        this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw2});
        //
        // gvw2
        //
        this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.col2Kind,
        this.col2DocNo,
        this.col2EvtDate,
        this.col2Descr,
        this.col2Qty,
        this.col2StatNm});
        this.gvw2.GridControl = this.grd2;
        this.gvw2.HighlightFocusedRow = true;
        this.gvw2.Name = "gvw2";
        this.gvw2.OptionsBehavior.Editable = false;
        this.gvw2.OptionsView.ColumnAutoWidth = false;
        this.gvw2.OptionsView.ShowGroupPanel = false;
        //
        // hyperlink2
        //
        this.hyperlink2.Name = "hyperlink2";
        //
        // col2Kind
        //
        this.col2Kind.Caption = "구분";
        this.col2Kind.FieldName = "evt_kind";
        this.col2Kind.Name = "col2Kind";
        this.col2Kind.Visible = true;
        this.col2Kind.VisibleIndex = 0;
        this.col2Kind.Width = 80;
        //
        // col2DocNo
        //
        this.col2DocNo.Caption = "문서번호";
        this.col2DocNo.ColumnEdit = this.hyperlink2;
        this.col2DocNo.FieldName = "doc_no";
        this.col2DocNo.Name = "col2DocNo";
        this.col2DocNo.Visible = true;
        this.col2DocNo.VisibleIndex = 1;
        this.col2DocNo.Width = 120;
        //
        // col2EvtDate
        //
        this.col2EvtDate.Caption = "일자";
        this.col2EvtDate.FieldName = "evt_date";
        this.col2EvtDate.Name = "col2EvtDate";
        this.col2EvtDate.Visible = true;
        this.col2EvtDate.VisibleIndex = 2;
        this.col2EvtDate.Width = 80;
        //
        // col2Descr
        //
        this.col2Descr.Caption = "내용";
        this.col2Descr.FieldName = "descr";
        this.col2Descr.Name = "col2Descr";
        this.col2Descr.Visible = true;
        this.col2Descr.VisibleIndex = 3;
        this.col2Descr.Width = 420;
        //
        // col2Qty
        //
        this.col2Qty.Caption = "수량";
        this.col2Qty.DisplayFormat.FormatString = "#,##0.####";
        this.col2Qty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.col2Qty.FieldName = "qty";
        this.col2Qty.Name = "col2Qty";
        this.col2Qty.Visible = true;
        this.col2Qty.VisibleIndex = 4;
        this.col2Qty.Width = 90;
        //
        // col2StatNm
        //
        this.col2StatNm.Caption = "상태";
        this.col2StatNm.FieldName = "stat_nm";
        this.col2StatNm.Name = "col2StatNm";
        this.col2StatNm.Visible = true;
        this.col2StatNm.VisibleIndex = 5;
        this.col2StatNm.Width = 80;
        //
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
        this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        this.panHeader.Controls.Add(this.lblSearchLotNo);
        this.panHeader.Controls.Add(this.txtSearchLotNo);
        this.panHeader.Controls.Add(this.lblSearchWoNo);
        this.panHeader.Controls.Add(this.txtSearchWoNo);
        this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.panHeader.Location = new System.Drawing.Point(5, 0);
        this.panHeader.Name = "panHeader";
        this.panHeader.Size = new System.Drawing.Size(1670, 49);
        this.panHeader.TabIndex = 0;
        //
        // lblSearchLotNo
        //
        this.lblSearchLotNo.Location = new System.Drawing.Point(25, 18);
        this.lblSearchLotNo.Name = "lblSearchLotNo";
        this.lblSearchLotNo.Size = new System.Drawing.Size(45, 15);
        this.lblSearchLotNo.TabIndex = 4;
        this.lblSearchLotNo.Text = "LOT번호";
        //
        // txtSearchLotNo
        //
        this.txtSearchLotNo.Location = new System.Drawing.Point(84, 15);
        this.txtSearchLotNo.Name = "txtSearchLotNo";
        this.txtSearchLotNo.Size = new System.Drawing.Size(160, 20);
        this.txtSearchLotNo.TabIndex = 5;
        //
        // lblSearchWoNo
        //
        this.lblSearchWoNo.Location = new System.Drawing.Point(268, 18);
        this.lblSearchWoNo.Name = "lblSearchWoNo";
        this.lblSearchWoNo.Size = new System.Drawing.Size(78, 15);
        this.lblSearchWoNo.TabIndex = 6;
        this.lblSearchWoNo.Text = "작업지시번호";
        //
        // txtSearchWoNo
        //
        this.txtSearchWoNo.Location = new System.Drawing.Point(360, 15);
        this.txtSearchWoNo.Name = "txtSearchWoNo";
        this.txtSearchWoNo.Size = new System.Drawing.Size(130, 20);
        this.txtSearchWoNo.TabIndex = 7;
        //
        // frmLotTrace
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1680, 800);
        this.Controls.Add(this.panBase);
        this.Name = "frmLotTrace";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        this.panBase.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).EndInit();
        this.panWork.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.hyperlink2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        this.panHeader.ResumeLayout(false);
        this.panHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchLotNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchWoNo.Properties)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panWork;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit hyperlink2;
    private DevExpress.XtraGrid.Columns.GridColumn col2Kind;
    private DevExpress.XtraGrid.Columns.GridColumn col2DocNo;
    private DevExpress.XtraGrid.Columns.GridColumn col2EvtDate;
    private DevExpress.XtraGrid.Columns.GridColumn col2Descr;
    private DevExpress.XtraGrid.Columns.GridColumn col2Qty;
    private DevExpress.XtraGrid.Columns.GridColumn col2StatNm;
    private SectionHeaderWyn shSub2;
    private SplitterWyn splitterWyn1;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn col1LotDisp;
    private DevExpress.XtraGrid.Columns.GridColumn col1ItemNm;
    private DevExpress.XtraGrid.Columns.GridColumn col1Unit;
    private DevExpress.XtraGrid.Columns.GridColumn col1InitQty;
    private DevExpress.XtraGrid.Columns.GridColumn col1GenProc;
    private DevExpress.XtraGrid.Columns.GridColumn col1RsltNo;
    private DevExpress.XtraGrid.Columns.GridColumn col1WoNo;
    private DevExpress.XtraGrid.Columns.GridColumn col1WhNm;
    private DevExpress.XtraGrid.Columns.GridColumn col1StockQty;
    private SectionHeaderWyn shList;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchLotNo;
    private TextEditWyn txtSearchLotNo;
    private DevExpress.XtraEditors.LabelControl lblSearchWoNo;
    private TextEditWyn txtSearchWoNo;
}
