namespace DiscreteMathLab4.v2;

record Debug(bool CorrectDegre, bool Connected);


class MainMenu2
{
    private Debug _debugNeeds;

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
                
                // Graph 3: Euler cycle exists
                new int[,]
                {
                    {0, 1, 1, 2},
                    {1, 0, 1, 0},
                    {1, 1, 0, 0},
                    {2, 0, 0, 0}
                },
                
                // Graph 4: Euler cycle exists
                new int[,]
                {
                    /*
                      
                            1  2  3  4  5  6  7  8
                    */
                    /* 1 */{0, 1, 1, 0, 0, 0, 0, 0},
                    /* 2 */{1, 0, 0, 1, 0, 0, 1, 1},
                    /* 3 */{1, 0, 0, 1, 1, 1, 0, 0},
                    /* 4 */{0, 1, 1, 0, 1, 0, 1, 0},
                    /* 5 */{0, 0, 1, 1, 0, 1, 1, 0},
                    /* 6 */{0, 0, 1, 0, 1, 0, 0, 2},
                    /* 7 */{0, 1, 0, 1, 1, 0, 0, 1},
                    /* 8 */{0, 1, 0, 0, 0, 2, 1, 0},
                }
            };

        _debugNeeds = new Debug(Connected: false, CorrectDegre: false);

        var start = 0;
        var cycleStop = adjacencyMatrices.Count;
        for (int i = start; i < cycleStop; i++)
        {
            Console.WriteLine($"Graph {i + 1}:");
            int[,] adjacencyMatrix = adjacencyMatrices[i];
            var eulerCycle = FindEulerCycle(adjacencyMatrix);

            if (eulerCycle != null)
            {
                Console.WriteLine("Euler Cycle: " + string.Join(" ", eulerCycle));
            }
            else
            {
                Console.WriteLine("Euler cycle is impossible.");
            }
            Console.WriteLine(new string('-', 40));
        }



        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    /// <summary>
    /// Finds an Euler cycle in the given graph if it exists.
    /// </summary>
    /// <param name="adjMatrix">Adjacency matrix representing the graph.</param>
    /// <returns>List of vertices representing the Euler cycle or null if impossible.</returns>
    List<int> FindEulerCycle(int[,] adjMatrix)
    {
        int nodeCount = adjMatrix.GetLength(0);

        // Check if all vertices with non-zero degree are connected
        if (!IsConnected(adjMatrix, nodeCount))
            return null;

        // Check if all vertices have even degree
        if (isCorrectDegre(adjMatrix, nodeCount) == false)
            return null;

        // Create a copy of the adjacency matrix to modify
        int[,] tempAdj = (int[,])adjMatrix.Clone();



        // Start from the first vertex with at least one edge
        List<int> path = GetPath(nodeCount, ref tempAdj);

        if (AllEdgesUsed(tempAdj) == false)
        {
            Console.WriteLine("Not all edges are used");
        }

        path.Reverse(); // The path is constructed in reverse
        return path;
    }

    private List<int> GetPath(int nodeCount, ref int[,] tempAdj)
    {
        List<int> path = new List<int>();
        Stack<int> stack = new Stack<int>();

        int nodeIndex = 0;
        for (; nodeIndex < nodeCount; nodeIndex++)
        {
            bool hasEdge = false;
            for (int otherNodeIndex = 0; otherNodeIndex < nodeCount; otherNodeIndex++)
            {

                var value = tempAdj[nodeIndex, otherNodeIndex];
                if (value > 0)
                {
                    hasEdge = true;
                    break;
                }
            }
            if (hasEdge)
                break;
        }

        stack.Push(nodeIndex);

        while (stack.Count > 0)
        {
            nodeIndex = stack.Peek();
            bool found = false;
            for (int otherNodeIndex = 0; otherNodeIndex < nodeCount; otherNodeIndex++)
            {

                var value = tempAdj[nodeIndex, otherNodeIndex];
                if (value > 0)
                {
                    // Remove edge from the graph
                    tempAdj[nodeIndex, otherNodeIndex]--;
                    tempAdj[otherNodeIndex, nodeIndex]--;

                    stack.Push(otherNodeIndex);
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                var nodeNumber = stack.Pop() + 1;
                path.Add(nodeNumber);
            }
        }

        return path;
    }

    private bool AllEdgesUsed(int[,] tempAdj)
    {
        // Check if all edges are used
        foreach (int val in tempAdj)
        {
            if (val != 0)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if the graph is connected using DFS.
    /// </summary>
    /// <param name="adjMatrix">Adjacency matrix representing the graph.</param>
    /// <param name="V">Number of vertices.</param>
    /// <returns>True if connected, otherwise false.</returns>
    bool IsConnected(int[,] adjMatrix, int V)
    {
        
        int start = -1;

        // Find a vertex with non-zero degree
        for (int i = 0; i < V; i++)
        {
            for (int j = 0; j < V; j++)
            {
                if (adjMatrix[i, j] > 0)
                {
                    start = i;
                    break;
                }
            }
            if (start != -1)
                break;
        }

        // If there are no edges, it's considered connected
        if (start == -1)
            return true;


        bool[] visited = new bool[V];
        DFS(adjMatrix, start, visited, V);

        // Check if all vertices with non-zero degree are visited
        for (int i = 0; i < V; i++)
        {
            for (int j = 0; j < V; j++)
            {
                if (adjMatrix[i, j] > 0 && !visited[i])
                    return false;
            }
        }

        return true;
    }

    void DFS(int[,] adjMatrix, int v, bool[] visited, int V)
    {
        visited[v] = true;
        for (int u = 0; u < V; u++)
        {
            if (adjMatrix[v, u] > 0 && !visited[u])
            {
                DFS(adjMatrix, u, visited, V);
            }
        }
    }

    private bool isCorrectDegre(int[,] adjMatrix, int nodeCount)
    {
        var _CorrectDegreDebugNeeds = _debugNeeds.CorrectDegre;

        for (int row = 0; row < nodeCount; row++)
        {
            int degree = 0;
            for (int column = 0; column < nodeCount; column++)
            {
                var value = adjMatrix[row, column];
                WriteDebugIfNeeded(_CorrectDegreDebugNeeds, $"[ {row}, {column} ]: {value}", isSpaceAfter: false);
                degree += value;
            }


            var checkDegree = degree % 2 != 0;
            WriteDebugIfNeeded(_CorrectDegreDebugNeeds, $"degree = {degree} -> (degree % 2 !=0 ) : {checkDegree}", isSpaceAfter: true);

            if (checkDegree)
            {
                Console.WriteLine("No Euler cycle (vertices with odd degree)");
                return false;
            }

        }
        return true;
    }



    private void WriteDebugIfNeeded(bool isNeeded, string message, bool isSpaceAfter)
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
}

