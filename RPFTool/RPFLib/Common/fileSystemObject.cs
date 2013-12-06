using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPFLib.Common
{
    public abstract class fileSystemObject
    {
        public string Name { get; set; }      
        public Directory ParentDirectory { get; set; }

        public string FullName
        {
            get
            {
                if (ParentDirectory == null)
                {
                    return Name;
                }
                else
                {
                    return ParentDirectory.FullName + @"\" + Name;
                }
            }
        }

        public abstract uint nameHash { get; set; }
        public abstract bool IsDirectory { get; }
        public abstract bool IsReturnDirectory { get; }
    }
}
