using System.IO;
using RPFLib.Common;

namespace RPFLib.RPF7
{

    internal class Header
    {
        public Header(File file)
        {
            File = file;
        }

        public HeaderIDs Identifier { get; set; }
        public int TOCSize { get; set; }
        public int EntryCount { get; set; }
        public int nameShift { get; set; }
        public int namesLength { get; set; }

        private int Unknown1 { get; set; }
        private int EncryptedFlag { get; set; }

        public File File { get; private set; }

        public bool Encrypted
        {
            get { return EncryptedFlag != 0; }
            set { EncryptedFlag = value ? -1 : 0; }
        }

        public void Read(BigEndianBinaryReader br)
        {
            Identifier = (HeaderIDs)br.ReadInt32();
            EntryCount = br.ReadInt32();
            TOCSize = EntryCount * 16;

            BitsStream stream = new BitsStream(br.BaseStream);
            stream.Position = 8;
            stream.ReadBits(1);
            nameShift = (int)stream.ReadBits(3);
            namesLength = (int)stream.ReadBits(28);
            br.BaseStream.Position = 12;

            EncryptedFlag = br.ReadInt32();
        }

        public void Write(BigEndianBinaryWriter bw)
        {
            bw.Write((int)Identifier);
            bw.Write(EntryCount);
            bw.Write(Unknown1);
            bw.Write((int)-3);           // not encrypted, we won't write encrypted archives :)
        }
    }
}