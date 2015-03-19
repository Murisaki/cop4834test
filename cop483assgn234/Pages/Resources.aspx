<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Resources.aspx.cs" Inherits="cop483assgn234.Pages.Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:TextBox ID="TextBox1" runat="server" OnTextChanged="TextBox1_TextChanged"></asp:TextBox>
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="ButtonPlaceholder" /> <br />
    <br />
    <asp:Table BackColor="SkyBlue" ID="Table1" BorderStyle="Groove"  BorderWidth="5" CellPadding="2" CellSpacing="3" runat="server">
        <asp:TableHeaderRow Font-Bold="true"   BorderWidth="5">
            <asp:TableHeaderCell BorderStyle="Ridge">Description</asp:TableHeaderCell>
            <asp:TableHeaderCell BorderStyle="Ridge">Link</asp:TableHeaderCell>
            <asp:TableHeaderCell BorderStyle="Ridge">Assignment</asp:TableHeaderCell>
            <asp:TableHeaderCell BorderStyle="Ridge">Action</asp:TableHeaderCell>
        </asp:TableHeaderRow>
        <asp:TableRow BorderStyle="Solid">
            <asp:TableCell BorderStyle="Solid">Basic HTML Editing</asp:TableCell>
            <asp:TableCell BorderStyle="Inset">
                <a href="https://msdn.microsoft.com/en-us/library/9z74w20y(v=vs.110).aspx">MSDN HTML Editing</a>
            </asp:TableCell>
            <asp:TableCell BorderStyle="Inset">A2-4</asp:TableCell>
            <asp:TableCell BorderStyle="Inset">test4</asp:TableCell>
        </asp:TableRow>

    </asp:Table>
</asp:Content>
