<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DashboardMaster.Master" AutoEventWireup="true" CodeBehind="Expense.aspx.cs" Inherits="SmartExpenseTracker.Pages.Expense" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/Assets/CSS/expense.css") %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="page-title">Expenses</h2>

    <!--Total Expense-->
    <div class="expense-summary">
        <asp:Label ID="lblTotalText" runat="server" Text="Total Expense(This Month): "></asp:Label>
        <asp:Label ID="lblTotalExpense" runat="server" Text="₹ 0.00"></asp:Label>
    </div>
    <div class="expense-container">
        <!--Add Expense-->
        <div class="card">
            <asp:Label ID="lblAddExpense" runat="server" Text="Add Expense" CssClass="card-title"></asp:Label>
        
            <!--Label category-->
            <asp:Label ID="lblCategory" runat="server" Text="Category"> </asp:Label><br /><br />
                <asp:DropDownList ID="ddlCategory" runat="server"  CssClass="input-box" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" ></asp:DropDownList>
            <!--Other Category-->
            <asp:Label ID="lblOtherCategory" runat="server" Text="Specify Category" Visible="false"></asp:Label>
             <asp:TextBox ID="txtOtherCategory" runat="server" CssClass="input-box"  Visible="false" ></asp:TextBox>
           
            <!--Amount-->
            <asp:Label ID="lblAmount" runat="server" Text="Amount (₹)"></asp:Label><br /><br />
            <asp:TextBox ID="txtAmount" runat="server" CssClass="input-box"></asp:TextBox>


            <!--Description-->
            <asp:Label ID="lblDescription" runat="server" Text="Description(Optional)"></asp:Label><br /><br />
            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="3" CssClass="input-box"></asp:TextBox>

            <!--Date-->
            <asp:Label ID="lblDate" runat="server" Text="Expense Date"></asp:Label>
            <asp:Calendar ID="calExpenseDate" runat="server" CssClass="calendar" ShowOtherMonths="false" OnDayRender="calExpenseDate_DayRender" ></asp:Calendar>

            <!--Button-->
            <asp:Button ID="btnAddExpense" runat="server" Text="Add Expense" CssClass="btn-expense" OnClick="btnAddExpense_Click" />
            <asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label>

        </div>
        <!--Expense History-->
        <div class="card">
            <h3>Expense History</h3>
            <!--Filter-->
            <div class="filter">
                <asp:DropDownList ID="ddlMonth" runat="server" CssClass="input-box">
                    <asp:ListItem Text="January" Value="1" ></asp:ListItem>
                    <asp:ListItem Text="February" Value="2" ></asp:ListItem>
                    <asp:ListItem Text="March" Value="3" ></asp:ListItem>
                    <asp:ListItem Text="April" Value="4" ></asp:ListItem>
                    <asp:ListItem Text="May" Value="5" ></asp:ListItem>
                    <asp:ListItem Text="June" Value="6" ></asp:ListItem>
                    <asp:ListItem Text="July" Value="7" ></asp:ListItem>
                    <asp:ListItem Text="August" Value="8" ></asp:ListItem>
                    <asp:ListItem Text="September" Value="9" ></asp:ListItem>
                    <asp:ListItem Text="October" Value="10" ></asp:ListItem>
                    <asp:ListItem Text="November" Value="11" ></asp:ListItem>
                    <asp:ListItem Text="December" Value="12" ></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddlYear" runat="server" CssClass="input-box"></asp:DropDownList>
                <asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" />
            </div>
            <!--Table-->
            <asp:GridView ID="gvExpenses" runat="server" AutoGenerateColumns="false" CssClass="expense-table" EmptyDataText="No income records found" DataKeyNames="ExpenseId" OnRowDeleting="gvExpenses_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="Category" HeaderText="Category" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount (₹)" />
                    <asp:BoundField DataField="ExpenseDate" HeaderText="Date" DataFormatString="{0:dd-MM-yyyy}" />
                    <asp:CommandField ShowDeleteButton="true" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
