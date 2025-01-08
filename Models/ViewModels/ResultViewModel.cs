using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspnetCoreMvcFull.Models.ViewModels
{
  public class ResultViewModel
  {
    private List<string> errorResponse;

    public bool Result { get; set; }
    public string Type { get; set; } // نوع خطا (مثلاً ModelState یا IdentityResult)
    public string Message { get; set; } // پیام کلی
    public List<string> Errors { get; set; } // لیست خطاها

    // سازنده برای ModelState
    public ResultViewModel(ModelStateDictionary modelState)
    {
      Result = false;
      Type = "ModelState";
      Message = "Validation errors occurred.";
      Errors = modelState.Values
          .SelectMany(v => v.Errors)
          .Select(e => e.ErrorMessage)
          .ToList();
    }

    // سازنده برای IdentityResult.Errors
    public ResultViewModel(IdentityResult identityResult)
    {
      Result = false;
      Type = "IdentityResult";
      Message = "Identity operation failed.";
      Errors = identityResult.Errors
          .Select(e => e.Description)
          .ToList();
    }

    public ResultViewModel(string message , bool result = true) {
      Type = "public";
      Message = message;
      Result = result;
    }



    public ResultViewModel(List<string> errorResponse)
    {
      Result = false;
      Type = "ModelState";
      Message = "Identity operation failed.";
      Errors = errorResponse;
    }
  }
}
