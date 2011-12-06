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
using System.Xml;

namespace NAnt.MSBuild.BuildEngine {

    /// <summary>
    /// MSBuild project wrapper for .NET 4.0
    /// </summary>
    internal class Project4 : Project {

        object _obj;
        Type _t;

        public Project4(Engine engine, System.Xml.XmlElement xmlDefinition, Version version)
        {
            //public Project LoadProject(
            //      XmlReader xmlReader,
            //      string toolsVersion
            //  )
            _obj = engine.Type.GetMethod("LoadProject", new Type[] { typeof(XmlReader), typeof(string) }).Invoke(engine.Object, new object[] { 
                    new XmlNodeReader(xmlDefinition),
                    version.ToString()
                });
            _t = _obj.GetType();
        }

        public override string FullFileName {
            get { return (string)_t.GetProperty("FullPath").GetValue(_obj, null); }
            set { _t.GetProperty("FullPath").SetValue(_obj, value, null); }
        }

        public override string ToolsVersion {
            get { PropertyInfo pi = _t.GetProperty("ToolsVersion"); if (pi == null) return "4.0"; return (string)pi.GetValue(_obj, null); }
        }

        public override void SetGlobalProperty(string propertyName, string propertyValue)
        {
            _t.GetMethod("SetGlobalProperty", new Type[] { typeof(string), typeof(string) }).Invoke(_obj, new object[] { propertyName, propertyValue });
        }

        public override string GetEvaluatedProperty(string propertyName)
        {
            //TODO: test whether it returns evaluated value. docs are not helpful here
            return (string)_t.GetMethod("GetPropertyValue").Invoke(_obj, new object[] { propertyName });
        }

        public override BuildItemGroup GetEvaluatedItemsByName(string itemName) {
            //TODO: test whether it returns by name (what is itemType?)
            return new BuildItemGroup4(this, _t.GetMethod("GetItems").Invoke(_obj, new object[] { itemName }));
        }
        internal BuildItem4 AddNewItem(string itemName, string itemInclude)
        {
            //the method returns IList<ProjectItem>
            object added = _t.GetMethod("AddItemFast", new Type[] { typeof(string), typeof(string) }).Invoke(_obj, new object[] { itemName, itemInclude });
            IEnumerator e = (added as IEnumerable).GetEnumerator();
            if (e.MoveNext() == false) return null; //nething added?
            return new BuildItem4(e.Current);
        }

        public override void RemoveItemsByName(string itemName) {
            object items = _t.GetMethod("GetItems").Invoke(_obj, new object[] { itemName });
            _t.GetMethod("RemoveItems").Invoke(_obj, new object[] { items });
        }

        public override BuildItemGroup AddNewItemGroup() {
            return new BuildItemGroup4(this, new object[0]);
        }

        public override bool Build() {
            return (bool) _t.GetMethod("Build", new Type[] {}).Invoke(_obj, null);
        }

    }
}
