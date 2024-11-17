namespace GraphLib.GraphDomain.GraphTypes;


public interface ITurnEageInDefaultDirection {
    public Edge Turn(Edge a);
}

public class DirectionalGraphTurnEageInDefaultDirection : ITurnEageInDefaultDirection {
    public Edge Turn(Edge a) {
        return a;
    }
}

public class UnoriginalizedGraffTurnEageInDefaultDirection : ITurnEageInDefaultDirection {
    public Edge Turn(Edge edge) {
        return edge.From.Number.CompareTo(edge.To.Number) <= 0
            ? edge
            : new Edge(edge.To, edge.From);

    }
}
