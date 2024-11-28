using GraphLib.GraphDomain;
using GraphLib.GraphTypes;
using Spectre.Console;

namespace DiscreteMathLab4.UI;

public record Debug(bool CorrectDegre, bool Connected);

public static class DebugConst {
    public static Debug DebugNeed = new Debug(false, false);
}

class Test {

    public void Run() {

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
        for (int i = start; i < cyclestop; i++) {
            Console.WriteLine($"graph {i + 1}:");
            int[,] adjacencymatrix = adjacencyMatrices[i];

            var adjMatrixResult = AdjacencyMatrix.Create(ArrayConverter.ConvertToJaggedArray(adjacencymatrix), AdjacencyMatrixNamePrefixes.Create());
            if (adjMatrixResult.IsFailure) throw new ArgumentException();

            AdjacencyMatrix adjMatrix = adjMatrixResult.Value;

            var graph = adjMatrix.ToGraph();
            EulerianLoopConstruction eulerianLoopConstruction = new(AnsiConsole.Console);
            eulerianLoopConstruction.Construct(graph);
        }


        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }


    private static void WriteDebugIfNeeded(bool isNeeded, string message, bool isSpaceAfter) {
        if (isNeeded) {
            Console.WriteLine(message);

            if (isSpaceAfter) {
                Console.WriteLine();
            }
        }
    }
}