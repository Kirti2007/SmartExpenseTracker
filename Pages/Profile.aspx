<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DashboardMaster.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="SmartExpenseTracker.Pages.Profile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Assets/CSS/profile.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnlProfile" runat="server" CssClass="card">

        <asp:Label ID="lblTitle" runat="server" Text="My Profile" CssClass="page-title" ></asp:Label>

        <!-- BASIC INFORMATION -->
        <asp:Label ID="lblBasic" runat="server" Text="Basic Information" CssClass="section-title"></asp:Label>

        <asp:Label runat="server" Text="Full Name" CssClass="label" ></asp:Label>
        <asp:TextBox ID="txtFullName" runat="server" CssClass="input" ></asp:TextBox>

        <asp:Label runat="server" Text="Username" CssClass="label" ></asp:Label>
        <asp:TextBox ID="txtUsername" runat="server" CssClass="input" ></asp:TextBox>

        <asp:Label runat="server" Text="Email" CssClass="label" ></asp:Label>
        <asp:TextBox ID="txtEmail" runat="server" CssClass="input" ></asp:TextBox>

        <asp:Label runat="server" Text="Phone" CssClass="label" ></asp:Label>
        <asp:TextBox ID="txtPhone" runat="server" CssClass="input" ></asp:TextBox>

        <asp:Button ID="btnSave" runat="server" Text="Save Changes" CssClass="btn-primary"
            OnClick="btnSave_Click" />

        <asp:Label ID="lblMsg" runat="server" CssClass="success-msg" ></asp:Label>

        <!-- SECURITY -->
        <asp:Label ID="lblSecurity" runat="server" Text="Security" CssClass="section-title" ></asp:Label>

        <asp:Button ID="btnChangePassword" runat="server" Text="Change Password"
            CssClass="btn-danger" PostBackUrl="~/Account/ChangePassword.aspx" />

    </asp:Panel>
    
</asp:Content>
