<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Authentication.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="SmartExpenseTracker.Account.ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="auth-container change-password">
        <div class="auth-banner"></div>
            <div class="auth-form">
                <div class="form-card">
                <h2>Change Password</h2>
                <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                <br />
                <!--Current Password-->
                <asp:Label runat="server" Text="Current Password" Font-Size="Medium"></asp:Label>
                <asp:TextBox ID="txtCurrent" runat="server" CssClass="input-box" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCurrent" ErrorMessage="Current password required" ForeColor="Red"></asp:RequiredFieldValidator>
                <br /><br />
                <!--New Password-->
                <asp:Label runat="server" Text="New Password" Font-Size="Medium"></asp:Label>
                <asp:TextBox ID="txtNew" runat="server" CssClass="input-box" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNew" ErrorMessage="New Password required" ForeColor="Red"></asp:RequiredFieldValidator>
                <br /><br />
                <!--Confirm Password-->
                <asp:Label runat="server" Text="Confirm New Password" Font-Size="Medium"></asp:Label>
                <asp:TextBox ID="txtConfirm" runat="server" CssClass="input-box" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtConfirm" ErrorMessage="Confirm password required" ForeColor="Red"></asp:RequiredFieldValidator>

                <asp:CompareValidator runat="server" ControlToValidate="txtConfirm" ControlToCompare="txtNew" ErrorMessage="Password do not match" ForeColor="Red"></asp:CompareValidator>
                <br /><br />
                <asp:Button ID="btnChange" runat="server" Text="Update Password" CssClass="btn" OnClick="btnChange_Click" /> 
             </div>
          </div>
    </div>
   
</asp:Content>
