using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace CapCap
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 多重起動防止
            bool createdNew = false;
            Mutex appMutex = new Mutex(true, @"CapCap", out createdNew);
            if (createdNew)
            {
                try
                {
                    // アプリケーション起動
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new frmMain());
                }
                finally
                {
                    appMutex.ReleaseMutex();
                    appMutex.Close();
                }
            }
            else
            {
                MessageBox.Show(@"すでに実行中です。多重起動はできません。", @"多重起動", MessageBoxButtons.OK, MessageBoxIcon.Error);
                appMutex.Close();
            }
        }
    }
}
