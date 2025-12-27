using MadeMan.IdleEmpire.Views;

namespace MadeMan.IdleEmpire;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// Register routes for settings sub-pages
		Routing.RegisterRoute("skillsguide", typeof(SkillsGuidePage));
		Routing.RegisterRoute("help", typeof(HelpPage));
	}
}
