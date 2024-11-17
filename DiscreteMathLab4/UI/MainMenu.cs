using Ardalis.SmartEnum;
using GraphLib.GraphTypes;
using GraphLib.UI;
using Shared;
using Shared.AnsiConsole;
using Spectre.Console;


namespace DiscreteMathLab4.UI;
public class MainMenu {
    private Optional<Graph> _graph = Optional<Graph>.Empty();

    private EulerianLoopConstruction eulerianLoopConstruction { get; init; }
    private InputMatrixMenu parseMatrixMenu { get; init; }

    private IAnsiConsole console;

    public MainMenu(IAnsiConsole console) {
        eulerianLoopConstruction = new(console);
        parseMatrixMenu = new(console);
        this.console = console;
    }
    public void Run() {
        RunLoop();
    }


    private sealed class MenuOption : SmartEnum<MenuOption> {
        public static readonly MenuOption InputAdjacencyMatrix = new(nameof(InputAdjacencyMatrix), 1, "Ввод графа с помощью матрицы смежности");
        public static readonly MenuOption ShowViews = new(nameof(ShowViews), 2, "Построение цикла Эйлера");
        public static readonly MenuOption Exit = new(nameof(Exit), 3, "Выход");

        private MenuOption(string name, int value, string display) : base(name, value) {
            Display = display;
        }

        public string Display { get; }

    }

    private void RunLoop() {
        var isExit = false;
        while (true) {
            var choice = console.Prompt(
                new SelectionPrompt<MenuOption>() { Converter = value => value.Display }
                    .Title("[bold]Главное меню[/]")
                    .PageSize(10)
                    .AddChoices(SmartEnumHelper.SortSmartEnumByValue<MenuOption>()));


            choice
             .When(MenuOption.InputAdjacencyMatrix).Then(() => {

                 var isOverwriteGraph = false;

                 if (_graph.HasValue) {
                     isOverwriteGraph = console.Prompt(
                         new SelectionPrompt<bool> { Converter = value => value ? "Да" : "Нет" }
                             .Title("Матрица уже была введена. Продолжить?")
                             .AddChoices(true, false));


                 }

                 if (isOverwriteGraph || _graph.IsEmpty) {
                     HandleRequesGraph();
                 }

             })
             .When(MenuOption.ShowViews).Then(() => {
                 if (_graph.IsEmpty) {
                     console.Write("Граф ещё не введён".FormatException());
                     HandleRequesGraph();
                 }
                 else {
                     eulerianLoopConstruction.Construct(_graph.Value);
                 }

             })
             .When(MenuOption.Exit).Then(() => {
                 isExit = true;
             });

            if (isExit) break;
        }
    }

    private void HandleRequesGraph() {
        _graph = parseMatrixMenu.RequestGraph();
    }
}
