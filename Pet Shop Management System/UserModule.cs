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

namespace Pet_Shop_Management_System
{
    public partial class UserModule : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        string title = "C# Pet Shop";

        bool check = false; // 1:23 Created for function checkAge far below.
        UserForm userForm;
        public UserModule(UserForm user)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            userForm = user;
            cbRole.SelectedIndex = 1; // 1:52 added
        }

        private void btnSave_Click(object sender, EventArgs e) // 58:00
        {
            try
            {
                CheckField(); // 1:24
                if (check)
                {
                    if (MessageBox.Show("Are you sure want to save this user?", "User Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("INSERT INTO tbUser(name,address,phone,role,dob,password)VALUES(@name,@address,@phone,@role,@dob,@password)", cn);
                        cm.Parameters.AddWithValue("@name", txtName.Text);
                        cm.Parameters.AddWithValue("@address", txtAddress.Text);
                        cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                        cm.Parameters.AddWithValue("@role", cbRole.Text);
                        cm.Parameters.AddWithValue("@dob", dtDob.Value);
                        cm.Parameters.AddWithValue("@password", txtPass.Text);

                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("A new user has been registered!", title);
                        Clear();
                        userForm.LoadUser();
                    }
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e) // 1:41 just copied from the btnSave above
        {
            try
            {
                CheckField(); // 1:24
                if (check)
                {
                    if (MessageBox.Show("Are you sure want to update this record?", "Data edit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("UPDATE tbUser SET name=@name,address=@address,phone=@phone,role=@role,dob=@dob,password=@password WHERE id=@id", cn);
                        cm.Parameters.AddWithValue("@id", lbluid.Text);
                        cm.Parameters.AddWithValue("@name", txtName.Text);
                        cm.Parameters.AddWithValue("@address", txtAddress.Text);
                        cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                        cm.Parameters.AddWithValue("@role", cbRole.Text);
                        cm.Parameters.AddWithValue("@dob", dtDob.Value);
                        cm.Parameters.AddWithValue("@password", txtPass.Text);

                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("User's data has been updated sucessfully!", title);
                        Clear();
                        userForm.LoadUser();
                        this.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void cbRole_SelectedIndexChanged(object sender, EventArgs e) // 1:15
        {
            if(cbRole.Text == "Employee")
            {
                this.Height = 616 - 50;
                lblPass.Visible = false;
                txtPass.Visible = false;
            }
            else
            {
                lblPass.Visible = true;
                txtPass.Visible = true;
                this.Height = 616;
            }
        }

        #region Method

        public void Clear()
        {
            txtName.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
            txtPass.Clear();
            cbRole.SelectedIndex = 0;
            dtDob.Value = DateTime.Now;

            btnUpdate.Enabled = false;
        }

        // to check field and date of birth
        public void CheckField() // 1:23
        {
            if(txtName.Text == "" || txtAddress.Text == "")
            {
                MessageBox.Show("This field requires data","Wanring");
                return;
            }
            if(checkAge(dtDob.Value) < 18)
            {
                MessageBox.Show("Persons under 18 years old can not be hired", "Wanring");
                return;
            }
            check = true;
        }
        // to calculate age under 18
        private static int checkAge(DateTime dateofBirth) // 1:21
        {
            int age = DateTime.Now.Year - dateofBirth.Year;
            if(DateTime.Now.DayOfYear < dateofBirth.DayOfYear)
            age = age - 1;
            return age;
        }
        #endregion Method
    }
}
