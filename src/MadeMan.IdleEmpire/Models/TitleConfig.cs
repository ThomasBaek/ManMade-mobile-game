namespace MadeMan.IdleEmpire.Models;

public static class TitleConfig
{
    public static readonly TitleLevel[] Titles = new[]
    {
        new TitleLevel(0, "Street Punk", "You are nobody... yet"),
        new TitleLevel(1, "Pickpocket", "Stealing to get by"),
        new TitleLevel(2, "Thug", "People start to fear you"),
        new TitleLevel(3, "Enforcer", "You collect debts for others"),
        new TitleLevel(5, "Soldato", "You are part of the family now"),
        new TitleLevel(7, "Caporegime", "You lead your own crew"),
        new TitleLevel(10, "Underboss", "Only one above you now"),
        new TitleLevel(15, "Consigliere", "Advisor to the powerful"),
        new TitleLevel(20, "Godfather", "You ARE the family")
    };

    public static TitleLevel GetTitle(int prestigeLevel)
    {
        TitleLevel result = Titles[0];
        foreach (var title in Titles)
        {
            if (prestigeLevel >= title.RequiredPrestige)
            {
                result = title;
            }
            else
            {
                break;
            }
        }
        return result;
    }

    public static TitleLevel? GetNextTitle(int prestigeLevel)
    {
        foreach (var title in Titles)
        {
            if (title.RequiredPrestige > prestigeLevel)
            {
                return title;
            }
        }
        return null; // Already max title
    }
}

public record TitleLevel(int RequiredPrestige, string Name, string Description);
