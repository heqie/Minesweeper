using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Runtime.InteropServices;

namespace Minesweeper
{
     static class Program
    {
    //     [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    //    private static extern bool SetDllDirectory(string lpPathName);

        [STAThread]
        static void Main()
        {
            // // 设置 Dlls 文件夹为 .dll 文件的搜索路径
            // string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dlls");
            // SetDllDirectory(dllPath);

            // AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            // 启动窗体
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);     
            Application.Run(new MainForm());
        }

    // private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
    // {
    //     string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dlls", new AssemblyName(args.Name).Name + ".dll");
    //     if (File.Exists(dllPath))
    //     {
    //         return Assembly.LoadFrom(dllPath);
    //     }
    //     return null;
    // }

    }

}