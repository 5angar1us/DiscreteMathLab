using Spectre.Console;
using Shared.AnsiConsole;


namespace GraphLib.UI.InputMatrix.Manual.NAdjacencyMatrixPrompt;

internal class AdjacencyMatrixFillPrompt : IPrompt<InputAdjacencyMatrixTypes>
{

    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    private Dictionary<ConsoleKey, Direction> keyDirectionMap = new Dictionary<ConsoleKey, Direction>
    {
        { ConsoleKey.UpArrow, Direction.Up },
        { ConsoleKey.DownArrow, Direction.Down },
        { ConsoleKey.LeftArrow, Direction.Left },
        { ConsoleKey.RightArrow, Direction.Right }
    };

    private readonly InputAdjacencyMatrixTypes matrix;

    public AdjacencyMatrixFillPrompt(InputAdjacencyMatrixTypes matrix)
    {
        this.matrix = new InputAdjacencyMatrixTypes(matrix);
    }

    public InputAdjacencyMatrixTypes Show(IAnsiConsole console)
    {
        return ShowAsync(console, CancellationToken.None).GetAwaiter().GetResult();
    }

    public async Task<InputAdjacencyMatrixTypes> ShowAsync(IAnsiConsole console, CancellationToken cancellationToken)
    {
        var instruct = "При заполнении матрицы смежности используйте клавиши со стрелками для перемещения по матрице и нажимайте «0» или «1»," +
            " чтобы заполнить ячейки. Нажмите «Enter», чтобы сохранить изменения.";

        return await console.RunExclusive(async () =>
        {
            CellIndexes currentCell = new CellIndexes(0, 0);

            var isErrorShowing = false;
            var isNeedExit = false;

            using HideCursor hideCursor = new(console);
            while (true)
            {

                console.MarkupLine(instruct);
                console.WriteLine();

                if (isErrorShowing)
                {
                    console.MarkupLine("Ошибка: Неверный ввод. Пожалуйста, введите '0' или '1'".FormatException());
                    console.WriteLine();
                    isErrorShowing = false;
                }

                console.Write(Align.Center(new InputAdjacencyMatrixTableView(matrix, currentCell).ToTable()));

                if (TryReadKey(console, out var keyInfo))
                {
                    if (keyDirectionMap.ContainsKey(keyInfo.Key))
                    {
                        currentCell = MoveCursor(currentCell, keyDirectionMap[keyInfo.Key], matrix.CornerCells, matrix.Size);
                    }
                    else
                    {
                        switch (keyInfo.Key)
                        {
                            case ConsoleKey.D0:
                            case ConsoleKey.NumPad0:
                                UpdateMatrix(currentCell, 0);
                                break;
                            case ConsoleKey.D1:
                            case ConsoleKey.NumPad1:
                                UpdateMatrix(currentCell, 1);
                                break;
                            case ConsoleKey.Enter:
                                isNeedExit = true;
                                break;
                            default:
                                isErrorShowing = true;
                                break;
                        }
                    }
                }

                
                console.Clear();
                if (isNeedExit)
                {
                    break;
                }
            }

             return matrix;


        }).ConfigureAwait(false);

    }

    private bool TryReadKey(IAnsiConsole console, out ConsoleKeyInfo keyInfo)
    {
        ConsoleKeyInfo? input = console.Input.ReadKey(true);

        if (input.HasValue)
        {
            keyInfo = input.Value;
            return true;
        }

        keyInfo = default;
        return false;
    }

    private void UpdateMatrix(CellIndexes currentCell, int value)
    {
        if (currentCell.Row == currentCell.Column) return;

        matrix[currentCell.Row, currentCell.Column] = value;
    }

    private CellIndexes MoveCursor(CellIndexes currentCell, Direction direction, CornerCells cornerСells, int matrixSize)
    {
        var newCell = CalculateNextCell(currentCell, direction, matrixSize);

        //For diagonal cells
        var isSameNode = newCell.Row == newCell.Column;
        if (isSameNode)
        {
            int last = matrixSize - 1;

            newCell = (newCell, direction) switch
            {
                (var cell, Direction.Up) when cell.Equals(cornerСells.TopLeft) => new CellIndexes(0, 1),
                (var cell, Direction.Left) when cell.Equals(cornerСells.TopLeft) => new CellIndexes(1, 0),
                (var cell, Direction.Down) when cell.Equals(cornerСells.BottomRigh) => new CellIndexes(last, last - 1),
                (var cell, Direction.Right) when cell.Equals(cornerСells.BottomRigh) => new CellIndexes(last - 1, last),
                _ => CalculateNextCell(newCell, direction, matrixSize)
            };
        }

        return newCell;
    }

    private CellIndexes CalculateNextCell(CellIndexes currentCell, Direction direction, int matrixSize)
    {
        return direction switch
        {
            Direction.Up => currentCell with { Row = (currentCell.Row - 1 + matrixSize) % matrixSize },
            Direction.Down => currentCell with { Row = (currentCell.Row + 1) % matrixSize },
            Direction.Left => currentCell with { Column = (currentCell.Column - 1 + matrixSize) % matrixSize },
            Direction.Right => currentCell with { Column = (currentCell.Column + 1) % matrixSize },
            _ => throw new ArgumentException()
        };
    }
}
