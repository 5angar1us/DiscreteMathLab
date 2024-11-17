using Spectre.Console;

namespace GraphLib.UI.InputMatrix.Manual;

internal class HideCursor : IDisposable {
    public HideCursor(IAnsiConsole console) {

        this.console = console;
        console.Cursor.Show(false);
    }

    private readonly IAnsiConsole console;

    public void Dispose() {
        console.Cursor.Show(true);
    }
}
