﻿using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Error;

namespace UserService.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/error")]
    public class ErrorController : ControllerBase
    {
        [HttpPost]
        public IActionResult HandleError()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
            return exception switch
            {
                BadHttpRequestException => Problem(
                    detail: exception.Message,
                    statusCode: 400,
                    title: "Action failed"
                    ),
                WrongUsernameOrPasswordException => Problem(
                    detail: exception.Message,
                    statusCode: 401,
                    title: "Invalid credential"
                    ),
                _ => Problem(
                    detail: "Internal server error",
                    statusCode: 500,
                    title: "Internal server error")
            };
        }
    }
}
