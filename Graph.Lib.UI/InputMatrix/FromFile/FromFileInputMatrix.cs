using GraphLib.GraphDomain;
using Shared;
using Shared.AnsiConsole;
using Spectre.Console;

namespace GraphLib.UI.InputMatrix.FromFile
{
    public class FromFileInputMatrix : IPromptProcess<Optional<AdjacencyMatrix>>
    {
        private const string targetExtention = ".txt";
        private readonly string DEFAULT_FIGURE_FILENAME = Path.Combine(Environment.CurrentDirectory, "AdjacencyMatrix" + targetExtention);
        private readonly ParserMatrixFromFile parserMatrixFromFile = new ParserMatrixFromFile();
        private readonly PathValidator pathValidator = new PathValidator();

        public Optional<AdjacencyMatrix> Show(IAnsiConsole console)
        {

            var pathPrompt = new TextPrompt<string>("Введите путь к файлу с матрицей смежности: ")
                .DefaultValue(DEFAULT_FIGURE_FILENAME)
                 .Validate(path => pathValidator.Validate(path, targetExtention));

            var filePath = console.Prompt(pathPrompt);

            try
            {
                var matrix = parserMatrixFromFile.Parse(filePath);

                if (matrix.IsFailure)
                {
                    console.Write(matrix.ValidationResult);
                    goto FailureCase;
                }

                console.WriteLine();
                console.MarkupLine("Матрицы смежности успешно загружена".FormatSuccess());

                return matrix.Value;

            }
            catch (UnauthorizedAccessException uaEx)
            {
                console.MarkupLine($"Нет доступа к файлу: {uaEx.Message}".FormatException());
            }
            catch (IOException ioEx)
            {
                console.MarkupLine($"Ошибка ввода-вывода при работе с файлом: {ioEx.Message}".FormatException());
            }
            catch (Exception ex)
            {
                console.MarkupLine($"Не обработанная ошибка загрузки из файла: {ex.Message}".FormatException());
            }

            FailureCase: return Optional<AdjacencyMatrix>.Empty();
        }

       
    }
}