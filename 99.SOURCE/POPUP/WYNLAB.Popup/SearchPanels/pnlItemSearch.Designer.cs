#nullable disable
using WYNLAB.Base.Controls;

namespace WYNLAB.Popup;

partial class pnlItemSearch
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) { components.Dispose(); }
        base.Dispose(disposing);
    }

    // 좌표를 직접 찍는 대신 TableLayoutPanel(2줄 x 4쌍)로 배치한다 - 라벨 열은 글자 폭에 맞춰 자동으로 늘어나고
    // (글꼴/배율이 달라도 라벨이 입력창을 덮지 않는다), 입력창 열은 고정 폭이라 같은 열은 항상 정렬된다.
    // 컨트롤 이름은 프로시저 파라미터명(p_keyword, p_asset_type, p_grp1_id~p_grp4_id)과 같아야 값이 넘어간다.
    private void InitializeComponent()
    {
            this.tableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lblKeyword = new DevExpress.XtraEditors.LabelControl();
            this.p_keyword = new WYNLAB.Base.Controls.TextEditWyn();
            this.lblAssetType = new DevExpress.XtraEditors.LabelControl();
            this.p_asset_type = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblGrp1 = new DevExpress.XtraEditors.LabelControl();
            this.p_grp1_id = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblGrp2 = new DevExpress.XtraEditors.LabelControl();
            this.p_grp2_id = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblGrp3 = new DevExpress.XtraEditors.LabelControl();
            this.p_grp3_id = new WYNLAB.Base.Controls.LookUpEditWyn();
            this.lblGrp4 = new DevExpress.XtraEditors.LabelControl();
            this.p_grp4_id = new WYNLAB.Base.Controls.LookUpEditWyn();
            ((System.ComponentModel.ISupportInitialize)(this.p_keyword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.p_asset_type.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.p_grp1_id.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.p_grp2_id.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.p_grp3_id.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.p_grp4_id.Properties)).BeginInit();
            this.tableLayout.SuspendLayout();
            this.SuspendLayout();
            //
            // tableLayout
            //
            this.tableLayout.BackColor = System.Drawing.Color.Transparent;
            this.tableLayout.ColumnCount = 8;
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayout.Controls.Add(this.lblKeyword, 0, 0);
            this.tableLayout.Controls.Add(this.p_keyword, 1, 0);
            this.tableLayout.Controls.Add(this.lblAssetType, 2, 0);
            this.tableLayout.Controls.Add(this.p_asset_type, 3, 0);
            this.tableLayout.Controls.Add(this.lblGrp1, 0, 1);
            this.tableLayout.Controls.Add(this.p_grp1_id, 1, 1);
            this.tableLayout.Controls.Add(this.lblGrp2, 2, 1);
            this.tableLayout.Controls.Add(this.p_grp2_id, 3, 1);
            this.tableLayout.Controls.Add(this.lblGrp3, 4, 1);
            this.tableLayout.Controls.Add(this.p_grp3_id, 5, 1);
            this.tableLayout.Controls.Add(this.lblGrp4, 6, 1);
            this.tableLayout.Controls.Add(this.p_grp4_id, 7, 1);
            this.tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout.Location = new System.Drawing.Point(0, 0);
            this.tableLayout.Name = "tableLayout";
            this.tableLayout.Padding = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.tableLayout.RowCount = 2;
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayout.TabIndex = 0;
            //
            // lblKeyword
            //
            this.lblKeyword.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblKeyword.Location = new System.Drawing.Point(13, 11);
            this.lblKeyword.Margin = new System.Windows.Forms.Padding(3, 3, 8, 3);
            this.lblKeyword.Name = "lblKeyword";
            this.lblKeyword.Size = new System.Drawing.Size(45, 14);
            this.lblKeyword.TabIndex = 0;
            this.lblKeyword.Text = "품번/품명";
            //
            // p_keyword
            //
            this.p_keyword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.p_keyword.Location = new System.Drawing.Point(72, 8);
            this.p_keyword.Margin = new System.Windows.Forms.Padding(3, 3, 16, 3);
            this.p_keyword.Name = "p_keyword";
            this.p_keyword.Size = new System.Drawing.Size(128, 20);
            this.p_keyword.TabIndex = 1;
            //
            // lblAssetType
            //
            this.lblAssetType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblAssetType.Location = new System.Drawing.Point(257, 11);
            this.lblAssetType.Margin = new System.Windows.Forms.Padding(3, 3, 8, 3);
            this.lblAssetType.Name = "lblAssetType";
            this.lblAssetType.Size = new System.Drawing.Size(40, 14);
            this.lblAssetType.TabIndex = 2;
            this.lblAssetType.Text = "자산구분";
            //
            // p_asset_type
            //
            this.p_asset_type.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.p_asset_type.EditValue = "";
            this.p_asset_type.Location = new System.Drawing.Point(303, 8);
            this.p_asset_type.LookupKey = "L_CM0002";
            this.p_asset_type.Margin = new System.Windows.Forms.Padding(3, 3, 16, 3);
            this.p_asset_type.Name = "p_asset_type";
            this.p_asset_type.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.p_asset_type.Properties.NullText = "";
            this.p_asset_type.Size = new System.Drawing.Size(128, 20);
            this.p_asset_type.TabIndex = 3;
            //
            // lblGrp1
            //
            this.lblGrp1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblGrp1.Location = new System.Drawing.Point(13, 41);
            this.lblGrp1.Margin = new System.Windows.Forms.Padding(3, 3, 8, 3);
            this.lblGrp1.Name = "lblGrp1";
            this.lblGrp1.Size = new System.Drawing.Size(45, 14);
            this.lblGrp1.TabIndex = 4;
            this.lblGrp1.Text = "품목그룹1";
            //
            // p_grp1_id
            //
            this.p_grp1_id.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.p_grp1_id.EditValue = "";
            this.p_grp1_id.Location = new System.Drawing.Point(72, 38);
            this.p_grp1_id.Margin = new System.Windows.Forms.Padding(3, 3, 16, 3);
            this.p_grp1_id.Name = "p_grp1_id";
            this.p_grp1_id.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.p_grp1_id.Properties.NullText = "";
            this.p_grp1_id.Size = new System.Drawing.Size(128, 20);
            this.p_grp1_id.TabIndex = 5;
            //
            // lblGrp2
            //
            this.lblGrp2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblGrp2.Location = new System.Drawing.Point(257, 41);
            this.lblGrp2.Margin = new System.Windows.Forms.Padding(3, 3, 8, 3);
            this.lblGrp2.Name = "lblGrp2";
            this.lblGrp2.Size = new System.Drawing.Size(45, 14);
            this.lblGrp2.TabIndex = 6;
            this.lblGrp2.Text = "품목그룹2";
            //
            // p_grp2_id
            //
            this.p_grp2_id.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.p_grp2_id.EditValue = "";
            this.p_grp2_id.Location = new System.Drawing.Point(303, 38);
            this.p_grp2_id.Margin = new System.Windows.Forms.Padding(3, 3, 16, 3);
            this.p_grp2_id.Name = "p_grp2_id";
            this.p_grp2_id.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.p_grp2_id.Properties.NullText = "";
            this.p_grp2_id.Size = new System.Drawing.Size(128, 20);
            this.p_grp2_id.TabIndex = 7;
            //
            // lblGrp3
            //
            this.lblGrp3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblGrp3.Location = new System.Drawing.Point(488, 41);
            this.lblGrp3.Margin = new System.Windows.Forms.Padding(3, 3, 8, 3);
            this.lblGrp3.Name = "lblGrp3";
            this.lblGrp3.Size = new System.Drawing.Size(45, 14);
            this.lblGrp3.TabIndex = 8;
            this.lblGrp3.Text = "품목그룹3";
            //
            // p_grp3_id
            //
            this.p_grp3_id.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.p_grp3_id.EditValue = "";
            this.p_grp3_id.Location = new System.Drawing.Point(548, 38);
            this.p_grp3_id.Margin = new System.Windows.Forms.Padding(3, 3, 16, 3);
            this.p_grp3_id.Name = "p_grp3_id";
            this.p_grp3_id.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.p_grp3_id.Properties.NullText = "";
            this.p_grp3_id.Size = new System.Drawing.Size(128, 20);
            this.p_grp3_id.TabIndex = 9;
            //
            // lblGrp4
            //
            this.lblGrp4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblGrp4.Location = new System.Drawing.Point(734, 41);
            this.lblGrp4.Margin = new System.Windows.Forms.Padding(3, 3, 8, 3);
            this.lblGrp4.Name = "lblGrp4";
            this.lblGrp4.Size = new System.Drawing.Size(45, 14);
            this.lblGrp4.TabIndex = 10;
            this.lblGrp4.Text = "품목그룹4";
            //
            // p_grp4_id
            //
            this.p_grp4_id.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.p_grp4_id.EditValue = "";
            this.p_grp4_id.Location = new System.Drawing.Point(796, 38);
            this.p_grp4_id.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.p_grp4_id.Name = "p_grp4_id";
            this.p_grp4_id.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.p_grp4_id.Properties.NullText = "";
            this.p_grp4_id.Size = new System.Drawing.Size(128, 20);
            this.p_grp4_id.TabIndex = 11;
            //
            // pnlItemSearch
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayout);
            this.Name = "pnlItemSearch";
            this.Size = new System.Drawing.Size(1062, 60);
            ((System.ComponentModel.ISupportInitialize)(this.p_keyword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.p_asset_type.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.p_grp1_id.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.p_grp2_id.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.p_grp3_id.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.p_grp4_id.Properties)).EndInit();
            this.tableLayout.ResumeLayout(false);
            this.tableLayout.PerformLayout();
            this.ResumeLayout(false);

    }

    private System.Windows.Forms.TableLayoutPanel tableLayout;
    private DevExpress.XtraEditors.LabelControl lblKeyword;
    private TextEditWyn p_keyword;
    private DevExpress.XtraEditors.LabelControl lblAssetType;
    private LookUpEditWyn p_asset_type;
    private DevExpress.XtraEditors.LabelControl lblGrp1;
    private LookUpEditWyn p_grp1_id;
    private DevExpress.XtraEditors.LabelControl lblGrp2;
    private LookUpEditWyn p_grp2_id;
    private DevExpress.XtraEditors.LabelControl lblGrp3;
    private LookUpEditWyn p_grp3_id;
    private DevExpress.XtraEditors.LabelControl lblGrp4;
    private LookUpEditWyn p_grp4_id;
}
