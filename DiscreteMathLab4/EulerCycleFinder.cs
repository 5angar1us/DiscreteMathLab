
using GraphLib;
using GraphLib.GraphDomain.GraphTypes;
using Shared;
using Spectre.Console;


namespace DiscreteMathLab4;

public class EulerCycleFinder {
    public static Optional<List<string>> FindEulerCycle(Graph graph) {
        // check For Euler Path
        if (IsNot(IsAllNonZeroConnected(graph)))
            return Optional<List<string>>.Empty();

        if (IsNot(IsAllDegreeEven(graph)))
            return Optional<List<string>>.Empty();


        var tempGraph = new Graph(graph.GetEdges());

        var path = GetPath(tempGraph);

        if (IsNot(AllEdgesUsed(tempGraph))) {
            AnsiConsole.WriteLine("Не все рёбра используются");
            return Optional<List<string>>.Empty();
        }

        path.Reverse(); // The path is constructed in reverse
        return path.Select(node => $"{GraphConsts.NODE_PREFIX}_{node.Number}").ToList();
    }

    private static bool IsAllNonZeroConnected(Graph graph) {
        var nodes = graph.GetNodes();
        var startNode = nodes.FirstOrDefault(n => n.HasOneConnection(graph));

        if (startNode == null)
            return true;

        var visited = new HashSet<Node>();

        DFS(graph, startNode, visited);

        // Check if all vertices with non-zero degree are visited
        foreach (var node in nodes) {
            if (node.HasOneConnection(graph) && !visited.Contains(node))
                return false;
        }

        return true;
    }
    private static void DFS(Graph graph, Node startNode, HashSet<Node> visited) {
        visited.Add(startNode);

        var neighbors = getNeigbors(graph, startNode);

        foreach (var neighbor in neighbors) {
            if (!visited.Contains(neighbor)) {
                DFS(graph, neighbor, visited);
            }
        }
    }

    private static IEnumerable<Node> getNeigbors(Graph graph, Node startNode) {
        var eadges = graph.GetArces();
        var toEages = eadges.Where(edge => edge.From == startNode).Select(edge => edge.To);
        var fromNodes = eadges.Where(edge => edge.To == startNode).Select(edge => edge.From);
        return toEages.Concat(fromNodes);
    }

    private static bool IsAllDegreeEven(Graph graph) {
        foreach (var node in graph.GetNodes()) {

            var isSuccess = graph.GetDegree(node) % 2 == 0;

            if (isSuccess == false) {
                AnsiConsole.WriteLine("Нет цикла Эйлера (вершины с нечетной степенью)");
                return false;
            }
        }
        return true;
    }

    //построения эйлерова пути
    private static List<Node> GetPath(Graph graph) {
        var path = new List<Node>();
        var currentNode = graph.GetNodes().FirstOrDefault(node => node.HasOneConnection(graph));

        if (currentNode == null)
            return path;

        var stack = new Stack<Node>();
        stack.Push(currentNode);

        while (stack.Any()) {
            currentNode = stack.Peek();

            var neighbor = getNeigbors(graph, currentNode).FirstOrDefault();

            if (neighbor != null) {
                var edge = graph.GetEdge(new HashSet<Node> { currentNode, neighbor });
                graph.RemoveEdge(edge.GetValueOrThrow());
                stack.Push(neighbor);
            }
            else {
                path.Add(stack.Pop());
            }
        }

        return path;
    }

    private static bool AllEdgesUsed(Graph graph) {
        return graph.GetEdges().Count == 0;
    }
}