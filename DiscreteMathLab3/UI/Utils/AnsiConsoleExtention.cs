using DiscreteMathLab3.GraphDomain;

using DiscreteMathLab3.UI.InputMatrix.Manual.NAdjacencyMatrixPrompt;

namespace DiscreteMathLab3.UI.Utils
{
    public static class AnsiConsoleExtention
    {

        public static InputAdjacencyMatrixTableView ToTableView(this AdjacencyMatrix adjacencyMatrix)
        {
            return new InputAdjacencyMatrixTableView(adjacencyMatrix);
        }
    }
}
