#nullable disable
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraLayout;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using System.Drawing;

namespace WYNLAB.SM.USERGROUP;

/// <summary>
/// 사용자그룹관리 화면 컨트롤 배치. 기초코드등록(CodeListForm)과 같은 컨트롤 세트(PanelWyn/
/// SectionHeaderWyn/GridControlWyn+GridViewWyn/SplitContainerWyn)를 그대로 써서 같은 디자인
/// 언어를 유지하고, 구조는 메뉴관리(MenuListForm)처럼 "좌측 리스트 + 우측 폼, 팝업 없이
/// 그 자리에서 한 건씩 저장"으로 짠다.
///
/// Dock 추가 순서 규칙(이 프로젝트 전반 공통 - Fill 먼저, Top/Bottom은 나중에 추가해야 그
/// 가장자리를 차지한다): splitContainer(Fill) -> searchPanel(Top) -> 화면 타이틀바(Top, .cs에서
/// BuildScreenHeader() 추가) 순서. rightPanel 안에서도 layoutFormPanel(Fill) -> memberSection
/// (Bottom) -> formFooterPanel(Bottom) 순서로 추가해야 푸터가 맨 아래, 그 위에 소속배정 그리드,
/// 나머지를 폼이 채우는 배치가 된다.
/// </summary>
public partial class UserGroupListForm
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

    // 상단 검색 패널(화면 전체 폭 - 기초코드등록의 panHeader와 같은 위치/역할)
    private PanelWyn searchPanel;
    private LabelControl lblSearchGrpNm;
    private TextEditWyn txtSearchGrpNm;
    private SimpleButton btnSearch;

    // 좌/우 스플리터
    private SplitContainerWyn splitContainer;
    private Panel leftPanel;
    private Panel rightPanel;

    // 좌측 - 그룹 리스트(조회 전용)
    private PanelWyn leftHeaderPanel;
    private SectionHeaderWyn sectionListHeader;
    private GridControlWyn grd1;
    private GridViewWyn gvw1;

    // 우측 - 그룹 기본정보 입력폼
    private PanelWyn formHeaderPanel;
    private SectionHeaderWyn sectionFormHeader;
    private LayoutControl layoutControl;
    private Panel layoutFormPanel;
    private TextEditWyn txtUserGrpCd;
    private TextEditWyn txtUserGrpNm;
    private MemoEdit txtDescription;
    private SpinEditWyn spnSortOrder;
    private CheckEdit chkUseYn;

    // 우측 하단 - 소속 사용자 배정(신규모드에서는 숨김 - 그룹코드가 아직 없어서)
    private Panel memberSectionPanel;
    private PanelWyn memberHeaderPanel;
    private SectionHeaderWyn sectionMemberHeader;
    private GridControlWyn grd2;
    private GridViewWyn gvw2;

    // 우측 최하단 - 저장/취소
    private Panel formFooterPanel;
    private SimpleButton btnSaveInline;
    private SimpleButton btnCancelEdit;

    private void InitializeComponent()
    {
        this.searchPanel = new PanelWyn();
        this.lblSearchGrpNm = new LabelControl();
        this.txtSearchGrpNm = new TextEditWyn();
        this.btnSearch = new SimpleButton();

        this.splitContainer = new SplitContainerWyn();
        this.leftPanel = new Panel { Dock = DockStyle.Fill };
        this.rightPanel = new Panel { Dock = DockStyle.Fill };

        this.leftHeaderPanel = new PanelWyn();
        this.sectionListHeader = new SectionHeaderWyn();
        this.grd1 = new GridControlWyn();
        this.gvw1 = new GridViewWyn();

        this.formHeaderPanel = new PanelWyn();
        this.sectionFormHeader = new SectionHeaderWyn();
        this.layoutControl = new LayoutControl();
        this.layoutFormPanel = new Panel { Dock = DockStyle.Fill };
        this.txtUserGrpCd = new TextEditWyn();
        this.txtUserGrpNm = new TextEditWyn();
        this.txtDescription = new MemoEdit();
        this.spnSortOrder = new SpinEditWyn();
        this.chkUseYn = new CheckEdit();

        this.memberSectionPanel = new Panel();
        this.memberHeaderPanel = new PanelWyn();
        this.sectionMemberHeader = new SectionHeaderWyn();
        this.grd2 = new GridControlWyn();
        this.gvw2 = new GridViewWyn();

        this.formFooterPanel = new Panel();
        this.btnSaveInline = new SimpleButton();
        this.btnCancelEdit = new SimpleButton();

        // layoutControl은 BeginInit()/EndInit()로 감싸지 않는다 - MenuListForm도 이 컨트롤에는
        // BeginUpdate/EndUpdate만 쓰고 BeginInit/EndInit은 아예 안 쓴다(검증된 패턴).
        ((System.ComponentModel.ISupportInitialize)this.txtSearchGrpNm.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.grd1).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.gvw1).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.txtUserGrpCd.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.txtUserGrpNm.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.txtDescription.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.spnSortOrder.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.chkUseYn.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.grd2).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.gvw2).BeginInit();
        this.SuspendLayout();

        //
        // searchPanel - 화면 전체 폭 검색바 (기초코드등록 panHeader와 같은 위치)
        //
        var searchBottomBorder = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Color.FromArgb(228, 229, 232) };
        this.lblSearchGrpNm.Location = new Point(16, 15);
        this.lblSearchGrpNm.Size = new Size(56, 18);
        this.lblSearchGrpNm.AutoSizeMode = LabelAutoSizeMode.None;
        this.lblSearchGrpNm.Text = "그룹명";
        this.lblSearchGrpNm.Appearance.ForeColor = Color.FromArgb(100, 100, 100);
        this.lblSearchGrpNm.Appearance.Font = AppFonts.Caption;

        this.txtSearchGrpNm.Location = new Point(74, 11);
        this.txtSearchGrpNm.Size = new Size(200, 24);
        this.txtSearchGrpNm.Properties.Appearance.Font = AppFonts.Body;
        this.txtSearchGrpNm.KeyDown += this.txtSearchGrpNm_KeyDown;

        this.btnSearch.Text = "검색";
        this.btnSearch.Location = new Point(282, 10);
        this.btnSearch.Size = new Size(72, 26);
        this.btnSearch.Appearance.Font = AppFonts.Body;
        this.btnSearch.Click += this.btnSearch_Click;

        this.searchPanel.Dock = DockStyle.Top;
        this.searchPanel.Height = 46;
        this.searchPanel.Controls.Add(this.btnSearch);
        this.searchPanel.Controls.Add(this.txtSearchGrpNm);
        this.searchPanel.Controls.Add(this.lblSearchGrpNm);
        this.searchPanel.Controls.Add(searchBottomBorder);

        //
        // splitContainer
        //
        this.splitContainer.Dock = DockStyle.Fill;
        this.splitContainer.Panel1.Controls.Add(this.leftPanel);
        this.splitContainer.Panel2.Controls.Add(this.rightPanel);
        this.splitContainer.Panel1.MinSize = 260;
        this.splitContainer.Panel2.MinSize = 420;
        this.splitContainer.SplitterPosition = 340;

        //
        // 좌측 - sectionListHeader / grd1(gvw1)
        //
        this.sectionListHeader.Dock = DockStyle.Fill;
        this.sectionListHeader.Icon = SectionHeaderIcon.Grid;
        this.sectionListHeader.Text = "사용자그룹 LIST";
        this.leftHeaderPanel.Dock = DockStyle.Top;
        this.leftHeaderPanel.Height = 27;
        this.leftHeaderPanel.Padding = new Padding(5, 0, 0, 0);
        this.leftHeaderPanel.Controls.Add(this.sectionListHeader);

        // grd1.MainView는 gvw1의 나머지 속성(Columns 등)을 건드리기 전에 먼저 연결해야 한다 -
        // CodeListForm/기존 UserGroupEditForm의 검증된 순서(grd.MainView 먼저, 컬럼/GridControl은
        // 나중)를 그대로 따른다. 순서를 반대로 했다가 InitializeComponent 안에서
        // NullReferenceException이 났던 적이 있다(view가 아직 GridControl에 붙지 않은 상태에서
        // Columns.AddRange를 호출한 게 원인으로 추정).
        this.grd1.Dock = DockStyle.Fill;
        this.grd1.MainView = this.gvw1;
        this.grd1.ViewCollection.AddRange(new BaseView[] { this.gvw1 });

        this.gvw1.OptionsBehavior.Editable = false;
        this.gvw1.OptionsBehavior.AutoPopulateColumns = false;
        this.gvw1.OptionsView.ShowGroupPanel = false;
        this.gvw1.Columns.AddRange(new GridColumn[]
        {
            new GridColumn { FieldName = "UserGrpCd", Caption = "그룹코드", Visible = true, VisibleIndex = 0, Width = 90 },
            new GridColumn { FieldName = "UserGrpNm", Caption = "그룹명", Visible = true, VisibleIndex = 1, Width = 130 },
            new GridColumn { FieldName = "Description", Caption = "설명", Visible = true, VisibleIndex = 2, Width = 180 },
            new GridColumn { FieldName = "SortOrder", Caption = "정렬순서", Visible = true, VisibleIndex = 3, Width = 70 },
            new GridColumn { FieldName = "UseYn", Caption = "사용", Visible = true, VisibleIndex = 4, Width = 55 },
            new GridColumn { FieldName = "MemberCount", Caption = "소속인원", Visible = true, VisibleIndex = 5, Width = 70 },
        });
        this.gvw1.GridControl = this.grd1;
        this.gvw1.FocusedRowObjectChanged += this.Gvw1_FocusedRowObjectChanged;

        this.leftPanel.Controls.Add(this.grd1);
        this.leftPanel.Controls.Add(this.leftHeaderPanel);

        //
        // 우측 - sectionFormHeader / layoutControl(기본정보 입력폼)
        //
        this.sectionFormHeader.Dock = DockStyle.Fill;
        this.sectionFormHeader.Icon = SectionHeaderIcon.Document;
        this.sectionFormHeader.Text = "사용자그룹 등록";
        this.formHeaderPanel.Dock = DockStyle.Top;
        this.formHeaderPanel.Height = 27;
        this.formHeaderPanel.Padding = new Padding(5, 0, 0, 0);
        this.formHeaderPanel.Controls.Add(this.sectionFormHeader);

        this.txtDescription.Properties.Appearance.Font = AppFonts.Body;

        this.layoutControl.Dock = DockStyle.Fill;
        this.layoutControl.Padding = new Padding(16);
        this.layoutControl.BeginUpdate();
        // layoutControl.Root가 실행 환경에 따라 생성자 직후에도 null로 관측되는 경우를 실제로
        // 겪었다(원인은 끝내 재현/특정하지 못함 - DevExpress LayoutControl의 지연 생성 타이밍
        // 문제로 추정). 원인과 무관하게 항상 안전하도록 null이면 직접 만들어서 대입한다 -
        // Root는 일반 속성이라 직접 설정 가능하고, 이후 코드는 이 root 하나만 참조하므로
        // 이렇게 방어해두면 어떤 상황에서도 NullReferenceException 없이 진행된다.
        var root = this.layoutControl.Root ?? (this.layoutControl.Root = new LayoutControlGroup());
        root.TextVisible = false;
        root.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);

        foreach (var edit in new BaseEdit[] { this.txtUserGrpCd, this.txtUserGrpNm, this.txtDescription, this.spnSortOrder, this.chkUseYn })
        {
            edit.Properties.Appearance.Font = AppFonts.Body;
            edit.Properties.Appearance.Options.UseFont = true;
        }

        var groupBasic = root.AddGroup("기본정보").StyleAsSection();
        groupBasic.AddItem("그룹코드", this.txtUserGrpCd).MarkRequired().FixedControlWidth(160);
        groupBasic.AddItem("그룹명", this.txtUserGrpNm).MarkRequired().FixedControlWidth(220);
        var descItem = groupBasic.AddItem("설명", this.txtDescription).FixedControlWidth(320);
        descItem.Control.Height = 50;
        groupBasic.AddItem("정렬순서", this.spnSortOrder).FixedControlWidth(80);

        var groupStatus = root.AddGroup("상태").StyleAsSection();
        var itemUseYn = groupStatus.AddItem(string.Empty, this.chkUseYn);
        itemUseYn.TextVisible = false;

        this.layoutControl.EndUpdate();

        this.layoutFormPanel.Controls.Add(this.layoutControl);
        this.layoutFormPanel.Controls.Add(this.formHeaderPanel);

        //
        // 우측 하단 - sectionMemberHeader / grd2(gvw2, 소속 사용자 배정 체크그리드)
        //
        this.sectionMemberHeader.Dock = DockStyle.Fill;
        this.sectionMemberHeader.Icon = SectionHeaderIcon.Grid;
        this.sectionMemberHeader.Text = "소속 사용자 배정";
        this.memberHeaderPanel.Dock = DockStyle.Top;
        this.memberHeaderPanel.Height = 27;
        this.memberHeaderPanel.Padding = new Padding(5, 0, 0, 0);
        this.memberHeaderPanel.Controls.Add(this.sectionMemberHeader);

        this.grd2.Dock = DockStyle.Fill;
        this.grd2.MainView = this.gvw2;
        this.grd2.ViewCollection.AddRange(new BaseView[] { this.gvw2 });

        this.gvw2.OptionsView.ShowGroupPanel = false;
        this.gvw2.OptionsView.ShowAutoFilterRow = true; // 소속 배정 그리드는 사용자 수가 많을 수 있어 필터를 켜둔다
        this.gvw2.OptionsBehavior.Editable = true;
        this.gvw2.OptionsBehavior.AutoPopulateColumns = false;
        this.gvw2.Columns.AddRange(new GridColumn[]
        {
            new GridColumn { FieldName = "UserId", Caption = "아이디", Visible = true, VisibleIndex = 0, Width = 100, OptionsColumn = { AllowEdit = false } },
            new GridColumn { FieldName = "UserNm", Caption = "이름", Visible = true, VisibleIndex = 1, Width = 100, OptionsColumn = { AllowEdit = false } },
            new GridColumn { FieldName = "DeptNm", Caption = "부서", Visible = true, VisibleIndex = 2, Width = 120, OptionsColumn = { AllowEdit = false } },
            new GridColumn { FieldName = "IsMember", Caption = "소속", Visible = true, VisibleIndex = 3, Width = 50 },
        });
        this.gvw2.GridControl = this.grd2;

        this.memberSectionPanel.Dock = DockStyle.Bottom;
        this.memberSectionPanel.Height = 240;
        this.memberSectionPanel.Controls.Add(this.grd2);
        this.memberSectionPanel.Controls.Add(this.memberHeaderPanel);

        //
        // formFooterPanel - 저장/취소
        //
        this.formFooterPanel.Dock = DockStyle.Bottom;
        this.formFooterPanel.Height = 56;
        this.formFooterPanel.BackColor = Color.FromArgb(245, 246, 248);
        var footerTopBorder = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = Color.FromArgb(230, 231, 234) };
        this.formFooterPanel.Controls.Add(footerTopBorder);

        this.btnSaveInline.Text = "저장";
        this.btnSaveInline.Size = new Size(90, 32);
        this.btnSaveInline.Appearance.BackColor = Color.FromArgb(37, 122, 201);
        this.btnSaveInline.Appearance.ForeColor = Color.White;
        this.btnSaveInline.Appearance.Options.UseBackColor = true;
        this.btnSaveInline.Appearance.Options.UseForeColor = true;
        this.btnSaveInline.Click += this.btnSaveInline_Click;

        this.btnCancelEdit.Text = "취소";
        this.btnCancelEdit.Size = new Size(90, 32);
        this.btnCancelEdit.Click += this.btnCancelEdit_Click;

        this.formFooterPanel.Controls.Add(this.btnSaveInline);
        this.formFooterPanel.Controls.Add(this.btnCancelEdit);
        this.formFooterPanel.Resize += this.formFooterPanel_Resize;

        // Dock 추가 순서: Fill(layoutFormPanel) -> Bottom(memberSectionPanel) -> Bottom(formFooterPanel)
        // 이 순서여야 footer가 맨 아래, 그 위에 소속배정 그리드, 나머지를 폼이 채운다.
        this.rightPanel.Controls.Add(this.layoutFormPanel);
        this.rightPanel.Controls.Add(this.memberSectionPanel);
        this.rightPanel.Controls.Add(this.formFooterPanel);

        //
        // UserGroupListForm
        //
        this.Text = "사용자그룹관리";
        this.Controls.Add(this.splitContainer);
        this.Controls.Add(this.searchPanel);
        this.Load += this.UserGroupListForm_Load;

        ((System.ComponentModel.ISupportInitialize)this.txtSearchGrpNm.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.grd1).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.gvw1).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.txtUserGrpCd.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.txtUserGrpNm.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.txtDescription.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.spnSortOrder.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.chkUseYn.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.grd2).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.gvw2).EndInit();
        this.ResumeLayout(false);
    }
}
