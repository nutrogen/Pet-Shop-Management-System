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
    public partial class ProductForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        SqlDataReader dr;
        string title = "C# Pet Shop";

        public ProductForm()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            LoadProduct();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ProductModule module = new ProductModule(this);
            module.ShowDialog();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct(); // 3:21
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvProduct.Columns[e.ColumnIndex].Name;
            if(colName == "Edit")
            {
                ProductModule module = new ProductModule(this);
                module.lblPcode.Text = dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtName.Text = dgvProduct.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.txtType.Text = dgvProduct.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.cbCategory.Text = dgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();
                module.txtQty.Text = dgvProduct.Rows[e.RowIndex].Cells[5].Value.ToString();
                module.txtPrice.Text = dgvProduct.Rows[e.RowIndex].Cells[6].Value.ToString();

                module.btnSave.Enabled = false;
                module.btnUpdate.Enabled = true;
                module.ShowDialog();
            }
            else if(colName == "Delete")
            {
                if(MessageBox.Show("Are you sure want to delete this product?", "Delete record", MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    dbcon.excuteQuery("DELETE FROM tbProduct WHERE pcode LIKE '" + dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString() + "'"); // 2:40 this line was added(its definition is in the DBConnect) and some codes below were removed.
                    MessageBox.Show("Product's records have been sucessfully deleted!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LoadProduct();
        }

        #region Method

        public void LoadProduct() // 2:08 Copied from UserForm
        {
            int i = 0;
            dgvProduct.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbProduct WHERE CONCAT(pname,ptype,pcategory) LIKE '%" + txtSearch.Text + "%'", cn);
            cn.Open(); 
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            dr.Close();
            cn.Close();
        }
        #endregion Method
    }
}
