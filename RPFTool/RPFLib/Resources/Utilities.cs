using System;
using System.Collections.Generic;
using System.Text;

namespace RPFLib.Resources
{
    public static class Utilities
    {

        public static int GetOffset(int offset)
        {
            if (offset == 0)
                return 0;
            else if (offset >> 28 != 5)
                throw new Exception("Bad offset");
            else
                return offset & 0x0fffffff;
        }

        public static int GetDataOffset(int offset)
        {
            if (offset == 0)
                return 0;
            else if (offset >> 28 != 6)
                throw new Exception("Bad offset");
            else
                return offset & 0x0fffff00;
        }

        public static int getVirtualSize(int Size)
        {
            if (Size % 128 != 0)
                Size += (128 - Size % 128);
            return Size;
        }

    }
}