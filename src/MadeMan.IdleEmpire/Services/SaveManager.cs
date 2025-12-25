using System.Text.Json;
using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Services;

public class SaveManager
{
    private const string SaveKey = "gamestate_v1";

    public void Save(GameState state)
    {
        try
        {
            state.LastPlayedUtc = DateTime.UtcNow;
            var json = JsonSerializer.Serialize(state);
            Preferences.Default.Set(SaveKey, json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Save failed: {ex.Message}");
        }
    }

    public GameState? Load()
    {
        try
        {
            var json = Preferences.Default.Get(SaveKey, string.Empty);
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            return JsonSerializer.Deserialize<GameState>(json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Load failed: {ex.Message}");
            return null;
        }
    }

    public void Delete()
    {
        try
        {
            Preferences.Default.Remove(SaveKey);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Delete failed: {ex.Message}");
        }
    }
}
