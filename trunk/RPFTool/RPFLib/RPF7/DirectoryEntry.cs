using System.IO;
using RPFLib.Common;
using System;

namespace RPFLib.RPF7
{
    internal class DirectoryEntry : TOCEntry
    {
        public DirectoryEntry(TOC toc)
        {
            TOC = toc;
        }

        public int Flags { get; set; }
        public int ContentEntryIndex { get; set; }
        public int ContentEntryCount { get; set; }
        public override int newEntryIndex { get; set; }

        public override bool IsDirectory
        {
            get { return true; }
        }

        public void setContentcount(int ContentCount)
        {
            ContentEntryCount = ContentCount; ;
        }

        public void setContentIndex(int newcontentindex)
        {
            ContentEntryIndex = newcontentindex;
        }

        public void setNewContentIndex(int neEntrywcontentindex)
        {
            newEntryIndex = neEntrywcontentindex;
        }

        public override void Read(BitsStream stream)
        {
            bool IsResourceFile = stream.ReadBool();
            long Offset = (long)stream.ReadBits(23);
            int Size = (int)stream.ReadBits(24);
            NameOffset = (int)stream.ReadBits(16);

            ContentEntryIndex = stream.ReadInt();
            ContentEntryCount = stream.ReadInt();
        }

        public override void Write(BigEndianBinaryWriter bw)
        {
            bw.Write(Flags);
            bw.Write(NameOffset);

            uint temp = (uint)ContentEntryIndex | 0x80000000;
            bw.Write(temp);
            bw.Write(ContentEntryCount);
        }
    }
}