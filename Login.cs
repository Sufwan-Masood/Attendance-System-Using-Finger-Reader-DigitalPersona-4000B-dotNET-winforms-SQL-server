using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atendance_System
{
    public partial class Login_form : Form
    {
        [DllImport("kernel32.dll")] private static extern bool AllocConsole();
        public Att_Enterance parent_enterance;

        public Login_form(Att_Enterance parent_enterance)
        {
            InitializeComponent();
            this.parent_enterance = parent_enterance;
            AllocConsole();
        }

        private void button1_Click(object sender, EventArgs e)
        {   
            bool is_admin = parent_enterance.isAdmin();
            if (is_admin) {
                parent_enterance.Show();
                this.Hide();
                Att_Enterance.dbPermission = true; 
            }
            
            
        }
    }
}
