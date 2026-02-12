using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartExpenseTracker.Pages
{
    public partial class Income : System.Web.UI.Page
    {
        string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                calIncomeDate.SelectedDate = DateTime.Today;
                LoadIncome();
                LoadMonthlyIncome();
            }

        }


       

        protected void ddlSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCustomSource.Visible = ddlSource.SelectedValue.Equals("Other");
        }

        protected void btnAddIncome_Click(object sender, EventArgs e)
        {
            string source = ddlSource.SelectedValue.Equals("Other") ? txtCustomSource.Text.Trim() : ddlSource.SelectedValue;
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(txtAmount.Text))
            {
                return;
            }
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Income(UserId,IncomeSource,Amount,IncomeDate,Description,CreatedAt)" + "VALUES (@UserId,@Source,@Amount,@Date,@Description,GETDATE())", con);

                cmd.Parameters.AddWithValue("@UserId",Convert.ToInt32(Session["UserId"]));
                cmd.Parameters.AddWithValue("@Source", source);
                cmd.Parameters.AddWithValue("@Amount", Convert.ToDecimal(txtAmount.Text));
                cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                cmd.Parameters.AddWithValue("@Date", calIncomeDate.SelectedDate);

                con.Open();
                cmd.ExecuteNonQuery();
               
                AddNotification( Convert.ToInt32(Session["UserId"]),"Income added: ₹" + txtAmount.Text + " from " + source );
            }
            ClearForm();
            LoadIncome();
            LoadMonthlyIncome();
        }
        void LoadIncome()
        {
            using(SqlConnection con=new SqlConnection(conStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT IncomeSource,Amount,IncomeDate FROM Income ORDER BY CreatedAt DESC", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvIncome.DataSource = dt;
                gvIncome.DataBind();
            }
        }

        void LoadMonthlyIncome()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT ISNULL(SUM(Amount),0) FROM Income " +
                    "WHERE MONTH(IncomeDate) = MONTH(GETDATE()) " +
                    "AND YEAR(IncomeDate) = YEAR(GETDATE())", con);

                con.Open();
                lblMonthlyIncome.Text = "₹ " + cmd.ExecuteScalar().ToString();
            }
        }

        protected void calIncomeDate_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.IsOtherMonth)
            {
                e.Cell.Controls.Clear();   // remove the date number
                e.Cell.Text = "";          // blank cell
                e.Cell.Enabled = false;    // disable click
                e.Cell.BackColor = System.Drawing.Color.Transparent;
            }

        }

        void AddNotification(int userId, string message)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Notifications (UserId, Message, CreatedAt, IsRead) " +
                    "VALUES (@UserId, @Message, GETDATE(), 0)", con);

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Message", message);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        void ClearForm()
        {
            ddlSource.SelectedIndex = 0;
            txtCustomSource.Text = "";
            txtCustomSource.Visible = false;
            txtAmount.Text = "";
            txtDescription.Text = "";
            calIncomeDate.SelectedDate = DateTime.Today;
        } 
    }
}

            