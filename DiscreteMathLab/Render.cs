using Spectre.Console;

namespace DiscreteMathLab;

internal class Render {
    public static void View(List<TruthTableItem> trughTable) {
        var tableDisplay = new Table();

        tableDisplay.AddColumn("a");
        tableDisplay.AddColumn("b");
        tableDisplay.AddColumn("c");
        tableDisplay.AddColumn("d");
        tableDisplay.AddColumn("e");
        tableDisplay.AddColumn("f");

        foreach (var row in trughTable) {
            var rowValues = row.GetAll()
                .Select(BoolToInt)
                .ToArray();

            tableDisplay.AddRow(
               rowValues
            );
        }

        AnsiConsole.Write(tableDisplay);
    }

    static string BoolToInt(bool value) {
        return value ? "1" : "0";
    }
}
