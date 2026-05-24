using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Delivery.Autenticacao
{
    public class SessionAuthorizeAttribute : ActionFilterAttribute
    {
        public string? RoleAnyOf { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var http = context.HttpContext;

            var userId = http.Session.GetInt32(SessionKeys.UserId);
            var role = http.Session.GetString(SessionKeys.UserRole);

            if (userId == null)
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
                return;
            }

            if (!string.IsNullOrWhiteSpace(RoleAnyOf))
            {
                var allowed = RoleAnyOf.Split(',');

                if (!allowed.Contains(role))
                {
                    context.Result = new RedirectToActionResult("AcessoNegado", "Home", null);
                    return;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}