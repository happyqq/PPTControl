using System;
using System.Collections.Generic;
using System.Text;

namespace CommonHelper
{
   public static class ProcessHelper
    {
        public static bool ProcessIsRunning(string ProcName, ref IntPtr Myhwnd, ref IntPtr MyMainhwnd)
        {
            System.Diagnostics.Process[] processList = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process process in processList)
            {
                if (process.ProcessName.ToUpper() == ProcName.ToUpper())
                {
                    Myhwnd = process.Handle;
                    MyMainhwnd = process.MainWindowHandle;
                    return true;

                }
            }
            Myhwnd = IntPtr.Zero;
            return false;
        }
        public static bool ProcessIsRunning(string ProcName)
        {
            IntPtr myhwnd = IntPtr.Zero;
            IntPtr MyMainhwnd = IntPtr.Zero;
            ProcessIsRunning(ProcName, ref myhwnd, ref MyMainhwnd);

            if (myhwnd != IntPtr.Zero)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ProcessIsFront(string ProcName)
        {
            IntPtr myhwnd = IntPtr.Zero;
            IntPtr MyMainhwnd = IntPtr.Zero;
            ProcessIsRunning(ProcName, ref myhwnd, ref MyMainhwnd);

            if (myhwnd != IntPtr.Zero)
            {
                int hwnd = (int)MyMainhwnd;
                //check if its nothing
                if (hwnd != 0)
                {
                    return false;
                }
                return true;
            }

            else
            {
                return false;
            }
        }
        public static void BringWindowToFrontNow(string ProcName)
        {

            IntPtr myhwnd = IntPtr.Zero;
            IntPtr MyMainhwnd = IntPtr.Zero;
            ProcessIsRunning(ProcName, ref myhwnd, ref MyMainhwnd);

            if (myhwnd != IntPtr.Zero)
            {
                //get the (int) hWnd of the process
                int hwnd = (int)MyMainhwnd;
                //check if its nothing
                if (hwnd != 0)
                {
                    //if the handle is other than 0, then set the active window
                    NativeMethods.SetForegroundWindow(MyMainhwnd);
                    NativeMethods.SetActiveWindow(hwnd);
                    NativeMethods.ShowWindow(MyMainhwnd, NativeMethods.ShowWindowEnum.Restore);

                    NativeMethods.PostMessage(MyMainhwnd, NativeMethods.WM_KEYDOWN, NativeMethods.VK_F5, 0);


                }
                else
                {
                    //we can assume that it is fully hidden or minimized, so lets show it!
                    NativeMethods.ShowWindow(myhwnd, NativeMethods.ShowWindowEnum.Restore);
                    NativeMethods.SetActiveWindow((int)MyMainhwnd);

                }
            }
            else
            {
                //tthe process is nothing, so start it
                //System.Diagnostics.Process.Start(@"C:\Program Files\B\B.exe");
            }
        }
    }
}
