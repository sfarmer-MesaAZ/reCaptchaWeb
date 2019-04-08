using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace recaptchaWeb
{
    public partial class _Default : Page
    {
        private const int NumberOfRetries = 3;
        private const int DelayOnRetry = 1000;
        private int i;
        private IPAddressRange TmobAddressRange     = new IPAddressRange(0, IPAddress.Parse("172.32.0.0"),IPAddress.Parse("172.63.255.255"));
        private IPAddressRange VerizonAddressRange  = new IPAddressRange(0,IPAddress.Parse("174.192.0.0"),IPAddress.Parse("174.255.255.255"));
        private IPAddressRange COMWifFiAddressRange = new IPAddressRange(0, IPAddress.Parse("199.101.32.0"),IPAddress.Parse("199.101.41.255"));
        private IPAddressRange ATTAddressRange      = new IPAddressRange(0,IPAddress.Parse("107.64.0.0"),IPAddress.Parse("107.127.255.255"));
        private IPAddressRange NobisAddressRange    = new IPAddressRange(0,IPAddress.Parse("23.83.128.0"),IPAddress.Parse("23.83.207.255"));

        CancellationTokenSource source = new CancellationTokenSource();

        protected void Page_Load(object sender, EventArgs e)
        {
            // IsReCaptchaValid();
        }


        protected void btnCleanCaptcha_Click(object sender, EventArgs e)
        {

            //Validate Captcha
            var captchaResult = IsReCaptchaValid();
            lblCleanCaptchaMessage.InnerHtml = captchaResult ? "<span style='color:green'>Verification success - please send this result!</span>" : "<span style='color:red'>Low Score - Please send this result!</span>";
            if (captchaResult)
            {

            }
            else { return; }
        }
        protected void btnCriterionCaptcha_Click(object sender, EventArgs e)
        {

            //Validate Captcha
            var filteredcaptchaResult = IsFilteredCaptchaValid();
            lblFilterCaptchaMessage.InnerHtml = filteredcaptchaResult ? "<span style='color:green'>Verification success - please send this result!</span>" : "<span style='color:red'>Low Score - Please send this result!</span>";
            if (filteredcaptchaResult)
            {

            }
            else { return; }
        }


        [WebMethod]
        public bool IsReCaptchaValid()
        {

            var result = false;
            var captchaResponse = captchatoken.Text;//Request.Form["g-recaptcha-response"];
                                                    //
            var secretKey = "6LdUdpgUAAAAAJoe6Fn4aRSQLum7Uc7li8bsObht";//ConfigurationManager.AppSettings["ReCaptchaKey"];
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);
            //Get browser type for IE exceptions (score = 0.1)
            string UserAgent = Request.ServerVariables["HTTP_USER_AGENT"];
            var browserRequest = HttpContext.Current.Request;
            var browserType = browserRequest.Browser.Type;
            double captchaScore = 0.0;
            using (WebResponse response = request.GetResponse())
            try
            {

                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    for (i = 1; i <= NumberOfRetries; ++i)
                    {
                        JObject jResponse = JObject.Parse(stream.ReadToEnd());
                        var isSuccess = jResponse.Value<bool>("success");
                        captchaScore = jResponse.Value<double>("score");
                        var action = jResponse.Value<string>("action");
                        if (browserType.Contains("InternetExplorer") | browserType.Contains("Firefox"))
                        {
                            result = (isSuccess && captchaScore >= 0.1 && action == "test") ? true : false;
                        }
                        else
                        {
                            result = (isSuccess && captchaScore >= 0.1 && action == "test") ? true : false;
                        }



                    }
                }
            }
            catch (Exception ex) when (i < NumberOfRetries)
            {
                var t = Task.Run(async delegate
                {
                    await Task.Delay(TimeSpan.FromSeconds(1.5), source.Token);
                    return 42;
                });
            }
            clean_os.Text       = browserRequest.UserHostName;
            clean_browser.Text  = browserType;
            clean_ip.Text       = browserRequest.UserHostAddress;
            clean_score.Text    = captchaScore.ToString();
            return result;
        }

        [WebMethod]
        public bool IsFilteredCaptchaValid()
        {
            var result = false;
            var captchaResponse = captchatoken.Text;                                                     
            var secretKey = "6LdUdpgUAAAAAJoe6Fn4aRSQLum7Uc7li8bsObht";//ConfigurationManager.AppSettings["ReCaptchaKey"];
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);
            //Get browser type for IE exceptions (score = 0.1)
            string UserAgent = Request.ServerVariables["HTTP_USER_AGENT"];
            var browserRequest = HttpContext.Current.Request;
            var browserType = browserRequest.Browser.Type;
            double filteredcaptchaScore = 0.0;
            using (WebResponse response = request.GetResponse())
                try
                {

                    using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                    {
                        for (i = 1; i <= NumberOfRetries; ++i)
                        {
                            JObject jResponse = JObject.Parse(stream.ReadToEnd());
                            var isSuccess = jResponse.Value<bool>("success");
                            filteredcaptchaScore = jResponse.Value<double>("score");
                            var action = jResponse.Value<string>("action");
                            //check ip ranges of known cellular networks first
                            var b = IPAddress.TryParse(browserRequest.UserHostAddress, out IPAddress userIPAddress);
                            if (
                                 !TmobAddressRange.IsInRange(userIPAddress) == true || !VerizonAddressRange.IsInRange(userIPAddress)
                                 || !COMWifFiAddressRange.IsInRange(userIPAddress) == true || !ATTAddressRange.IsInRange(userIPAddress)
                                 || !NobisAddressRange.IsInRange(userIPAddress) == true
                               )
                            {
                                if (browserType.Contains("InternetExplorer") || browserType.Contains("Firefox") || browserType.Contains("Safari"))
                                {
                                    result = (isSuccess && filteredcaptchaScore >= 0.1 && action == "test") ? true : false;
                                }
                                else
                                {
                                    result = (isSuccess && filteredcaptchaScore >= 0.1 && action == "test") ? true : false;
                                }
                            }
                            else // user ip falls into ranges of "safe" cellular/wifi ips
                            {
                                result = (isSuccess && action == "test") ? true : false;
                            }
                        }
                    }
                }
                catch (Exception ex) when (i < NumberOfRetries)
                {
                    var t = Task.Run(async delegate
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1.5), source.Token);
                        return 42;
                    });
                }
            filter_os.Text      = browserRequest.UserHostName;
            filter_browser.Text = browserType;
            filter_ip.Text      = browserRequest.UserHostAddress;
            filter_score.Text   = filteredcaptchaScore.ToString();
            return result;
        }
    }
}

  