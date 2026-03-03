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
    public partial class Expense : System.Web.UI.Page 
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        int UserId =>Convert.ToInt32(Session["UserId"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Pages/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                calExpenseDate.SelectedDate = DateTime.Today;
                LoadCategories();
                LoadMonthYear();
                LoadExpenses();
                LoadTotalExpense();
            }

        }

        void LoadCategories()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"SELECT CategoryId, CategoryName FROM Categories WHERE UserId = @UserId OR UserId IS NULL", con);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                con.Open();
                ddlCategory.DataSource = cmd.ExecuteReader();
                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryId";
                ddlCategory.DataBind();
            }

            ddlCategory.Items.Insert(0,new System.Web.UI.WebControls.ListItem("Select Category", ""));
        }
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isOther = ddlCategory.SelectedItem.Text == "Other";
            lblOtherCategory.Visible = isOther;
            txtOtherCategory.Visible = isOther;
        }


        void LoadMonthYear()
        {
            ddlYear.Items.Clear();
            int startYear = 2026;
            int endYear = DateTime.Now.Year + 10;
            for (int year = startYear; year <= endYear; year++)
            {
                ddlYear.Items.Add(year.ToString());
            }
            ddlYear.SelectedValue = DateTime.Now.Year.ToString();
            ddlMonth.SelectedValue = DateTime.Now.Month.ToString(); 
        }


        protected void btnAddExpense_Click(object sender, EventArgs e)
        {
            if (ddlCategory.SelectedIndex == 0 || txtAmount.Text == "")
            {
                lblMessage.Text = "Please select category and enter amount.";
                return;
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"INSERT INTO Expense (UserId, CategoryId, Amount, ExpenseDate, Description) VALUES (@UserId, @CategoryId, @Amount, @Date, @Desc)", con);

                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Parameters.AddWithValue("@CategoryId", ddlCategory.SelectedValue);
                cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
                cmd.Parameters.AddWithValue("@Date", calExpenseDate.SelectedDate);
                cmd.Parameters.AddWithValue("@Desc", txtDescription.Text);

                con.Open();
                cmd.ExecuteNonQuery();
             
                CheckBudgetExceeded();

            }

            lblMessage.Text = "Expense added successfully ";
            txtOtherCategory.Text = "";
            LoadExpenses();
            LoadTotalExpense();
        }

        void LoadExpenses()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    @"SELECT 
                        E.ExpenseId,
                        C.CategoryName AS Category,
                        E.Amount,
                        E.ExpenseDate
                      FROM Expense E
                      INNER JOIN Categories C ON E.CategoryId = C.CategoryId
                      WHERE E.UserId = @UserId
                        AND MONTH(E.ExpenseDate) = @Month
                        AND YEAR(E.ExpenseDate) = @Year
                      ORDER BY E.ExpenseDate DESC", con);

                da.SelectCommand.Parameters.AddWithValue("@UserId", UserId);
                da.SelectCommand.Parameters.AddWithValue("@Month", ddlMonth.SelectedValue);
                da.SelectCommand.Parameters.AddWithValue("@Year", ddlYear.SelectedValue);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvExpenses.DataSource = dt;
                gvExpenses.DataBind();
            }
        }
        void LoadTotalExpense()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"SELECT ISNULL(SUM(Amount),0) FROM Expense WHERE UserId = @UserId AND MONTH(ExpenseDate) = MONTH(GETDATE()) AND YEAR(ExpenseDate) = YEAR(GETDATE())", con);

                cmd.Parameters.AddWithValue("@UserId", UserId);

                con.Open();
                lblTotalExpense.Text = "₹ " + cmd.ExecuteScalar().ToString();
            }
        }
        protected void calExpenseDate_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.IsOtherMonth)
            {
                e.Cell.Controls.Clear();   // remove the date number
                e.Cell.Text = "";          // blank cell
                e.Cell.Enabled = false;    // disable click
                e.Cell.BackColor = System.Drawing.Color.Transparent;
            }

        }
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            LoadExpenses();
        }

        protected void gvExpenses_RowDeleting(object sender,System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int expenseId = Convert.ToInt32(gvExpenses.DataKeys[e.RowIndex].Value);

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Expense WHERE ExpenseId = @Id AND UserId = @UserId", con);

                cmd.Parameters.AddWithValue("@Id", expenseId);
                cmd.Parameters.AddWithValue("@UserId", UserId);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            LoadExpenses();
            LoadTotalExpense();
        }
        void AddNotification(int userId, string message)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Notifications (UserId, Message, CreatedAt, IsRead) " + "VALUES (@UserId, @Message, GETDATE(), 0)", con);

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Message", message);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        void CheckBudgetExceeded()
        {
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;

            decimal totalExpense = 0;
            decimal budgetAmount = 0;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                // Total Expense
                SqlCommand expCmd = new SqlCommand(@"SELECT ISNULL(SUM(Amount),0) FROM Expense WHERE UserId=@UserId AND MONTH(ExpenseDate)=@Month AND YEAR(ExpenseDate)=@Year", con);

                expCmd.Parameters.AddWithValue("@UserId", UserId);
                expCmd.Parameters.AddWithValue("@Month", month);
                expCmd.Parameters.AddWithValue("@Year", year);

                totalExpense = Convert.ToDecimal(expCmd.ExecuteScalar());

                // Budget
                SqlCommand budCmd = new SqlCommand(@"SELECT ISNULL(Amount,0) FROM Budgets WHERE UserId=@UserId AND Month=@Month AND Year=@Year", con);

                budCmd.Parameters.AddWithValue("@UserId", UserId);
                budCmd.Parameters.AddWithValue("@Month", month);
                budCmd.Parameters.AddWithValue("@Year", year);

                object result = budCmd.ExecuteScalar();
                if (result != null)
                    budgetAmount = Convert.ToDecimal(result);
            }
            if (budgetAmount > 0 && totalExpense > budgetAmount)
            {
                AddNotification( UserId," Expense exceeded monthly budget by Rs." + (totalExpense - budgetAmount));
            }
        }
    }
}