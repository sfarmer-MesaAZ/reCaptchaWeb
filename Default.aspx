<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="recaptchaWeb._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
  <script type="text/javascript">
            grecaptcha.ready(function () {
                grecaptcha.execute('6LdUdpgUAAAAAFnuf6lPVa_oMBU6lUyLoOUIOCtu', {action: 'test'})
                .then(function(token) {
                // Verify the token on the server.
                    console.log(token);
                    $('#MainContent_captchatoken').val(token);
                });
            });

      $("#btnCriterionCaptcha").click(function () {
      });

    </script>

    <div class="jumbotron">
        <h1>Captcha Profiler</h1>
        <p class="lead">Hello - You are helping us diagnose traffic flows to Google Analytics.  </p>
            <ol>
                <li>Click on the "Clean Test" button first.</li>
                <li>Next - click on the "Filter Test" button.</li>
                <li>Look for some information below the button.</li>
                <li>Click Send Result!</li>
            </ol>
        <p>If you happen to get an error saying you should try again in a few moments, please be sure to click send result.</p>
        <p></p>
    </div>

    <div class="row">
        <asp:UpdatePanel runat="server" ID="updatepnl">
            <ContentTemplate>

                <div id="captchav3" style="margin:0,30px 0 0">
                    <div class="g-recaptcha" data-sitekey="6LdUdpgUAAAAAFnuf6lPVa_oMBU6lUyLoOUIOCtu" style="float:left;clear:left;margin:0 60px 0 90px" ></div>
                    <asp:Button ID="btnCleanCaptcha" runat="server" style="margin-top:20px !important"  
                        Text="Clean Test" CssClass="btn btn-primary" OnClick="btnCleanCaptcha_Click" 
                        OnClientClick="ShowStuff()"  />
                        <br />  
                    <asp:Button ID="btnCriterionCaptcha" runat="server" style="margin-top:20px !important"  
                        Text="Filter Test" CssClass="btn btn-warning" OnClick="btnCriterionCaptcha_Click" 
                        OnClientClick="ShowStuff()"  />
                        <br />  
                </div>
                <div id="traffic_data" class="r-border" >
                    <asp:Label runat="server" id="clean_os" name="os" Text="Operating System"/>
                    <asp:Label runat="server" id="clean_browser" name="browser" Text="Browser"/>
                    <asp:Label runat="server" id="clean_ip" name="ip" Text="IpAddress"/>
                    <asp:Label runat="server" id="clean_score" name="score" Text="Score"/>
                    </br>
                    <asp:Label runat="server" id="filter_os" name="os" Text="Operating System"/>
                    <asp:Label runat="server" id="filter_browser" name="browser" Text="Browser"/>
                    <asp:Label runat="server" id="filter_ip" name="ip" Text="IpAddress"/>
                    <asp:Label runat="server" id="filter_score" name="score" Text="Score"/>
                </div>
        

            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="the_hidden" style="visibility:hidden;">
            <asp:Button ID="btnSendToGoog" runat="server" style="margin-top:20px !important;" Text="Send Result" CssClass="btn btn-success"/>
               
                <br />  

            <label id="lblCleanCaptchaMessage" runat="server" clientidmode="static"></label> </br>
            <label id="lblFilterCaptchaMessage" runat="server" clientidmode="static"></label> </br>
            <asp:Textbox runat="server" type="text" id="captchatoken" name="captchatoken" style="visibility:hidden;" />
        
            </br>
        
        </div>
                <div id="thankyou" style="visibility:hidden;float:right;">
                            Thank You!
                </div>

 <%--           <asp:Label runat="server" id="browser" name="browser" Text="Browser"/>--%>


    </div>
    <script type="text/javascript">

        $(document).ready(function () {
            $("#traffic_data").css('visibility', 'hidden');
        });

        function ShowStuff(){
            $("#the_hidden").css('visibility', 'visible');
            grecaptcha.execute('6LdUdpgUAAAAAFnuf6lPVa_oMBU6lUyLoOUIOCtu', { action: 'test' })
                .then(function (token) {
                    // Verify the token on the server.
                    console.log(token);
                    $('#MainContent_captchatoken').val(token);
                });
        };

        $("#MainContent_btnSendToGoog").click(function () {
            $("#thankyou").css('visibility', 'visible');
            alert('Thank You! You may now close this page');
            return false;
        });

    </script>
</asp:Content>
