using System;
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
    public partial class MemoryForm : Form
    {
        public MemoryForm(byte[] memory)
        {
            byte[] mem = memory;
            InitializeComponent();
            try
            {
                for (int i = 0; i < 1000; i++)
                    this.textBox1.AppendText("0x" + Convert.ToString(i, 16).PadLeft(4, '0') + ": 0x" + Convert.ToString(mem[i], 16).PadLeft(2, '0') + "\t");
            }
            catch (Exception e)
            {
                MessageBox.Show("Uninitialized Memory! Please Select a binary file and run!");
            }
        }
    }
}
