<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DashboardMaster.Master" AutoEventWireup="true" CodeBehind="Notification.aspx.cs" Inherits="SmartExpenseTracker.Pages.Notification" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Assets/CSS/notifications.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="page-title">Notifications</h2>

    <!-- Info Label -->
    <asp:Label ID="lblMessage" runat="server" CssClass="info-label"></asp:Label>

    <!-- Notifications Grid -->
    <asp:GridView ID="gvNotifications" runat="server" AutoGenerateColumns="False" CssClass="notification-grid" DataKeyNames="NotificationId" OnRowCommand="gvNotifications_RowCommand">

        <Columns>

            <asp:BoundField DataField="Message" HeaderText="Message" />

            <asp:BoundField DataField="CreatedAt" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy hh:mm tt}" />

            <asp:TemplateField HeaderText="Status">
                <ItemTemplate>
                    <asp:Label ID="lblStatus" runat="server" Text='<%# Convert.ToBoolean(Eval("IsRead")) ? "Read" : "Unread" %>' CssClass='<%# Convert.ToBoolean(Eval("IsRead")) ? "read" : "unread" %>'> </asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:Button ID="btnRead" runat="server" Text="Mark as Read" CommandName="Read" CommandArgument='<%# Eval("NotificationId") %>'  Visible='<%# !Convert.ToBoolean(Eval("IsRead")) %>' CssClass="btn-read" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Delete">
                <ItemTemplate>
                         <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="DeleteNotification" CommandArgument='<%# Eval("NotificationId") %>' CssClass="btn-delete" />
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>


</asp:Content>
