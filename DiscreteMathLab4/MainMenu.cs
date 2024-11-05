using DiscreteMathLab3.GraphDomain;
using DiscreteMathLab3.GraphDomain.GraphTypes;
using DiscreteMathLab3.GraphTypes;
using System.Xml.Linq;

namespace DiscreteMathLab4.v3;

public record Debug(bool CorrectDegre, bool Connected);

public static class DebugConst
{
    public static Debug DebugNeed = new Debug(false, false);
}

class MainMenu
{

    public void Run()
    {
        // Define three adjacency matrices (graphs)
        List<int[,]> adjacencyMatrices = new List<int[,]>
            {
                // Graph 1: Euler cycle exists
                new int[,]
                {
                    {0, 1, 1, 0, 0},
                    {1, 0, 0, 1, 0},
                    {1, 0, 0, 0, 1},
                    {0, 1, 0, 0, 1},
                    {0, 0, 1, 1, 0}
                },
                
                // Graph 2: No Euler cycle (vertices with odd degree)
                new int[,]
                {
                    {0, 1, 0, 0},
                    {1, 0, 1, 1},
                    {0, 1, 0, 0},
                    {0, 1, 0, 0}
                },
                
                // Graph 3: No Euler cycle (vertices with odd degree)
                new int[,]
                {
                    {0, 1, 1, 1},
                    {1, 0, 1, 0},
                    {1, 1, 0, 0},
                    {1, 0, 0, 0}
                },
                
                // Graph 4: No Euler cycle (vertices with odd degree)
                new int[,]
                {
                    /*
                      
                            1  2  3  4    5  6  7  8
                    */
                    /* 1 */{0, 1, 1, 0,   0, 0, 0, 0},
                    /* 2 */{1, 0, 0, 1,   0, 0, 1, 1},
                    /* 3 */{1, 0, 0, 1,   1, 1, 0, 0},
                    /* 4 */{0, 1, 1, 0,   1, 0, 1, 0},


                    /* 5 */{0, 0, 1, 1,   0, 1, 1, 0},
                    /* 6 */{0, 0, 1, 0,   1, 0, 0, 1},
                    /* 7 */{0, 1, 0, 1,   1, 0, 0, 1},
                    /* 8 */{0, 1, 0, 0,   0, 1, 1, 0},
                },

                // Graph 5: 
                new int[,]
                {
                    /*
                      
                            1  2  3  4    5  6  7  8
                    */
                    /* 1 */{0, 1, 1, 0,   0, 0, 0, 0},
                    /* 2 */{1, 0, 0, 1,   0, 0, 1, 1},
                    /* 3 */{1, 0, 0, 1,   1, 1, 0, 0},
                    /* 4 */{0, 1, 1, 0,   0, 0, 0, 0},


                    /* 5 */{0, 0, 1, 0,   0, 0, 1, 0},
                    /* 6 */{0, 0, 1, 0,   0, 0, 0, 1},
                    /* 7 */{0, 1, 0, 0,   1, 0, 0, 0},
                    /* 8 */{0, 1, 0, 0,   0, 1, 0, 0},
                }
            };

        DebugConst.DebugNeed = new Debug(Connected: false, CorrectDegre: true);

        var start = 0;
        var cyclestop = adjacencyMatrices.Count;
        for (int i = start; i < cyclestop; i++)
        {
            Console.WriteLine($"graph {i + 1}:");
            int[,] adjacencymatrix = adjacencyMatrices[i];

            var eulercycle = EulerCycleFinder.FindEulerCycle(adjacencymatrix);

            if (eulercycle != null)
            {
                Console.WriteLine("euler cycle: " + string.Join(" ", eulercycle));
            }
            else
            {
                Console.WriteLine("euler cycle is impossible.");
            }
            Console.WriteLine(new string('-', 40));
        }

        //int[,] adjacencyMatrix = adjacencyMatrices[1];
        //var eulerCycle = EulerCycleFinder.FindEulerCycle(adjacencyMatrix);

        //if (eulerCycle != null)
        //{
        //    Console.WriteLine("Euler Cycle: " + string.Join(" ", eulerCycle));
        //}
        //else
        //{
        //    Console.WriteLine("Euler cycle is impossible.");
        //}
        //Console.WriteLine(new string('-', 40));


        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }


    private static void WriteDebugIfNeeded(bool isNeeded, string message, bool isSpaceAfter)
    {
        if (isNeeded)
        {
            Console.WriteLine(message);

            if (isSpaceAfter)
            {
                Console.WriteLine();
            }
        }
    }


    public class EulerCycleFinder
    {

        static int[][] ConvertToJaggedArray(int[,] adjMatrix)
        {
            int rows = adjMatrix.GetLength(0);
            int cols = adjMatrix.GetLength(1);

            int[][] matrix = new int[rows][];

            for (int i = 0; i < rows; i++)
            {
                matrix[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                {
                    matrix[i][j] = adjMatrix[i, j];
                }
            }

            return matrix;
        }

        public static List<int> FindEulerCycle(int[,] adjMatrixArray)
        {

            var adjMatrixResult = AdjacencyMatrix.Create(ConvertToJaggedArray(adjMatrixArray), AdjacencyMatrixNamePrefixes.Create());
            if (adjMatrixResult.IsFailure) throw new ArgumentException(); //TODO

            AdjacencyMatrix adjMatrix = adjMatrixResult.Value;

            var graph = adjMatrix.ToGraph();
            var nodeCount = adjMatrix.NodeCount;

            // Check if all vertices with non-zero degree are connected
            // TODO я вроде такое уже писал
            if (!IsConnected(graph))
                return null;

            // Check if all vertices have even degree
            if (!IsCorrectDegree(graph))
                return null;

            // Create a copy of the graph to modify
            var tempGraph = new Graph(graph.GetEdges(), graph.GraphType);

            // Start from the first vertex with at least one edge
            var path = GetPath(tempGraph);

            if (!AllEdgesUsed(tempGraph))
            {
                Console.WriteLine("Not all edges are used");
            }

            path.Reverse(); // The path is constructed in reverse
            return path.Select(node => node.Number).ToList();
        }

        private static bool IsConnected(Graph graph)
        {
            var nodes = graph.GetNodes();
            var startNode = nodes.FirstOrDefault(n => n.HasOneConnection(graph));

            if (startNode == null)
                return true;

            var visited = new HashSet<Node>();

            //TODO DFS может их вынести куда-то
            DFS(graph, startNode, visited);

            // Check if all vertices with non-zero degree are visited
            foreach (var node in nodes)
            {
                if (node.HasOneConnection(graph) && !visited.Contains(node))
                    return false;
            }

            return true;
        }

        private static void DFS(Graph graph, Node startNode, HashSet<Node> visited)
        {
            visited.Add(startNode);

            var neighbors = getNeigbors(graph, startNode);

            foreach (var neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    DFS(graph, neighbor, visited);
                }
            }
        }

        private static IEnumerable<Node> getNeigbors(Graph graph, Node startNode)
        {
            var eadges = graph.GetEdges();
            var toEages = eadges.Where(edge => edge.From == startNode).Select(edge => edge.To);
            var fromNodes = eadges.Where(edge => edge.To == startNode).Select(edge => edge.From);
            return toEages.Concat(fromNodes).ToList();
        }

        private static bool IsCorrectDegree(Graph graph)
        {
            foreach (var node in graph.GetNodes())
            {
                WriteDebugIfNeeded(DebugConst.DebugNeed.CorrectDegre, $"[{node.ToString()}]: Neigbors: {getNeigbors(graph, node)} Degree: {graph.GetDegree(node)} ", isSpaceAfter: false);

                var degree = graph.GetDegree(node);

                var isSuccess = false;

                graph.GetDegree(node).Match(
                    Unoriginalized =>
                    {
                        isSuccess = Unoriginalized.Degree % 2 == 0;
                    },
                    Directional =>
                    {
                        //TODO Правильно????
                        isSuccess = Directional.InDegree + Directional.OutDegree % 2 == 0;
                    },
                    Mixed =>
                    {
                        // TODO Правильно ????
                        isSuccess = Mixed.Degree % 2 != 0 || Mixed.InDegree + Mixed.OutDegree % 2 == 0;
                    }
                    );

                if (isSuccess == false)
                {
                    Console.WriteLine("No Euler cycle (vertices with odd degree)");
                    return false;
                }
            }
            return true;
        }

        //TODO на каком алгоритме он основан?????
        private static List<Node> GetPath(Graph graph)
        {
            var path = new List<Node>();
            var currentNode = graph.GetNodes().FirstOrDefault(node => node.HasOneConnection(graph));

            if (currentNode == null)
                return path;

            var stack = new Stack<Node>();
            stack.Push(currentNode);

            while (stack.Any())
            {
                currentNode = stack.Peek();

                var neighbor = getNeigbors(graph, currentNode).FirstOrDefault();

                // ????????
                if (neighbor != null)
                {
                    var edge = graph.GetEdge(new HashSet<Node> { currentNode, neighbor });
                    graph.RemoveEdge(edge);
                    stack.Push(neighbor);
                }
                else
                {
                    path.Add(stack.Pop());
                }
            }

            return path;
        }

        private static bool AllEdgesUsed(Graph graph)
        {
            return graph.GetEdges().Count == 0;
        }
    }
}