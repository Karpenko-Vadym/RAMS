<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RAMS.Email.Default" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>RAMS Emails</title>
        <link rel="stylesheet"href="//netdna.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css"/>
    </head>
    <body>
        <form id="defaultForm" runat="server">
        <div>
        <asp:GridView ID="gvEmails" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover">
                <Columns>
                    <asp:BoundField HeaderText="Email Id" DataField="EmailId" />
                    <asp:BoundField HeaderText="Email Address" DataField="Address" />
                    <asp:BoundField HeaderText="Email Subject" DataField="Subject" />
                    <asp:BoundField HeaderText="Email Content" DataField="Body" />
                    <asp:BoundField HeaderText="Email Sent?" DataField="Status" />
                </Columns>
            </asp:GridView>
        </div>
        </form>
    </body>
</html>
