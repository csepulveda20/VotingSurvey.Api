using MediatR;
using Microsoft.AspNetCore.Mvc;
using VotingSurvey.Application.Models;
namespace VotingSurvey.Presentation.Controllers
{
    [ApiController]
    public abstract class ApiBaseController : ControllerBase
    {
        protected ISender Sender => HttpContext.RequestServices.GetRequiredService<ISender>();

        protected IActionResult Problem(IReadOnlyList<string>? errors = null)
        {
            return errors != null && errors.Count != 0 ? UnprocessableEntity(ApiResponse<string>.Failure(errors)) : base.Problem();
        }

        protected IActionResult Problem(IReadOnlyDictionary<string, string>? errors)
        {
            return errors != null && errors.Count != 0 ? UnprocessableEntity(ApiResponse<string>.Failure(errors)) : base.Problem();
        }
    }
}