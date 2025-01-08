using AspnetCoreMvcFull.Core.Context;
using AspnetCoreMvcFull.Models.User;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// تنظیمات EF Core و دیتابیس
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// تنظیم Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// تنظیم احراز هویت
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
      options.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;
      options.LoginPath = "/Auth/LoginBasic"; // تغییر مسیر صفحه لاگین
      options.AccessDeniedPath = "/Auth/AccessDenied";
      options.Cookie.HttpOnly = true;
      options.ExpireTimeSpan = TimeSpan.FromHours(12);
      options.SlidingExpiration = true;
      options.Cookie.SecurePolicy = CookieSecurePolicy.None;
    })
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
      };
    });

// تنظیمات Data Protection
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("./keys"))
    .SetApplicationName("AspNetCoreMvcFull");

// اضافه کردن سرویس‌ها
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
  {
    Title = "My API",
    Version = "v1"
  });

  // فیلتر برای محدود کردن به ApiControllers
  c.DocInclusionPredicate((docName, apiDesc) =>
  {
    return apiDesc.ActionDescriptor is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor descriptor
           && descriptor.ControllerTypeInfo.GetCustomAttributes(typeof(ApiControllerAttribute), true).Any();
  });
});

var app = builder.Build();

// پیکربندی Pipeline
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
  options.SwaggerEndpoint("/swagger/v1/swagger.json", "RCT API V1");
  options.RoutePrefix = "swagger";
});

// Middleware برای بررسی وضعیت احراز هویت
app.Use(async (context, next) =>
{
  var user = context.User;
  Console.WriteLine($"IsAuthenticated: {user.Identity.IsAuthenticated}");
  Console.WriteLine($"Name: {user.Identity.Name}");
  await next.Invoke();
});

app.UseRouting();

// استفاده از احراز هویت و مجوزها
app.UseAuthentication();
app.UseAuthorization();

// اجرای مایگریشن پایگاه داده (اختیاری)
using (var scope = app.Services.CreateScope())
{
  var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
  dbContext.Database.Migrate();
}

// تنظیم مسیر پیش‌فرض
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboards}/{action=Index}/{id?}");

app.Run();
