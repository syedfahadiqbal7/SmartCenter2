using Microsoft.AspNetCore.Http.Extensions;

namespace AFFZ_Customer.Utils
{
    public class SessionExpiryMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionExpiryMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (!context.Session.Keys.Contains("UserId"))
                {
                    // Check if the request is not for the login page to avoid redirect loop
                    var path = context.Request.Path.Value;
                    var isLoginPage = path == "/" || path == "/Login/Index" || path == "/Login/CustomersLogin" || path == "/Signup" || path == "/SignUp/CustomersRegister" || path == "/Login/ForgotPassword" || path == "/ResetPassword";

                    if (!isLoginPage)
                    {
                        if (path.StartsWith("/MerchantList/SelectedMerchantList"))
                        {
                            var returnUrl = Uri.EscapeDataString(context.Request.GetDisplayUrl());
                            var loginUrl = $"https://localhost:7195/Login/Index?returnUrl={returnUrl}";
                            context.Response.Redirect(loginUrl);
                            return;
                        }

                        // For other pages, redirect to the home page or dashboard
                        context.Response.Redirect("/");
                        return;
                    }
                }

                await _next(context);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    // Extension method to add the middleware to the pipeline
    public static class SessionExpiryMiddlewareExtensions
    {
        public static IApplicationBuilder UseSessionExpiryMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SessionExpiryMiddleware>();
        }
    }

}
