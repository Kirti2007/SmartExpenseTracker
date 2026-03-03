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
                int startYear = 2026;
                int endYear = startYear + 10;

                for (int y = startYear; y <= endYear; y++)
                {
                    ddlYear.Items.Add(y.ToString());
                }
                ddlMonth.SelectedValue = DateTime.Now.Month.ToString();
                ddlYear.SelectedValue = DateTime.Now.Year.ToString();
            }
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            int month = Convert.ToInt32(ddlMonth.SelectedValue);
            int year = Convert.ToInt32(ddlYear.SelectedValue);

            decimal income = 0;
            decimal expense = 0;
            decimal budget = 0;

            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();

                // GET TOTAL INCOME
                SqlCommand cmdIncome = new SqlCommand("SELECT ISNULL(SUM(Amount),0) FROM Income WHERE UserId=@uid AND MONTH(IncomeDate)=@m AND YEAR(IncomeDate)=@y",con);

                cmdIncome.Parameters.AddWithValue("@uid", userId);
                cmdIncome.Parameters.AddWithValue("@m", month);
                cmdIncome.Parameters.AddWithValue("@y", year);

                income = Convert.ToDecimal(cmdIncome.ExecuteScalar());


                // GET TOTAL EXPENSE
                SqlCommand cmdExpense = new SqlCommand("SELECT ISNULL(SUM(Amount),0) FROM Expense WHERE UserId=@uid AND MONTH(ExpenseDate)=@m AND YEAR(ExpenseDate)=@y",con);

                cmdExpense.Parameters.AddWithValue("@uid", userId);
                cmdExpense.Parameters.AddWithValue("@m", month);
                cmdExpense.Parameters.AddWithValue("@y", year);

                expense = Convert.ToDecimal(cmdExpense.ExecuteScalar());


                // DISPLAY VALUES
                lblIncome.Text = "₹ " + income.ToString("N2");
                lblExpense.Text = "₹ " + expense.ToString("N2");
                lblSavings.Text = "₹ " + (income - expense).ToString("N2");


                // BAR CHART (Income vs Expense)
                ChartBar.Series["Finance"].Points.Clear();
                ChartBar.Series["Finance"].Points.AddXY("Income", income);
                ChartBar.Series["Finance"].Points.AddXY("Expense", expense);
                ChartBar.DataBind();


                // IE CHART (Category-wise Expense)
                ChartPie.Series["Category"].Points.Clear();

                SqlCommand cmdPie = new SqlCommand(
                    @"SELECT c.CategoryName, SUM(e.Amount)
                    FROM Expense e
                    JOIN Categories c ON e.CategoryId = c.CategoryId
                    WHERE e.UserId=@uid AND MONTH(e.ExpenseDate)=@m AND YEAR(e.ExpenseDate)=@y
                    GROUP BY c.CategoryName",con);

                cmdPie.Parameters.AddWithValue("@uid", userId);
                cmdPie.Parameters.AddWithValue("@m", month);
                cmdPie.Parameters.AddWithValue("@y", year);

                SqlDataReader dr = cmdPie.ExecuteReader();

                while (dr.Read())
                {
                    ChartPie.Series["Category"].Points.AddXY(dr["CategoryName"].ToString(),Convert.ToDecimal(dr[1]));
                }
                dr.Close();
                ChartPie.DataBind();


                // GET BUDGET
                SqlCommand cmdBudget = new SqlCommand("SELECT ISNULL(SUM(Amount),0) FROM Budgets WHERE UserId=@uid AND Month=@m AND Year=@y",con);

                cmdBudget.Parameters.AddWithValue("@uid", userId);
                cmdBudget.Parameters.AddWithValue("@m", month);
                cmdBudget.Parameters.AddWithValue("@y", year);
                budget = Convert.ToDecimal(cmdBudget.ExecuteScalar());

                // CHECK BUDGET STATUS
                decimal difference = expense - budget;

                if (difference > 0)
                {
                    lblBudgetStatus.Text = "Over Budget by ₹ " + difference.ToString("N2");
                }
                else if (difference < 0)
                {
                    lblBudgetStatus.Text = "Under Budget by ₹ " + Math.Abs(difference).ToString("N2");
                }
                else
                {
                    lblBudgetStatus.Text = "Budget Exactly Utilized";
                }
            }

        }
    }

}