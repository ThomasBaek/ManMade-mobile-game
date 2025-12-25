using Microsoft.Extensions.Logging;
using MadeMan.IdleEmpire.Services;
using MadeMan.IdleEmpire.ViewModels;

namespace MadeMan.IdleEmpire;

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
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Services
		builder.Services.AddSingleton<SaveManager>();
		builder.Services.AddSingleton<IGameEngine, GameEngine>();

		// ViewModels
		builder.Services.AddTransient<MainViewModel>();

		// Pages
		builder.Services.AddTransient<Views.MainPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
