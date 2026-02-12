<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DashboardMaster.Master" AutoEventWireup="true" CodeBehind="Income.aspx.cs" Inherits="SmartExpenseTracker.Pages.Income" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/Assets/CSS/income.css") %>" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="page-title">Income</h2>

    <!--Monthly Total-->
    <div class="income-summary">
        <asp:Label ID="lblSummaryText" runat="server" Text="Total Income(This Month):"></asp:Label>
        <asp:Label ID="lblMonthlyIncome" runat="server" CssClass="income-amount" Text="₹ 0"></asp:Label>
    </div>
    <div class="income-layout">
        <!--Left side : add income-->

        <div class="income-form">
            <asp:Label ID="lblAddIncome" runat="server" Text="Add Income" CssClass="section-title"></asp:Label>
            
            <!--Income source-->
            <asp:DropDownList ID="ddlSource" runat="server" CssClass="input-box" AutoPostBack="true" OnSelectedIndexChanged="ddlSource_SelectedIndexChanged">
                <asp:ListItem Text="Select Source" Value=""></asp:ListItem>
                <asp:ListItem Text="Salary"></asp:ListItem>
                <asp:ListItem Text="Freelance"></asp:ListItem>
                <asp:ListItem Text="Business" ></asp:ListItem>
                <asp:ListItem Text="Interest"></asp:ListItem>
                <asp:ListItem Text="Other"></asp:ListItem>
            </asp:DropDownList>
            <br /><br />

            <!--Custom soucr-->
            <asp:TextBox ID="txtCustomSource" runat="server" CssClass="input-box" Placeholder="Enter custom source" Visible="false" />

            <!--Amount-->
            <asp:Label ID="lblAmount" runat="server" Text="Amount(Rs.)"></asp:Label>
            <asp:TextBox ID="txtAmount" runat="server" CssClass="input-box"></asp:TextBox>
            <br /><br />

            <!--Short Description-->
            <asp:Label ID="lblDescription" runat="server" Text="Description (Optional)"></asp:Label>
            <asp:TextBox ID="txtDescription" runat="server" CssClass="input-box" TextMode="MultiLine" Rows="3"></asp:TextBox> 
            <!--Date-->
            <asp:Label ID="lblDate" runat="server" Text="Income Date"></asp:Label>
            <asp:Calendar ID="calIncomeDate" runat="server" CssClass="calendar" ShowOtherMonths="false" OnDayRender="calIncomeDate_DayRender" ></asp:Calendar>

            <!--Button-->
            <asp:Button ID="btnAddIncome" runat="server" Text="Add Income" CssClass="btn-primary" OnClick="btnAddIncome_Click" />

            </div>

        <!--Right side (Income history)-->
        <div class="income-history">
            <asp:Label ID="lblHistory" runat="server" Text="Income History" CssClass="section-title"></asp:Label>
            <asp:GridView ID="gvIncome" runat="server" AutoGenerateColumns="false" CssClass="income-table" EmptyDataText="No income records found">

            <Columns>
                <asp:BoundField DataField="IncomeSource" HeaderText="Source" />
                <asp:BoundField DataField="Amount" HeaderText="Amount(Rs.)" />
                <asp:BoundField DataField="IncomeDate" HeaderText="Date" DataFormatString="{0:dd-MM-yyyy}" />

            </Columns>
            </asp:GridView>
        </div>
        </div>
    
</asp:Content>
