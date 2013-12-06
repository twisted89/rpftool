using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RPFLib.Common;

namespace RPFLib.RPF4
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

        public void Read(BinaryReader br, int extra = 0)
        {
            if (File.Header.Encrypted)
            {
                int tocSize = File.Header.TOCSize;
                byte[] tocData = br.ReadBytes(tocSize);

                tocData = DataUtil.Decrypt(tocData);

                // Create a memory stream and override our active binary reader
                var ms = new MemoryStream(tocData);
                br = new BinaryReader(ms);

                byte[] tData = br.ReadBytes(File.Header.TOCSize);
                //System.IO.File.WriteAllBytes(@"D:\RPF Tool\MP3\toc_test.hex", tData);
                br.BaseStream.Position = 0;
            }

            

            int entryCount = File.Header.EntryCount;
            for (int i = 0; i < entryCount; i++)
            {
                TOCEntry entry;
                if (TOCEntry.ReadAsDirectory(br))
                {
                    entry = new DirectoryEntry(this);
                }
                else
                {
                    entry = new FileEntry(this);
                }
                entry.Read(br);
                _entries.Add(entry);
            }

            int stringDataSize = File.Header.TOCSize - File.Header.EntryCount * 16;
            byte[] stringData = br.ReadBytes(stringDataSize);
            _nameStringTable = Encoding.ASCII.GetString(stringData);
        }

        public void Write(BinaryWriter bw)
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
            BinaryWriter tempbw = new BinaryWriter(ms);

            foreach (var entry in _entries)
            {
                entry.Write(tempbw);
            }
            BinaryReader tempbr = new BinaryReader(ms);
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