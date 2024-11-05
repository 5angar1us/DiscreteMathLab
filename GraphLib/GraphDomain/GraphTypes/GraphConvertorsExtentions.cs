using DiscreteMathLab3.GraphDomain;
using DiscreteMathLab3.GraphDomain.GraphTypes;
using System.Xml.Linq;

namespace DiscreteMathLab3.GraphTypes;

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

        return new Graph(edges, nodes.ToHashSet(), DetermineGraphType(matrix));
    }

    //TODO что тут происходит???
    private static EGraphType DetermineGraphType(AdjacencyMatrix matrix)
    {
        var isDirected = false;
        var isUndirected = true;

        for (int i = 0; i < matrix.NodeCount; i++)
        {
            for (int j = 0; j < matrix.NodeCount; j++)
            {
                if (matrix[i, j] != matrix[j, i])
                {
                    isUndirected = false;
                }

                if (matrix[i, j] == 1 && matrix[j, i] == 0)
                {
                    isDirected = true;
                }
            }
        }

        if (isDirected && isUndirected)
        {
            return EGraphType.Mixed;
        }
        else if (isDirected)
        {
            return EGraphType.Directional;
        }
        else if (isUndirected)
        {
            return EGraphType.Unoriginalized;
        }
        else
        {
            throw new InvalidOperationException("Unable to determine graph type.");
        }
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

            // TODO Mixed??????
            if (graph.GraphType == EGraphType.Unoriginalized)
            {
                adjacencyMatrix[vertex2Index][vertex1Index] = 1;
            }
        });

        return AdjacencyMatrix.Create(adjacencyMatrix, AdjacencyMatrixNamePrefixes.Create()).Value;
    }

    public static IncidentMatrix ToIncidentMatrix(this Graph graph)
    {
        var excludeOneOfUndirectedEdges = graph.GraphType == EGraphType.Mixed || graph.GraphType == EGraphType.Unoriginalized;

        var edges = excludeOneOfUndirectedEdges ? FilterReverseEdges(graph) : graph.GetEdges();

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
        var edges = graph.GetEdges().ToList();

        var relationListItems = nodes.Select(node =>
        {
            var neighborsFrom = edges.Where(edge => edge.From.Equals(node))
                .Select(edge => edge.To);

            var neighborsTo = edges.Where(edge => edge.To.Equals(node))
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

        var excludeOneOfUndirectedEdges = graph.GraphType == EGraphType.Mixed || graph.GraphType == EGraphType.Unoriginalized;

        var edges = excludeOneOfUndirectedEdges ? FilterReverseEdges(graph) : graph.GetEdges();

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

    private static List<Edge> FilterReverseEdges(Graph graph)
    {
        var edges = graph.GetEdges();
        var filteredEdges = edges.Where(edge =>
        {
            var reverseEdge = graph.GetEdgeOrTrow(edge.To, edge.From);

            if (reverseEdge == null) return false;

            var isEqualsOrBiger = edge.CompareTo(reverseEdge) <= 0;

            if (isEqualsOrBiger) return true;

            return false;
        });

        return filteredEdges.ToList();
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
