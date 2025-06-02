using ArgentoWebApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.Cookie.HttpOnly = true;
		options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
		options.Cookie.SameSite = SameSiteMode.Strict;
		options.LoginPath = "/login";
	});

builder.Services.AddAuthorization();

builder.Services.AddScoped<JwtSessionService>();

var authServiceUrl = builder.Configuration["AuthService:BaseUrl"];

builder.Services.AddHttpClient<AuthServiceClient>(client =>
{
	client.BaseAddress = new Uri(authServiceUrl);
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();