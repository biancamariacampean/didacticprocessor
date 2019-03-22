using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace AssemblerApplication
{
    public class Assembler
    {
        protected String filename;
        protected string outputFileName = "";
        protected int[] outputFile = new int[1024];
        protected int outputFileIndex = 0;
        protected Dictionary<string, int> twoOperandsInstructions = new Dictionary<string, int>() {
            //Instructiuni cu 2 operanzi
            {"MOV",         0},
            {"ADD",         1 },
            {"SUB",         2 },
            {"CMP",         3 },
            {"AND",         4 },
            {"OR",          5 },
            {"XOR",         6 }   
        };
        protected Dictionary<string, int> oneOperandInstructions = new Dictionary<string, int>() {
            //instructiuni cu un operand
            {"CLR",         512},
            {"NEG",         513},
            {"INC",         514},
            {"DEC",         515},
            {"ASL",         516},
            {"ASR",         517},
            {"LSR",         518},
            {"ROL",         519},
            {"ROR",         520},
            {"RLC",         521},
            {"RRC",         522},
            {"JMP",         523 },
            {"PUSH",        524 },
            {"POP",         525 },
            {"CALL",        526 }
        };
        protected Dictionary<string, int> branchInstructions = new Dictionary<string, int>() {
            //instructiuni de salt
            {"BR",          192 },
            {"BNE",         193 },
            {"BEQ",         194 },
            {"BPL",         195 },
            {"BMI",         196 },
            {"BCS",         197 },
            {"BCC",         198 },
            {"BVS",         199 },
            {"BVC",         200 }
        };
        protected Dictionary<string, int> otherInstructions = new Dictionary<string, int>() {
            //diverse
            {"CLC",         57344 },
            {"CLV",         57345 },
            {"CLZ",         57346 },
            {"CLS",         57347 },
            {"CCC",         57348 },
            {"SEC",         57349 },
            {"SEV",         57350 },
            {"SEZ",         57351 },
            {"SES",         57352 },
            {"SCC",         57353 },
            {"NOP",         57354 },
            {"HALT",        57355},
            {"WAIT",        57356 },
            {"PUSH PC",     57357 },
            {"POP PC",      57358 },
            {"PUSH FLAG",   57359 },
            {"POP FLAG",    57360 },
            {"RET",         57361 },
            {"RETI",        57362 }
        };
        protected Dictionary<string, int> registers = new Dictionary<string, int>() {
            {"R0", 0 },
            {"R1", 1 },
            {"R2", 2 },
            {"R3", 3 },
            {"R4", 4 },
            {"R5", 5 },
            {"R6", 6 },
            {"R7", 7 },
            {"R8", 8 },
            {"R9", 9 },
            {"R10", 10 },
            {"R11", 11 },
            {"R12", 12 },
            {"R13", 13 },
            {"R14", 14 },
            {"R15", 15 }
        };
        protected Dictionary<string, int> etichete = new Dictionary<string, int>();

        protected AssemblerMainForm amf;

        protected int SS = 0;
        protected int DS = 0;
        protected int CS = 0;

        protected List<string> liniiFisierIntrare = new List<string>();
        protected List<Int32> variabileDB = new List<Int32>();
        protected List<Int32> variabileDW = new List<Int32>();
        protected List<Int32> variabileDD = new List<Int32>();

        
        protected int noOfVariables = 0;
        protected int nrEtichete = 0;
        protected int instructionCode;
        protected int indexedCode;
        protected int indexedCode2;
        protected int etJumpValue;
        protected int PC = 0;

        public Assembler(AssemblerMainForm f) {
            this.amf = f;
        }
        public Assembler() { }

        public String getFileName(String filter)
        {
            try
            {
                /* Local variable used to store the filename */
                String fileNameWithPath = "";
                /* Instantiate an OpenFileDialog */
                OpenFileDialog of = new OpenFileDialog();
                /* Set the filter */
                of.Filter = filter;
                /* Display the Open File dialog */
                if (of.ShowDialog() == DialogResult.OK)
                {
                    /* Get only the filename with full path */
                    fileNameWithPath = of.FileName;
                    /* Get only the filename without path */
                    filename = of.SafeFileName;
                }
                /* Return the filename with complete path */
                return fileNameWithPath;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

        public void OpenTestFile(string filename) {
            amf.ASMShowTextBox.Text = "";
            outputFileName = filename.Substring(0, filename.Length - 3) + "bin";
            try
            {
                StreamReader stream = new StreamReader(filename);
                string line = null;
                while (!stream.EndOfStream)
                {
                    line = stream.ReadLine().Trim();
                    liniiFisierIntrare.Add(line);
                    amf.ASMShowTextBox.AppendText(line);
                    amf.ASMShowTextBox.AppendText("\r\n");
                }
                stream.Close();
            }
            catch (Exception e) {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void generateBinaryFile() {
            foreach (string s in liniiFisierIntrare)
            {
                string[] split;
                int index = s.IndexOf(';'); //comentariu               
                if (index > 0)
                {
                    split = s.Substring(0, index).Split(' ', '\n');
                }
                else if (index == 0)
                {
                    continue;
                }
                else
                {
                    split = s.Split(' ', '\n');
                }
                
                //directivele .stack, .data, .code
                if (split[0][0] == '.')
                {
                    if (split[0].ToLower().Contains("stack")) 
                    {
                        if (split.Length != 1)
                        {
                            if (split[1].Substring(split[1].Length - 1, 1).ToLower() == "h")
                            {
                                //dimensiunea stivei e declarata de utilizator si e in hexa
                                SS = Convert.ToInt32(split[1].Substring(0, split[1].Length - 1)); 
                                amf.ResultTextBox.AppendText("Dimensiunea Stivei(SS) "+SS.ToString());
                            }
                            else
                            {
                                //dimensiunea stivei e declarata de utilizator, dar nu e in hexa
                                SS = Convert.ToInt32(split[1], 16); 
                                amf.ResultTextBox.AppendText("Dimensiunea Stivei(SS) "+SS.ToString()); 
                            }

                        }
                        else
                        {
                            //dimensiunea stivei standard
                            SS = Convert.ToInt32(1024); 
                            amf.ResultTextBox.AppendText("Dimensiunea Stivei(SS) " + SS.ToString()); 
                        }
                    }
                    else if (split[0].ToLower().Contains("data")) 
                    {
                        DS = 0;
                    }
                    else if (split[0].ToLower().Contains("code"))
                    { 
                        CS = 0;
                    }
                }

                else if (split[0].ToLower().Contains("end"))
                {
                    break;
                }

                //variabile
                else if (split[0].Contains("#"))
                { 
                    switch (split[1].ToUpper())
                    {
                        case "DB":
                            if (split[2].Substring(split[2].Length - 1, 1).ToLower() == "h")
                            {
                                variabileDB.Add(Convert.ToInt32(split[2].Substring(0, split[2].Length - 1)));
                            }
                            else
                                variabileDB.Add(Convert.ToInt32(split[2], 16));
                            noOfVariables++;
                            break;
                        case "DW":
                            if (split[2].Substring(split[2].Length - 1, 1).ToLower() == "h")
                            {
                                variabileDW.Add(Convert.ToInt32(split[2].Substring(0, split[2].Length - 1)));
                            }
                            else
                                variabileDW.Add(Convert.ToInt32(split[2], 16));
                            noOfVariables++;
                            break;
                        case "DD":
                            if (split[2].Substring(split[2].Length - 1, 1).ToLower() == "h")
                            {
                                variabileDD.Add(Convert.ToInt32(split[2].Substring(0, split[2].Length - 1)));
                            }
                            else
                            {
                                variabileDD.Add(Convert.ToInt32(split[2], 16));
                            }
                            noOfVariables++;
                            break;
                        default:
                            break;
                    }
                }

                //eticheta
                else if (split[0].Contains("*"))
                {
                    etichete.Add(split[0].Substring(0, split[0].Length - 1), CS + 1);
                    nrEtichete++;
                }

                //instructiune
                else
                {
                    CS++;
                    PC = PC + 2;
                    instructionCode = 0;
                    indexedCode = 0;
                    indexedCode2 = 0;

                    //verificare daca e instr cu 2 operanzi
                    foreach (KeyValuePair<string, int> kvp in twoOperandsInstructions) {
                        if (split[0].ToUpper().Equals(kvp.Key)) {
                            //verificare mod de adresare

                            
                            string[] splitParametersByComma = split[1].Split(',', ' ');
                            
                            //pun codul intructiunii in instructionCode
                            instructionCode = Convert.ToInt32(kvp.Value) << 12;
                            
                            //mod direct pt primul operand
                            if (splitParametersByComma[1].ToUpper().Substring(0, 1) == "R")
                            {
                                //pun modul de adresare pt primul operand (01)
                                instructionCode = instructionCode + (1 << 10);
                                foreach (KeyValuePair<string, int> k in registers)
                                {
                                    if (splitParametersByComma[1].ToUpper() == k.Key)
                                    {
                                        //pun valoarea registrului
                                        instructionCode = instructionCode + (k.Value << 6);
                                        break;
                                    }
                                }
                            }

                            //mod indirect pt primul operand
                            else if (splitParametersByComma[1].Substring(0, 1) == "(" && splitParametersByComma[1].Substring(splitParametersByComma[1].Length - 1, 1) == ")")
                            {
                                //pun modul de adresare pt primul operand (10)
                                instructionCode = instructionCode + (2 << 10);
                                foreach (KeyValuePair<string, int> k in registers)
                                {
                                    if (splitParametersByComma[1].Substring(1, splitParametersByComma[1].Length - 2).ToUpper() == k.Key)
                                    {
                                        //pun valoarea registrului
                                        instructionCode = instructionCode + (k.Value << 6);
                                        break;
                                    }
                                }
                            }
                           
                            //mod indexat cu indexul la final pt primul operand
                            else if ((splitParametersByComma[1].Substring(0, 1) == "(" && splitParametersByComma[1].Substring(splitParametersByComma[1].Length - 1, 1).Any(c => char.IsDigit(c))))
                            {   
                                //pun modul de adresare pt primul operand (11)
                                instructionCode = instructionCode + (3 << 10);
                                indexedCode = Convert.ToInt32(splitParametersByComma[1].Substring(splitParametersByComma[1].IndexOf(')') + 1));
                                PC += 2;
                               foreach (KeyValuePair<string, int> k in registers)
                                {
                                    if (splitParametersByComma[1].Substring(splitParametersByComma[1].IndexOf('(') + 1, splitParametersByComma[1].IndexOf(')') - 1).ToUpper() == k.Key)
                                    {
                                       //pun valoarea registrului
                                        instructionCode = instructionCode + (k.Value << 6);
                                        break;
                                    }
                                }
                            }
                            
                            //mod indexat cu indexul la inceput pt primul operand
                            else if ((splitParametersByComma[1].Substring(splitParametersByComma[1].Length - 1, 1) == ")" && splitParametersByComma[1].Substring(0, 1).Any(c => char.IsDigit(c))))
                            {
                                //pun modul de adresare pt primul operand (11)
                                instructionCode = instructionCode + (3 << 10);
                                indexedCode = Convert.ToInt32(splitParametersByComma[1].Substring(0, splitParametersByComma[1].IndexOf('(')));
                                PC += 2;
                                string[] getRegister = splitParametersByComma[1].Split('(', ')');
                                foreach (KeyValuePair<string, int> k in registers)
                                {
                                    if (getRegister[1].ToUpper() == k.Key)
                                    {
                                        //pun valoarea registrului
                                        instructionCode = instructionCode + (k.Value << 6);
                                        break;
                                    }
                                }
                            }
                            
                            //mod imediat
                            else
                            {
                                //pun modul de adresare pt primul operand (00)
                                instructionCode = instructionCode + (0 << 4);
                                indexedCode = Convert.ToInt32(splitParametersByComma[1]);
                                PC += 2;
                            }

                            //verificare mod de adresare pt destinatie
                            //mod direct
                            if (splitParametersByComma[0].Substring(0, 1) == "R")
                            {
                                instructionCode += (1 << 4);
                                foreach (KeyValuePair<string, int> k in registers)
                                {
                                    if (splitParametersByComma[0].ToUpper() == k.Key)
                                    {
                                        
                                        instructionCode = instructionCode + k.Value;
                                        break;
                                    }
                                }
                            }

                            //mod indirect
                            else if (splitParametersByComma[0].Substring(0, 1) == "(" && splitParametersByComma[0].Substring(splitParametersByComma[0].Length - 1, 1) == ")")
                            {
                                //pun (10)
                                instructionCode = instructionCode + (2 << 4);
                                foreach (KeyValuePair<string, int> k in registers)
                                {
                                    if (splitParametersByComma[0].Substring(1, splitParametersByComma[0].Length - 2).ToUpper() == k.Key)
                                    {
                                        //pun valoarea registrului
                                        instructionCode = instructionCode + k.Value;
                                        break;
                                    }
                                }
                            }

                            //mod indexat cu indexul la final
                            else if ((splitParametersByComma[0].Substring(0, 1) == "(" && splitParametersByComma[0].Substring(splitParametersByComma[0].Length - 1, 1).Any(c => char.IsDigit(c))))
                            {
                                //pun (11)
                                instructionCode = instructionCode + (3 << 4);
                                indexedCode2 = Convert.ToInt32(splitParametersByComma[0].Substring(splitParametersByComma[0].IndexOf(')') + 1));
                                PC += 2;
                                foreach (KeyValuePair<string, int> k in registers)
                                {
                                    if (splitParametersByComma[0].Substring(splitParametersByComma[0].IndexOf('(') + 1, splitParametersByComma[0].IndexOf(')') - 1).ToUpper() == k.Key)
                                    {
                                        //pun val registrului
                                        instructionCode = instructionCode + k.Value;
                                        break;
                                    }
                                }
                            }

                            //mod indexat cu indexul la inceput
                            else if ((splitParametersByComma[0].Substring(splitParametersByComma[0].Length - 1, 1) == ")" && splitParametersByComma[0].Substring(0, 1).Any(c => char.IsDigit(c))))
                            {   
                                //pun(11)
                                instructionCode = instructionCode + (3 << 4);
                                indexedCode2 = Convert.ToInt32(splitParametersByComma[0].Substring(0, splitParametersByComma[0].IndexOf('(')));
                                PC += 2;
                                string[] getRegister = splitParametersByComma[0].Split('(', ')');
                                foreach (KeyValuePair<string, int> k in registers)
                                {
                                    if (getRegister[1].ToUpper() == k.Key)
                                    {
                                        //pun valoarea registrului
                                        instructionCode = instructionCode + k.Value;
                                        break;
                                    }
                                }
                            }
                            else {
                                MessageBox.Show("Cod Ilegal");
                            }
                        }
                    }
                    

                    //instructiuni cu un operand
                    foreach (KeyValuePair<string, int> k in oneOperandInstructions)
                    {
                        if (split[0].ToUpper().Equals(k.Key) && split[1].ToUpper() != "PC" && split[1].ToUpper() != "FLAG") {
                            instructionCode = Convert.ToInt32(k.Value) << 6;

                            //verificare mod de adresare pt operand
                            
                            //mod direct
                            if (split[1].ToUpper().Substring(0, 1) == "R")
                            {
                                //pun (01)
                                instructionCode = instructionCode + (1 << 4);
                                foreach (KeyValuePair<string, int> kk in registers)
                                {
                                    if (split[1].ToUpper() == kk.Key)
                                    {
                                        //pun val registrului
                                        instructionCode = instructionCode + kk.Value;
                                        break;
                                    }
                                }
                            }

                            //mod indirect
                            else if (split[1].Substring(0, 1) == "(" && split[1].Substring(split[1].Length - 1, 1) == ")")
                            {
                                //pun (10)
                                instructionCode += (2 << 4);

                                foreach (KeyValuePair<string, int> kk in registers)
                                {
                                    if (split[1].Substring(1, split[1].Length - 2).ToUpper() == kk.Key)
                                    {
                                        //pun val. registrului
                                        instructionCode = instructionCode + kk.Value;
                                        break;
                                    }
                                }
                            }

                            //mod indexat cu indexul la sfarsit
                            else if (split[1].Substring(0, 1) == "(" && split[1].Substring(split[1].Length - 1, 1).Any(c => char.IsDigit(c)))
                            {
                                //pun (11)
                                instructionCode += (3 << 4);
                                indexedCode = Convert.ToInt32(split[1].Substring(split[1].IndexOf(')') + 1));
                                PC += 2;
                                foreach (KeyValuePair<string, int> kk in registers)
                                {
                                    if (split[1].Substring(split[1].IndexOf('(') + 1, split[1].IndexOf(')') - 1).ToUpper() == kk.Key)
                                    {
                                        //pun val registrului
                                        instructionCode = instructionCode + kk.Value;
                                        break;
                                    }
                                }
                            }

                            //mod indexat cu indexul la inceput
                            else if ((split[1].Substring(split[1].Length - 1, 1) == ")" && split[1].Substring(0, 1).Any(c => char.IsDigit(c))))
                            {
                                //pun (11)
                                instructionCode += (3 << 4);
                                indexedCode = Convert.ToInt32(split[1].Substring(0, split[1].IndexOf('(')));
                                PC += 2;
                                string[] getRegister = split[1].Split('(', ')');
                                foreach (KeyValuePair<string, int> kk in registers)
                                {
                                    if (getRegister[1].ToUpper() == kk.Key)
                                    {
                                        //pun valoarea registrului
                                        instructionCode = instructionCode + kk.Value;
                                        break;
                                    }
                                }
                            }

                            //mod imediat
                            else {
                                MessageBox.Show("Code ilegal!");
                            }
                        }
                    }
                   
                    //instructiuni de branch
                    foreach (KeyValuePair<string, int> k in branchInstructions) {
                        if (split[0].ToUpper().Equals(k.Key))
                        {
                            instructionCode = Convert.ToInt32(k.Value) << 8;
                            bool found = false;
                            foreach (KeyValuePair<string, int> et in etichete)
                            {
                                if (split[1].Equals(et.Key.Substring(1,et.Key.Length-1)))
                                {
                                    etJumpValue = et.Value*2;
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                MessageBox.Show("Eticheta incorecta!");
                            }
                            else {
                                if (CS > etJumpValue)
                                {
                                    amf.ResultTextBox.AppendText("PC "+PC.ToString());
                                    instructionCode = instructionCode + ((~(PC)+1)&0x00FF)+etJumpValue;
                                    amf.ResultTextBox.AppendText("\n");
                                    amf.ResultTextBox.AppendText("ALaBALA"+ instructionCode.ToString()); 
                                }
                                else
                                {
                                    instructionCode = instructionCode + PC;
                                    amf.ResultTextBox.AppendText("\n");
                                    amf.ResultTextBox.AppendText(instructionCode.ToString());
                                }
                            }
                        }
                    }

                    //alte instructiuni
                    if (split.Length.Equals(1))
                    {
                        foreach (KeyValuePair<string, int> k in otherInstructions)
                        {
                            if (split[0].ToUpper().Equals(k.Key))
                            {
                                instructionCode = k.Value;
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (KeyValuePair<string, int> k in otherInstructions)
                        {
                           
                            if ((split[0].ToUpper() + " " + split[1].ToUpper()).Equals(k.Key))
                            {
                                instructionCode = k.Value;
                                break;
                            }
                        }
                    }

                    outputFile[outputFileIndex] = instructionCode;
                    outputFileIndex++;
                    if (indexedCode != 0)
                    {
                        outputFile[outputFileIndex] = indexedCode;
                        outputFileIndex++;
                        if (indexedCode2 != 0)
                        {
                            outputFile[outputFileIndex] = indexedCode2;
                            outputFileIndex++;
                        }
                    }
                }
            }

            for (int i = 0; i < outputFileIndex; i++)
            {
                amf.ResultTextBox.AppendText("\n");
                amf.ResultTextBox.AppendText(outputFile[i].ToString());
            }
            writeToBinFile();

            amf.ResultTextBox.AppendText("\n");
            foreach (KeyValuePair<string, int> k in etichete) {
                amf.ResultTextBox.AppendText("\n");
                amf.ResultTextBox.AppendText(k.Key + " " + k.Value);
            } 

            amf.ResultTextBox.AppendText("CS " + CS.ToString());
        }

        //scriere in fisier .bin
        public void writeToBinFile() {
            Stream s = new FileStream(outputFileName,FileMode.Create);
            BinaryWriter bw = new BinaryWriter(s);
            UInt16 ui;

            for (int i = 0; i < outputFileIndex; i++) {
                ui = Convert.ToUInt16(outputFile[i]);
                bw.Write(ui);
            }
            bw.Close();
        }


    }
}
