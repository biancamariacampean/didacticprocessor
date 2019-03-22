using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerApplication
{
    public class Secventiator
    {
        public abstract class B1 {
            public const UInt16 CLASS_BITS_MASK = 0x0;
            public abstract class Mask
            {
                public const UInt16 OP_CODE = 0xF000;
                public const UInt16 MAS = 0xC00;
                public const UInt16 RS = 0x3C0;
                public const UInt16 MAD = 0x30;
                public const UInt16 RD = 0xF;
            }

            public abstract class PositionsToShift
            {
                public const UInt16 OP_CODE = 12;
                public const UInt16 MAS = 10;
                public const UInt16 RS = 6;
                public const UInt16 MAD = 4;
                public const UInt16 RD = 0;
            }
            public static bool IsLegalInstruction(UInt16 code)
            {
                if ((code & 0x8000) != CLASS_BITS_MASK)
                {
                    return false;
                }

                return (UInt16)((UInt16)(code & Mask.OP_CODE) >> PositionsToShift.OP_CODE) <= 0x6;
            }
        }
        public abstract class B2 {
            public const UInt16 CLASS_BITS_MASK = 0x8000;
            public abstract class Mask
            {
                public const UInt16 OP_CODE = 0xFFC0;
                public const UInt16 MAD = 0x30;
                public const UInt16 RD = 0xF;
            }

            public abstract class PositionsToShift
            {
                public const UInt16 OP_CODE = 6;
                public const UInt16 MAD = 4;
                public const UInt16 RD = 0;
            }
            public static bool IsLegalInstruction(UInt16 code)
            {
                if ((code & 0xc000) != CLASS_BITS_MASK)
                {
                    return false;
                }

                return (UInt16)((UInt16)(code & ~CLASS_BITS_MASK & Mask.OP_CODE) >> PositionsToShift.OP_CODE) <= 0xe;
            }
        }
        public abstract class B3
        {
            public const UInt16 CLASS_BITS_MASK = 0xc000;

            public abstract class Mask
            {
                public const UInt16 OP_CODE = 0xFF00;
                public const UInt16 OFFSET = 0xFF;
            }

            public abstract class PositionsToShift
            {
                public const UInt16 OP_CODE = 8;
                public const UInt16 OFFSET = 0;
            }

            public static bool IsLegalInstruction(UInt16 code)
            {
                if ((code & 0xe000) != CLASS_BITS_MASK)
                {
                    return false;
                }

                return (UInt16)((UInt16)(code & ~CLASS_BITS_MASK & Mask.OP_CODE) >> PositionsToShift.OP_CODE) <= 0x8;
            }
        }
        public abstract class B4 {
            public const UInt16 CLASS_BITS_MASK = 0xe000;

            public static bool IsLegalInstruction(UInt16 code)
            {
                if ((code & 0xe000) != CLASS_BITS_MASK)
                {
                    return false;
                }

                return ((UInt16)(code & ~CLASS_BITS_MASK)) <= 0x12;
            }
        }

        public abstract class AddressingMode {
            public const UInt16 IMMEDIATE = 0;
            public const UInt16 DIRECT = 1;
            public const UInt16 INDIRECT = 2;
            public const UInt16 INDEXED = 3;
        }
        private abstract class Mask {
            public const UInt64 SBUS = 0x1E00000000;    //4b
            public const UInt64 DBUS = 0x1C0000000;     //3b
            public const UInt64 ALU = 0x3C000000;       //4b
            public const UInt64 RBUS = 0x3800000;       //3b
            public const UInt64 MEM = 0x600000;         //2b
            public const UInt64 OTHER = 0x1F0000;       //5b
            public const UInt64 COND = 0xF000;          //4b
            public const UInt64 INDEX = 0xE00;          //3b
            public const UInt64 TF = 0x100;             //1b
            public const UInt64 UADR = 0xFF;            //8b

            public const UInt16 CARRY = 0x8;
            public const UInt16 ZERO = 0x4;
            public const UInt16 SIGN = 0x2;
            public const UInt16 OVERFLOW = 0x1;
        }

        private abstract class FieldsShiftPosition
        {
            public const int SBUS = 33;
            public const int DBUS = 30;
            public const int ALU = 26;
            public const int RBUS = 23;
            public const int MEM = 21;
            public const int OTHER = 16;
            public const int COND = 12;
            public const int INDEX = 9;
            public const int TF = 8;
            public const int UADR = 0;

            public const int CARRY = 3;
            public const int ZERO = 2;
            public const int SIGN = 1;
            public const int OVERFLOW = 0;
        }

        private byte MAR; //adreseaza continutul mpm
        private UInt64 MIR; //retine fiecare microinstructiune

        public enum State : byte {
            s0,
            s1,
            s2
        } // stari secventiator
        public enum SursaSBUS : byte
        {
            NONE,
            PD_FLAG,
            PD_RG,
            PD_SP,
            PD_T,
            PD_NT,
            PD_PC,
            PD_IVR,
            PD_MDR,
            PD_0
        } //enum microcomenzi SBUS
        public enum SursaDBUS : byte
        {
            NONE,
            PD_IR_OFF,
            PD_RG,
            PD_MDR,
            PD_N1,
            PD_0
        } //enum microcomenzi DBUS
        public enum ALU : byte
        {
            NONE,
            NSBUS,
            SUM,
            AND,
            OR,
            XOR,
            ASL,
            ASR,
            LSR,
            ROL,
            ROR,
            RLC,
            RRC
        } //enum microcomenzi ALU
        public enum DestinatieRBUS : byte
        {
            NONE,
            PM_RG,
            PM_SP,
            PM_T,
            PM_PC,
            PM_IVR,
            PM_ADR,
            PM_MDR
        } //enum microcomenzi RBUS
        public enum MemOP : byte
        {
            NOP,
            IFCH,
            READ,
            WRITE
        } //enum microcomenzi Memory Operations
        public enum OtherOperations : byte
        {
            NOP,
            CIN_AND_PD_COND,
            INTA_AND_MINUS_TWO_SP,
            PD_COND,
            CIN,
            PM_FLAG,
            PLUS_TWO_SP,
            MINUS_TWO_SP,
            PLUS_TWO_PC,
            SET_BE0,
            SET_BE1,
            RESET_C,
            SET_C,
            RESET_Z,
            SET_Z,
            RESET_S,
            SET_S,
            RESET_V,
            SET_V,
            RESET_CZSV,
            SET_CZSV
        } //enum microcomenzi Other Operations
        public enum BranchCondition : byte
        {
            NONE,
            B1,
            AM,
            AD,
            INTR,
            ACLOW,
            CIL,
            C,
            Z,
            S,
            V
        } //enum microcomenzi conditii de branch

        protected byte[] memory; //memoria unde se incarca programul
        private static readonly UInt64[] mpm =
        {
            0xD4B285083,
0x6084,
0x203,
0x407,
0x610,
0xC47,
0xE59,
0xD4B480100,
0x1149800610,
0x549800100,
0x610,
0x54B400100,
0x1149800610,
0xD4B480100,
0x4CB400100,
0x1149800610,
0xD4B480015,
0x128B800015,
0x128B400015,
0xD4B480100,
0x108B400100,
0x1817,
0xA25,
0x94B803145,
0x1148800081,
0x8CB833145,
0x1148800081,
0xACB813145,
0x1148800081,
0xACB810081,
0x100,
0x8CF833145,
0x1148800081,
0x8D3833145,
0x1148800081,
0x8D7833145,
0x1148800081,
0x134B833145,
0x1148800081,
0x1007833145,
0x1148800081,
0x100B813145,
0x1148800081,
0x110B833145,
0x1148800081,
0x101B803145,
0x1148800081,
0x101F803145,
0x1148800081,
0x1023803145,
0x1148800081,
0x1027803145,
0x1148800081,
0x102B803145,
0x1148800081,
0x102F803145,
0x1148800081,
0x1033803145,
0x1148800081,
0x114A000081,
0x100,
0x73184,
0x74B600081,
0x74B403184,
0x1148860081,
0x1149800100,
0xD4B870100,
0x74B600100,
0x94A000081,
0x2084,
0x600081,
0xC4A000081,
0x100,
0x8147,
0x81,
0x8047,
0x81,
0x9147,
0x81,
0x9047,
0x81,
0x7047,
0x81,
0x7147,
0x81,
0xA047,
0x81,
0xA147,
0x81,
0xB0081,
0x100,
0x110081,
0x100,
0xD0081,
0x100,
0xF0081,
0x100,
0x130081,
0x100,
0xC0081,
0x100,
0x120081,
0x100,
0xE0081,
0x100,
0x100081,
0x100,
0x140081,
0x100,
0x81,
0x100,
0x100,
0x100,
0x4085,
0x71,
0xD4B870100,
0x74B600081,
0x74B400100,
0x114A060081,
0x34B870100,
0x74B600081,
0x74B460100,
0x1148050081,
0x74B400100,
0x114A060081,
0x74B460100,
0x1148050100,
0x74B460100,
0x114A000081,
0x4085,
0x0,
0x90085,
0xA0085,
0xD4B820100,
0x74B600100,
0x34B870100,
0x74B600100,
0xF4A000100,
0x134A000000
        }; //contine microprogramul de emulare

        public State state;
        private Int16 DBUS;
        private Int16 SBUS;
        private Int16 RBUS;
        private Int16[] REG;
        private Int16 T; //registru tampon
        private Int16 MDR;

        private UInt16 FLAG;
        private UInt16 IR;
        private UInt16 SP;
        private UInt16 PC;
        private UInt16 IVR;
        private UInt16 ADR;

        private bool ACLOW;
        private bool CIL;
        private bool CLK;

        private bool SBUSINT; //signed SBUS
        private bool DBUSINT; //signed DBUS

        protected Simulator s;
        public Secventiator(Simulator s) {
            this.s = s;
        }

        public byte[] GetMemory()
        {
            return memory;
        }
        public UInt16 GetSP()
        {
            return SP;
        }
        public byte GetState()
        {
            return (byte)state;
        }
        public Int16 GetDBUS()
        {
            return DBUS;
        }
        public Int16 GetSBUS()
        {
            return SBUS;
        }
        public Int16 GetRBUS()
        {
            return RBUS;
        }
        public Int16[] GetREG()
        {
            return REG;
        }
        public Int16 GetT()
        {
            return T;
        }
        public Int16 GetMDR()
        {
            return MDR;
        }
        public UInt16 GetFLAG()
        {
            return FLAG;
        }
        public UInt16 GetIR()
        {
            return IR;
        }
        public UInt16 GetPC()
        {
            return PC;
        }
        public UInt16 GetIVR()
        {
            return IVR;
        }
        public UInt16 GetADR()
        {
            return ADR;
        }
        public bool GetACLOW()
        {
            return ACLOW;
        }
        public bool GetCIL()
        {
            return CIL;
        }
        public bool GetSBUSINT()
        {
            return SBUSINT;
        }
        public bool GetDBUSINT()
        {
            return DBUSINT;
        }
        public byte GetMAR()
        {
            return MAR;
        }
        public UInt64 GetMIR()
        {
            return MIR;
        }
        public void SetState(State s)
        {
            this.state = s;
        }
        public SursaSBUS sBUS { get; set; }
        public SursaDBUS dBUS { get; set; }
        public ALU alu { get; set; }
        public DestinatieRBUS rBUS { get; set; }
        public MemOP memOP { get; set; }
        public OtherOperations otherOperations { get; set; }
        public BranchCondition branchCondition { get; set; }
        public byte uadrPLUSidx { get; set; }
        public byte microAdr { get; set; }


        public Secventiator() { }

        public void fillMemory() {
            Assembler asm = new Assembler();
            string binaryFilePath = asm.getFileName("BIN file for didactical processor(*.bin)|*.bin");
            using (BinaryReader br = new BinaryReader(File.Open(binaryFilePath, FileMode.Open)))
            {
                byte[] temp = br.ReadBytes((int)new FileInfo(binaryFilePath).Length);
                memory = new byte[UInt16.MaxValue + 1];
                memory.Initialize();
                Array.Copy(temp, 0, memory, 0, temp.Length);
            }
        }

        public void Start()
        {
            switch (state) {
                case State.s0:
                    {
                        MIR = mpm[MAR];

                        if (s.ShowHexBtn.Text == "Show Hex")
                            s.MIRTextBox.Text = Convert.ToString(Convert.ToInt64(MIR), 2);
                        else
                            s.MIRTextBox.Text = "0x"+Convert.ToString(Convert.ToInt64(MIR), 16);
                        state = State.s1;
                        break;
                    }
                case State.s1:
                    {
                        SursaSBUS ss = (SursaSBUS)((MIR & Mask.SBUS) >> FieldsShiftPosition.SBUS);
                        SursaDBUS ds = (SursaDBUS)((MIR & Mask.DBUS) >> FieldsShiftPosition.DBUS);
                        ALU ao = (ALU)((MIR & Mask.ALU) >> FieldsShiftPosition.ALU);
                        DestinatieRBUS rd = (DestinatieRBUS)((MIR & Mask.RBUS) >> FieldsShiftPosition.RBUS);
                        OtherOperations oo = (OtherOperations)((MIR & Mask.OTHER) >> FieldsShiftPosition.OTHER);

                        UInt16 cond = 0;
                        switch (ss)
                        {
                            case SursaSBUS.NONE:
                                {
                                    break;
                                }

                            case SursaSBUS.PD_FLAG:
                                {
                                    
                                    SBUS = (Int16)FLAG;
                                    SBUSINT = false;
                                    break;
                                }

                            case SursaSBUS.PD_RG:
                                {
                                    
                                    SBUS = REG[((IR & B1.Mask.RS) >> B1.PositionsToShift.RS)];
                                    SBUSINT = true;
                                    break;
                                }

                            case SursaSBUS.PD_SP:
                                {
                                    
                                    SBUS = (Int16)SP;
                                    SBUSINT = false;
                                    break;
                                }

                            case SursaSBUS.PD_T:
                                {
                                    SBUS = T;
                                    SBUSINT = true;
                                    break;
                                }

                            case SursaSBUS.PD_NT:
                                {
                                    SBUS = (Int16)~T;
                                    SBUSINT = true;
                                    break;
                                }

                            case SursaSBUS.PD_PC:
                                {
                                    SBUS = (Int16)PC;
                                    SBUSINT = false;
                                    break;
                                }

                            case SursaSBUS.PD_IVR:
                                {
                                    SBUS = (Int16)IVR;
                                    SBUSINT = false;
                                    break;
                                }

                            case SursaSBUS.PD_MDR:
                                {
                                    SBUS = MDR;
                                    SBUSINT = true;
                                    break;
                                }

                            case SursaSBUS.PD_0:
                                {
                                    SBUS = 0;
                                    SBUSINT = false;
                                    break;
                                }
                        }
                        sBUS = ss;
                        switch (ds)
                        {
                            case SursaDBUS.NONE:
                                {
                                    break;
                                }

                            case SursaDBUS.PD_IR_OFF:
                                {
                                    DBUS = Convert.ToInt16((SByte)(IR & 0xFF));
                                    DBUSINT = true;
                                    break;
                                }

                            case SursaDBUS.PD_RG:
                                {
                                    if (getClasa() == 0)
                                    {
                                        //instructiune cu 2 operanzi
                                        DBUS = REG[((IR & B1.Mask.RD) >> B1.PositionsToShift.RD)];
                                    }
                                    else
                                    {
                                        //instructiune cu 1 operand
                                        DBUS = REG[((IR & B2.Mask.RD) >> B2.PositionsToShift.RD)];
                                    }
                                    DBUSINT = true;
                                    break;
                                }

                            case SursaDBUS.PD_MDR:
                                {
                                    DBUS = MDR;
                                    DBUSINT = true;
                                    break;
                                }

                            case SursaDBUS.PD_N1:
                                {
                                    DBUS = -1;
                                    DBUSINT = true;
                                    break;
                                }

                            case SursaDBUS.PD_0:
                                {
                                    DBUS = 0;
                                    DBUSINT = false;
                                    break;
                                }
                        }
                        dBUS = ds;
                        switch (ao)
                        {
                            case ALU.NONE:
                                {
                                    break;
                                }

                            case ALU.NSBUS:
                                {
                                    RBUS = (Int16)~SBUS;
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.SIGN, (RBUS < 0));
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.ZERO, (RBUS == 0));
                                    break;
                                }

                            case ALU.SUM:
                                {
                                    RBUS = (Int16)(SBUS + DBUS + ((oo == OtherOperations.CIN || oo == OtherOperations.CIN_AND_PD_COND) ? 1 : 0));
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.SIGN, (RBUS < 0));
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.ZERO, (RBUS == 0));

                                    if (SBUSINT || DBUSINT)
                                    {
                                        Int32 fullResult = (SBUS + DBUS);
                                        SetRegistruFLAG(ref cond, FieldsShiftPosition.OVERFLOW, (fullResult > Int16.MaxValue || fullResult < Int16.MinValue));
                                    }
                                    else
                                    {
                                        UInt32 fullResult = (UInt32)(UInt16)SBUS + (UInt32)(UInt16)DBUS;
                                        SetRegistruFLAG(ref cond, FieldsShiftPosition.CARRY, ((fullResult & 0x10000) == 0x10000));
                                    }                                 
                                    break;
                                }

                            case ALU.AND:
                                {
                                    RBUS = (Int16)(SBUS & DBUS);
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.SIGN, (RBUS < 0));
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.ZERO, (RBUS == 0));
                                    break;
                                }

                            case ALU.OR:
                                {
                                    RBUS = (Int16)(SBUS | DBUS);
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.SIGN, (RBUS < 0));
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.ZERO, (RBUS == 0));
                                    break;
                                }

                            case ALU.XOR:
                                {
                                    RBUS = (Int16)(SBUS ^ DBUS);
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.SIGN, (RBUS < 0));
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.ZERO, (RBUS == 0));
                                    break;
                                }

                            case ALU.ASL:
                                {
                                    RBUS = (Int16)(SBUS << 1);
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.SIGN, (RBUS < 0));
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.ZERO, (RBUS == 0));
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.CARRY, (RBUS < 0));
                                   break;
                                }

                            case ALU.ASR:
                                {
                                    RBUS = (Int16)(SBUS >> 1);
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.SIGN, (RBUS < 0));
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.ZERO, (RBUS == 0));
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.CARRY, ((SBUS & 1) == 1));
                                    break;
                                }

                            case ALU.LSR:
                                {
                                    RBUS = (Int16)((UInt16)SBUS >> 1);
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.SIGN, (RBUS < 0));
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.ZERO, (RBUS == 0));
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.CARRY, ((SBUS & 1) == 1));
                                    break;
                                }

                            case ALU.ROL:
                                {
                                    RBUS = (Int16)((SBUS << 1) | ((UInt16)SBUS >> 15));
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.SIGN, (RBUS < 0));
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.ZERO, (RBUS == 0));
                                    break;
                                }

                            case ALU.ROR:
                                {
                                    RBUS = (Int16)(((UInt16)SBUS >> 1) | (SBUS << 15));
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.SIGN, (RBUS < 0));
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.ZERO, (RBUS == 0));
                                    break;
                                }

                            case ALU.RLC:
                                {
                                    RBUS = (Int16)((SBUS << 1) | ((FLAG & Mask.CARRY) >> FieldsShiftPosition.CARRY));
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.SIGN, (RBUS < 0));
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.ZERO, (RBUS == 0));
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.CARRY, (SBUS < 0));
                                    break;
                                }

                            case ALU.RRC:
                                {
                                    RBUS = (Int16)(((UInt16)SBUS >> 1) | ((FLAG & Mask.CARRY) << (15 - FieldsShiftPosition.CARRY)));
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.SIGN, (RBUS < 0));
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.ZERO, (RBUS == 0));
                                    SetRegistruFLAG(ref cond, FieldsShiftPosition.CARRY, ((SBUS & 1) == 1));
                                    break;
                                }
                        }
                        alu = ao;
                        switch (rd)
                        {
                            case DestinatieRBUS.NONE:
                                {
                                    break;
                                }

                            case DestinatieRBUS.PM_RG:
                                {
                                    int cls = getClasa();
                                    int i = 0;

                                    if (cls == 0 || cls == 1)
                                    {
                                        REG[(IR & B1.Mask.RD)] = RBUS;
                                    }
                                    else
                                    {
                                        CIL = true;
                                    }
                                    break;
                                }

                            case DestinatieRBUS.PM_SP:
                                {
                                    SP = (UInt16)RBUS;
                                    break;
                                }

                            case DestinatieRBUS.PM_T:
                                {
                                    T = RBUS;
                                    break;
                                }

                            case DestinatieRBUS.PM_PC:
                                {
                                    PC = (UInt16)RBUS;
                                    break;
                                }

                            case DestinatieRBUS.PM_IVR:
                                {
                                    IVR = (UInt16)RBUS;
                                    break;
                                }

                            case DestinatieRBUS.PM_ADR:
                                {
                                    ADR = (UInt16)RBUS;
                                    break;
                                }

                            case DestinatieRBUS.PM_MDR:
                                {
                                    MDR = RBUS;
                                    break;
                                }
                        }
                        rBUS = rd;
                        switch (oo)
                        {
                            case OtherOperations.NOP:
                                {
                                    break;
                                }

                            case OtherOperations.CIN_AND_PD_COND:
                                {
                                    FLAG = cond;
                                    break;
                                }

                            case OtherOperations.INTA_AND_MINUS_TWO_SP:
                                {
                                    if (ACLOW)
                                    {
                                        ACLOW = false;
                                    }
                                    else
                                    {
                                        if (CIL)
                                        {
                                            
                                            CIL = false;
                                        }
                                    }

                                    SP = (UInt16)(SP - 2);
                                    break;
                                }

                            case OtherOperations.PD_COND:
                                {
                                    FLAG = cond;
                                    break;
                                }

                            case OtherOperations.CIN:
                                {
                                    break;
                                }

                            case OtherOperations.PM_FLAG:
                                {
                                    FLAG = (UInt16)RBUS;
                                    break;
                                }

                            case OtherOperations.PLUS_TWO_SP:
                                {
                                    SP = (UInt16)(SP + 2);
                                    break;
                                }

                            case OtherOperations.MINUS_TWO_SP:
                                {
                                    SP = (UInt16)(SP - 2);
                                    break;
                                }

                            case OtherOperations.PLUS_TWO_PC:
                                {
                                    PC = (UInt16)(PC + 2);
                                   break;
                                }

                            case OtherOperations.SET_BE0:
                                {
                                    ACLOW = true;
                                    break;
                                }
                            case OtherOperations.SET_BE1:
                                {
                                    CIL = true;
                                    break;
                                }

                            case OtherOperations.RESET_C:
                                {
                                    s.OOTextBox.Text = "RESET CARRY";
                                    SetRegistruFLAG(ref FLAG, FieldsShiftPosition.CARRY, false);
                                    break;
                                }

                            case OtherOperations.SET_C:
                                {
                                    s.OOTextBox.Text = "SET CARRY";
                                    SetRegistruFLAG(ref FLAG, FieldsShiftPosition.CARRY, true);
                                    break;
                                }

                            case OtherOperations.RESET_Z:
                                {
                                    s.OOTextBox.Text = "RESET ZERO";
                                    SetRegistruFLAG(ref FLAG, FieldsShiftPosition.ZERO, false);
                                    break;
                                }

                            case OtherOperations.SET_Z:
                                {
                                    s.OOTextBox.Text = "SET ZERO";
                                    SetRegistruFLAG(ref FLAG, FieldsShiftPosition.ZERO, true);
                                    break;
                                }

                            case OtherOperations.RESET_S:
                                {
                                    s.OOTextBox.Text = "RESET SIGN";
                                    SetRegistruFLAG(ref FLAG, FieldsShiftPosition.SIGN, false);
                                    break;
                                }

                            case OtherOperations.SET_S:
                                {
                                    s.OOTextBox.Text = "SET SIGN";
                                    SetRegistruFLAG(ref FLAG, FieldsShiftPosition.SIGN, true);
                                   break;
                                }

                            case OtherOperations.RESET_V:
                                {
                                    s.OOTextBox.Text = "RESET OVERFLOW";
                                    SetRegistruFLAG(ref FLAG, FieldsShiftPosition.OVERFLOW, false);
                                    break;
                                }

                            case OtherOperations.SET_V:
                                {
                                    s.OOTextBox.Text = "SET OVERFLOW";
                                    SetRegistruFLAG(ref FLAG, FieldsShiftPosition.OVERFLOW, true);
                                    break;
                                }

                            case OtherOperations.RESET_CZSV:
                                {
                                    //s.OOTextBox.Text = "RESET FLAG";
                                    FLAG = 0;
                                    break;
                                }

                            case OtherOperations.SET_CZSV:
                                {
                                    //s.OOTextBox.Text = "SET FLAG";
                                    FLAG = 0xF;
                                    break;
                                }
                        }
                        otherOperations = oo;
                        state = State.s2;
                        break;
                    }

                case State.s2:
                    {
                        MemOP mo = (MemOP)((MIR & Mask.MEM) >> FieldsShiftPosition.MEM);
                        
                        switch (mo)
                        {
                            case MemOP.NOP:
                                {
                                    break;
                                }

                            case MemOP.IFCH:
                                {
                                    IR = (UInt16)(memory[ADR] | (memory[ADR + 1] << 8));
                                    CIL = !(B1.IsLegalInstruction(IR) || B2.IsLegalInstruction(IR) || B3.IsLegalInstruction(IR) || B4.IsLegalInstruction(IR));
                                    break;
                                }

                            case MemOP.READ:
                                {
                                    MDR = (Int16)(memory[ADR] | (memory[ADR + 1] << 8));
                                    break;
                                }

                            case MemOP.WRITE:
                                {
                                    memory[ADR + 1] = (byte)(MDR >> 8);
                                    memory[ADR] = (byte)(MDR);
                                    break;
                                }
                        }
                        memOP = mo;

                        BranchCondition bc = (BranchCondition)((MIR & Mask.COND) >> FieldsShiftPosition.COND);
                        branchCondition = bc;
                        byte idx = (byte)((MIR & Mask.INDEX) >> FieldsShiftPosition.INDEX);
                        bool tf = (((MIR & Mask.TF) >> FieldsShiftPosition.TF) == 1);
                        byte uadr = (byte)(MIR & Mask.UADR);
                        microAdr = uadr;

                        if (GetGFunction(bc) ^ tf)
                        {
                            MAR = (byte)(uadr + GetIndex(idx));
                            if (s.ShowHexBtn.Text == "Show Hex")
                                s.MARTextBox.Text = Convert.ToString(MAR, 2).PadLeft(8, '0');
                            else
                                s.MARTextBox.Text = "0x" + Convert.ToString(MAR, 16).PadLeft(2, '0');
                            uadrPLUSidx = MAR;
                        }
                        else
                        {
                            MAR++;
                            if (s.ShowHexBtn.Text == "Show Hex")
                                s.MARTextBox.Text = Convert.ToString(MAR, 2).PadLeft(8, '0');
                            else
                                s.MARTextBox.Text = "0x" + Convert.ToString(MAR, 16).PadLeft(2, '0');
                            uadrPLUSidx = MAR;
                        }
                        
                        state = State.s0;
                        break;
                    }
            }
        }

        public UInt16 GetIndex(UInt16 index)
        {
            switch (index)
            {
                case 0:
                    {
                        return 0;
                    }

                case 1:
                    {
                        return (UInt16)getClasa();
                    }

                case 2:
                    {
                        return (UInt16)(((UInt16)(IR & B1.Mask.MAS) >> B1.PositionsToShift.MAS) << 1);
                    }

                case 3:
                    {
                        int cls = getClasa();

                        if (cls == 0)
                        {
                            return (UInt16)((UInt16)(IR & B1.Mask.MAD) >> B1.PositionsToShift.MAD);
                        }

                        return (UInt16)((UInt16)(IR & B2.Mask.MAD) >> B2.PositionsToShift.MAD);
                    }

                case 4:
                    {
                        return (UInt16)(((UInt16)(IR & B1.Mask.OP_CODE) >> B1.PositionsToShift.OP_CODE) << 1);
                    }

                case 5:
                    {
                        return (UInt16)(((UInt16)(IR & B2.Mask.OP_CODE & ~B2.CLASS_BITS_MASK) >> B2.PositionsToShift.OP_CODE) << 1);
                    }

                case 6:
                    {
                        return (UInt16)(((UInt16)(IR & B3.Mask.OP_CODE & ~B3.CLASS_BITS_MASK) >> B3.PositionsToShift.OP_CODE) << 1);
                    }

                case 7:
                    {
                        return (UInt16)((UInt16)(IR & ~B4.CLASS_BITS_MASK) << 1);
                    }

                default:
                    {
                        throw new ArgumentException("Undefined index");
                    }
            }
        }

        public void ResetSeq()
        {
            int i = 0;
            MAR = 0;
            if (s.ShowHexBtn.Text == "Show Hex")
                s.MARTextBox.Text = Convert.ToString(MAR, 2).PadLeft(8, '0');
            else
                s.MARTextBox.Text = "0x" + Convert.ToString(MAR, 16).PadLeft(2, '0');

            state = State.s0;

            DBUS = 0;
            SBUS = 0;
            RBUS = 0;

            FLAG = 0;
            if (s.ShowHexBtn.Text == "Show Hex")
                s.FLAGTextBox.Text = Convert.ToString(FLAG, 2).PadLeft(16, '0');
            else
                s.FLAGTextBox.Text = "0x" + Convert.ToString(FLAG, 16).PadLeft(4, '0');

            IR = 0;
            if (s.ShowHexBtn.Text == "Show Hex")
                s.IRTextBox.Text = Convert.ToString(IR, 2).PadLeft(16, '0');
            else
                s.IRTextBox.Text = "0x" + Convert.ToString(IR, 16).PadLeft(4, '0');

            SP = (UInt16)(memory.Length - 1 - 1024);
            if (s.ShowHexBtn.Text == "Show Hex")
                s.SPTextBox.Text = Convert.ToString(SP, 2).PadLeft(16, '0');
            else
                s.SPTextBox.Text = "0x" + Convert.ToString(SP, 16).PadLeft(4, '0');
            
            T = 0;
            if (s.ShowHexBtn.Text == "Show Hex")
                s.TTextBox.Text = Convert.ToString(T, 2).PadLeft(16, '0');
            else
                s.TTextBox.Text = "0x" + Convert.ToString(T, 16).PadLeft(4, '0');

            PC = 6;
            if (s.ShowHexBtn.Text == "Show Hex")
                s.PCTextBox.Text = Convert.ToString(PC, 2).PadLeft(16, '0');
            else
                s.PCTextBox.Text = "0x" + Convert.ToString(PC, 16).PadLeft(4, '0');

            IVR = 0;
            if (s.ShowHexBtn.Text == "Show Hex")
                s.IVRTextBox.Text = Convert.ToString(IVR, 2).PadLeft(16, '0');
            else
                s.IVRTextBox.Text = "0x" + Convert.ToString(IVR, 16).PadLeft(4, '0');

            ADR = 0;
            if (s.ShowHexBtn.Text == "Show Hex")
                s.ADRTextBox.Text = Convert.ToString(ADR, 2).PadLeft(16, '0');
            else
                s.ADRTextBox.Text = "0x" + Convert.ToString(ADR, 2).PadLeft(16, '0');

            MDR = 0;
            if (s.ShowHexBtn.Text == "Show Hex")
                s.MDRTextBox.Text = Convert.ToString(MDR, 2).PadLeft(16, '0');
            else
                s.MDRTextBox.Text = "0x" + Convert.ToString(MDR, 16).PadLeft(4, '0');

            REG = new Int16[16];
            REG.Initialize();
            s.ControlTextBox.AppendText("\n");

            if (s.ShowHexBtn.Text == "Show Hex")
            {
                s.RGTextBox.Text = "";
                foreach (Int16 reg in REG)
                {
                    s.RGTextBox.AppendText("R" + i.ToString() + " " + Convert.ToString(reg, 2).PadLeft(16, '0') + "\n");
                    i++;
                }
            }
            else
            {
                s.RGTextBox.Text = "";
                foreach (Int16 reg in REG)
                {
                    s.RGTextBox.AppendText("R" + i.ToString() + " 0x" + Convert.ToString(reg, 16).PadLeft(4, '0') + "\n");
                    i++;
                }
            }

            ACLOW = false;
            CIL = false;
            CLK = true;
        }


        private int getClasa()
        {
            int CL0 = (((IR & 0x8000) == 0x8000) && !((IR & 0x6000) == 0x4000)) ? 1 : 0; //CL0 = IR15 && !IR13 -> can be 1 or 0, 0x8000 = 1000.0000.0000.0000, 0x6000 = 0110.0000.0000.0000, 0x4000 = 0100.0000.0000.0000
            int CL1 = ((IR & 0xC000) == 0xC000) ? 2 : 0;

            return (CL0 | CL1);
        }

        public bool GetGFunction(BranchCondition br)
        {
            bool cl0 = (((IR & 0x8000) == 0x8000) && !((IR & 0x6000) == 0x4000));
            bool cl1 = ((IR & 0xC000) == 0xC000);

            switch (br)
            {
                case BranchCondition.NONE:
                    {
                        return true;
                    }

                case BranchCondition.B1:
                    {
                        return ((UInt16)(IR & 0x8000) == 0);
                    }

                case BranchCondition.AM:
                    {
                        int cls = getClasa();

                        if (cls == 0)
                        {
                            return (((IR & B1.Mask.MAD) >> B1.PositionsToShift.MAD) == AddressingMode.IMMEDIATE);
                        }

                        return (((IR & B2.Mask.MAD) >> B2.PositionsToShift.MAD) == AddressingMode.IMMEDIATE);
                    }

                case BranchCondition.AD:
                    {
                        if (!cl1 && !cl0)
                        {
                            return (((IR & B1.Mask.MAD) >> B1.PositionsToShift.MAD) == AddressingMode.DIRECT);
                        }

                        return (((IR & B2.Mask.MAD) >> B2.PositionsToShift.MAD) == AddressingMode.DIRECT);
                    }

                case BranchCondition.INTR:
                    {
                        if (s.checkBox1.Checked)
                        {
                            s.checkBox1.Checked = false;
                            return true;
                            
                        }
                        return (ACLOW || CIL || s.checkBox1.Checked);
                    }

                case BranchCondition.ACLOW:
                    {
                        return ACLOW;
                    }

                case BranchCondition.CIL:
                    {
                        return CIL;
                    }

                case BranchCondition.C:
                    {
                        return (((UInt16)(FLAG & Mask.CARRY) >> FieldsShiftPosition.CARRY) == 1);
                    }

                case BranchCondition.Z:
                    {
                        return (((UInt16)(FLAG & Mask.ZERO) >> FieldsShiftPosition.ZERO) == 1);
                    }

                case BranchCondition.S:
                    {
                        return (((UInt16)(FLAG & Mask.SIGN) >> FieldsShiftPosition.SIGN) == 1);
                    }

                case BranchCondition.V:
                    {
                        return (((UInt16)(FLAG & Mask.OVERFLOW) >> FieldsShiftPosition.OVERFLOW) == 1);
                    }

                default:
                    {
                        throw new ArgumentException("Undefined condition.");
                    }
            }

        }

        public void SetRegistruFLAG(ref UInt16 flag, UInt16 posToShift, bool value)
        {
            flag = (value) ? (UInt16)(flag | (1 << posToShift)) : (UInt16)(flag & ~(1 << posToShift)); //if value is true set 1 to position of FLAG (C,V,Z,S) else set 0 to position of FLAG (C,V,Z,S)
            //example if value is true for Carry it will put 1 to Carry acording to Shift position set to reach the Carry Flag in FLAG Register
        }
    }
}
