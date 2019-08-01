using System;
using System.Collections.Generic;
using System.Text;

using XDocsDesigner;
using Microsoft.Office.Interop.Excel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Acrobat;

namespace Chap13
{
    [ComVisible(true)]
    class Program
    {
        [StructLayout(LayoutKind.Sequential)]
        class OSVersionInfo
        {
            public Int32 dwOSVersionInfoSize;
            public Int32 dwMajorVersion;
            public Int32 dwMinorVersion;
            public Int32 dwBuildNumber;
            public Int32 PlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
        }

        
        [StructLayout(LayoutKind.Explicit)]
        class OSVersionInfo2
        {
            [FieldOffset(0)]
            public float dwUnion;
            [FieldOffset(0)]
            public Int32 dwOSVersionInfoSize;
            [FieldOffset(4)]
            public Int32 dwMajorVersion;
            [FieldOffset(8)]
            public Int32 dwMinorVersion;
            [FieldOffset(12)]
            public Int32 dwBuildNumber;
            [FieldOffset(16)]
            public Int32 PlatformId;
            [FieldOffset(20)]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
        }

        private static object optionalparam = Type.Missing;

        private const Int32 BufferSize = 256;
        [DllImport("user32.dll",ExactSpelling = false)]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        private static extern Int32 GetWindowText(IntPtr hWnd, StringBuilder textValue, Int32 counter);
        static void Main(string[] args)
        {
            //Acrobat.AcroAppClass a = new AcroAppClass();
            
            //EmployeeCOM.EmployeeClass emp = new EmployeeCOM.EmployeeClass();
            Microsoft.Office.Interop.Excel.Application newexcelapp = new Application();
            object o = new object();
            newexcelapp.Visible = true;
            
            try
            {

                newexcelapp.Workbooks.Add(optionalparam);
            }
            catch (RuntimeWrappedException rwex)
            {
                System.Diagnostics.Trace.WriteLine(rwex.ToString());
            }

            GetScreenDemo();

            UnmangedCallbackDemo.RunDemo();

            UnmanagedErrorDemo.ThrowMessageBoxException();

        }

        static void GetScreenDemo()
        {
            StringBuilder sb = new StringBuilder(BufferSize);
            IntPtr DemoHandle = GetForegroundWindow();
            if (GetWindowText(DemoHandle, sb, BufferSize) > 0)
            {
                Console.WriteLine(sb.ToString());
            }
        }
    }

    public class UnmangedCallbackDemo
    {
        public delegate Boolean DemoCallback(IntPtr hWnd, Int32 lParam);
        private const string UserReference = "user32.dll";
        private const Int32 BufferSize = 100;

        [DllImport(UserReference)]
        
        public static extern Int32 EnumWindows(DemoCallback callback, Int32 param);

        [DllImport(UserReference)]
        public static extern Int32 GetWindowText(IntPtr hWnd, StringBuilder lpString, Int32 nMaxCount);

        public static Boolean DisplayWindowInfo(IntPtr hWnd, Int32 lParam)
        {
            StringBuilder sb = new StringBuilder(BufferSize);
            if (GetWindowText(hWnd, sb, BufferSize) != 0)
            {
                Console.WriteLine("Window Text:" + sb.ToString());
            }
            return true;
        }

        public static void RunDemo()
        {
         
            EnumWindows(DisplayWindowInfo, 0);
            Console.WriteLine("Beginning process...");
            Console.ReadLine();
        }
    }

    public class UnmanagedErrorDemo
    {
        private const string KernelReference = "kernel32.dll";
        private const string UserReference = "user32.dll";
        private const Int32 MessageSize = 255;
        [DllImport(KernelReference)]
        private static extern Int32 FormatMessage(Int32 dwFlags, Int32 lpSource, Int32 intdwMessageId, Int32 dwLanguageID,
            ref string lpBuffer, Int32 nSize, Int32 Arguments);
        [DllImport(UserReference)]
        private static extern Int32 MessageBox(IntPtr hWnd, string pText, string pCaption, Int32 uType);

        public static void ThrowMessageBoxException()
        {
            IntPtr ProblemCasuer = (IntPtr)(-100);
            MessageBox(ProblemCasuer, "This won't work", "Caption- This won't work", 0);
            Int32 ErrorCode = Marshal.GetLastWin32Error();
            Console.WriteLine("Error code:" + ErrorCode.ToString());
            Console.WriteLine("Real Error Code:" + GetLastErrorMessage(ErrorCode));
        }
        public static string GetLastErrorMessage(Int32 errorValue)
        {
            Int32 FORMAT_MESSAGE_ALLOCATE_BUFFER =  0x00000100;
            Int32 FORMAT_MESSAGE_IGNORE_INSERTS =   0x00000200;
            Int32 FORMAT_MESSAGE_FROM_SYSTEM =      0x00001000;
            string lpMsgBuff = string.Empty;
            Int32 dwFlags = FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS;
            Int32 ReturnValue = FormatMessage(dwFlags, 0, errorValue, 0, ref lpMsgBuff, MessageSize, 0);
            if (ReturnValue == 0) { return null; }
            else { return lpMsgBuff; }
        }
    }
}