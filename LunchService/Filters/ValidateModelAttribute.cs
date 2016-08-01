using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LunchService.Filters
{
  public class ValidateModelAttribute : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      if (filterContext.ModelState.IsValid == false)
      {
        filterContext.Result = new BadRequestObjectResult(filterContext.ModelState);
      }
    }
  }
}
