using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                //Total Income (Current Month)
                SqlCommand cmdIncome = new SqlCommand(@"SELECT ISNULL(SUM(Amount),0) FROM Income WHERE UserId = @UserId AND MONTH(IncomeDate) = @Month  AND YEAR(IncomeDate) = @Year", con);

                cmdIncome.Parameters.AddWithValue("@UserId", userId);
                cmdIncome.Parameters.AddWithValue("@Month", month);
                cmdIncome.Parameters.AddWithValue("@Year", year);

                decimal totalIncome = Convert.ToDecimal(cmdIncome.ExecuteScalar());

                // Total Expense (Current Month)
                SqlCommand cmdExpense = new SqlCommand(@"SELECT ISNULL(SUM(Amount),0) FROM Expense WHERE UserId = @UserId AND MONTH(ExpenseDate) = @Month AND YEAR(ExpenseDate) = @Year", con);

                cmdExpense.Parameters.AddWithValue("@UserId", userId);
                cmdExpense.Parameters.AddWithValue("@Month", month);
                cmdExpense.Parameters.AddWithValue("@Year", year);

                decimal totalExpense = Convert.ToDecimal(cmdExpense.ExecuteScalar());

                // Current Month Budget 
                SqlCommand cmdBudget = new SqlCommand(@"SELECT ISNULL(Amount,0) FROM Budgets WHERE UserId = @UserId AND Month = @Month AND Year = @Year", con);

                cmdBudget.Parameters.AddWithValue("@UserId", userId);
                cmdBudget.Parameters.AddWithValue("@Month", month);
                cmdBudget.Parameters.AddWithValue("@Year", year);

                object result = cmdBudget.ExecuteScalar();
                decimal totalBudget = result != null ? Convert.ToDecimal(result) : 0;

                decimal remainingBudget = totalBudget - totalExpense;
                decimal savings = totalIncome - totalExpense;

                // Assign Values
                lblTotalIncome.Text = "₹ " + totalIncome.ToString("N2");
                lblTotalExpense.Text = "₹ " + totalExpense.ToString("N2");
                lblRemainingBudget.Text = "₹ " + remainingBudget.ToString("N2");
                lblSavings.Text = "₹ " + savings.ToString("N2");

                // Financial Health Calculation
                decimal percent = 0;

                if (totalIncome > 0)
                    percent = (savings * 100) / totalIncome;

                lblHealthPercent.Text = percent.ToString("N1") + "%";

                if (percent > 30)
                    lblHealthStatus.Text = "Excellent";
                else if (percent >= 10)
                    lblHealthStatus.Text = "Good";
                else
                    lblHealthStatus.Text = "Poor";
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
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                    SELECT TOP 10
                        FORMAT(E.ExpenseDate, 'dd-MM-yyyy') AS TransactionDate,
                        C.CategoryName,
                        E.Amount,
                        'Expense' AS Type
                    FROM Expense E
                    INNER JOIN Categories C ON E.CategoryId = C.CategoryId
                    WHERE E.UserId = @UserId
                    AND MONTH(E.ExpenseDate) = @Month
                    AND YEAR(E.ExpenseDate) = @Year
                    ORDER BY E.ExpenseDate DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvTransactions.DataSource = dt;
                gvTransactions.DataBind();
            }
        }
    }
}