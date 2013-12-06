using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using RPFLib.Common;
using System.Runtime.ExceptionServices;
using System.Windows.Forms;
namespace RPFLib.Resources
{
    public class RSCFile
    {
        #region Vars
        private Stream xscStream;
        private RSCHeader hdr;
        public byte[] fileData;
        #endregion

        #region Contructor
        public RSCFile(byte[] fileData)
            : this(new MemoryStream(fileData, false)) { }
        public RSCFile(Stream input)
        {
            this.xscStream = input;
            using (BigEndianBinaryReader reader = new BigEndianBinaryReader(this.xscStream))
            {
                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\xcompress32.dll";
                if (!System.IO.File.Exists(path))
                {
                    MessageBox.Show("xcompress32.dll not found the RPFTool directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    fileData = null;
                    return;
                }
                hdr.ReadHeader(reader);
                byte[] buffer;
                switch (hdr.m_dwMagic)
                {
                    case -2059185326:
                        if (hdr.m_dwVersion == 2)
                        {
                            reader.BaseStream.Position = 16;
                            buffer = reader.ReadBytes((int)reader.BaseStream.Length - 16);
                            buffer = DataUtil.Decrypt(buffer);
                            using (BigEndianBinaryReader readerFile = new BigEndianBinaryReader(new MemoryStream(buffer)))
                            {
                                fileData = new byte[hdr.getSizeV() + hdr.getSizeP()];
                                readerFile.BaseStream.Position = 8;
                                if (Decompress(readerFile.ReadBytes((int)readerFile.BaseStream.Length), fileData) == -1)
                                {
                                    MessageBox.Show("Failed to decompress file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    fileData = null;
                                }
                            }
                        }
                        else
                        {
                            reader.BaseStream.Position = 16;
                            buffer = reader.ReadBytes((int)reader.BaseStream.Length - 16);
                            using (BigEndianBinaryReader readerFile = new BigEndianBinaryReader(new MemoryStream(buffer)))
                            {
                                fileData = new byte[hdr.getSizeV() + hdr.getSizeP()];
                                readerFile.BaseStream.Position = 8;
                                if (Decompress(readerFile.ReadBytes((int)readerFile.BaseStream.Length), fileData) == -1)
                                {
                                    MessageBox.Show("Failed to decompress file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    fileData = null;
                                }
                            }
                        }
                        break;
                    case 88298322:
                        reader.BaseStream.Position = 20;
                        buffer = reader.ReadBytes((int)reader.BaseStream.Length - 20);
                        fileData = new byte[hdr.getSizeV() + hdr.getSizeP()];
                        if (Decompress(buffer, fileData) == -1)
                        {
                            MessageBox.Show("Failed to decompress file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            fileData = null;
                        }
                        //System.IO.File.WriteAllBytes(@"Z:\D\Downloads\gms\RDR", fileData);
                        break;
                    default:
                        MessageBox.Show("Unrecognised header", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        fileData = null;
                        break;

                }
            }
        }
        #endregion

        #region HeaderStruct
        public struct RSCHeader
        {
            //Header
            public int m_dwMagic;
            public int m_dwVersion;
            public int m_dwFlags1;
            public int m_dwFlags2;
            //RSC flags
            public int dwExtVSize; //: 14;
            public int dwExtPSize; //: 14;
            public int _f14_30;    //: 3; 
            public bool bUseExtSize; //: 1;

            public bool ReadHeader(BigEndianBinaryReader reader)
            {
                try
                {
                    this.m_dwMagic = reader.ReadInt32();
                    switch (m_dwMagic)
                    {
                        case -2059185326:
                            this.m_dwVersion = reader.ReadInt32();
                            this.m_dwFlags1 = reader.ReadInt32();
                            this.m_dwFlags2 = reader.ReadInt32();
                            dwExtVSize = (int)(m_dwFlags2 & 0x7FFF);
                            dwExtPSize = (int)((m_dwFlags2 & 0xFFF7000) >> 14);
                            _f14_30 = (int)(m_dwFlags2 & 0x70000000);
                            bUseExtSize = (m_dwFlags2 & 0x80000000) == 0x80000000 ? true : false;
                            return true;
                        case 88298322:
                            this.m_dwVersion = reader.ReadInt32();
                            this.m_dwFlags1 = reader.ReadInt32();
                            //dwExtVSize = (int)(m_dwFlags2 & 0x7FFF);
                            //dwExtPSize = (int)((m_dwFlags2 & 0xFFF7000) >> 14);
                            //_f14_30 = (int)(m_dwFlags2 & 0x70000000);
                            bUseExtSize = false;
                            return true;
                        default:
                            return false;
                    }
                }
                catch (Exception ex) { return false; }
            }

            public int getSizeV()
            {
                return bUseExtSize ? (dwExtVSize << 12) : ((int)(m_dwFlags1 & 0x7FF) << ((int)((m_dwFlags1 >> 11) & 15) + 8));
            }

            public int getSizeP()
            {
                return bUseExtSize ? (dwExtPSize << 12) : ((int)((m_dwFlags1 >> 15) & 0x7FF) << ((int)((m_dwFlags1 >> 26) & 15) + 8));
            }

            public int getObjectStart()
            {
                // get offset of the main object in the resource
                return (bUseExtSize && _f14_30 < 4) ? ((dwExtVSize >> (_f14_30 + 1)) << (_f14_30 + 13)) : 0;
            }

            public void Dispose() { GC.SuppressFinalize(this); }
        }
        #endregion

        #region De/Compression
        [HandleProcessCorruptedStateExceptionsAttribute]
        int Decompress(byte[] compressedData, byte[] decompressedData)
        {
            // Setup our decompression context
            int DecompressionContext = 0;
            int hr = xcompress.XMemCreateDecompressionContext(
                xcompress.XMEMCODEC_TYPE.XMEMCODEC_LZX,
                0, 0, ref DecompressionContext);

            // Now lets decompress
            int compressedLen = compressedData.Length;
            int decompressedLen = decompressedData.Length;
            try
            {
                hr = xcompress.XMemDecompress(DecompressionContext,
                    decompressedData, ref decompressedLen,
                    compressedData, compressedLen);
            }
            catch
            {
                return -1;
            }

            // Go ahead and destroy our context
            xcompress.XMemDestroyDecompressionContext(DecompressionContext);

            // Return our hr
            return hr;
        }

        byte[] Compress(byte[] decompressedData)
        {
            // Setup our compression context
            int compressionContext = 0;
            int hr = xcompress.XMemCreateCompressionContext(
                xcompress.XMEMCODEC_TYPE.XMEMCODEC_LZX,
                0, 0, ref compressionContext);

            // Now lets compress
            int compressedLen = decompressedData.Length * 2;
            byte[] compressed = new byte[compressedLen];
            int decompressedLen = decompressedData.Length;
            hr = xcompress.XMemCompress(compressionContext,
                compressed, ref compressedLen,
                decompressedData, decompressedLen);

            // Go ahead and destory our context
            xcompress.XMemDestroyCompressionContext(compressionContext);

            // Resize our compressed data
            Array.Resize<byte>(ref compressed, compressedLen);

            // Now lets return it
            return compressed;
        }
        #endregion
    }
}
