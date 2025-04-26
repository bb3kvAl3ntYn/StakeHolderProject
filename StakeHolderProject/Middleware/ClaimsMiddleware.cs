using StakeHolderProject.Services.Interfaces;

namespace StakeHolderProject.Middleware
{
    public class ClaimsMiddleware
    {
        private readonly RequestDelegate _next;

        public ClaimsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IClaimsService claimsService)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                claimsService.Initialize(context.User);
            }

            await _next(context);
        }
    }

    public static class ClaimsMiddlewareExtensions
    {
        public static IApplicationBuilder UseClaimsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ClaimsMiddleware>();
        }
    }
} 