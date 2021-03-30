using BasicAuthenticationWebAPI.Authenticate;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BasicAuthenticationWebAPI.Filters
{

    public class BasicAuthenticationAttribute :
        AuthorizationFilterAttribute
    {

        private const string Realm = "My Realm";

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response =
                        actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);

                if (actionContext.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    actionContext.Response.Headers.Add("WWW-Authrnticate", string.Format("Basic realm=\"{0}\"", Realm));
                }
            }
            else
            {
                string authenticationToken = actionContext.Request.Headers
                                                    .Authorization.Parameter;

                string decodeAuthenticationToken =
                    Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));

                string[] usernamePasswordArray = decodeAuthenticationToken.Split(':');

                string username = usernamePasswordArray[0];

                string password = usernamePasswordArray[1];

                if (UserValidate.Login(username, password))
                {
                    var identity = new GenericIdentity(username);

                    IPrincipal principal = new GenericPrincipal(identity, null);

                    Thread.CurrentPrincipal = principal;

                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.User = principal;
                    }
                }
                else
                {
                    actionContext.Response = actionContext.Request.
                        CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
        }
    }
}