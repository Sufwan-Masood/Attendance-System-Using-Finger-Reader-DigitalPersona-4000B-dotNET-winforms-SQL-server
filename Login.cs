using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
        string cs = "Data Source=DESKTOP-1907SQ5;Initial Catalog=Attendance;Integrated Security=True";
        public Login_form(Att_Enterance parent_enterance)
        {
            InitializeComponent();
            this.parent_enterance = parent_enterance;
            AllocConsole();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool is_admin = parent_enterance.isAdmin(); // matches Finger Data;
            bool idPassMatched = id_pass_isMatched(); // matches Id and Password
            if (!is_admin) { Console.WriteLine("Admin Finger does not Match"); }
            if (!idPassMatched) { Console.WriteLine("Admin ID/Password does not Match"); }
            if (is_admin && idPassMatched)
            {
                parent_enterance.Show();
                this.Hide();
                Att_Enterance.dbPermission = true;  // make this true when login is successfull
            }
            else
                MessageBox.Show("Admin Credentials Not Matched", "Login Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public bool id_pass_isMatched()
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "select admin_email , admin_pass from Admin where admin_email = @email and admin_pass = @pass";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@email", textBox1.Text);
            cmd.Parameters.AddWithValue("@pass", textBox2.Text);
            con.Open();
            SqlDataReader dr =cmd.ExecuteReader();
            if (dr.HasRows)
            {
                con.Close();
                return true;
            }
            else 
            {
                con.Close();
                return false; 
            }  
        }
    }
}
