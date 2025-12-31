// bflat minimal runtime library
// Copyright (C) 2021-2022 Michal Strehovsky
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Runtime;
using System.Runtime.CompilerServices;

namespace System
{
	public partial class Object
	{
#pragma warning disable 169
		// The layout of object is a contract with the compiler.
		internal unsafe MethodTable* m_pMethodTable;
#pragma warning restore 169

		public virtual unsafe Type GetType()
		{
			MethodTable* mt = m_pMethodTable;
			if (mt == null)
				return null;

			RuntimeType* runtimeType = (RuntimeType*)mt->_writableData;

			if (runtimeType == null)
				return null;
		
			return Unsafe.AsRef<RuntimeType>(runtimeType);

			//return null;
		}

		public virtual unsafe string ToString()
		{
			/*Type TT = GetType();

			if (TT == null)
				return "object (no type)";*/

			return "object";
		}
	}
}
