// NAnt - A .NET build tool
// Copyright (C) 2001-2011 Gerry Shaw
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Martin Aliger (martin_aliger@myrealbox.com)

using System;
using System.Collections;
using System.Text;
using System.Reflection;
using System.IO;

namespace NAnt.MSBuild.BuildEngine {
    internal class BuildItemGroup4 : BuildItemGroup {
        
        #region Enumerator
        private class BuildItemEnumerator : IEnumerator {
            IEnumerator _po;

            internal BuildItemEnumerator(IEnumerator po) {
                _po = po;
            }

            public object Current {
                get { return new BuildItem4(_po.Current); }
            }

            public bool MoveNext() {
                return _po.MoveNext();
            }

            public void Reset() {
                _po.Reset();
            }
        }
        #endregion

        Project4 _project;
        IEnumerable _obj;

        internal BuildItemGroup4(Project4 project, object o) {
            _project = project;
            _obj = (IEnumerable)o;
        }

        public override System.Collections.IEnumerator GetEnumerator() {
            return new BuildItemEnumerator(_obj.GetEnumerator());
        }

        public override BuildItem AddNewItem(string itemName, string itemInclude) {
            return _project.AddNewItem(itemName, itemInclude);
        }
    }
}
