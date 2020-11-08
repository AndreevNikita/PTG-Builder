namespace PTG_Builder
{
	partial class LayerSettings
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
			this.variablesListBox = new System.Windows.Forms.ListBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.valueTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.nameLabel = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.addVariableButton = new System.Windows.Forms.Button();
			this.removeVariableButton = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.showVarsMessageBox = new System.Windows.Forms.Button();
			this.showFunctionsDialog = new System.Windows.Forms.Button();
			this.functionTextBox = new System.Windows.Forms.TextBox();
			this.okButton = new System.Windows.Forms.Button();
			this.pickButton = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.overlayComboBox = new System.Windows.Forms.ComboBox();
			this.panel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// variablesListBox
			// 
			this.variablesListBox.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.variablesListBox.FormattingEnabled = true;
			this.variablesListBox.ItemHeight = 20;
			this.variablesListBox.Location = new System.Drawing.Point(6, 27);
			this.variablesListBox.Name = "variablesListBox";
			this.variablesListBox.Size = new System.Drawing.Size(180, 164);
			this.variablesListBox.TabIndex = 0;
			this.variablesListBox.SelectedValueChanged += new System.EventHandler(this.variablesListBox_SelectedValueChanged);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.pickButton);
			this.panel1.Controls.Add(this.valueTextBox);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.nameLabel);
			this.panel1.Location = new System.Drawing.Point(192, 27);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(196, 173);
			this.panel1.TabIndex = 1;
			// 
			// valueTextBox
			// 
			this.valueTextBox.Location = new System.Drawing.Point(7, 75);
			this.valueTextBox.Name = "valueTextBox";
			this.valueTextBox.Size = new System.Drawing.Size(175, 22);
			this.valueTextBox.TabIndex = 7;
			this.valueTextBox.TextChanged += new System.EventHandler(this.valueTextBox_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(4, 47);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(59, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Значение";
			// 
			// nameLabel
			// 
			this.nameLabel.AutoSize = true;
			this.nameLabel.Location = new System.Drawing.Point(4, 15);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(29, 13);
			this.nameLabel.TabIndex = 3;
			this.nameLabel.Text = "Имя";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.addVariableButton);
			this.groupBox1.Controls.Add(this.removeVariableButton);
			this.groupBox1.Controls.Add(this.variablesListBox);
			this.groupBox1.Controls.Add(this.panel1);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(405, 234);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Переменные";
			// 
			// addVariableButton
			// 
			this.addVariableButton.Location = new System.Drawing.Point(153, 205);
			this.addVariableButton.Name = "addVariableButton";
			this.addVariableButton.Size = new System.Drawing.Size(33, 23);
			this.addVariableButton.TabIndex = 4;
			this.addVariableButton.Text = "+";
			this.addVariableButton.UseVisualStyleBackColor = true;
			this.addVariableButton.Click += new System.EventHandler(this.addVariableButton_Click);
			// 
			// removeVariableButton
			// 
			this.removeVariableButton.Location = new System.Drawing.Point(117, 205);
			this.removeVariableButton.Name = "removeVariableButton";
			this.removeVariableButton.Size = new System.Drawing.Size(30, 23);
			this.removeVariableButton.TabIndex = 3;
			this.removeVariableButton.Text = "-";
			this.removeVariableButton.UseVisualStyleBackColor = true;
			this.removeVariableButton.Click += new System.EventHandler(this.removeVariableButton_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.showVarsMessageBox);
			this.groupBox2.Controls.Add(this.showFunctionsDialog);
			this.groupBox2.Controls.Add(this.functionTextBox);
			this.groupBox2.Location = new System.Drawing.Point(12, 252);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(405, 210);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "f(x, z)";
			// 
			// showVarsMessageBox
			// 
			this.showVarsMessageBox.Location = new System.Drawing.Point(99, 181);
			this.showVarsMessageBox.Name = "showVarsMessageBox";
			this.showVarsMessageBox.Size = new System.Drawing.Size(152, 23);
			this.showVarsMessageBox.TabIndex = 5;
			this.showVarsMessageBox.Text = "Доступные переменные";
			this.showVarsMessageBox.UseVisualStyleBackColor = true;
			this.showVarsMessageBox.Click += new System.EventHandler(this.showVarsMessageBox_Click);
			// 
			// showFunctionsDialog
			// 
			this.showFunctionsDialog.Location = new System.Drawing.Point(257, 181);
			this.showFunctionsDialog.Name = "showFunctionsDialog";
			this.showFunctionsDialog.Size = new System.Drawing.Size(142, 23);
			this.showFunctionsDialog.TabIndex = 4;
			this.showFunctionsDialog.Text = "Фунции по умолчанию";
			this.showFunctionsDialog.UseVisualStyleBackColor = true;
			this.showFunctionsDialog.Click += new System.EventHandler(this.showFunctionsDialog_Click);
			// 
			// functionTextBox
			// 
			this.functionTextBox.Location = new System.Drawing.Point(6, 19);
			this.functionTextBox.Multiline = true;
			this.functionTextBox.Name = "functionTextBox";
			this.functionTextBox.Size = new System.Drawing.Size(393, 158);
			this.functionTextBox.TabIndex = 0;
			this.functionTextBox.TextChanged += new System.EventHandler(this.functionTextBox_TextChanged);
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.Location = new System.Drawing.Point(563, 450);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 6;
			this.okButton.Text = "ОК";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// pickButton
			// 
			this.pickButton.Location = new System.Drawing.Point(40, 101);
			this.pickButton.Name = "pickButton";
			this.pickButton.Size = new System.Drawing.Size(108, 23);
			this.pickButton.TabIndex = 8;
			this.pickButton.Text = "Подбор";
			this.pickButton.UseVisualStyleBackColor = true;
			this.pickButton.Click += new System.EventHandler(this.pickButton_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.overlayComboBox);
			this.groupBox3.Controls.Add(this.label1);
			this.groupBox3.Location = new System.Drawing.Point(423, 12);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(221, 234);
			this.groupBox3.TabIndex = 7;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Найстроки слоя";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(69, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Наложение";
			// 
			// overlayComboBox
			// 
			this.overlayComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.overlayComboBox.FormattingEnabled = true;
			this.overlayComboBox.Items.AddRange(new object[] {
            "Сложение",
            "Вычитание",
            "Умножение",
            "Наименьшее",
            "Наибольшее"});
			this.overlayComboBox.Location = new System.Drawing.Point(81, 15);
			this.overlayComboBox.Name = "overlayComboBox";
			this.overlayComboBox.Size = new System.Drawing.Size(134, 21);
			this.overlayComboBox.TabIndex = 1;
			this.overlayComboBox.SelectedIndexChanged += new System.EventHandler(this.overlayComboBox_SelectedIndexChanged);
			// 
			// LayerSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(656, 485);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Name = "LayerSettings";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Слой";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LayerSettings_FormClosing);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox variablesListBox;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button addVariableButton;
		private System.Windows.Forms.Button removeVariableButton;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button showFunctionsDialog;
		private System.Windows.Forms.TextBox functionTextBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button showVarsMessageBox;
		private System.Windows.Forms.TextBox valueTextBox;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button pickButton;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox overlayComboBox;
	}
}