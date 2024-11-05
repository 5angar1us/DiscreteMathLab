using GraphLib.GraphDomain;

namespace GraphLib.UI.InputMatrix.Manual.NAdjacencyMatrixPrompt;

public record CornerCells(CellIndexes TopLeft, CellIndexes BottomRigh);

// Represents a matrix of size NxN
public class InputAdjacencyMatrixTypes
{
    public int Size { get; init; }

    private readonly int[,] data;

    public CornerCells CornerCells { get; init; }


    public InputAdjacencyMatrixTypes(int size)
    {
        Size = size;
        data = new int[size, size];
        CornerCells = CreateCornerCells(Size);
    }

    public InputAdjacencyMatrixTypes(InputAdjacencyMatrixTypes other)
    {
        Size = other.Size;

        var copyData = new int[Size, Size];
        Array.Copy(other.data, copyData, other.data.Length);
        data = copyData;

        CornerCells = other.CornerCells;
    }

    public InputAdjacencyMatrixTypes(AdjacencyMatrix adjacencyMatrix)
    {
        Size = adjacencyMatrix.NodeCount;
        data = new int[Size, Size];

        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                data[i, j] = adjacencyMatrix[i, j];
            }
        }
        CornerCells = CreateCornerCells(Size);
    }

    public static CornerCells CreateCornerCells(int size)
    {
        return new CornerCells(
            TopLeft: new CellIndexes(0, 0),
            BottomRigh: new CellIndexes(size - 1, size - 1));
    }

    public int this[int row, int column]
    {
        get
        {
            if (IsNot(IsValidIndexes(row, column)))
                throw new IndexOutOfRangeException("Index(es) is out of range of the matrix");

            return data[row, column];
        }
        set
        {
            if (IsNot(IsValidIndexes(row, column)))
                throw new IndexOutOfRangeException("Index(es) is out of range of the matrix");

            data[row, column] = value;
        }
    }

    private bool IsValidIndexes(int row, int column)
    {
        var isValidRow = 0 <= row && row <= Size;

        var isValidColumn = 0 <= column && column <= Size;

        return isValidRow && isValidColumn;
    }

}


