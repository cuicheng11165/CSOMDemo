using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace CSOMWebService
{
    class Program
    {
        static void Main(string[] args)
        {
            var securityString = new SecureString();
            foreach (var tmpChar in "passord")
            {
                securityString.AppendChar(tmpChar);
            }

            string siteUrl = "https://aveulm001.sharepoint.com";
            string serviceLocation = "/_vti_bin/UserProfileService.asmx";

            var cred = new SharePointOnlineCredentials("admin@aveulm001.onmicrosoft.com", securityString);

            var client = new SPUserProfileService.UserProfileServiceSoapClient("UserProfileServiceSoap");
            client.Endpoint.Address = new EndpointAddress(new Uri(siteUrl + serviceLocation));


            using (new OperationContextScope(client.InnerChannel))
            {
                HttpRequestMessageProperty request = new HttpRequestMessageProperty();
                request.Headers["Cookie"] = cred.GetAuthenticationCookie(new Uri(siteUrl));
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = request;
                var schema = client.GetUserProfileSchema();
            }
        }
    }
}
