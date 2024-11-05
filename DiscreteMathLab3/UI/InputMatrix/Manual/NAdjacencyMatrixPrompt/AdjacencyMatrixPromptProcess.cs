using DiscreteMathLab3.GraphDomain;
using Shared.AnsiConsole;
using Spectre.Console;

namespace DiscreteMathLab3.UI.InputMatrix.Manual.NAdjacencyMatrixPrompt;

internal class AdjacencyMatrixPromptProcess : IPromptProcess<AdjacencyMatrix>
{
    public AdjacencyMatrixPromptProcess(InputAdjacencyMatrixTypes sourceMatrix)
    {
        adjacencyMatrixFillPrompt = new AdjacencyMatrixFillPrompt(sourceMatrix);
        this.sourceMatrix = sourceMatrix;
    }

    private readonly InputAdjacencyMatrixTypes sourceMatrix;
    private readonly AdjacencyMatrixFillPrompt adjacencyMatrixFillPrompt;

    public AdjacencyMatrix Show(IAnsiConsole console)
    {
        var matrixRaw = new InputAdjacencyMatrixTypes(sourceMatrix);

        while (true)
        {

            var matrix = adjacencyMatrixFillPrompt.Show(console)
                .ToAdjacencyMatrix(AdjacencyMatrixNamePrefixes.Create());

            if (matrix.IsFailure)
            {
                console.Write(matrix.ValidationResult);
                continue;
            }

            return matrix.Value;
        }
    }

}
