using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Services;

/// <summary>
/// Holds game state and provides access via IGameStateProvider.
/// Registered as singleton to share state across services.
/// </summary>
public class GameStateHolder : IGameStateProvider
{
    public GameState State { get; set; } = new();
}
