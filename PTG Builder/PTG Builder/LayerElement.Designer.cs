namespace PTG_Builder
{
	partial class LayerElement
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
			this.nameLabel = new System.Windows.Forms.Label();
			this.visiblityCheckBox = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// nameLabel
			// 
			this.nameLabel.AutoSize = true;
			this.nameLabel.Location = new System.Drawing.Point(129, 27);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(35, 13);
			this.nameLabel.TabIndex = 0;
			this.nameLabel.Text = "label1";
			this.nameLabel.Click += new System.EventHandler(this.nameLabel_Click);
			// 
			// visiblityCheckBox
			// 
			this.visiblityCheckBox.AutoSize = true;
			this.visiblityCheckBox.Location = new System.Drawing.Point(14, 26);
			this.visiblityCheckBox.Name = "visiblityCheckBox";
			this.visiblityCheckBox.Size = new System.Drawing.Size(82, 17);
			this.visiblityCheckBox.TabIndex = 1;
			this.visiblityCheckBox.Text = "Видимость";
			this.visiblityCheckBox.UseVisualStyleBackColor = true;
			this.visiblityCheckBox.CheckedChanged += new System.EventHandler(this.visiblityCheckBox_CheckedChanged);
			// 
			// LayerElement
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.visiblityCheckBox);
			this.Controls.Add(this.nameLabel);
			this.Name = "LayerElement";
			this.Size = new System.Drawing.Size(261, 68);
			this.DoubleClick += new System.EventHandler(this.LayerElement_DoubleClick);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.CheckBox visiblityCheckBox;
	}
}
