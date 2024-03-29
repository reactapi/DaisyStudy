﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DaisyStudy.Controllers;
public class BaseController : Controller
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var sessions = context.HttpContext.Session.GetString("Session");
        if (sessions == null)
        {
            context.Result = new RedirectResult("/Identity/Account/Login");
        }
        base.OnActionExecuting(context);
    }
}