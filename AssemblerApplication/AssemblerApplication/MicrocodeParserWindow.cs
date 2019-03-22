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
    public partial class MicrocodeParserWindow : Form
    {
        private MicrocodeParser microcodeParser;
        public MicrocodeParserWindow(MicrocodeParser microcodeParser)
        {
            InitializeComponent();
            this.microcodeParser = microcodeParser;
        }

        private void BrowseBtn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "CSV file (*.csv)|*.csv";
                ofd.InitialDirectory = Environment.CurrentDirectory;
                ofd.Title = "Choose a file";

                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    if (ofd.FileName != null)
                    {
                        fileNameTB.Text = ofd.FileName;
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

            ParseResultTB.Text = "";
        }

        private void ParseBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string microcodePath = microcodeParser.ParseFile(fileNameTB.Text);
                ParseResultTB.Text = "Parsing successful. File created at: " + microcodePath;
            }
            catch (Exception exception)
            {
                ParseResultTB.Text = "Error: " + exception.Message + "\r\n";
            }
        }
    }
}
