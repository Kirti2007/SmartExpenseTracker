<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DashboardMaster.Master" AutoEventWireup="true" CodeBehind="GoalTracker.aspx.cs" Inherits="SmartExpenseTracker.Pages.GoalTracker" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Assets/CSS/goal.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <!-- Page Title -->
    <asp:Label ID="lblPageTitle" runat="server" Text="Saving Goal Planner"
        CssClass="page-title"></asp:Label>

    <br /><br />

    <!-- Add Goal Panel -->
    <asp:Panel ID="pnlAddGoal" runat="server" CssClass="goal-panel">

        <asp:Label ID="lblTitle" runat="server"
            Text="Add Saving Goal" CssClass="title"></asp:Label>

        <br /><br />

        <asp:Label ID="Label1" runat="server" Text="Goal Name"></asp:Label>
        <asp:TextBox ID="txtGoalName" runat="server"
            CssClass="textbox"></asp:TextBox>

        <br /><br />

        <asp:Label ID="Label2" runat="server" Text="Target Amount"></asp:Label>
        <asp:TextBox ID="txtTargetAmount" runat="server"
            CssClass="textbox"></asp:TextBox>

        <br /><br />

        <asp:Label ID="Label3" runat="server"
            Text="Target Date"></asp:Label>

        <asp:TextBox ID="txtTargetDate" runat="server"
            CssClass="textbox" ReadOnly="true"></asp:TextBox>

        <br /><br />

        <!-- ASP.NET Calendar -->
        <asp:Calendar ID="calTargetDate" runat="server"
            CssClass="calendar"
            OnSelectionChanged="calTargetDate_SelectionChanged" OnDayRender="calGoalDate_DayRender">
        </asp:Calendar>

        <br />

        <asp:Button ID="btnSaveGoal" runat="server"
            Text="Save Goal"
            CssClass="btn"
            OnClick="btnSaveGoal_Click" />

        <br /><br />

        <asp:Label ID="lblMessage" runat="server"></asp:Label>

    </asp:Panel>

    <br />

    <!-- Goals Grid -->
    <asp:GridView ID="gvGoals" runat="server"
        AutoGenerateColumns="False"
        CssClass="grid">
        <Columns>
            <asp:BoundField DataField="GoalName" HeaderText="Goal Name" />
            <asp:BoundField DataField="TargetAmount" HeaderText="Target Amount" />
            <asp:BoundField DataField="SavedAmount" HeaderText="Saved Amount" />
            <asp:BoundField DataField="Progress" HeaderText="Progress (%)" />
            <asp:BoundField DataField="RemainingAmount" HeaderText="Remaining Amount" />
            <asp:BoundField DataField="DaysLeft" HeaderText="Days Left" />
        </Columns>
    </asp:GridView>
</asp:Content>
