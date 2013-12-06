using System.IO;
using RPFLib.Common;

namespace RPFLib.RPF4
{
    internal abstract class TOCEntry : Entry
    {
        public int NameOffset { get; set; }
        public TOC TOC { get; set; }

        public abstract bool IsDirectory { get; }

        public abstract void Read(BinaryReader br);
        public abstract void Write(BinaryWriter bw);
        public abstract int newEntryIndex { get; set; }

        public override void Delete()
        {
            TOC.Delete(this);
        }

        internal static bool ReadAsDirectory(BinaryReader br)
        {
            bool dir;

            br.BaseStream.Seek(8, SeekOrigin.Current);
            dir = br.ReadInt32() < 0;
            br.BaseStream.Seek(-12, SeekOrigin.Current);

            return dir;
        }
    }
}