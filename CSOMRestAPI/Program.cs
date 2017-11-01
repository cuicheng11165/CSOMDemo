using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace CSOMRestAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            var securityPassword = new SecureString();
            foreach (var c in "passord")
            {
                securityPassword.AppendChar(c);
            };

            var credentials = new SharePointOnlineCredentials("admin@aveulm001.onmicrosoft.com", securityPassword);

            ClientContext context = new ClientContext("https://aveulm001.sharepoint.com");
            context.Credentials = credentials;
            var formDigest = context.GetFormDigestDirect();

            string url = "https://aveulm001.sharepoint.com/_api/sitepages/publishingsite/create";

            var webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = "POST";
            webRequest.Accept = "application/json;odata=verbose";
            webRequest.ContentType = "application/json;odata=verbose";
            webRequest.Headers.Add("X-RequestDigest", formDigest.DigestValue);

            var cookie = credentials.GetAuthenticationCookie(new Uri(url));
            var index = cookie.IndexOf("=");

            webRequest.CookieContainer = new CookieContainer();
            webRequest.CookieContainer.Add(new Cookie(cookie.Substring(0, index), cookie.Substring(index + 1), "/", new Uri(url).Host));

            var buffer = Encoding.UTF8.GetBytes("{\"request\":{\"__metadata\":{\"type\":\"SP.Publishing.PublishingSiteCreationRequest\"},\"Title\":\"1019\",\"Url\":\"https://aveulm001.sharepoint.com/sites/10191\",\"Description\":\"1019\",\"Classification\":\"Da Lian\",\"SiteDesignId\":\"6142d2a0-63a5-4ba0-aede-d9fefca2c767\",\"lcid\":1033,\"AllowFileSharingForGuestUsers\":false,\"WebTemplateExtensionId\":\"00000000-0000-0000-0000-000000000000\"}}");
            var stream = webRequest.GetRequestStream();
            stream.Write(buffer, 0, buffer.Length);
            stream.Close();

            var response = webRequest.GetResponse() as HttpWebResponse;

            var ret = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
    }
}
