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

namespace Pet_Shop_Management_System
{
    public partial class UserForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect(); // DbConnect == DbConnect class. The variable dbCon was created discretionally here.
        SqlDataReader dr;
        string title = "C# Pet Shop";

        public UserForm()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            LoadUser();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadUser();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            UserModule module = new UserModule(this);// 1:09 there occurs an error where no argument given..error. --> put "this" as a parameter.
            module.ShowDialog();
        }

       private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e) // 1:32
        {
            string colName = dgvUser.Columns[e.ColumnIndex].Name;
            if(colName=="Edit")
            {
                UserModule module = new UserModule(this);
                module.lbluid.Text = dgvUser.Rows[e.RowIndex].Cells[1].Value.ToString(); // ID is in the 2nd position in the column edit window.
                module.txtName.Text = dgvUser.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.txtAddress.Text = dgvUser.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.txtPhone.Text = dgvUser.Rows[e.RowIndex].Cells[4].Value.ToString();
                module.cbRole.Text = dgvUser.Rows[e.RowIndex].Cells[5].Value.ToString();
                module.dtDob.Text = dgvUser.Rows[e.RowIndex].Cells[6].Value.ToString();
                module.txtPass.Text = dgvUser.Rows[e.RowIndex].Cells[7].Value.ToString();

                module.btnSave.Enabled = false;
                module.btnUpdate.Enabled = true;
                module.ShowDialog();
            }
            else if(colName == "Delete")
            {
                if(MessageBox.Show("Are you sure want to delete this record?", "Delete record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dbcon.excuteQuery("DELETE FROM tbUser WHERE id LIKE '" + dgvUser.Rows[e.RowIndex].Cells[1].Value.ToString() + "'"); // 2:39 this line was added(its definition is in the DBConnect) and some codes below were removed.
                    MessageBox.Show("User data has been deleted!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LoadUser();
        }

        #region Method
        public void LoadUser() // 1:05. Shows the search result in the DGV
        {
            int i = 0;
            dgvUser.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbUser WHERE CONCAT(name,address,phone,dob,role) LIKE '%" + txtSearch.Text + "%'", cn);
            cn.Open(); // 1:09 Exception unhandled error. See how to fix this. add ";Connect Timeout=30" at the end of DBconnect
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvUser.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), DateTime.Parse(dr[5].ToString()).ToShortDateString(), dr[6].ToString()); // 1:27 partially modified. 1:28 another error. See how to fix it.
            }
            dr.Close();
            cn.Close();
        }
        #endregion Method
    }
}
