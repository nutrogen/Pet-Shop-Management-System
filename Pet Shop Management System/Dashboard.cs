﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pet_Shop_Management_System
{
    public partial class Dashboard : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        SqlDataReader dr;
        string title = "C# Pet Shop";
        public Dashboard()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
        }

        #region Method
        public int extractData(string str) // 5:15. See how data is extracted from SQL and displayed in the dashboard.
        {
            int data = 0;
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT ISNULL(SUM(pqty),0) AS qty FROM tbProduct WHERE pcategory='"+ str +"'", cn); // 5:26 partially changed
                data = int.Parse(cm.ExecuteScalar().ToString());
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message,title);
            }
            return data;
        }
        #endregion Method

        private void Dashboard_Load(object sender, EventArgs e) // 5:18
        {
            lblDog.Text = extractData("Dog").ToString();
            lblCat.Text = extractData("Cat").ToString();
            lblFish.Text = extractData("Fish").ToString();
            lblBird.Text = extractData("Bird").ToString();
        }
    }
}