<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Authentication.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SmartExpenseTracker.Account.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="auth-container">
    <!--Left Banner-->
    <div class="auth-banner"></div>
    <!--Right Login Form-->
    <div class="auth-form">
        <div class="form-card">
            <h2>Welcome Back !</h2>

            <!--Error Message-->
            <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Size="Medium" ></asp:Label>
            <br />
            <!--Email--> 
            <asp:Label ID="lblEmail" runat="server" Text="Email" Font-Size="Medium"></asp:Label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="input-box" ></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required" ForeColor="Red" Display="Dynamic" Font-Size="Medium"></asp:RequiredFieldValidator>
            <br /><br />

            <!--Password-->
            <asp:Label ID="lblPassword" runat="server" Text="Password" Font-Size="Medium"></asp:Label>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="input-box" TextMode="Password"></asp:TextBox>
             <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" ErrorMessage="Password is required" ForeColor="Red" Display="Dynamic" Font-Size="Medium"></asp:RequiredFieldValidator>
            <br /><br />

            <!--Login Button-->
            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn" OnClick="btnLogin_Click" />
            <br /><br />
            <!--Register Link-->
            <div class="link">
                <asp:Label ID="lblNoacc" runat="server" Text="Don't have an account?" Font-Size="Medium"></asp:Label>
                <a href="SignUp.aspx">Sign Up</a> 
            </div> 
        </div>
    </div>
</div>
</asp:Content>
