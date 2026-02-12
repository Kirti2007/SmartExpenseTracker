using System;
using System.Data.SqlClient;
using System.Configuration;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartExpenseTracker.Pages
{
    public partial class Report : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                for (int y = 2020; y <= DateTime.Now.Year; y++)
                    ddlYear.Items.Add(y.ToString());

                ddlMonth.SelectedValue = DateTime.Now.Month.ToString();
                ddlYear.SelectedValue = DateTime.Now.Year.ToString();


            }
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            int userId = GetUserId();
            int month = Convert.ToInt32(ddlMonth.SelectedValue);
            int year = Convert.ToInt32(ddlYear.SelectedValue);

            decimal income = 0, expense = 0, budget = 0;

            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();

                income = ExecuteScalar(con,
                    "SELECT ISNULL(SUM(Amount),0) FROM Income WHERE UserId=@uid AND MONTH(IncomeDate)=@m AND YEAR(IncomeDate)=@y",
                    userId, month, year);

                expense = ExecuteScalar(con,
                    "SELECT ISNULL(SUM(Amount),0) FROM Expense WHERE UserId=@uid AND MONTH(ExpenseDate)=@m AND YEAR(ExpenseDate)=@y",
                    userId, month, year);

                lblIncome.Text = "₹ " + income.ToString("N2");
                lblExpense.Text = "₹ " + expense.ToString("N2");
                lblSavings.Text = "₹ " + (income - expense).ToString("N2");

                // BAR CHART
                ChartBar.Series["Finance"].Points.Clear();
                ChartBar.Series["Finance"].Points.AddXY("Income", income);
                ChartBar.Series["Finance"].Points.AddXY("Expense", expense);
                ChartBar.DataBind();

                // PIE CHART
                ChartPie.Series["Category"].Points.Clear();

                SqlCommand cmdPie = new SqlCommand(
                    @"SELECT c.CategoryName, SUM(e.Amount)
                      FROM Expense e
                      JOIN Categories c ON e.CategoryId = c.CategoryId
                      WHERE e.UserId=@uid AND MONTH(e.ExpenseDate)=@m AND YEAR(e.ExpenseDate)=@y
                      GROUP BY c.CategoryName", con);

                cmdPie.Parameters.AddWithValue("@uid", userId);
                cmdPie.Parameters.AddWithValue("@m", month);
                cmdPie.Parameters.AddWithValue("@y", year);

                SqlDataReader dr = cmdPie.ExecuteReader();
                while (dr.Read())
                {
                    ChartPie.Series["Category"].Points.AddXY(
                        dr[0].ToString(),
                        Convert.ToDecimal(dr[1]));
                }
                dr.Close();
                ChartPie.DataBind();

                // BUDGET
                budget = ExecuteScalar(con,
                    "SELECT ISNULL(SUM(Amount),0) FROM Budgets WHERE UserId=@uid AND Month=@m AND Year=@y",
                    userId, month, year);

                lblBudgetStatus.Text = expense > budget
                    ? "Over Budget by ₹ " + (expense - budget).ToString("N2")
                    : "Under Budget";
            }
        }

        private decimal ExecuteScalar(SqlConnection con, string query, int uid, int m, int y)
        {
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@uid", uid);
            cmd.Parameters.AddWithValue("@m", m);
            cmd.Parameters.AddWithValue("@y", y);
            return Convert.ToDecimal(cmd.ExecuteScalar());
        }

        private int GetUserId()
        {
            if (Session["UserId"] != null)
                return Convert.ToInt32(Session["UserId"]);

           
            return 1;   // change to real logged-in user id
        }
    }
           
    
}