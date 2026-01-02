using Internal.Runtime.CompilerHelpers;
using System;

namespace Fish
{
	public unsafe class CoreTask
	{
		EFI_BOOT_SERVICES* bs;
		EFI_MP_SERVICES_PROTOCOL* mpServices;
		int NumProc;

		public CoreTask(EFI_BOOT_SERVICES* bs)
		{
			this.bs = bs;
			EFI_GUID EfiMpServiceProtocolGUID = new EFI_GUID(0x3fdda605, 0xa76e, 0x4f46, 0xad, 0x29, 0x12, 0xf4, 0x53, 0x1b, 0x3d, 0x08);
			mpServices = null;

			fixed (EFI_MP_SERVICES_PROTOCOL** pMpServices = &mpServices)
				bs->LocateProtocol(&EfiMpServiceProtocolGUID, null, (void**)pMpServices);

			if (mpServices == null)
			{
				Console.WriteLine("Unable to locate MP Services Protocol");
				return;
			}


			nuint numProc = 0;
			nuint numEnabled = 0;
			mpServices->GetNumberOfProcessors(mpServices, &numProc, &numEnabled);

			//Console.Print("NumProc = ", numProc, ", NumEnabled = ", numEnabled);
			NumProc = (int)numProc;

			/*Console.Write("EnableDisableAP - ");
			ulong res = mpServices->EnableDisableAP(mpServices, 1, 0, null);
			Console.WriteLine(Utils.PtrToHexString((nint)res));

			mpServices->GetNumberOfProcessors(mpServices, &numProc, &numEnabled);

			Console.Print("NumProc = ", numProc, ", NumEnabled = ", numEnabled);
			NumProc = (int)numProc;*/
		}

		public void RunOnCore(EFI_AP_PROCEDURE A, int ProcNum)
		{
			EFI_TPL TPL_NOTIFY = new EFI_TPL();
			TPL_NOTIFY._tpl = 16;

			EFI_EVENT Evt = new EFI_EVENT();
			ulong res = bs->CreateEvent(0, TPL_NOTIFY, null, null, &Evt);
			if (res != 0)
				return;

			//Console.Write("EnableDisableAP - ");
			res = mpServices->EnableDisableAP(mpServices, (nuint)ProcNum, 0x1, null);
			//Console.WriteLine(Utils.PtrToHexString((nint)res));

			if (res != 0)
				return;


			//Console.Write("StartupThisAP - ");
			res = mpServices->StartupThisAP(mpServices, (delegate* unmanaged<void*, EFI_STATUS>)A.m_functionPointer, (nuint)ProcNum, Evt, 0, mpServices, null);

			//bs->Stall(500);
			//Console.WriteLine(Utils.PtrToHexString((nint)res));
		}

		public int GetCurrentProcessorNumber()
		{
			nuint procNum = 0;
			ulong res = mpServices->WhoAmI(mpServices, &procNum);

			if (res != 0)
				return -1;

			return (int)procNum;
		}
	}
}
