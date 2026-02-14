using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartExpenseTracker.Pages
{
    public partial class Dashboard : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            if (!IsPostBack)
            {
                LoadSummaryCards();
                LoadRecentTransactions();
            }
        }
        private void LoadSummaryCards()
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                //Total Incomee
                SqlCommand cmdIncome = new SqlCommand("SELECT ISNULL(CAST(SUM(Amount) AS DECIMAL(10,2)), 0) FROM Income WHERE UserId = @UserId",con);
                cmdIncome.Parameters.AddWithValue("@UserId", userId);
                decimal totalIncome = Convert.ToDecimal(cmdIncome.ExecuteScalar());

                //Total Expense
                SqlCommand cmdExpense = new SqlCommand("SELECT ISNULL(CAST(SUM(Amount) AS DECIMAL(10,2)), 0) FROM Expense WHERE UserId = @UserId",con);
                cmdExpense.Parameters.AddWithValue("@UserId", userId);
                decimal totalExpense = Convert.ToDecimal(cmdExpense.ExecuteScalar());

                //Total Budget
                SqlCommand cmdBudget = new SqlCommand("SELECT ISNULL(CAST(SUM(Amount) AS DECIMAL(10,2)), 0) FROM Budgets WHERE UserId = @UserId",con);
                cmdBudget.Parameters.AddWithValue("@UserId", userId);
                decimal totalBudget = Convert.ToDecimal(cmdBudget.ExecuteScalar());

                decimal remainingBudget = totalBudget - totalExpense;
                decimal savings = totalIncome - totalExpense;

                lblTotalIncome.Text = "₹ " + totalIncome.ToString("N2");
                lblTotalExpense.Text = "₹ " + totalExpense.ToString("N2");
                lblRemainingBudget.Text = "₹ " + remainingBudget.ToString("N2");
                lblSavings.Text = "₹ " + savings.ToString("N2");

                //Finanacial Health of user
                decimal percent = 0;
                if (totalIncome > 0)
                    percent = (savings * 100) / totalIncome;
                lblHealthPercent.Text = percent.ToString("N1") + "%";
                if (percent > 30)
                {
                    lblHealthStatus.Text = "Excellent";
                }
                else if (percent >= 10)
                {
                    lblHealthStatus.Text = "Good";
                }
                else
                {
                    lblHealthStatus.Text = "Poor";
                }
            }
        }
        protected void gvTransactions_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTransactions.PageIndex = e.NewPageIndex;
            LoadRecentTransactions();
        }

        private void LoadRecentTransactions()
        {
            int userId = Convert.ToInt32(Session["UserId"]);

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                SELECT 
                    FORMAT(E.ExpenseDate, 'dd-MM-yyyy') AS TransactionDate,
                    C.CategoryName,
                    E.Amount,
                    'Expense' AS Type
                FROM Expense E
                INNER JOIN Categories C ON E.CategoryId = C.CategoryId
                WHERE E.UserId = @UserId
                ORDER BY E.ExpenseDate DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvTransactions.DataSource = dt;
                gvTransactions.DataBind();
            }
        }
    }
}


   