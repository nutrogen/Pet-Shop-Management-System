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
    public partial class CashForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        SqlDataReader dr;
        string title = "C# Pet Shop";
        MainForm main; // 3:43
        public CashForm(MainForm form)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            main = form; // 3:43
            getTransno();
            loadCash();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CashProduct product = new CashProduct(this);
            product.uname = main.LblUsername.Text; // 3:43
            product.ShowDialog();
        }

        private void btnCash_Click(object sender, EventArgs e) // 4:51
        {
            CashCustomer customer = new CashCustomer(this); // 4:56 Concat error
            customer.ShowDialog();
            main.loadDailySale(); // 5:43 added

            if (MessageBox.Show("Would you like to proceed to pay?", "Payment", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                getTransno();
                for(int i=0; i<dgvCash.Rows.Count; i++)
                {
                    dbcon.excuteQuery("UPDATE tbProduct SET pqty=pqty -"+int.Parse(dgvCash.Rows[i].Cells[4].Value.ToString()) +" WHERE pcode LIKE "+ dgvCash.Rows[i].Cells[2].Value.ToString()+ ""); // 4:54 RowIndex error. This is solved by using i. see the video.
                }
                dgvCash.Rows.Clear();
            }
        }

        private void dgvCash_CellContentClick(object sender, DataGridViewCellEventArgs e) // 4:00
        {
            string colName = dgvCash.Columns[e.ColumnIndex].Name;
            removeitem: // 4:09
            if(colName == "Delete")
            {
                if (MessageBox.Show("Are you sure want to delete this data?", "Delete data", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // 4:01 copied from UserForm
                {
                    dbcon.excuteQuery("DELETE FROM tbCash WHERE cashid LIKE '" + dgvCash.Rows[e.RowIndex].Cells[1].Value.ToString() + "'"); 
                    MessageBox.Show("Payment data has been sucessfully deleted!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (colName == "Increase") // 4:05 quantity control part. 4:06 Qutntity control does not work. So these 2 else if statements below were cut and pasted out of the existing if code block.
            {
                int i = checkPqty(dgvCash.Rows[e.RowIndex].Cells[2].Value.ToString()); // 4:14. checkQuantity function is down below.
                if(int.Parse(dgvCash.Rows[e.RowIndex].Cells[4].Value.ToString()) < i) // 4:15. 
                {
                    dbcon.excuteQuery("UPDATE tbCash SET qty = qty + " + 1 + " WHERE cashid LIKE '" + dgvCash.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");
                }
                else
                {
                    MessageBox.Show("Available quantity : " + i + "!", "Out of Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else if (colName == "Decrease")
            {
                if(int.Parse(dgvCash.Rows[e.RowIndex].Cells[4].Value.ToString()) == 1) // 4:08 Prevents negative quantity
                {
                    colName = "Delete";
                    goto removeitem; // 4:09
                }
                dbcon.excuteQuery("UPDATE tbCash SET qty = qty - " + 1 + " WHERE cashid LIKE '" + dgvCash.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");
            }
            loadCash();
        }

        // 4:00 cut and pasted here
        #region Method
        public void getTransno() // 3:36 this was created in orde to solve the running error at 3:33. See the video. 
        {
            try
            {
                string sdate = DateTime.Now.ToString("yyyyMMdd");
                int count;
                string transno;

                cn.Open();
                cm = new SqlCommand("SELECT TOP 1 transno FROM tbCash WHERE transno LIKE '" + sdate + "%' ORDER BY cashid DESC", cn); // 3:55 syntex error ommitting transno 
                dr = cm.ExecuteReader();
                dr.Read();

                if (dr.HasRows)
                {
                    transno = dr[0].ToString();
                    count = int.Parse(transno.Substring(8, 4));
                    lblTransno.Text = sdate + (count + 1);
                }
                else
                {
                    transno = sdate + "1001";
                    lblTransno.Text = transno;
                }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }
        }

        public void loadCash() // 3:50
        {
            try
            {
                int i = 0;
                double total = 0; // 4:18 to calculate payment
                dgvCash.Rows.Clear();
                cm = new SqlCommand("SELECT cashid,pcode,pname,qty,price,total,c.name,cashier from tbCash as cash LEFT JOIN tbCustomer c ON cash.cid = c.id WHERE transno LIKE " + lblTransno.Text + "", cn);
                cn.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvCash.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());
                    total += double.Parse(dr[5].ToString()); // 4:19
                }
                dr.Close();
                cn.Close();
                lblTotal.Text = total.ToString("#,##0.00");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        public int checkPqty(string pcode) // 4:11 Prevents negative quantity
        {
            int i = 0;
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT pqty FROM tbProduct WHERE pcode LIKE '"+pcode+"'", cn);
                i = int.Parse(cm.ExecuteScalar().ToString()); // To learn executeScalar
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message,title);
            }
            return i;
        }
        #endregion Method
    }
}
