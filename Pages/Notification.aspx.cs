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
    public partial class Notification : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadNotifications();
            }
        }
        void LoadNotifications()
        {
            int userId = Convert.ToInt32(Session["UserId"]);

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT NotificationId, Message, CreatedAt, IsRead " +
                    "FROM Notifications WHERE UserId=@UserId ORDER BY CreatedAt DESC", con);

                da.SelectCommand.Parameters.AddWithValue("@UserId", userId);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvNotifications.DataSource = dt;
                gvNotifications.DataBind();

                lblMessage.Text = dt.Rows.Count == 0 ? "No notifications available." : "";
            }
        }
        protected void gvNotifications_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            int notifId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Read")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE Notifications SET IsRead = 1 WHERE NotificationId = @Id AND UserId=@UserId", con);

                    cmd.Parameters.AddWithValue("@Id", notifId);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            else if (e.CommandName == "DeleteNotification")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(
                        "DELETE FROM Notifications WHERE NotificationId = @Id AND UserId=@UserId", con);

                    cmd.Parameters.AddWithValue("@Id", notifId);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    lblMessage.Text = rows > 0 ? "Deleted successfully" : "Deletion failed";
                }
            }

            LoadNotifications();
        }

    }

}
