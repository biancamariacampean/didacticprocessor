namespace AssemblerApplication
{
    partial class AssemblerMainForm
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
            this.FileNameTextBox = new System.Windows.Forms.TextBox();
            this.BrowseBtn = new System.Windows.Forms.Button();
            this.ASMShowTextBox = new System.Windows.Forms.TextBox();
            this.ConvertBtn = new System.Windows.Forms.Button();
            this.ResultTextBox = new System.Windows.Forms.TextBox();
            this.ExitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // FileNameTextBox
            // 
            this.FileNameTextBox.BackColor = System.Drawing.Color.AliceBlue;
            this.FileNameTextBox.Location = new System.Drawing.Point(12, 34);
            this.FileNameTextBox.Multiline = true;
            this.FileNameTextBox.Name = "FileNameTextBox";
            this.FileNameTextBox.Size = new System.Drawing.Size(514, 27);
            this.FileNameTextBox.TabIndex = 0;
            // 
            // BrowseBtn
            // 
            this.BrowseBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BrowseBtn.Location = new System.Drawing.Point(532, 34);
            this.BrowseBtn.Name = "BrowseBtn";
            this.BrowseBtn.Size = new System.Drawing.Size(175, 27);
            this.BrowseBtn.TabIndex = 1;
            this.BrowseBtn.Text = "Browse ASM File";
            this.BrowseBtn.UseVisualStyleBackColor = true;
            this.BrowseBtn.Click += new System.EventHandler(this.BrowseBtn_Click);
            // 
            // ASMShowTextBox
            // 
            this.ASMShowTextBox.BackColor = System.Drawing.Color.SkyBlue;
            this.ASMShowTextBox.Location = new System.Drawing.Point(12, 106);
            this.ASMShowTextBox.Multiline = true;
            this.ASMShowTextBox.Name = "ASMShowTextBox";
            this.ASMShowTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ASMShowTextBox.Size = new System.Drawing.Size(344, 314);
            this.ASMShowTextBox.TabIndex = 2;
            // 
            // ConvertBtn
            // 
            this.ConvertBtn.Enabled = false;
            this.ConvertBtn.Location = new System.Drawing.Point(37, 67);
            this.ConvertBtn.Name = "ConvertBtn";
            this.ConvertBtn.Size = new System.Drawing.Size(256, 33);
            this.ConvertBtn.TabIndex = 3;
            this.ConvertBtn.Text = "Convert";
            this.ConvertBtn.UseVisualStyleBackColor = true;
            this.ConvertBtn.Click += new System.EventHandler(this.ConvertBtn_Click);
            // 
            // ResultTextBox
            // 
            this.ResultTextBox.BackColor = System.Drawing.Color.SkyBlue;
            this.ResultTextBox.Location = new System.Drawing.Point(373, 106);
            this.ResultTextBox.Multiline = true;
            this.ResultTextBox.Name = "ResultTextBox";
            this.ResultTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ResultTextBox.Size = new System.Drawing.Size(344, 314);
            this.ResultTextBox.TabIndex = 4;
            // 
            // ExitButton
            // 
            this.ExitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExitButton.Location = new System.Drawing.Point(417, 67);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(271, 33);
            this.ExitButton.TabIndex = 5;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // AssemblerMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.ClientSize = new System.Drawing.Size(739, 433);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.ResultTextBox);
            this.Controls.Add(this.ConvertBtn);
            this.Controls.Add(this.ASMShowTextBox);
            this.Controls.Add(this.BrowseBtn);
            this.Controls.Add(this.FileNameTextBox);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "AssemblerMainForm";
            this.Text = "Assembler";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox FileNameTextBox;
        public System.Windows.Forms.Button BrowseBtn;
        public System.Windows.Forms.TextBox ASMShowTextBox;
        public Assembler assembler;
        public System.Windows.Forms.Button ConvertBtn;
        public System.Windows.Forms.TextBox ResultTextBox;
        private System.Windows.Forms.Button ExitButton;
    }
}

