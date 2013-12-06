using DevExpress.XtraEditors;
using RPFLib.Common;
using RPFLib.Resources;
using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace RPFTool.Viewers
{
    public partial class xscView : XtraForm
    {
        /// Code Data
        private byte[] data;

        // XSC Values
        private int dwClassOffset;
        private int m_pPageMap;     // +04 NOTE: pgBase field
        private int[] m_ppCodePages; // +08
        private int m_dwCodeSize;   // +0C
        private int _f10;           // +10
        private int m_dwStaticSize; // +14
        private ArrayList m_pStatic;      // +18
        private int m_dwGlobalsVersion; // +1C
        private int m_dwNativeSize; // +20
        private ArrayList m_ppNatives;    // +24
        //private byte[] m_codeArray;

        //Local vars
        private int page = 0;


        public xscView(byte[] rscdata)
        {
            InitializeComponent();
            RSCFile rsc = new RSCFile(rscdata);
            if (rsc.fileData == null)
            {
                return;
            }
            data = rsc.fileData;
        }

        public StringBuilder Decode()
        {
            StringBuilder log = new StringBuilder();
            MemoryStream ms = new MemoryStream(data);
            using (BigEndianBinaryReader reader = new BigEndianBinaryReader(ms))
            {
                try
                {
                    ////////////////////
                    //Find Header
                    ////////////////////
                    bool valid;
                    try
                    {
                        valid = false;
                        dwClassOffset = 0;
                        reader.BaseStream.Position = 0;
                        while (dwClassOffset < reader.BaseStream.Length)
                        {
                            if (reader.ReadInt32() == -1462287616) //0x43D7A8
                            {
                                valid = true;
                                break;
                            }
                            dwClassOffset += 4096;
                            reader.BaseStream.Position = dwClassOffset;
                        }
                        reader.BaseStream.Position = 0;
                    }
                    catch (System.Exception ex)
                    {
                        log.AppendLine("Error: Failed to Find Header");
                        return log;
                    }
                    log.AppendLine(string.Format("TRACE: length = {0}, valid = {1}, offset = 0x{2:X}\n", reader.BaseStream.Length, valid ? "true" : "false", dwClassOffset));
                    if (!valid)
                    {
                        log.AppendLine("Error: Invalid XSC File");
                        return log;
                    }

                    ////////////////////
                    //Read Header
                    ////////////////////
                    try
                    {
                        reader.BaseStream.Position = dwClassOffset + 4;
                        this.m_pPageMap = reader.ReadInt32();
                        int temp_codePagesOffset = (reader.ReadInt32() & 0xFFFFFF); // Just get pages offset until we have code size
                        this.m_dwCodeSize = reader.ReadInt32();
                        this._f10 = reader.ReadInt32();
                        this.m_dwStaticSize = reader.ReadInt32();
                        this.m_pStatic = getStatics(reader);
                        this.m_dwGlobalsVersion = reader.ReadInt32();
                        this.m_dwNativeSize = reader.ReadInt32();
                        this.m_ppNatives = getNatives(reader);

                        this.m_ppCodePages = getPages(reader, temp_codePagesOffset); // go back and get pages
                        for (int i = 0; i < m_ppCodePages.Length; i++)
                        {
                            log.AppendLine(string.Format("TRACE: page {0:d}: offset = {1:X}\n", i, m_ppCodePages[i]));
                        }
                        lb_natives.Items.AddRange(this.m_ppNatives.ToArray());
                        lb_locals.Items.AddRange(this.m_pStatic.ToArray());
                    }
                    catch
                    {
                        log.AppendLine("Error: Failed reading Header");
                        return log;
                    }

                    ////////////////////
                    //Read opcodes
                    ////////////////////
                    try
                    {
                        page = 0;
                        reader.BaseStream.Position = m_ppCodePages[0];
                        StringBuilder script = new StringBuilder();
                        for (int i = 0; i < m_dwCodeSize; i = disassembleCommand(reader, i, script)) ;
                        codeBox.Text = script.ToString();
                    }
                    catch
                    {
                        log.AppendLine("Error: Failed reading opcodes");
                        return log;
                    }
                }
                catch
                {
                    log.AppendLine("Error: Failed reading XSC file");
                    return log;
                }
                return log;
            }
        }

        #region opcodes
        private struct opcode
        {
            public string Name;
            public int Length;

            public opcode(string id, int val)
            {
                Name = id;
                Length = val;
            }
        }
        opcode[] ops = new opcode[] 
        {
            new opcode( "nop", 1      ),  // 0 - skip command, switch page if necessary
            new opcode( "iadd", 1     ),  // 1 - integer +, stack operands
            new opcode( "isub", 1     ),  // 2 - integer -, stack operandes
            new opcode( "imul", 1     ),  // 3 - integer *, stack operands
            new opcode( "idiv", 1     ),  // 4 - integer /, stack operands
            new opcode( "imod", 1     ),  // 5 - integer %, stack operands
            new opcode( "inot", 1     ),  // 6 - logical not, stack operand
            new opcode( "ineg", 1     ),  // 7 - integer negate, stack operand
            new opcode( "icmpeq", 1   ),  // 8 - integer compare =, stack operands
            new opcode( "icmpne", 1   ),  // 9 - integer compare !=, stack operands
            new opcode( "icmpgt", 1   ),  // 10 - integer compare >, stack operands
            new opcode( "icmpge", 1   ),  // 11 - integer compare >=, stack operands
            new opcode( "icmplt", 1   ),  // 12 - integer compare <, stack operands
            new opcode( "icmple", 1   ),  // 13 - integer compare <=, stack operands
            new opcode( "fadd", 1     ),  // 14 - float +, stack operands
            new opcode( "fsub", 1     ),  // 15
            new opcode( "fmul", 1     ),  // 16
            new opcode( "fdiv", 1     ),  // 17
            new opcode( "fmod", 1     ),  // 18
            new opcode( "fneg", 1     ),  // 19
            new opcode( "fcmpeq", 1   ),  // 20
            new opcode( "fcmpne", 1   ),  // 21
            new opcode( "fcmpgt", 1   ),  // 22
            new opcode( "fcmpge", 1   ),  // 23
            new opcode( "fcmplt", 1   ),  // 24
            new opcode( "fcmple", 1   ),  // 25
            new opcode( "vadd", 1     ),  // 26 - vector +, stack operands
            new opcode( "vsub", 1     ),  // 27
            new opcode( "vmul", 1     ),  // 28
            new opcode( "vdiv", 1     ),  // 29
            new opcode( "vneg", 1     ),  // 30
            new opcode( "iand", 1     ),  // 31 - integer &, stack operands
            new opcode( "ior", 1      ),  // 32
            new opcode( "ixor", 1     ),  // 33
            new opcode( "itof", 1     ),  // 34
            new opcode( "ftoi", 1     ),  // 35
            new opcode( "dup2", 1     ),  // 36
            new opcode( "ipush1", 2   ),  // 37   ipush imm1
            new opcode( "ipush12", 3  ),  // 38   ipush imm1, imm1
            new opcode( "ipush13", 4  ),  // 39   ipush imm1, imm1, imm1
            new opcode( "ipush", 5    ),  // 40
            new opcode( "fpush", 5    ),  // 41
            new opcode( "dup", 1      ),  // 42
            new opcode( "drop", 1     ),  // 43
            new opcode( "native", 3   ),  // 44
            new opcode( "enter", 0    ),  // 45   - 5 + last byte 
            new opcode( "ret", 3      ),  // 46 
            new opcode( "pget", 1     ),  // 47
            new opcode( "pset", 1     ),  // 48
            new opcode( "ppeekset", 1 ),  // 49
            new opcode( "tostack", 1  ),  // 50
            new opcode( "fromstack", 1),  // 51
            new opcode( "parray", 2   ),  // 52
            new opcode( "aget",2      ),  // 53
            new opcode( "aset",2      ),  // 54
            new opcode( "pframe1",  2 ),  // 55
            new opcode( "getf",   2   ),  // 56 - get value from the stack frame, immediate 1-byte offset
            new opcode( "setf",   2   ),  // 57 - set value to the stack frame, immediate 1-byte offset
            new opcode( "stackgetp",2 ),  // 58
            new opcode( "stackget", 2 ),  // 59
            new opcode( "stackset", 2 ),  // 60
            new opcode( "iaddimm1", 2 ),  // 61
            new opcode( "pgetimm1", 2 ),  // 62
            new opcode( "psetimm1", 2 ),  // 63
            new opcode( "imulimm1", 2 ),  // 64
            new opcode( "ipush2", 3   ),  // 65
            new opcode( "iaddimm2", 3 ),  // 66
            new opcode( "pgetimm2", 3 ),  // 67
            new opcode( "psetimm2", 3 ),  // 68
            new opcode( "imulimm2", 3 ),  // 69
            new opcode( "arraygetp2", 3), // 70
            new opcode( "arrayget2", 3),  // 71
            new opcode( "arrayset2", 3),  // 72
            new opcode( "pframe2", 3  ),  // 73
            new opcode( "frameget2", 3),  // 74
            new opcode( "frameset2", 3),  // 75
            new opcode( "pstatic2", 3 ),  // 76
            new opcode( "staticget2", 3), // 77
            new opcode( "staticset2", 3), // 78
            new opcode( "pglobal2", 3 ),  // 79
            new opcode( "globalget2",3),  // 80
            new opcode( "globalset2",3),  // 81
            new opcode( "call2", 3    ),  // 82   call imm2
            new opcode( "call2h1", 3  ),  // 83   call (imm2|0x1000)
            new opcode( "call2h2", 3  ),  // 84   call (imm2|0x2000)
            new opcode( "call2h3", 3  ),  // 85   call (imm2|0x3000)
            new opcode( "call2h4", 3  ),  // 86   call (imm2|0x4000)
            new opcode( "call2h5", 3  ),  // 87   call (imm2|0x5000)
            new opcode( "call2h6", 3  ),  // 88   call (imm2|0x6000)
            new opcode( "call2h7", 3  ),  // 89   call (imm2|0x7000)
            new opcode( "call2h8", 3  ),  // 90   call (imm2|0x8000)
            new opcode( "call2h9", 3  ),  // 91   call (imm2|0x9000)
            new opcode( "call2ha", 3  ),  // 92   call (imm2|0xA000)
            new opcode( "call2hb", 3  ),  // 93   call (imm2|0xB000)
            new opcode( "call2hc", 3  ),  // 94   call (imm2|0xC000)
            new opcode( "call2hd", 3  ),  // 95   call (imm2|0xD000)
            new opcode( "call2he", 3  ),  // 96   call (imm2|0xE000)
            new opcode( "call2hf", 3  ),  // 97   call (imm2|0xF000)
            new opcode( "jr2", 3      ),  // 98   jump relative signed imm2
            new opcode( "jfr2", 3     ),  // 99   jump if false relative signed imm2
            new opcode( "jner2", 3    ),  // 100
            new opcode( "jeqr2", 3    ),  // 101
            new opcode( "jler2", 3    ),  // 102  TODO: <= or > ?
            new opcode( "jltr2", 3    ),  // 103  TODO: < or >= ?
            new opcode( "jger2", 3    ),  // 104  TODO: >= or < ?
            new opcode( "jgtr2", 3    ),  // 105  TODO: > or <= ?
            new opcode( "pglobal3", 4 ),  // 106
            new opcode( "globalget3",4),  // 107
            new opcode( "globalset3",4),  // 108
            new opcode( "ipush3", 4   ),  // 109
            new opcode( "switchr2", 0 ),  // 110  length = 2 + byte[1]*6
            new opcode( "spush", 0    ),  // 111  length = 2 + byte[1]
            new opcode( "spushl", 0   ),  // 112  length = 5 + dword[1]
            new opcode( "spush0", 1   ),  // 113  push ""
            new opcode( "scpy", 2     ),  // 114
            new opcode( "itos", 2     ),  // 115
            new opcode( "sadd", 2     ),  // 116
            new opcode( "saddi", 2    ),  // 117
            new opcode( "sncpy", 1    ),  // 118
            new opcode( "catch", 1    ),  // 119
            new opcode( "throw", 1    ),  // 120
            new opcode( "pcall", 1    ),  // 121
            new opcode( "ret0r0", 1   ),  // 122 - return: 0 parameters, 0 results
            new opcode( "ret0r1", 1   ),  // 123 - return: 0 parameters, 1 result 
            new opcode( "ret0r2", 1   ),  // 124 - return: 0 parameters, 2 results
            new opcode( "ret0r3", 1   ),  // 125 - return: 0 parameters, 3 results
            new opcode( "ret1r0", 1   ),  // 126 - return: 1 parameter, 0 results
            new opcode( "ret1r1", 1   ),  // 127 - return: 1 parameter, 1 result
            new opcode( "ret1r2", 1   ),  // 128 - return: 1 parameter, 2 results
            new opcode( "ret1r3", 1   ),  // 129 - return: 1 parameter, 3 results
            new opcode( "ret2r0", 1   ),  // 130 - return: 2 parameters, 0 results 
            new opcode( "ret2r1", 1   ),  // 131 - return: 2 parameters, 1 result
            new opcode( "ret2r2", 1   ),  // 132 - return: 2 parameters, 2 results 
            new opcode( "ret2r3", 1   ),  // 133 - return: 2 parameters, 3 results 
            new opcode( "ret3r0", 1    ),  // 134 - return: 3 parameters, 0 results 
            new opcode( "ret3r1", 1    ),  // 135 - return: 3 parameters, 1 result
            new opcode( "ret3r2", 1    ),  // 136 - return: 3 parameters, 2 results 
            new opcode( "ret3r3", 1    ),  // 137 - return: 3 parameters, 3 results 
            new opcode( "iimmn1",1 ), // 138  push -1
            new opcode( "iimm0", 1 ), // 139  push 0
            new opcode( "iimm1", 1 ), // 140  push 1
            new opcode( "iimm2", 1 ), // 141  push 2
            new opcode( "iimm3", 1 ), // 142  push 3
            new opcode( "iimm4", 1 ), // 143  push 4
            new opcode( "iimm5", 1 ), // 144  push 5
            new opcode( "iimm6", 1 ), // 145  push 6
            new opcode( "iimm7", 1 ), // 146  push 7
            new opcode( "fimmn1", 1), // 147  push -1.0f
            new opcode( "fimm0", 1),  // 148  push 0.0f
            new opcode( "fimm1", 1),  // 149  push 1.0f
            new opcode( "fimm2", 1),  // 150  push 2.0f
            new opcode( "fimm3", 1),  // 151  push 3.0f
            new opcode( "fimm4", 1),  // 152  push 4.0f
            new opcode( "fimm5", 1),  // 153  push 5.0f
            new opcode( "fimm6", 1),  // 154  push 6.0f
            new opcode( "fimm7", 1)   // 155  push 7.0f
        };
        #endregion

        [StructLayout(LayoutKind.Explicit)]
        struct scrArg
        {
            [FieldOffset(0)]
            public byte bValue0;

            [FieldOffset(1)]
            public byte bValue1;

            [FieldOffset(2)]
            public byte bValue2;

            [FieldOffset(3)]
            public byte bValue3;

            [FieldOffset(0)]
            public float fValue;

            [FieldOffset(0)]
            public int nValue;

            [FieldOffset(0)]
            public uint dwValue;

            [FieldOffset(0)]
            public short sValue;

            [FieldOffset(0)]
            public ushort wValue;

        }

        #region Dissasembler
        private int disassembleCommand(BigEndianBinaryReader reader, int dwAddress, StringBuilder log)
        {
            if (dwAddress > m_dwCodeSize) // check we haven't gone over our code size
                return 0x7FFFFFFF;
            if ((dwAddress >> 14) > page) //Switch pages if needed
            {
                page++;
                reader.BaseStream.Position = m_ppCodePages[page];
            }

            byte op = reader.ReadByte();
            if (op > 155)
            {
                log.Append(string.Format("ERROR: invalid opcode {0:d}", op));
                return 0x7FFFFFFF;
            }

            if (op == 45)  // enter
                log.Append(string.Format("{0:X6}: === sub_{0:X6} ==========\n", dwAddress));
            //log.Append(string.Format("{0:X6}:\t{1:d3}\t{2:s}\t", dwAddress, op, ops[op].Name));
            log.Append(string.Format("{0:s}\t",ops[op].Name));
            int l = ops[op].Length;


            // prepare opcode parameter(s)
            scrArg arg;
            // set defaults to keep c# compiler quiet
            arg.bValue3 = 0;
            arg.bValue2 = 0;
            arg.bValue1 = 0;
            arg.bValue0 = 0;
            arg.dwValue = 0;
            arg.fValue = 0;
            arg.nValue = 0;
            arg.sValue = 0;
            arg.wValue = 0;
            switch (l)
            {
                case 5: // usually DWORD or float
                    arg.bValue3 = reader.ReadByte();
                    arg.bValue2 = reader.ReadByte();
                    arg.bValue1 = reader.ReadByte();
                    arg.bValue0 = reader.ReadByte();
                    break;
                case 4: // 3-byte int or 3 1-byte ints
                    arg.bValue3 = 0;
                    arg.bValue2 = reader.ReadByte();
                    arg.bValue1 = reader.ReadByte();
                    arg.bValue0 = reader.ReadByte();
                    break;
                case 3: // short or two 1-byte ints
                    arg.bValue3 = 0;
                    arg.bValue2 = 0;
                    arg.bValue1 = reader.ReadByte();
                    arg.bValue0 = reader.ReadByte();
                    break;
                case 2: // BYTE
                    arg.dwValue = 0;
                    arg.bValue0 = reader.ReadByte();
                    break;
            }

            // print 
            switch (op)
            {
                case 37:    // ipush1
                    log.Append(string.Format("ipush\t{0:d}", arg.bValue0));
                    break;
                case 38:
                    log.Append(string.Format("ipush\t{0:d}, {1:d}", arg.bValue1, arg.bValue0));
                    break;
                case 39:
                    log.Append(string.Format("ipush\t{0:d}, {1:d}, {2:d}", arg.bValue2, arg.bValue1, arg.bValue0));
                    break;
                case 40:    // ipush
                    log.Append(string.Format("ipush\t{0:d}", arg.nValue));
                    break;
                case 41:    // fpush
                    log.Append(string.Format("fpush\t{0:f}", arg.fValue));
                    break;
                case 44:
                    {
                        int index = ((arg.bValue1 << 2) & 0x300) | arg.bValue0;
                        log.Append(string.Format("native\t{0:X4} => {1:d}:${2:X8}, ({3:d}):{4:d}", arg.wValue, index, index < m_dwNativeSize ? m_ppNatives[index] : 0, (arg.bValue1 & 0x3E) >> 1, arg.bValue1 & 1));    // TODO: get hash from .native seg
                    }
                    break;
                case 45:
                    arg.bValue3 = reader.ReadByte();
                    arg.bValue2 = reader.ReadByte();
                    arg.bValue1 = reader.ReadByte();
                    arg.bValue0 = reader.ReadByte();
                    log.Append(string.Format("enter\t{0:d} 0x{1:X} {2:d} ", arg.bValue3, (arg.bValue2 << 8) | arg.bValue1, arg.bValue0));
                    if (arg.bValue0 > 0)
                    {
                        // printf (" WARNING: 'enter' last parameter != 0 " );
                        for (int i = 1; i < arg.bValue0; i++)
                            log.Append(string.Format("{0:d}", reader.ReadByte()));
                        //putc(dwPage[dwAddressLow + 5 + i], stdout);
                    }
                    l = 5 + arg.bValue0;
                    break;
                case 46:
                    log.Append(string.Format("ret  \t{0:d} {1:d}", arg.bValue1, arg.bValue0));
                    break;
                case 52:    // parray
                case 53:    // arrayget1
                case 54:    // arrayset1
                case 55:    // pframe1
                case 56:    // frameget
                case 57:    // frameset
                case 58:    // stackgetp
                case 59:    // stackget
                case 60:    // stackset
                case 61:    // iaddimm1
                case 62:    // pgetimm1
                case 63:    // psetimm1
                case 64:    // imulimm1
                    log.Append(string.Format("{0:d}", arg.bValue0));
                    break;
                case 65:    // ipush2
                case 66:    // iaddimm2
                    log.Append(string.Format("{0:d}", arg.wValue));
                    break;
                case 67:    // pgetimm2
                case 68:    // psetimm2
                    log.Append(string.Format("0x{0:X}", arg.wValue));
                    break;
                case 69:    // imilimm2
                    log.Append(string.Format("{0:d}", arg.wValue));
                    break;
                case 70:    // arraygetp2
                case 71:    // arrayget2
                case 72:    // arrayset2
                case 73:    // pframe2
                case 74:    // frameget2
                case 75:    // frameset2
                case 76:    // pstatic2
                case 77:    // staticget2
                case 78:    // staticset2
                case 79:    // pglobal2
                case 80:    // globalget2
                case 81:    // globalset2
                    log.Append(string.Format("0x{0:X}", arg.wValue));
                    break;
                case 82:    // call2h0
                case 83:
                case 84:
                case 85:
                case 86:
                case 87:
                case 88:
                case 89:
                case 90:
                case 91:
                case 92:
                case 93:
                case 94:
                case 95:
                case 96:
                case 97:    // call2hf
                    log.Append(string.Format("call \t#{0:X}", ((op - 82) << 16) | arg.wValue));
                    break;
                case 98:    // jr2
                case 99:    // jfr2
                case 100:   // jner2
                case 101:   // jeqr2
                case 102:   // jler2
                case 103:   // jltr2
                case 104:   // jger2:
                case 105:   // jgtr2
                    log.Append(string.Format("{0:d} => #{0:X}", arg.sValue, dwAddress + 3 + arg.sValue));  // TODO: calculate target
                    break;
                case 106:   // pglobal3
                    log.Append(string.Format("pglobal\t{0:X}", arg.dwValue));
                    break;
                case 107:   // globalget3
                    log.Append(string.Format("getg\t{0:X}", arg.dwValue));
                    break;
                case 108:   // globalget3
                    log.Append(string.Format("setg\t{0:X}", arg.dwValue));
                    break;
                case 109:   // ipush3
                    log.Append(string.Format("ipush\t{0:d}", arg.dwValue));
                    break;
                //    { "switchr2", 0 },  // 110  length = 2 + byte[1]*6
                case 110:   // TODO: calculate targets
                    {
                        arg.bValue0 = reader.ReadByte();
                        l = 2 + arg.bValue0 * 6;
                        log.Append(string.Format("[{0:d}]: ", arg.bValue0));
                        for (int i = 0; i < arg.bValue0; i++)
                        {
                            //int off = (int)reader.BaseStream.Position + (i * 6);
                            //reader.BaseStream.Position = off;
                            int value = (reader.ReadByte() << 24) + (reader.ReadByte() << 16) + (reader.ReadByte() << 8) + reader.ReadByte();
                            int target = (reader.ReadByte() << 8) + reader.ReadByte();
                            log.Append(string.Format("<{0:d}, {1:d} => #{2:X}>", value, target, target + reader.BaseStream.Position));
                        }
                    }
                    break;
                case 111:
                    arg.bValue0 = reader.ReadByte();
                    l = arg.bValue0 + 2;
                    log.Append(string.Format("spush\t[0x{0:X}] '{1}'", arg.bValue0, readString(reader, arg.bValue0)));
                    break;
                case 112:
                    arg.bValue3 = reader.ReadByte();
                    arg.bValue2 = reader.ReadByte();
                    arg.bValue1 = reader.ReadByte();
                    arg.bValue0 = reader.ReadByte();
                    l = (int)(arg.nValue + 5);
                    log.Append(string.Format("spush\t[0x{0:X}] '{1}'", arg.dwValue, readString(reader, arg.nValue)));
                    break;
                //    { "spush0", 1   },  // 113  push ""
                case 114:   // scpy
                case 115:   // itos
                case 116:   // sadd
                case 117:   // saddi
                    log.Append(string.Format("[{0:d}]", arg.bValue0));
                    break;
                case 122:
                case 123:
                case 124:
                case 125:
                case 126:
                case 127:
                case 128:
                case 129: // ret0r0 .. ret1r3
                case 130:
                case 131:
                case 132:
                case 133:
                case 134:
                case 135:
                case 136:
                case 137: // ret2r0 .. ret3r3
                    log.Append(string.Format("ret  \t{0:d} {1:d}", (op - 122) >> 2, (op - 122) & 3));
                    break;
                case 138:
                case 139:
                case 140:
                case 141:
                case 142:
                case 143:
                case 144:
                case 145:
                case 146:   // iimmn1 .. iimm7
                    log.Append(string.Format("ipush\t{0:d}", op - 139));
                    break;
                case 147:
                case 148:
                case 149:
                case 150:
                case 151:
                case 152:
                case 153:
                case 154:
                case 155:   // fimmn1 .. fimm7
                    log.Append(string.Format("fpush\t{0:f}", op - 148.0f));
                    break;
            }
            log.Append("\n");
            return dwAddress + l;
        }

        #endregion

        #region Helpers
        private string readString(BigEndianBinaryReader reader, int length)
        {
            byte[] stringarray = new byte[length];
            for (int i = 0; i < length; i++)
            {
                stringarray[i] = reader.ReadByte();
            }
            return System.Text.Encoding.UTF8.GetString(stringarray).Substring(0, length-1);
        }

        private int readPInt(BigEndianBinaryReader reader)
        {
            long returnpos = reader.BaseStream.Position + 4;
            reader.BaseStream.Position = (reader.ReadInt32() & 0xFFFFFF);
            int val = reader.ReadInt32();
            reader.BaseStream.Position = returnpos;
            return val;
        }

        private int readPPInt(BigEndianBinaryReader reader)
        {
            long returnpos = reader.BaseStream.Position + 4;
            reader.BaseStream.Position = (reader.ReadInt32() & 0xFFFFFF);
            reader.BaseStream.Position = (reader.ReadInt32() & 0xFFFFFF);
            int val = reader.ReadInt32();
            reader.BaseStream.Position = returnpos;
            return val;
        }

        private int readPPByte(BigEndianBinaryReader reader)
        {
            long returnpos = reader.BaseStream.Position + 4;
            reader.BaseStream.Position = (reader.ReadInt32() & 0xFFFFFF);
            reader.BaseStream.Position = (reader.ReadInt32() & 0xFFFFFF);
            int val = reader.ReadByte();
            reader.BaseStream.Position = returnpos;
            return val;
        }

        private int[] getPages(BigEndianBinaryReader reader, int offset)
        {
            reader.BaseStream.Position = offset;
            int[] pages = new int[(m_dwCodeSize + (1 << 14) - 1) >> 14];
            for (int i = 0; i < pages.Length; i++)
            {
                pages[i] = reader.ReadInt32() & 0xFFFFFF;
            }
            return pages;
        }

        private ArrayList getStatics(BigEndianBinaryReader reader)
        {
            long returnpos = reader.BaseStream.Position + 4;
            reader.BaseStream.Position = (reader.ReadInt32() & 0xFFFFFF);
            ArrayList statics = new ArrayList();
            for (int i = 0; i < m_dwStaticSize; i++)
            {
                statics.Add(reader.ReadInt32());
            }
            reader.BaseStream.Position = returnpos;
            return statics;
        }

        private ArrayList getNatives(BigEndianBinaryReader reader)
        {
            long returnpos = reader.BaseStream.Position + 4;
            reader.BaseStream.Position = (reader.ReadInt32() & 0xFFFFFF);
            ArrayList Natives = new ArrayList();
            for (int i = 0; i < m_dwNativeSize; i++)
            {
                Natives.Add(reader.ReadInt32());
            }
            reader.BaseStream.Position = returnpos;
            return Natives;
        }

        private void writeToLog(string text)
        {
            tb_log.Text += string.Format("{0}{1}", text, Environment.NewLine);
        }
        #endregion

        #region Form Methods
        private void xscView_Load(object sender, EventArgs e)
        {
            if (data == null)
                this.Close();
            writeToLog(Decode().ToString());
        }
        #endregion

        #region Button Methods
        private void btn_close_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}