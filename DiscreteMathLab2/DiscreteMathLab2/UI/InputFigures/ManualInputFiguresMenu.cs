using Ardalis.SmartEnum;
using DiscreteMathLab2.Domain;
using DiscreteMathLab2.UI.InputFigures.FromFile.Parser.Exceptions;
using Shared;
using Shared.AnsiConsole;
using Spectre.Console;


namespace DiscreteMathLab2.Menus.Input;

public class ManualInputFiguresMenu {
    public Optional<Figures> Handle() {

        var allFiguresName = string.Join(", ", Enum.GetNames(typeof(EFigures)));

        AnsiConsole.MarkupLine($"[bold]Ввод фигур: {allFiguresName}[/]");

        var figuresRaw = Enum.GetNames(typeof(EFigures))
            .ToDictionary(figureName => figureName, figureName => HandleFigureInput(figureName));

        var figures = Figures.Create(figuresRaw);

        if (figures.IsSuccess) {
            return figures.Value;
        }
        else {
            foreach (var error in figures.ValidationResult.Errors) {
                AnsiConsole.WriteLine(error.ErrorMessage.FormatException());
            }
            return Optional<Figures>.Empty();
        }
    }

    private Figure HandleFigureInput(string figureName) {
        while (true) {
            if (TryManualInputFigure(figureName, out Figure figure)) {
                AnsiConsole.MarkupLine("Фигура успешно добавлена".FormatSuccess());
                return figure;
            }
            else {
                AnsiConsole.MarkupLine("Фигура не создана. Попробуйте ещё раз".FormatException());
            }
        }
    }

    private sealed class FigureType : SmartEnum<FigureType> {
        public static readonly FigureType Circular = new(nameof(Circular), 1, "Круг");
        public static readonly FigureType Rectagle = new(nameof(Rectagle), 2, "Прямоугольник");

        private FigureType(string name, int value, string display) : base(name, value) {
            Display = display;
        }

        public string Display { get; }

    }

    enum EFigureType {
        circular,
        rectagle
    }
    private bool TryManualInputFigure(string name, out Figure? returnFigure) {
        AnsiConsole.MarkupLine($"[bold]Ввод фигуры {name}[/]");
        var figureType = AnsiConsole.Prompt(
            new SelectionPrompt<FigureType>() { Converter = value => value.Display }
                .Title("Выберите тип фигуры:")
                .AddChoices(SmartEnumHelper.SortSmartEnumByValue<FigureType>()));



        var centerX = AskCenterCoordinate("X");
        var centerY = AskCenterCoordinate("Y");


        Figure figure = AskSpecificPropertiesByTypeAndCreateFigure(figureType, Point.Create(centerX, centerY));

        ShowFigureProperties(figureType, figure);

        var isProcessFigure = AnsiConsole.Prompt(
            new SelectionPrompt<bool> { Converter = value => value ? "Да" : "Нет" }
                .Title("Добавить эту фигуру?")
                .AddChoices(true, false)
        );

        if (IsNot(isProcessFigure)) {
            returnFigure = default;
            return false;
        }

        returnFigure = figure;
        return true;
    }



    private Figure AskSpecificPropertiesByTypeAndCreateFigure(FigureType figureType, Point center) {
        float askParameter(string parameterName) {
            return AnsiConsole.Prompt(
                new TextPrompt<float>($"Введите {parameterName} (float > 0): ")
                  .Validate(input => input > 0)
                  .ValidationErrorMessage($"Неверный ввод параметра \"{parameterName}\"".FormatException()));
        }

        if (FigureType.Circular == figureType) {
            var radius = askParameter("радиус");

            return Figure.CreateCircle(center, radius).Value;
        }
        if (FigureType.Rectagle == figureType) {
            var width = askParameter("ширину");
            var height = askParameter("высоту");

            return Figure.CreateRectangle(center, width, height).Value;
        }


        throw new IncorrectFigureTypeException(figureType.Display);
    }

    private void ShowFigureProperties(FigureType figureType, Figure figure) {
        var table = new Table()
             .AddColumn(new TableColumn(header: "Поле").Centered())
             .AddColumn(new TableColumn(header: "Значение").Centered());

        table.AddRow("Центр", $" Х:{figure.X0} Y:{figure.Y0}");
        table.AddRow("Тип", figureType.ToString());

        if (figure.IsCircle) {
            table = table.AddRow("Радиус", figure.Radius.ToString());
        }
        else {
            table = table
                .AddRow("Ширина", figure.Width.ToString())
                .AddRow("Высота", figure.Height.ToString());
        }

        AnsiConsole.Write(table);
    }
}