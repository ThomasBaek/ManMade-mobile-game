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

		// Game Engine (uses IServiceProvider for lazy skill service resolution)
		builder.Services.AddSingleton<IGameEngine, GameEngine>();

		// Skill Services (registered AFTER GameEngine, get state from engine)
		builder.Services.AddSingleton<ISkillService>(sp =>
		{
			var engine = sp.GetRequiredService<IGameEngine>();
			return new SkillService(() => engine.State);
		});
		builder.Services.AddSingleton<IMilestoneService>(sp =>
		{
			var engine = sp.GetRequiredService<IGameEngine>();
			var skillService = sp.GetRequiredService<ISkillService>();
			return new MilestoneService(() => engine.State, skillService);
		});

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
