using System;
using System.IO;
using System.Windows.Forms;
//using RageLib.Common.Resources;
using RPFLib.Common;
using ICSharpCode.SharpZipLib.Zip;

namespace RPFLib.RPF7
{
    internal class FileEntry : TOCEntry
    {
        public FileEntry(TOC toc)
        {
            TOC = toc;
        }

        public int Size; // (ala uncompressed size)
        public Int64 Offset;
        public int SizeInArchive;
        public bool IsCompressed;
        public bool IsEncrypted;
        public bool IsResourceFile;
        public int customsize;
        public byte ResourceType;

        public byte[] CustomData { get; private set; }
        public override int newEntryIndex { get; set; }

        public int getSize()
        {
            return Size;
        }

        public void setIndex(int index)
        {
            newEntryIndex = index; ;
        }

        public void SetCustomData(byte[] data)
        {
            try
            {
                if (data == null)
                {
                    CustomData = null;
                }
                else
                {
                    customsize = data.Length;

                    if (IsCompressed)
                    {
                        data = DataUtil.Compress(data, ICSharpCode.SharpZipLib.Zip.Compression.Deflater.BEST_COMPRESSION);
                    }
                    CustomData = data;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public override bool IsDirectory
        {
            get { return false; }
        }

        public override void Read(BitsStream stream)
        {
            try
            {
                IsResourceFile = stream.ReadBool();
                Offset = (long)stream.ReadBits(23);
                SizeInArchive = (int)stream.ReadBits(24);
                NameOffset = (int)stream.ReadBits(16);

                Offset <<= 9;

                if (IsResourceFile)
                {
                    if (Size == 0xFFFFFF)
                    {
                        throw new Exception("Resource with size -1, not supported");
                    }
                    uint systemFlag = (uint)stream.ReadInt();
                    uint graphicsFlag = (uint)stream.ReadInt();

                    IsCompressed = false;
                    IsEncrypted = false;
                    Size = SizeInArchive;
                }
                else
                {
                    Size = stream.ReadInt();
                    IsEncrypted = stream.ReadInt() == 1;
                    IsCompressed = Size != 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public override void Write(BigEndianBinaryWriter bw)
        {
        }
    }
}