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
    public partial class LoginForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        SqlDataReader dr;
        string title = "C# Pet Shop";
        public LoginForm()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
        }

        private void btnClose_Click(object sender, EventArgs e) // 4:23
        {
            if (MessageBox.Show("Would you like to exit the application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e) // 4:26
        {
            try
            {
                string _name = "", _role = "";
                cn.Open();
                cm = new SqlCommand("SELECT name, role FROM tbUser WHERE name=@name and password=@password", cn);
                cm.Parameters.AddWithValue("@name", txtname.Text);
                cm.Parameters.AddWithValue("@password", txtpass.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if(dr.HasRows)
                {
                    _name = dr["name"].ToString();
                    _role = dr["role"].ToString();
                    MessageBox.Show("Welcome " + _name + "!", "Acess granted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MainForm main = new MainForm();
                    main.LblUsername.Text = _name;
                    main.LblRole.Text = _role;
                    if (_role == "Administrator")
                        main.btnUser.Enabled = true;
                    this.Hide();
                    main.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Invalid username and/or password!", "Acess denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message,title);
            }
        }

        private void btnForget_Click(object sender, EventArgs e) // 4:33
        {
            MessageBox.Show("Please contact the administrator", "Password lost", MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
    }
}
