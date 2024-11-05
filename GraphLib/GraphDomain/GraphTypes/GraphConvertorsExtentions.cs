using GraphLib.GraphDomain;
using GraphLib.GraphDomain.GraphTypes;

namespace GraphLib.GraphTypes;

public static class GraphConvertorsExtentions
{
    public static Graph ToGraph(this AdjacencyMatrix matrix)
    {
        var nodeCount = matrix.NodeCount;
        var nodes = NodeGenerator.GenerateNodes(nodeCount).OrderBy(node => node.Number).ToList();

        var edges = new List<Edge>();
        for (var firstNodeIndex = 0; firstNodeIndex < nodeCount; firstNodeIndex++)
        {
            for (var secondNodeIndex = 0; secondNodeIndex < nodeCount; secondNodeIndex++)
            {
                if (firstNodeIndex == secondNodeIndex)
                {
                    continue;
                }

                if (matrix[firstNodeIndex, secondNodeIndex] > 0)
                {
                    edges.Add(new Edge(nodes[firstNodeIndex], nodes[secondNodeIndex]));
                }
            }
        }

        return new Graph(edges, nodes.ToHashSet());
    }

    public static AdjacencyMatrix ToInputAdjacencyMatrix(this Graph graph)
    {
        var edges = graph.GetEdges();

        int vertexCount = graph.GetNodes().Count;
        int[][] adjacencyMatrix = new int[vertexCount][];
        for (int i = 0; i < vertexCount; i++)
        {
            adjacencyMatrix[i] = new int[vertexCount];
        }

        var nodeAndIndexPairs = GetNodeIndexPairs(graph);

        edges.ForEach(edge =>
        {
            int vertex1Index = nodeAndIndexPairs[edge.From];
            int vertex2Index = nodeAndIndexPairs[edge.To];

            adjacencyMatrix[vertex1Index][vertex2Index] = 1;
            adjacencyMatrix[vertex2Index][vertex1Index] = 1;
        });

        return AdjacencyMatrix.Create(adjacencyMatrix, AdjacencyMatrixNamePrefixes.Create()).Value;
    }

    public static IncidentMatrix ToIncidentMatrix(this Graph graph)
    {
        var edges = graph.GetEdges();

        int vertexCount = graph.GetNodes().Count;
        int edgeCount = edges.Count;

        int[][] incidentMatrix = new int[vertexCount][];
        for (int i = 0; i < vertexCount; i++)
        {
            incidentMatrix[i] = new int[edgeCount];
        }

        var adjacencyMatrix = graph.ToInputAdjacencyMatrix();
        var nodeAndIndexPairs = GetNodeIndexPairs(graph);

        for (int edgeIndex = 0; edgeIndex < edgeCount; edgeIndex++)
        {
            var edge = edges[edgeIndex];
            int vertex1Index = nodeAndIndexPairs[edge.From];
            int vertex2Index = nodeAndIndexPairs[edge.To];


            var isMirrorElementsMatch = adjacencyMatrix[vertex1Index, vertex2Index] == adjacencyMatrix[vertex2Index, vertex1Index];
            if (isMirrorElementsMatch)
            {
                incidentMatrix[vertex1Index][edgeIndex] = 1;
                incidentMatrix[vertex2Index][edgeIndex] = 1;
            }
            else
            {
                incidentMatrix[vertex1Index][edgeIndex] = -1;
                incidentMatrix[vertex2Index][edgeIndex] = 1;
            }
        }

        return new IncidentMatrix(incidentMatrix, new IncidentMatrixNamePrefixs(GraphConsts.NODE_PREFIX + "_", GraphConsts.EDGE_PREFIX + "_"));
    }

    public static RelationshipLists ToRelationshipLists(this Graph graph)
    {
        var nodes = graph.GetNodes().ToList();
        var arces = graph.GetArces();

        var relationListItems = nodes.Select(node =>
        {
            var neighborsFrom = arces.Where(arc => arc.From.Equals(node))
                .Select(edge => edge.To);

            var neighborsTo = arces.Where(arc => arc.To.Equals(node))
                .Select(edge => edge.From);

            var neighbors = neighborsTo.Concat(neighborsTo)
                .ToHashSet()
                .ToArray();

            return new RelationListItem(node, neighbors!);
        }).ToArray();

        return new RelationshipLists(relationListItems);
    }

    public static IncidentsLists ToIncidentsLists(this Graph graph)
    {
        var nodes = graph.GetNodes().ToList();

        var edges = graph.GetEdges();

        var incidentsListItems = nodes.Select(node =>
        {
            var currentEdges = edges.Where(edge => edge.From.Equals(node))
                .ToArray();

            var items = currentEdges.Select(edge =>
            {
                return new IncidentsListsItem(edge, new[] { edge.From, edge.To });
            });

            return items;
        })
        .SelectMany(x => x)
        .ToArray();

        return new IncidentsLists(incidentsListItems);
    }



    private static Dictionary<Node, int> GetNodeIndexPairs(Graph graph)
    {
        var nodeAndIndexPairs = graph.GetNodes()
           .OrderBy(node => node.Number)
           .Select((node, index) => (node: node, index: index))
           .ToDictionary(pair => pair.node, pair => pair.index);

        return nodeAndIndexPairs;
    }
}
