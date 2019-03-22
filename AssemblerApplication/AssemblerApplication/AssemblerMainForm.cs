﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssemblerApplication
{
    public partial class AssemblerMainForm : Form
    {
        public AssemblerMainForm()
        {
            InitializeComponent();
            assembler = new Assembler(this);
        }

        private void BrowseBtn_Click(object sender, EventArgs e)
        {
            try
            {
                /* String used to be displayed in ASMFileTextBox */
                String filename = "";
                /* Reinitialize the Text property of OutputTextBox */
                ASMShowTextBox.Text = "";
                /* Take the filename selected by user */
                filename = assembler.getFileName("ASM file for didactical processor(*.asm)|*.asm");
                /* Display the filename in ASMFileTextBox */
                FileNameTextBox.Text = filename != null ? filename : FileNameTextBox.Text;
                /* Enable/Disable the ParseFileButton depending of user choice */
                if (!filename.Equals(""))
                {
                    ConvertBtn.Enabled = true;
                }
                else
                {
                    ConvertBtn.Enabled = false;
                }
                assembler.OpenTestFile(filename);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConvertBtn_Click(object sender, EventArgs e)
        {
            assembler.generateBinaryFile();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }
}
