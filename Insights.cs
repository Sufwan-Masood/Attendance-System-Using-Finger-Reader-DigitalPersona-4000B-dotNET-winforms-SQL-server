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
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Atendance_System
{
    public partial class Insights : Form
    {
        string cs = "Data Source=DESKTOP-1907SQ5;Initial Catalog=Attendance;Integrated Security=True";
        Login_form parent_LoginForm;
        public Insights(Login_form parent_LoginForm)
        {
            InitializeComponent();
            sqlDataBind();
            this.parent_LoginForm = parent_LoginForm;
        }
        public Insights()
        {
            InitializeComponent();
            sqlDataBind();
        }
        private void sqlDataBind()
        {
            SqlConnection con = new SqlConnection(cs);
            string query2 = @"select e.Emp_Id , e.Emp_Name ,e.Emp_Joining ,count(*)as Attendance_Count from employees as e join Attendances as a on e.Emp_Id = a.id  group by e.Emp_Id ,e.Emp_Name , e.Emp_Joining;";
            //string query = "select Emp_Id,Emp_Name,Emp_Joining from Employees";
            SqlDataAdapter da = new SqlDataAdapter(query2, con);
            DataTable table = new DataTable();
            da.Fill(table);
            dataGridView1.DataSource = table;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Console.WriteLine("Check");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"showing Data of:{dataGridView1.SelectedRows[0].Cells[1].Value.ToString()}({dataGridView1.SelectedRows[0].Cells[0].Value.ToString()})");
                Console.ResetColor();
                string id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                showData(id, 1);
                //exportToExcel(int.Parse(id));
                label1.Text = "Employee ID: " + dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                label2.Text = "Name: " + dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                label3.Text = "Joining Date: " + dataGridView1.SelectedRows[0].Cells[2].Value.ToString();

                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                button2.Text = $"Download Data of {dataGridView1.SelectedRows[0].Cells[1].Value.ToString()}";
            }
            catch { }
        }

        private void showData(string id, int procedure)
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            parent_LoginForm.Show();
        }
        private void exportToExcel(int id)
        {
            string name = "Null";
            string query = @"Select e.Emp_Name, a._Date as Date,a.CheckIN,a.CheckOUT from Attendances as a  Join Employees as e on a.Id = e.Emp_Id and a.id = @id";
            string query1 = "SELECT a._Date AS Date, a.CheckIN, a.CheckOUT FROM Attendances AS a JOIN Employees AS e ON a.Id = e.Emp_Id WHERE a.Id = @id";

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Employee Data");
                    int currentRow = 1;

                    // Adding Headers
                    worksheet.Cell(currentRow, 1).Value = "Date";
                    worksheet.Cell(currentRow, 2).Value = "Check-In";
                    worksheet.Cell(currentRow, 3).Value = "Check-Out";

                    
                    // Adding Data
                    while (dr.Read())
                    {
                        name = dr["Emp_Name"].ToString();
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = dr["Date"] != DBNull.Value ? dr["CheckIN"].ToString() : string.Empty;
                        worksheet.Cell(currentRow, 2).Value = dr["CheckIN"] != DBNull.Value ? dr["CheckIN"].ToString() : string.Empty;
                        worksheet.Cell(currentRow, 3).Value = dr["CheckOUT"] != DBNull.Value ? dr["CheckOUT"].ToString() : string.Empty;
                    }

                    dr.Close();
                    workbook.SaveAs($"{name}.xlsx");
                    MessageBox.Show($"Data has been exported to {name}.xlsx successfully!", "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try {
                exportToExcel(int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString()));
            }
            catch { }
           
        }
    }
}
