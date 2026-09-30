// 공정관리(frmProc) - 싱글그리드 구조(TBAPROC). 공정코드/공정명/정렬/사용여부/비고를 그리드에서 바로 추가/수정/삭제한다.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.PR;

public partial class frmProc
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
        this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
        this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
        this.chkcolUse = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
        this.colAccId = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colProcCd = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colProcNm = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colSort = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colUseYn = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colUsedYn = new DevExpress.XtraGrid.Columns.GridColumn();
        this.colRemark = new DevExpress.XtraGrid.Columns.GridColumn();
        this.panTool = new WYNLAB.Base.Controls.PanelWyn();
        this.btnAddRow1 = new WYNLAB.Base.Controls.ButtonWyn();
        this.btnDeletRow1 = new WYNLAB.Base.Controls.ButtonWyn();
        this.lblHint = new DevExpress.XtraEditors.LabelControl();
        this.shList = new WYNLAB.Base.Controls.SectionHeaderWyn();
        this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
        this.lblSearchKeyword = new DevExpress.XtraEditors.LabelControl();
        this.txtSearchKeyword = new WYNLAB.Base.Controls.TextEditWyn();
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
        this.panBase.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).BeginInit();
        this.panWork.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkcolUse)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panTool)).BeginInit();
        this.panTool.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
        this.panHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).BeginInit();
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
        this.panWork.Controls.Add(this.grd1);
        this.panWork.Controls.Add(this.panTool);
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
        this.shList.Text = "공정 목록";
        //
        // panTool
        //
        this.panTool.Controls.Add(this.btnAddRow1);
        this.panTool.Controls.Add(this.btnDeletRow1);
        this.panTool.Controls.Add(this.lblHint);
        this.panTool.Dock = System.Windows.Forms.DockStyle.Top;
        this.panTool.Location = new System.Drawing.Point(3, 27);
        this.panTool.Name = "panTool";
        this.panTool.Size = new System.Drawing.Size(1664, 30);
        this.panTool.TabIndex = 1;
        //
        // btnAddRow1
        //
        this.btnAddRow1.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnAddRow1.Location = new System.Drawing.Point(8, 3);
        this.btnAddRow1.Name = "btnAddRow1";
        this.btnAddRow1.Size = new System.Drawing.Size(60, 24);
        this.btnAddRow1.TabIndex = 0;
        this.btnAddRow1.Text = "행추가";
        //
        // btnDeletRow1
        //
        this.btnDeletRow1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(198)))), ((int)(((byte)(193)))));
        this.btnDeletRow1.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnDeletRow1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(236)))), ((int)(((byte)(234)))));
        this.btnDeletRow1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
        this.btnDeletRow1.Location = new System.Drawing.Point(72, 3);
        this.btnDeletRow1.Name = "btnDeletRow1";
        this.btnDeletRow1.Size = new System.Drawing.Size(60, 24);
        this.btnDeletRow1.TabIndex = 1;
        this.btnDeletRow1.Text = "행삭제";
        //
        // lblHint
        //
        this.lblHint.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
        this.lblHint.Appearance.Options.UseForeColor = true;
        this.lblHint.Location = new System.Drawing.Point(152, 8);
        this.lblHint.Name = "lblHint";
        this.lblHint.Size = new System.Drawing.Size(60, 15);
        this.lblHint.TabIndex = 2;
        this.lblHint.Text = "※ 공정코드는 신규 등록 때만 입력할 수 있고, 라우팅/작업지시에서 쓰는 공정은 삭제할 수 없습니다(사용여부를 끄세요).";
        //
        // grd1
        //
        this.grd1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grd1.Location = new System.Drawing.Point(3, 57);
        this.grd1.MainView = this.gvw1;
        this.grd1.Name = "grd1";
        this.grd1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
        this.chkcolUse});
        this.grd1.Size = new System.Drawing.Size(1664, 689);
        this.grd1.TabIndex = 2;
        this.grd1.UseEmbeddedNavigator = false;
        this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        this.gvw1});
        //
        // gvw1
        //
        this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
        this.colAccId,
        this.colProcCd,
        this.colProcNm,
        this.colSort,
        this.colUseYn,
        this.colUsedYn,
        this.colRemark});
        this.gvw1.GridControl = this.grd1;
        this.gvw1.HighlightFocusedRow = true;
        this.gvw1.Name = "gvw1";
        this.gvw1.OptionsView.ColumnAutoWidth = false;
        this.gvw1.OptionsView.ShowGroupPanel = false;
        //
        // chkcolUse
        //
        this.chkcolUse.AutoHeight = false;
        this.chkcolUse.Name = "chkcolUse";
        this.chkcolUse.ValueChecked = "Y";
        this.chkcolUse.ValueUnchecked = "N";
        //
        // colAccId
        //
        this.colAccId.Caption = "사업장";
        this.colAccId.FieldName = "acc_id";
        this.colAccId.Name = "colAccId";
        this.colAccId.Visible = false;
        //
        // colProcCd
        //
        this.colProcCd.Caption = "공정코드";
        this.colProcCd.FieldName = "proc_cd";
        this.colProcCd.Name = "colProcCd";
        this.colProcCd.Visible = true;
        this.colProcCd.VisibleIndex = 0;
        this.colProcCd.Width = 110;
        //
        // colProcNm
        //
        this.colProcNm.Caption = "공정명";
        this.colProcNm.FieldName = "proc_nm";
        this.colProcNm.Name = "colProcNm";
        this.colProcNm.Visible = true;
        this.colProcNm.VisibleIndex = 1;
        this.colProcNm.Width = 180;
        //
        // colSort
        //
        this.colSort.Caption = "정렬";
        this.colSort.DisplayFormat.FormatString = "#,##0";
        this.colSort.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        this.colSort.FieldName = "sort";
        this.colSort.Name = "colSort";
        this.colSort.Visible = true;
        this.colSort.VisibleIndex = 2;
        this.colSort.Width = 60;
        //
        // colUseYn
        //
        this.colUseYn.Caption = "사용";
        this.colUseYn.ColumnEdit = this.chkcolUse;
        this.colUseYn.FieldName = "use_yn";
        this.colUseYn.Name = "colUseYn";
        this.colUseYn.Visible = true;
        this.colUseYn.VisibleIndex = 3;
        this.colUseYn.Width = 50;
        //
        // colUsedYn
        //
        this.colUsedYn.Caption = "사용중";
        this.colUsedYn.ColumnEdit = this.chkcolUse;
        this.colUsedYn.FieldName = "used_yn";
        this.colUsedYn.Name = "colUsedYn";
        this.colUsedYn.OptionsColumn.AllowEdit = false;
        this.colUsedYn.ToolTip = "라우팅/작업지시에서 쓰고 있는 공정";
        this.colUsedYn.Visible = true;
        this.colUsedYn.VisibleIndex = 4;
        this.colUsedYn.Width = 55;
        //
        // colRemark
        //
        this.colRemark.Caption = "비고";
        this.colRemark.FieldName = "remark";
        this.colRemark.Name = "colRemark";
        this.colRemark.Visible = true;
        this.colRemark.VisibleIndex = 5;
        this.colRemark.Width = 300;
        //
        // panHeader
        //
        this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.panHeader.Appearance.Options.UseBackColor = true;
        this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        this.panHeader.Controls.Add(this.lblSearchKeyword);
        this.panHeader.Controls.Add(this.txtSearchKeyword);
        this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.panHeader.Location = new System.Drawing.Point(5, 0);
        this.panHeader.Name = "panHeader";
        this.panHeader.Size = new System.Drawing.Size(1670, 49);
        this.panHeader.TabIndex = 0;
        //
        // lblSearchKeyword
        //
        this.lblSearchKeyword.Location = new System.Drawing.Point(25, 18);
        this.lblSearchKeyword.Name = "lblSearchKeyword";
        this.lblSearchKeyword.Size = new System.Drawing.Size(60, 15);
        this.lblSearchKeyword.TabIndex = 0;
        this.lblSearchKeyword.Text = "공정코드/명";
        //
        // txtSearchKeyword
        //
        this.txtSearchKeyword.Location = new System.Drawing.Point(100, 15);
        this.txtSearchKeyword.Name = "txtSearchKeyword";
        this.txtSearchKeyword.Size = new System.Drawing.Size(180, 20);
        this.txtSearchKeyword.TabIndex = 1;
        //
        // frmProc
        //
        this.Appearance.Options.UseFont = true;
        this.ClientSize = new System.Drawing.Size(1680, 800);
        this.Controls.Add(this.panBase);
        this.Name = "frmProc";
        ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
        this.panBase.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.panWork)).EndInit();
        this.panWork.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.chkcolUse)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panTool)).EndInit();
        this.panTool.ResumeLayout(false);
        this.panTool.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
        this.panHeader.ResumeLayout(false);
        this.panHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).EndInit();
        this.ResumeLayout(false);
    }

    private PanelWyn panBase;
    private PanelWyn panWork;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkcolUse;
    private DevExpress.XtraGrid.Columns.GridColumn colAccId;
    private DevExpress.XtraGrid.Columns.GridColumn colProcCd;
    private DevExpress.XtraGrid.Columns.GridColumn colProcNm;
    private DevExpress.XtraGrid.Columns.GridColumn colSort;
    private DevExpress.XtraGrid.Columns.GridColumn colUseYn;
    private DevExpress.XtraGrid.Columns.GridColumn colUsedYn;
    private DevExpress.XtraGrid.Columns.GridColumn colRemark;
    private PanelWyn panTool;
    private ButtonWyn btnAddRow1;
    private ButtonWyn btnDeletRow1;
    private DevExpress.XtraEditors.LabelControl lblHint;
    private SectionHeaderWyn shList;
    private PanelWyn panHeader;
    private DevExpress.XtraEditors.LabelControl lblSearchKeyword;
    private TextEditWyn txtSearchKeyword;
}
