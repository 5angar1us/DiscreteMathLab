namespace Shared.AnsiConsole;

public static class ColorMessages {
    public static string FormatSuccess(this string message) {
        return $"[green]{message}[/]";
    }

    public static string FormatException(this string message) {
        return $"[red]{message}[/]";
    }
}
