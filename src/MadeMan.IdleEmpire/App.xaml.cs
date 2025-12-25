using MadeMan.IdleEmpire.Services;

namespace MadeMan.IdleEmpire;

public partial class App : Application
{
	private readonly IGameEngine _gameEngine;
	private readonly SaveManager _saveManager;

	public App(IGameEngine gameEngine, SaveManager saveManager)
	{
		InitializeComponent();
		_gameEngine = gameEngine;
		_saveManager = saveManager;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}

	protected override void OnSleep()
	{
		base.OnSleep();
		_saveManager.Save(_gameEngine.State);
	}

	protected override void OnResume()
	{
		base.OnResume();
		// Offline earnings beregnes automatisk i Initialize
		// som kaldes fra MainViewModel
	}
}
