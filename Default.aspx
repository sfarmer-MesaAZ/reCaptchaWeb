<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="recaptchaWeb._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
  <script type="text/javascript">
            grecaptcha.ready(function () {
                grecaptcha.execute('6LdBOpgUAAAAAOwiA35gBcQBjgimSnL_JiC4C5mH', {action: 'test'})
                .then(function(token) {
                // Verify the token on the server.
                    $('#MainContent_captchatoken').val(token);
                });
            });
    </script>

    <div class="jumbotron">
        <h1>ASP.NET</h1>
        <p class="lead">ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS, and JavaScript.</p>
        <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-4">
        </div>
        <asp:UpdatePanel runat="server" ID="updatepnl">
            <ContentTemplate>

                <div id="captchav3" style="margin:0,30px 0 0">
                    <div class="g-recaptcha" data-sitekey="6Ldi54AUAAAAALp_hS0ipme0fqvPlPzb6O0Mqn8R "style="float:left;clear:left;margin:0 60px 0 90px" ></div>
                    <asp:Button ID="btnDoAction" runat="server" style="margin-top:20px !important"  
                        Text="Do Action" CssClass="btn btn-primary" OnClick="btnDispositionReport_Click" 
                        OnClientClick="ShowStuff()"  />
                        <br />  
                <asp:Label runat="server" id="os" name="os" Text="Operating System"/></br>
                <asp:Label runat="server" id="browser" name="browser" Text="Browser"/></br>
                <asp:Label runat="server" id="ip" name="ip" Text="IpAddress"/></br>
                <asp:Label runat="server" id="score" name="score" Text="Score"/></br>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="the_hidden" style="visibility:hidden;">
            <asp:Button ID="btnSendToGoog" runat="server" style="margin-top:20px !important;"  
                Text="Send Result" CssClass="btn btn-primary;"/>
                <br />  

            <label id="lblCaptchaMessage" runat="server" clientidmode="static"></label> </br>
            <asp:Textbox runat="server" type="text" id="captchatoken" name="captchatoken" style="visibility:hidden;" />
        
            </br>
        
        </div>
        

 <%--           <asp:Label runat="server" id="browser" name="browser" Text="Browser"/>--%>


    </div>
    <script type="text/javascript">
        function ShowStuff(){
            $("#the_hidden").css("visibility", "visible");;
        };
    </script>
</asp:Content>
