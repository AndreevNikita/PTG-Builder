namespace PTG_Builder
{
	partial class MainWindow
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.controlsPanel = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.viewModeComboBox = new System.Windows.Forms.ComboBox();
			this.addButton = new System.Windows.Forms.Button();
			this.removeButton = new System.Windows.Forms.Button();
			this.layersTabControl = new System.Windows.Forms.TabControl();
			this.textureTabPage = new System.Windows.Forms.TabPage();
			this.textureListbox = new PTG_Builder.PanelListbox();
			this.reliefTabPage = new System.Windows.Forms.TabPage();
			this.reliefListbox = new PTG_Builder.PanelListbox();
			this.recompileShaderButton = new System.Windows.Forms.Button();
			this.drawTimer = new System.Windows.Forms.Timer(this.components);
			this.glControlPanel = new System.Windows.Forms.Panel();
			this.inputTimer = new System.Windows.Forms.Timer(this.components);
			this.compileErrorLabel = new System.Windows.Forms.Label();
			this.bottomPanel = new System.Windows.Forms.Panel();
			this.openCodeButton = new System.Windows.Forms.Button();
			this.controlsPanel.SuspendLayout();
			this.layersTabControl.SuspendLayout();
			this.textureTabPage.SuspendLayout();
			this.reliefTabPage.SuspendLayout();
			this.bottomPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.Location = new System.Drawing.Point(48, 6);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(146, 20);
			this.textBox1.TabIndex = 1;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(200, 4);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(33, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "R";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(4, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Зерно";
			// 
			// controlsPanel
			// 
			this.controlsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.controlsPanel.Controls.Add(this.label2);
			this.controlsPanel.Controls.Add(this.viewModeComboBox);
			this.controlsPanel.Controls.Add(this.addButton);
			this.controlsPanel.Controls.Add(this.removeButton);
			this.controlsPanel.Controls.Add(this.layersTabControl);
			this.controlsPanel.Controls.Add(this.label1);
			this.controlsPanel.Controls.Add(this.button1);
			this.controlsPanel.Controls.Add(this.textBox1);
			this.controlsPanel.Location = new System.Drawing.Point(699, 12);
			this.controlsPanel.Name = "controlsPanel";
			this.controlsPanel.Size = new System.Drawing.Size(241, 500);
			this.controlsPanel.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(4, 102);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(76, 13);
			this.label2.TabIndex = 9;
			this.label2.Text = "Отображение";
			// 
			// viewModeComboBox
			// 
			this.viewModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.viewModeComboBox.FormattingEnabled = true;
			this.viewModeComboBox.Items.AddRange(new object[] {
            "Текстура + свет",
            "Нормали",
            "Нормали + свет",
            "Высоты"});
			this.viewModeComboBox.Location = new System.Drawing.Point(81, 99);
			this.viewModeComboBox.Name = "viewModeComboBox";
			this.viewModeComboBox.Size = new System.Drawing.Size(145, 21);
			this.viewModeComboBox.TabIndex = 8;
			this.viewModeComboBox.SelectedIndexChanged += new System.EventHandler(this.viewModeComboBox_SelectedIndexChanged);
			// 
			// addButton
			// 
			this.addButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.addButton.Location = new System.Drawing.Point(143, 476);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(43, 23);
			this.addButton.TabIndex = 6;
			this.addButton.Text = "+";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// removeButton
			// 
			this.removeButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.removeButton.Location = new System.Drawing.Point(191, 476);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size(42, 23);
			this.removeButton.TabIndex = 5;
			this.removeButton.Text = "-";
			this.removeButton.UseVisualStyleBackColor = true;
			this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
			// 
			// layersTabControl
			// 
			this.layersTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.layersTabControl.Controls.Add(this.textureTabPage);
			this.layersTabControl.Controls.Add(this.reliefTabPage);
			this.layersTabControl.Location = new System.Drawing.Point(3, 131);
			this.layersTabControl.Name = "layersTabControl";
			this.layersTabControl.SelectedIndex = 0;
			this.layersTabControl.Size = new System.Drawing.Size(235, 343);
			this.layersTabControl.TabIndex = 4;
			// 
			// textureTabPage
			// 
			this.textureTabPage.Controls.Add(this.textureListbox);
			this.textureTabPage.Location = new System.Drawing.Point(4, 22);
			this.textureTabPage.Name = "textureTabPage";
			this.textureTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.textureTabPage.Size = new System.Drawing.Size(227, 317);
			this.textureTabPage.TabIndex = 0;
			this.textureTabPage.Text = "Текстура";
			this.textureTabPage.UseVisualStyleBackColor = true;
			// 
			// textureListbox
			// 
			this.textureListbox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textureListbox.Location = new System.Drawing.Point(3, 3);
			this.textureListbox.Name = "textureListbox";
			this.textureListbox.Selection = null;
			this.textureListbox.Size = new System.Drawing.Size(221, 311);
			this.textureListbox.TabIndex = 1;
			// 
			// reliefTabPage
			// 
			this.reliefTabPage.Controls.Add(this.reliefListbox);
			this.reliefTabPage.Location = new System.Drawing.Point(4, 22);
			this.reliefTabPage.Name = "reliefTabPage";
			this.reliefTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.reliefTabPage.Size = new System.Drawing.Size(227, 317);
			this.reliefTabPage.TabIndex = 1;
			this.reliefTabPage.Text = "Рельеф";
			this.reliefTabPage.UseVisualStyleBackColor = true;
			// 
			// reliefListbox
			// 
			this.reliefListbox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.reliefListbox.Location = new System.Drawing.Point(3, 3);
			this.reliefListbox.Name = "reliefListbox";
			this.reliefListbox.Selection = null;
			this.reliefListbox.Size = new System.Drawing.Size(221, 311);
			this.reliefListbox.TabIndex = 0;
			// 
			// recompileShaderButton
			// 
			this.recompileShaderButton.Location = new System.Drawing.Point(3, 3);
			this.recompileShaderButton.Name = "recompileShaderButton";
			this.recompileShaderButton.Size = new System.Drawing.Size(98, 23);
			this.recompileShaderButton.TabIndex = 7;
			this.recompileShaderButton.Text = "Компилировать";
			this.recompileShaderButton.UseVisualStyleBackColor = true;
			this.recompileShaderButton.Click += new System.EventHandler(this.recompileShaderButton_Click);
			// 
			// drawTimer
			// 
			this.drawTimer.Interval = 16;
			this.drawTimer.Tick += new System.EventHandler(this.drawTimer_Tick);
			// 
			// glControlPanel
			// 
			this.glControlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.glControlPanel.Location = new System.Drawing.Point(12, 12);
			this.glControlPanel.Name = "glControlPanel";
			this.glControlPanel.Size = new System.Drawing.Size(681, 500);
			this.glControlPanel.TabIndex = 5;
			// 
			// inputTimer
			// 
			this.inputTimer.Interval = 16;
			this.inputTimer.Tick += new System.EventHandler(this.inputTimer_Tick);
			// 
			// compileErrorLabel
			// 
			this.compileErrorLabel.AutoSize = true;
			this.compileErrorLabel.BackColor = System.Drawing.Color.Transparent;
			this.compileErrorLabel.ForeColor = System.Drawing.Color.Red;
			this.compileErrorLabel.Location = new System.Drawing.Point(602, 8);
			this.compileErrorLabel.Name = "compileErrorLabel";
			this.compileErrorLabel.Size = new System.Drawing.Size(323, 13);
			this.compileErrorLabel.TabIndex = 11;
			this.compileErrorLabel.Text = "Ошибка компиляции (нажмите сюда для простмотра деталей)";
			this.compileErrorLabel.Visible = false;
			this.compileErrorLabel.Click += new System.EventHandler(this.compileErrorLabel_Click);
			// 
			// bottomPanel
			// 
			this.bottomPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.bottomPanel.Controls.Add(this.openCodeButton);
			this.bottomPanel.Controls.Add(this.compileErrorLabel);
			this.bottomPanel.Controls.Add(this.recompileShaderButton);
			this.bottomPanel.Location = new System.Drawing.Point(12, 517);
			this.bottomPanel.Name = "bottomPanel";
			this.bottomPanel.Size = new System.Drawing.Size(928, 32);
			this.bottomPanel.TabIndex = 6;
			// 
			// openCodeButton
			// 
			this.openCodeButton.Location = new System.Drawing.Point(107, 3);
			this.openCodeButton.Name = "openCodeButton";
			this.openCodeButton.Size = new System.Drawing.Size(98, 23);
			this.openCodeButton.TabIndex = 12;
			this.openCodeButton.Text = "Открыть код";
			this.openCodeButton.UseVisualStyleBackColor = true;
			this.openCodeButton.Click += new System.EventHandler(this.openCodeButton_Click);
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(941, 553);
			this.Controls.Add(this.bottomPanel);
			this.Controls.Add(this.glControlPanel);
			this.Controls.Add(this.controlsPanel);
			this.Name = "MainWindow";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "PTG Builder";
			this.Load += new System.EventHandler(this.MainWindow_Load);
			this.controlsPanel.ResumeLayout(false);
			this.controlsPanel.PerformLayout();
			this.layersTabControl.ResumeLayout(false);
			this.textureTabPage.ResumeLayout(false);
			this.reliefTabPage.ResumeLayout(false);
			this.bottomPanel.ResumeLayout(false);
			this.bottomPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel controlsPanel;
		private System.Windows.Forms.TabControl layersTabControl;
		private System.Windows.Forms.TabPage textureTabPage;
		private System.Windows.Forms.TabPage reliefTabPage;
		private System.Windows.Forms.Timer drawTimer;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.Panel glControlPanel;
		private System.Windows.Forms.Timer inputTimer;
		private System.Windows.Forms.Button recompileShaderButton;
		private System.Windows.Forms.ComboBox viewModeComboBox;
		private System.Windows.Forms.Label label2;
		private PanelListbox reliefListbox;
		private System.Windows.Forms.Label compileErrorLabel;
		private System.Windows.Forms.Panel bottomPanel;
		private System.Windows.Forms.Button openCodeButton;
		private PanelListbox textureListbox;
	}
}

