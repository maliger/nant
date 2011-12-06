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
    internal class BuildItem4 : BuildItem {
        object _obj;
        Type _t;

        internal BuildItem4(object o) {
            _obj = o;
            _t = _obj.GetType();
        }

        public string Name {
            get { return (string)_t.GetProperty("ItemType").GetValue(_obj, null); }
        }        

        public override string FinalItemSpec {
            get { return (string)_t.GetProperty("EvaluatedInclude").GetValue(_obj, null); }
        }

        public override string GetMetadata(string metadataName) {
            return (string)_t.GetMethod("GetMetadataValue").Invoke(_obj, new object[] { metadataName });
        }

        public override void SetMetadata(string metadataName, string metadataValue) {
            _t.GetMethod("SetMetadataValue", new Type[] { typeof(string), typeof(string) }).Invoke(_obj, new object[] { metadataName, metadataValue });
        }
    }
}
