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
    public partial class Budgets : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Pages/Login.aspx");
            }
            if (!IsPostBack)
            {
                LoadYears();
                SetCurrentMonthYear();
                LoadMonthlyBudget();
                LoadBudgetSummary();
                LoadCategoryWiseExpense();
            }
        }
        //This will load year dropdown
        private void LoadYears()
        {
            ddlYear.Items.Clear();
            int startYear = 2000;
            int endYear = DateTime.Now.Year + 10;
            for(int year=startYear; year <= endYear; year++)
            {
                ddlYear.Items.Add(year.ToString());
            }
            ddlYear.SelectedValue = DateTime.Now.Year.ToString();//Automatically selects current year by default
        }
        //This will automatically set current month
        private void SetCurrentMonthYear()
        {
            ddlMonth.SelectedValue = DateTime.Now.Month.ToString();
        }
        //Save or update budgets
        protected void btnSaveBudget_Click1(object sender, EventArgs e)
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            int month = Convert.ToInt32(ddlMonth.SelectedValue);
            int year = Convert.ToInt32(ddlYear.SelectedValue);
            decimal amount = Convert.ToDecimal(txtBudgetAmount.Text);
            using(SqlConnection con=new SqlConnection(conStr))
            {
                string query = @"SELECT COUNT(*) FROM Budgets WHERE UserId=@UserId AND Month=@Month AND Year=@Year";
                SqlCommand checkCmd = new SqlCommand(query, con);
                checkCmd.Parameters.AddWithValue("@UserId", userId);
                checkCmd.Parameters.AddWithValue("@Month", month);
                checkCmd.Parameters.AddWithValue("@Year", year);
                con.Open();
                int count = (int)checkCmd.ExecuteScalar();
                if (count > 0)
                {
                    //update
                    string updateQuery = @"UPDATE Budgets 
                                           SET Amount=@Amount 
                                           WHERE UserId=@UserId AND Month=@Month AND Year=@Year";

                    SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                    updateCmd.Parameters.AddWithValue("@Amount", amount);
                    updateCmd.Parameters.AddWithValue("@UserId", userId);
                    updateCmd.Parameters.AddWithValue("@Month", month);
                    updateCmd.Parameters.AddWithValue("@Year", year);
                    updateCmd.ExecuteNonQuery();
                    AddNotification(userId,"Monthly budget updated to ₹" + amount +" for " + ddlMonth.SelectedItem.Text + " " + ddlYear.SelectedValue);
                }
                else
                {
                    //Insert
                    string insertQuery = @"INSERT INTO Budgets(UserId, Month, Year, Amount, CreatedAt)
                                           VALUES(@UserId, @Month, @Year, @Amount, GETDATE())";

                    SqlCommand insertCmd = new SqlCommand(insertQuery, con);
                    insertCmd.Parameters.AddWithValue("@UserId", userId);
                    insertCmd.Parameters.AddWithValue("@Month", month);
                    insertCmd.Parameters.AddWithValue("@Year", year);
                    insertCmd.Parameters.AddWithValue("@Amount", amount);
                    insertCmd.ExecuteNonQuery();
                    AddNotification(userId, "Monthly budget set: ₹" + amount + " for " + ddlMonth.SelectedItem.Text + " " + ddlYear.SelectedValue);
                }
            }
            LoadMonthlyBudget();
            LoadBudgetSummary();
            LoadCategoryWiseExpense();
        }
        //Load montly budget
        private void LoadMonthlyBudget()
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            int month = Convert.ToInt32(ddlMonth.SelectedValue);
            int year = Convert.ToInt32(ddlYear.SelectedValue);

            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"SELECT Amount FROM Budgets 
                                 WHERE UserId=@UserId AND Month=@Month AND Year=@Year";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);

                con.Open();
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    lblMonthlyBudget.Text = "₹ " + result.ToString();
                }
                else
                {
                    lblMonthlyBudget.Text = "₹ 0";
                }
            }
        }

        //Budget summary
        private void LoadBudgetSummary()
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            int month = Convert.ToInt32(ddlMonth.SelectedValue);
            int year = Convert.ToInt32(ddlYear.SelectedValue);

            decimal totalBudget = 0;
            decimal totalExpense = 0;

            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                // Budget
                SqlCommand budgetCmd = new SqlCommand(
                    @"SELECT ISNULL(SUM(Amount),0) FROM Budgets 
                      WHERE UserId=@UserId AND Month=@Month AND Year=@Year", con);

                budgetCmd.Parameters.AddWithValue("@UserId", userId);
                budgetCmd.Parameters.AddWithValue("@Month", month);
                budgetCmd.Parameters.AddWithValue("@Year", year);

                totalBudget = Convert.ToDecimal(budgetCmd.ExecuteScalar());

                // Expense
                SqlCommand expenseCmd = new SqlCommand(
                    @"SELECT ISNULL(SUM(Amount),0) FROM Expense 
                      WHERE UserId=@UserId AND MONTH(ExpenseDate)=@Month 
                      AND YEAR(ExpenseDate)=@Year", con);

                expenseCmd.Parameters.AddWithValue("@UserId", userId);
                expenseCmd.Parameters.AddWithValue("@Month", month);
                expenseCmd.Parameters.AddWithValue("@Year", year);

                totalExpense = Convert.ToDecimal(expenseCmd.ExecuteScalar());
            }

            lblTotalBudget.Text = "₹ " + totalBudget;
            lblTotalExpense.Text = "₹ " + totalExpense;
            decimal remaining = totalBudget - totalExpense;

            if (remaining < 0)
            {
                lblRemainingBudget.Text = "Over Budget by ₹ " + Math.Abs(remaining);
                lblRemainingBudget.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                lblRemainingBudget.Text = "₹ " + remaining;
                lblRemainingBudget.ForeColor = System.Drawing.Color.Green;
            }
        }
        //Category wise expenses

        private void LoadCategoryWiseExpense()
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            int month = Convert.ToInt32(ddlMonth.SelectedValue);
            int year = Convert.ToInt32(ddlYear.SelectedValue);

            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"SELECT c.CategoryName,
                                SUM(e.Amount) AS SpentAmount
                                FROM Expense e
                                INNER JOIN Categories c ON e.CategoryId = c.CategoryId
                                WHERE e.UserId=@UserId
                                AND MONTH(e.ExpenseDate)=@Month
                                AND YEAR(e.ExpenseDate)=@Year
                                GROUP BY c.CategoryName";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvCategoryBudget.DataSource = dt;
                gvCategoryBudget.DataBind();
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
    }
}