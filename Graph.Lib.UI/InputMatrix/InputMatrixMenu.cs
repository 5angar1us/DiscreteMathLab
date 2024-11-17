using Ardalis.SmartEnum;
using GraphLib.GraphDomain;
using GraphLib.GraphTypes;
using GraphLib.UI.InputMatrix.FromFile;
using GraphLib.UI.InputMatrix.Manual;
using Shared;
using Spectre.Console;

namespace GraphLib.UI;

public class InputMatrixMenu {

    private IAnsiConsole console;
    private ManualInputMatrix manualInputMatrix = new();
    private FromFileInputMatrix fromFileInputMatrixMenu = new();

    public InputMatrixMenu(IAnsiConsole console) {
        this.console = console;
    }

    private sealed class MenuOption : SmartEnum<MenuOption> {
        public static readonly MenuOption ManualInput = new(nameof(ManualInput), 1, "Ручной");
        public static readonly MenuOption InputFromFile = new(nameof(InputFromFile), 2, "Из файла");
        public static readonly MenuOption Back = new(nameof(Back), 3, "Назад");

        private MenuOption(string name, int value, string display) : base(name, value) {
            Display = display;
        }

        public string Display { get; }

    }

    public Optional<Graph> RequestGraph() {
        var isNeedExit = false;
        while (true) {
            var choice = AnsiConsole.Prompt(
        new SelectionPrompt<MenuOption>() { Converter = value => value.Display }
            .Title("[bold]Выберите способ ввода матрицы смежности[/]")
        .PageSize(10)
            .AddChoices(SmartEnumHelper.SortSmartEnumByValue<MenuOption>()));


            Optional<AdjacencyMatrix> matrix = Optional<AdjacencyMatrix>.Empty();

            choice
             .When(MenuOption.ManualInput).Then(() => {
                 matrix = manualInputMatrix.Show(console);
                 isNeedExit = matrix.Equals(Optional<AdjacencyMatrix>.Empty());
             })
             .When(MenuOption.InputFromFile).Then(() => {
                 matrix = fromFileInputMatrixMenu.Show(console);
                 isNeedExit = matrix.Equals(Optional<AdjacencyMatrix>.Empty());
             })
             .When(MenuOption.Back).Then(() => { isNeedExit = true; });


            if (matrix.HasValue) {
                return matrix.Value.ToGraph();
            }
            else {
                if (isNeedExit) return Optional<Graph>.Empty();
            }
        }
    }
}
