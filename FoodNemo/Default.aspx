<%@ Page Title="" Language="C#" MasterPageFile="~/header.master" AutoEventWireup="true" Inherits="_Default" Codebehind="Default.aspx.cs" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <label class="title"><h3>Any food in Nus</h3>
    <h3>We get it for you</h3></label>
    <asp:TextBox ID="descriptionText" CssClass="description" runat="server" placeholder="Enter your order e.g. Curry Chicken Rice Set @ Engin Indo Panggang" />
    <asp:TextBox ID="locationText" CssClass="textBox left location" runat="server" placeholder="Hostel A"></asp:TextBox>
    <asp:TextBox ID="telnoText" CssClass="textBox right telno" runat="server" placeholder="+65 12345678"></asp:TextBox>
    <div class="clear"></div>
    <div class="btnContainer">
        <asp:Button ID="submitBtn" CssClass="sendBtn" runat="server" Text="Order" OnClick="submit_Click" />  
    </div>
    <label class="err"></label>
    <script>
        (function ($) {
            var btn = $('.sendBtn');
            var form = $('#orderForm');
            var descTextBox = $('.description');
            var locTextBox = $('.location');
            var telnoTextBox = $('.telno');
            var errLabel = $('label.err');
            btn.on('click', function () {
                //form.validate({onsubmit: false});
                form.validate({
                    rules: {
                        <%= descriptionText.UniqueID%>: {
                            required:true
                        },
                        <%= locationText.UniqueID%>: {
                            required:true
                        },
                        <%= telnoText.UniqueID%>: {
                            required:true
                        }
                    },
                    messages: {
                        <%= descriptionText.UniqueID%>: '*The pick-up location field is compulsory.<br/>',
                        <%= locationText.UniqueID%>: '*The contact number field is compulsory.<br/>',
                        <%= telnoText.UniqueID%>: '*The pick-up order field is compulsory.<br/>'
                    },
                    errorPlacement: function(err, ele){
                        err.appendTo(errLabel);
                    }
                });
            });
        })(jQuery);
    </script>
</asp:Content>

