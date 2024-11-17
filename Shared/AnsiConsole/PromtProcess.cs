using Spectre.Console;

namespace Shared.AnsiConsole;

public interface IPromptProcess<T> {
    T Show(IAnsiConsole console);
}
