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
    /// <summary>
    /// MSBuild project wrapper for .NET 2.0/3.0/3.5
    /// </summary>
    internal class Project3 : Project {
        object _obj;
        Type _t;

        public Project3(Engine engine) {
            _t = engine.Assembly.GetType("Microsoft.Build.BuildEngine.Project");
            _obj = Activator.CreateInstance(_t, engine.Object);                      
        }

        public override string FullFileName {
            get { return (string)_t.GetProperty("FullFileName").GetValue(_obj, null); }
            set { _t.GetProperty("FullFileName").SetValue(_obj, value, null); }
        }

        public override string ToolsVersion {
            get { PropertyInfo pi = _t.GetProperty("ToolsVersion"); if (pi == null) return "2.0"; return (string)pi.GetValue(_obj, null); }
        }
        internal void SetToolsVersion(string value) {
            PropertyInfo pi = _t.GetProperty("ToolsVersion"); if (pi == null) return; pi.SetValue(_obj, value, null);
        }

        public void LoadXml(string projectXml) {
            _t.GetMethod("LoadXml", new Type[] { typeof(string) }).Invoke(_obj, new object[] { projectXml });
        }

        //public BuildPropertyGroup GlobalProperties {
        //    get { return new BuildPropertyGroup(_t.GetProperty("GlobalProperties").GetValue(_obj, null)); }
        //}
        public override void SetGlobalProperty(string propertyName, string propertyValue)
        {
            object globals = _t.GetProperty("GlobalProperties").GetValue(_obj, null);
            globals.GetType().GetMethod("SetProperty", new Type[] { typeof(string), typeof(string) }).Invoke(globals, new object[] { propertyName, propertyValue });
        }

        public override string GetEvaluatedProperty(string propertyName) {
            return (string)_t.GetMethod("GetEvaluatedProperty").Invoke(_obj, new object[] { propertyName });
        }

        public override BuildItemGroup GetEvaluatedItemsByName(string itemName) {
            return new BuildItemGroup3(_t.GetMethod("GetEvaluatedItemsByName").Invoke(_obj, new object[] { itemName }));
        }

        public override void RemoveItemsByName(string itemName) {
            _t.GetMethod("RemoveItemsByName").Invoke(_obj, new object[] { itemName });            
        }

        public override BuildItemGroup AddNewItemGroup() {
            return new BuildItemGroup3(_t.GetMethod("AddNewItemGroup").Invoke(_obj, null));
        }

        public override bool Build() {
            return (bool) _t.GetMethod("Build", new Type[] {}).Invoke(_obj, null);
        }

    }
}
