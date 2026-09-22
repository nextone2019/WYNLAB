// 싱글그리드 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE) 구조를 따라 손으로 작성 - 2026-09-15.
// 사용자가 VS 디자이너에서 컨트롤 위치/크기를 직접 드래그로 조정할 수 있도록 partial class로 분리.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

public partial class frmExcRate
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExcRate));
            this.panBody = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colBaseDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dateEditBaseDate = new WYNLAB.Base.Controls.DateColumnEdit();
            this.colCurCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCurNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTtb = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTts = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnitAmt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colExcRate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRegDt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.paTitle1 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.lblFrDate = new DevExpress.XtraEditors.LabelControl();
            this.ymdFrDate = new WYNLAB.Base.Controls.DateEditWyn();
            this.ymdToDate = new WYNLAB.Base.Controls.DateEditWyn();
            this.btnReceive = new WYNLAB.Base.Controls.ButtonWyn();
            this.paTitleH = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBody)).BeginInit();
            this.panBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditBaseDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditBaseDate.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle1)).BeginInit();
            this.paTitle1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ymdFrDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ymdFrDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ymdToDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ymdToDate.Properties)).BeginInit();
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
            this.panBody.Size = new System.Drawing.Size(1235, 485);
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
            this.dateEditBaseDate});
            this.grd1.Size = new System.Drawing.Size(1235, 450);
            this.grd1.TabIndex = 1;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            // 
            // gvw1
            // 
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colBaseDate,
            this.colCurCd,
            this.colCurNm,
            this.colTtb,
            this.colTts,
            this.colUnitAmt,
            this.colExcRate,
            this.colRegDt});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsBehavior.Editable = false;
            this.gvw1.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            // 
            // colBaseDate
            // 
            this.colBaseDate.Caption = "고시일자";
            this.colBaseDate.ColumnEdit = this.dateEditBaseDate;
            this.colBaseDate.FieldName = "base_date";
            this.colBaseDate.Name = "colBaseDate";
            this.colBaseDate.Visible = true;
            this.colBaseDate.VisibleIndex = 0;
            this.colBaseDate.Width = 100;
            // 
            // dateEditBaseDate
            // 
            this.dateEditBaseDate.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditBaseDate.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditBaseDate.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dateEditBaseDate.Name = "dateEditBaseDate";
            // 
            // colCurCd
            // 
            this.colCurCd.Caption = "통화코드";
            this.colCurCd.FieldName = "cur_cd";
            this.colCurCd.Name = "colCurCd";
            this.colCurCd.Visible = true;
            this.colCurCd.VisibleIndex = 1;
            this.colCurCd.Width = 90;
            // 
            // colCurNm
            // 
            this.colCurNm.Caption = "통화명";
            this.colCurNm.FieldName = "cur_nm";
            this.colCurNm.Name = "colCurNm";
            this.colCurNm.Visible = true;
            this.colCurNm.VisibleIndex = 2;
            this.colCurNm.Width = 140;
            // 
            // colTtb
            // 
            this.colTtb.Caption = "전신환매입율";
            this.colTtb.DisplayFormat.FormatString = "N4";
            this.colTtb.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colTtb.FieldName = "ttb";
            this.colTtb.Name = "colTtb";
            this.colTtb.Visible = true;
            this.colTtb.VisibleIndex = 3;
            this.colTtb.Width = 110;
            // 
            // colTts
            // 
            this.colTts.Caption = "전신환매도율";
            this.colTts.DisplayFormat.FormatString = "N4";
            this.colTts.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colTts.FieldName = "tts";
            this.colTts.Name = "colTts";
            this.colTts.Visible = true;
            this.colTts.VisibleIndex = 4;
            this.colTts.Width = 110;
            // 
            // colUnitAmt
            // 
            this.colUnitAmt.Caption = "단위";
            this.colUnitAmt.DisplayFormat.FormatString = "N0";
            this.colUnitAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colUnitAmt.FieldName = "unit_amt";
            this.colUnitAmt.Name = "colUnitAmt";
            this.colUnitAmt.Visible = true;
            this.colUnitAmt.VisibleIndex = 5;
            this.colUnitAmt.Width = 70;
            // 
            // colExcRate
            // 
            this.colExcRate.Caption = "매매기준율";
            this.colExcRate.DisplayFormat.FormatString = "N4";
            this.colExcRate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colExcRate.FieldName = "exc_rate";
            this.colExcRate.Name = "colExcRate";
            this.colExcRate.Visible = true;
            this.colExcRate.VisibleIndex = 6;
            this.colExcRate.Width = 110;
            // 
            // colRegDt
            // 
            this.colRegDt.Caption = "등록일시";
            this.colRegDt.FieldName = "reg_dt";
            this.colRegDt.Name = "colRegDt";
            this.colRegDt.Visible = true;
            this.colRegDt.VisibleIndex = 7;
            this.colRegDt.Width = 140;
            // 
            // paTitle1
            // 
            this.paTitle1.Controls.Add(this.sectionHeaderWyn4);
            this.paTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitle1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitle1.Location = new System.Drawing.Point(0, 8);
            this.paTitle1.Name = "paTitle1";
            this.paTitle1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.paTitle1.Size = new System.Drawing.Size(1235, 27);
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
            this.sectionHeaderWyn4.Size = new System.Drawing.Size(1230, 25);
            this.sectionHeaderWyn4.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn4.SvgIcon")));
            this.sectionHeaderWyn4.TabIndex = 8;
            this.sectionHeaderWyn4.Text = "일자별 환율 List";
            // 
            // panHeader
            // 
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.lblFrDate);
            this.panHeader.Controls.Add(this.ymdFrDate);
            this.panHeader.Controls.Add(this.ymdToDate);
            this.panHeader.Controls.Add(this.btnReceive);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(5, 30);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1235, 60);
            this.panHeader.TabIndex = 0;
            // 
            // lblFrDate
            // 
            this.lblFrDate.Location = new System.Drawing.Point(8, 23);
            this.lblFrDate.Name = "lblFrDate";
            this.lblFrDate.Size = new System.Drawing.Size(40, 14);
            this.lblFrDate.TabIndex = 0;
            this.lblFrDate.Text = "조회기간";
            // 
            // ymdFrDate
            // 
            this.ymdFrDate.EditValue = null;
            this.ymdFrDate.Location = new System.Drawing.Point(70, 20);
            this.ymdFrDate.Name = "ymdFrDate";
            this.ymdFrDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ymdFrDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ymdFrDate.Size = new System.Drawing.Size(120, 20);
            this.ymdFrDate.TabIndex = 0;
            this.ymdFrDate.YyyyMmDd = null;
            // 
            // ymdToDate
            // 
            this.ymdToDate.EditValue = null;
            this.ymdToDate.Location = new System.Drawing.Point(192, 20);
            this.ymdToDate.Name = "ymdToDate";
            this.ymdToDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ymdToDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ymdToDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.ymdToDate.Size = new System.Drawing.Size(120, 20);
            this.ymdToDate.TabIndex = 1;
            this.ymdToDate.YyyyMmDd = null;
            // 
            // btnReceive
            // 
            this.btnReceive.BackColor = System.Drawing.Color.Transparent;
            this.btnReceive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnReceive.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReceive.FillColor = System.Drawing.Color.White;
            this.btnReceive.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnReceive.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnReceive.Image = null;
            this.btnReceive.Location = new System.Drawing.Point(320, 16);
            this.btnReceive.Name = "btnReceive";
            this.btnReceive.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnReceive.Size = new System.Drawing.Size(110, 28);
            this.btnReceive.TabIndex = 2;
            this.btnReceive.Text = "환율정보수신";
            this.btnReceive.ToolTip = null;
            // 
            // paTitleH
            // 
            this.paTitleH.Controls.Add(this.sectionHeaderWyn1);
            this.paTitleH.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitleH.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitleH.Location = new System.Drawing.Point(5, 5);
            this.paTitleH.Name = "paTitleH";
            this.paTitleH.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.paTitleH.Size = new System.Drawing.Size(1235, 25);
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
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(1230, 25);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 11;
            this.sectionHeaderWyn1.Text = "환율정보수신 [frmExcRate]";
            // 
            // frmExcRate
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1245, 580);
            this.Controls.Add(this.panBody);
            this.Controls.Add(this.panHeader);
            this.Controls.Add(this.paTitleH);
            this.Name = "frmExcRate";
            this.Padding = new System.Windows.Forms.Padding(5);
            ((System.ComponentModel.ISupportInitialize)(this.panBody)).EndInit();
            this.panBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditBaseDate.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditBaseDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle1)).EndInit();
            this.paTitle1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ymdFrDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ymdFrDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ymdToDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ymdToDate.Properties)).EndInit();
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
    private DevExpress.XtraEditors.LabelControl lblFrDate;
    private DateEditWyn ymdFrDate;
    private DateEditWyn ymdToDate;
    private ButtonWyn btnReceive;
    private DevExpress.XtraGrid.Columns.GridColumn colBaseDate;
    private DevExpress.XtraGrid.Columns.GridColumn colCurCd;
    private DevExpress.XtraGrid.Columns.GridColumn colCurNm;
    private DevExpress.XtraGrid.Columns.GridColumn colTtb;
    private DevExpress.XtraGrid.Columns.GridColumn colTts;
    private DevExpress.XtraGrid.Columns.GridColumn colUnitAmt;
    private DevExpress.XtraGrid.Columns.GridColumn colExcRate;
    private DevExpress.XtraGrid.Columns.GridColumn colRegDt;
    private DateColumnEdit dateEditBaseDate;
    private SectionHeaderWyn sectionHeaderWyn1;
}
