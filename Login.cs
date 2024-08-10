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
        int _adminID;
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
            bool is_admin = false;
            _adminID = parent_enterance.isAdmin(); //matches finger
            if (_adminID > 0)
            {
                is_admin = true;
            }
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
            string query = "select * from Admin where admin_email = @email and admin_pass = @pass and admin_id =@id";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@email", textBox1.Text);
            cmd.Parameters.AddWithValue("@pass", textBox2.Text);
            cmd.Parameters.AddWithValue("@id", _adminID);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
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

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Focus();
                errorProvider1.SetError(textBox1, "Email is mandatory field");
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(textBox2.Text))
            {
                textBox2.Focus();
                errorProvider2.SetError(textBox2, "Password is mandatory field");
            }
            else
            {
                errorProvider2.Clear();
                pictureBox1.Visible = true;
                //MessageBox.Show("Press OK after placing your finger on the Reader");
                button1.Visible = true;
            }
            //if (Att_Enterance._bitmap != null)
            //{

            //    pictureBox1.Image = Att_Enterance._bitmap;
            //    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            //}
            //else
            //    MessageBox.Show("Bitmap is null now");
        }

        private void Login_form_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Att_Enterance._bitmap != null)
            {

                pictureBox1.Image = Att_Enterance._bitmap;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }
    }
}
