using System;
using System.Runtime.InteropServices;
using System.Runtime;
using System.Runtime.CompilerServices;
using Internal.Runtime.CompilerHelpers;
using Fish;
using System.IO;
using Internal.Runtime;

public unsafe static class Program
{
	static string ToStr(nint ptr)
	{
		//return ptr.ToString("X16");
		return Utils.PtrToHexString(ptr);
	}

	public static int Main()
	{
		Console.SetCursorPosition(0, 0);
		Console.WriteLine("efi_main Hello World");


		Framebuffer FB = Framebuffer.InitFramebuffer(EfiImageHandle, EfiSystemTable);
		if (FB == null)
		{
			Console.WriteLine("Unable to init framebuffer");
			return 1;
		}

		string fileText = File.ReadAllText("img.bin");
		if (fileText == null)
			Console.WriteLine("fileText is NULL");
		else
		{
			Console.Print("File read! ", fileText.Length, " chars");
			Console.WriteLine(fileText);
		}


		//FB.Init(1920, 1080);
		FB.Init(1280, 720);
		Console.WriteLine();
		Test();
		Console.WriteLine();

		/*FishGL.DrawColor = new Color(255, 0, 0);
        FishGL.Rect(100, 100, 100, 100);

        FishGL.DrawColor = new Color(0, 255, 0);
        FishGL.Rect(120, 120, 100, 100);

        FishGL.DrawColor = new Color(0, 0, 255);
        FishGL.Rect(140, 140, 100, 100);


        FB.SwapBuffer();
        Console.WriteLine("Rectangles drawn!");*/
		return 0;
	}

	//public static TestStruct TS;

	/*static ref TestStruct GetTS()
    {
        return ref TS;
    }*/


	unsafe static void Test()
	{
		/*Console.WriteLine("1");
		Type A = new Type();
		RuntimeType B = new RuntimeType(null);

		Console.WriteLine("2");
		object Obj1 = B;
		RuntimeType C = (RuntimeType)Obj1;

		Console.WriteLine("3");
		object Obj2 = A;
		RuntimeType D = (RuntimeType)Obj2;

		Console.WriteLine("4");
		Console.Print("C is null -", (C == null));
		Console.Print("D is null -", (D == null));

		return;*/

		MethodTable i = new MethodTable();
		Console.Print("MethodTable len - ", Utils.PtrToHexString(sizeof(MethodTable)));
		i.PrintMethodTable();

		Console.Print("typeof(string) - ", typeof(string).ToString());
		Console.Print("typeof(IntPtr) - ", typeof(IntPtr).ToString());
		Console.Print("typeof(object) - ", typeof(object).ToString());
		Console.Print("typeof(TestStruct) - ", typeof(TestStruct).ToString());
		Console.Print("typeof(ulong) - ", typeof(ulong).ToString());
		Console.Print("typeof(Type) - ", typeof(Type).ToString());
		Console.Print("typeof(RuntimeType) - ", typeof(RuntimeType).ToString());

		Console.Print("typeof(string) == typeof(string) - ", (typeof(string) == typeof(string)));
		Console.Print("typeof(string) == typeof(Type) - ", (typeof(string) == typeof(Type)));
		Console.Print("typeof(nuint) == typeof(string) - ", (typeof(nuint) == typeof(string)));
		Console.Print("typeof(Type) == typeof(RuntimeType) - ", (typeof(Type) == typeof(RuntimeType)));

		return;
		TestStruct TS = new TestStruct();
		Console.Print("TestStruct = ", TS.GetType().ToString());

		TestStruct* TSp = &TS;
		Console.Print("TS @ ", (nuint)(TSp));

		TS = new TestStruct();
		TS.Str = "Test String 1";
		TS.Num = 25;
		Console.WriteLine(TS.Str);

		ref TestStruct TS2 = ref TS;
		TS2.Str = "Some other string";

		Console.WriteLine(TS.Str);
		Console.WriteLine("Done!");
	}
}

public struct TestStruct
{
	public string Str;
	public int Num;
}

