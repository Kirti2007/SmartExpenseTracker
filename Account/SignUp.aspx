<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Authentication.Master" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="SmartExpenseTracker.Account.SignUp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="auth-container">

    <!--Left Banner-->
    <div class="auth-banner"></div>

    <!--Right Form-->
    <div class="auth-form">
        <div class="form-card">
            <h2>Create Account</h2>

            <!--Full Name-->
            <asp:Label ID="lblFullName" runat="server" Text="Name" Font-Size="Medium"></asp:Label>
            <asp:TextBox ID="txtName" runat="server" CssClass="input-box"></asp:TextBox>
            <br />
            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="Full Name is required" ForeColor="Red" Display="Dynamic" Font-Size="Small"></asp:RequiredFieldValidator>
            <br /><br />

            <!--Email-->
            <asp:Label ID="lblEmail" runat="server" Text="Email" Font-Size="Medium"></asp:Label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="input-box"></asp:TextBox>
            <br />
            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="Email is required" ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>
            <br />
            <asp:RegularExpressionValidator ID="revEmail" runat="server" ErrorMessage="Invalid Email Format" Font-Size="Small" ForeColor="Red" ValidationExpression="^[^\s@]+@[^\s@]+\.[^\s@]+$" ControlToValidate="txtEmail"></asp:RegularExpressionValidator>

            <br />

            <!--Password-->
            <asp:Label ID="lblPassword" runat="server" Text="Password" Font-Size="Medium"></asp:Label>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="input-box" TextMode="Password"></asp:TextBox>
            <br />
            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" Display="Dynamic" ErrorMessage="Password is required" ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>
            <br /><br />

            <!--Confirm Paasword-->
            <asp:Label ID="lblComPassword" runat="server" Text="Confirm Password" Font-Size="Medium"></asp:Label>
            <asp:TextBox ID="txtConfirm" runat="server" CssClass="input-box" TextMode="Password"></asp:TextBox>
            <br />
            <asp:RequiredFieldValidator ID="rfvConfirm" runat="server" ControlToValidate="txtConfirm" Display="Dynamic" ErrorMessage="Confirm Password is required" ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>
            <br />

            <!--Password Match Validation-->
            <asp:CompareValidator ID="cvPassword" runat="server" ErrorMessage="Password do not match" ControlToCompare="txtPassword" ControlToValidate="txtConfirm" ForeColor="Red" Font-Size="Small"></asp:CompareValidator>
            <br />
            
            <!--Register Button-->
            <asp:Button ID="btnSignUp" runat="server" CssClass="btn" OnClick="btnSignUp_Click" Text="Sign Up" />
            <br /><br />

            <div class="link">
                <asp:Label ID="lblAccExist" runat="server" Text="Already have an account?" Font-Size="Medium"></asp:Label>
                <a href="Login.aspx">Login </a>
            </div>
        </div>
    </div>
</div>
</asp:Content>
