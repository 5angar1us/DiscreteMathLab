using DiscreteMathLab3.GraphDomain.GraphTypes;
using DiscreteMathLab3.GraphDomain;
using Spectre.Console;
using System.Data;

namespace DiscreteMathLab3.UI.Utils
{
    internal static class GrapTypesTableConverters
    {
        public static Table ToTable(this AdjacencyMatrix matrix)
        {
            var table = new Table();

            table.AddColumn(new TableColumn($"Вершина \\ Вершина"));

            for (int columnIndex = 0; columnIndex < matrix.NodeCount; columnIndex++)
            {
                table.AddColumn($"{matrix.NameTemplate.Column}{columnIndex + 1}");
            }

            for (int rowIndex = 0; rowIndex < matrix.NodeCount; rowIndex++)
            {
                var cells = new List<Text>() {
                    new($"{matrix.NameTemplate.Row}{rowIndex+1}")
                };

                for (int cellIndex = 0; cellIndex < matrix.NodeCount; cellIndex++)
                {
                    cells.Add(new(matrix[rowIndex, cellIndex].ToString()));
                }
                table.AddRow(cells.ToArray());
            }

            return table;
        }

        public static Table ToTable(this IncidentMatrix matrix)
        {

            var table = new Table();
            table.AddColumn(new TableColumn($" Вершина \\ Ребро"));

            for (int columnIndex = 0; columnIndex < matrix.EdgeCount; columnIndex++)
            {
                table.AddColumn($"{matrix.NameTemplate.Column}{columnIndex + 1}");
            }

            for (int rowIndex = 0; rowIndex < matrix.NodeCount; rowIndex++)
            {
                var cells = new List<Text>() {
                new($"{matrix.NameTemplate.Row}{rowIndex + 1}")
            };

                for (int cellIndex = 0; cellIndex < matrix.EdgeCount; cellIndex++)
                {
                    cells.Add(new(matrix[rowIndex, cellIndex].ToString()));
                }
                table.AddRow(cells.ToArray());
            }
            return table;
        }

        public static Table ToTable(this IncidentsLists incidentsLists)
        {
            var sortedItems = incidentsLists.OrderBy(x => x.Edge.From).ToList();

            var table = new Table();
            table.AddColumn("Рёбро");
            table.AddColumn("Вершины");

            for (int index = 0; index < incidentsLists.Count; index++)
            {
                var item = sortedItems[index];
                var edgeLabel = $"{GraphConsts.EDGE_PREFIX}_{index + 1} ( {CreateNodeLable(item.Edge.From)}, {CreateNodeLable(item.Edge.To)} )";
                var neighbors = string.Join<Node>(" ", item.Neighbors);

                table.AddRow($"{edgeLabel}", neighbors);
            }

            return table;

        }

        public static Table ToTable(this RelationshipLists relationshipLists)
        {
            var sortedItems = relationshipLists.OrderBy(x => x.Node.Number).ToList();

            var table = new Table();
            table.AddColumn("Вершина");
            table.AddColumn("Веришны - Соседи");

            foreach (var item in sortedItems)
            {
                var neighbors = string.Join<Node>(" ", item.Neighbors);
                table.AddRow($"{CreateNodeLable(item.Node)}", neighbors);
            }

            return table;
        }

        private static string CreateNodeLable(Node node)
        {
            return $"{GraphConsts.NODE_PREFIX}_{node.Number}";
        }

        private class EageCompare : IComparer<Edge>
        {
            public int Compare(Edge? x, Edge? y)
            {
                if (x == null && y == null)
                    return 0;
                if (x == null)
                    return -1;
                if (y == null)
                    return 1;

                var xName = x.ToString();
                var yName = y.ToString();

                return xName.CompareTo(yName);
            }
        }
    }
}
