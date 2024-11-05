using DiscreteMathLab3.GraphDomain;
using DiscreteMathLab3.GraphTypes;
using DiscreteMathLab4.v3;
using Spectre.Console;
using static DiscreteMathLab4.UI.MainMenu;

namespace DiscreteMathLab4.v4
{
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
}