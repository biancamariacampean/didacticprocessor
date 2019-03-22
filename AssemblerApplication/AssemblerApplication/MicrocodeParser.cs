using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerApplication
{
    public partial class MicrocodeParser
    {

        private abstract class SbusSource
        {
            public static readonly Dictionary<string, byte> dictionary = new Dictionary<string, byte>
                {
                    { "NONE", 0x0 },
                    { "PdFLAG", 0x1 },
                    { "PdRG", 0x2 },
                    { "PdSP", 0x3 },
                    { "PdT", 0x4 },
                    { "PdNT", 0x5 },
                    { "PdPC", 0x6 },
                    { "PdIVR", 0x7 },
                    { "PdMDR", 0x8 },
                    { "Pd0", 0x9 },
                };

            public static byte GetCode(string ucmd)
            {
                if (dictionary.TryGetValue(ucmd, out byte returnValue))
                {
                    return returnValue;
                }

                throw new Exception("Illegal microcommand: " + ucmd);
            }
        }

        private abstract class DbusSource
        {
            public static readonly Dictionary<string, byte> dictionary = new Dictionary<string, byte>
                {
                    {"NONE", 0x0 },
                    {"PdIR[OFF]", 0x1 },
                    {"PdRG", 0x2 },
                    {"PdMDR", 0x3 },
                    {"Pd-1", 0x4 },
                    {"Pd0", 0x5 },
                };

            public static byte GetCode(string ucmd)
            {
                if (dictionary.TryGetValue(ucmd, out byte returnValue))
                {
                    return returnValue;
                }

                throw new Exception("Illegal microcommand: " + ucmd);
            }
        }

        private abstract class AluOperation
        {
            public static readonly Dictionary<string, byte> dictionary = new Dictionary<string, byte>
                {
                    { "NONE", 0x0 },
                    { "NSBUS", 0x1 },
                    { "SUM", 0x2 },
                    { "AND", 0x3 },
                    { "OR", 0x4 },
                    { "XOR", 0x5 },
                    { "ASL", 0x6 },
                    { "ASR", 0x7 },
                    { "LSR", 0x8 },
                    { "ROL", 0x9 },
                    { "ROR", 0xa },
                    { "RLC", 0xb },
                    { "RRC", 0xc },
                };

            public static byte GetCode(string ucmd)
            {
                if (dictionary.TryGetValue(ucmd, out byte returnValue))
                {
                    return returnValue;
                }

                throw new Exception("Illegal microcommand: " + ucmd);
            }
        }

        private abstract class RbusDestination
        {
            public static readonly Dictionary<string, byte> dictionary = new Dictionary<string, byte>
                {
                    {"NONE", 0x0 },
                    {"PmRG", 0x1 },
                    {"PmSP", 0x2 },
                    {"PmT", 0x3 },
                    {"PmPC", 0x4 },
                    {"PmIVR", 0x5 },
                    {"PmADR", 0x6 },
                    {"PmMDR", 0x7 },
                };

            public static byte GetCode(string ucmd)
            {
                if (dictionary.TryGetValue(ucmd, out byte returnValue))
                {
                    return returnValue;
                }

                throw new Exception("Illegal microcommand: " + ucmd);
            }
        }

        private abstract class MemoryOperation
        {
            public static readonly Dictionary<string, byte> dictionary = new Dictionary<string, byte>
                {
                    {"NOP", 0x0 },
                    {"IFCH", 0x1 },
                    {"READ", 0x2 },
                    {"WRITE", 0x3 },
                };

            public static byte GetCode(string ucmd)
            {
                if (dictionary.TryGetValue(ucmd, out byte returnValue))
                {
                    return returnValue;
                }

                throw new Exception("Illegal microcommand: " + ucmd);
            }
        }

        private abstract class OtherOperations
        {
            public static readonly Dictionary<string, byte> dictionary = new Dictionary<string, byte>
                {
                    { "NOP", 0x0 },
                    { "(Cin, PdCOND)", 0x1 },
                    { "(INTA, -2SP)", 0x2 },
                    { "PdCOND", 0x3 },
                    { "Cin", 0x4 },
                    { "PmFLAG", 0x5 },
                    { "+2SP", 0x6 },
                    { "-2SP", 0x7 },
                    { "+2PC", 0x8 },
                    { "A(1)BE0", 0x9 },
                    { "A(1)BE1", 0xa },
                    { "A(0)C", 0xb },
                    { "A(1)C", 0xc },
                    { "A(0)Z", 0xd },
                    { "A(1)Z", 0xe },
                    { "A(0)S", 0xf },
                    { "A(1)S", 0x10},
                    { "A(0)V", 0x11 },
                    { "A(1)V", 0x12 },
                    { "A(0)CZSV", 0x13 },
                    { "A(1)CZSV", 0x14 },
                };

            public static byte GetCode(string ucmd)
            {
                if (dictionary.TryGetValue(ucmd, out byte returnValue))
                {
                    return returnValue;
                }

                throw new Exception("Illegal microcommand: " + ucmd);
            }
        }

        private abstract class BranchCondtion
        {
            public const UInt64 TRUE = 0;
            public const UInt64 FALSE = 1;

            public static readonly Dictionary<string, byte> dictionary = new Dictionary<string, byte>
                {
                    { "NONE", 0x0 },
                    { "B1", 0x1 },
                    { "AM", 0x2 },
                    { "AD", 0x3 },
                    { "INTR", 0x4 },
                    { "ACLOW", 0x5 },
                    { "CIL", 0x6 },
                    { "C", 0x7 },
                    { "Z", 0x8 },
                    { "S", 0x9 },
                    { "V", 0xa }
                };

            public static byte GetCode(string ucmd)
            {
                if (dictionary.TryGetValue(ucmd, out byte returnValue))
                {
                    return returnValue;
                }

                throw new Exception("Undefined branch condition: " + ucmd);
            }
        }

        private abstract class BranchDestination
        {
            public static readonly Dictionary<string, byte> dictionary = new Dictionary<string, byte>
                {
                    { "IF", 0x0 },
                    { "B1", 0x3 },
                    { "FOSAM", 0x7 },
                    { "FODAM", 0x10 },
                    { "CLASS_TEST", 0x15 },
                    { "MOV", 0x17 },
                    { "CLR", 0x25 },
                    { "WRITE_MEM", 0x45 },
                    { "BR", 0x47 },
                    { "CLC", 0x59 },
                    { "WAIT", 0x71 },
                    { "INTR_TEST", 0x81 },
                    { "POWER_FAIL", 0x83 },
                    { "ILLEGAL", 0x84 },
                    { "INT", 0x85 },
                };

            public static byte GetCode(string ucmd)
            {
                if (dictionary.TryGetValue(ucmd, out byte returnValue))
                {
                    return returnValue;
                }

                throw new Exception("Undefined branch destination: " + ucmd);
            }
        }



        private Simulator mainWindow;
        private MicrocodeParserWindow microcodeParserWindow;
        private string txtPath;

        public MicrocodeParser(Simulator mainWindow)
        {
            this.mainWindow = mainWindow;
            microcodeParserWindow = new MicrocodeParserWindow(this);
            microcodeParserWindow.Closing += MicrocodeParserWindow_Closing;

            mainWindow.Hide();
            microcodeParserWindow.Show();
        }

        private void MicrocodeParserWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mainWindow.Show();
        }

        public string ParseFile(string path)
        {
            txtPath = path.Substring(0, path.LastIndexOf('.')) + ".txt";
            string[] csvFile = File.ReadAllLines(path);
            IEnumerator enumerator = csvFile.GetEnumerator();
            enumerator.MoveNext();

            List<UInt64> uprogram = new List<UInt64>();
            UInt16 uadr;

            for (uadr = 0; enumerator.MoveNext(); uadr++)
            {
                string line = enumerator.Current.ToString().Trim();

                if (line == "")
                {
                    break;
                }

                string[] tokens = line.Split(';');
                UInt64 sbus = SbusSource.GetCode(tokens[2]);
                UInt64 dbus = DbusSource.GetCode(tokens[3]);
                UInt64 alu = AluOperation.GetCode(tokens[4]);
                UInt64 rbus = RbusDestination.GetCode(tokens[5]);
                UInt64 mem = MemoryOperation.GetCode(tokens[6]);
                UInt64 other = OtherOperations.GetCode(tokens[7]);
                UInt64 branchCondition;
                UInt64 branchIndex;
                UInt64 branchOn;
                UInt64 branchAddress;
                string[] successor = tokens[8].Trim().Split(' ');

                switch (successor[0])
                {
                    case "JUMP":
                        {
                            branchCondition = BranchCondtion.GetCode("NONE");
                            branchIndex = 0;
                            branchOn = BranchCondtion.TRUE;
                            branchAddress = BranchDestination.GetCode(successor[1]);
                            break;
                        }

                    case "JUMPI":
                        {
                            string[] dest = successor[1].Split(new[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);

                            branchCondition = BranchCondtion.GetCode("NONE");
                            branchIndex = (UInt64)(dest[1][dest[1].Length - 1] - '0');
                            branchOn = BranchCondtion.TRUE;
                            branchAddress = BranchDestination.GetCode(dest[0]);
                            break;
                        }

                    case "IF":
                        {
                            if (successor[1].StartsWith("N"))
                            {
                                branchCondition = BranchCondtion.GetCode(successor[1].Substring(1));
                                branchOn = BranchCondtion.FALSE;
                            }
                            else
                            {
                                branchCondition = BranchCondtion.GetCode(successor[1]);
                                branchOn = BranchCondtion.TRUE;
                            }

                            if (successor[2] == "JUMP")
                            {
                                branchAddress = BranchDestination.GetCode(successor[3]);
                                branchIndex = 0;
                            }
                            else
                            {
                                string[] dest = successor[3].Split(new[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                                branchAddress = BranchDestination.GetCode(dest[0]);
                                branchIndex = (UInt64)(dest[1][dest[1].Length - 1] - '0');
                            }

                            break;
                        }

                    case "STEP":
                        {
                            branchCondition = BranchCondtion.GetCode("NONE");
                            branchIndex = 0x0;
                            branchOn = BranchCondtion.FALSE;
                            branchAddress = 0;
                            break;
                        }

                    case "NONE":
                        {
                            branchCondition = BranchCondtion.GetCode("NONE");
                            branchIndex = 0x0;
                            branchOn = BranchCondtion.FALSE;
                            branchAddress = 0;
                            break;
                        }

                    default:
                        {
                            throw new Exception("Illegal microinstruction. (Address: " + Convert.ToString(uadr, 16) + ")");
                        }
                }

                uprogram.Add((sbus << 33) | (dbus << 30) | (alu << 26) | (rbus << 23) | (mem << 21) | (other << 16) | (branchCondition << 12) | (branchIndex << 9) | (branchOn << 8) | branchAddress);
            }

            CreateFileAndWrite(uprogram);

            return txtPath;
        }

        private void CreateFileAndWrite(List<UInt64> uprogram)
        {
            if (File.Exists(txtPath))
            {
                File.Delete(txtPath);
            }

            using (StreamWriter sw = File.AppendText(txtPath))
            {
                foreach (UInt64 uinstr in uprogram)
                {
                    sw.Write("0x" + uinstr.ToString("X") + ",\n");
                }
            }
        }

    }
}
