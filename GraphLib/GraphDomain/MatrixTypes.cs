using FluentValidation;
using Shared;
using System.Collections.ObjectModel;

namespace GraphLib.GraphDomain;

public abstract class Matrix
{
    private readonly int[][] data;

    protected readonly bool isSquare;

    public Matrix(int[][] matrix)
    {
        this.data = matrix;

        var size = matrix.Length;
        isSquare = matrix.All(row => row.Length == size);
    }

    public ReadOnlyCollection<ReadOnlyCollection<int>> Data
    {
        get
        {
            var readOnlyData = new ReadOnlyCollection<ReadOnlyCollection<int>>(
                Array.ConvertAll(data, row => new ReadOnlyCollection<int>(row))
            );
            return readOnlyData;
        }
    }


    public int this[int row, int column]
    {
        get
        {
            if (IsNot(IsValidIndexes(row, column)))
                throw new IndexOutOfRangeException("Index(es) is out of range of the matrix");

            return data[row][column];
        }
    }

    private bool IsValidIndexes(int row, int column)
    {
        var numRows = data.Length;
        var isValidRow = 0 <= row && row <= numRows;
        if (IsNot(isValidRow)) return false;

        var numColumns = Data[row].Count;
        var isValidColumn = 0 <= column && column <= numColumns;
        if (IsNot(isValidColumn)) return false;


        return true;
    }
}

public abstract class NamedMatix : Matrix
{
    public NamePrefixs NameTemplate { get; }

    public NamedMatix(int[][] matrix, NamePrefixs nameTemplate) : base(matrix)
    {
        NameTemplate = nameTemplate;
    }
}

public sealed class AdjacencyMatrix : NamedMatix
{
    public int NodeCount { get => Data.Count; }


    private AdjacencyMatrix(int[][] matrix, AdjacencyMatrixNamePrefixes namePrefix) : base(matrix, namePrefix)
    {
        
    }

    public static ResultFluent<AdjacencyMatrix> Create(int[][] matrix, AdjacencyMatrixNamePrefixes namePrefix)
    {
        var adjacencyMatrix = new AdjacencyMatrix(matrix, namePrefix);

        var figureValidator = new AdjacencyMatrixValidator();
        var validationResult = figureValidator.Validate(adjacencyMatrix);

        if (IsNot(validationResult.IsValid))
        {
            return validationResult;
        }
        else
        {
            return adjacencyMatrix;
        }
    }

}

public class AdjacencyMatrixValidator : AbstractValidator<AdjacencyMatrix>
{
    public AdjacencyMatrixValidator()
    {
        RuleFor(matrix => matrix.Data)
            .Must(BeSquare)
            .WithMessage("The matrix must be square.");

        RuleFor(matrix => matrix.Data)
            .Must(ContainOnlyZerosAndOnes)
            .WithMessage("The matrix must contain only 0s and 1s.");

        RuleFor(matrix => matrix.Data)
            .Must(NotContainSelfLoops)
            .WithMessage("No node can be connected to itself.");

        RuleFor(matrix => matrix.Data)
            .Must(HavePathFromAtLeastOneNodeToAllOthers)
            .WithMessage("There must be a path from at least one node to all other nodes.");

        RuleFor(matrix => matrix.Data)
            .Must(BeUndirected)
            .WithMessage("The graph must be undirected.");
    }

    private bool BeSquare(ReadOnlyCollection<ReadOnlyCollection<int>> matrix)
    {
        int size = matrix.Count;
        return matrix.All(row => row.Count == size);
    }

    private bool ContainOnlyZerosAndOnes(ReadOnlyCollection<ReadOnlyCollection<int>> matrix)
    {
        return matrix.All(row =>
            row.All(element => element == 0 || element == 1)
        );
    }

    private bool NotContainSelfLoops(ReadOnlyCollection<ReadOnlyCollection<int>> matrix)
    {
        for (int i = 0; i < matrix.Count; i++)
        {
            if (matrix[i][i] != 0)
                return false;
        }
        return true;
    }

    private bool HavePathFromAtLeastOneNodeToAllOthers(ReadOnlyCollection<ReadOnlyCollection<int>> matrix)
    {
        int size = matrix.Count;
        var visited = new bool[size];

        for (int startNodeIndex = 0; startNodeIndex < size; startNodeIndex++)
        {
            DFS(matrix, visited, startNodeIndex);

            if (visited.All(node => node))
                return true;

            Array.Fill(visited, false);
        }

        return false;
    }

    private bool BeUndirected(ReadOnlyCollection<ReadOnlyCollection<int>> matrix)
    {
        for (int i = 0; i < matrix.Count; i++)
        {
            for (int j = i + 1; j < matrix.Count; j++)
            {
                if (matrix[i][j] != matrix[j][i])
                    return false;
            }
        }
        return true;
    }

    private void DFS(ReadOnlyCollection<ReadOnlyCollection<int>> matrix, bool[] visited, int vertexIndex)
    {
        visited[vertexIndex] = true;

        for (int otherNodeIndex = 0; otherNodeIndex < matrix.Count; otherNodeIndex++)
        {
            if (matrix[vertexIndex][otherNodeIndex] == 1 && IsNot(visited[otherNodeIndex]))
                DFS(matrix, visited, otherNodeIndex);
        }
    }
}

public class IncidentMatrix : NamedMatix
{
    public int EdgeCount
    {
        get => Data[0].Count;
    }

    public int NodeCount { get => Data.Count; }

    public IncidentMatrix(int[][] matrix, IncidentMatrixNamePrefixs namePrefixs) : base(matrix, namePrefixs)
    {

    }
}
