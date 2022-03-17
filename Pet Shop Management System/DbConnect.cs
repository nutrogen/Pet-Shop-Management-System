using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Pet_Shop_Management_System // 48:00. This is a class. See what's been happening before.
{
    class DbConnect
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        private string con;

        public string connection()
        {
            con = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\nutro\source\repos\Pet Shop Management System\Pet Shop Management System\dbPetShop.mdf;Integrated Security=True; Connect Timeout=30"; // 1:09 Exception unhandled error. See how to fix this. add ";Connect Timeout=30" at the end.
            return con;
        }

        public void excuteQuery(string sql) // 2:38
        {
            try
            {
                cn.ConnectionString = connection();
                cn.Open();
                cm = new SqlCommand(sql, cn);
                cm.ExecuteNonQuery(); // 3:05 Added to solve the delete function error.
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
