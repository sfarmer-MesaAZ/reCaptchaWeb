using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Hosting;

namespace recaptchaWeb
{
    public class IPAddressRangeChecker
    {
        public bool IsUserIPAddressExcepted(IPAddress useripaddress)
        {
            int rangecount = 0;
            bool result = false;
            //get list of networks to test
            var networklist = GetExceptedNetworks();
            foreach (var nw in networklist)
            {
                if (nw.IPAddressRange.IsInRange(useripaddress))
                    rangecount++;
            }

            return result = rangecount > 0 ? true : false;
        }

        public List<Scope> GetExceptedNetworks()
        {
            var scopes = new List<Scope>();
            try
            {
                //Server.MapPath(@"~/App_Data/file.txt"));
                using (StreamReader r = new StreamReader(HostingEnvironment.MapPath(@"/Config/Settings.json")))
                {
                    string json = r.ReadToEnd();
                    var jsonResult = JObject.Parse(json);
                    var rootobject = JsonConvert.DeserializeObject<Rootobject>(json);

                    //var ro = JsonConvert.DeserializeObject<Scope[]>(json);
                    //generate iprange
                    foreach (var s in rootobject.Scopes)
                    {
                        s.IPAddressRange = new IPAddressRange(0, IPAddress.Parse(s.Lower), IPAddress.Parse(s.Upper));
                        scopes.Add(s);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write($"ERROR: {ex.Message}");
            }
            return scopes;
        }


    }

    public class IPAddressRange
    {
        readonly AddressFamily addressFamily;
        readonly byte[] lowerBytes;
        readonly byte[] upperBytes;


        public IPAddressRange(int v, IPAddress lowerInclusive, IPAddress upperInclusive)
        {
            // Assert that lower.AddressFamily == upper.AddressFamily

            this.addressFamily = lowerInclusive.AddressFamily;
            this.lowerBytes = lowerInclusive.GetAddressBytes();
            this.upperBytes = upperInclusive.GetAddressBytes();
        }


        public bool IsInRange(IPAddress address)
        {
            if (address.AddressFamily != addressFamily)
            {
                return false;
            }

            byte[] addressBytes = address.GetAddressBytes();

            bool lowerBoundary = true, upperBoundary = true;

            for (int i = 0; i < this.lowerBytes.Length &&
                (lowerBoundary || upperBoundary); i++)
            {
                if ((lowerBoundary && addressBytes[i] < lowerBytes[i]) ||
                    (upperBoundary && addressBytes[i] > upperBytes[i]))
                {
                    return false;
                }

                lowerBoundary &= (addressBytes[i] == lowerBytes[i]);
                upperBoundary &= (addressBytes[i] == upperBytes[i]);
            }

            return true;
        }

    }



    public class Rootobject
    {
        public Scope[] Scopes { get; set; }
        public Scope Scope { get; set; }
        public Browserexception[] BrowserExceptions { get; set; }
    }

    public class Scope
    {
        public string Name { get; set; }
        public string Lower { get; set; }
        public string Upper { get; set; }
        public string MinimumScore { get; set; }
        public IPAddressRange IPAddressRange { get; set; }

    }

    public class Browserexception
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string MinimumScore { get; set; }
    }


}