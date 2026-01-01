using System;

namespace System.Runtime.InteropServices
{
	sealed class FieldOffsetAttribute : Attribute
	{
		public FieldOffsetAttribute(int offset)
		{
			Value = offset;
		}

		public int Value { get; }
	}
}
