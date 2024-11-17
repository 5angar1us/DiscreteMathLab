using Ardalis.SmartEnum;
using DiscreteMathLab2.Domain;
using DiscreteMathLab2.Menus;
using DiscreteMathLab2.UI;
using Shared;
using Shared.AnsiConsole;
using Spectre.Console;

public class MainMenu {
    public void Run() {
        _figures = Optional<Figures>.Empty();
        RunLoop();
    }
    private Optional<Figures> _figures { get; set; } = Optional<Figures>.Empty();

    private InputFigureMenu parseFigureMenu = new();
    private IsPointInSetMenu isPointInSetMenu = new();

    private sealed class MenuOption : SmartEnum<MenuOption> {
        public static readonly MenuOption InputFigures = new(nameof(InputFigures), 1, "Ввод фигур");
        public static readonly MenuOption IsPointInSeet = new(nameof(IsPointInSeet), 2, "Проверка принадлежности точки множеству");
        public static readonly MenuOption Exit = new(nameof(Exit), 3, "Выход");

        private MenuOption(string name, int value, string display) : base(name, value) {
            Display = display;
        }

        public string Display { get; }

    }

    private void RunLoop() {
        var isExit = false;
        while (true) {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuOption>() { Converter = value => value.Display }
                    .Title("[bold]Главное меню[/]")
                    .PageSize(10)
                    .AddChoices(SmartEnumHelper.SortSmartEnumByValue<MenuOption>()));


            choice
             .When(MenuOption.InputFigures).Then(() => {

                 var isOverwriteFigures = false;

                 if (_figures.HasValue) {
                     isOverwriteFigures = AnsiConsole.Prompt(
                         new SelectionPrompt<bool> { Converter = value => value ? "Да" : "Нет" }
                             .Title("Фигуры уже существуют. Продолжить?")
                             .AddChoices(true, false));
                 }

                 if (isOverwriteFigures || _figures.IsEmpty) {
                     _figures = parseFigureMenu.HandleAskFigures();
                 }

             })
             .When(MenuOption.IsPointInSeet).Then(() => {

                 if (_figures.IsEmpty) {
                     AnsiConsole.MarkupLine("Сначала нужно ввести фигуры".FormatException());
                     return;
                 }
                 else {
                     isPointInSetMenu.Handle(_figures.GetValueOrThrow());
                 }

             })
             .When(MenuOption.Exit).Then(() => {
                 isExit = true;
             });

            if (isExit) break;
        }
    }
}
