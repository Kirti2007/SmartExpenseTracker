using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartExpenseTracker.Pages
{
    public partial class SmartInsights : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GenerateInsights();
            }
        }
        void GenerateInsights()
        {
            int userId = Convert.ToInt32(Session["UserID"]);

            decimal totalIncome = GetSingleDecimal("SELECT ISNULL(SUM(Amount),0) FROM Income WHERE UserId=@uid", userId);
            decimal totalExpense = GetSingleDecimal("SELECT ISNULL(SUM(Amount),0) FROM Expense WHERE UserId=@uid", userId);
            decimal budget = GetSingleDecimal("SELECT ISNULL(Amount,0) FROM Budgets WHERE UserId=@uid AND Month=MONTH(GETDATE()) AND Year=YEAR(GETDATE())", userId);

            // 1️⃣ Monthly Trend
            lblMonthlyTrend.Text = "📊 This month, you have spent ₹" + totalExpense +
                " which is " + (totalExpense > (budget * 0.7m) ? "high" : "under control") + ".";

            // 2️⃣ Budget Warning
            if (budget > 0 && totalExpense >= budget * 0.9m)
                lblBudgetWarning.Text = "⚠ Warning: You have used more than 90% of your monthly budget.";
            else
                lblBudgetWarning.Text = "✅ Your spending is within the safe budget limit.";

            // 3️⃣ Savings Tip
            decimal savings = totalIncome - totalExpense;
            lblSavingsTip.Text = "💰 You have saved ₹" + savings +
                ". Try to invest at least 20% of your savings for future growth.";

            // 4️⃣ Highest Spending Category
            lblTopCategory.Text = "🔥 Highest spending category: " + GetTopCategory(userId);

            // 5️⃣ Expense Control Suggestion
            lblExpenseControl.Text = totalExpense > totalIncome * 0.7m ?
                "🚨 Your expenses are high compared to income. Reduce unnecessary spending." :
                "🎯 Good job! You are maintaining healthy expense control.";
        }

        decimal GetSingleDecimal(string query, int uid)
        {
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@uid", uid);

            con.Open();
            decimal val = Convert.ToDecimal(cmd.ExecuteScalar());
            con.Close();

            return val;
        }

        string GetTopCategory(int uid)
        {
            SqlCommand cmd = new SqlCommand(@"
                SELECT TOP 1 C.CategoryName
                FROM Expense E
                JOIN Categories C ON E.CategoryId = C.CategoryId
                WHERE E.UserId = @uid
                GROUP BY C.CategoryName
                ORDER BY SUM(E.Amount) DESC", con);

            cmd.Parameters.AddWithValue("@uid", uid);

            con.Open();
            object result = cmd.ExecuteScalar();
            con.Close();

            return result == null ? "No data" : result.ToString();
        }
    }
}