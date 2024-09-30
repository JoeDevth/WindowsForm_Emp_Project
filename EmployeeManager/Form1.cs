using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Mysqlx.Datatypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Xml.Linq;

namespace EmployeeManager
{
    public partial class LoadData : Form
    {
        // Connection string ที่เชื่อมต่อกับ MySQL บน XAMPP
        string connectionString = "Server=localhost;Database=employeedb;Uid=root;Pwd=;Port=3306;";

        public LoadData()
        {
            InitializeComponent();
        }

        private void LoadData_Load(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM employees", conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridViewEmployees.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void dataGridViewEmployees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //เมื่อเลือกข้อมูลใน Grid จะให้ไปแสดงข้อมูลใน Txt, cbb
            if (e.RowIndex == -1) //ไม่เลือกอย่างอื่นที่ไม่ใช่ข้อมูล
                return; //จบการทำงาน
            else
            {
                dataGridViewEmployees.Rows[e.RowIndex].Selected = true;
                //ดึงข้อมูลจาก grid
                DataGridViewRow dgr = dataGridViewEmployees.Rows[e.RowIndex];
                txtEmployeeID.Text = dgr.Cells[0].Value.ToString();
                txtName.Text = dgr.Cells[1].Value.ToString();
                txtSalary.Text = dgr.Cells[2].Value.ToString();
            }

        }



        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Employees (empID, empName, empSalary) VALUES (@id, @name, @salary)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtEmployeeID.Text)); // ใช้ empID จากการกรอกของผู้ใช้
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@salary", Convert.ToDecimal(txtSalary.Text));
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("เพิ่มพนักงานเรียบร้อยแล้ว");
                    ClearFields();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
                }
            }
        }




        private void ClearFields()
        {
            throw new NotImplementedException();
        }

        private void btnEdit_Click (object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Employees SET empName = @name, empSalary = @salary WHERE empID = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtEmployeeID.Text)); // กรอก empID ที่ต้องการแก้ไข
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@salary", Convert.ToDecimal(txtSalary.Text));
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("แก้ไขข้อมูลพนักงานเรียบร้อยแล้ว");
                    ClearFields();
                    ShowAllEmployees();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
                }
            }
            
        }



        private void btnDelete_Click(object sender, EventArgs e)
        {
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM Employees WHERE empID = @id" ;
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtEmployeeID.Text));
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("ลบพนักงานเรียบร้อยแล้ว");
                        ClearFields();
                        ShowAllEmployees();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("เกิดข้อผิดพลาด: " +ex.Message);
                    }
                }
            }
        }

        // ฟังก์ชันแสดงข้อมูลพนักงานทั้งหมดใน DataGridView
        private void ShowAllEmployees()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT* FROM Employees";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    System.Data.DataTable table = new System.Data.DataTable();
                    adapter.Fill(table);
                    dataGridViewEmployees.DataSource = table;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาด:" + ex.Message);

                }
            }
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            ShowAllEmployees();
        }

       
    }
}
