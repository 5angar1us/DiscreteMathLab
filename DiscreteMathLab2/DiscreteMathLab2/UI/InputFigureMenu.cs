using Ardalis.SmartEnum;
using DiscreteMathLab2.Domain;
using DiscreteMathLab2.Menus.Input;
using DiscreteMathLab2.UI.InputFigures.FromFile;
using Shared;
using Spectre.Console;

namespace DiscreteMathLab2.UI;

public class InputFigureMenu {
    private ManualInputFiguresMenu manualInputFigures = new();
    private FromFileInputFiguresMenu fromFileInputFiguresMenu = new();

    private sealed class MenuOption : SmartEnum<MenuOption> {
        public static readonly MenuOption ManualInput = new(nameof(ManualInput), 1, "Ручной");
        public static readonly MenuOption InputFromFile = new(nameof(InputFromFile), 2, "Из файла");
        public static readonly MenuOption Back = new(nameof(Back), 3, "Назад");

        private MenuOption(string name, int value, string display) : base(name, value) {
            Display = display;
        }

        public string Display { get; }

    }

    public Optional<Figures> HandleAskFigures() {
        var choice = AnsiConsole.Prompt(
        new SelectionPrompt<MenuOption>() { Converter = value => value.Display }
            .Title("[bold]Выберите способ ввода фигур[/]")
        .PageSize(10)
            .AddChoices(SmartEnumHelper.SortSmartEnumByValue<MenuOption>()));

        Optional<Figures> figures = Optional<Figures>.Empty();

        choice
         .When(MenuOption.ManualInput).Then(() => {
             figures = manualInputFigures.Handle();
         })
         .When(MenuOption.InputFromFile).Then(() => {
             figures = fromFileInputFiguresMenu.Handle(AnsiConsole.Console);
         })
         .When(MenuOption.Back).Then(() => { });

        return figures;
    }
}
