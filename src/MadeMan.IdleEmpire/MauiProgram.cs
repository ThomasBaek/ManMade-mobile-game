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

		// Core Services
		builder.Services.AddSingleton<SaveManager>();

		// State holder (breaks circular dependency - registered first)
		builder.Services.AddSingleton<GameStateHolder>();
		builder.Services.AddSingleton<IGameStateProvider>(sp => sp.GetRequiredService<GameStateHolder>());

		// Skill Services (depend on IGameStateProvider)
		builder.Services.AddSingleton<ISkillService, SkillService>();
		builder.Services.AddSingleton<IMilestoneService, MilestoneService>();

		// Game Engine (depends on GameStateHolder, SaveManager, and skill services)
		builder.Services.AddSingleton<IGameEngine, GameEngine>();

		// ViewModels (Singleton to prevent timer duplication and resource leaks)
		builder.Services.AddSingleton<SkillViewModel>();
		builder.Services.AddSingleton<MainViewModel>();
		builder.Services.AddTransient<SettingsViewModel>();

		// Pages
		builder.Services.AddTransient<Views.MainPage>();
		builder.Services.AddTransient<Views.OrgCrimePage>();
		builder.Services.AddTransient<Views.CasinoPage>();
		builder.Services.AddTransient<Views.SkillsTabPage>();
		builder.Services.AddTransient<Views.SettingsPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
