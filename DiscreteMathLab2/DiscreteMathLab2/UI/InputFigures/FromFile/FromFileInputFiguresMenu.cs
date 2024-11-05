using DiscreteMathLab2.Menus;
using DiscreteMathLab2.Domain;
using DiscreteMathLab2.UI.InputFigures.FromFile.Parser;
using DiscreteMathLab2.UI.InputFigures.FromFile.Parser.Exceptions;
using Shared;
using Newtonsoft.Json;
using Spectre.Console;
using System;
using Shared.AnsiConsole;

namespace DiscreteMathLab2.UI.InputFigures.FromFile
{
    public class FromFileInputFiguresMenu
    {
        private const string targetExtention = ".json";
        private readonly string DEFAULT_FIGURE_FILENAME = Path.Combine(Environment.CurrentDirectory, "figures" + targetExtention);
        private readonly ParserFiguresFromFile parserFiguresFromFile = new();
        private readonly PathValidator pathValidator = new PathValidator();


        public Optional<Figures> Handle(IAnsiConsole console)
        {
            var pathPrompt = new TextPrompt<string>("Введите путь к файлу с фигурами: ")
                .DefaultValue(DEFAULT_FIGURE_FILENAME)
                 .Validate(path => pathValidator.Validate(path, targetExtention));

            var filePath = console.Prompt(pathPrompt);

            try
            {
                var parsedFigures = parserFiguresFromFile.Parse(filePath);

                if (parsedFigures.Any(x => x.Value.IsFailure))
                {
                    parsedFigures.Values.Where(x => x.IsFailure)
                        .Select(t => t.ValidationResult)
                        .ToList()
                        .ForEach(WriteValidationErros);


                    goto FailureCase;
                }

                Dictionary<string, Figure> rawFigures = parsedFigures
                    .Where(kv => kv.Value.IsSuccess)
                    .ToDictionary(
                        kv => kv.Key,
                        kv => kv.Value.Value
                    );

                var figures = Figures.Create(rawFigures);

                return GetValueOrValidationErrors(figures, "Фигуры успешно загружены из файла");
            }
            catch (JsonException ex)
            {
                AnsiConsole.MarkupLine($"Ошибка в формате JSON файла {ex.Message}".FormatException());
            }
            catch (ParseException ex)
            {
                AnsiConsole.MarkupLine(ex.Message.FormatException());
            }
            catch (UnauthorizedAccessException uaEx)
            {
                AnsiConsole.MarkupLine($"Нет доступа к файлу: {uaEx.Message}".FormatException());
            }
            catch (IOException ioEx)
            {
                AnsiConsole.MarkupLine($"Ошибка ввода-вывода при работе с файлом: {ioEx.Message}".FormatException());
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"Не обработанная ошибка загрузки из файла: {ex.Message}".FormatException());
            }
        FailureCase: return Optional<Figures>.Empty();
        }
        private static Optional<Figures> GetValueOrValidationErrors(ResultFluent<Figures> figures, string successMessage)
        {
            if (figures.IsSuccess)
            {
                if (IsNot(string.IsNullOrEmpty(successMessage)))
                {

                    AnsiConsole.MarkupLine(successMessage.FormatSuccess());
                }

                return figures.Value;
            }
            else
            {
                WriteValidationErros(figures.ValidationResult);
                return Optional<Figures>.Empty();
            }
        }

        private static void WriteValidationErros(FluentValidation.Results.ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                AnsiConsole.MarkupLine($"{error.ErrorMessage.FormatException()}");
            }
        }
    }

}
