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

using Internal.Runtime;
using System.Runtime.CompilerServices;

namespace System
{
	public unsafe partial class Object
	{
		// The layout of object is a contract with the compiler.
		internal MethodTable* m_pMethodTable;

		public void PrintMethodTable()
		{
			int Sz = sizeof(MethodTable);
			byte* ptr = (byte*)m_pMethodTable;

			int j = 0;
			Console.Write("0 ");
			for (int i = 0; i < Sz; i++)
			{
				Console.Write(Utils.PtrToHexString((nint)ptr[i], false, 1) + " ");
				j++;

				if (j == 4)
					Console.Write("  ");

				if (j >= 8)
				{
					Console.WriteLine();
					j = 0;

					Console.Write((i / 8 + 1).ToString() + " ");
				}
			}
			Console.WriteLine();

			//Console.Print("MethodTable ptr: ", sizeof(MethodTable));
		}

		public virtual Type GetType()
		{
			MethodTable* mt = m_pMethodTable;
			if (mt == null)
				return null;

			return new RuntimeType(mt);
		}

		public virtual string ToString()
		{
			return "object";
		}

		public virtual bool Equals(object obj)
		{
			// Compare references: true if the same object
			return ReferenceEquals(this, obj);
		}

		public static bool ReferenceEquals(object a, object b)
		{
			Console.WriteLine("Reference equals!");
			void* pa = Unsafe.AsPointer(ref a);
			void* pb = Unsafe.AsPointer(ref b);
			return pa == pb;
		}

		public static bool operator ==(object a, object b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(object a, object b)
		{
			return !a.Equals(b);
		}
	}
}
