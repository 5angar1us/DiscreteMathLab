using Ardalis.SmartEnum;
using GraphLib.GraphTypes;
using GraphLib.UI;
using Shared;
using Spectre.Console;

namespace DiscreteMathLab3.UI;

public class ShowViewsMenu {
    private IAnsiConsole console { get; init; }
    private CliDisplay cliDisplay { get; init; }

    public ShowViewsMenu(IAnsiConsole console) {
        this.console = console;
        cliDisplay = new CliDisplay(console);
    }

    private sealed class MenuOption : SmartEnum<MenuOption> {
        public static readonly MenuOption All = new(nameof(AdjacencyMatrix), 1, "Все");
        public static readonly MenuOption AdjacencyMatrix = new(nameof(AdjacencyMatrix), 2, "Матрица смежности");
        public static readonly MenuOption IncidentMatrix = new(nameof(IncidentMatrix), 3, "Матрица инциденций");
        public static readonly MenuOption RelationshipLists = new(nameof(RelationshipLists), 4, "Списки смежности");
        public static readonly MenuOption IncidentsLists = new(nameof(IncidentsLists), 5, "Списки индениций");
        public static readonly MenuOption Back = new(nameof(Back), 6, "Назад");

        private MenuOption(string name, int value, string display) : base(name, value) {
            Display = display;
        }

        public string Display { get; }

    }

    public void Show(Graph graph) {

        var isExit = false;
        while (true) {
            var choice = console.Prompt(
            new SelectionPrompt<MenuOption>() { Converter = value => value.Display }
                .Title("[bold]Выберите представление[/]")
                .PageSize(10)
                .AddChoices(SmartEnumHelper.SortSmartEnumByValue<MenuOption>()));

            choice
              .When(MenuOption.All).Then(() => {
                  cliDisplay.Show(graph.ToInputAdjacencyMatrix());
                  cliDisplay.Show(graph.ToIncidentMatrix());
                  cliDisplay.Show(graph.ToRelationshipLists());
                  cliDisplay.Show(graph.ToIncidentsLists());

              })
             .When(MenuOption.AdjacencyMatrix).Then(() => {
                 cliDisplay.Show(graph.ToInputAdjacencyMatrix());
             })
             .When(MenuOption.IncidentMatrix).Then(() => {
                 cliDisplay.Show(graph.ToIncidentMatrix());
             })
             .When(MenuOption.RelationshipLists).Then(() => {
                 cliDisplay.Show(graph.ToRelationshipLists());
             })
             .When(MenuOption.IncidentsLists).Then(() => {
                 cliDisplay.Show(graph.ToIncidentsLists());
             })
             .When(MenuOption.Back).Then(() => { isExit = true; });

            if (isExit) break;
        }
    }
}

