<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DashboardMaster.Master" AutoEventWireup="true" CodeBehind="Budgets.aspx.cs" Inherits="SmartExpenseTracker.Pages.Budgets" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Assets/CSS/budget.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <!-- Page Title -->
    <div class="page-header">
        <asp:Label ID="lblBudgetTitle" runat="server" Text="Budget" CssClass="page-title"></asp:Label>
        <div class="monthly-budget">
            Monthly Budget:
            <span><asp:Label ID="lblMonthlyBudget" runat="server" Text="₹ 0"></asp:Label></span>
        </div>
    </div>

    <!-- Main Row -->
    <div class="budget-row">

        <!-- Left -->
        <asp:Panel ID="pnlSetBudget" runat="server" CssClass="budget-card">

            <div class="card-title">Set Monthly Budget</div>

            <asp:Label ID="lblMonth" runat="server" Text="Month" CssClass="lblBudget"></asp:Label>
            <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control">
                <asp:ListItem Text="January" Value="1" />
                <asp:ListItem Text="February" Value="2" />
                <asp:ListItem Text="March" Value="3" />
                <asp:ListItem Text="April" Value="4" />
                <asp:ListItem Text="May" Value="5" />
                <asp:ListItem Text="June" Value="6" />
                <asp:ListItem Text="July" Value="7" />
                <asp:ListItem Text="August" Value="8" />
                <asp:ListItem Text="September" Value="9" />
                <asp:ListItem Text="October" Value="10" />
                <asp:ListItem Text="November" Value="11" />
                <asp:ListItem Text="December" Value="12" />
            </asp:DropDownList>

            <asp:Label ID="lblYear" runat="server" Text="Year" CssClass="lblBudget"></asp:Label>
            <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control"></asp:DropDownList>

            <asp:Label ID="Amount" runat="server" Text="Budget Amount(₹)" CssClass="lblBudget"></asp:Label>
            <asp:TextBox ID="txtBudgetAmount" runat="server" CssClass="form-control" ></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ControlToValidate="txtBudgetAmount" ErrorMessage="Budget amount is required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="revAmount" runat="server" ControlToValidate="txtBudgetAmount" ValidationExpression="^\d+(\.\d{1,2})?$" ErrorMessage="Enter valid numeric amount only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>

            <asp:Button ID="btnSaveBudget" runat="server" Text="Save Budget"
                CssClass="btn-save" OnClick="btnSaveBudget_Click1" />

        </asp:Panel>

        <!-- Right -->
        <asp:Panel ID="pnlBudgetSummary" runat="server" CssClass="budget-card">

            <div class="card-title">Budget Summary</div>

            <div class="summary-row">
                Total Budget:
                <span class="total-budget">
                    <asp:Label ID="lblTotalBudget" runat="server" />
                </span>
            </div>

            <div class="summary-row">
                Total Expense:
                <span class="total-expense">
                    <asp:Label ID="lblTotalExpense" runat="server" />
                </span>
            </div>

            <div class="summary-row">
                Remaining Budget:
                <span class="remaining-budget">
                    <asp:Label ID="lblRemainingBudget" runat="server" />
                </span>
            </div>
        </asp:Panel>
    </div>

    <!-- Category Table -->
    <asp:Panel ID="pnlCategoryBudget" runat="server" CssClass="budget-card full-width">

        <div class="card-title">Category-wise Expense Summary</div>

        <asp:GridView ID="gvCategoryBudget" runat="server"
            AutoGenerateColumns="False" CssClass="grid-view">
            <Columns>
                <asp:BoundField DataField="CategoryName" HeaderText="Category" />
                <asp:BoundField DataField="SpentAmount" HeaderText="Spent Amount (₹)" />
            </Columns>
        </asp:GridView>
    </asp:Panel> 
</asp:Content>
    