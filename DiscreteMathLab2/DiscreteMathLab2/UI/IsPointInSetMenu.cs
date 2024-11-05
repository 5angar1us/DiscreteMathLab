using DiscreteMathLab2.Domain;
using Shared.AnsiConsole;
using Spectre.Console;
using static DiscreteMathLab2.Domain.SetOperations;

namespace DiscreteMathLab2.Menus
{
    public class IsPointInSetMenu
    {

        public void Handle(Figures figures)
        {
            var centerX = AnsiConsoleUtils.AskCenterCoordinate("X");
            var centerY = AnsiConsoleUtils.AskCenterCoordinate("Y");

            var isInSet = IsPointInSet(Point.Create(centerX, centerY), figures);

            var setAssignmentMessage = isInSet ? "принадлежит".FormatSuccess() : "не принадлежит".FormatException();

            AnsiConsole.MarkupLine($"Точка {setAssignmentMessage} множеству.");
        }
    }
}
