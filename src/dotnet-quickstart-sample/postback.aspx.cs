using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using System.Web.Mvc;
using System.IO;

using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;

using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace ClaimAppDemo01
{
    public partial class postback : System.Web.UI.Page
    {
        public string _HTML = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            var tenantid = "";
            var userid = "";
            var emailaddress = "";
            var givenname = "";
            var surname = "";
            var displayname = "";
            var state = "";
            var nonce = "";
            var redirecturi = "";

            string rawJwtData = Request["token"];

            // TODO 1: check referer is from Auth Svc
            // TODO 2: check the state in the token is a valid state in your application
            // TODO 3:
            // create you own application login token hereusing the token or the information you get from Auth Svc
            // redirect to the page you want after login


            if (!string.IsNullOrEmpty(rawJwtData))
            {
                //if (ValidateJwtToken(rawJwtData)) // check jwt signature and expiration
                //{
                    // split out the "parts" (header, payload and signature)
                    string[] parts = rawJwtData.Split('.');
                    string headerJson = parts[0];
                    string payloadJson = parts[1];
                    string signatureJson = parts[2];

                    // Resolve Invalid length for a Base-64 char array or string
                    payloadJson = payloadJson.Replace(" ", "+");
                    int mod4 = payloadJson.Length % 4;
                    if (mod4 > 0)
                    {
                        payloadJson += new string('=', 4 - mod4);
                    }

                    byte[] data = Convert.FromBase64String(payloadJson);
                    string decodedString = Encoding.UTF8.GetString(data);

                    JObject o = JObject.Parse(decodedString);
                    tenantid = o["tenantid"].ToString();
                    userid = o["userid"].ToString();
                    emailaddress = o["emailaddress"].ToString();
                    givenname = o["givenname"].ToString();
                    surname = o["surname"].ToString();
                    displayname = o["displayname"].ToString();
                    state = o["state"].ToString();
                    nonce = o["nonce"].ToString();
                    redirecturi = o["redirecturi"].ToString();
                //}
            }


            // form html

            _HTML = "<b>Dot Net - Login successfully.</b> <br/><br/><b>Token Information:</b><br/>" +
                "userid: " + userid + "<br/>" +
                "emailaddress: " + emailaddress + "<br/>" +
                " givenname: " + givenname + "<br/>" +
                " surname: " + surname + "<br/>" +
                " displayname: " + displayname + "<br/>" +
                " tenantid: " + tenantid + "<br/>" +
                " nonce: " + nonce + "<br/>" +
                " redirecturi: " + redirecturi + "<br/>" +
                " state: " + state + "<br/>";


        }



        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public bool ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;

            var key = Encoding.ASCII.GetBytes("[SECRET USED TO SIGN AND VERIFY JWT TOKENS, IT CAN BE ANY STRING]");
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                //var accountId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // return account id from JWT token if validation successful
                return true;
            }
            catch
            {
                // return null if validation fails
                // return null;
                return false;
            }
        }
    }
}