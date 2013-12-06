using System.IO;
using RPFLib.Common;

namespace RPFLib.RPF3
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

        private int Unknown1 { get; set; }
        private int EncryptedFlag { get; set; }

        public File File { get; private set; }

        public bool Encrypted
        {
            get { return EncryptedFlag != 0; }
            set { EncryptedFlag = value ? -1 : 0; }
        }

        public void Read(BinaryReader br)
        {
            Identifier = (HeaderIDs)br.ReadInt32();
            TOCSize = br.ReadInt32();
            EntryCount = br.ReadInt32();
            Unknown1 = br.ReadInt32();
            EncryptedFlag = br.ReadInt32();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write((int)Identifier);
            bw.Write(TOCSize);
            bw.Write(EntryCount);
            bw.Write(Unknown1);
            bw.Write((int)-1);           // not encrypted, we won't write encrypted archives :)
        }
    }
}