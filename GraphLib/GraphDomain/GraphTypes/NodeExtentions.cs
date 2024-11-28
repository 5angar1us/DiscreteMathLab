namespace GraphLib.GraphDomain.GraphTypes;

public static class NodeExtentions {

    public static bool HasOneOrMoreConnection(this Node node, Graph graph) {
        return graph.GetDegree(node) > 0;
    }
}
