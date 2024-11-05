using DiscreteMathLab3.GraphDomain;
using DiscreteMathLab3.UI.InputMatrix.Manual.NAdjacencyMatrixPrompt;
using DiscreteMathLab3.UI.Utils;
using Shared;
using Shared.AnsiConsole;
using Spectre.Console;

namespace DiscreteMathLab3.UI.InputMatrix.Manual;


public class ManualInputMatrix : IPromptProcess<Optional<AdjacencyMatrix>>
{
    public Optional<AdjacencyMatrix> Show(IAnsiConsole console)
    {
        var size = console.Prompt(
                new TextPrompt<int>("Введите размер матрицы (больше 1):")
                .Validate(parsedSize =>
                {
                    if (parsedSize >= 2 && parsedSize <= 10)
                    {
                        return ValidationResult.Success();
                    }
                    else
                    {
                        return ValidationResult.Error("Неверный размер. Размер должен быть не менее 2.".FormatException());
                    }
                })
            );

        var matrixRaw = new InputAdjacencyMatrixTypes(size);

        var matrix = new AdjacencyMatrixPromptProcess(matrixRaw).Show(console);

        new CliDisplay(console).Show(matrix);

        var isMatrixCorrect = console.Prompt(
                new SelectionPrompt<bool>
                {
                    Converter = value => value ? "Да" : "Нет"
                }
                .Title("Вас устраивает этот граф в виде матрицы смежности?")
                .AddChoices(true, false)
            );

        console.Clear();

        return isMatrixCorrect ? matrix : Optional<AdjacencyMatrix>.Empty();
    }

}
