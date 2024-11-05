using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.AnsiConsole;

public static class WriteExtention
{
    public static void Write(this IAnsiConsole console, FluentValidation.Results.ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            console.MarkupLine($"{error.ErrorMessage.FormatException()}");
        }
    }
}
