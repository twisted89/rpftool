using System;
using System.IO;
using RPFLib.Common;
using System.Windows.Forms;

namespace RPFLib.RPF7
{
    internal class File
    {
        private Stream _stream;

        public File()
        {
            Header = new Header(this);
            TOC = new TOC(this);
        }

        public Header Header { get; private set; }
        public TOC TOC { get; private set; }

        public int Open(string filename)
        {
            _stream = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            var br = new BigEndianBinaryReader(_stream);
            Header.Read(br);
            if (!Enum.IsDefined(typeof(HeaderIDs), (int)Header.Identifier))
            {
                _stream.Close();
                return 0;
            }
            _stream.Seek(0x10, SeekOrigin.Begin);
            TOC.Read(br);
            return Header.EntryCount;
        }

        public void Close()
        {
            if (_stream != null)
            {
                _stream.Close();
            }
        }

        public void save()
        {
            try
            {
                if (_stream != null)
                {
                    if (_stream.Length > 0)
                    {
                        _stream.Position = 0;

                        var bw = new BigEndianBinaryWriter(_stream);

                        Header.Write(bw);

                        // Recalculate the offset/sizes of the TOC entries
                        var tocOffset = 0x800;

                        foreach (var entry in TOC)
                        {
                            var fileEntry = entry as FileEntry;
                            if (fileEntry != null && fileEntry.CustomData != null)
                            {

                                if (fileEntry.CustomData.Length <= fileEntry.SizeInArchive)
                                {
                                    // Clear up the old data
                                    _stream.Seek(fileEntry.Offset, SeekOrigin.Begin);
                                    bw.Write(new byte[fileEntry.SizeInArchive]);

                                    // We can fit it in the existing block... so lets do that.
                                    _stream.Seek(fileEntry.Offset, SeekOrigin.Begin);

                                    fileEntry.SizeInArchive = fileEntry.CustomData.Length;
                                    fileEntry.Size = fileEntry.customsize;
                                    bw.Write(fileEntry.CustomData);
                                }
                                else
                                {
                                    bool zero = true;
                                    byte[] zbuffer = new byte[fileEntry.CustomData.Length - fileEntry.SizeInArchive];
                                    zbuffer = ReadData(fileEntry.Offset + fileEntry.SizeInArchive, zbuffer.Length);
                                    foreach (byte b in zbuffer)
                                    {
                                        if (b != 0)
                                        {
                                            zero = false;
                                            break;
                                        }
                                    }
                                    if (zero == false)
                                    {
                                        MessageBox.Show("The new data was " + Convert.ToString(fileEntry.CustomData.Length - fileEntry.SizeInArchive) + " bytes larger than the original and there was not enough trailing space to write the file, make the file smaller and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    }
                                    else
                                    {
                                        // Clear up the old data
                                        _stream.Seek(fileEntry.Offset, SeekOrigin.Begin);
                                        bw.Write(new byte[fileEntry.SizeInArchive]);

                                        // We can fit it in the existing block... so lets do that.
                                        _stream.Seek(fileEntry.Offset, SeekOrigin.Begin);

                                        fileEntry.SizeInArchive = fileEntry.CustomData.Length;
                                        fileEntry.Size = fileEntry.customsize;
                                        bw.Write(fileEntry.CustomData);
                                    }
                                }
                                fileEntry.SetCustomData(null);
                            }
                        }

                        _stream.Seek(tocOffset, SeekOrigin.Begin);

                        TOC.Write(bw);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               // throw new Exception(ex.Message, ex);
            }
        }

        public byte[] ReadData(long offset, int length)
        {
            var buffer = new byte[length];
            _stream.Seek(offset, SeekOrigin.Begin);
            _stream.Read(buffer, 0, length);
            return buffer;
        }

    }
}