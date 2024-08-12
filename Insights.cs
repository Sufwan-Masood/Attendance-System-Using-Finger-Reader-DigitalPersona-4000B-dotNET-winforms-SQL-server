using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atendance_System
{
    public partial class Insights : Form
    {
        string cs = "Data Source=DESKTOP-1907SQ5;Initial Catalog=Attendance;Integrated Security=True";

        public Insights()
        {
            InitializeComponent();
            sqlDataBind();
        }
        private void sqlDataBind()
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "select Emp_Id,Emp_Name,Emp_Joining from Employees";
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable table = new DataTable();
            da.Fill(table);
            dataGridView1.DataSource = table;

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {   Console.ForegroundColor= ConsoleColor.Green;
                Console.WriteLine($"showing Data of:{dataGridView1.SelectedRows[0].Cells[1].Value.ToString()}({dataGridView1.SelectedRows[0].Cells[0].Value.ToString()})");
                Console.ResetColor();
                showData(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), 1);
            }
            catch { }
        }

        private void showData(string? id, int procedure)
        {
            string query1 = $"Select  a._Date as Date,a.CheckIN,a.CheckOUT from Attendances as a  Join Employees as e on a.Id = e.Emp_Id where a.id = {id}";
            Console.WriteLine(id);
            if (procedure == 1)
            {
                SqlConnection con = new SqlConnection(cs);
                SqlDataAdapter da = new SqlDataAdapter(query1, con);
                DataTable table = new DataTable();
                da.Fill(table);
                dataGridView2.DataSource = table;
            }
        }
    }

}
