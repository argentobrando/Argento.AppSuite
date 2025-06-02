using Microsoft.AspNetCore.Components.WebView.Maui;
using Shared.Clients;
using Shared.Models;
using Shared.UI; // Optional — so components can be used in MainPage if needed

namespace ArgentoWindowsApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();

		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();

		// Example service registration:
		// builder.Services.AddScoped<WindowsAuthSessionService>();

		// Example: Add HttpClient for AuthServiceClient

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif

		return builder.Build();
	}
}
