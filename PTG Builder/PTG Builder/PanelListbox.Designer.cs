namespace PTG_Builder
{
	partial class PanelListbox
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

		#region Код, автоматически созданный конструктором компонентов

		/// <summary> 
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.vScrollBar = new System.Windows.Forms.VScrollBar();
			this.viewPanel = new System.Windows.Forms.Panel();
			this.panelsContainerPanel = new System.Windows.Forms.Panel();
			this.viewPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// vScrollBar
			// 
			this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
			this.vScrollBar.Location = new System.Drawing.Point(280, 0);
			this.vScrollBar.Maximum = 40;
			this.vScrollBar.Name = "vScrollBar";
			this.vScrollBar.Size = new System.Drawing.Size(20, 364);
			this.vScrollBar.TabIndex = 0;
			this.vScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar_Scroll);
			// 
			// viewPanel
			// 
			this.viewPanel.Controls.Add(this.panelsContainerPanel);
			this.viewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.viewPanel.Location = new System.Drawing.Point(0, 0);
			this.viewPanel.Name = "viewPanel";
			this.viewPanel.Size = new System.Drawing.Size(280, 364);
			this.viewPanel.TabIndex = 1;
			// 
			// panelsContainerPanel
			// 
			this.panelsContainerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.panelsContainerPanel.Location = new System.Drawing.Point(3, 3);
			this.panelsContainerPanel.Name = "panelsContainerPanel";
			this.panelsContainerPanel.Size = new System.Drawing.Size(274, 93);
			this.panelsContainerPanel.TabIndex = 0;
			// 
			// PanelListbox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.viewPanel);
			this.Controls.Add(this.vScrollBar);
			this.Name = "PanelListbox";
			this.Size = new System.Drawing.Size(300, 364);
			this.viewPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.VScrollBar vScrollBar;
		private System.Windows.Forms.Panel viewPanel;
		private System.Windows.Forms.Panel panelsContainerPanel;
	}
}
