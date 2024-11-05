﻿using Spectre.Console;
using Shared.AnsiConsole;

namespace DiscreteMathLab2.Menus
{
    internal static class AnsiConsoleUtils
    {
        public static float AskCenterCoordinate(String coordinateName)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<float>($"Введите {coordinateName} координату центра (float): ")
                    .ValidationErrorMessage("Неверный ввод координаты".FormatException()));
        }
    }
}