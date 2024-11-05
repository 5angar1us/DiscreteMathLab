using Shared.AnsiConsole;
using Spectre.Console;
using System.Reflection;

namespace Shared;

public class PathValidator
{
    public ValidationResult Validate(string filePath, string targetExtention)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return ValidationResult.Error("Путь к файлу не указан".FormatException());
        }

        if (IsNot(File.Exists(filePath)))
        {
            return ValidationResult.Error($"Файл по пути '{filePath}' не существует".FormatException());
        }

        string fileExtension = Path.GetExtension(filePath).ToLowerInvariant();
        if (fileExtension != targetExtention)
        {
            return ValidationResult.Error($"Файл по пути '{filePath}' не имеет расширение {targetExtention}. Текущее расширение: {fileExtension}".FormatException());
        }

        return ValidationResult.Success();
    }
}
