using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartExpenseTracker.Account
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
            }
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT COUNT(*) FROM Users
                              WHERE UserId=@UserId AND PasswordHash=@PasswordHash";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@PasswordHash", txtCurrent.Text.Trim());

                con.Open();
                int count = (int)cmd.ExecuteScalar();
                if (count == 0)
                {
                    lblMsg.Text = "Current password is incorrrect";
                    return;

                }
                //updating password
                string updateQuery = @"UPDATE Users 
                       SET PasswordHash = @PasswordHash
                       WHERE UserID = @UserID";

                SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                updateCmd.Parameters.AddWithValue("@PasswordHash", txtNew.Text.Trim());
                updateCmd.Parameters.AddWithValue("@UserID", userId);
                updateCmd.ExecuteNonQuery();
                con.Close();
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Text = "Password changed successfully";

            }
        } 
    }
}