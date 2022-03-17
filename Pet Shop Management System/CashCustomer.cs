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
    public partial class CashCustomer : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        SqlDataReader dr;
        string title = "C# Pet Shop";
        CashForm cash; // 4:48 added in order to implement dgvCustomer_CellContentClick fn
        public CashCustomer(CashForm form) // 4:48
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            cash = form; // 4:48
            LoadCustomer();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadCustomer();
        }

        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e) // 4:47
        {
            string colName = dgvCustomer.Columns[e.ColumnIndex].Name;
            if(colName =="Choice")
            {
                dbcon.excuteQuery("UPDATE tbCash SET cid="+dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString()+"WHERE transno=" +cash.lblTransno.Text+"");
                cash.loadCash();
                this.Dispose();
            }
        }

        #region Method
        public void LoadCustomer() // 4:44 copied from CustomerForm and insert the core codes in the try statement. See the video.
        {
           try
            {
                int i = 0;
                dgvCustomer.Rows.Clear();
                cm = new SqlCommand("SELECT id,name,phone FROM tbCustomer WHERE name LIKE '%" + txtSearch.Text + "%'", cn);
                cn.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvCustomer.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
                }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }
        #endregion Method
    }
}
