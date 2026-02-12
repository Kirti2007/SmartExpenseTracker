using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartExpenseTracker.MasterPages
{
    public partial class DashboardMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string currentPage = System.IO.Path.GetFileName(Request.Path).ToLower();
            // Pages that should open without login
            if (Session["UserId"] == null &&currentPage != "~/Pages/Home.aspx" &&currentPage != "~/Account/Login.aspx" &&currentPage != "~/Account/Signup.aspx")
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            if (!IsPostBack)
            {
                if (Session["UserName"] != null)
                {
                    lblUserName.Text = "Hello, " + Session["UserName"].ToString();
                    ddlProfileMenu.SelectedIndex = -1;
                }
                else
                {
                    lblUserName.Text = "Hello, Guest";
                }
            }
        }

        protected void ddlProfileMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlProfileMenu.SelectedValue))
                return;

            switch (ddlProfileMenu.SelectedValue)
            {
                case "Profile":
                    Response.Redirect("~/Pages/Profile.aspx");
                    break;

                case "ChangePassword":
                    Response.Redirect("~/Account/ChangePassword.aspx");
                    break;

                case "MyNotification":
                    Response.Redirect("~/Pages/Notification.aspx");
                    break;

                case "Logout":
                    Session.Clear();
                    Session.Abandon();
                    Response.Redirect("~/Pages/Home.aspx");
                    break;
            }
        }
    }
}
           