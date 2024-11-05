using DiscreteMathLab3.GraphTypes;

namespace DiscreteMathLab3.GraphDomain.GraphTypes;

public static class NodeExtentions
{

    public static bool HasOneConnection(this Node node, Graph graph)
    {
        return graph.GetDegree(node) > 0; ;
    }
}
