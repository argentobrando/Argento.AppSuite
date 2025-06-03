using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.Clients;

namespace ArgentoMAUIApp;

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

		// Add appsettings.json
		builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

		// Read AuthService BaseUrl from config
		var authServiceBaseUrl = builder.Configuration["AuthService:BaseUrl"];

		builder.Services.AddHttpClient<AuthServiceClient>(client =>
		{
			client.BaseAddress = new Uri(authServiceBaseUrl);
		});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
