using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Services;

/// <summary>
/// Provides access to game state. Used to break circular dependency
/// between GameEngine and skill services.
/// </summary>
public interface IGameStateProvider
{
    GameState State { get; }
}
