using Microsoft.AspNetCore.Mvc.Filters;

namespace WW.OpenAPI.Filters;

public class AuthorizationFiter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Do something before the action executes.
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Do something after the action executes.
    }
}
//todo:ดูวิธีการใช้ใน https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-6.0#authorization-filters
