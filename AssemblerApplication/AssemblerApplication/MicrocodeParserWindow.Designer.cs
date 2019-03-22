namespace AssemblerApplication
{
    partial class MicrocodeParserWindow
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
            this.BrowseBtn = new System.Windows.Forms.Button();
            this.ParseBtn = new System.Windows.Forms.Button();
            this.ParseResultTB = new System.Windows.Forms.TextBox();
            this.fileNameTB = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // BrowseBtn
            // 
            this.BrowseBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BrowseBtn.Location = new System.Drawing.Point(562, 12);
            this.BrowseBtn.Name = "BrowseBtn";
            this.BrowseBtn.Size = new System.Drawing.Size(151, 31);
            this.BrowseBtn.TabIndex = 0;
            this.BrowseBtn.Text = "Browse";
            this.BrowseBtn.UseVisualStyleBackColor = true;
            this.BrowseBtn.Click += new System.EventHandler(this.BrowseBtn_Click);
            // 
            // ParseBtn
            // 
            this.ParseBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ParseBtn.Location = new System.Drawing.Point(197, 49);
            this.ParseBtn.Name = "ParseBtn";
            this.ParseBtn.Size = new System.Drawing.Size(313, 31);
            this.ParseBtn.TabIndex = 1;
            this.ParseBtn.Text = "Parse";
            this.ParseBtn.UseVisualStyleBackColor = true;
            this.ParseBtn.Click += new System.EventHandler(this.ParseBtn_Click);
            // 
            // ParseResultTB
            // 
            this.ParseResultTB.BackColor = System.Drawing.Color.SkyBlue;
            this.ParseResultTB.Location = new System.Drawing.Point(12, 86);
            this.ParseResultTB.Multiline = true;
            this.ParseResultTB.Name = "ParseResultTB";
            this.ParseResultTB.Size = new System.Drawing.Size(715, 335);
            this.ParseResultTB.TabIndex = 2;
            // 
            // fileNameTB
            // 
            this.fileNameTB.BackColor = System.Drawing.Color.AliceBlue;
            this.fileNameTB.Location = new System.Drawing.Point(24, 12);
            this.fileNameTB.Multiline = true;
            this.fileNameTB.Name = "fileNameTB";
            this.fileNameTB.Size = new System.Drawing.Size(512, 31);
            this.fileNameTB.TabIndex = 3;
            // 
            // MicrocodeParserWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.ClientSize = new System.Drawing.Size(739, 433);
            this.Controls.Add(this.fileNameTB);
            this.Controls.Add(this.ParseResultTB);
            this.Controls.Add(this.ParseBtn);
            this.Controls.Add(this.BrowseBtn);
            this.Name = "MicrocodeParserWindow";
            this.Text = "Microcode Parser";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BrowseBtn;
        private System.Windows.Forms.Button ParseBtn;
        private System.Windows.Forms.TextBox ParseResultTB;
        private System.Windows.Forms.TextBox fileNameTB;
    }
}