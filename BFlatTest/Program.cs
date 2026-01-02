using System;
using System.Runtime.InteropServices;
using System.Runtime;
using System.Runtime.CompilerServices;
using Internal.Runtime.CompilerHelpers;
using Kernel;
using Fish;
using System.IO;

public unsafe static class Program
{
    static string ToStr(nint ptr)
    {
        //return ptr.ToString("X16");
        return Utils.PtrToHexString(ptr);
    }


    [UnmanagedCallersOnly(EntryPoint = "EfiMain2")]
    public static int EfiMain2(EFI_HANDLE imageHandle, EFI_SYSTEM_TABLE* systemTable)
    {
        systemTable->BootServices->SetWatchdogTimer(0, 0, 0, null);
        Console.SetCursorPosition(0, 0);
        Console.WriteLine("efi_main Hello World");
        File.EfiInit(imageHandle, systemTable);


        Framebuffer FB = Framebuffer.InitFramebuffer(imageHandle, systemTable);
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
        Test();

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

    static TestStruct TS = new TestStruct();

    unsafe static void Test()
    {
        fixed (TestStruct* TSp = &TS)
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

