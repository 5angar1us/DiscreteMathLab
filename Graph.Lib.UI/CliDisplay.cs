using GraphLib.GraphDomain;
using Spectre.Console;

namespace GraphLib.UI
{
    public class CliDisplay
    {
        public CliDisplay(IAnsiConsole console)
        {
            Console = console;
        }

        private IAnsiConsole Console { get; init; }

        public void Show(AdjacencyMatrix matrix)
        {
            var rule = new Rule("[bold green]Матрица смежности[/]");
            Console.Write(rule);
            Console.Write(Align.Center(matrix.ToTable()));
        }

        public void Show(IncidentMatrix matrix)
        {
            var rule = new Rule("[bold green]Матрица инциденций[/]");
            Console.Write(rule);
            Console.Write(Align.Center(matrix.ToTable()));
        }

        public void Show(RelationshipLists relationshipLists)
        {
            var rule = new Rule("[bold green]Список смежности[/]");
            Console.Write(rule);
            Console.Write(Align.Center(relationshipLists.ToTable()));
        }

        public void Show(IncidentsLists incidentsLists)
        {
            var rule = new Rule("[bold green]Список инциденций[/]");
            Console.Write(rule);
            Console.Write(Align.Center(incidentsLists.ToTable()));
        }
    }

   
}
