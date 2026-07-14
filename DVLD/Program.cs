using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Login;
using DVLD.People;

namespace DVLD
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frmMain(new frmLogin));
            //Application.Run(new frmLogin());
            while (true)
            {
                using (frmLogin login = new frmLogin())
                {
                    if (login.ShowDialog() == DialogResult.OK)
                    {
                        using (frmMain mainScreen = new frmMain())
                        {
                            mainScreen.ShowDialog();
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

        }
    }
}
