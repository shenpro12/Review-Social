using Microsoft.AspNetCore.Http;
using review.Common.Constatnts;
using System.Linq;
using System.Security.Claims;

namespace review.Common.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = ((ClaimsIdentity)claimsPrincipal.Identity).Claims.Single(x => x.Type == ClaimConstant.ID);
            return claim.Value;
        }

        public static string GetUserId(IHttpContextAccessor httpContextAccessor)
        {
            var claim = httpContextAccessor.HttpContext.User;
            return claim.GetUserId();
        }
        //
        public static bool IsAdmin(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = ((ClaimsIdentity)claimsPrincipal.Identity).Claims.Single(x => x.Type == ClaimConstant.Admin);
            return claim.Value == "True";
        }
        public static bool IsAdmin(IHttpContextAccessor httpContextAccessor)
        {
            var claim = httpContextAccessor.HttpContext.User;
            return claim.IsAdmin();
        }
        //
        public static string GetUserEmail(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = ((ClaimsIdentity)claimsPrincipal.Identity).Claims.Single(x => x.Type == ClaimConstant.Email);
            return claim.Value;
        }
        public static string GetUserEmail(IHttpContextAccessor httpContextAccessor)
        {
            var claim = httpContextAccessor.HttpContext.User;
            return claim.GetUserEmail();
        }       
    }
}