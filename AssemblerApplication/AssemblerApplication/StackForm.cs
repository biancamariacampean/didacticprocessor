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
    public partial class StackForm : Form
    {
        public StackForm(byte[] mem)
        {
            byte[] memory = mem;
            InitializeComponent();
            try
            {
                for (int i = UInt16.MaxValue - 1 - 1024; i >= UInt16.MaxValue - 1 - 2024; i--)
                {
                    this.textBox1.AppendText("0x" + Convert.ToString(i, 16).PadLeft(4, '0') + ": 0x" + Convert.ToString(mem[i], 16).PadLeft(2, '0') + "\t");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Uninitialized memory! Please select a binary file and run!");
            }
        }
    }
}
