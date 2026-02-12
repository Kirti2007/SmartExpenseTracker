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
    public partial class SignUp : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            {
                if (string.IsNullOrWhiteSpace(txtName.Text) ||string.IsNullOrWhiteSpace(txtEmail.Text) ||string.IsNullOrWhiteSpace(txtPassword.Text) ||string.IsNullOrWhiteSpace(txtConfirm.Text))
                {
                    Response.Write("<script>alert('All fields are required');</script>");
                    return;
                }
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    //Checking if email already exist
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email=@Email";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                    checkCmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        Response.Write("<script>alert('Email already exists!')</script>");
                        return;
                    }
                    //Inserting user details
                    string query = @"Insert INTO Users(FullName,Email,PasswordHash,Role,CreatedAt)VALUES (@FullName,@Email,@PasswordHash,@Role,GETDATE())";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@FullName", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@PasswordHash", txtPassword.Text);
                    cmd.Parameters.AddWithValue("@Role", "User");
                    cmd.ExecuteNonQuery();
                    Response.Redirect("Login.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();

                }
            }
        }
    }
}