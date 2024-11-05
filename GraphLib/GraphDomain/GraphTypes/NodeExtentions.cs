using DiscreteMathLab3.GraphTypes;

namespace DiscreteMathLab3.GraphDomain.GraphTypes;

public static class NodeExtentions
{

    public static bool HasOneConnection(this Node node, Graph graph)
    {
        var hasConnection = false;

        graph.GetDegree(node).Match(
            Unoriginalized =>
            {
                hasConnection = Unoriginalized.Degree > 0;
            },
            Directional =>
            {
                //TODO Правильно????
                hasConnection = Directional.InDegree > 0 || Directional.OutDegree > 0;
            },
            Mixed =>
            {
                // TODO Правильно ????
                hasConnection = Mixed.Degree > 0 || Mixed.InDegree > 0 || Mixed.OutDegree > 0;
            }
            );


        return hasConnection;
    }
}
