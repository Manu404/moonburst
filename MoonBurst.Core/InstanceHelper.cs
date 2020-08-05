using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace MoonBurst.Core
{
    public class InstanceHelper
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        private Process _existingProcess;

        public bool SetExstingProcessForegroundWindow()
        {
            if (_existingProcess == null) return false;
            return SetForegroundWindow(_existingProcess.MainWindowHandle);
        }

        public bool IsSingleInstance()
        {
            Process current = Process.GetCurrentProcess();
            string currentmd5 = getFileMd5(current.MainModule.FileName);
            Process[] processlist = Process.GetProcesses();
            foreach (Process process in processlist)
            {
                if (process.Id != current.Id)
                {
                    try
                    {
                        if (currentmd5 == getFileMd5(process.MainModule.FileName))
                        {
                            _existingProcess = process;
                            return false;
                        }
                    }
                    catch { }
                }
            }
            return _existingProcess == null;
        }

        private string getFileMd5(string file)
        {
            string check;
            using (FileStream FileCheck = File.OpenRead(file))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] md5Hash = md5.ComputeHash(FileCheck);
                check = BitConverter.ToString(md5Hash).Replace("-", "").ToLower();
            }

            return check;
        }
    }
}
