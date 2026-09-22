// AI Builder가 싱글그리드 템플릿을 복제해서 자동 생성 - 2026-09-14.
// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.
#nullable disable
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.BA;

public partial class frmCustList
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCustList));
            this.panBody = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.col1CustId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.spinEditcol1 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.col1CustNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1BizNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1Tel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1CurCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1OwnerNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1ZipCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1Addr1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1Addr2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1Homepage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1Email = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1Fax = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1BizKind = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1BizType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1TransOpenDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dateEditcol1 = new WYNLAB.Base.Controls.DateColumnEdit();
            this.col1VatType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpcol1VatType = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.col1VatRate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1Remark = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col1StatCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpColumnEdit2 = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.col1EmpId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpColumnEdit1 = new WYNLAB.Base.Controls.LookUpColumnEdit();
            this.paTitle1 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.txtCustNm_Q = new WYNLAB.Base.Controls.TextEditWyn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.paTitleH = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBody)).BeginInit();
            this.panBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditcol1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditcol1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditcol1.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcol1VatType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpColumnEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpColumnEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle1)).BeginInit();
            this.paTitle1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustNm_Q.Properties)).BeginInit();
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
            this.panBody.Location = new System.Drawing.Point(5, 83);
            this.panBody.Name = "panBody";
            this.panBody.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panBody.Size = new System.Drawing.Size(1235, 492);
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
            this.spinEditcol1,
            this.dateEditcol1,
            this.lookUpcol1VatType,
            this.lookUpColumnEdit1,
            this.lookUpColumnEdit2});
            this.grd1.Size = new System.Drawing.Size(1235, 457);
            this.grd1.TabIndex = 1;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            // 
            // gvw1
            // 
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col1CustId,
            this.col1CustNm,
            this.col1BizNo,
            this.col1Tel,
            this.col1CurCd,
            this.col1OwnerNm,
            this.col1ZipCode,
            this.col1Addr1,
            this.col1Addr2,
            this.col1Homepage,
            this.col1Email,
            this.col1Fax,
            this.col1BizKind,
            this.col1BizType,
            this.col1TransOpenDate,
            this.col1VatType,
            this.col1VatRate,
            this.col1Remark,
            this.col1StatCd,
            this.col1EmpId});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsBehavior.Editable = false;
            this.gvw1.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            // 
            // col1CustId
            // 
            this.col1CustId.Caption = "거래처ID";
            this.col1CustId.ColumnEdit = this.spinEditcol1;
            this.col1CustId.FieldName = "CUST_ID";
            this.col1CustId.Name = "col1CustId";
            this.col1CustId.OptionsColumn.ReadOnly = true;
            this.col1CustId.Visible = true;
            this.col1CustId.VisibleIndex = 0;
            this.col1CustId.Width = 60;
            // 
            // spinEditcol1
            // 
            this.spinEditcol1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEditcol1.Name = "spinEditcol1";
            // 
            // col1CustNm
            // 
            this.col1CustNm.Caption = "거래처명";
            this.col1CustNm.FieldName = "cust_nm";
            this.col1CustNm.Name = "col1CustNm";
            this.col1CustNm.OptionsColumn.ReadOnly = true;
            this.col1CustNm.Visible = true;
            this.col1CustNm.VisibleIndex = 1;
            this.col1CustNm.Width = 144;
            // 
            // col1BizNo
            // 
            this.col1BizNo.Caption = "사업자번호";
            this.col1BizNo.FieldName = "biz_no";
            this.col1BizNo.Name = "col1BizNo";
            this.col1BizNo.OptionsColumn.ReadOnly = true;
            this.col1BizNo.Visible = true;
            this.col1BizNo.VisibleIndex = 3;
            this.col1BizNo.Width = 98;
            // 
            // col1Tel
            // 
            this.col1Tel.Caption = "연락처";
            this.col1Tel.FieldName = "tel";
            this.col1Tel.Name = "col1Tel";
            this.col1Tel.OptionsColumn.ReadOnly = true;
            this.col1Tel.Visible = true;
            this.col1Tel.VisibleIndex = 8;
            this.col1Tel.Width = 114;
            // 
            // col1CurCd
            // 
            this.col1CurCd.Caption = "통화";
            this.col1CurCd.FieldName = "cur_cd";
            this.col1CurCd.Name = "col1CurCd";
            this.col1CurCd.OptionsColumn.ReadOnly = true;
            this.col1CurCd.Visible = true;
            this.col1CurCd.VisibleIndex = 4;
            this.col1CurCd.Width = 68;
            // 
            // col1OwnerNm
            // 
            this.col1OwnerNm.Caption = "대표자";
            this.col1OwnerNm.FieldName = "owner_nm";
            this.col1OwnerNm.Name = "col1OwnerNm";
            this.col1OwnerNm.OptionsColumn.ReadOnly = true;
            this.col1OwnerNm.Visible = true;
            this.col1OwnerNm.VisibleIndex = 2;
            this.col1OwnerNm.Width = 105;
            // 
            // col1ZipCode
            // 
            this.col1ZipCode.Caption = "우편번호";
            this.col1ZipCode.FieldName = "zip_code";
            this.col1ZipCode.Name = "col1ZipCode";
            this.col1ZipCode.OptionsColumn.ReadOnly = true;
            this.col1ZipCode.Visible = true;
            this.col1ZipCode.VisibleIndex = 11;
            this.col1ZipCode.Width = 76;
            // 
            // col1Addr1
            // 
            this.col1Addr1.Caption = "주소1";
            this.col1Addr1.FieldName = "addr1";
            this.col1Addr1.Name = "col1Addr1";
            this.col1Addr1.OptionsColumn.ReadOnly = true;
            this.col1Addr1.Visible = true;
            this.col1Addr1.VisibleIndex = 12;
            this.col1Addr1.Width = 241;
            // 
            // col1Addr2
            // 
            this.col1Addr2.Caption = "주소2";
            this.col1Addr2.FieldName = "addr2";
            this.col1Addr2.Name = "col1Addr2";
            this.col1Addr2.OptionsColumn.ReadOnly = true;
            this.col1Addr2.Visible = true;
            this.col1Addr2.VisibleIndex = 13;
            this.col1Addr2.Width = 261;
            // 
            // col1Homepage
            // 
            this.col1Homepage.Caption = "HomePage";
            this.col1Homepage.FieldName = "homepage";
            this.col1Homepage.Name = "col1Homepage";
            this.col1Homepage.OptionsColumn.ReadOnly = true;
            this.col1Homepage.Visible = true;
            this.col1Homepage.VisibleIndex = 14;
            this.col1Homepage.Width = 99;
            // 
            // col1Email
            // 
            this.col1Email.Caption = "E-mail";
            this.col1Email.FieldName = "email";
            this.col1Email.Name = "col1Email";
            this.col1Email.OptionsColumn.ReadOnly = true;
            this.col1Email.Visible = true;
            this.col1Email.VisibleIndex = 10;
            this.col1Email.Width = 141;
            // 
            // col1Fax
            // 
            this.col1Fax.Caption = "Fax";
            this.col1Fax.FieldName = "fax";
            this.col1Fax.Name = "col1Fax";
            this.col1Fax.OptionsColumn.ReadOnly = true;
            this.col1Fax.Visible = true;
            this.col1Fax.VisibleIndex = 9;
            this.col1Fax.Width = 104;
            // 
            // col1BizKind
            // 
            this.col1BizKind.Caption = "업종";
            this.col1BizKind.FieldName = "biz_kind";
            this.col1BizKind.Name = "col1BizKind";
            this.col1BizKind.OptionsColumn.ReadOnly = true;
            this.col1BizKind.Visible = true;
            this.col1BizKind.VisibleIndex = 15;
            this.col1BizKind.Width = 89;
            // 
            // col1BizType
            // 
            this.col1BizType.Caption = "업태";
            this.col1BizType.FieldName = "biz_type";
            this.col1BizType.Name = "col1BizType";
            this.col1BizType.OptionsColumn.ReadOnly = true;
            this.col1BizType.Visible = true;
            this.col1BizType.VisibleIndex = 16;
            this.col1BizType.Width = 105;
            // 
            // col1TransOpenDate
            // 
            this.col1TransOpenDate.Caption = "거래시작일";
            this.col1TransOpenDate.ColumnEdit = this.dateEditcol1;
            this.col1TransOpenDate.FieldName = "trans_open_date";
            this.col1TransOpenDate.Name = "col1TransOpenDate";
            this.col1TransOpenDate.OptionsColumn.ReadOnly = true;
            this.col1TransOpenDate.Visible = true;
            this.col1TransOpenDate.VisibleIndex = 17;
            this.col1TransOpenDate.Width = 90;
            // 
            // dateEditcol1
            // 
            this.dateEditcol1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditcol1.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditcol1.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dateEditcol1.Name = "dateEditcol1";
            // 
            // col1VatType
            // 
            this.col1VatType.Caption = "부가세유형";
            this.col1VatType.ColumnEdit = this.lookUpcol1VatType;
            this.col1VatType.FieldName = "vat_type";
            this.col1VatType.Name = "col1VatType";
            this.col1VatType.OptionsColumn.ReadOnly = true;
            this.col1VatType.Visible = true;
            this.col1VatType.VisibleIndex = 5;
            this.col1VatType.Width = 124;
            // 
            // lookUpcol1VatType
            // 
            this.lookUpcol1VatType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpcol1VatType.LookupKey = "L_CM0004";
            this.lookUpcol1VatType.Name = "lookUpcol1VatType";
            this.lookUpcol1VatType.NullText = "";
            this.lookUpcol1VatType.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // col1VatRate
            // 
            this.col1VatRate.Caption = "부가세율";
            this.col1VatRate.ColumnEdit = this.spinEditcol1;
            this.col1VatRate.FieldName = "vat_rate";
            this.col1VatRate.Name = "col1VatRate";
            this.col1VatRate.OptionsColumn.ReadOnly = true;
            this.col1VatRate.Visible = true;
            this.col1VatRate.VisibleIndex = 6;
            this.col1VatRate.Width = 84;
            // 
            // col1Remark
            // 
            this.col1Remark.Caption = "비고";
            this.col1Remark.FieldName = "remark";
            this.col1Remark.Name = "col1Remark";
            this.col1Remark.OptionsColumn.ReadOnly = true;
            this.col1Remark.Visible = true;
            this.col1Remark.VisibleIndex = 18;
            this.col1Remark.Width = 442;
            // 
            // col1StatCd
            // 
            this.col1StatCd.Caption = "거래처상태";
            this.col1StatCd.ColumnEdit = this.lookUpColumnEdit2;
            this.col1StatCd.FieldName = "stat_cd";
            this.col1StatCd.Name = "col1StatCd";
            this.col1StatCd.OptionsColumn.ReadOnly = true;
            this.col1StatCd.Visible = true;
            this.col1StatCd.VisibleIndex = 7;
            this.col1StatCd.Width = 79;
            // 
            // lookUpColumnEdit2
            // 
            this.lookUpColumnEdit2.AutoHeight = false;
            this.lookUpColumnEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpColumnEdit2.LookupKey = "L_BA0001";
            this.lookUpColumnEdit2.Name = "lookUpColumnEdit2";
            this.lookUpColumnEdit2.NullText = "";
            this.lookUpColumnEdit2.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            // 
            // col1EmpId
            // 
            this.col1EmpId.Caption = "거래처담당자ID";
            this.col1EmpId.ColumnEdit = this.spinEditcol1;
            this.col1EmpId.FieldName = "EMP_ID";
            this.col1EmpId.Name = "col1EmpId";
            this.col1EmpId.OptionsColumn.ReadOnly = true;
            this.col1EmpId.Width = 157;
            // 
            // lookUpColumnEdit1
            // 
            this.lookUpColumnEdit1.AutoHeight = false;
            this.lookUpColumnEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpColumnEdit1.Name = "lookUpColumnEdit1";
            this.lookUpColumnEdit1.NullText = "";
            this.lookUpColumnEdit1.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
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
            this.sectionHeaderWyn4.Text = "LIST";
            // 
            // panHeader
            // 
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.txtCustNm_Q);
            this.panHeader.Controls.Add(this.labelControl1);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(5, 30);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1235, 53);
            this.panHeader.TabIndex = 0;
            // 
            // txtCustNm_Q
            // 
            this.txtCustNm_Q.Location = new System.Drawing.Point(56, 17);
            this.txtCustNm_Q.Name = "txtCustNm_Q";
            this.txtCustNm_Q.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtCustNm_Q.Size = new System.Drawing.Size(178, 20);
            this.txtCustNm_Q.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(14, 20);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 15);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "거래처";
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
            this.sectionHeaderWyn1.TabIndex = 14;
            this.sectionHeaderWyn1.Text = "거래처현황 [frmCustList]";
            this.sectionHeaderWyn1.Click += new System.EventHandler(this.sectionHeaderWyn1_Click);
            // 
            // frmCustList
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1245, 580);
            this.Controls.Add(this.panBody);
            this.Controls.Add(this.panHeader);
            this.Controls.Add(this.paTitleH);
            this.Name = "frmCustList";
            this.Padding = new System.Windows.Forms.Padding(5);
            ((System.ComponentModel.ISupportInitialize)(this.panBody)).EndInit();
            this.panBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditcol1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditcol1.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditcol1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpcol1VatType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpColumnEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpColumnEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle1)).EndInit();
            this.paTitle1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panHeader)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustNm_Q.Properties)).EndInit();
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
    private DevExpress.XtraGrid.Columns.GridColumn col1CustId;
    private DevExpress.XtraGrid.Columns.GridColumn col1CustNm;
    private DevExpress.XtraGrid.Columns.GridColumn col1BizNo;
    private DevExpress.XtraGrid.Columns.GridColumn col1Tel;
    private DevExpress.XtraGrid.Columns.GridColumn col1CurCd;
    private DevExpress.XtraGrid.Columns.GridColumn col1OwnerNm;
    private DevExpress.XtraGrid.Columns.GridColumn col1ZipCode;
    private DevExpress.XtraGrid.Columns.GridColumn col1Addr1;
    private DevExpress.XtraGrid.Columns.GridColumn col1Addr2;
    private DevExpress.XtraGrid.Columns.GridColumn col1Homepage;
    private DevExpress.XtraGrid.Columns.GridColumn col1Email;
    private DevExpress.XtraGrid.Columns.GridColumn col1Fax;
    private DevExpress.XtraGrid.Columns.GridColumn col1BizKind;
    private DevExpress.XtraGrid.Columns.GridColumn col1BizType;
    private DevExpress.XtraGrid.Columns.GridColumn col1TransOpenDate;
    private DevExpress.XtraGrid.Columns.GridColumn col1VatType;
    private DevExpress.XtraGrid.Columns.GridColumn col1VatRate;
    private DevExpress.XtraGrid.Columns.GridColumn col1Remark;
    private DevExpress.XtraGrid.Columns.GridColumn col1StatCd;
    private DevExpress.XtraGrid.Columns.GridColumn col1EmpId;
    private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spinEditcol1;
    private WYNLAB.Base.Controls.DateColumnEdit dateEditcol1;
    private LookUpColumnEdit lookUpcol1VatType;
    private SectionHeaderWyn sectionHeaderWyn1;
    private TextEditWyn txtCustNm_Q;
    private DevExpress.XtraEditors.LabelControl labelControl1;
    private LookUpColumnEdit lookUpColumnEdit2;
    private LookUpColumnEdit lookUpColumnEdit1;
}
