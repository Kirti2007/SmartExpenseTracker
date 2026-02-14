<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DashboardMaster.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="SmartExpenseTracker.Pages.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Assets/CSS/dashboard.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h2 class="page-title">Dashboard Overview</h2>
<!--Summary cards-->
    <div class="card-container">
        <div class="summary-card income">
            <h3>Total Income</h3>
            <p><asp:Label ID="lblTotalIncome" runat="server" Text="₹ 0"></asp:Label></p>
             
        </div>
        <div class="summary-card expense">
            <h3>Total Expense</h3>
            <p><asp:Label ID="lblTotalExpense" runat="server" Text="₹ 0"></asp:Label></p>
        </div>

        <div class="summary-card budget">
            <h3>Remaining Budget</h3>
            <p><asp:Label ID="lblRemainingBudget" runat="server"  Text="₹ 0"></asp:Label> </p>
        </div>
        
       <div class="summary-card savings">
           <h3>Total Savings</h3>
           <p><asp:Label ID="lblSavings" runat="server"  Text="₹ 0"></asp:Label></p>
       </div>
        <!-- Financial Health score Card -->
        <div class="summary-card health" id="healthCard" runat="server">
            <h3>Financial Health</h3>
            <p>
                <asp:Label ID="lblHealthStatus" runat="server" Text="--"></asp:Label><br />
                <small><asp:Label ID="lblHealthPercent" runat="server" Text="0%"></asp:Label></small>
            </p>
        </div>
    </div>

    <!--Recent Transaction-->
    <div class="section">
        <h3>Recent Transactions</h3>
     <asp:GridView ID="gvTransactions" runat="server" CssClass="table"
            AutoGenerateColumns="false" EmptyDataText="No transactions found" AllowPaging="True" PageSize="5" OnPageIndexChanging="gvTransactions_PageIndexChanging">
            <Columns>
                <asp:BoundField HeaderText="Date" DataField="TransactionDate"  />
                <asp:BoundField HeaderText="Category" DataField="CategoryName" />
                <asp:BoundField HeaderText="Amount" DataField="Amount" />
                <asp:BoundField HeaderText="Type" DataField="Type" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
