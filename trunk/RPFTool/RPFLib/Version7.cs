using System;
using System.Collections.Generic;
using System.IO;
using RPFLib.RPF7;
using RPFLib.Common;
using System.Reflection;
using System.Linq;
using System.Windows.Forms;
using System.Text;

namespace RPFLib
{
    class Version7 : Archive
    {
        #region Vars
        public override RPFLib.Common.Directory RootDirectory { get; set; }
        private RPFLib.RPF7.File _rpfFile;
        #endregion

        public override void Open(string filename)
        {

            _rpfFile = new RPFLib.RPF7.File();
            int filecount = _rpfFile.Open(filename);
            if (filecount < 1)
            {
                throw new Exception("Could not open RPF file.");
            }

            BuildFS();
        }

        private string GetName(TOCEntry entry)
        {
            if (entry == _rpfFile.TOC[0])
            {
                return "Root";
            }
            return _rpfFile.TOC.GetName(entry.NameOffset << _rpfFile.Header.nameShift);
        }

        public override List<fileSystemObject> search(RPFLib.Common.Directory dir, string searchText)
        {
            List<fileSystemObject> searchList = new List<fileSystemObject>();
            searchList.AddRange((from pv in dir._fsObjectsByName
                                 where pv.Key.Contains(searchText)
                                 select pv.Value));
            subsearch(dir, searchList, searchText);
            return searchList;
        }

        public void subsearch(RPFLib.Common.Directory dir, List<fileSystemObject> searchList, string searchText)
        {
            foreach (fileSystemObject item in dir)
            {
                if (item.IsDirectory)
                {
                    var subdir = item as RPFLib.Common.Directory;
                    searchList.AddRange((from pv in subdir._fsObjectsByName
                                         where pv.Key.Contains(searchText)
                                         select pv.Value));
                    subsearch(subdir, searchList, searchText);
                }
            }
        }

        private byte[] LoadData(FileEntry entry, bool getCustom)
        {
            /*
            if (entry.CustomData == null)
            {

                byte[] data = _rpfFile.ReadData(entry.Offset, entry.SizeInArchive);

                if (entry.IsCompressed)
                {
                    data = DataUtil.DecompressDeflate(data, entry.Size);
                }

                return data;
            }
            else
            {
                return entry.CustomData;
            }
             */

            byte[] data;
            if (getCustom && entry.CustomData != null)
                data = entry.CustomData;
            else
                data = _rpfFile.ReadData(entry.Offset, entry.SizeInArchive);
            if (entry.IsCompressed && entry.IsEncrypted && !entry.IsResourceFile)
            {
                data = DataUtil.DecryptNew(data);
                data = xcompress2.Decompress(data, (int)entry.Size);
            }
            else if (entry.IsCompressed && !entry.IsEncrypted && !entry.IsResourceFile)
            {
                data = xcompress2.Decompress(data, (int)entry.Size);
            }
            return data;
        }

        private void StoreData(FileEntry entry, byte[] data)
        {
            entry.SetCustomData(data);
        }

        public override void Close()
        {
            _rpfFile.Close();
        }


        private void BuildFSDirectory(DirectoryEntry dirEntry, RPFLib.Common.Directory fsDirectory)
        {
            try
            {
                fsDirectory.Name = GetName(dirEntry);
                for (int i = 0; i < dirEntry.ContentEntryCount; i++)
                {

                    TOCEntry entry = _rpfFile.TOC[dirEntry.ContentEntryIndex + i];
                    if (entry.IsDirectory)
                    {
                        var subdirEntry = entry as DirectoryEntry;
                        var dir = new RPFLib.Common.Directory();
                        dir._Contentcount = nCount => subdirEntry.setContentcount(nCount);
                        dir._ContentIndex = ContentIndex => subdirEntry.setContentIndex(ContentIndex);
                        dir._Index = NewContentIndex => subdirEntry.setNewContentIndex(NewContentIndex);
                        dir.Attributes = "Folder";
                        //dir.entryNum = dirEntry.ContentEntryIndex + i;
                        dir.ParentDirectory = fsDirectory;
                        BuildFSDirectory(entry as DirectoryEntry, dir);
                        fsDirectory.AddObject(dir);
                    }
                    else
                    {
                        var fileEntry = entry as FileEntry;
                        var file = new Common.File();
                        file._dataLoad = getCustom => LoadData(fileEntry, getCustom);
                        file._dataStore = data => StoreData(fileEntry, data);
                        file._dataCustom = () => fileEntry.CustomData != null;
                        file.d1 = () => fileEntry.getSize();
                        file._Index = nIndex => fileEntry.setIndex(nIndex);
                        file._delete = () => fileEntry.Delete();

                        //file.entryNum = dirEntry.ContentEntryIndex + i;
                        file.CompressedSize = fileEntry.SizeInArchive;
                        file.IsCompressed = fileEntry.IsCompressed;
                        file.Name = GetName(fileEntry);
                        //file.Size = fileEntry.Size;
                        file.IsResource = fileEntry.IsResourceFile;
                        file.ParentDirectory = fsDirectory;
                        StringBuilder attributes = new StringBuilder();
                        if (file.IsResource)
                        {
                            attributes.Append(string.Format("Resource [Version {0}", fileEntry.ResourceType));
                            if (file.IsCompressed)
                            {
                                attributes.Append("Compressed");
                            }
                            attributes.Append("]");
                        }
                        else if (file.IsCompressed)
                        {
                            attributes.Append("Compressed");
                        }
                        else
                            attributes.Append("None");
                        fsDirectory.AddObject(file);
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public override void Save()
        {
            _rpfFile.save();
        }

        private void BuildFS()
        {
            RootDirectory = new RPFLib.Common.Directory();

            TOCEntry entry = _rpfFile.TOC[0];
            BuildFSDirectory(entry as DirectoryEntry, RootDirectory);
        }
    }
}
