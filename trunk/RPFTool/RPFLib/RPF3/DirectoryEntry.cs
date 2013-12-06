using System.IO;
using RPFLib.Common;
using System;

namespace RPFLib.RPF3
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

        public override void Read(BinaryReader br)
        {
            NameOffset = br.ReadInt32();
            Flags = br.ReadInt32();
            ContentEntryIndex = (int)(br.ReadUInt32() & 0x7fffffff);
            ContentEntryCount = br.ReadInt32() & 0x0fffffff;
        }

        public override void Write(BinaryWriter bw)
        {
            bw.Write(NameOffset);
            bw.Write(Flags);

            uint temp = (uint)ContentEntryIndex | 0x80000000;
            bw.Write(temp);
            bw.Write(ContentEntryCount);
        }
    }
}