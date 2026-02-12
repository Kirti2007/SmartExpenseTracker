<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DashboardMaster.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="SmartExpenseTracker.Pages.Report" %>
<%@ Register Assembly="System.Web.DataVisualization" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="chart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link href="<%= ResolveUrl("~/Assets/CSS/report.css") %>" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="page-heading">
    <asp:Label ID="lblTitle" runat="server" Text="Financial Report"
        CssClass="page-title"></asp:Label>
</div>

<div class="filter-section">
    Month:
    <asp:DropDownList ID="ddlMonth" runat="server">
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

    Year:
    <asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList>

    <asp:Button ID="btnGenerate" runat="server" Text="Generate Report"
        CssClass="btn" OnClick="btnGenerate_Click" />
</div>

<div class="summary-section">
    <div>Total Income<br /><asp:Label ID="lblIncome" runat="server" /></div>
    <div>Total Expense<br /><asp:Label ID="lblExpense" runat="server" /></div>
    <div>Savings<br /><asp:Label ID="lblSavings" runat="server" /></div>
</div>

<h3>Income vs Expense</h3>
<asp:Chart ID="ChartBar" runat="server" Width="600px" Height="300px">
    <Series>
        <asp:Series Name="Finance" ChartType="Column" IsValueShownAsLabel="true" />
    </Series>

    <ChartAreas>
        <asp:ChartArea Name="ChartArea1">
            <AxisX>
                <MajorGrid Enabled="False" />
            </AxisX>
            <AxisY>
                <MajorGrid Enabled="False" />
            </AxisY>
        </asp:ChartArea>
    </ChartAreas>
</asp:Chart>

<h3>Expense by Category</h3>
<chart:Chart ID="ChartPie" runat="server" Width="600px" Height="300px">
    <Series>
        <chart:Series Name="Category" ChartType="Pie"
            IsValueShownAsLabel="true"
            Label="#VALX : ₹#VALY" />
    </Series>
    <ChartAreas>
        <chart:ChartArea Name="ChartArea2" />
    </ChartAreas>
</chart:Chart>

<h3>Budget Status</h3>
<asp:Label ID="lblBudgetStatus" runat="server" />

</asp:Content>
