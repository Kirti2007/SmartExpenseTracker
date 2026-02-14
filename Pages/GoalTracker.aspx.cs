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
    public partial class GoalTracker : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                calTargetDate.SelectedDate = DateTime.Today;
                LoadGoals();

            }
        }
        protected void calTargetDate_SelectionChanged(object sender, EventArgs e)
        {
            txtTargetDate.Text = calTargetDate.SelectedDate.ToString("yyyy-MM-dd");
        }

        protected void btnSaveGoal_Click(object sender, EventArgs e)
        {
            int userId = Convert.ToInt32(Session["UserId"]);

            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO SavingGoals (UserId, GoalName, TargetAmount, TargetDate) VALUES (@UserId,@GoalName,@TargetAmount,@TargetDate)", con);

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@GoalName", txtGoalName.Text.Trim());
                cmd.Parameters.AddWithValue("@TargetAmount", Convert.ToDecimal(txtTargetAmount.Text));
                cmd.Parameters.AddWithValue("@TargetDate", Convert.ToDateTime(txtTargetDate.Text));

                con.Open();
                cmd.ExecuteNonQuery();
            }

            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = "Goal Added Successfully!";

            txtGoalName.Text = "";
            txtTargetAmount.Text = "";
            txtTargetDate.Text = "";

            LoadGoals();
        }

        private void LoadGoals()
        {
            int userId = Convert.ToInt32(Session["UserId"]);

            decimal totalIncome = GetTotalIncome(userId);
            decimal totalExpense = GetTotalExpense(userId);
            decimal totalSavings = totalIncome - totalExpense;

            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM SavingGoals WHERE UserId=@UserId", con);

                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dt.Columns.Add("SavedAmount");
                dt.Columns.Add("Progress");
                dt.Columns.Add("RemainingAmount");
                dt.Columns.Add("DaysLeft");

                foreach (DataRow row in dt.Rows)
                {
                    decimal target = Convert.ToDecimal(row["TargetAmount"]);

                    decimal progress = 0;
                    decimal remaining = target;

                    if (target > 0)
                    {
                        progress = (totalSavings / target) * 100;
                        remaining = target - totalSavings;
                    }

                    int daysLeft =(Convert.ToDateTime(row["TargetDate"]) - DateTime.Now).Days;
                    row["SavedAmount"] = totalSavings;
                    row["Progress"] = progress.ToString("0.00");
                    row["RemainingAmount"] = remaining < 0 ? 0 : remaining;
                    row["DaysLeft"] = daysLeft < 0 ? 0 : daysLeft;
                }
                gvGoals.DataSource = dt;
                gvGoals.DataBind();
            }
        }

        private decimal GetTotalIncome(int userId)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT ISNULL(SUM(Amount),0) FROM Income WHERE UserId=@UserId", con);

                cmd.Parameters.AddWithValue("@UserId", userId);

                con.Open();
                return Convert.ToDecimal(cmd.ExecuteScalar());
            }
        }

        private decimal GetTotalExpense(int userId)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT ISNULL(SUM(Amount),0) FROM Expense WHERE UserId=@UserId", con);
                cmd.Parameters.AddWithValue("@UserId", userId);
                con.Open();
                return Convert.ToDecimal(cmd.ExecuteScalar());
            }
        }

        protected void calGoalDate_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.IsOtherMonth)
            {
                e.Cell.Controls.Clear();   // remove the date number
                e.Cell.Text = "";          // blank cell
                e.Cell.Enabled = false;    // disable click
                e.Cell.BackColor = System.Drawing.Color.Transparent;
            }

        }
    }
}