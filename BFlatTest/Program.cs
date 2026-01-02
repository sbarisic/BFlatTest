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
        Console.WriteLine("efi_main Hello World");
        File.EfiInit(imageHandle, systemTable);


        Framebuffer FB = Framebuffer.InitFramebuffer(imageHandle, systemTable);
        if (FB == null)
        {
            Console.WriteLine("Unable to init framebuffer");
            return 1;
        }


        //FB.Init(1920, 1080);
        FB.Init(1280, 720);
        Console.Print("Hello ", FB.Width, "x", FB.Height, " World!");

        /*FishGL.DrawColor = new Color(255, 0, 0);
        FishGL.Rect(100, 100, 100, 100);

        FishGL.DrawColor = new Color(0, 255, 0);
        FishGL.Rect(120, 120, 100, 100);

        FishGL.DrawColor = new Color(0, 0, 255);
        FishGL.Rect(140, 140, 100, 100);*/


        Console.WriteLine("Rectangles drawn!");
        FB.SwapBuffer();
        return 0;
    }
}

