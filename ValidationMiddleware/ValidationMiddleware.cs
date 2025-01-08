using AspnetCoreMvcFull.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace AspnetCoreMvcFull.ValidationMiddleware
{
  public class ValidationMiddleware
  {
    private readonly RequestDelegate _next;

    public ValidationMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      // ذخیره جریان اصلی پاسخ
      var originalBodyStream = context.Response.Body;

      using (var responseBody = new MemoryStream())
      {
        context.Response.Body = responseBody;

        // ادامه پردازش Middleware بعدی
        await _next(context);

        // اگر وضعیت 400 است، بررسی کنیم که آیا خطای اعتبارسنجی است
        if (context.Response.StatusCode == StatusCodes.Status400BadRequest)
        {
          responseBody.Seek(0, SeekOrigin.Begin);
          var bodyText = await new StreamReader(responseBody).ReadToEndAsync();

          if (IsValidationError(bodyText))
          {
            // بازنویسی پاسخ خطای اعتبارسنجی
            var errorResponse = ExtractValidationErrors(bodyText);

            // بازنشانی پاسخ
            context.Response.ContentType = "application/json";
            context.Response.Body = originalBodyStream;
            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new ResultViewModel(errorResponse)));
            return;
          }
        }

        // کپی پاسخ اصلی
        responseBody.Seek(0, SeekOrigin.Begin);
        await responseBody.CopyToAsync(originalBodyStream);
      }
    }

    private bool IsValidationError(string responseBody)
    {
      // بررسی اینکه آیا پاسخ شامل کلیدهای خاص خطای اعتبارسنجی است
      return responseBody.Contains("\"traceId\"");
    }

    private List<string> ExtractValidationErrors(string responseBody)
    {
      // استخراج خطاها از پاسخ
      var jsonDoc = JsonDocument.Parse(responseBody);
      var errors = new List<string>();

      if (jsonDoc.RootElement.TryGetProperty("errors", out var errorElement))
      {
        foreach (var error in errorElement.EnumerateObject())
        {
          errors.AddRange(error.Value.EnumerateArray().Select(e => e.GetString() ?? "").ToList());
        }
      }

      return errors;
    }
  }

}
