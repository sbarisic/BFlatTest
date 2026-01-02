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

namespace System
{
	public struct Void { }

	// The layout of primitive types is special cased because it would be recursive.
	// These really don't need any fields to work.
	public struct Boolean
	{
		public override string ToString()
		{
			if (this)
				return "true";

			return "false";
		}
	}
	public struct Char
	{
		public override string ToString()
		{
			return Utils.NumToDecString((long)this);
		}
	}
	public struct SByte
	{
		public override string ToString()
		{
			return Utils.NumToDecString((long)this);
		}
	}
	public struct Byte
	{
		public override string ToString()
		{
			return Utils.NumToDecString((long)this);
		}
	}
	public struct Int16
	{
		public override string ToString()
		{
			return Utils.NumToDecString((long)this);
		}
	}
	public struct UInt16
	{
		public override string ToString()
		{
			return Utils.NumToDecString((long)this);
		}
	}
	public struct Int32
	{
		public const int MaxValue = 0x7fffffff;
		public const int MinValue = unchecked((int)0x80000000);

		public override string ToString()
		{
			return Utils.NumToDecString((long)this);
		}
	}
	public struct UInt32
	{
		public override string ToString()
		{
			return Utils.NumToDecString((long)this);
		}
	}
	public struct Int64
	{
		public const long MaxValue = 0x7fffffffffffffffL;
		public const long MinValue = unchecked((long)0x8000000000000000L);

		public override string ToString()
		{
			return Utils.NumToDecString((long)this);
		}
	}
	public struct UInt64
	{
		public override string ToString()
		{
			return Utils.NumToDecString((long)this);
		}
	}
	public struct IntPtr {
		public static readonly IntPtr Zero = new IntPtr();
	}
	public struct UIntPtr { }
	public struct Single { }
	public struct Double { }
}
