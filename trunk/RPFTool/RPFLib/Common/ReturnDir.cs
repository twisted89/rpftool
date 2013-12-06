using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPFLib.Common
{
    class ReturnDir : fileSystemObject, IEnumerable<fileSystemObject>
    {
        private readonly List<fileSystemObject> _fsObjects = new List<fileSystemObject>();
        private readonly Dictionary<string, fileSystemObject> _fsObjectsByName = new Dictionary<string, fileSystemObject>();

        public RPFLib.Common.Directory Tag { get; set; }
        public new string Name { get { return ".."; } set { Name = value; } }

        public override bool IsDirectory
        {
            get { return true; }
        }

        public override bool IsReturnDirectory
        {
            get { return true; }
        }

        public string Attributes
        {
            get { return ""; }
        }

        private string empty;

        public override uint nameHash { get; set; }
        public string Size { get { return ""; } set { empty = value; } }
        public string SizeS { get { return ""; } set { empty = value; } }
        public string IsResource { get { return ""; } set { empty = value; } }
        public string resourcetype { get { return ""; } set { empty = value; } }
        public string IsCompressed { get { return ""; } set { empty = value; } }

        #region IEnumerable<FSObject> Members

        public IEnumerator<fileSystemObject> GetEnumerator()
        {
            return _fsObjects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _fsObjects.GetEnumerator();
        }

        #endregion
    }
}
