using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartExpenseTracker.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT UserId,FullName FROM Users WHERE Email=@Email AND PasswordHash=@PasswordHash";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@PasswordHash", txtPassword.Text.Trim());

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    Session["UserId"] = dr["UserId"].ToString();
                    Session["UserName"] = dr["FullName"].ToString();

                    InsertLoginHistory(Convert.ToInt32(dr["UserId"]), "Success");
                    Response.Redirect("~/Pages/Dashboard.aspx");
                }
                else
                {
                    lblError.Text = "Invalid Email or Password";
                    InsertLoginHistory(null, "Failed");
                }
                con.Close();
            }  
        }
        private void InsertLoginHistory(int? userId, string status)
        {
            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"INSERT INTO LoginHistory(UserId,LoginTime,Status)VALUES(@UserId,GETDATE(),@STATUS)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", (object)userId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", status);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}