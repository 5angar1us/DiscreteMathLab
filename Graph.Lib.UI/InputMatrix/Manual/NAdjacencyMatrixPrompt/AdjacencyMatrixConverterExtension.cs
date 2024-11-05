using DiscreteMathLab3.GraphDomain;
using Shared;

namespace DiscreteMathLab3.UI.InputMatrix.Manual.NAdjacencyMatrixPrompt;

public static class AdjacencyMatrixConverterExtension
{
    public static ResultFluent<AdjacencyMatrix> ToAdjacencyMatrix(this InputAdjacencyMatrixTypes inputMatrix, AdjacencyMatrixNamePrefixes namePrefix)
    {
        int[][] matrix = new int[inputMatrix.Size][];
        for (int row = 0; row < inputMatrix.Size; row++)
        {
            matrix[row] = new int[inputMatrix.Size];
            for (int column = 0; column < inputMatrix.Size; column++)
            {
                matrix[row][column] = inputMatrix[row, column];
            }
        }

        return AdjacencyMatrix.Create(matrix, namePrefix);
    }

    public static InputAdjacencyMatrixTypes ToInputAdjacencyMatrix(this AdjacencyMatrix adjacencyMatrix)
    {

        int size = adjacencyMatrix.NodeCount;

        var inputAdjacencyMatrix = new InputAdjacencyMatrixTypes(size);

        for (int row = 0; row < size; row++)
        {
            for (int column = 0; column < size; column++)
            {
                inputAdjacencyMatrix[row, column] = adjacencyMatrix[row, column];
            }
        }

        return inputAdjacencyMatrix;
    }
}


