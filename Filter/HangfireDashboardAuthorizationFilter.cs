using Hangfire.Dashboard;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Owin;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Text;

namespace Orquestror.Filter;

public class HangfireDashboardAuthorizationFilter : IAuthorizationFilter, IDashboardAuthorizationFilter

{
    public HangfireDashboardAuthorizationFilter()
    { }

    public bool Authorize([NotNull] DashboardContext context)
    {
        //return Authorize(context.GetOwinEnvironment());
        return true;
    }

    private bool CheatChallenge(OwinContext context)
    {
        return true;
    }

    private bool Challenge(OwinContext context)
    {
        context.Response.StatusCode = 401;
        context.Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Hangfire Dashboard\"");

        context.Response.Write("Authentication is required.");

        return false;
    }

    public bool Authorize([NotNull] IDictionary<string, object> owinEnvironment)
    {
        OwinContext context = new OwinContext(owinEnvironment);
#if DEBUG
            return true;
#endif
        string header = context.Request.Headers["Authorization"];

        if (string.IsNullOrWhiteSpace(header) == false)
        {
            AuthenticationHeaderValue authValues = AuthenticationHeaderValue.Parse(header);

            if ("Basic".Equals(authValues.Scheme, StringComparison.InvariantCultureIgnoreCase))
            {
                string parameter = Encoding.UTF8.GetString(Convert.FromBase64String(authValues.Parameter));
                var parts = parameter.Split(':');

                if (parts.Length > 1)
                {
                    string login = parts[0];
                    string password = parts[1];

                    if ((string.IsNullOrWhiteSpace(login) == false) && (string.IsNullOrWhiteSpace(password) == false))
                    {
                        if (login.Equals("WFDROOT") && password.Equals("IndustrialEngineering123!")) { return CheatChallenge(context); }
                    }
                }
            }
        }
        return false;
        //return Challenge(context);
    }

    //private bool Challenge(OwinContext context)
    //{
    //    context.Response.StatusCode = 401;
    //    context.Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Hangfire Dashboard\"");

    //    context.Response.Write("Authentication is required.");

    //    return false;
    //}

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        throw new NotImplementedException();
    }
}