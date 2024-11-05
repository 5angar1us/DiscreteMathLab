namespace DiscreteMathLab4;

public class MainMenu1
{
    public class Matrix
    {
        private int[,] data;

        public int Length { get => data.Length; }

        public Matrix(int[,] data)
        {
            this.data = data;
        }

        public int this[int row, int column]
        {
            get
            {
                return data[row, column];
            }
        }
    }



    public  void Run()
    {
        // Define three adjacency matrices
        // Graph 1: Eulerian Cycle Exists
        int[,] adjMatrix1 = {
            {0, 1, 1, 0, 0},
            {1, 0, 1, 1, 1},
            {1, 1, 0, 1, 1},
            {0, 1, 1, 0, 1},
            {0, 1, 1, 1, 0}
        };

        // Graph 2: No Eulerian Cycle (has vertices with odd degree)
        int[,] adjMatrix2 = {
            {0, 1, 0, 0},
            {1, 0, 1, 1},
            {0, 1, 0, 0},
            {0, 1, 0, 0}
        };

        // Graph 3: No Eulerian Cycle (disconnected)
        int[,] adjMatrix3 = {
            {0, 1, 0, 0, 0},
            {1, 0, 1, 0, 0},
            {0, 1, 0, 0, 0},
            {0, 0, 0, 0, 1},
            {0, 0, 0, 1, 0}
        };

        Matrix[] graphs = { new Matrix(adjMatrix1), new Matrix(adjMatrix2), new Matrix(adjMatrix3) };

        for (int idx = 0; idx < graphs.Length; idx++)
        {
            Console.WriteLine($"\nGraph {idx + 1}:");
            List<int> cycle = FindEulerCycle(graphs[idx]);
            if (cycle != null)
                PrintEulerCycle(cycle);
            else
                Console.WriteLine("Euler cycle is impossible.");
        }
    }

    public List<int> FindEulerCycle(Matrix adjMatrix)
    {
        int n = (int)Math.Sqrt(adjMatrix.Length);

        // Build the adjacency list
        Dictionary<int, Queue<int>> adj = new Dictionary<int, Queue<int>>();
        for (int i = 0; i < n; i++)
        {
            adj[i] = new Queue<int>();
            for (int j = 0; j < n; j++)
            {
                if (adjMatrix[i, j] == 1)
                    adj[i].Enqueue(j);
            }
        }

        // Check if all vertices have even degree
        for (int i = 0; i < n; i++)
        {
            int degree = Enumerable.Range(0, n).Sum(j => adjMatrix[i, j]);
            if (degree % 2 != 0)
                return null;
        }

        // Check if all non-zero degree vertices are connected
        bool IsConnected()
        {
            HashSet<int> visited = new HashSet<int>();
            // Find a vertex with non-zero degree
            int start = Enumerable.Range(0, n).FirstOrDefault(v => Enumerable.Range(0, n).Sum(j => adjMatrix[v, j]) > 0);
            if (start == 0 && Enumerable.Range(0, n).All(v => Enumerable.Range(0, n).Sum(j => adjMatrix[v, j]) == 0))
                return true;  // No edges in the graph

            Stack<int> stack = new Stack<int>();
            stack.Push(start);
            while (stack.Count > 0)
            {
                int v = stack.Pop();
                if (!visited.Contains(v))
                {
                    visited.Add(v);
                    foreach (int neighbor in adj[v])
                    {
                        if (!visited.Contains(neighbor))
                            stack.Push(neighbor);
                    }
                }
            }

            for (int v = 0; v < n; v++)
            {
                if (Enumerable.Range(0, n).Sum(j => adjMatrix[v, j]) > 0 && !visited.Contains(v))
                    return false;
            }
            return true;
        }

        if (!IsConnected())
            return null;

        // Hierholzer’s Algorithm
        Dictionary<int, Queue<int>> adjCopy = adj.ToDictionary(kvp => kvp.Key, kvp => new Queue<int>(kvp.Value));
        List<int> circuit = new List<int>();
        Stack<int> stack = new Stack<int>();
        stack.Push(0);  // Start from vertex 0
        while (stack.Count > 0)
        {
            int v = stack.Peek();
            if (adjCopy[v].Count > 0)
            {
                int u = adjCopy[v].Dequeue();
                stack.Push(u);
            }
            else
            {
                circuit.Add(stack.Pop());
            }
        }

        circuit.Reverse();

        // Verify if all edges are used
        int totalEdges = Enumerable.Range(0, n).Sum(i => Enumerable.Range(0, n).Sum(j => adjMatrix[i, j]));
        if (circuit.Count - 1 != totalEdges)
            return null;

        return circuit;
    }

    public void PrintEulerCycle(List<int> cycle)
    {
        Console.WriteLine("Euler Cycle: " + string.Join(" ", cycle));
    }
}
