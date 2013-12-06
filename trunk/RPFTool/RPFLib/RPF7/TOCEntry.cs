using System.IO;
using RPFLib.Common;

namespace RPFLib.RPF7
{
    internal abstract class TOCEntry : Entry
    {
        public int NameOffset { get; set; }
        public TOC TOC { get; set; }

        public abstract bool IsDirectory { get; }

        public abstract void Read(BitsStream br);
        public abstract void Write(BigEndianBinaryWriter bw);
        public abstract int newEntryIndex { get; set; }

        public override void Delete()
        {
            TOC.Delete(this);
        }

        internal static bool ReadAsDirectory(BitsStream stream)
        {
            bool dir = stream.ReadInt() == 2147483392;
            stream.Seek(stream.Position - 4);
            return dir;
        }
    }
}