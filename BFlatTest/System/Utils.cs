using System;

namespace System
{
    public unsafe class Utils
	{
		public static string PtrToHexString(nint value)
		{
			const string hex = "0123456789ABCDEF";

			// 2 for "0x" + 16 nybbles for 64-bit + null
			char* buf = stackalloc char[2 + 16];

			buf[0] = '0';
			buf[1] = 'x';

			ulong v = (ulong)value;

			for (int i = 0; i < 16; i++)
			{
				int shift = (15 - i) * 4;
				buf[2 + i] = hex[(int)((v >> shift) & 0xF)];
			}

			return new string(buf);
		}

		public static string NumToDecString(long Val)
		{
			if (Val == 0)
				return "0";

			bool isNegative = false;

			if (Val < 0)
			{
				isNegative = true;
				Val = -Val;
			}

			char* buf = stackalloc char[24]; // enough for any 64-bit integer
			int pos = 0;

			while (Val != 0)
			{
				int digit = (int)(Val % 10);
				buf[pos++] = (char)('0' + digit);
				Val /= 10;
			}

			if (isNegative)
			{
				buf[pos++] = '-';
			}

			// Reverse the string
			for (int i = 0; i < pos / 2; i++)
			{
				char temp = buf[i];
				buf[i] = buf[pos - 1 - i];
				buf[pos - 1 - i] = temp;
			}

			return new string(buf, 0, pos);
		}

		public static string NumToDecString(uint Val)
		{
			return NumToDecString((int)Val);
		}
	}
}
