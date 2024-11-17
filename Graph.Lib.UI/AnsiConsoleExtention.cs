using GraphLib.GraphDomain;

using GraphLib.UI.InputMatrix.Manual.NAdjacencyMatrixPrompt;

namespace DiscreteMathLab3.UI;

public static class AnsiConsoleExtention {

    public static InputAdjacencyMatrixTableView ToTableView(this AdjacencyMatrix adjacencyMatrix) {
        return new InputAdjacencyMatrixTableView(adjacencyMatrix);
    }
}
