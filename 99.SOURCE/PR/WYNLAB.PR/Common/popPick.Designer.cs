// 범용 불러오기 팝업(popPick) 디자이너 - 검색 패널 + 체크 그리드 + 하단 버튼. 컬럼은 부르는 화면이 popPick.cs의 Pick()으로 넘긴다.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.PR;

public partial class popPick
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.lblDate = new DevExpress.XtraEditors.LabelControl();
        this.dteFrom = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblTilde = new DevExpress.XtraEditors.LabelControl();
        this.dteTo = new WYNLAB.Base.Controls.DateEditWyn();
        this.lblDocNo = new DevExpress.XtraEditors.LabelControl();
        this.txtDocNo = new WYNLAB.Base.Controls.TextEditWyn();
        this.lblKeyword = new DevExpress.XtraEditors.LabelControl();
        this.txtKeyword = new WYNLAB.Base.Controls.TextEditWyn();
        this.btnSearch = new WYNLAB.Base.Controls.ButtonWyn();
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.colSel = new DevExpress.XtraGrid.Columns.GridColumn();
        this.chkSel = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
        this.panFooter = new WYNLAB.Base.Controls.PanelWyn();
        this.chkAll = new DevExpress.XtraEditors.CheckEdit();
        this.lblHint = new DevExpress.XtraEditors.LabelControl();
        this.btnOk = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnClose = new WYNLAB.Base.Controls.ButtonWyn();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        this.panHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dteFrom.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteFrom.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteTo.Properties.CalendarTimeProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteTo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDocNo.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtKeyword.Properties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkSel)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panFooter)).BeginInit();
        this.panFooter.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.chkAll.Properties)).BeginInit();
        this.SuspendLayout();
        //
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
        this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        this.panHeader.Controls.Add(this.lblDate);
        this.panHeader.Controls.Add(this.dteFrom);
        this.panHeader.Controls.Add(this.lblTilde);
        this.panHeader.Controls.Add(this.dteTo);
        this.panHeader.Controls.Add(this.lblDocNo);
        this.panHeader.Controls.Add(this.txtDocNo);
        this.panHeader.Controls.Add(this.lblKeyword);
        this.panHeader.Controls.Add(this.txtKeyword);
        this.panHeader.Controls.Add(this.btnSearch);
        this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.panHeader.Location = new System.Drawing.Point(0, 0);
        this.panHeader.Name = "panHeader";
        this.panHeader.Size = new System.Drawing.Size(1000, 46);
        this.panHeader.TabIndex = 0;
        //
        // lblDate
        //
        this.lblDate.Location = new System.Drawing.Point(28, 14);
        this.lblDate.Name = "lblDate";
        this.lblDate.Size = new System.Drawing.Size(48, 15);
        this.lblDate.TabIndex = 0;
        this.lblDate.Text = "작업일자";
        //
        // dteFrom
        //
        this.dteFrom.Location = new System.Drawing.Point(90, 11);
        this.dteFrom.Name = "dteFrom";
        this.dteFrom.Size = new System.Drawing.Size(110, 20);
        this.dteFrom.TabIndex = 1;
        //
        // lblTilde
        //
        this.lblTilde.Location = new System.Drawing.Point(206, 14);
        this.lblTilde.Name = "lblTilde";
        this.lblTilde.Size = new System.Drawing.Size(7, 15);
        this.lblTilde.TabIndex = 2;
        this.lblTilde.Text = "~";
        //
        // dteTo
        //
        this.dteTo.Location = new System.Drawing.Point(219, 11);
        this.dteTo.Name = "dteTo";
        this.dteTo.Size = new System.Drawing.Size(110, 20);
        this.dteTo.TabIndex = 3;
        //
        // lblDocNo
        //
        this.lblDocNo.Location = new System.Drawing.Point(352, 14);
        this.lblDocNo.Name = "lblDocNo";
        this.lblDocNo.Size = new System.Drawing.Size(66, 15);
        this.lblDocNo.TabIndex = 4;
        this.lblDocNo.Text = "문서번호";
        //
        // txtDocNo
        //
        this.txtDocNo.Location = new System.Drawing.Point(432, 11);
        this.txtDocNo.Name = "txtDocNo";
        this.txtDocNo.Size = new System.Drawing.Size(130, 20);
        this.txtDocNo.TabIndex = 5;
        //
        // lblKeyword
        //
        this.lblKeyword.Location = new System.Drawing.Point(584, 14);
        this.lblKeyword.Name = "lblKeyword";
        this.lblKeyword.Size = new System.Drawing.Size(72, 15);
        this.lblKeyword.TabIndex = 6;
        this.lblKeyword.Text = "LOT/품번/품명";
        //
        // txtKeyword
        //
        this.txtKeyword.Location = new System.Drawing.Point(668, 11);
        this.txtKeyword.Name = "txtKeyword";
        this.txtKeyword.Size = new System.Drawing.Size(160, 20);
        this.txtKeyword.TabIndex = 7;
        //
        // btnSearch
        //
        this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnSearch.Location = new System.Drawing.Point(848, 9);
        this.btnSearch.Name = "btnSearch";
        this.btnSearch.Size = new System.Drawing.Size(80, 24);
        this.btnSearch.TabIndex = 8;
        this.btnSearch.Text = "조회";
        //
        // grd1
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd1.Location = new System.Drawing.Point(0, 46);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
        this.chkSel});
        this.grd1.Size = new System.Drawing.Size(1000, 422);
        this.grd1.TabIndex = 1;
        this.grd1.UseEmbeddedNavigator = false;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw1});
        //
        // gvw1
        //
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colSel});
        this.gvw1.GridControl = this.grd1;
        this.gvw1.HighlightFocusedRow = true;
        this.gvw1.Name = "gvw1";
        this.gvw1.OptionsView.ColumnAutoWidth = false;
        this.gvw1.OptionsView.ShowGroupPanel = false;
        //
        // colSel
        //
        this.colSel.Caption = "선택";
        this.colSel.ColumnEdit = this.chkSel;
        this.colSel.FieldName = "sel";
        this.colSel.Name = "colSel";
        this.colSel.Visible = true;
        this.colSel.VisibleIndex = 0;
        this.colSel.Width = 45;
        //
        // chkSel
        //
        this.chkSel.AutoHeight = false;
        this.chkSel.Name = "chkSel";
        //
        // panFooter
        //
        this.panFooter.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.panFooter.Appearance.Options.UseBackColor = true;
        this.panFooter.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        this.panFooter.Controls.Add(this.chkAll);
        this.panFooter.Controls.Add(this.lblHint);
        this.panFooter.Controls.Add(this.btnOk);
        this.panFooter.Controls.Add(this.btnClose);
        this.panFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.panFooter.Location = new System.Drawing.Point(0, 468);
        this.panFooter.Name = "panFooter";
        this.panFooter.Size = new System.Drawing.Size(1000, 44);
        this.panFooter.TabIndex = 2;
        //
        // chkAll
        //
        this.chkAll.Location = new System.Drawing.Point(16, 12);
        this.chkAll.Name = "chkAll";
        this.chkAll.Properties.Caption = "전체 선택";
        this.chkAll.Size = new System.Drawing.Size(80, 20);
        this.chkAll.TabIndex = 0;
        //
        // lblHint
        //
        this.lblHint.Location = new System.Drawing.Point(112, 15);
        this.lblHint.Name = "lblHint";
        this.lblHint.Size = new System.Drawing.Size(60, 15);
        this.lblHint.TabIndex = 1;
        this.lblHint.Text = "체크한 행을 불러옵니다.";
        //
        // btnOk
        //
        this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnOk.Location = new System.Drawing.Point(796, 10);
        this.btnOk.Name = "btnOk";
        this.btnOk.Size = new System.Drawing.Size(90, 24);
        this.btnOk.TabIndex = 2;
        this.btnOk.Text = "불러오기";
        //
        // btnClose
        //
        this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnClose.Location = new System.Drawing.Point(894, 10);
        this.btnClose.Name = "btnClose";
        this.btnClose.Size = new System.Drawing.Size(90, 24);
        this.btnClose.TabIndex = 3;
        this.btnClose.Text = "닫기";
        //
        // popPick
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1000, 512);
        this.Controls.Add(this.grd1);
        this.Controls.Add(this.panFooter);
        this.Controls.Add(this.panHeader);
        this.MinimizeBox = false;
        this.Name = "popPick";
        this.ShowIcon = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "불러오기";
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        this.panHeader.ResumeLayout(false);
        this.panHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dteFrom.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteFrom.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteTo.Properties.CalendarTimeProperties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dteTo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtDocNo.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.txtKeyword.Properties)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkSel)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panFooter)).EndInit();
        this.panFooter.ResumeLayout(false);
        this.panFooter.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.chkAll.Properties)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblDate;
    private DateEditWyn dteFrom;
    private DevExpress.XtraEditors.LabelControl lblTilde;
    private DateEditWyn dteTo;
    private DevExpress.XtraEditors.LabelControl lblDocNo;
    private TextEditWyn txtDocNo;
    private DevExpress.XtraEditors.LabelControl lblKeyword;
    private TextEditWyn txtKeyword;
    private ButtonWyn btnSearch;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colSel;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkSel;
    private PanelWyn panFooter;
    private DevExpress.XtraEditors.CheckEdit chkAll;
    private DevExpress.XtraEditors.LabelControl lblHint;
    private ButtonWyn btnOk;
    private ButtonWyn btnClose;
}
