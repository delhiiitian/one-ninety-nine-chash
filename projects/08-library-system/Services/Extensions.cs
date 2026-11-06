using LibrarySystem.Domain;

namespace LibrarySystem.Services;

public static class DisplayExtensions
{
    public static string Badge(this Member member) => member.IsPremium ? "premium" : "standard";

    public static string ShortDate(this DateOnly date) => date.ToString("dd MMM");

    public static string Pluralize(this int count, string word) => $"{count} {word}{(count == 1 ? "" : "s")}";
}
