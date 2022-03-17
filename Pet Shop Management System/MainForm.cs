using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Pet_Shop_Management_System
{
    public partial class MainForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        SqlDataReader dr;
        string title = "C# Pet Shop";
        public MainForm()
        {
            InitializeComponent();
            btnDashboard.PerformClick(); // After logging, dashboard shows first automatically.
            cn = new SqlConnection(dbcon.connection());
            loadDailySale();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Would you like to exit the application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // 5:04 copied from LoginForm
            {
                Application.Exit();
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            openChldForm(new Dashboard());
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            openChldForm(new CustomerForm());
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            openChldForm(new UserForm());
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            openChldForm(new ProductForm());
        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            openChldForm(new CashForm(this));// 3:46 red error line
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Would you like to end the application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                LoginForm login = new LoginForm();
                this.Dispose();
                login.ShowDialog();
            }
        }

        private void MainForm_Load(object sender, EventArgs e) // 5:49
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            progress.Invoke((MethodInvoker)delegate // 5:52
            {
                progress.Text = DateTime.Now.ToString("hh:mm:ss");
                progress.Value = Convert.ToInt32(DateTime.Now.Second);
            });
        }

        #region Method
        private Form activeForm = null;
        public void openChldForm(Form childForm) // 1:47
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            lblTitle.Text = childForm.Text;
            panelChild.Controls.Add(childForm);
            panelChild.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        public void loadDailySale() // 5:39
        {
            string sdate = DateTime.Now.ToString("yyyyMMdd");
            try // copied from Dashboard codes in MainForm
            {
                cn.Open();
                cm = new SqlCommand("SELECT ISNULL(SUM(total),0) AS total FROM tbCash WHERE transno LIKE '" + sdate + "%'", cn);
                lblDailySale.Text = double.Parse(cm.ExecuteScalar().ToString()).ToString("#,##0.00"); // 5:41 Can not convert type double to string error. Just add ToString();
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }
        #endregion Method
    }
}
