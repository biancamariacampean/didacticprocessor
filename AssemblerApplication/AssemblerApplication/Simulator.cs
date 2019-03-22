using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssemblerApplication
{
    public partial class Simulator : Form
    {
        
        public Simulator()
        {
            InitializeComponent();
            secventiator = new Secventiator(this);
            resetAllColors();
        }

        private void newBinaryFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AssemblerMainForm asm = new AssemblerMainForm();
            asm.Show();
        }

        private void openBinaryFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            secventiator.fillMemory();
        }


        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            secventiator.ResetSeq();
            secventiator.Start();
            StepBtn.Enabled = true;
            ShowHexBtn.Enabled = true;
        }

        private void StepBtn_Click(object sender, EventArgs e)
        {
            ControlTextBox.Text = "";
            while (secventiator.GetState() != Convert.ToByte(2))
            {
                secventiator.Start();
            }
            resetAllColors();
            switch (secventiator.sBUS)
            {
                case Secventiator.SursaSBUS.NONE:
                    {
                        ALUSbusTB.Text = "NONE";
                        break;
                    }

                case Secventiator.SursaSBUS.PD_FLAG:
                    {
                        
                        PdFLAGsLine.BorderColor = Color.Yellow;
                        SBUSLine.BorderColor = Color.Yellow;
                        ALUSbusLine1.BorderColor = Color.Yellow;
                        ALUSbusLine2.BorderColor = Color.Yellow;
                        if (ShowHexBtn.Text == "Show Hex")
                            ALUSbusTB.Text = Convert.ToString(secventiator.GetSBUS(), 2).PadLeft(16, '0');
                        else
                            ALUSbusTB.Text = "0x" + Convert.ToString(secventiator.GetSBUS(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.SursaSBUS.PD_RG:
                    {
                        PdRGsLine.BorderColor = Color.Yellow;
                        SBUSLine.BorderColor = Color.Yellow;
                        ALUSbusLine1.BorderColor = Color.Yellow;
                        ALUSbusLine2.BorderColor = Color.Yellow;
                        if (ShowHexBtn.Text == "Show Hex")
                            ALUSbusTB.Text = Convert.ToString(secventiator.GetSBUS(), 2).PadLeft(16, '0');
                        else
                            ALUSbusTB.Text = "0x" + Convert.ToString(secventiator.GetSBUS(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.SursaSBUS.PD_SP:
                    {
                        PdSPsLine.BorderColor = Color.Yellow;
                        SBUSLine.BorderColor = Color.Yellow;
                        ALUSbusLine1.BorderColor = Color.Yellow;
                        ALUSbusLine2.BorderColor = Color.Yellow;
                        if (ShowHexBtn.Text == "Show Hex")
                            ALUSbusTB.Text = Convert.ToString(secventiator.GetSBUS(), 2).PadLeft(16, '0');
                        else
                            ALUSbusTB.Text = "0x" + Convert.ToString(secventiator.GetSBUS(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.SursaSBUS.PD_T:
                    {
                        PdTsLine.BorderColor = Color.Yellow;
                        SBUSLine.BorderColor = Color.Yellow;
                        ALUSbusLine1.BorderColor = Color.Yellow;
                        ALUSbusLine2.BorderColor = Color.Yellow;
                        if (ShowHexBtn.Text == "Show Hex")
                            ALUSbusTB.Text = Convert.ToString(secventiator.GetSBUS(), 2).PadLeft(16, '0');
                        else
                            ALUSbusTB.Text = "0x" + Convert.ToString(secventiator.GetSBUS(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.SursaSBUS.PD_NT:
                    {
                        PdNTsLine.BorderColor = Color.Yellow;
                        SBUSLine.BorderColor = Color.Yellow;
                        ALUSbusLine1.BorderColor = Color.Yellow;
                        ALUSbusLine2.BorderColor = Color.Yellow;
                        if (ShowHexBtn.Text == "Show Hex")
                            ALUSbusTB.Text = Convert.ToString(secventiator.GetSBUS(), 2).PadLeft(16, '0');
                        else
                            ALUSbusTB.Text = "0x" + Convert.ToString(secventiator.GetSBUS(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.SursaSBUS.PD_PC:
                    {
                        PdPCsLine.BorderColor = Color.Yellow;
                        SBUSLine.BorderColor = Color.Yellow;
                        ALUSbusLine1.BorderColor = Color.Yellow;
                        ALUSbusLine2.BorderColor = Color.Yellow;
                        if (ShowHexBtn.Text == "Show Hex")
                            ALUSbusTB.Text = Convert.ToString(secventiator.GetSBUS(), 2).PadLeft(16, '0');
                        else
                            ALUSbusTB.Text = "0x" + Convert.ToString(secventiator.GetSBUS(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.SursaSBUS.PD_IVR:
                    {
                        PdIVRsLine.BorderColor = Color.Yellow;
                        SBUSLine.BorderColor = Color.Yellow;
                        ALUSbusLine1.BorderColor = Color.Yellow;
                        ALUSbusLine2.BorderColor = Color.Yellow;
                        if (ShowHexBtn.Text == "Show Hex")
                            ALUSbusTB.Text = Convert.ToString(secventiator.GetSBUS(), 2).PadLeft(16, '0');
                        else
                            ALUSbusTB.Text = "0x" + Convert.ToString(secventiator.GetSBUS(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.SursaSBUS.PD_MDR:
                    {
                        PdMDRsLine.BorderColor = Color.Yellow;
                        SBUSLine.BorderColor = Color.Yellow;
                        ALUSbusLine1.BorderColor = Color.Yellow;
                        ALUSbusLine2.BorderColor = Color.Yellow;
                        if (ShowHexBtn.Text == "Show Hex")
                            ALUSbusTB.Text = Convert.ToString(secventiator.GetSBUS(), 2).PadLeft(16, '0');
                        else
                            ALUSbusTB.Text = "0x" + Convert.ToString(secventiator.GetSBUS(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.SursaSBUS.PD_0:
                    {
                        Pd0SLine.BorderColor = Color.Yellow;
                        SBUSLine.BorderColor = Color.Yellow;
                        ALUSbusLine1.BorderColor = Color.Yellow;
                        ALUSbusLine2.BorderColor = Color.Yellow;
                        if (ShowHexBtn.Text == "Show Hex")
                            ALUSbusTB.Text = Convert.ToString(secventiator.GetSBUS(), 2).PadLeft(16, '0');
                        else
                            ALUSbusTB.Text = "0x" + Convert.ToString(secventiator.GetSBUS(), 2).PadLeft(4, '0');
                        break;
                    }
            }
            switch (secventiator.dBUS)
            {
                case Secventiator.SursaDBUS.NONE:
                    {
                        ALUDbusTB.Text = "NONE";
                        break;
                    }

                case Secventiator.SursaDBUS.PD_IR_OFF:
                    {
                        PdIROFdLine.BorderColor = Color.Yellow;
                        DBUSLine.BorderColor = Color.Yellow;
                        ALUDbusLine.BorderColor = Color.Yellow;
                        if (ShowHexBtn.Text == "Show Hex")
                            ALUDbusTB.Text = Convert.ToString(secventiator.GetDBUS(), 2).PadLeft(16, '0');
                        else
                            ALUDbusTB.Text = "0x" + Convert.ToString(secventiator.GetDBUS(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.SursaDBUS.PD_RG:
                    {
                        PdRGdLine.BorderColor = Color.Yellow;
                        DBUSLine.BorderColor = Color.Yellow;
                        ALUDbusLine.BorderColor = Color.Yellow;
                        if (ShowHexBtn.Text == "Show ex")
                            ALUDbusTB.Text = Convert.ToString(secventiator.GetDBUS(), 2).PadLeft(16, '0');
                        else
                            ALUDbusTB.Text = "0x" + Convert.ToString(secventiator.GetDBUS(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.SursaDBUS.PD_MDR:
                    {
                        PdMDRdLine.BorderColor = Color.Yellow;
                        DBUSLine.BorderColor = Color.Yellow;
                        ALUDbusLine.BorderColor = Color.Yellow;
                        if (ShowHexBtn.Text == "Show Hex")
                            ALUDbusTB.Text = Convert.ToString(secventiator.GetDBUS(), 2).PadLeft(16, '0');
                        else
                            ALUDbusTB.Text = "0x" + Convert.ToString(secventiator.GetDBUS(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.SursaDBUS.PD_N1:
                    {
                        PdN1dLine.BorderColor = Color.Yellow;
                        DBUSLine.BorderColor = Color.Yellow;
                        ALUDbusLine.BorderColor = Color.Yellow;
                        if (ShowHexBtn.Text == "Show Hex")
                            ALUDbusTB.Text = Convert.ToString(secventiator.GetDBUS(), 2).PadLeft(16, '0');
                        else
                            ALUDbusTB.Text = "0x" + Convert.ToString(secventiator.GetDBUS(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.SursaDBUS.PD_0:
                    {
                        Pd0dLine.BorderColor = Color.Yellow;
                        DBUSLine.BorderColor = Color.Yellow;
                        ALUDbusLine.BorderColor = Color.Yellow;
                        if (ShowHexBtn.Text == "Show Hex")
                            ALUDbusTB.Text = Convert.ToString(secventiator.GetDBUS(), 2).PadLeft(16, '0');
                        else
                            ALUDbusTB.Text = "0x" + Convert.ToString(secventiator.GetDBUS(), 16).PadLeft(4, '0');
                        break;
                    }
            }
            switch (secventiator.alu)
            {
                case Secventiator.ALU.NONE:
                    { 
                        OPALUTextBox.Text = "NONE";
                        break;
                    }

                case Secventiator.ALU.NSBUS:
                    {
                        RBUSLine.BorderColor = Color.Yellow;
                        RBUSLine2.BorderColor = Color.Yellow;
                        RBUSLine3.BorderColor = Color.Yellow;
                        ALURbusLine.BorderColor = Color.Yellow;
                        OPALUTextBox.Text = "NSBUS";
                        if (ShowHexBtn.Text == "Show Hex")
                            ALURbusTB.Text = Convert.ToString(secventiator.GetRBUS(), 2).PadLeft(16, '0');
                        else
                            ALURbusTB.Text = "0x" + Convert.ToString(secventiator.GetRBUS(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.ALU.SUM:
                    {

                        if (secventiator.GetSBUSINT() || secventiator.GetDBUSINT())
                        {
                            Int32 fullResult = (secventiator.GetSBUS() + secventiator.GetDBUS());
                            if (ShowHexBtn.Text == "Show Hex")
                                ALURbusTB.Text = Convert.ToString(fullResult, 2).PadLeft(16, '0');
                            else
                                ALURbusTB.Text = "0x" + Convert.ToString(fullResult, 16).PadLeft(4, '0');
                        }
                        else
                        {
                            UInt32 fullResult = (UInt32)(UInt16)secventiator.GetSBUS() + (UInt32)(UInt16)secventiator.GetDBUS();
                            if (ShowHexBtn.Text == "Show Hex")
                                ALURbusTB.Text = Convert.ToString(fullResult, 2).PadLeft(16, '0');
                            else
                                ALURbusTB.Text = "0x" + Convert.ToString(fullResult, 16).PadLeft(4, '0');
                        }
                        RBUSLine.BorderColor = Color.Yellow;
                        RBUSLine2.BorderColor = Color.Yellow;
                        RBUSLine3.BorderColor = Color.Yellow;
                        ALURbusLine.BorderColor = Color.Yellow;
                        OPALUTextBox.Text = "SUM";
                        break;
                    }

                case Secventiator.ALU.AND:
                    {
                        RBUSLine.BorderColor = Color.Yellow;
                        RBUSLine2.BorderColor = Color.Yellow;
                        RBUSLine3.BorderColor = Color.Yellow;
                        ALURbusLine.BorderColor = Color.Yellow;
                        OPALUTextBox.Text = "AND";
                        if (ShowHexBtn.Text == "Show Hex")
                            ALURbusTB.Text = Convert.ToString(secventiator.GetRBUS(), 2).PadLeft(16, '0');
                        else
                            ALURbusTB.Text = "0x" + Convert.ToString(secventiator.GetRBUS(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.ALU.OR:
                    {
                        RBUSLine.BorderColor = Color.Yellow;
                        RBUSLine2.BorderColor = Color.Yellow;
                        RBUSLine3.BorderColor = Color.Yellow;
                        ALURbusLine.BorderColor = Color.Yellow;
                        OPALUTextBox.Text = "OR";
                        if (ShowHexBtn.Text == "Show Hex")
                            ALURbusTB.Text = Convert.ToString(secventiator.GetRBUS(), 2).PadLeft(16, '0');
                        else
                            ALURbusTB.Text = "0x" + Convert.ToString(secventiator.GetRBUS(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.ALU.XOR:
                    {
                        RBUSLine.BorderColor = Color.Yellow;
                        RBUSLine2.BorderColor = Color.Yellow;
                        RBUSLine3.BorderColor = Color.Yellow;
                        ALURbusLine.BorderColor = Color.Yellow;
                        OPALUTextBox.Text = "XOR";
                        if (ShowHexBtn.Text == "Show Hex")
                            ALURbusTB.Text = Convert.ToString(secventiator.GetRBUS(), 2).PadLeft(16, '0');
                        else
                            ALURbusTB.Text = "0x" + Convert.ToString(secventiator.GetRBUS(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.ALU.ASL:
                    {
                        RBUSLine.BorderColor = Color.Yellow;
                        RBUSLine2.BorderColor = Color.Yellow;
                        RBUSLine3.BorderColor = Color.Yellow;
                        ALURbusLine.BorderColor = Color.Yellow;
                        OPALUTextBox.Text = "ASL";
                        if (ShowHexBtn.Text == "Show Hex")
                            ALURbusTB.Text = Convert.ToString(secventiator.GetRBUS(), 2).PadLeft(16, '0');
                        else
                            ALURbusTB.Text = "0x" + Convert.ToString(secventiator.GetRBUS(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.ALU.ASR:
                    {
                        RBUSLine.BorderColor = Color.Yellow;
                        RBUSLine2.BorderColor = Color.Yellow;
                        RBUSLine3.BorderColor = Color.Yellow;
                        ALURbusLine.BorderColor = Color.Yellow;
                        OPALUTextBox.Text = "ASR";
                        if (ShowHexBtn.Text == "Show Hex")
                            ALURbusTB.Text = Convert.ToString(secventiator.GetRBUS(), 2).PadLeft(16, '0');
                        else
                            ALURbusTB.Text = "0x" + Convert.ToString(secventiator.GetRBUS(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.ALU.LSR:
                    {
                        RBUSLine.BorderColor = Color.Yellow;
                        RBUSLine2.BorderColor = Color.Yellow;
                        RBUSLine3.BorderColor = Color.Yellow;
                        ALURbusLine.BorderColor = Color.Yellow;
                        OPALUTextBox.Text = "LSR";
                        if (ShowHexBtn.Text == "Show Hex")
                            ALURbusTB.Text = Convert.ToString(secventiator.GetRBUS(), 2).PadLeft(16, '0');
                        else
                            ALURbusTB.Text = "0x" + Convert.ToString(secventiator.GetRBUS(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.ALU.ROL:
                    {
                        RBUSLine.BorderColor = Color.Yellow;
                        RBUSLine2.BorderColor = Color.Yellow;
                        RBUSLine3.BorderColor = Color.Yellow;
                        ALURbusLine.BorderColor = Color.Yellow;
                        OPALUTextBox.Text = "ROL";
                        if (ShowHexBtn.Text == "Show Hex")
                            ALURbusTB.Text = Convert.ToString(secventiator.GetRBUS(), 2).PadLeft(16, '0');
                        else
                            ALURbusTB.Text = "0x" + Convert.ToString(secventiator.GetRBUS(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.ALU.ROR:
                    {
                        RBUSLine.BorderColor = Color.Yellow;
                        RBUSLine2.BorderColor = Color.Yellow;
                        RBUSLine3.BorderColor = Color.Yellow;
                        ALURbusLine.BorderColor = Color.Yellow;
                        OPALUTextBox.Text = "ROR";
                        if (ShowHexBtn.Text == "Show Hex")
                            ALURbusTB.Text = Convert.ToString(secventiator.GetRBUS(), 2).PadLeft(16, '0');
                        else
                            ALURbusTB.Text = "0x" + Convert.ToString(secventiator.GetRBUS(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.ALU.RLC:
                    {
                        RBUSLine.BorderColor = Color.Yellow;
                        RBUSLine2.BorderColor = Color.Yellow;
                        RBUSLine3.BorderColor = Color.Yellow;
                        ALURbusLine.BorderColor = Color.Yellow;
                        OPALUTextBox.Text = "RLC";
                        if (ShowHexBtn.Text == "Show Hex")
                            ALURbusTB.Text = Convert.ToString(secventiator.GetRBUS(), 2).PadLeft(16, '0');
                        else
                            ALURbusTB.Text = "0x" + Convert.ToString(secventiator.GetRBUS(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.ALU.RRC:
                    {
                        RBUSLine.BorderColor = Color.Yellow;
                        RBUSLine2.BorderColor = Color.Yellow;
                        RBUSLine3.BorderColor = Color.Yellow;
                        ALURbusLine.BorderColor = Color.Yellow;
                        OPALUTextBox.Text = "RRC";
                        if (ShowHexBtn.Text == "Show Hex")
                            ALURbusTB.Text = Convert.ToString(secventiator.GetRBUS(), 2).PadLeft(16, '0');
                        else
                            ALURbusTB.Text = "0x" + Convert.ToString(secventiator.GetRBUS(), 16).PadLeft(4, '0');
                        break;
                    }
            }
            switch (secventiator.rBUS)
            {
                case Secventiator.DestinatieRBUS.NONE:
                    {
                        break;
                    }

                case Secventiator.DestinatieRBUS.PM_RG:
                    {
                        int i = 0;
                        PmRGLine.BorderColor = Color.Yellow;
                        if (ShowHexBtn.Text == "Show Hex")
                        {
                            RGTextBox.Text = "";
                            foreach (Int16 reg in secventiator.GetREG())
                            {
                                RGTextBox.AppendText("R" + i.ToString() + " " + Convert.ToString(reg, 2).PadLeft(16, '0') + "\n");
                                i++;
                            }
                        }
                        else
                        {
                            RGTextBox.Text = "";
                            foreach (Int16 reg in secventiator.GetREG())
                            {
                                RGTextBox.AppendText("R" + i.ToString() + " 0x" + Convert.ToString(reg, 16).PadLeft(4, '0') + "\n");
                                i++;
                            }
                        }
                        break;
                    }

                case Secventiator.DestinatieRBUS.PM_SP:
                    {
                        PmSPLine.BorderColor = Color.Yellow;
                        if (ShowHexBtn.Text == "Show Hex")
                            SPTextBox.Text = Convert.ToString(secventiator.GetSP(), 2).PadLeft(16, '0');
                        else
                            SPTextBox.Text = "0x" + Convert.ToString(secventiator.GetSP(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.DestinatieRBUS.PM_T:
                    {
                        PmTLine.BorderColor = Color.Yellow;
                        if (ShowHexBtn.Text == "Show Hex")
                            TTextBox.Text = Convert.ToString(secventiator.GetT(), 2).PadLeft(16, '0');
                        else
                            TTextBox.Text = "0x" + Convert.ToString(secventiator.GetT(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.DestinatieRBUS.PM_PC:
                    {
                        PmPCLine.BorderColor = Color.Yellow;
                        if (ShowHexBtn.Text == "Show Hex")
                            PCTextBox.Text = Convert.ToString(secventiator.GetPC(), 2).PadLeft(16, '0');
                        else
                            PCTextBox.Text = "0x" + Convert.ToString(secventiator.GetPC(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.DestinatieRBUS.PM_IVR:
                    {
                        PmIVRLine.BorderColor = Color.Yellow;
                        if (ShowHexBtn.Text == "Show Hex")
                            IVRTextBox.Text = Convert.ToString(secventiator.GetIVR(), 2).PadLeft(16, '0');
                        else
                            IVRTextBox.Text = "0x" + Convert.ToString(secventiator.GetIVR(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.DestinatieRBUS.PM_ADR:
                    {
                        PmADRLine.BorderColor = Color.Yellow;
                        if (ShowHexBtn.Text == "Show Hex")
                            ADRTextBox.Text = Convert.ToString(secventiator.GetADR(), 2).PadLeft(16, '0');
                        else
                            ADRTextBox.Text = "0x" + Convert.ToString(secventiator.GetADR(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.DestinatieRBUS.PM_MDR:
                    {
                        PmMDRLine.BorderColor = Color.Yellow;
                        if (ShowHexBtn.Text == "Show Hex")
                            MDRTextBox.Text = Convert.ToString(secventiator.GetMDR(), 2).PadLeft(16, '0');
                        else
                            MDRTextBox.Text = "0x" + Convert.ToString(secventiator.GetMDR(), 16).PadLeft(4, '0');
                        break;
                    }
            }
            switch (secventiator.otherOperations)
            {
                case Secventiator.OtherOperations.NOP:
                    {
                        OOTextBox.Text = "NOP";
                        break;
                    }

                case Secventiator.OtherOperations.CIN_AND_PD_COND:
                    {
                        OOTextBox.Text = "CIN & PD COND";
                        PdCONDLine1.BorderColor = Color.Yellow;
                        PdCONDLine2.BorderColor = Color.Yellow;
                        PdCONDOval.BorderColor = Color.Yellow;
                        PmFLAGLine.BorderColor = Color.Yellow;
                        if (ShowHexBtn.Text == "Show Hex")
                            FLAGTextBox.Text = Convert.ToString(secventiator.GetFLAG(), 2).PadLeft(16, '0');
                        else
                            FLAGTextBox.Text = "0x" + Convert.ToString(secventiator.GetFLAG(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.OtherOperations.INTA_AND_MINUS_TWO_SP:
                    {
                        OOTextBox.Text = "INTA -2 SP";
                        if (ShowHexBtn.Text == "Show Hex")
                            SPTextBox.Text = Convert.ToString(secventiator.GetSP(), 2).PadLeft(16, '0');
                        else
                            SPTextBox.Text = "0x" + Convert.ToString(secventiator.GetSP(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.OtherOperations.PD_COND:
                    {
                        OOTextBox.Text = "PdCOND";
                        PmFLAGLine2.BorderColor = Color.Yellow;
                        PdCONDLine1.BorderColor = Color.Yellow;
                        PdCONDLine2.BorderColor = Color.Yellow;
                        PdCONDOval.FillColor = Color.Yellow;
                        if (ShowHexBtn.Text == "Show Hex")
                            FLAGTextBox.Text = Convert.ToString(secventiator.GetFLAG(), 2).PadLeft(16, '0');
                        else
                            FLAGTextBox.Text = "0x" + Convert.ToString(secventiator.GetFLAG(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.OtherOperations.CIN:
                    {
                        OOTextBox.Text = "CIN";
                        break;
                    }

                case Secventiator.OtherOperations.PM_FLAG:
                    {
                        OOTextBox.Text = "PmFLAG";
                        PmFLAGLine.BorderColor = Color.Yellow;
                        PmFLAGLine2.BorderColor = Color.Yellow;
                        if (ShowHexBtn.Text == "Show Hex")
                            FLAGTextBox.Text = Convert.ToString(secventiator.GetFLAG(), 2).PadLeft(16, '0');
                        else
                            FLAGTextBox.Text = "0x" + Convert.ToString(secventiator.GetFLAG(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.OtherOperations.PLUS_TWO_SP:
                    {
                        OOTextBox.Text = "+2SP";
                        if (ShowHexBtn.Text == "Show Hex")
                            SPTextBox.Text = Convert.ToString(secventiator.GetSP(), 2).PadLeft(16, '0');
                        else
                            SPTextBox.Text = Convert.ToString(secventiator.GetSP(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.OtherOperations.MINUS_TWO_SP:
                    {
                        OOTextBox.Text = "-2SP";
                        if (ShowHexBtn.Text == "Show Hex")
                            SPTextBox.Text = Convert.ToString(secventiator.GetSP(), 2).PadLeft(16, '0');
                        else
                            SPTextBox.Text = "0x" + Convert.ToString(secventiator.GetSP(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.OtherOperations.PLUS_TWO_PC:
                    {
                        OOTextBox.Text = "+2PC";
                        if (ShowHexBtn.Text == "Show Hex")
                            PCTextBox.Text = Convert.ToString(secventiator.GetPC(), 2).PadLeft(16, '0');
                        else
                            PCTextBox.Text = "0x" + Convert.ToString(secventiator.GetPC(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.OtherOperations.SET_BE0:
                    {
                        break;
                    }
                case Secventiator.OtherOperations.SET_BE1:
                    {
                        break;
                    }

                case Secventiator.OtherOperations.RESET_C:
                    {
                        OOTextBox.Text = "RESET CARRY";
                        if (ShowHexBtn.Text == "Show Hex")
                            FLAGTextBox.Text = Convert.ToString(secventiator.GetFLAG(), 2).PadLeft(16, '0');
                        else
                            FLAGTextBox.Text = "0x" + Convert.ToString(secventiator.GetFLAG(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.OtherOperations.SET_C:
                    {
                        OOTextBox.Text = "SET CARRY";
                        if (ShowHexBtn.Text == "Show Hex")
                            FLAGTextBox.Text = Convert.ToString(secventiator.GetFLAG(), 2).PadLeft(16, '0');
                        else
                            FLAGTextBox.Text = "0x" + Convert.ToString(secventiator.GetFLAG(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.OtherOperations.RESET_Z:
                    {
                        OOTextBox.Text = "RESET ZERO";
                        if (ShowHexBtn.Text == "Show Hex")
                            FLAGTextBox.Text = Convert.ToString(secventiator.GetFLAG(), 2).PadLeft(16, '0');
                        else
                            FLAGTextBox.Text = "0x" + Convert.ToString(secventiator.GetFLAG(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.OtherOperations.SET_Z:
                    {
                        OOTextBox.Text = "SET ZERO";
                        if (ShowHexBtn.Text == "Show Hex")
                            FLAGTextBox.Text = Convert.ToString(secventiator.GetFLAG(), 2).PadLeft(16, '0');
                        else
                            FLAGTextBox.Text = "0x" + Convert.ToString(secventiator.GetFLAG(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.OtherOperations.RESET_S:
                    {
                        OOTextBox.Text = "RESET SIGN";
                        if (ShowHexBtn.Text == "Show Hex")
                            FLAGTextBox.Text = Convert.ToString(secventiator.GetFLAG(), 2).PadLeft(16, '0');
                        else
                            FLAGTextBox.Text = "0x" + Convert.ToString(secventiator.GetFLAG(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.OtherOperations.SET_S:
                    {
                        OOTextBox.Text = "SET SIGN";
                        if (ShowHexBtn.Text == "Show Hex")
                            FLAGTextBox.Text = Convert.ToString(secventiator.GetFLAG(), 2).PadLeft(16, '0');
                        else
                            FLAGTextBox.Text = "0x" + Convert.ToString(secventiator.GetFLAG(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.OtherOperations.RESET_V:
                    {
                        OOTextBox.Text = "RESET OVERFLOW";
                        if (ShowHexBtn.Text == "Show Hex")
                            FLAGTextBox.Text = Convert.ToString(secventiator.GetFLAG(), 2).PadLeft(16, '0');
                        else
                            FLAGTextBox.Text = "0x" + Convert.ToString(secventiator.GetFLAG(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.OtherOperations.SET_V:
                    {
                        OOTextBox.Text = "SET OVERFLOW";
                        if (ShowHexBtn.Text == "Show Hex")
                            FLAGTextBox.Text = Convert.ToString(secventiator.GetFLAG(), 2).PadLeft(16, '0');
                        else
                            FLAGTextBox.Text = "0x" + Convert.ToString(secventiator.GetFLAG(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.OtherOperations.RESET_CZSV:
                    {
                        if (ShowHexBtn.Text == "Show Hex")
                            FLAGTextBox.Text = Convert.ToString(secventiator.GetFLAG(), 2).PadLeft(16, '0');
                        else
                            FLAGTextBox.Text = "0x" + Convert.ToString(secventiator.GetFLAG(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.OtherOperations.SET_CZSV:
                    { 
                        if (ShowHexBtn.Text == "Show Hex")
                            FLAGTextBox.Text = Convert.ToString(secventiator.GetFLAG(), 2).PadLeft(16, '0');
                        else
                            FLAGTextBox.Text = "0x" + Convert.ToString(secventiator.GetFLAG(), 16).PadLeft(4, '0');
                        break;
                    }
            }
            secventiator.Start();
            switch (secventiator.memOP)
            {
                case Secventiator.MemOP.NOP:
                    {
                        break;
                    }

                case Secventiator.MemOP.IFCH:
                    {
                        MemoryOPTextBox.Text = "IFCH";
                        if (ShowHexBtn.Text == "Show Hex")
                            IRTextBox.Text = Convert.ToString(secventiator.GetIR(), 2).PadLeft(16, '0');
                        else
                            IRTextBox.Text = "0x" + Convert.ToString(secventiator.GetIR(), 16).PadLeft(4, '0');
                        DataOUTLine1.BorderColor = Color.Yellow;
                        DataOUTLine2.BorderColor = Color.Yellow;
                        DataOUTLine3.BorderColor = Color.Yellow;
                        DataOUTOval.FillColor = Color.Yellow;
                        PdADRsLine.BorderColor = Color.Yellow;
                        AddressOVAL.FillColor = Color.Yellow;
                        AddressLine1.BorderColor = Color.Yellow;
                        AddressLine2.BorderColor = Color.Yellow;
                        break;
                    }

                case Secventiator.MemOP.READ:
                    {
                        MemoryOPTextBox.Text = "READ";
                        PdADRsLine.BorderColor = Color.Yellow;
                        AddressLine1.BorderColor = Color.Yellow;
                        AddressLine2.BorderColor = Color.Yellow;
                        AddressOVAL.FillColor = Color.Yellow;
                        DataOUTLine1.BorderColor = Color.Yellow;
                        DataOUTLine4.BorderColor = Color.Yellow;
                        DataOUTLine5.BorderColor = Color.Yellow;
                        PmMDRLine.BorderColor = Color.Yellow;
                        if (ShowHexBtn.Text == "Show Hex")
                            MDRTextBox.Text = Convert.ToString(secventiator.GetMDR(), 2).PadLeft(16, '0');
                        else
                            MDRTextBox.Text = "0x" + Convert.ToString(secventiator.GetMDR(), 16).PadLeft(4, '0');
                        break;
                    }

                case Secventiator.MemOP.WRITE:
                    {
                        MemoryOPTextBox.Text = "WRITE";
                        PdMDRsLine.BorderColor = Color.Yellow;
                        DataINLine1.BorderColor = Color.Yellow;
                        DataINLine2.BorderColor = Color.Yellow;
                        DataINOval.FillColor = Color.Yellow;
                        break;
                    }
            }
            this.ControlTextBox.AppendText(secventiator.sBUS.ToString() + "\t" + secventiator.dBUS.ToString() + "\t" + secventiator.alu.ToString() + "\t" + secventiator.rBUS.ToString() +
    "\t" + secventiator.memOP.ToString() + "\t" + secventiator.otherOperations.ToString() + "\t" + secventiator.branchCondition.ToString() + "\t" + "0x" + Convert.ToString(secventiator.uadrPLUSidx, 16).PadLeft(2, '0'));

        }

        private void parseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MicrocodeParserWindow microcodeParserWindow = new MicrocodeParserWindow(new MicrocodeParser(this));
        }

        public void resetAllColors() {
            this.SBUSLine.BorderColor = Color.WhiteSmoke;
            this.DBUSLine.BorderColor = Color.WhiteSmoke;
            this.PmPCLine.BorderColor = Color.WhiteSmoke;
            this.PmIVRLine.BorderColor = Color.WhiteSmoke;
            this.PmTLine.BorderColor = Color.WhiteSmoke;
            this.PmFLAGLine.BorderColor = Color.WhiteSmoke;
            this.PmFLAGLine2.BorderColor = Color.WhiteSmoke;
            this.PdCONDOval.FillColor = Color.WhiteSmoke;
            this.PdCONDLine1.BorderColor = Color.WhiteSmoke;
            this.PdCONDLine2.BorderColor = Color.WhiteSmoke;
            this.ALURbusLine.BorderColor = Color.WhiteSmoke;
            this.ALUSbusLine1.BorderColor = Color.WhiteSmoke;
            this.ALUSbusLine2.BorderColor = Color.WhiteSmoke;
            this.PdPCsLine.BorderColor = Color.WhiteSmoke;
            this.PdIVRsLine.BorderColor = Color.WhiteSmoke;
            this.PdTsLine.BorderColor = Color.WhiteSmoke;
            this.PdNTsLine.BorderColor = Color.WhiteSmoke;
            this.PdSPsLine.BorderColor = Color.WhiteSmoke;
            this.PdFLAGsLine.BorderColor = Color.WhiteSmoke;
            this.ALUDbusLine.BorderColor = Color.WhiteSmoke;
            this.Pd0SLine.BorderColor = Color.WhiteSmoke;
            this.Pd0dLine.BorderColor = Color.WhiteSmoke;
            this.PdN1dLine.BorderColor = Color.WhiteSmoke;
            this.PdRGsLine.BorderColor = Color.WhiteSmoke;
            this.PdRGdLine.BorderColor = Color.WhiteSmoke;
            this.PmRGLine.BorderColor = Color.WhiteSmoke;
            this.DataOUTLine1.BorderColor = Color.WhiteSmoke;
            this.DataOUTLine2.BorderColor = Color.WhiteSmoke;
            this.DataOUTLine3.BorderColor = Color.WhiteSmoke;
            this.DataOUTLine4.BorderColor = Color.WhiteSmoke;
            this.DataOUTLine5.BorderColor = Color.WhiteSmoke;
            this.DataOUTOval.FillColor = Color.WhiteSmoke;
            this.DataINOval.FillColor = Color.WhiteSmoke;
            this.PmMDRLine.BorderColor = Color.WhiteSmoke;
            this.PmADRLine.BorderColor = Color.WhiteSmoke;
            this.DataINLine1.BorderColor = Color.WhiteSmoke;
            this.DataINLine2.BorderColor = Color.WhiteSmoke;
            this.PdMDRsLine.BorderColor = Color.WhiteSmoke;
            this.PdMDRdLine.BorderColor = Color.WhiteSmoke;
            this.PdIROFdLine.BorderColor = Color.WhiteSmoke;
            this.PmSPLine.BorderColor = Color.WhiteSmoke;
            this.RBUSLine.BorderColor = Color.WhiteSmoke;
            this.RBUSLine2.BorderColor = Color.WhiteSmoke;
            this.RBUSLine3.BorderColor = Color.WhiteSmoke;
            this.PdADRdLine.BorderColor = Color.WhiteSmoke;
            this.PdADRsLine.BorderColor = Color.WhiteSmoke;
            this.AddressLine1.BorderColor = Color.WhiteSmoke;
            this.AddressLine2.BorderColor = Color.WhiteSmoke;
            this.AddressOVAL.FillColor = Color.WhiteSmoke;
            this.OOTextBox.Text = "";
            this.MemoryOPTextBox.Text = "";
            this.ALUSbusTB.Text = "";
            this.ALUDbusTB.Text = "";
            this.ALURbusTB.Text = "";
            this.OPALUTextBox.Text = "";
        }

        private void ShowHexBtn_Click(object sender, EventArgs e)
        {
            Int16 T; 
            Int16 MDR;

            UInt16 FLAG;
            UInt16 IR;
            UInt16 SP;
            UInt16 PC;
            UInt16 IVR;
            UInt16 ADR;
            Int16[] regs = new Int16[16];
            byte MAR; //adreseaza continutul mpm
            UInt64 MIR; //retine fiecare microinstructiune
            Int16 DBUS;
            Int16 SBUS;
            Int16 RBUS;
            if (ShowHexBtn.Text.Equals("Show Hex"))
            {
                
                int i = 0;

                this.RGTextBox.Text = this.RGTextBox.Text.TrimEnd('\n');
                foreach (string line in RGTextBox.Lines)
                {
                    if (line.IndexOf(" ").Equals(2))
                        regs[i] = Convert.ToInt16(line.Substring(3, 16), 2);    //pentru r0<->r9
                    else
                    {
                        regs[i] = Convert.ToInt16(line.Substring(4, 16), 2);    //pentru r10<->r15
                    }
                    i++;
                }
                this.RGTextBox.Text = "";
                i = 0;
                foreach (Int16 reg in regs)
                {
                    this.RGTextBox.AppendText("R" + i.ToString() + " 0x" + Convert.ToString(reg, 16).PadLeft(4, '0') + "\n");
                    i++;
                }

                PC = Convert.ToUInt16(this.PCTextBox.Text,2);
                this.PCTextBox.Text = "0x" + Convert.ToString(PC, 16).PadLeft(4, '0');

                T = Convert.ToInt16(this.TTextBox.Text,2);
                this.TTextBox.Text = "0x" + Convert.ToString(T, 16).PadLeft(4, '0');

                MDR = Convert.ToInt16(this.MDRTextBox.Text,2);
                this.MDRTextBox.Text = "0x" + Convert.ToString(MDR, 16).PadLeft(4, '0');

                FLAG = Convert.ToUInt16(this.FLAGTextBox.Text,2);
                this.FLAGTextBox.Text = "0x" + Convert.ToString(FLAG, 16).PadLeft(4, '0');

                IR = Convert.ToUInt16(this.IRTextBox.Text, 2);
                this.IRTextBox.Text = "0x" + Convert.ToString(IR, 16).PadLeft(4, '0');

                SP = (UInt16)(Convert.ToInt64(this.SPTextBox.Text,2));
                this.SPTextBox.Text = "0x" + Convert.ToString(SP, 16).PadLeft(4, '0');

                IVR = Convert.ToUInt16(this.IVRTextBox.Text, 2);
                this.IVRTextBox.Text = "0x" + Convert.ToString(IVR, 16).PadLeft(4, '0');

                ADR = Convert.ToUInt16(this.ADRTextBox.Text, 2);
                this.ADRTextBox.Text = "0x" + Convert.ToString(ADR, 16).PadLeft(4, '0');

                MAR = Convert.ToByte(this.MARTextBox.Text, 2);
                this.MARTextBox.Text = "0x" + Convert.ToString(MAR, 16).PadLeft(2,'0');

                MIR = Convert.ToUInt64(this.MIRTextBox.Text, 2);
                this.MIRTextBox.Text = "0x" + Convert.ToString(Convert.ToInt64(MIR), 16);

                if (ALUDbusTB.Text == "NONE"||ALUDbusTB.Text == "") { }
                else
                {
                    DBUS = Convert.ToInt16(this.ALUDbusTB.Text, 2);
                    this.ALUDbusTB.Text = "0x" + Convert.ToString(DBUS, 16).PadLeft(4, '0');
                }

                if (ALUSbusTB.Text == "NONE" || ALUSbusTB.Text == "") { }
                else
                {
                    SBUS = Convert.ToInt16(this.ALUSbusTB.Text, 2);
                    this.ALUSbusTB.Text = "0x" + Convert.ToString(SBUS, 16).PadLeft(4, '0');
                }

                if (ALURbusTB.Text == "NONE" || ALURbusTB.Text == "") { }
                else
                {
                    RBUS = Convert.ToInt16(this.ALURbusTB.Text, 2);
                    this.ALURbusTB.Text = "0x" + Convert.ToString(RBUS, 16).PadLeft(4, '0');
                }

                ShowHexBtn.Text = "Show Bin";
            }
            else {
                int i = 0;

                this.RGTextBox.Text = this.RGTextBox.Text.TrimEnd('\n');
                foreach (string line in RGTextBox.Lines)
                {
                    if (line.IndexOf(" ").Equals(2))
                        regs[i] = Convert.ToInt16(line.Substring(5, 4), 16);    //r0<->r9
                    else
                    {
                        regs[i] = Convert.ToInt16(line.Substring(6, 4), 16);    //r10<->r15
                    }
                    i++;
                }
                this.RGTextBox.Text = "";
                i = 0;
                foreach (Int16 reg in regs)
                {
                    this.RGTextBox.AppendText("R" + i.ToString() + " " + Convert.ToString(reg, 2).PadLeft(16, '0') + "\n");
                    i++;
                }

                PC = UInt16.Parse(this.PCTextBox.Text.Substring(2, 4), System.Globalization.NumberStyles.HexNumber);
                this.PCTextBox.Text = Convert.ToString(PC, 2).PadLeft(16, '0');

                T = Int16.Parse(this.TTextBox.Text.Substring(2, 4), System.Globalization.NumberStyles.HexNumber);
                this.TTextBox.Text = Convert.ToString(T, 2).PadLeft(16, '0');

                MDR = Int16.Parse(this.MDRTextBox.Text.Substring(2, 4), System.Globalization.NumberStyles.HexNumber);
                this.MDRTextBox.Text = Convert.ToString(MDR, 2).PadLeft(16, '0');

                FLAG = UInt16.Parse(this.FLAGTextBox.Text.Substring(2, 4), System.Globalization.NumberStyles.HexNumber);
                this.FLAGTextBox.Text = Convert.ToString(FLAG, 2).PadLeft(16, '0');

                IR = UInt16.Parse(this.IRTextBox.Text.Substring(2, 4), System.Globalization.NumberStyles.HexNumber);
                this.IRTextBox.Text = Convert.ToString(IR, 2).PadLeft(16, '0');

                SP = (UInt16)(Int64.Parse(this.SPTextBox.Text.Substring(2, 4), System.Globalization.NumberStyles.HexNumber));
                this.SPTextBox.Text = Convert.ToString(SP, 2).PadLeft(16, '0');

                IVR = UInt16.Parse(this.IVRTextBox.Text.Substring(2, 4), System.Globalization.NumberStyles.HexNumber);
                this.IVRTextBox.Text = Convert.ToString(IVR, 2).PadLeft(16, '0');

                ADR = UInt16.Parse(this.ADRTextBox.Text.Substring(2, 4), System.Globalization.NumberStyles.HexNumber);
                this.ADRTextBox.Text = Convert.ToString(ADR, 2).PadLeft(16, '0');

                MAR = Byte.Parse(this.MARTextBox.Text.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                this.MARTextBox.Text = Convert.ToString(MAR, 2).PadLeft(8,'0');

                MIR = UInt64.Parse(this.MIRTextBox.Text.Substring(2), System.Globalization.NumberStyles.HexNumber);
                this.MIRTextBox.Text = Convert.ToString(Convert.ToInt64(MIR), 2);

                if (ALUDbusTB.Text == "NONE" || ALUDbusTB.Text == "") { }
                else
                {
                    DBUS = Int16.Parse(this.ALUDbusTB.Text.Substring(2, 4), System.Globalization.NumberStyles.HexNumber);
                    this.ALUDbusTB.Text = Convert.ToString(DBUS, 2).PadLeft(16, '0');
                }

                if (ALUSbusTB.Text == "NONE" || ALUSbusTB.Text == "") { }
                else
                {
                    SBUS = Int16.Parse(this.ALUSbusTB.Text.Substring(2, 4), System.Globalization.NumberStyles.HexNumber);
                    this.ALUSbusTB.Text = Convert.ToString(SBUS, 2).PadLeft(16, '0');
                }

                if (ALURbusTB.Text == "NONE" || ALURbusTB.Text == "") { }
                else
                {
                    RBUS = Int16.Parse(this.ALURbusTB.Text.Substring(2, 4), System.Globalization.NumberStyles.HexNumber);
                    this.ALURbusTB.Text = Convert.ToString(RBUS, 2).PadLeft(16, '0');
                }
          
                ShowHexBtn.Text = "Show Hex";
            }
        }

        private void showMemoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemoryForm memory = new MemoryForm(secventiator.GetMemory());
            memory.Show();
        }

        private void showStackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StackForm stack = new StackForm(secventiator.GetMemory());
            stack.Show();
        }
    }
}
