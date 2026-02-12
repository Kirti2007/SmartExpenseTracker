<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="SmartExpenseTracker.Pages.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Smart Expense Tracker</title>
    <link href="/Assets/CSS/home.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
      <!-- Top Navigation Bar -->
<div class="top-navbar">
    <div class="logo">Smart Expense Tracker</div>

    <div class="menu">
        <asp:HyperLink ID="lnkHome" runat="server" NavigateUrl="~/Pages/Home.aspx" Text="Home" />
        <asp:HyperLink ID="lnkFeatures" runat="server" NavigateUrl="#features" Text="Features" />
        <asp:HyperLink ID="lnkHow" runat="server" NavigateUrl="#how" Text="How It Works" />
        <asp:HyperLink ID="lnkLogin" runat="server" NavigateUrl="~/Account/Login.aspx" Text="Login" />
        <asp:HyperLink ID="lnkSignup" runat="server" NavigateUrl="~/Account/SignUp.aspx" Text="Sign Up" CssClass="signup-btn"/>
    </div>
</div>

<!-- Hero Section -->
<div class="hero">
    <h1>Smart Expense Tracker & Budget Monitoring System</h1>
    <p>Track    Analyze    Save    Grow</p>
    <div class="hero-buttons">
        <asp:Button ID="btnGetStarted" runat="server" Text="Get Started" CssClass="btn-primary" OnClick="btnGetStarted_Click"/>
        <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn-outline" OnClick="btnLogin_Click"/>
    </div>
</div>

<!-- Features -->
<div id="features" class="features">
    <h2>Powerful Features</h2>

    <div class="feature-grid">
        <div class="feature-card">
            <h3>Expense Tracking</h3>
            <p>Record and monitor daily expenses easily.</p>
        </div>

        <div class="feature-card">
            <h3>Budget Monitoring</h3>
            <p>Set monthly budgets and track spending.</p>
        </div>

        <div class="feature-card">
            <h3>Smart Insights</h3>
            <p>Financial suggestions and alerts.</p>
        </div>

        <div class="feature-card">
            <h3>Visual Reports</h3>
            <p>Graphs and charts for clear understanding.</p>
        </div>

        <div class="feature-card">
            <h3>Notifications</h3>
            <p>Smart alerts on adding budget and expense</p>
        </div>
    </div>
</div>

<!-- How It Works section  -->
<div id="how" class="how-it-works">
    <h2>How It Works</h2>

    <div class="steps">
        <div class="step">1. Create Account</div>
        <div class="step">2. Add Income</div>
        <div class="step">3. Track Expenses</div>
        <div class="step">4. View Insights</div>
        <div class="step">5. Save More</div>
    </div>
</div>

<!-- Footer -->
<div class="footer">
    © 2026 Smart Expense Tracker 
</div> 
</form>
</body>
</html>
