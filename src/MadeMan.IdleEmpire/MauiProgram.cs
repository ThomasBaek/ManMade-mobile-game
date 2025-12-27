using Microsoft.Extensions.Logging;
using MadeMan.IdleEmpire.Services;
using MadeMan.IdleEmpire.ViewModels;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace MadeMan.IdleEmpire;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseSkiaSharp()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("BebasNeue-Regular.ttf", "BebasNeue");
				fonts.AddFont("Inter-Regular.ttf", "Inter");
				fonts.AddFont("Inter-SemiBold.ttf", "InterSemiBold");
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

		// Pages (Transient - Shell creates them via ContentTemplate)
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
