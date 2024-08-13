using System.Runtime.InteropServices;

namespace Atendance_System
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            //// To customize application configuration such as set high DPI settings or default font,
            //// see https://aka.ms/applicationconfiguration.
            //ApplicationConfiguration.Initialize();
            //Att_Enterance entranceOBJ = new Att_Enterance();
            //Login_form login_FormOBJ = new Login_form(entranceOBJ);
            //Application.Run(login_FormOBJ);

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Insights insights = new Insights();
            Att_Enterance entranceOBJ = new Att_Enterance();
            Login_form login_FormOBJ = new Login_form(entranceOBJ);
            Application.Run(insights);
        }


    }
}