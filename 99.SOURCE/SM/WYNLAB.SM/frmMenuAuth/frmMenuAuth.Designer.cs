// VS 디자이너가 자동 생성하는 필드 선언에는 = null!을 안 붙여서 nullable 경고(CS8618)가
// 계속 나기 때문에, 이 파일(디자이너 전용)만 nullable 검사를 끈다 - 흔한 관례(frmUserAuth와 동일).
#nullable disable
using WYNLAB.Base.Controls;

namespace WYNLAB.SM;

public partial class frmMenuAuth
{
    private System.ComponentModel.IContainer components = null;

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
            this.panBase = new WYNLAB.Base.Controls.PanelWyn();
            this.panelRight = new WYNLAB.Base.Controls.PanelWyn();
            this.tabControl = new WYNLAB.Base.Controls.TabControlWyn();
            this.tabUser = new DevExpress.XtraTab.XtraTabPage();
            this.grdUser = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvwUser = new WYNLAB.Base.Controls.GridViewWyn();
            this.colUserTargetCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUserTargetNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUserSubNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUserView = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkEditUser = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colUserInsert = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUserUpdate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUserDelete = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUserAuth01 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUserAuth02 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUserAuth03 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUserAuth04 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUserAuth05 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUserAuth06 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUserAuth07 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUserAuth08 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUserAuth09 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUserAuth10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tabGroup = new DevExpress.XtraTab.XtraTabPage();
            this.grdGroup = new WYNLAB.Base.Controls.GridControlWyn();
            this.gvwGroup = new WYNLAB.Base.Controls.GridViewWyn();
            this.colGroupTargetCd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGroupTargetNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGroupSubNm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGroupView = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkEditGroup = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colGroupInsert = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGroupUpdate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGroupDelete = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGroupAuth01 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGroupAuth02 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGroupAuth03 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGroupAuth04 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGroupAuth05 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGroupAuth06 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGroupAuth07 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGroupAuth08 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGroupAuth09 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGroupAuth10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelHeaderRight = new WYNLAB.Base.Controls.PanelWyn();
            this.lblSelectedMenu = new DevExpress.XtraEditors.LabelControl();
            this.btnSave = new WYNLAB.Base.Controls.ButtonWyn();
            this.splitterWyn1 = new WYNLAB.Base.Controls.SplitterWyn();
            this.panelLeft = new WYNLAB.Base.Controls.PanelWyn();
            this.menuTree = new WYNLAB.Base.Controls.TreeListWyn();
            this.colMenuNm = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.lblMenuHeader = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
            this.panBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelRight)).BeginInit();
            this.panelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEditUser)).BeginInit();
            this.tabGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEditGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelHeaderRight)).BeginInit();
            this.panelHeaderRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelLeft)).BeginInit();
            this.panelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.menuTree)).BeginInit();
            this.SuspendLayout();
            // 
            // panBase
            // 
            this.panBase.Controls.Add(this.panelRight);
            this.panBase.Controls.Add(this.splitterWyn1);
            this.panBase.Controls.Add(this.panelLeft);
            this.panBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBase.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panBase.Location = new System.Drawing.Point(0, 0);
            this.panBase.Name = "panBase";
            this.panBase.Size = new System.Drawing.Size(1161, 580);
            this.panBase.TabIndex = 0;
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.tabControl);
            this.panelRight.Controls.Add(this.panelHeaderRight);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelRight.Location = new System.Drawing.Point(261, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(900, 580);
            this.panelRight.TabIndex = 2;
            // 
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 40);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedTabPage = this.tabUser;
            this.tabControl.Size = new System.Drawing.Size(900, 540);
            this.tabControl.TabIndex = 0;
            this.tabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabUser,
            this.tabGroup});
            // 
            // tabUser
            // 
            this.tabUser.Controls.Add(this.grdUser);
            this.tabUser.Name = "tabUser";
            this.tabUser.Size = new System.Drawing.Size(898, 514);
            this.tabUser.Text = "사용자";
            // 
            // grdUser
            // 
            this.grdUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdUser.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdUser.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdUser.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdUser.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdUser.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdUser.Location = new System.Drawing.Point(0, 0);
            this.grdUser.MainView = this.gvwUser;
            this.grdUser.Name = "grdUser";
            this.grdUser.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.chkEditUser});
            this.grdUser.Size = new System.Drawing.Size(898, 514);
            this.grdUser.TabIndex = 0;
            this.grdUser.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvwUser});
            // 
            // gvwUser
            // 
            this.gvwUser.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colUserTargetCd,
            this.colUserTargetNm,
            this.colUserSubNm,
            this.colUserView,
            this.colUserInsert,
            this.colUserUpdate,
            this.colUserDelete,
            this.colUserAuth01,
            this.colUserAuth02,
            this.colUserAuth03,
            this.colUserAuth04,
            this.colUserAuth05,
            this.colUserAuth06,
            this.colUserAuth07,
            this.colUserAuth08,
            this.colUserAuth09,
            this.colUserAuth10});
            this.gvwUser.GridControl = this.grdUser;
            this.gvwUser.Name = "gvwUser";
            this.gvwUser.OptionsBehavior.Editable = false;
            this.gvwUser.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            // 
            // colUserTargetCd
            // 
            this.colUserTargetCd.Caption = "사용자ID";
            this.colUserTargetCd.FieldName = "TargetCd";
            this.colUserTargetCd.Name = "colUserTargetCd";
            this.colUserTargetCd.OptionsColumn.AllowEdit = false;
            this.colUserTargetCd.Visible = true;
            this.colUserTargetCd.VisibleIndex = 0;
            this.colUserTargetCd.Width = 90;
            // 
            // colUserTargetNm
            // 
            this.colUserTargetNm.Caption = "사용자명";
            this.colUserTargetNm.FieldName = "TargetNm";
            this.colUserTargetNm.Name = "colUserTargetNm";
            this.colUserTargetNm.OptionsColumn.AllowEdit = false;
            this.colUserTargetNm.Visible = true;
            this.colUserTargetNm.VisibleIndex = 1;
            this.colUserTargetNm.Width = 110;
            // 
            // colUserSubNm
            // 
            this.colUserSubNm.Caption = "부서";
            this.colUserSubNm.FieldName = "SubNm";
            this.colUserSubNm.Name = "colUserSubNm";
            this.colUserSubNm.OptionsColumn.AllowEdit = false;
            this.colUserSubNm.Visible = true;
            this.colUserSubNm.VisibleIndex = 2;
            this.colUserSubNm.Width = 90;
            // 
            // colUserView
            // 
            this.colUserView.Caption = "조회";
            this.colUserView.ColumnEdit = this.chkEditUser;
            this.colUserView.FieldName = "ViewYn";
            this.colUserView.Name = "colUserView";
            this.colUserView.Visible = true;
            this.colUserView.VisibleIndex = 3;
            this.colUserView.Width = 55;
            // 
            // chkEditUser
            // 
            this.chkEditUser.Name = "chkEditUser";
            // 
            // colUserInsert
            // 
            this.colUserInsert.Caption = "입력";
            this.colUserInsert.ColumnEdit = this.chkEditUser;
            this.colUserInsert.FieldName = "InsertYn";
            this.colUserInsert.Name = "colUserInsert";
            this.colUserInsert.Visible = true;
            this.colUserInsert.VisibleIndex = 4;
            this.colUserInsert.Width = 55;
            // 
            // colUserUpdate
            // 
            this.colUserUpdate.Caption = "저장";
            this.colUserUpdate.ColumnEdit = this.chkEditUser;
            this.colUserUpdate.FieldName = "UpdateYn";
            this.colUserUpdate.Name = "colUserUpdate";
            this.colUserUpdate.Visible = true;
            this.colUserUpdate.VisibleIndex = 5;
            this.colUserUpdate.Width = 55;
            // 
            // colUserDelete
            // 
            this.colUserDelete.Caption = "삭제";
            this.colUserDelete.ColumnEdit = this.chkEditUser;
            this.colUserDelete.FieldName = "DeleteYn";
            this.colUserDelete.Name = "colUserDelete";
            this.colUserDelete.Visible = true;
            this.colUserDelete.VisibleIndex = 6;
            this.colUserDelete.Width = 55;
            // 
            // colUserAuth01
            // 
            this.colUserAuth01.Caption = "Auth01";
            this.colUserAuth01.ColumnEdit = this.chkEditUser;
            this.colUserAuth01.FieldName = "Auth01";
            this.colUserAuth01.Name = "colUserAuth01";
            this.colUserAuth01.Visible = true;
            this.colUserAuth01.VisibleIndex = 7;
            this.colUserAuth01.Width = 60;
            // 
            // colUserAuth02
            // 
            this.colUserAuth02.Caption = "Auth02";
            this.colUserAuth02.ColumnEdit = this.chkEditUser;
            this.colUserAuth02.FieldName = "Auth02";
            this.colUserAuth02.Name = "colUserAuth02";
            this.colUserAuth02.Visible = true;
            this.colUserAuth02.VisibleIndex = 8;
            this.colUserAuth02.Width = 60;
            // 
            // colUserAuth03
            // 
            this.colUserAuth03.Caption = "Auth03";
            this.colUserAuth03.ColumnEdit = this.chkEditUser;
            this.colUserAuth03.FieldName = "Auth03";
            this.colUserAuth03.Name = "colUserAuth03";
            this.colUserAuth03.Visible = true;
            this.colUserAuth03.VisibleIndex = 9;
            this.colUserAuth03.Width = 60;
            // 
            // colUserAuth04
            // 
            this.colUserAuth04.Caption = "Auth04";
            this.colUserAuth04.ColumnEdit = this.chkEditUser;
            this.colUserAuth04.FieldName = "Auth04";
            this.colUserAuth04.Name = "colUserAuth04";
            this.colUserAuth04.Visible = true;
            this.colUserAuth04.VisibleIndex = 10;
            this.colUserAuth04.Width = 60;
            // 
            // colUserAuth05
            // 
            this.colUserAuth05.Caption = "Auth05";
            this.colUserAuth05.ColumnEdit = this.chkEditUser;
            this.colUserAuth05.FieldName = "Auth05";
            this.colUserAuth05.Name = "colUserAuth05";
            this.colUserAuth05.Visible = true;
            this.colUserAuth05.VisibleIndex = 11;
            this.colUserAuth05.Width = 60;
            // 
            // colUserAuth06
            // 
            this.colUserAuth06.Caption = "Auth06";
            this.colUserAuth06.ColumnEdit = this.chkEditUser;
            this.colUserAuth06.FieldName = "Auth06";
            this.colUserAuth06.Name = "colUserAuth06";
            this.colUserAuth06.Visible = true;
            this.colUserAuth06.VisibleIndex = 12;
            this.colUserAuth06.Width = 60;
            // 
            // colUserAuth07
            // 
            this.colUserAuth07.Caption = "Auth07";
            this.colUserAuth07.ColumnEdit = this.chkEditUser;
            this.colUserAuth07.FieldName = "Auth07";
            this.colUserAuth07.Name = "colUserAuth07";
            this.colUserAuth07.Visible = true;
            this.colUserAuth07.VisibleIndex = 13;
            this.colUserAuth07.Width = 60;
            // 
            // colUserAuth08
            // 
            this.colUserAuth08.Caption = "Auth08";
            this.colUserAuth08.ColumnEdit = this.chkEditUser;
            this.colUserAuth08.FieldName = "Auth08";
            this.colUserAuth08.Name = "colUserAuth08";
            this.colUserAuth08.Visible = true;
            this.colUserAuth08.VisibleIndex = 14;
            this.colUserAuth08.Width = 60;
            // 
            // colUserAuth09
            // 
            this.colUserAuth09.Caption = "Auth09";
            this.colUserAuth09.ColumnEdit = this.chkEditUser;
            this.colUserAuth09.FieldName = "Auth09";
            this.colUserAuth09.Name = "colUserAuth09";
            this.colUserAuth09.Visible = true;
            this.colUserAuth09.VisibleIndex = 15;
            this.colUserAuth09.Width = 60;
            // 
            // colUserAuth10
            // 
            this.colUserAuth10.Caption = "Auth10";
            this.colUserAuth10.ColumnEdit = this.chkEditUser;
            this.colUserAuth10.FieldName = "Auth10";
            this.colUserAuth10.Name = "colUserAuth10";
            this.colUserAuth10.Visible = true;
            this.colUserAuth10.VisibleIndex = 16;
            this.colUserAuth10.Width = 60;
            // 
            // tabGroup
            // 
            this.tabGroup.Controls.Add(this.grdGroup);
            this.tabGroup.Name = "tabGroup";
            this.tabGroup.Size = new System.Drawing.Size(898, 514);
            this.tabGroup.Text = "사용자그룹";
            // 
            // grdGroup
            // 
            this.grdGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdGroup.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grdGroup.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grdGroup.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grdGroup.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grdGroup.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grdGroup.Location = new System.Drawing.Point(0, 0);
            this.grdGroup.MainView = this.gvwGroup;
            this.grdGroup.Name = "grdGroup";
            this.grdGroup.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.chkEditGroup});
            this.grdGroup.Size = new System.Drawing.Size(898, 514);
            this.grdGroup.TabIndex = 0;
            this.grdGroup.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvwGroup});
            // 
            // gvwGroup
            // 
            this.gvwGroup.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colGroupTargetCd,
            this.colGroupTargetNm,
            this.colGroupSubNm,
            this.colGroupView,
            this.colGroupInsert,
            this.colGroupUpdate,
            this.colGroupDelete,
            this.colGroupAuth01,
            this.colGroupAuth02,
            this.colGroupAuth03,
            this.colGroupAuth04,
            this.colGroupAuth05,
            this.colGroupAuth06,
            this.colGroupAuth07,
            this.colGroupAuth08,
            this.colGroupAuth09,
            this.colGroupAuth10});
            this.gvwGroup.GridControl = this.grdGroup;
            this.gvwGroup.Name = "gvwGroup";
            this.gvwGroup.OptionsBehavior.Editable = false;
            this.gvwGroup.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            // 
            // colGroupTargetCd
            // 
            this.colGroupTargetCd.Caption = "그룹코드";
            this.colGroupTargetCd.FieldName = "TargetCd";
            this.colGroupTargetCd.Name = "colGroupTargetCd";
            this.colGroupTargetCd.OptionsColumn.AllowEdit = false;
            this.colGroupTargetCd.Visible = true;
            this.colGroupTargetCd.VisibleIndex = 0;
            this.colGroupTargetCd.Width = 90;
            // 
            // colGroupTargetNm
            // 
            this.colGroupTargetNm.Caption = "그룹명";
            this.colGroupTargetNm.FieldName = "TargetNm";
            this.colGroupTargetNm.Name = "colGroupTargetNm";
            this.colGroupTargetNm.OptionsColumn.AllowEdit = false;
            this.colGroupTargetNm.Visible = true;
            this.colGroupTargetNm.VisibleIndex = 1;
            this.colGroupTargetNm.Width = 110;
            // 
            // colGroupSubNm
            // 
            this.colGroupSubNm.Caption = "인원";
            this.colGroupSubNm.FieldName = "SubNm";
            this.colGroupSubNm.Name = "colGroupSubNm";
            this.colGroupSubNm.OptionsColumn.AllowEdit = false;
            this.colGroupSubNm.Visible = true;
            this.colGroupSubNm.VisibleIndex = 2;
            this.colGroupSubNm.Width = 90;
            // 
            // colGroupView
            // 
            this.colGroupView.Caption = "조회";
            this.colGroupView.ColumnEdit = this.chkEditGroup;
            this.colGroupView.FieldName = "ViewYn";
            this.colGroupView.Name = "colGroupView";
            this.colGroupView.Visible = true;
            this.colGroupView.VisibleIndex = 3;
            this.colGroupView.Width = 55;
            // 
            // chkEditGroup
            // 
            this.chkEditGroup.Name = "chkEditGroup";
            // 
            // colGroupInsert
            // 
            this.colGroupInsert.Caption = "입력";
            this.colGroupInsert.ColumnEdit = this.chkEditGroup;
            this.colGroupInsert.FieldName = "InsertYn";
            this.colGroupInsert.Name = "colGroupInsert";
            this.colGroupInsert.Visible = true;
            this.colGroupInsert.VisibleIndex = 4;
            this.colGroupInsert.Width = 55;
            // 
            // colGroupUpdate
            // 
            this.colGroupUpdate.Caption = "저장";
            this.colGroupUpdate.ColumnEdit = this.chkEditGroup;
            this.colGroupUpdate.FieldName = "UpdateYn";
            this.colGroupUpdate.Name = "colGroupUpdate";
            this.colGroupUpdate.Visible = true;
            this.colGroupUpdate.VisibleIndex = 5;
            this.colGroupUpdate.Width = 55;
            // 
            // colGroupDelete
            // 
            this.colGroupDelete.Caption = "삭제";
            this.colGroupDelete.ColumnEdit = this.chkEditGroup;
            this.colGroupDelete.FieldName = "DeleteYn";
            this.colGroupDelete.Name = "colGroupDelete";
            this.colGroupDelete.Visible = true;
            this.colGroupDelete.VisibleIndex = 6;
            this.colGroupDelete.Width = 55;
            // 
            // colGroupAuth01
            // 
            this.colGroupAuth01.Caption = "Auth01";
            this.colGroupAuth01.ColumnEdit = this.chkEditGroup;
            this.colGroupAuth01.FieldName = "Auth01";
            this.colGroupAuth01.Name = "colGroupAuth01";
            this.colGroupAuth01.Visible = true;
            this.colGroupAuth01.VisibleIndex = 7;
            this.colGroupAuth01.Width = 60;
            // 
            // colGroupAuth02
            // 
            this.colGroupAuth02.Caption = "Auth02";
            this.colGroupAuth02.ColumnEdit = this.chkEditGroup;
            this.colGroupAuth02.FieldName = "Auth02";
            this.colGroupAuth02.Name = "colGroupAuth02";
            this.colGroupAuth02.Visible = true;
            this.colGroupAuth02.VisibleIndex = 8;
            this.colGroupAuth02.Width = 60;
            // 
            // colGroupAuth03
            // 
            this.colGroupAuth03.Caption = "Auth03";
            this.colGroupAuth03.ColumnEdit = this.chkEditGroup;
            this.colGroupAuth03.FieldName = "Auth03";
            this.colGroupAuth03.Name = "colGroupAuth03";
            this.colGroupAuth03.Visible = true;
            this.colGroupAuth03.VisibleIndex = 9;
            this.colGroupAuth03.Width = 60;
            // 
            // colGroupAuth04
            // 
            this.colGroupAuth04.Caption = "Auth04";
            this.colGroupAuth04.ColumnEdit = this.chkEditGroup;
            this.colGroupAuth04.FieldName = "Auth04";
            this.colGroupAuth04.Name = "colGroupAuth04";
            this.colGroupAuth04.Visible = true;
            this.colGroupAuth04.VisibleIndex = 10;
            this.colGroupAuth04.Width = 60;
            // 
            // colGroupAuth05
            // 
            this.colGroupAuth05.Caption = "Auth05";
            this.colGroupAuth05.ColumnEdit = this.chkEditGroup;
            this.colGroupAuth05.FieldName = "Auth05";
            this.colGroupAuth05.Name = "colGroupAuth05";
            this.colGroupAuth05.Visible = true;
            this.colGroupAuth05.VisibleIndex = 11;
            this.colGroupAuth05.Width = 60;
            // 
            // colGroupAuth06
            // 
            this.colGroupAuth06.Caption = "Auth06";
            this.colGroupAuth06.ColumnEdit = this.chkEditGroup;
            this.colGroupAuth06.FieldName = "Auth06";
            this.colGroupAuth06.Name = "colGroupAuth06";
            this.colGroupAuth06.Visible = true;
            this.colGroupAuth06.VisibleIndex = 12;
            this.colGroupAuth06.Width = 60;
            // 
            // colGroupAuth07
            // 
            this.colGroupAuth07.Caption = "Auth07";
            this.colGroupAuth07.ColumnEdit = this.chkEditGroup;
            this.colGroupAuth07.FieldName = "Auth07";
            this.colGroupAuth07.Name = "colGroupAuth07";
            this.colGroupAuth07.Visible = true;
            this.colGroupAuth07.VisibleIndex = 13;
            this.colGroupAuth07.Width = 60;
            // 
            // colGroupAuth08
            // 
            this.colGroupAuth08.Caption = "Auth08";
            this.colGroupAuth08.ColumnEdit = this.chkEditGroup;
            this.colGroupAuth08.FieldName = "Auth08";
            this.colGroupAuth08.Name = "colGroupAuth08";
            this.colGroupAuth08.Visible = true;
            this.colGroupAuth08.VisibleIndex = 14;
            this.colGroupAuth08.Width = 60;
            // 
            // colGroupAuth09
            // 
            this.colGroupAuth09.Caption = "Auth09";
            this.colGroupAuth09.ColumnEdit = this.chkEditGroup;
            this.colGroupAuth09.FieldName = "Auth09";
            this.colGroupAuth09.Name = "colGroupAuth09";
            this.colGroupAuth09.Visible = true;
            this.colGroupAuth09.VisibleIndex = 15;
            this.colGroupAuth09.Width = 60;
            // 
            // colGroupAuth10
            // 
            this.colGroupAuth10.Caption = "Auth10";
            this.colGroupAuth10.ColumnEdit = this.chkEditGroup;
            this.colGroupAuth10.FieldName = "Auth10";
            this.colGroupAuth10.Name = "colGroupAuth10";
            this.colGroupAuth10.Visible = true;
            this.colGroupAuth10.VisibleIndex = 16;
            this.colGroupAuth10.Width = 60;
            // 
            // panelHeaderRight
            // 
            this.panelHeaderRight.Controls.Add(this.lblSelectedMenu);
            this.panelHeaderRight.Controls.Add(this.btnSave);
            this.panelHeaderRight.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeaderRight.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelHeaderRight.Location = new System.Drawing.Point(0, 0);
            this.panelHeaderRight.Name = "panelHeaderRight";
            this.panelHeaderRight.Padding = new System.Windows.Forms.Padding(16, 8, 16, 0);
            this.panelHeaderRight.Size = new System.Drawing.Size(900, 40);
            this.panelHeaderRight.TabIndex = 0;
            // 
            // lblSelectedMenu
            // 
            this.lblSelectedMenu.Location = new System.Drawing.Point(0, 10);
            this.lblSelectedMenu.Name = "lblSelectedMenu";
            this.lblSelectedMenu.Size = new System.Drawing.Size(84, 14);
            this.lblSelectedMenu.TabIndex = 0;
            this.lblSelectedMenu.Text = "메뉴를 선택하세요";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(134)))), ((int)(((byte)(232)))));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            this.btnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnSave.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnSave.Image = null;
            this.btnSave.Location = new System.Drawing.Point(794, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.btnSave.Size = new System.Drawing.Size(90, 24);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "저장";
            this.btnSave.ToolTip = null;
            // 
            // splitterWyn1
            // 
            this.splitterWyn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitterWyn1.Location = new System.Drawing.Point(260, 0);
            this.splitterWyn1.Name = "splitterWyn1";
            this.splitterWyn1.Size = new System.Drawing.Size(1, 580);
            this.splitterWyn1.TabIndex = 1;
            this.splitterWyn1.TabStop = false;
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.menuTree);
            this.panelLeft.Controls.Add(this.lblMenuHeader);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.EdgeLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(229)))), ((int)(((byte)(232)))));
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(260, 580);
            this.panelLeft.TabIndex = 0;
            // 
            // menuTree
            // 
            this.menuTree.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colMenuNm});
            this.menuTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuTree.Location = new System.Drawing.Point(0, 22);
            this.menuTree.Name = "menuTree";
            this.menuTree.OptionsBehavior.Editable = false;
            this.menuTree.OptionsView.ShowColumns = false;
            this.menuTree.RowHeight = 26;
            this.menuTree.Size = new System.Drawing.Size(260, 558);
            this.menuTree.TabIndex = 0;
            // 
            // colMenuNm
            // 
            this.colMenuNm.Caption = "메뉴명";
            this.colMenuNm.FieldName = "MenuNm";
            this.colMenuNm.Name = "colMenuNm";
            this.colMenuNm.Visible = true;
            this.colMenuNm.VisibleIndex = 0;
            // 
            // lblMenuHeader
            // 
            this.lblMenuHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMenuHeader.Location = new System.Drawing.Point(0, 0);
            this.lblMenuHeader.Name = "lblMenuHeader";
            this.lblMenuHeader.Padding = new System.Windows.Forms.Padding(16, 8, 0, 0);
            this.lblMenuHeader.Size = new System.Drawing.Size(36, 22);
            this.lblMenuHeader.TabIndex = 1;
            this.lblMenuHeader.Text = "메뉴";
            // 
            // frmMenuAuth
            // 
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1161, 580);
            this.Controls.Add(this.panBase);
            this.Name = "frmMenuAuth";
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.panBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelRight)).EndInit();
            this.panelRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEditUser)).EndInit();
            this.tabGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEditGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelHeaderRight)).EndInit();
            this.panelHeaderRight.ResumeLayout(false);
            this.panelHeaderRight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelLeft)).EndInit();
            this.panelLeft.ResumeLayout(false);
            this.panelLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.menuTree)).EndInit();
            this.ResumeLayout(false);

    }

    private PanelWyn panBase;
    private PanelWyn panelLeft;
    private TreeListWyn menuTree;
    private DevExpress.XtraTreeList.Columns.TreeListColumn colMenuNm;
    private DevExpress.XtraEditors.LabelControl lblMenuHeader;
    private SplitterWyn splitterWyn1;
    private PanelWyn panelRight;
    private PanelWyn panelHeaderRight;
    private DevExpress.XtraEditors.LabelControl lblSelectedMenu;
    private ButtonWyn btnSave;
    private TabControlWyn tabControl;
    private DevExpress.XtraTab.XtraTabPage tabUser;
    private GridControlWyn grdUser;
    private GridViewWyn gvwUser;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkEditUser;
    private DevExpress.XtraGrid.Columns.GridColumn colUserTargetCd;
    private DevExpress.XtraGrid.Columns.GridColumn colUserTargetNm;
    private DevExpress.XtraGrid.Columns.GridColumn colUserSubNm;
    private DevExpress.XtraGrid.Columns.GridColumn colUserView;
    private DevExpress.XtraGrid.Columns.GridColumn colUserInsert;
    private DevExpress.XtraGrid.Columns.GridColumn colUserUpdate;
    private DevExpress.XtraGrid.Columns.GridColumn colUserDelete;
    private DevExpress.XtraGrid.Columns.GridColumn colUserAuth01;
    private DevExpress.XtraGrid.Columns.GridColumn colUserAuth02;
    private DevExpress.XtraGrid.Columns.GridColumn colUserAuth03;
    private DevExpress.XtraGrid.Columns.GridColumn colUserAuth04;
    private DevExpress.XtraGrid.Columns.GridColumn colUserAuth05;
    private DevExpress.XtraGrid.Columns.GridColumn colUserAuth06;
    private DevExpress.XtraGrid.Columns.GridColumn colUserAuth07;
    private DevExpress.XtraGrid.Columns.GridColumn colUserAuth08;
    private DevExpress.XtraGrid.Columns.GridColumn colUserAuth09;
    private DevExpress.XtraGrid.Columns.GridColumn colUserAuth10;
    private DevExpress.XtraTab.XtraTabPage tabGroup;
    private GridControlWyn grdGroup;
    private GridViewWyn gvwGroup;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkEditGroup;
    private DevExpress.XtraGrid.Columns.GridColumn colGroupTargetCd;
    private DevExpress.XtraGrid.Columns.GridColumn colGroupTargetNm;
    private DevExpress.XtraGrid.Columns.GridColumn colGroupSubNm;
    private DevExpress.XtraGrid.Columns.GridColumn colGroupView;
    private DevExpress.XtraGrid.Columns.GridColumn colGroupInsert;
    private DevExpress.XtraGrid.Columns.GridColumn colGroupUpdate;
    private DevExpress.XtraGrid.Columns.GridColumn colGroupDelete;
    private DevExpress.XtraGrid.Columns.GridColumn colGroupAuth01;
    private DevExpress.XtraGrid.Columns.GridColumn colGroupAuth02;
    private DevExpress.XtraGrid.Columns.GridColumn colGroupAuth03;
    private DevExpress.XtraGrid.Columns.GridColumn colGroupAuth04;
    private DevExpress.XtraGrid.Columns.GridColumn colGroupAuth05;
    private DevExpress.XtraGrid.Columns.GridColumn colGroupAuth06;
    private DevExpress.XtraGrid.Columns.GridColumn colGroupAuth07;
    private DevExpress.XtraGrid.Columns.GridColumn colGroupAuth08;
    private DevExpress.XtraGrid.Columns.GridColumn colGroupAuth09;
    private DevExpress.XtraGrid.Columns.GridColumn colGroupAuth10;
}
