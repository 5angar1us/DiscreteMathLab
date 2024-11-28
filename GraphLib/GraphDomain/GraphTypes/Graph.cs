using Dunet;
using Shared;

namespace GraphLib.GraphDomain.GraphTypes;

public sealed partial class Graph {
    private List<Edge> Edges { get; init; }
    private HashSet<Node> Nodes { get; init; }
    private Dictionary<Node, HashSet<Edge>> IncidenceMap { get; init; }

    public Graph() {
        this.IncidenceMap = new Dictionary<Node, HashSet<Edge>>();
        this.Edges = new List<Edge>();
        this.Nodes = new HashSet<Node>();
    }

    public Graph(List<Edge> edges)
        : this() {
        foreach (var e in edges) {
            this.AddEdge(e);
        }
    }

    public Graph(List<Edge> edges, HashSet<Node> nodes)
        : this(edges) {
        foreach (var node in nodes) {
            this.AddNode(node);
        }
    }

    private bool AddNode(Node node) {

        if (this.Nodes.Contains(node)) {
            return false;
        }

        this.Nodes.Add(node);
        return true;
    }


    public List<Edge> GetEdges() {
        var arces = GetArces();

        return FilterReverseArces(arces, this);
    }

    public List<Edge> GetArces() {
        return this.Edges
            .OrderBy(edge => edge.From)
            .ToList();
    }

    public HashSet<Node> GetNodes() {
        var hs = new HashSet<Node>();
        foreach (var n in this.Nodes) {
            hs.Add(n);
        }

        return hs;
    }

    public struct DegreeInfo {
        public int Degree;      // Для неориентированных графов
        public int InDegree;    // Для ориентированных графов
        public int OutDegree;   // Для ориентированных графов
    }

    [Union]
    public partial record Degree {
        // 3. Define the union variants as inner partial records.
        partial record DegreeUnoriginalized(int Degree);
        partial record DegreeDirectional(int InDegree, int OutDegree);
        partial record DegreeMixed(int Degree, int InDegree, int OutDegree);
    }

    public int GetDegree(Node node) {
        if (!Nodes.Contains(node)) {
            throw new ArgumentException("The specified node does not exist in the graph");
        }

        // Неориентированный граф: степень — количество инцидентных рёбер
        // Там всегда в 2 раза больше 
        return IncidenceMap[node].Count / 2;

    }

    public Optional<Edge> GetEdge(HashSet<Node> nodes) {
        if (nodes.Count != 2) {
            throw new Exception(
                "GetEdge can only take a set of two nodes. i.e. the nodes that are incident with the edge."
            );
        }

        var nodeList = nodes.ToList();
        var maybeEdge = this.GetEdge(nodeList[0], nodeList[1]);
        if (maybeEdge.HasValue) {
            return maybeEdge;
        }

        var maybeEadgeReverse = this.GetEdge(nodeList[1], nodeList[0]);

        return maybeEadgeReverse;
    }

    public Optional<Edge> GetEdge(Node node1, Node node2) {
        var edge = new Edge(node1, node2);
        foreach (var e in this.Edges) {
            if (edge.Equals(e)) {
                return e;
            }
        }

        return Optional<Edge>.Empty();
    }

    internal bool AddEdge(Edge edge) {
        if (this.GetEdges().Contains(edge)) {
            return false;
        }


        this.Edges.Add(edge);
        var from = edge.From;
        var to = edge.To;
        this.AddNode(to);
        this.AddNode(from);
        this.AddIncidence(from, edge);
        this.AddIncidence(to, edge);

        return true;
    }


    private void AddIncidence(Node node, Edge edge) {
        HashSet<Edge> edges;
        if (this.IncidenceMap.TryGetValue(node, out edges)) {
            edges.Add(edge);
            this.IncidenceMap[node] = edges;
        }
        else {
            var el = new HashSet<Edge>
            {
                edge
            };
            this.IncidenceMap.Add(node, el);
        }
    }

    public bool RemoveNode(Node node) {
        if (!this.Nodes.Contains(node)) {
            return false;
        }

        var incidentEdges = this.IncidenceMap[node];
        if (incidentEdges != null) {
            this.Edges.RemoveAll(e => incidentEdges.Contains(e));
            incidentEdges.RemoveWhere(e => incidentEdges.Contains(e));
        }

        this.Nodes.Remove(node);
        return true;
    }

    public bool RemoveEdge(HashSet<Node> nodes) {
        if (nodes.Count != 2) {
            throw new Exception(
                "RemoveEdge can only take a set of two nodes. i.e. the nodes that are incident with the edge."
            );
        }

        var nodeList = nodes.ToList();
        if (!this.RemoveEdge((nodeList[0], nodeList[1]))) {
            return this.RemoveEdge((nodeList[1], nodeList[0]));
        }

        return true;
    }


    public bool RemoveEdge((Node, Node) fromToNodes) {
        var edge = new Edge(fromToNodes.Item1, fromToNodes.Item2);
        return this.RemoveEdge(edge);
    }

    public bool RemoveEdge(Edge edge) {
        if (!this.Edges.Contains(edge)) {
            return false;
        }

        var f = edge.From;
        var t = edge.To;
        var fromIncidentEdges = this.IncidenceMap[f];
        if (fromIncidentEdges != null) {
            fromIncidentEdges.RemoveWhere(e => e.Equals(edge));
        }

        var toIncidentEdges = this.IncidenceMap[t];
        if (toIncidentEdges != null) {
            toIncidentEdges.RemoveWhere(e => e.Equals(edge));
        }

        this.Edges.Remove(edge);

        return true;
    }

    private void RemoveIncidence(Node node, Edge edge) {
        HashSet<Edge> edges;
        if (this.IncidenceMap.TryGetValue(node, out edges)) {
            edges.Remove(edge);
            this.IncidenceMap[node] = edges;
        }
    }

    private bool IsIncident(Node node, Edge edge) {
        HashSet<Edge> edges;
        if (this.IncidenceMap.TryGetValue(node, out edges)) {
            return edges.Contains(edge);
        }
        else {
            return false;
        }
    }

    private static List<Edge> FilterReverseArces(List<Edge> edges, Graph graph) {
        var filteredEdges = edges.Where(edge => {
            var reverseEdge = graph.GetEdge(edge.To, edge.From);

            if (reverseEdge.IsEmpty) return false;

            var isEqualsOrBiger = edge.CompareTo(reverseEdge.GetValueOrThrow()) <= 0;

            if (isEqualsOrBiger) return true;

            return false;
        });

        return filteredEdges.ToList();
    }
}
