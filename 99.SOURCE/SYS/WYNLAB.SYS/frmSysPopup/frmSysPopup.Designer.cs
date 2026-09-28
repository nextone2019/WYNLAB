// VS ?붿옄?대꼫媛 ?먮룞 ?앹꽦?섎뒗 ?꾨뱶 ?좎뼵?먮뒗 = null!????遺숈뿬??nullable 寃쎄퀬(CS8618)媛
// 怨꾩냽 ?섍린 ?뚮Ц?? ???뚯씪(?붿옄?대꼫 ?꾩슜)留?nullable 寃?щ? ?덈떎 - ?뷀븳 愿濡. ???꾨줈?앺듃瑜?// 蹂듭궗?댁꽌 ???붾㈃??留뚮뱾 ?뚮룄 洹??붾㈃??Designer.cs 留??꾩뿉 ??以꾩쓣 洹몃?濡??좎???寃?
#nullable disable
using DevExpress.XtraGrid;
using WYNLAB.Base.Controls;

namespace WYNLAB.SYS;

/// <summary>
/// ???붾㈃ 媛쒕컻???쒗뵆由?- frmMinorCode(湲곗큹肄붾뱶?깅줉)?먯꽌 ?쒖? ?덉씠?꾩썐(?쒕ぉ諛?+ 寃?됲뙣??+
/// 醫뚯슦 ?ㅽ뵆由ы꽣 + 紐⑸줉洹몃━???곸꽭?⑤꼸/?섏쐞洹몃━??留??④린怨?洹??붾㈃ ?꾩슜 而щ읆/?낅젰而⑦듃濡ㅼ?
/// ?꾨? 吏??寃? ?ъ슜踰?
///   1. ???꾨줈?앺듃 ?대뜑(WYNLAB.SYS)瑜????붾㈃ ?꾨줈?앺듃濡?蹂듭궗
///   2. 蹂듭궗???대뜑 ?덉뿉??frmSysPopup.cs / .Designer.cs / .resx ?뚯씪紐낆쓣 ???붾㈃紐낆쑝濡?///      諛붽씀怨? 洹??덉쓽 ?대옒?ㅻ챸(frmSysPopup)쨌?ㅼ엫?ㅽ럹?댁뒪(WYNLAB.SYS)????///      ?붾㈃紐?WYNLAB.SM?쇰줈 諛붽씔??- 諛섎뱶???뚯씪?????대쫫?쇰줈 諛붽씔 "?ㅼ쓬?? ?대옒?ㅻ챸??///      諛붽? 寃?癒쇱? rename 由ы뙥?곕????곕㈃ ?먮낯怨??대쫫??寃뱀퀜 而댄뙆???먮윭媛 ?쒕떎 - ?ㅼ젣濡?///      寃れ쓬, frmUserAuth 留뚮뱾 ?????쒖꽌瑜???吏耳쒖꽌 1142媛??먮윭媛 ?ъ뿀??.
///   3. grd1(紐⑸줉)/panData(?곸꽭 ?낅젰)/grd2(?섏쐞 紐⑸줉) ?먮━???붿옄?대꼫濡?而щ읆/而⑦듃濡ㅼ쓣 諛곗튂
///   4. ?꾩꽦?섎㈃ ??WYNLAB.SYS ?꾨줈?앺듃瑜?WYNLAB.SM.sln?먯꽌 鍮쇨퀬 ?ㅼ젣 ?붾㈃
///      ?꾨줈?앺듃瑜??붾（?섏뿉 異붽?(?먮뒗 WYNLAB.SM 紐⑤뱢 ?꾨줈?앺듃 諛??대뜑濡???꺼 ?ｊ린)
/// </summary>
public partial class frmSysPopup
{
    private System.ComponentModel.IContainer components = null;
    private PanelWyn panBase = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSysPopup));
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn3 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn4 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd2 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw2 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colColumnNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCaption = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colControlType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBoxControlType = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.colLookupProcNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSort = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWidth = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVisibleYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEditVisible = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.panelWyn1 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn7 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnDeletRow2 = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnAddRow2 = new WYNLAB.Base.Controls.ButtonWyn();
            this.sectionHeaderWyn2 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.grd3 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw3 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colParamNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSearchCaption = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSearchControlType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBoxSearchControlType = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.colSearchSort = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSearchWidth = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSearchLookupKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSearchRowNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSearchControlNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBoxSearchControlNm = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.panelWyn9 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn10 = new WYNLAB.Base.Controls.PanelWyn();
            this.btnDeletRow3 = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnAddRow3 = new WYNLAB.Base.Controls.ButtonWyn();
            this.sectionHeaderWyn5 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panelWyn5 = new WYNLAB.Base.Controls.PanelWyn();
            this.panData = new WYNLAB.Base.Controls.PanelWyn();
            this.lblPopupKey = new DevExpress.XtraEditors.LabelControl();
            this.txtPopupKey = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblPopupNm = new DevExpress.XtraEditors.LabelControl();
            this.txtPopupNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblProcNm = new DevExpress.XtraEditors.LabelControl();
            this.txtProcNm = new WYNLAB.Base.Controls.TextEditWyn();
            this.btnGenerateColumns = new WYNLAB.Base.Controls.ButtonWyn();
            this.btnCopyPopup = new WYNLAB.Base.Controls.ButtonWyn();
            this.chkHierarchical = new WYNLAB.Base.Controls.CheckBoxWyn();
            this.lblKeyField = new DevExpress.XtraEditors.LabelControl();
            this.txtKeyField = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblParentField = new DevExpress.XtraEditors.LabelControl();
            this.txtParentField = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblDisplayField = new DevExpress.XtraEditors.LabelControl();
            this.txtDisplayField = new WYNLAB.Base.Controls.TextEditWyn();
            this.chkUseYn = new WYNLAB.Base.Controls.CheckBoxWyn();
            this.lblPopupWidth = new DevExpress.XtraEditors.LabelControl();
            this.txtPopupWidth = new WYNLAB.Base.Controls.TextEditWyn();
            this.txtPopupHeight = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblRemark = new DevExpress.XtraEditors.LabelControl();
            this.panelWyn6 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn3 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelWyn8 = new WYNLAB.Base.Controls.PanelWyn();
            this.grd1 = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvw1 = new WYNLAB.Base.Controls.GridViewWyn();
            this.colPopupKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPopupNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProcNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colHierarchical = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelWyn2 = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn4 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.panHeader = new WYNLAB.Base.Controls.PanelWyn();
            this.txtSearchQ = new WYNLAB.Base.Controls.TextEditWyn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.paTitle = new WYNLAB.Base.Controls.PanelWyn();
            this.sectionHeaderWyn1 = new WYNLAB.Base.Controls.SectionHeaderWyn();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtRemark = new WYNLAB.Base.Controls.MemoEditWyn();
            this.lblSearchPanel = new DevExpress.XtraEditors.LabelControl();
            this.txtSearchPanelClass = new WYNLAB.Base.Controls.TextEditWyn();
            this.panelWyn11 = new WYNLAB.Base.Controls.PanelWyn();
            this.panelWyn12 = new WYNLAB.Base.Controls.PanelWyn();
            this.splitterWyn2 = new WYNLAB.Base.Controls.SplitterWyn();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
            this.panBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).BeginInit();
            this.panelWyn3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).BeginInit();
            this.panelWyn4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxControlType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEditVisible)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).BeginInit();
            this.panelWyn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).BeginInit();
            this.panelWyn7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxSearchControlType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxSearchControlNm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn9)).BeginInit();
            this.panelWyn9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn10)).BeginInit();
            this.panelWyn10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).BeginInit();
            this.panelWyn5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panData)).BeginInit();
            this.panData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPopupKey.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPopupNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProcNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkHierarchical.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKeyField.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParentField.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDisplayField.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkUseYn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPopupWidth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPopupHeight.Properties)).BeginInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchQ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).BeginInit();
            this.paTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchPanelClass.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn11)).BeginInit();
            this.panelWyn11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn12)).BeginInit();
            this.panelWyn12.SuspendLayout();
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
            this.panBase.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.panBase.Size = new System.Drawing.Size(1487, 600);
            this.panBase.TabIndex = 5;
            // 
            // panelWyn3
            // 
            this.panelWyn3.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn3.Appearance.Options.UseBackColor = true;
            this.panelWyn3.Controls.Add(this.panelWyn4);
            this.panelWyn3.Controls.Add(this.splitterWyn1);
            this.panelWyn3.Controls.Add(this.panelWyn8);
            this.panelWyn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn3.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn3.Location = new System.Drawing.Point(3, 82);
            this.panelWyn3.Name = "panelWyn3";
            this.panelWyn3.Size = new System.Drawing.Size(1481, 515);
            this.panelWyn3.TabIndex = 7;
            // 
            // panelWyn4
            // 
            this.panelWyn4.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn4.Appearance.Options.UseBackColor = true;
            this.panelWyn4.Controls.Add(this.panelWyn11);
            this.panelWyn4.Controls.Add(this.splitterWyn2);
            this.panelWyn4.Controls.Add(this.panelWyn12);
            this.panelWyn4.Controls.Add(this.panelWyn5);
            this.panelWyn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn4.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn4.Location = new System.Drawing.Point(412, 0);
            this.panelWyn4.Name = "panelWyn4";
            this.panelWyn4.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.panelWyn4.Size = new System.Drawing.Size(1069, 515);
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
            this.grd2.Location = new System.Drawing.Point(0, 27);
            this.grd2.MainView = this.gvw2;
            this.grd2.Name = "grd2";
            this.grd2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBoxControlType,
            this.repositoryItemCheckEditVisible});
            this.grd2.Size = new System.Drawing.Size(1066, 71);
            this.grd2.TabIndex = 7;
            this.grd2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw2});
            // 
            // gvw2
            // 
            this.gvw2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colColumnNm,
            this.colCaption,
            this.colControlType,
            this.colLookupProcNm,
            this.colSort,
            this.colWidth,
            this.colVisibleYn});
            this.gvw2.GridControl = this.grd2;
            this.gvw2.HighlightFocusedRow = true;
            this.gvw2.Name = "gvw2";
            this.gvw2.OptionsBehavior.Editable = false;
            this.gvw2.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw2.OptionsView.ColumnAutoWidth = false;
            this.gvw2.OptionsView.ShowGroupPanel = false;
            // 
            // colColumnNm
            // 
            this.colColumnNm.Caption = "컬럼명";
            this.colColumnNm.FieldName = "column_nm";
            this.colColumnNm.Name = "colColumnNm";
            this.colColumnNm.OptionsColumn.AllowEdit = false;
            this.colColumnNm.Visible = true;
            this.colColumnNm.VisibleIndex = 0;
            this.colColumnNm.Width = 120;
            // 
            // colCaption
            // 
            this.colCaption.Caption = "罹≪뀡";
            this.colCaption.FieldName = "caption";
            this.colCaption.Name = "colCaption";
            this.colCaption.Visible = true;
            this.colCaption.VisibleIndex = 1;
            this.colCaption.Width = 100;
            // 
            // colControlType
            // 
            this.colControlType.Caption = "컨트롤타입";
            this.colControlType.ColumnEdit = this.repositoryItemComboBoxControlType;
            this.colControlType.FieldName = "control_type";
            this.colControlType.Name = "colControlType";
            this.colControlType.Visible = true;
            this.colControlType.VisibleIndex = 2;
            this.colControlType.Width = 90;
            // 
            // repositoryItemComboBoxControlType
            // 
            this.repositoryItemComboBoxControlType.AutoHeight = false;
            this.repositoryItemComboBoxControlType.Items.AddRange(new object[] {
            "TEXT",
            "DATE",
            "LOOKUP",
            "CHECK"});
            this.repositoryItemComboBoxControlType.Name = "repositoryItemComboBoxControlType";
            this.repositoryItemComboBoxControlType.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // colLookupProcNm
            // 
            this.colLookupProcNm.Caption = "Lookup 프로시저";
            this.colLookupProcNm.FieldName = "lookup_proc_nm";
            this.colLookupProcNm.Name = "colLookupProcNm";
            this.colLookupProcNm.Visible = true;
            this.colLookupProcNm.VisibleIndex = 3;
            this.colLookupProcNm.Width = 140;
            // 
            // colSort
            // 
            this.colSort.Caption = "순서";
            this.colSort.FieldName = "sort";
            this.colSort.Name = "colSort";
            this.colSort.Visible = true;
            this.colSort.VisibleIndex = 4;
            this.colSort.Width = 50;
            // 
            // colWidth
            // 
            this.colWidth.Caption = "폭(px)";
            this.colWidth.FieldName = "width";
            this.colWidth.Name = "colWidth";
            this.colWidth.Visible = true;
            this.colWidth.VisibleIndex = 5;
            this.colWidth.Width = 60;
            // 
            // colVisibleYn
            // 
            this.colVisibleYn.Caption = "표시";
            this.colVisibleYn.ColumnEdit = this.repositoryItemCheckEditVisible;
            this.colVisibleYn.FieldName = "visible_yn";
            this.colVisibleYn.Name = "colVisibleYn";
            this.colVisibleYn.Visible = true;
            this.colVisibleYn.VisibleIndex = 6;
            this.colVisibleYn.Width = 50;
            // 
            // repositoryItemCheckEditVisible
            // 
            this.repositoryItemCheckEditVisible.AutoHeight = false;
            this.repositoryItemCheckEditVisible.Name = "repositoryItemCheckEditVisible";
            this.repositoryItemCheckEditVisible.ValueChecked = "Y";
            this.repositoryItemCheckEditVisible.ValueUnchecked = "N";
            // 
            // panelWyn1
            // 
            this.panelWyn1.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn1.Appearance.Options.UseBackColor = true;
            this.panelWyn1.Controls.Add(this.sectionHeaderWyn2);
            this.panelWyn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn1.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn1.Location = new System.Drawing.Point(0, 0);
            this.panelWyn1.Name = "panelWyn1";
            this.panelWyn1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn1.Size = new System.Drawing.Size(1066, 27);
            this.panelWyn1.TabIndex = 8;
            // 
            // panelWyn7
            // 
            this.panelWyn7.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn7.Appearance.Options.UseBackColor = true;
            this.panelWyn7.Controls.Add(this.btnDeletRow2);
            this.panelWyn7.Controls.Add(this.btnAddRow2);
            this.panelWyn7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn7.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn7.Location = new System.Drawing.Point(998, 0);
            this.panelWyn7.Name = "panelWyn7";
            this.panelWyn7.Padding = new System.Windows.Forms.Padding(5, 3, 0, 2);
            this.panelWyn7.Size = new System.Drawing.Size(68, 30);
            this.panelWyn7.TabIndex = 9;
            // 
            // btnDeletRow2
            // 
            this.btnDeletRow2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeletRow2.BackColor = System.Drawing.Color.Transparent;
            this.btnDeletRow2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(198)))), ((int)(((byte)(193)))));
            this.btnDeletRow2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeletRow2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(236)))), ((int)(((byte)(234)))));
            this.btnDeletRow2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDeletRow2.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnDeletRow2.Image = ((System.Drawing.Image)(resources.GetObject("btnDeletRow2.Image")));
            this.btnDeletRow2.Location = new System.Drawing.Point(68, 4);
            this.btnDeletRow2.Name = "btnDeletRow2";
            this.btnDeletRow2.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDeletRow2.Size = new System.Drawing.Size(58, 22);
            this.btnDeletRow2.Text = "";
            this.btnDeletRow2.TabIndex = 0;
            this.btnDeletRow2.ToolTip = "행삭제";
            this.btnDeletRow2.Click += new System.EventHandler(this.btnDeletRow2_Click);
            // 
            // btnAddRow2
            // 
            this.btnAddRow2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddRow2.BackColor = System.Drawing.Color.Transparent;
            this.btnAddRow2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(231)))), ((int)(((byte)(203)))));
            this.btnAddRow2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddRow2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(245)))), ((int)(((byte)(231)))));
            this.btnAddRow2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnAddRow2.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnAddRow2.Image = ((System.Drawing.Image)(resources.GetObject("btnAddRow2.Image")));
            this.btnAddRow2.Location = new System.Drawing.Point(6, 4);
            this.btnAddRow2.Name = "btnAddRow2";
            this.btnAddRow2.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnAddRow2.Size = new System.Drawing.Size(58, 22);
            this.btnAddRow2.Text = "";
            this.btnAddRow2.TabIndex = 0;
            this.btnAddRow2.ToolTip = "행추가";
            this.btnAddRow2.Click += new System.EventHandler(this.btnAddRow2_Click);
            // 
            // sectionHeaderWyn2
            // 
            this.sectionHeaderWyn2.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn2.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn2.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn2.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn2.Name = "sectionHeaderWyn2";
            this.sectionHeaderWyn2.Size = new System.Drawing.Size(1061, 25);
            this.sectionHeaderWyn2.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn2.SvgIcon")));
            this.sectionHeaderWyn2.TabIndex = 8;
            this.sectionHeaderWyn2.Text = "컬럼 설정";
            // 
            // grd3
            // 
            this.grd3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd3.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grd3.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grd3.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grd3.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grd3.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grd3.Location = new System.Drawing.Point(0, 27);
            this.grd3.MainView = this.gvw3;
            this.grd3.Name = "grd3";
            this.grd3.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBoxSearchControlType,
            this.repositoryItemComboBoxSearchControlNm});
            this.grd3.Size = new System.Drawing.Size(1066, 253);
            this.grd3.TabIndex = 11;
            this.grd3.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw3});
            // 
            // gvw3
            // 
            this.gvw3.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colParamNm,
            this.colSearchCaption,
            this.colSearchControlType,
            this.colSearchSort,
            this.colSearchWidth,
            this.colSearchLookupKey,
            this.colSearchRowNo,
            this.colSearchControlNm});
            this.gvw3.GridControl = this.grd3;
            this.gvw3.HighlightFocusedRow = true;
            this.gvw3.Name = "gvw3";
            this.gvw3.OptionsBehavior.Editable = false;
            this.gvw3.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw3.OptionsView.ColumnAutoWidth = false;
            this.gvw3.OptionsView.ShowGroupPanel = false;
            // 
            // colParamNm
            // 
            this.colParamNm.Caption = "파라미터명";
            this.colParamNm.FieldName = "param_nm";
            this.colParamNm.Name = "colParamNm";
            this.colParamNm.OptionsColumn.AllowEdit = false;
            this.colParamNm.Visible = true;
            this.colParamNm.VisibleIndex = 0;
            this.colParamNm.Width = 110;
            // 
            // colSearchCaption
            // 
            this.colSearchCaption.Caption = "라벨";
            this.colSearchCaption.FieldName = "caption";
            this.colSearchCaption.Name = "colSearchCaption";
            this.colSearchCaption.Visible = true;
            this.colSearchCaption.VisibleIndex = 1;
            this.colSearchCaption.Width = 140;
            // 
            // colSearchControlType
            // 
            this.colSearchControlType.Caption = "컨트롤타입";
            this.colSearchControlType.ColumnEdit = this.repositoryItemComboBoxSearchControlType;
            this.colSearchControlType.FieldName = "control_type";
            this.colSearchControlType.Name = "colSearchControlType";
            this.colSearchControlType.Visible = true;
            this.colSearchControlType.VisibleIndex = 2;
            this.colSearchControlType.Width = 90;
            // 
            // repositoryItemComboBoxSearchControlType
            // 
            this.repositoryItemComboBoxSearchControlType.AutoHeight = false;
            this.repositoryItemComboBoxSearchControlType.Items.AddRange(new object[] {
            "TEXT",
            "DATE",
            "LOOKUP"});
            this.repositoryItemComboBoxSearchControlType.Name = "repositoryItemComboBoxSearchControlType";
            this.repositoryItemComboBoxSearchControlType.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // colSearchSort
            // 
            this.colSearchSort.Caption = "순서";
            this.colSearchSort.FieldName = "sort";
            this.colSearchSort.Name = "colSearchSort";
            this.colSearchSort.Visible = true;
            this.colSearchSort.VisibleIndex = 3;
            this.colSearchSort.Width = 50;
            // 
            // colSearchWidth
            // 
            this.colSearchWidth.Caption = "입력창 폭(px)";
            this.colSearchWidth.FieldName = "width";
            this.colSearchWidth.Name = "colSearchWidth";
            this.colSearchWidth.Visible = true;
            this.colSearchWidth.VisibleIndex = 4;
            this.colSearchWidth.Width = 80;
            // 
            // colSearchLookupKey
            // 
            this.colSearchLookupKey.Caption = "LookUp??";
            this.colSearchLookupKey.FieldName = "lookup_key";
            this.colSearchLookupKey.Name = "colSearchLookupKey";
            this.colSearchLookupKey.Visible = true;
            this.colSearchLookupKey.VisibleIndex = 5;
            this.colSearchLookupKey.Width = 100;
            // 
            // colSearchRowNo
            // 
            this.colSearchRowNo.Caption = "以꾨쾲??";
            this.colSearchRowNo.FieldName = "row_no";
            this.colSearchRowNo.Name = "colSearchRowNo";
            this.colSearchRowNo.Visible = true;
            this.colSearchRowNo.VisibleIndex = 6;
            this.colSearchRowNo.Width = 50;
            //
            // colSearchControlNm
            //
            this.colSearchControlNm.Caption = "?⑤꼸 而⑦듃濡?";
            this.colSearchControlNm.ColumnEdit = this.repositoryItemComboBoxSearchControlNm;
            this.colSearchControlNm.FieldName = "control_nm";
            this.colSearchControlNm.Name = "colSearchControlNm";
            this.colSearchControlNm.Visible = true;
            this.colSearchControlNm.VisibleIndex = 7;
            this.colSearchControlNm.Width = 120;
            //
            // repositoryItemComboBoxSearchControlNm
            //
            this.repositoryItemComboBoxSearchControlNm.AutoHeight = false;
            this.repositoryItemComboBoxSearchControlNm.Name = "repositoryItemComboBoxSearchControlNm";
            // 
            // panelWyn9
            // 
            this.panelWyn9.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn9.Appearance.Options.UseBackColor = true;
            this.panelWyn9.Controls.Add(this.sectionHeaderWyn5);
            this.panelWyn9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn9.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn9.Location = new System.Drawing.Point(0, 0);
            this.panelWyn9.Name = "panelWyn9";
            this.panelWyn9.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn9.Size = new System.Drawing.Size(1066, 27);
            this.panelWyn9.TabIndex = 12;
            // 
            // panelWyn10
            // 
            this.panelWyn10.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn10.Appearance.Options.UseBackColor = true;
            this.panelWyn10.Controls.Add(this.btnDeletRow3);
            this.panelWyn10.Controls.Add(this.btnAddRow3);
            this.panelWyn10.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn10.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn10.Location = new System.Drawing.Point(998, 0);
            this.panelWyn10.Name = "panelWyn10";
            this.panelWyn10.Padding = new System.Windows.Forms.Padding(5, 3, 0, 2);
            this.panelWyn10.Size = new System.Drawing.Size(68, 30);
            this.panelWyn10.TabIndex = 13;
            // 
            // btnDeletRow3
            // 
            this.btnDeletRow3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeletRow3.BackColor = System.Drawing.Color.Transparent;
            this.btnDeletRow3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(198)))), ((int)(((byte)(193)))));
            this.btnDeletRow3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeletRow3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(236)))), ((int)(((byte)(234)))));
            this.btnDeletRow3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDeletRow3.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnDeletRow3.Image = ((System.Drawing.Image)(resources.GetObject("btnDeletRow3.Image")));
            this.btnDeletRow3.Location = new System.Drawing.Point(68, 4);
            this.btnDeletRow3.Name = "btnDeletRow3";
            this.btnDeletRow3.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnDeletRow3.Size = new System.Drawing.Size(58, 22);
            this.btnDeletRow3.Text = "";
            this.btnDeletRow3.TabIndex = 0;
            this.btnDeletRow3.ToolTip = "행삭제";
            this.btnDeletRow3.Click += new System.EventHandler(this.btnDeletRow3_Click);
            // 
            // btnAddRow3
            // 
            this.btnAddRow3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddRow3.BackColor = System.Drawing.Color.Transparent;
            this.btnAddRow3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(231)))), ((int)(((byte)(203)))));
            this.btnAddRow3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddRow3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(245)))), ((int)(((byte)(231)))));
            this.btnAddRow3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnAddRow3.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnAddRow3.Image = ((System.Drawing.Image)(resources.GetObject("btnAddRow3.Image")));
            this.btnAddRow3.Location = new System.Drawing.Point(6, 4);
            this.btnAddRow3.Name = "btnAddRow3";
            this.btnAddRow3.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnAddRow3.Size = new System.Drawing.Size(58, 22);
            this.btnAddRow3.Text = "";
            this.btnAddRow3.TabIndex = 0;
            this.btnAddRow3.ToolTip = "행추가";
            this.btnAddRow3.Click += new System.EventHandler(this.btnAddRow3_Click);
            // 
            // sectionHeaderWyn5
            // 
            this.sectionHeaderWyn5.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionHeaderWyn5.Font = new System.Drawing.Font("맑은 고딕", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn5.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn5.Location = new System.Drawing.Point(5, 0);
            this.sectionHeaderWyn5.Name = "sectionHeaderWyn5";
            this.sectionHeaderWyn5.Size = new System.Drawing.Size(1061, 25);
            this.sectionHeaderWyn5.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn5.SvgIcon")));
            this.sectionHeaderWyn5.TabIndex = 8;
            this.sectionHeaderWyn5.Text = "議고쉶議곌굔";
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
            this.panelWyn5.Size = new System.Drawing.Size(1066, 127);
            this.panelWyn5.TabIndex = 6;
            // 
            // panData
            // 
            this.panData.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panData.Appearance.Options.UseBackColor = true;
            this.panData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panData.Controls.Add(this.txtSearchPanelClass);
            this.panData.Controls.Add(this.lblSearchPanel);
            this.panData.Controls.Add(this.txtRemark);
            this.panData.Controls.Add(this.lblPopupKey);
            this.panData.Controls.Add(this.txtPopupKey);
            this.panData.Controls.Add(this.labelControl2);
            this.panData.Controls.Add(this.lblPopupNm);
            this.panData.Controls.Add(this.txtPopupNm);
            this.panData.Controls.Add(this.lblProcNm);
            this.panData.Controls.Add(this.txtProcNm);
            this.panData.Controls.Add(this.chkHierarchical);
            this.panData.Controls.Add(this.lblKeyField);
            this.panData.Controls.Add(this.txtKeyField);
            this.panData.Controls.Add(this.lblParentField);
            this.panData.Controls.Add(this.txtParentField);
            this.panData.Controls.Add(this.lblDisplayField);
            this.panData.Controls.Add(this.txtDisplayField);
            this.panData.Controls.Add(this.chkUseYn);
            this.panData.Controls.Add(this.lblPopupWidth);
            this.panData.Controls.Add(this.txtPopupWidth);
            this.panData.Controls.Add(this.txtPopupHeight);
            this.panData.Controls.Add(this.lblRemark);
            this.panData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panData.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panData.Location = new System.Drawing.Point(0, 27);
            this.panData.Name = "panData";
            this.panData.Size = new System.Drawing.Size(1066, 100);
            this.panData.TabIndex = 8;
            // 
            // lblPopupKey
            // 
            this.lblPopupKey.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblPopupKey.Appearance.Options.UseFont = true;
            this.lblPopupKey.Location = new System.Drawing.Point(7, 12);
            this.lblPopupKey.Name = "lblPopupKey";
            this.lblPopupKey.Size = new System.Drawing.Size(48, 15);
            this.lblPopupKey.TabIndex = 0;
            this.lblPopupKey.Text = "팝업키";
            // 
            // txtPopupKey
            // 
            this.txtPopupKey.Location = new System.Drawing.Point(62, 10);
            this.txtPopupKey.Name = "txtPopupKey";
            this.txtPopupKey.Size = new System.Drawing.Size(97, 20);
            this.txtPopupKey.TabIndex = 1;
            // 
            // lblPopupNm
            // 
            this.lblPopupNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblPopupNm.Appearance.Options.UseFont = true;
            this.lblPopupNm.Location = new System.Drawing.Point(19, 36);
            this.lblPopupNm.Name = "lblPopupNm";
            this.lblPopupNm.Size = new System.Drawing.Size(36, 15);
            this.lblPopupNm.TabIndex = 2;
            this.lblPopupNm.Text = "팝업명";
            // 
            // txtPopupNm
            // 
            this.txtPopupNm.Location = new System.Drawing.Point(62, 34);
            this.txtPopupNm.Name = "txtPopupNm";
            this.txtPopupNm.Size = new System.Drawing.Size(158, 20);
            this.txtPopupNm.TabIndex = 3;
            // 
            // lblProcNm
            // 
            this.lblProcNm.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblProcNm.Appearance.Options.UseFont = true;
            this.lblProcNm.Location = new System.Drawing.Point(256, 13);
            this.lblProcNm.Name = "lblProcNm";
            this.lblProcNm.Size = new System.Drawing.Size(60, 15);
            this.lblProcNm.TabIndex = 4;
            this.lblProcNm.Text = "프로시저명";
            // 
            // txtProcNm
            // 
            this.txtProcNm.Location = new System.Drawing.Point(322, 11);
            this.txtProcNm.Name = "txtProcNm";
            this.txtProcNm.Size = new System.Drawing.Size(223, 20);
            this.txtProcNm.TabIndex = 5;
            // 
            // btnGenerateColumns
            // 
            this.btnGenerateColumns.BackColor = System.Drawing.Color.Transparent;
            this.btnGenerateColumns.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnGenerateColumns.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerateColumns.FillColor = System.Drawing.Color.White;
            this.btnGenerateColumns.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnGenerateColumns.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnGenerateColumns.Image = null;
            this.btnGenerateColumns.Location = new System.Drawing.Point(93, 1);
            this.btnGenerateColumns.Name = "btnGenerateColumns";
            this.btnGenerateColumns.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnGenerateColumns.Size = new System.Drawing.Size(90, 23);
            this.btnGenerateColumns.TabIndex = 6;
            this.btnGenerateColumns.Text = "컬럼생성";
            this.btnGenerateColumns.ToolTip = null;
            this.btnGenerateColumns.Click += new System.EventHandler(this.btnGenerateColumns_Click);
            //
            // btnCopyPopup
            //
            this.btnCopyPopup.BackColor = System.Drawing.Color.Transparent;
            this.btnCopyPopup.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnCopyPopup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCopyPopup.FillColor = System.Drawing.Color.White;
            this.btnCopyPopup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnCopyPopup.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnCopyPopup.Image = null;
            this.btnCopyPopup.Location = new System.Drawing.Point(187, 1);
            this.btnCopyPopup.Name = "btnCopyPopup";
            this.btnCopyPopup.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnCopyPopup.Size = new System.Drawing.Size(90, 23);
            this.btnCopyPopup.TabIndex = 7;
            this.btnCopyPopup.Text = "蹂듭궗";
            this.btnCopyPopup.ToolTip = "?좏깮???앹뾽??媛?而щ읆/議고쉶議곌굔 ?ы븿)??洹몃?濡?蹂듭궗???좉퇋 ?낅젰 ?곹깭濡?留뚮벊?덈떎(?앹뾽?ㅻ쭔 ?덈줈 ?낅젰)";
            //
            // chkHierarchical
            // 
            this.chkHierarchical.Location = new System.Drawing.Point(62, 64);
            this.chkHierarchical.Name = "chkHierarchical";
            this.chkHierarchical.Properties.Caption = "";
            this.chkHierarchical.Size = new System.Drawing.Size(24, 20);
            this.chkHierarchical.TabIndex = 8;
            // 
            // lblKeyField
            // 
            this.lblKeyField.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblKeyField.Appearance.Options.UseFont = true;
            this.lblKeyField.Location = new System.Drawing.Point(84, 66);
            this.lblKeyField.Name = "lblKeyField";
            this.lblKeyField.Size = new System.Drawing.Size(36, 15);
            this.lblKeyField.TabIndex = 9;
            this.lblKeyField.Text = "키필드";
            // 
            // txtKeyField
            // 
            this.txtKeyField.Location = new System.Drawing.Point(126, 64);
            this.txtKeyField.Name = "txtKeyField";
            this.txtKeyField.Size = new System.Drawing.Size(94, 20);
            this.txtKeyField.TabIndex = 10;
            // 
            // lblParentField
            // 
            this.lblParentField.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblParentField.Appearance.Options.UseFont = true;
            this.lblParentField.Location = new System.Drawing.Point(268, 66);
            this.lblParentField.Name = "lblParentField";
            this.lblParentField.Size = new System.Drawing.Size(48, 15);
            this.lblParentField.TabIndex = 11;
            this.lblParentField.Text = "부모필드";
            // 
            // txtParentField
            // 
            this.txtParentField.Location = new System.Drawing.Point(289, 64);
            this.txtParentField.Name = "txtParentField";
            this.txtParentField.Size = new System.Drawing.Size(94, 20);
            this.txtParentField.TabIndex = 12;
            // 
            // lblDisplayField
            // 
            this.lblDisplayField.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDisplayField.Appearance.Options.UseFont = true;
            this.lblDisplayField.Location = new System.Drawing.Point(398, 66);
            this.lblDisplayField.Name = "lblDisplayField";
            this.lblDisplayField.Size = new System.Drawing.Size(48, 15);
            this.lblDisplayField.TabIndex = 13;
            this.lblDisplayField.Text = "표시필드";
            // 
            // txtDisplayField
            // 
            this.txtDisplayField.Location = new System.Drawing.Point(450, 64);
            this.txtDisplayField.Name = "txtDisplayField";
            this.txtDisplayField.Size = new System.Drawing.Size(95, 20);
            this.txtDisplayField.TabIndex = 14;
            // 
            // chkUseYn
            // 
            this.chkUseYn.Location = new System.Drawing.Point(178, 9);
            this.chkUseYn.Name = "chkUseYn";
            this.chkUseYn.Properties.Caption = "사용";
            this.chkUseYn.Size = new System.Drawing.Size(55, 20);
            this.chkUseYn.TabIndex = 16;
            // 
            // lblPopupWidth
            // 
            this.lblPopupWidth.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblPopupWidth.Appearance.Options.UseFont = true;
            this.lblPopupWidth.Location = new System.Drawing.Point(261, 39);
            this.lblPopupWidth.Name = "lblPopupWidth";
            this.lblPopupWidth.Size = new System.Drawing.Size(55, 15);
            this.lblPopupWidth.TabIndex = 17;
            this.lblPopupWidth.Text = "Size(W*H)";
            // 
            // txtPopupWidth
            // 
            this.txtPopupWidth.EditValue = "Width";
            this.txtPopupWidth.Location = new System.Drawing.Point(322, 37);
            this.txtPopupWidth.Name = "txtPopupWidth";
            this.txtPopupWidth.Size = new System.Drawing.Size(110, 20);
            this.txtPopupWidth.TabIndex = 18;
            // 
            // txtPopupHeight
            // 
            this.txtPopupHeight.EditValue = "Height";
            this.txtPopupHeight.Location = new System.Drawing.Point(435, 37);
            this.txtPopupHeight.Name = "txtPopupHeight";
            this.txtPopupHeight.Size = new System.Drawing.Size(110, 20);
            this.txtPopupHeight.TabIndex = 20;
            // 
            // lblRemark
            // 
            this.lblRemark.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblRemark.Appearance.Options.UseFont = true;
            this.lblRemark.Location = new System.Drawing.Point(581, 12);
            this.lblRemark.Name = "lblRemark";
            this.lblRemark.Size = new System.Drawing.Size(24, 15);
            this.lblRemark.TabIndex = 21;
            this.lblRemark.Text = "鍮꾧퀬";
            // 
            // panelWyn6
            // 
            this.panelWyn6.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn6.Appearance.Options.UseBackColor = true;
            this.panelWyn6.Controls.Add(this.btnGenerateColumns);
            this.panelWyn6.Controls.Add(this.btnCopyPopup);
            this.panelWyn6.Controls.Add(this.sectionHeaderWyn3);
            this.panelWyn6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn6.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn6.Location = new System.Drawing.Point(0, 0);
            this.panelWyn6.Name = "panelWyn6";
            this.panelWyn6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.panelWyn6.Size = new System.Drawing.Size(1066, 27);
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
            this.sectionHeaderWyn3.Size = new System.Drawing.Size(1061, 25);
            this.sectionHeaderWyn3.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn3.SvgIcon")));
            this.sectionHeaderWyn3.TabIndex = 8;
            this.sectionHeaderWyn3.Text = "팝업 상세";
            // 
            // splitterWyn1
            // 
            this.splitterWyn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn1.Location = new System.Drawing.Point(402, 0);
            this.splitterWyn1.Name = "splitterWyn1";
            this.splitterWyn1.Size = new System.Drawing.Size(10, 515);
            this.splitterWyn1.TabIndex = 9;
            this.splitterWyn1.TabStop = false;
            // 
            // panelWyn8
            // 
            this.panelWyn8.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn8.Appearance.Options.UseBackColor = true;
            this.panelWyn8.Controls.Add(this.grd1);
            this.panelWyn8.Controls.Add(this.panelWyn2);
            this.panelWyn8.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelWyn8.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn8.Location = new System.Drawing.Point(0, 0);
            this.panelWyn8.Name = "panelWyn8";
            this.panelWyn8.Size = new System.Drawing.Size(402, 515);
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
            this.grd1.Size = new System.Drawing.Size(402, 488);
            this.grd1.TabIndex = 10;
            this.grd1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvw1});
            // 
            // gvw1
            // 
            this.gvw1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colPopupKey,
            this.colPopupNm,
            this.colProcNm,
            this.colHierarchical});
            this.gvw1.GridControl = this.grd1;
            this.gvw1.HighlightFocusedRow = true;
            this.gvw1.Name = "gvw1";
            this.gvw1.OptionsBehavior.Editable = false;
            this.gvw1.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.gvw1.OptionsSelection.InvertSelection = true;
            this.gvw1.OptionsView.ColumnAutoWidth = false;
            this.gvw1.OptionsView.ShowGroupPanel = false;
            // 
            // colPopupKey
            // 
            this.colPopupKey.Caption = "팝업키";
            this.colPopupKey.FieldName = "popup_key";
            this.colPopupKey.Name = "colPopupKey";
            this.colPopupKey.Visible = true;
            this.colPopupKey.VisibleIndex = 0;
            this.colPopupKey.Width = 80;
            // 
            // colPopupNm
            // 
            this.colPopupNm.Caption = "팝업명";
            this.colPopupNm.FieldName = "popup_nm";
            this.colPopupNm.Name = "colPopupNm";
            this.colPopupNm.Visible = true;
            this.colPopupNm.VisibleIndex = 1;
            this.colPopupNm.Width = 120;
            // 
            // colProcNm
            // 
            this.colProcNm.Caption = "프로시저명";
            this.colProcNm.FieldName = "proc_nm";
            this.colProcNm.Name = "colProcNm";
            this.colProcNm.Visible = true;
            this.colProcNm.VisibleIndex = 2;
            this.colProcNm.Width = 140;
            // 
            // colHierarchical
            // 
            this.colHierarchical.Caption = "계층형";
            this.colHierarchical.FieldName = "hierarchical_yn";
            this.colHierarchical.Name = "colHierarchical";
            this.colHierarchical.Visible = true;
            this.colHierarchical.VisibleIndex = 3;
            this.colHierarchical.Width = 60;
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
            this.panelWyn2.Size = new System.Drawing.Size(402, 27);
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
            this.sectionHeaderWyn4.Size = new System.Drawing.Size(397, 25);
            this.sectionHeaderWyn4.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn4.SvgIcon")));
            this.sectionHeaderWyn4.TabIndex = 8;
            this.sectionHeaderWyn4.Text = "팝업 목록";
            // 
            // panHeader
            // 
            this.panHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panHeader.Appearance.Options.UseBackColor = true;
            this.panHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.panHeader.Controls.Add(this.txtSearchQ);
            this.panHeader.Controls.Add(this.labelControl1);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panHeader.Location = new System.Drawing.Point(3, 33);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1481, 49);
            this.panHeader.TabIndex = 8;
            // 
            // txtSearchQ
            // 
            this.txtSearchQ.Location = new System.Drawing.Point(106, 15);
            this.txtSearchQ.Name = "txtSearchQ";
            this.txtSearchQ.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtSearchQ.Size = new System.Drawing.Size(265, 20);
            this.txtSearchQ.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(25, 18);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 15);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "검색조건";
            // 
            // paTitle
            // 
            this.paTitle.Appearance.BackColor = System.Drawing.Color.White;
            this.paTitle.Appearance.Options.UseBackColor = true;
            this.paTitle.Controls.Add(this.sectionHeaderWyn1);
            this.paTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.paTitle.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.paTitle.Location = new System.Drawing.Point(3, 0);
            this.paTitle.Name = "paTitle";
            this.paTitle.Size = new System.Drawing.Size(1481, 33);
            this.paTitle.TabIndex = 5;
            // 
            // sectionHeaderWyn1
            // 
            this.sectionHeaderWyn1.BackColor = System.Drawing.Color.White;
            this.sectionHeaderWyn1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionHeaderWyn1.Icon = WYNLAB.Base.Controls.SectionHeaderIcon.Folder;
            this.sectionHeaderWyn1.Location = new System.Drawing.Point(5, 6);
            this.sectionHeaderWyn1.Name = "sectionHeaderWyn1";
            this.sectionHeaderWyn1.Size = new System.Drawing.Size(209, 23);
            this.sectionHeaderWyn1.SvgIcon = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("sectionHeaderWyn1.SvgIcon")));
            this.sectionHeaderWyn1.TabIndex = 8;
            this.sectionHeaderWyn1.Text = "팝업관리 [SYS_POPUP]";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(9, 66);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(46, 15);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "Tree援ъ“";
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(587, 13);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(428, 46);
            this.txtRemark.TabIndex = 22;
            //
            // lblSearchPanel
            //
            this.lblSearchPanel.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSearchPanel.Appearance.Options.UseFont = true;
            this.lblSearchPanel.Location = new System.Drawing.Point(557, 66);
            this.lblSearchPanel.Name = "lblSearchPanel";
            this.lblSearchPanel.Size = new System.Drawing.Size(48, 15);
            this.lblSearchPanel.TabIndex = 23;
            this.lblSearchPanel.Text = "寃?됲뙣??";
            //
            // txtSearchPanelClass
            //
            this.txtSearchPanelClass.Location = new System.Drawing.Point(615, 64);
            this.txtSearchPanelClass.Name = "txtSearchPanelClass";
            this.txtSearchPanelClass.Size = new System.Drawing.Size(400, 20);
            this.txtSearchPanelClass.TabIndex = 24;
            // 
            // panelWyn11
            // 
            this.panelWyn11.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn11.Appearance.Options.UseBackColor = true;
            this.panelWyn11.Controls.Add(this.grd2);
            this.panelWyn11.Controls.Add(this.panelWyn7);
            this.panelWyn11.Controls.Add(this.panelWyn1);
            this.panelWyn11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWyn11.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn11.Location = new System.Drawing.Point(3, 417);
            this.panelWyn11.Name = "panelWyn11";
            this.panelWyn11.Size = new System.Drawing.Size(1066, 98);
            this.panelWyn11.TabIndex = 13;
            // 
            // panelWyn12
            // 
            this.panelWyn12.Appearance.BackColor = System.Drawing.Color.White;
            this.panelWyn12.Appearance.Options.UseBackColor = true;
            this.panelWyn12.Controls.Add(this.grd3);
            this.panelWyn12.Controls.Add(this.panelWyn10);
            this.panelWyn12.Controls.Add(this.panelWyn9);
            this.panelWyn12.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWyn12.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelWyn12.Location = new System.Drawing.Point(3, 127);
            this.panelWyn12.Name = "panelWyn12";
            this.panelWyn12.Size = new System.Drawing.Size(1066, 280);
            this.panelWyn12.TabIndex = 14;
            // 
            // splitterWyn2
            // 
            this.splitterWyn2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitterWyn2.Location = new System.Drawing.Point(3, 407);
            this.splitterWyn2.Name = "splitterWyn2";
            this.splitterWyn2.Size = new System.Drawing.Size(1066, 10);
            this.splitterWyn2.TabIndex = 15;
            this.splitterWyn2.TabStop = false;
            // 
            // frmSysPopup
            // 
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1487, 600);
            this.Controls.Add(this.panBase);
            this.Name = "frmSysPopup";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn3)).EndInit();
            this.panelWyn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn4)).EndInit();
            this.panelWyn4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxControlType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEditVisible)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn1)).EndInit();
            this.panelWyn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn7)).EndInit();
            this.panelWyn7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxSearchControlType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxSearchControlNm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn9)).EndInit();
            this.panelWyn9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn10)).EndInit();
            this.panelWyn10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn5)).EndInit();
            this.panelWyn5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panData)).EndInit();
            this.panData.ResumeLayout(false);
            this.panData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPopupKey.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPopupNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProcNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkHierarchical.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKeyField.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParentField.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDisplayField.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkUseYn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPopupWidth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPopupHeight.Properties)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchQ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paTitle)).EndInit();
            this.paTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchPanelClass.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn11)).EndInit();
            this.panelWyn11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelWyn12)).EndInit();
            this.panelWyn12.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    private PanelWyn paTitle;
    private PanelWyn panelWyn3;
    private PanelWyn panelWyn4;
    private PanelWyn panelWyn5;
    private PanelWyn panData;
    private DevExpress.XtraEditors.LabelControl lblPopupKey;
    private TextEditWyn txtPopupKey;
    private DevExpress.XtraEditors.LabelControl lblPopupNm;
    private TextEditWyn txtPopupNm;
    private DevExpress.XtraEditors.LabelControl lblProcNm;
    private TextEditWyn txtProcNm;
    private ButtonWyn btnGenerateColumns;
    private ButtonWyn btnCopyPopup;
    private CheckBoxWyn chkHierarchical;
    private DevExpress.XtraEditors.LabelControl lblKeyField;
    private TextEditWyn txtKeyField;
    private DevExpress.XtraEditors.LabelControl lblParentField;
    private TextEditWyn txtParentField;
    private DevExpress.XtraEditors.LabelControl lblDisplayField;
    private TextEditWyn txtDisplayField;
    private CheckBoxWyn chkUseYn;
    private DevExpress.XtraEditors.LabelControl lblPopupWidth;
    private TextEditWyn txtPopupWidth;
    private TextEditWyn txtPopupHeight;
    private DevExpress.XtraEditors.LabelControl lblRemark;
    private PanelWyn panelWyn6;
    private SectionHeaderWyn sectionHeaderWyn1;
    private SplitterWyn splitterWyn1;
    private SectionHeaderWyn sectionHeaderWyn3;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;
    private DevExpress.XtraGrid.Columns.GridColumn colPopupKey;
    private DevExpress.XtraGrid.Columns.GridColumn colPopupNm;
    private DevExpress.XtraGrid.Columns.GridColumn colProcNm;
    private DevExpress.XtraGrid.Columns.GridColumn colHierarchical;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;
    private DevExpress.XtraGrid.Columns.GridColumn colColumnNm;
    private DevExpress.XtraGrid.Columns.GridColumn colCaption;
    private DevExpress.XtraGrid.Columns.GridColumn colControlType;
    private DevExpress.XtraGrid.Columns.GridColumn colLookupProcNm;
    private DevExpress.XtraGrid.Columns.GridColumn colSort;
    private DevExpress.XtraGrid.Columns.GridColumn colWidth;
    private DevExpress.XtraGrid.Columns.GridColumn colVisibleYn;
    private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxControlType;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEditVisible;
    private PanelWyn panelWyn2;
    private SectionHeaderWyn sectionHeaderWyn4;
    private PanelWyn panelWyn1;
    private SectionHeaderWyn sectionHeaderWyn2;
    private PanelWyn panHeader;
    private TextEditWyn txtSearchQ;
    private DevExpress.XtraEditors.LabelControl labelControl1;
    private ButtonWyn btnDeletRow2;
    private ButtonWyn btnAddRow2;
    private PanelWyn panelWyn7;
    private PanelWyn panelWyn8;
    private GridControlWyn grd3;
    private GridViewWyn gvw3;
    private DevExpress.XtraGrid.Columns.GridColumn colParamNm;
    private DevExpress.XtraGrid.Columns.GridColumn colSearchCaption;
    private DevExpress.XtraGrid.Columns.GridColumn colSearchControlType;
    private DevExpress.XtraGrid.Columns.GridColumn colSearchLookupKey;
    private DevExpress.XtraGrid.Columns.GridColumn colSearchRowNo;
    private DevExpress.XtraGrid.Columns.GridColumn colSearchControlNm;
    private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxSearchControlNm;
    private DevExpress.XtraGrid.Columns.GridColumn colSearchSort;
    private DevExpress.XtraGrid.Columns.GridColumn colSearchWidth;
    private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxSearchControlType;
    private PanelWyn panelWyn9;
    private PanelWyn panelWyn10;
    private ButtonWyn btnDeletRow3;
    private ButtonWyn btnAddRow3;
    private SectionHeaderWyn sectionHeaderWyn5;
    private DevExpress.XtraEditors.LabelControl labelControl2;
    private PanelWyn panelWyn11;
    private SplitterWyn splitterWyn2;
    private PanelWyn panelWyn12;
    private MemoEditWyn txtRemark;
    private DevExpress.XtraEditors.LabelControl lblSearchPanel;
    private WYNLAB.Base.Controls.TextEditWyn txtSearchPanelClass;
}
