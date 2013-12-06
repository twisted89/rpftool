using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;
using RPFLib.Common;

namespace RPFLib.Resources
{
    public class xtd
    {
        /*
        public XTDHeader header;
        public int[] textureHashes;
        public List<TextureInfo> Textures;

        public xtd(string FilePath)
            : base(FilePath)
        { }

        public xtd(byte[] fileData)
            : base(fileData)
        { }

        public void Read()
        {
            // Read our base info first to do the decompression and such
            readHeader();
            decompressData();
            // Read our header
            data.BaseStream.Position = 0;
            header = new XTDHeader(data);

            // Read our texture hashes
            textureHashes = new int[header.TextureCount];
            data.BaseStream.Position = header.HashTableOffset;
            for (int i = 0; i < header.TextureCount; i++)
                textureHashes[i] = data.ReadInt32();

            // Read our info offsets
            int[] infoOffsets = new int[header.TextureCount];
            data.BaseStream.Position = header.TextureListOffset;
            for (int i = 0; i < header.TextureCount; i++)
                infoOffsets[i] = Utilities.GetOffset(data.ReadInt32());

            // Read our texture info
            Textures = new List<TextureInfo>();
            for (int i = 0; i < header.TextureCount; i++)
            {
                data.BaseStream.Position = infoOffsets[i];
                Textures.Add(new TextureInfo(data));
            }
        }

        public struct XTDHeader
        {
            public int VTable;
            public int BlockMapOffset; // Offset to the block map
            public int ParentDictionary;
            public int UsageCount;
            public int HashTableOffset; // Offset to the hash table
            public short TextureCount; // Number of textures in this dictionary
            public short TextureCount2; // Repeated texture count
            public int TextureListOffset; // Offset to the texture list
            public short TextureCount3; // Repeated texture count
            public short TextureCount4; // Repeated texture count

            public XTDHeader(BigEndianBinaryReader er)
            {
                // rage::datBase            
                VTable = er.ReadInt32();

                // rage::pgBase
                BlockMapOffset = Utilities.GetOffset(er.ReadInt32());
                ParentDictionary = er.ReadInt32();
                UsageCount = er.ReadInt32();

                // CSimpleCollection<DWORD>
                HashTableOffset = Utilities.GetOffset(er.ReadInt32());
                TextureCount = er.ReadInt16();
                TextureCount2 = er.ReadInt16();

                // CPtrCollection<T>
                TextureListOffset = Utilities.GetOffset(er.ReadInt32());
                TextureCount3 = er.ReadInt16();
                TextureCount4 = er.ReadInt16();
            }
        }

        public struct TextureInfo
        {
            public int VTable;
            public int BlockMapOffset; // 0x00000000
            public int Unknown1; // 0x00000001
            public int Unknown2; // 0x00000000
            public int Unknown3; // 0x00000000
            public int NameOffset; // Null terminated texture name
            public int TextureInfoExOffset; // Offset to extra texture info
            public short Width; // Width of texture
            public short Height; // Height of texture
            public int Unknown4; // 0x00000001 Maybe number of levels??
            public float UnknownFloat1;  // 1.0f
            public float UnknownFloat2; // 1.0f
            public float UnknownFloat3;  // 1.0f
            public float UnknownFloat4;  // 0
            public float UnknownFloat5;  // 0
            public float UnknownFloat6;  // 0
            public byte[] Padding; // 4 bytes of 0xCD padding

            public string TextureName;
            public TextureInfoEx TextureInfoEx;
            public int dataSize;
            public int vHeight;
            public int vWidth;

            public TextureInfo(BigEndianBinaryReader er)
            {
                VTable = er.ReadInt32();
                BlockMapOffset = er.ReadInt32();
                Unknown1 = er.ReadInt32();
                Unknown2 = er.ReadInt32();
                Unknown3 = er.ReadInt32();
                NameOffset = Utilities.GetOffset(er.ReadInt32());
                TextureInfoExOffset = Utilities.GetOffset(er.ReadInt32());
                Width = er.ReadInt16();
                Height = er.ReadInt16();
                Unknown4 = er.ReadInt32();
                UnknownFloat1 = er.ReadSingle();
                UnknownFloat2 = er.ReadSingle();
                UnknownFloat3 = er.ReadSingle();
                UnknownFloat4 = er.ReadSingle();
                UnknownFloat5 = er.ReadSingle();
                UnknownFloat6 = er.ReadSingle();
                Padding = er.ReadBytes(4);

                //Move to the texture name and read it
                er.BaseStream.Position = NameOffset;
                TextureName = er.ReadNullTerminatedString();

                //Move to the extra info and read it
                er.BaseStream.Position = TextureInfoExOffset;
                TextureInfoEx = new TextureInfoEx(er);

                //Get size of the data
                vWidth = Utilities.getVirtualSize(Width);
                vHeight = Utilities.getVirtualSize(Height);
                switch (TextureInfoEx.type)
                {
                    case TextureType.DXT1:
                        dataSize = (vWidth * vHeight / 2);
                        break;
                    case TextureType.DXT3:
                    case TextureType.DXT5:
                        dataSize = (vWidth * vHeight);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            public Image GeneratePreview(BigEndianBinaryReader er)
            {
                // Seek to the data offset and read it
                er.BaseStream.Position = TextureInfoEx.GpuTextureDataOffset;
                byte[] data = er.ReadBytes(dataSize);

                // Convert it to ao linear texture
                data = DXTDecoder.ConvertToLinearTexture(data,
                    (int)vWidth, (int)vHeight, TextureInfoEx.type);

                // Decode the dxt
                switch (TextureInfoEx.type)
                {
                    case TextureType.DXT1:
                        data = DXTDecoder.DecodeDXT1(data, (int)vWidth, (int)vHeight);
                        break;
                    case TextureType.DXT3:
                        data = DXTDecoder.DecodeDXT3(data, (int)vWidth, (int)vHeight);
                        break;
                    case TextureType.DXT5:
                        data = DXTDecoder.DecodeDXT5(data, (int)vWidth, (int)vHeight);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                // Now lets create a bitmap from it
                Bitmap bmp = new Bitmap((int)vWidth, (int)vHeight, PixelFormat.Format32bppArgb);
                Rectangle rect = new Rectangle(0, 0, (int)vWidth, (int)vHeight);
                BitmapData bmpdata = bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                Marshal.Copy(data, 0, bmpdata.Scan0, (int)vWidth * (int)vHeight * 4);
                bmp.UnlockBits(bmpdata);

                // Return our new bitmap
                return bmp;
            }

            public void SaveDDS(BigEndianBinaryReader er, string FilePath)
            {
                // Create a new dds
                FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
                BinaryWriter bw = new BinaryWriter(fs);

                // Write the dds header
                bw.Write(new byte[] { 0x44, 0x44, 0x53, 0x20, 0x7C, 0x00, 0x00, 0x00, 
                    0x07, 0x10, 0x08, 0x00 });

                // Write the height
                bw.Write((int)Height);

                // Write the width
                bw.Write((int)Width);

                // Write the pitch
                bw.Write((int)(Width * 4));

                // Write more header info
                bw.Write(new byte[] { */ /*0x00, 0x00, 0x04, 0x00,*/  /*0x00, 0x00, 0x00, 0x00, 
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                    0x00, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 
                    0x00});

                // Write the dxt type
                switch (TextureInfoEx.type)
                {
                    case TextureType.DXT1:
                        bw.Write(0x31545844);
                        break;
                    case TextureType.DXT3:
                        bw.Write(0x33545844);
                        break;
                    case TextureType.DXT5:
                        bw.Write(0x35545844);
                        break;
                }

                // Write the remainder of the header
                bw.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                    0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00});

                // Seek to the data offset and read it
                er.BaseStream.Position = TextureInfoEx.GpuTextureDataOffset;
                byte[] data = er.ReadBytes(dataSize);

                // Untile the data
                data = DXTDecoder.ConvertToLinearTexture(data, Width, Height, TextureInfoEx.type);

                // Now endian swap all the data
                for (int x = 0; x < dataSize; x += 2)
                    Array.Reverse(data, x, 2);

                // Now lets write it out
                bw.Write(data);

                // Close our new dds
                bw.Close();
            }

            public void InjectDDS(BigEndianBinaryWriter ew, string FilePath)
            {
                // First lets open up our dds
                FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);

                // Make sure the data isnt larger
                if (fs.Length - 0x80 > dataSize)
                {
                    System.Windows.Forms.MessageBox.Show("Cant inject because dds is larger!");
                    fs.Close();
                    return;
                }

                // Move past the dds header
                fs.Position = 0x80;

                // Read all the data
                byte[] data = br.ReadBytes(dataSize);

                // Now endian swap all the data
                for (int x = 0; x < dataSize; x += 2)
                    Array.Reverse(data, x, 2);

                // Tile the data
                data = DXTDecoder.ConvertFromLinearTexture(data, Width, Height,
                    TextureInfoEx.type);

                // Now seek to the position of the gpu data and write it
                ew.BaseStream.Position = TextureInfoEx.GpuTextureDataOffset;
                ew.Write(data);
            }

            public override string ToString() { return TextureName; }

            public string GetName()
            {
                string[] split = TextureName.Split('/');
                return split[split.Length - 1];
            }

        }

        public struct TextureInfoEx
        {
            public short Unknown1; // 0x0020
            public short Unknown2; // 0x0003
            public int Unknown3; // 0x00000001
            public int Unknown4; // 0x00000000
            public int Unknown5; // 0x00000000
            public int Unknown6; // 0x00000000
            public int Unknown7; // 0xFFFF0000
            public int Unknown8; // 0xFFFF0000
            public int Unknown9; // 0x84000002
            public int GpuTextureDataOffset; // 0x60000054, need to remove the upper and lower 8 bits
            public int Unknown10; // 0x003FE1FF
            public int Unknown11; // 0x00000D10
            public int Unknown12; // 0x00000000
            public int Unknown13; // 0x00000200
            public byte[] Padding; // 12 bytes of 0xCD padding

            public TextureType type;

            public TextureInfoEx(BigEndianBinaryReader er)
            {
                Unknown1 = er.ReadInt16();
                Unknown2 = er.ReadInt16();
                Unknown3 = er.ReadInt32();
                Unknown4 = er.ReadInt32();
                Unknown5 = er.ReadInt32();
                Unknown6 = er.ReadInt32();
                Unknown7 = er.ReadInt32();
                Unknown8 = er.ReadInt32();
                Unknown9 = er.ReadInt32();
                int temp = er.ReadInt32();
                GpuTextureDataOffset = Utilities.GetDataOffset(temp);
                type = (TextureType)(temp & 0xFF);
                Unknown10 = er.ReadInt32();
                Unknown11 = er.ReadInt32();
                Unknown12 = er.ReadInt32();
                Unknown13 = er.ReadInt32();
                Padding = er.ReadBytes(12);
            }
        }
        */

    }
}