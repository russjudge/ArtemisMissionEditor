namespace ArtemisMissionEditor
{
	partial class _FormFindReplace
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._FFR_tc_Main = new System.Windows.Forms.TabControl();
			this._FFR_tc_Main_Find = new System.Windows.Forms.TabPage();
			this._FFR_cb_SelectedNodeFind = new System.Windows.Forms.CheckBox();
			this._FFR_cb_ExactFind = new System.Windows.Forms.CheckBox();
			this._FFR_cb_CaseFind = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this._FFR_cb_DisplayedText = new System.Windows.Forms.CheckBox();
			this._FFR_cb_CommentariesFind = new System.Windows.Forms.CheckBox();
			this._FFR_cb_NodeNamesFind = new System.Windows.Forms.CheckBox();
			this._FFR_cob_AttNameFind = new System.Windows.Forms.ComboBox();
			this._FFR_b_FindAll = new System.Windows.Forms.Button();
			this._FFR_b_FindPrevious = new System.Windows.Forms.Button();
			this._FFR_b_FindNext = new System.Windows.Forms.Button();
			this._FFR_cb_XmlValuesFind = new System.Windows.Forms.CheckBox();
			this._FFR_cb_XmlNames = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this._FFR_tb_Input = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this._FFR_tc_Main_Replace = new System.Windows.Forms.TabPage();
			this._FFR_cb_CommentariesReplace = new System.Windows.Forms.CheckBox();
			this._FFR_cb_NodeNamesReplace = new System.Windows.Forms.CheckBox();
			this._FFR_cb_SelectedNodeReplace = new System.Windows.Forms.CheckBox();
			this._FFR_cb_ExactReplace = new System.Windows.Forms.CheckBox();
			this._FFR_cb_CaseReplace = new System.Windows.Forms.CheckBox();
			this.label9 = new System.Windows.Forms.Label();
			this._FFR_b_ReplaceAll = new System.Windows.Forms.Button();
			this._FFR_b_ReplaceNext = new System.Windows.Forms.Button();
			this._FFR_b_FindNextInReplace = new System.Windows.Forms.Button();
			this._FFR_cb_XmlValuesReplace = new System.Windows.Forms.CheckBox();
			this.label8 = new System.Windows.Forms.Label();
			this._FFR_cob_AttNameReplace = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this._FFR_tb_ReplaceWith = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this._FFR_tb_ReplaceWhat = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this._FFR_b_ReplacePrevious = new System.Windows.Forms.Button();
			this._FFR_b_FindPreviousInReplace = new System.Windows.Forms.Button();
			this._FFR_tc_Main.SuspendLayout();
			this._FFR_tc_Main_Find.SuspendLayout();
			this._FFR_tc_Main_Replace.SuspendLayout();
			this.SuspendLayout();
			// 
			// _FFR_tc_Main
			// 
			this._FFR_tc_Main.Controls.Add(this._FFR_tc_Main_Find);
			this._FFR_tc_Main.Controls.Add(this._FFR_tc_Main_Replace);
			this._FFR_tc_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this._FFR_tc_Main.Location = new System.Drawing.Point(0, 0);
			this._FFR_tc_Main.Name = "_FFR_tc_Main";
			this._FFR_tc_Main.SelectedIndex = 0;
			this._FFR_tc_Main.Size = new System.Drawing.Size(299, 420);
			this._FFR_tc_Main.TabIndex = 0;
			this._FFR_tc_Main.TabStop = false;
			this._FFR_tc_Main.SelectedIndexChanged += new System.EventHandler(this._E_FFR_tc_Main_SelectedIndexChanged);
			// 
			// _FFR_tc_Main_Find
			// 
			this._FFR_tc_Main_Find.Controls.Add(this._FFR_cb_SelectedNodeFind);
			this._FFR_tc_Main_Find.Controls.Add(this._FFR_cb_ExactFind);
			this._FFR_tc_Main_Find.Controls.Add(this._FFR_cb_CaseFind);
			this._FFR_tc_Main_Find.Controls.Add(this.label5);
			this._FFR_tc_Main_Find.Controls.Add(this.label3);
			this._FFR_tc_Main_Find.Controls.Add(this._FFR_cb_DisplayedText);
			this._FFR_tc_Main_Find.Controls.Add(this._FFR_cb_CommentariesFind);
			this._FFR_tc_Main_Find.Controls.Add(this._FFR_cb_NodeNamesFind);
			this._FFR_tc_Main_Find.Controls.Add(this._FFR_cob_AttNameFind);
			this._FFR_tc_Main_Find.Controls.Add(this._FFR_b_FindAll);
			this._FFR_tc_Main_Find.Controls.Add(this._FFR_b_FindPrevious);
			this._FFR_tc_Main_Find.Controls.Add(this._FFR_b_FindNext);
			this._FFR_tc_Main_Find.Controls.Add(this._FFR_cb_XmlValuesFind);
			this._FFR_tc_Main_Find.Controls.Add(this._FFR_cb_XmlNames);
			this._FFR_tc_Main_Find.Controls.Add(this.label2);
			this._FFR_tc_Main_Find.Controls.Add(this._FFR_tb_Input);
			this._FFR_tc_Main_Find.Controls.Add(this.label1);
			this._FFR_tc_Main_Find.Location = new System.Drawing.Point(4, 22);
			this._FFR_tc_Main_Find.Name = "_FFR_tc_Main_Find";
			this._FFR_tc_Main_Find.Padding = new System.Windows.Forms.Padding(3);
			this._FFR_tc_Main_Find.Size = new System.Drawing.Size(291, 394);
			this._FFR_tc_Main_Find.TabIndex = 0;
			this._FFR_tc_Main_Find.Text = "Find";
			this._FFR_tc_Main_Find.UseVisualStyleBackColor = true;
			// 
			// _FFR_cb_SelectedNodeFind
			// 
			this._FFR_cb_SelectedNodeFind.AutoSize = true;
			this._FFR_cb_SelectedNodeFind.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._FFR_cb_SelectedNodeFind.Location = new System.Drawing.Point(11, 308);
			this._FFR_cb_SelectedNodeFind.Name = "_FFR_cb_SelectedNodeFind";
			this._FFR_cb_SelectedNodeFind.Size = new System.Drawing.Size(140, 19);
			this._FFR_cb_SelectedNodeFind.TabIndex = 18;
			this._FFR_cb_SelectedNodeFind.Text = "Only in selected node";
			this._FFR_cb_SelectedNodeFind.UseVisualStyleBackColor = true;
			// 
			// _FFR_cb_ExactFind
			// 
			this._FFR_cb_ExactFind.AutoSize = true;
			this._FFR_cb_ExactFind.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._FFR_cb_ExactFind.Location = new System.Drawing.Point(11, 283);
			this._FFR_cb_ExactFind.Name = "_FFR_cb_ExactFind";
			this._FFR_cb_ExactFind.Size = new System.Drawing.Size(121, 19);
			this._FFR_cb_ExactFind.TabIndex = 17;
			this._FFR_cb_ExactFind.Text = "Match exact value";
			this._FFR_cb_ExactFind.UseVisualStyleBackColor = true;
			// 
			// _FFR_cb_CaseFind
			// 
			this._FFR_cb_CaseFind.AutoSize = true;
			this._FFR_cb_CaseFind.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._FFR_cb_CaseFind.Location = new System.Drawing.Point(11, 258);
			this._FFR_cb_CaseFind.Name = "_FFR_cb_CaseFind";
			this._FFR_cb_CaseFind.Size = new System.Drawing.Size(86, 19);
			this._FFR_cb_CaseFind.TabIndex = 16;
			this._FFR_cb_CaseFind.Text = "Match case";
			this._FFR_cb_CaseFind.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.label5.Location = new System.Drawing.Point(8, 239);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(76, 15);
			this.label5.TabIndex = 15;
			this.label5.Text = "Find options:";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label3.Location = new System.Drawing.Point(28, 158);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(134, 19);
			this.label3.TabIndex = 14;
			this.label3.Text = "where attribute name is:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _FFR_cb_DisplayedText
			// 
			this._FFR_cb_DisplayedText.AutoSize = true;
			this._FFR_cb_DisplayedText.Checked = true;
			this._FFR_cb_DisplayedText.CheckState = System.Windows.Forms.CheckState.Checked;
			this._FFR_cb_DisplayedText.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._FFR_cb_DisplayedText.Location = new System.Drawing.Point(11, 83);
			this._FFR_cb_DisplayedText.Name = "_FFR_cb_DisplayedText";
			this._FFR_cb_DisplayedText.Size = new System.Drawing.Size(155, 19);
			this._FFR_cb_DisplayedText.TabIndex = 13;
			this._FFR_cb_DisplayedText.Text = "Displayed statement text";
			this._FFR_cb_DisplayedText.UseVisualStyleBackColor = true;
			// 
			// _FFR_cb_CommentariesFind
			// 
			this._FFR_cb_CommentariesFind.AutoSize = true;
			this._FFR_cb_CommentariesFind.Checked = true;
			this._FFR_cb_CommentariesFind.CheckState = System.Windows.Forms.CheckState.Checked;
			this._FFR_cb_CommentariesFind.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._FFR_cb_CommentariesFind.Location = new System.Drawing.Point(11, 180);
			this._FFR_cb_CommentariesFind.Name = "_FFR_cb_CommentariesFind";
			this._FFR_cb_CommentariesFind.Size = new System.Drawing.Size(104, 19);
			this._FFR_cb_CommentariesFind.TabIndex = 12;
			this._FFR_cb_CommentariesFind.Text = "Commentaries";
			this._FFR_cb_CommentariesFind.UseVisualStyleBackColor = true;
			// 
			// _FFR_cb_NodeNamesFind
			// 
			this._FFR_cb_NodeNamesFind.AutoSize = true;
			this._FFR_cb_NodeNamesFind.Checked = true;
			this._FFR_cb_NodeNamesFind.CheckState = System.Windows.Forms.CheckState.Checked;
			this._FFR_cb_NodeNamesFind.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._FFR_cb_NodeNamesFind.Location = new System.Drawing.Point(11, 208);
			this._FFR_cb_NodeNamesFind.Name = "_FFR_cb_NodeNamesFind";
			this._FFR_cb_NodeNamesFind.Size = new System.Drawing.Size(135, 19);
			this._FFR_cb_NodeNamesFind.TabIndex = 11;
			this._FFR_cb_NodeNamesFind.Text = "Mission node names";
			this._FFR_cb_NodeNamesFind.UseVisualStyleBackColor = true;
			// 
			// _FFR_cob_AttNameFind
			// 
			this._FFR_cob_AttNameFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._FFR_cob_AttNameFind.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._FFR_cob_AttNameFind.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this._FFR_cob_AttNameFind.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._FFR_cob_AttNameFind.FormattingEnabled = true;
			this._FFR_cob_AttNameFind.Location = new System.Drawing.Point(168, 156);
			this._FFR_cob_AttNameFind.Name = "_FFR_cob_AttNameFind";
			this._FFR_cob_AttNameFind.Size = new System.Drawing.Size(115, 23);
			this._FFR_cob_AttNameFind.TabIndex = 9;
			this._FFR_cob_AttNameFind.Leave += new System.EventHandler(this._E_FFR_cob_AttName_Leave);
			// 
			// _FFR_b_FindAll
			// 
			this._FFR_b_FindAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._FFR_b_FindAll.AutoSize = true;
			this._FFR_b_FindAll.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._FFR_b_FindAll.Location = new System.Drawing.Point(7, 361);
			this._FFR_b_FindAll.Name = "_FFR_b_FindAll";
			this._FFR_b_FindAll.Size = new System.Drawing.Size(88, 25);
			this._FFR_b_FindAll.TabIndex = 7;
			this._FFR_b_FindAll.Text = "Find All";
			this._FFR_b_FindAll.UseVisualStyleBackColor = true;
			this._FFR_b_FindAll.Click += new System.EventHandler(this._E_FFR_b_FindAll_Click);
			// 
			// _FFR_b_FindPrevious
			// 
			this._FFR_b_FindPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._FFR_b_FindPrevious.AutoSize = true;
			this._FFR_b_FindPrevious.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._FFR_b_FindPrevious.Location = new System.Drawing.Point(101, 361);
			this._FFR_b_FindPrevious.Name = "_FFR_b_FindPrevious";
			this._FFR_b_FindPrevious.Size = new System.Drawing.Size(88, 25);
			this._FFR_b_FindPrevious.TabIndex = 6;
			this._FFR_b_FindPrevious.Text = "Find Previous";
			this._FFR_b_FindPrevious.UseVisualStyleBackColor = true;
			this._FFR_b_FindPrevious.Click += new System.EventHandler(this._E_FFR_b_Previous_Click);
			// 
			// _FFR_b_FindNext
			// 
			this._FFR_b_FindNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._FFR_b_FindNext.AutoSize = true;
			this._FFR_b_FindNext.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._FFR_b_FindNext.Location = new System.Drawing.Point(195, 361);
			this._FFR_b_FindNext.Name = "_FFR_b_FindNext";
			this._FFR_b_FindNext.Size = new System.Drawing.Size(88, 25);
			this._FFR_b_FindNext.TabIndex = 5;
			this._FFR_b_FindNext.Text = "Find Next";
			this._FFR_b_FindNext.UseVisualStyleBackColor = true;
			this._FFR_b_FindNext.Click += new System.EventHandler(this._E_FFR_b_Next_Click);
			// 
			// _FFR_cb_XmlValuesFind
			// 
			this._FFR_cb_XmlValuesFind.AutoSize = true;
			this._FFR_cb_XmlValuesFind.Checked = true;
			this._FFR_cb_XmlValuesFind.CheckState = System.Windows.Forms.CheckState.Checked;
			this._FFR_cb_XmlValuesFind.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._FFR_cb_XmlValuesFind.Location = new System.Drawing.Point(11, 133);
			this._FFR_cb_XmlValuesFind.Name = "_FFR_cb_XmlValuesFind";
			this._FFR_cb_XmlValuesFind.Size = new System.Drawing.Size(131, 19);
			this._FFR_cb_XmlValuesFind.TabIndex = 4;
			this._FFR_cb_XmlValuesFind.Text = "Xml attribute values";
			this._FFR_cb_XmlValuesFind.UseVisualStyleBackColor = true;
			this._FFR_cb_XmlValuesFind.CheckedChanged += new System.EventHandler(this._E_FFR_cb_XmlValues_CheckedChanged);
			// 
			// _FFR_cb_XmlNames
			// 
			this._FFR_cb_XmlNames.AutoSize = true;
			this._FFR_cb_XmlNames.Checked = true;
			this._FFR_cb_XmlNames.CheckState = System.Windows.Forms.CheckState.Checked;
			this._FFR_cb_XmlNames.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._FFR_cb_XmlNames.Location = new System.Drawing.Point(11, 108);
			this._FFR_cb_XmlNames.Name = "_FFR_cb_XmlNames";
			this._FFR_cb_XmlNames.Size = new System.Drawing.Size(133, 19);
			this._FFR_cb_XmlNames.TabIndex = 3;
			this._FFR_cb_XmlNames.Text = "Xml attribute names";
			this._FFR_cb_XmlNames.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.label2.Location = new System.Drawing.Point(8, 63);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(49, 15);
			this.label2.TabIndex = 2;
			this.label2.Text = "Look in:";
			// 
			// _FFR_tb_Input
			// 
			this._FFR_tb_Input.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._FFR_tb_Input.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._FFR_tb_Input.Location = new System.Drawing.Point(8, 30);
			this._FFR_tb_Input.Name = "_FFR_tb_Input";
			this._FFR_tb_Input.Size = new System.Drawing.Size(275, 23);
			this._FFR_tb_Input.TabIndex = 1;
			this._FFR_tb_Input.TextChanged += new System.EventHandler(this._E_FFR_tb_Input_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.label1.Location = new System.Drawing.Point(8, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(62, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "Find what:";
			// 
			// _FFR_tc_Main_Replace
			// 
			this._FFR_tc_Main_Replace.Controls.Add(this._FFR_b_ReplacePrevious);
			this._FFR_tc_Main_Replace.Controls.Add(this._FFR_b_FindPreviousInReplace);
			this._FFR_tc_Main_Replace.Controls.Add(this._FFR_cb_CommentariesReplace);
			this._FFR_tc_Main_Replace.Controls.Add(this._FFR_cb_NodeNamesReplace);
			this._FFR_tc_Main_Replace.Controls.Add(this._FFR_cb_SelectedNodeReplace);
			this._FFR_tc_Main_Replace.Controls.Add(this._FFR_cb_ExactReplace);
			this._FFR_tc_Main_Replace.Controls.Add(this._FFR_cb_CaseReplace);
			this._FFR_tc_Main_Replace.Controls.Add(this.label9);
			this._FFR_tc_Main_Replace.Controls.Add(this._FFR_b_ReplaceAll);
			this._FFR_tc_Main_Replace.Controls.Add(this._FFR_b_ReplaceNext);
			this._FFR_tc_Main_Replace.Controls.Add(this._FFR_b_FindNextInReplace);
			this._FFR_tc_Main_Replace.Controls.Add(this._FFR_cb_XmlValuesReplace);
			this._FFR_tc_Main_Replace.Controls.Add(this.label8);
			this._FFR_tc_Main_Replace.Controls.Add(this._FFR_cob_AttNameReplace);
			this._FFR_tc_Main_Replace.Controls.Add(this.label7);
			this._FFR_tc_Main_Replace.Controls.Add(this._FFR_tb_ReplaceWith);
			this._FFR_tc_Main_Replace.Controls.Add(this.label4);
			this._FFR_tc_Main_Replace.Controls.Add(this._FFR_tb_ReplaceWhat);
			this._FFR_tc_Main_Replace.Controls.Add(this.label6);
			this._FFR_tc_Main_Replace.Location = new System.Drawing.Point(4, 22);
			this._FFR_tc_Main_Replace.Name = "_FFR_tc_Main_Replace";
			this._FFR_tc_Main_Replace.Padding = new System.Windows.Forms.Padding(3);
			this._FFR_tc_Main_Replace.Size = new System.Drawing.Size(291, 394);
			this._FFR_tc_Main_Replace.TabIndex = 1;
			this._FFR_tc_Main_Replace.Text = "Replace";
			this._FFR_tc_Main_Replace.UseVisualStyleBackColor = true;
			// 
			// _FFR_cb_CommentariesReplace
			// 
			this._FFR_cb_CommentariesReplace.AutoSize = true;
			this._FFR_cb_CommentariesReplace.Checked = true;
			this._FFR_cb_CommentariesReplace.CheckState = System.Windows.Forms.CheckState.Checked;
			this._FFR_cb_CommentariesReplace.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._FFR_cb_CommentariesReplace.Location = new System.Drawing.Point(11, 180);
			this._FFR_cb_CommentariesReplace.Name = "_FFR_cb_CommentariesReplace";
			this._FFR_cb_CommentariesReplace.Size = new System.Drawing.Size(104, 19);
			this._FFR_cb_CommentariesReplace.TabIndex = 26;
			this._FFR_cb_CommentariesReplace.Text = "Commentaries";
			this._FFR_cb_CommentariesReplace.UseVisualStyleBackColor = true;
			// 
			// _FFR_cb_NodeNamesReplace
			// 
			this._FFR_cb_NodeNamesReplace.AutoSize = true;
			this._FFR_cb_NodeNamesReplace.Checked = true;
			this._FFR_cb_NodeNamesReplace.CheckState = System.Windows.Forms.CheckState.Checked;
			this._FFR_cb_NodeNamesReplace.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._FFR_cb_NodeNamesReplace.Location = new System.Drawing.Point(11, 208);
			this._FFR_cb_NodeNamesReplace.Name = "_FFR_cb_NodeNamesReplace";
			this._FFR_cb_NodeNamesReplace.Size = new System.Drawing.Size(135, 19);
			this._FFR_cb_NodeNamesReplace.TabIndex = 25;
			this._FFR_cb_NodeNamesReplace.Text = "Mission node names";
			this._FFR_cb_NodeNamesReplace.UseVisualStyleBackColor = true;
			// 
			// _FFR_cb_SelectedNodeReplace
			// 
			this._FFR_cb_SelectedNodeReplace.AutoSize = true;
			this._FFR_cb_SelectedNodeReplace.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._FFR_cb_SelectedNodeReplace.Location = new System.Drawing.Point(11, 308);
			this._FFR_cb_SelectedNodeReplace.Name = "_FFR_cb_SelectedNodeReplace";
			this._FFR_cb_SelectedNodeReplace.Size = new System.Drawing.Size(140, 19);
			this._FFR_cb_SelectedNodeReplace.TabIndex = 24;
			this._FFR_cb_SelectedNodeReplace.Text = "Only in selected node";
			this._FFR_cb_SelectedNodeReplace.UseVisualStyleBackColor = true;
			// 
			// _FFR_cb_ExactReplace
			// 
			this._FFR_cb_ExactReplace.AutoSize = true;
			this._FFR_cb_ExactReplace.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._FFR_cb_ExactReplace.Location = new System.Drawing.Point(11, 283);
			this._FFR_cb_ExactReplace.Name = "_FFR_cb_ExactReplace";
			this._FFR_cb_ExactReplace.Size = new System.Drawing.Size(121, 19);
			this._FFR_cb_ExactReplace.TabIndex = 23;
			this._FFR_cb_ExactReplace.Text = "Match exact value";
			this._FFR_cb_ExactReplace.UseVisualStyleBackColor = true;
			// 
			// _FFR_cb_CaseReplace
			// 
			this._FFR_cb_CaseReplace.AutoSize = true;
			this._FFR_cb_CaseReplace.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._FFR_cb_CaseReplace.Location = new System.Drawing.Point(11, 258);
			this._FFR_cb_CaseReplace.Name = "_FFR_cb_CaseReplace";
			this._FFR_cb_CaseReplace.Size = new System.Drawing.Size(86, 19);
			this._FFR_cb_CaseReplace.TabIndex = 22;
			this._FFR_cb_CaseReplace.Text = "Match case";
			this._FFR_cb_CaseReplace.UseVisualStyleBackColor = true;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.label9.Location = new System.Drawing.Point(8, 239);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(76, 15);
			this.label9.TabIndex = 21;
			this.label9.Text = "Find options:";
			// 
			// _FFR_b_ReplaceAll
			// 
			this._FFR_b_ReplaceAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._FFR_b_ReplaceAll.AutoSize = true;
			this._FFR_b_ReplaceAll.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._FFR_b_ReplaceAll.Location = new System.Drawing.Point(7, 361);
			this._FFR_b_ReplaceAll.Name = "_FFR_b_ReplaceAll";
			this._FFR_b_ReplaceAll.Size = new System.Drawing.Size(88, 25);
			this._FFR_b_ReplaceAll.TabIndex = 20;
			this._FFR_b_ReplaceAll.Text = "Replace All";
			this._FFR_b_ReplaceAll.UseVisualStyleBackColor = true;
			this._FFR_b_ReplaceAll.Click += new System.EventHandler(this._E_FFR_b_ReplaceAll_Click);
			// 
			// _FFR_b_ReplaceNext
			// 
			this._FFR_b_ReplaceNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._FFR_b_ReplaceNext.AutoSize = true;
			this._FFR_b_ReplaceNext.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._FFR_b_ReplaceNext.Location = new System.Drawing.Point(195, 361);
			this._FFR_b_ReplaceNext.Name = "_FFR_b_ReplaceNext";
			this._FFR_b_ReplaceNext.Size = new System.Drawing.Size(88, 25);
			this._FFR_b_ReplaceNext.TabIndex = 19;
			this._FFR_b_ReplaceNext.Text = "Replace Next";
			this._FFR_b_ReplaceNext.UseVisualStyleBackColor = true;
			this._FFR_b_ReplaceNext.Click += new System.EventHandler(this._E_FFR_b_ReplaceNext_Click);
			// 
			// _FFR_b_FindNextInReplace
			// 
			this._FFR_b_FindNextInReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._FFR_b_FindNextInReplace.AutoSize = true;
			this._FFR_b_FindNextInReplace.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._FFR_b_FindNextInReplace.Location = new System.Drawing.Point(195, 330);
			this._FFR_b_FindNextInReplace.Name = "_FFR_b_FindNextInReplace";
			this._FFR_b_FindNextInReplace.Size = new System.Drawing.Size(88, 25);
			this._FFR_b_FindNextInReplace.TabIndex = 18;
			this._FFR_b_FindNextInReplace.Text = "Find Next";
			this._FFR_b_FindNextInReplace.UseVisualStyleBackColor = true;
			this._FFR_b_FindNextInReplace.Click += new System.EventHandler(this._E_FFR_b_FindNextInReplace_Click);
			// 
			// _FFR_cb_XmlValuesReplace
			// 
			this._FFR_cb_XmlValuesReplace.AutoSize = true;
			this._FFR_cb_XmlValuesReplace.Checked = true;
			this._FFR_cb_XmlValuesReplace.CheckState = System.Windows.Forms.CheckState.Checked;
			this._FFR_cb_XmlValuesReplace.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._FFR_cb_XmlValuesReplace.Location = new System.Drawing.Point(11, 133);
			this._FFR_cb_XmlValuesReplace.Name = "_FFR_cb_XmlValuesReplace";
			this._FFR_cb_XmlValuesReplace.Size = new System.Drawing.Size(131, 19);
			this._FFR_cb_XmlValuesReplace.TabIndex = 17;
			this._FFR_cb_XmlValuesReplace.Text = "Xml attribute values";
			this._FFR_cb_XmlValuesReplace.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this._FFR_cb_XmlValuesReplace.UseVisualStyleBackColor = true;
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label8.Location = new System.Drawing.Point(28, 158);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(134, 19);
			this.label8.TabIndex = 16;
			this.label8.Text = "where attribute name is:";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _FFR_cob_AttNameReplace
			// 
			this._FFR_cob_AttNameReplace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._FFR_cob_AttNameReplace.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._FFR_cob_AttNameReplace.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this._FFR_cob_AttNameReplace.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._FFR_cob_AttNameReplace.FormattingEnabled = true;
			this._FFR_cob_AttNameReplace.Location = new System.Drawing.Point(168, 156);
			this._FFR_cob_AttNameReplace.Name = "_FFR_cob_AttNameReplace";
			this._FFR_cob_AttNameReplace.Size = new System.Drawing.Size(115, 23);
			this._FFR_cob_AttNameReplace.TabIndex = 15;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.label7.Location = new System.Drawing.Point(8, 114);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(49, 15);
			this.label7.TabIndex = 6;
			this.label7.Text = "Look in:";
			// 
			// _FFR_tb_ReplaceWith
			// 
			this._FFR_tb_ReplaceWith.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._FFR_tb_ReplaceWith.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._FFR_tb_ReplaceWith.Location = new System.Drawing.Point(8, 81);
			this._FFR_tb_ReplaceWith.Name = "_FFR_tb_ReplaceWith";
			this._FFR_tb_ReplaceWith.Size = new System.Drawing.Size(275, 23);
			this._FFR_tb_ReplaceWith.TabIndex = 5;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.label4.Location = new System.Drawing.Point(8, 63);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(77, 15);
			this.label4.TabIndex = 4;
			this.label4.Text = "Replace with:";
			// 
			// _FFR_tb_ReplaceWhat
			// 
			this._FFR_tb_ReplaceWhat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._FFR_tb_ReplaceWhat.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._FFR_tb_ReplaceWhat.Location = new System.Drawing.Point(8, 30);
			this._FFR_tb_ReplaceWhat.Name = "_FFR_tb_ReplaceWhat";
			this._FFR_tb_ReplaceWhat.Size = new System.Drawing.Size(275, 23);
			this._FFR_tb_ReplaceWhat.TabIndex = 3;
			this._FFR_tb_ReplaceWhat.TextChanged += new System.EventHandler(this._E_FFR_tb_ReplaceWhat_TextChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.label6.Location = new System.Drawing.Point(8, 12);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(62, 15);
			this.label6.TabIndex = 2;
			this.label6.Text = "Find what:";
			// 
			// _FFR_b_ReplacePrevious
			// 
			this._FFR_b_ReplacePrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._FFR_b_ReplacePrevious.AutoSize = true;
			this._FFR_b_ReplacePrevious.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._FFR_b_ReplacePrevious.Location = new System.Drawing.Point(101, 361);
			this._FFR_b_ReplacePrevious.Name = "_FFR_b_ReplacePrevious";
			this._FFR_b_ReplacePrevious.Size = new System.Drawing.Size(88, 25);
			this._FFR_b_ReplacePrevious.TabIndex = 28;
			this._FFR_b_ReplacePrevious.Text = "Replace Prev.";
			this._FFR_b_ReplacePrevious.UseVisualStyleBackColor = true;
			this._FFR_b_ReplacePrevious.Click += new System.EventHandler(this._E_FFR_b_ReplacePrevious_Click);
			// 
			// _FFR_b_FindPreviousInReplace
			// 
			this._FFR_b_FindPreviousInReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._FFR_b_FindPreviousInReplace.AutoSize = true;
			this._FFR_b_FindPreviousInReplace.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._FFR_b_FindPreviousInReplace.Location = new System.Drawing.Point(101, 330);
			this._FFR_b_FindPreviousInReplace.Name = "_FFR_b_FindPreviousInReplace";
			this._FFR_b_FindPreviousInReplace.Size = new System.Drawing.Size(88, 25);
			this._FFR_b_FindPreviousInReplace.TabIndex = 27;
			this._FFR_b_FindPreviousInReplace.Text = "Find Previous";
			this._FFR_b_FindPreviousInReplace.UseVisualStyleBackColor = true;
			this._FFR_b_FindPreviousInReplace.Click += new System.EventHandler(this._E_FFR_b_FindPreviousInReplace_Click);
			// 
			// _FormFindReplace
			// 
			this.ClientSize = new System.Drawing.Size(299, 420);
			this.Controls.Add(this._FFR_tc_Main);
			this.HelpButton = true;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "_FormFindReplace";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Find and Replace";
			this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this._E_FFR_HelpButtonClicked);
			this.Activated += new System.EventHandler(this._E_FFR_Activated);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this._E_FFR_FormClosing);
			this.Load += new System.EventHandler(this._E_FFR_Load);
			this.VisibleChanged += new System.EventHandler(this._E_FFR_VisibleChanged);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this._E_FFR_KeyDown);
			this._FFR_tc_Main.ResumeLayout(false);
			this._FFR_tc_Main_Find.ResumeLayout(false);
			this._FFR_tc_Main_Find.PerformLayout();
			this._FFR_tc_Main_Replace.ResumeLayout(false);
			this._FFR_tc_Main_Replace.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.TabPage _FFR_tc_Main_Find;
		private System.Windows.Forms.TextBox _FFR_tb_Input;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabPage _FFR_tc_Main_Replace;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button _FFR_b_FindPrevious;
		private System.Windows.Forms.Button _FFR_b_FindNext;
		private System.Windows.Forms.CheckBox _FFR_cb_XmlValuesFind;
		private System.Windows.Forms.CheckBox _FFR_cb_XmlNames;
        private System.Windows.Forms.Button _FFR_b_FindAll;
        private System.Windows.Forms.ComboBox _FFR_cob_AttNameFind;
        private System.Windows.Forms.CheckBox _FFR_cb_DisplayedText;
        private System.Windows.Forms.CheckBox _FFR_cb_CommentariesFind;
        private System.Windows.Forms.CheckBox _FFR_cb_NodeNamesFind;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TabControl _FFR_tc_Main;
        private System.Windows.Forms.CheckBox _FFR_cb_ExactFind;
        private System.Windows.Forms.CheckBox _FFR_cb_CaseFind;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox _FFR_tb_ReplaceWhat;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox _FFR_tb_ReplaceWith;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox _FFR_cb_XmlValuesReplace;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox _FFR_cob_AttNameReplace;
        private System.Windows.Forms.CheckBox _FFR_cb_ExactReplace;
        private System.Windows.Forms.CheckBox _FFR_cb_CaseReplace;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button _FFR_b_ReplaceAll;
        private System.Windows.Forms.Button _FFR_b_ReplaceNext;
        private System.Windows.Forms.Button _FFR_b_FindNextInReplace;
        private System.Windows.Forms.CheckBox _FFR_cb_SelectedNodeFind;
        private System.Windows.Forms.CheckBox _FFR_cb_SelectedNodeReplace;
        private System.Windows.Forms.CheckBox _FFR_cb_CommentariesReplace;
        private System.Windows.Forms.CheckBox _FFR_cb_NodeNamesReplace;
		private System.Windows.Forms.Button _FFR_b_ReplacePrevious;
		private System.Windows.Forms.Button _FFR_b_FindPreviousInReplace;
	}
}
