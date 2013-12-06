using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RPFLib.Common;

namespace RPFLib.RPF7
{
    internal class TOC : IEnumerable<TOCEntry>
    {
        private readonly List<TOCEntry> _entries = new List<TOCEntry>();
        private string _nameStringTable;

        public TOC(File file)
        {
            File = file;
        }

        public File File { get; private set; }

        public TOCEntry this[int index]
        {
            get { return _entries[index]; }
        }

        public bool Delete(TOCEntry entry)
        {
            try
            {
                _entries.Remove(entry);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetName(int offset)
        {
            if (offset > _nameStringTable.Length)
            {
                throw new Exception("Invalid offset for name");
            }

            int endOffset = offset;
            while (_nameStringTable[endOffset] != 0)
            {
                endOffset++;
            }
            return _nameStringTable.Substring(offset, endOffset - offset);
        }

        #region IFileAccess Members

        public static void AppendAllBytes(string path, byte[] bytes)
        {
            //argument-checking here.

            using (var stream = new FileStream(path, FileMode.Append))
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        public void Read(BigEndianBinaryReader br, int extra = 0)
        {
            if (File.Header.Encrypted)
            {
                int tocSize = File.Header.TOCSize  + File.Header.namesLength;
                byte[] tocData = br.ReadBytes(tocSize);

                tocData = DataUtil.DecryptNew(tocData);

                // Create a memory stream and override our active binary reader
                var ms = new MemoryStream(tocData);
                br = new BigEndianBinaryReader(ms);

                byte[] tData = br.ReadBytes(File.Header.TOCSize);
                System.IO.File.WriteAllBytes(@"Z:\D\Downloads\gms\GTAV\toc_test.bin", tData);
                br.BaseStream.Position = 0;
            }        

            int entryCount = File.Header.EntryCount;
            BitsStream stream = new BitsStream(br.BaseStream);
            for (int i = 0; i < entryCount; i++)
            {
                TOCEntry entry;
                if (TOCEntry.ReadAsDirectory(stream))
                {
                    entry = new DirectoryEntry(this);
                }
                else
                {
                    entry = new FileEntry(this);
                }
                entry.Read(stream);
                _entries.Add(entry);
            }

            br.BaseStream.Position = File.Header.EntryCount * 16;
            byte[] stringData = br.ReadBytes(File.Header.namesLength);
            _nameStringTable = Encoding.ASCII.GetString(stringData);
        }

        public void Write(BigEndianBinaryWriter bw)
        {
            /*
            foreach (var entry in _entries)
            {
                entry.Write(bw);
            }
            byte[] stringData = Encoding.ASCII.GetBytes(_nameStringTable);
            bw.Write(stringData);
             */

            MemoryStream ms = new MemoryStream();
            BigEndianBinaryWriter tempbw = new BigEndianBinaryWriter(ms);

            foreach (var entry in _entries)
            {
                entry.Write(tempbw);
            }
            BigEndianBinaryReader tempbr = new BigEndianBinaryReader(ms);
            ms.Position = 0;
            //bw.Write(DataUtil.Encrypt(tempbr.ReadBytes((int)tempbr.BaseStream.Length)));
            bw.Write(tempbr.ReadBytes((int)tempbr.BaseStream.Length));
            byte[] stringData = Encoding.ASCII.GetBytes(_nameStringTable);
            //bw.Write(DataUtil.Encrypt(stringData));
            bw.Write(stringData);
        }

        #endregion

        #region Implementation of IEnumerable

        public IEnumerator<TOCEntry> GetEnumerator()
        {
            return _entries.GetEnumerator();
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}