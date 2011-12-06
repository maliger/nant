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
    internal abstract class Project {

        protected Project() {}

        public abstract string FullFileName { get; set; }
        public abstract string ToolsVersion { get; }

        public abstract void SetGlobalProperty(string propertyName, string propertyValue);
        public abstract string GetEvaluatedProperty(string propertyName);

        public abstract BuildItemGroup GetEvaluatedItemsByName(string itemName);
        public abstract void RemoveItemsByName(string itemName);
        public abstract BuildItemGroup AddNewItemGroup();

        public abstract bool Build();
    }
}
