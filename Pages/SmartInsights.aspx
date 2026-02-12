<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DashboardMaster.Master" AutoEventWireup="true" CodeBehind="SmartInsights.aspx.cs" Inherits="SmartExpenseTracker.Pages.SmartInsights" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Assets/CSS/smartinsights.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="page-title">Smart Financial Insights</h2>

<div class="insight-container">

    <div class="insight-card info">
        <asp:Label ID="lblMonthlyTrend" runat="server" />
    </div>

    <div class="insight-card warning">
        <asp:Label ID="lblBudgetWarning" runat="server" />
    </div>

    <div class="insight-card success">
        <asp:Label ID="lblSavingsTip" runat="server" />
    </div>

    <div class="insight-card primary">
        <asp:Label ID="lblTopCategory" runat="server" />
    </div>

    <div class="insight-card danger">
        <asp:Label ID="lblExpenseControl" runat="server" />
    </div>

</div>
   
</asp:Content>
