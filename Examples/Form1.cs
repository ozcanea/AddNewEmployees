using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace Examples
{
    public partial class Form1 : Form
    {
         
        SqlConnection connect=new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;
        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                foreach (string dep in Enum.GetNames(typeof(Departmant)))
                {
                    cBDepartmant.Items.Add(dep);
                }
                cBDepartmant.SelectedIndex = 0;
                cmd.CommandText = "SELECT FirstName,LastName,Departmant,BirthDate,HireDate,Salary FROM [Staff Info]";
                cmd.Connection = connect;
                listBox1.Items.Clear();
                if (connect.State == ConnectionState.Closed)
                    connect.Open();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                    while (dr.Read())
                    {
                        listBox1.Items.Add(dr.GetString(0) + " " + dr.GetString(1) + " " + dr.GetString(2) + " " + dr.GetDateTime(3) + " " + dr.GetDateTime(4) + " " + dr.GetDecimal(5));
                    }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {

                CloseConnection();

            }
            


        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime date = DateTime.Now;
                if (!String.IsNullOrEmpty(txtFirstName.Text) || !String.IsNullOrWhiteSpace(txtFirstName.Text) || String.IsNullOrWhiteSpace(txtLastName.Text) || String.IsNullOrEmpty(txtLastName.Text))
                {
                    cmd.CommandText = "INSERT INTO [Staff Info](Firstname,LastName,BirthDate,HireDate,Departmant,Salary) VALUES (@Name,@Surname,@BirthDate,@HireDate,@Departman,@Salary)";
                    cmd.Connection = connect;
                    cmd.Parameters.AddWithValue("@Name", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@Surname", txtLastName.Text);
                    cmd.Parameters.AddWithValue("@BirthDate", dTPBirthDate.Value);
                    cmd.Parameters.AddWithValue("@HireDate", date);
                    cmd.Parameters.AddWithValue("@Departman", cBDepartmant.SelectedItem);
                    decimal salary = GetSalary(cBDepartmant.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Salary", salary);
                    if(connect.State == ConnectionState.Closed)
                        connect.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    FillListBox();


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                
                connect.Close();
                

            }
        }
        decimal GetSalary(string departmant)
        {
            decimal salary = 0;
            foreach (int wage in Enum.GetValues(typeof(Departmant)))
            {
                if (departmant == Enum.GetName(typeof(Departmant), wage))
                    salary = wage;
                  
            }
            return salary;
        }
        void FillListBox()
        {
            listBox1.Items.Clear();
            CloseConnection();
            connect.Open();
            cmd.Parameters.Clear();
            cmd.CommandText = "SELECT FirstName,LastName,Departmant,BirthDate,HireDate,Salary FROM [Staff Info]";
            cmd.Connection=connect;
            dr=cmd.ExecuteReader();
            if(dr.HasRows)
                while(dr.Read())
                {
                    listBox1.Items.Add(dr.GetString(0) + " " + dr.GetString(1) + " " + dr.GetString(2) + " " + dr.GetDateTime(3) + " " + dr.GetDateTime(4) + " " + dr.GetDecimal(5));
                }
            CloseConnection();

        }
        void CloseConnection()
        {
            connect.Close();
            dr.Close();
        }
//        CREATE TABLE[Staff Info](
//ID INT PRIMARY KEY IDENTITY(1,1),
//FirstName NVARCHAR(20) NOT NULL,
//LastName NVARCHAR(20) NOT NULL,
//Departmant NVARCHAR(20) NOT NULL,
//BirthDate DATETIME NOT NULL,
//HireDate DATETIME NOT NULL,
//Salary MONEY NOT NULL)
    }
}
