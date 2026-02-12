using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartExpenseTracker.Pages
{
    public partial class Profile : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
                Response.Redirect("~/Account/Login.aspx");

            if (!IsPostBack)
            {
                LoadProfile();
            }
        }
        private void LoadProfile()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT FullName, Username, Email, Phone 
                                 FROM Users WHERE UserID=@UserID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserID", Session["UserId"]);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtFullName.Text = dr["FullName"].ToString();
                    txtUsername.Text = dr["Username"].ToString();
                    txtEmail.Text = dr["Email"].ToString();
                    txtPhone.Text = dr["Phone"].ToString();
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"UPDATE Users SET
                                FullName=@FullName,
                                Username=@Username,
                                Email=@Email,
                                Phone=@Phone
                                WHERE UserID=@UserID";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                cmd.Parameters.AddWithValue("@UserID", Session["UserId"]);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            lblMsg.Text = "Profile updated successfully.";
        }

    }
    
   
}