using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace recaptchaWeb
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           // IsReCaptchaValid();
        }


        protected void btnDispositionReport_Click(object sender, EventArgs e)
        {

            //Validate Captcha
            var captchaResult = IsReCaptchaValid();
            lblCaptchaMessage.InnerHtml = captchaResult ? "<span style='color:green'>Captcha verification success</span>" : "<span style='color:red'>Please try your search again in a few seconds.</span>";
            if (captchaResult)
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
            var secretKey = "6LdBOpgUAAAAALMCzXpe60f7j-RWDJeVooNbPTq4";//ConfigurationManager.AppSettings["ReCaptchaKey"];
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);
            //Get browser type for IE exceptions (score = 0.1)
            string UserAgent = Request.ServerVariables["HTTP_USER_AGENT"];
            var browserRequest = HttpContext.Current.Request;
            var browserType = browserRequest.Browser.Type;
            double captchaScore;
            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
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
                        result = (isSuccess && captchaScore >= 0.7 && action == "test") ? true : false;
                    }

                }
            }
            os.Text         = browserRequest.UserHostName;
            browser.Text    = browserType;
            ip.Text         = browserRequest.UserHostAddress;
            score.Text      = captchaScore.ToString();
            return result;
        }

    }
}