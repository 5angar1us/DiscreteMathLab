using GraphLib.GraphTypes;
using Spectre.Console;

namespace DiscreteMathLab4;

internal class EulerianLoopConstruction
{
    private IAnsiConsole console;

    public EulerianLoopConstruction(IAnsiConsole console)
    {
        this.console = console;
    }

    internal void Construct(Graph graph)
    {
        var eulercycle = EulerCycleFinder.FindEulerCycle(graph);

        if (eulercycle.HasValue)
        {
            console.WriteLine("Эйлеров цикл: " + string.Join(" ", eulercycle.GetValueOrThrow()));
        }
        else
        {
            console.WriteLine("Эйлеров цикл невозможен");
        }
        console.WriteLine(new string('-', 40));
    }
}