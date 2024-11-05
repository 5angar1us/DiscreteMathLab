using GraphLib.GraphDomain;
using Shared;
using Spectre.Console;


namespace GraphLib.UI.InputMatrix.Manual.NAdjacencyMatrixPrompt
{

    public record InputAdjacencyMatrixTableView(InputAdjacencyMatrixTypes Matrix, Optional<CellIndexes> CurrentCell)
    {
        public InputAdjacencyMatrixTableView(AdjacencyMatrix adjacencyMatrix) : this(new InputAdjacencyMatrixTypes(adjacencyMatrix), Optional<CellIndexes>.Empty())
        {

        }
    }

    public static class AnsiConsoleWriteExtention
    {

        public static Table ToTable(this InputAdjacencyMatrixTableView matrixTableView)
        {
            var table = new Table().RoundedBorder();

            AddTableHeader(ref table, matrixTableView.Matrix.Size);
            AddTableRows(ref table, matrixTableView.Matrix, matrixTableView.CurrentCell);

            return table;
        }

        private static void AddTableHeader(ref Table table, int size)
        {
            table.AddColumn(new TableColumn("").Centered());
            for (int i = 0; i < size; i++)
            {
                table.AddColumn(new TableColumn((i + 1).ToString()).Centered());
            }
        }

        private static void AddTableRows(ref Table table, InputAdjacencyMatrixTypes matrix, Optional<CellIndexes> currentCell)
        {
            for (int rowIndex = 0; rowIndex < matrix.Size; rowIndex++)
            {
                var cells = new List<Text>
                {
                    new((rowIndex+1).ToString())
                };

                for (int cellIndex = 0; cellIndex < matrix.Size; cellIndex++)
                {

                    var cellValue = matrix[rowIndex, cellIndex].ToString();

                    Style? style = null;


                    var isSelectedCell = currentCell.HasValue
                        && currentCell.Value.Row == rowIndex
                        && currentCell.Value.Column == cellIndex;

                    var isDiagonalCell = rowIndex == cellIndex;

                    if (isSelectedCell)
                    {
                        style = Style.Parse("bold yellow");
                    }
                    else if (isDiagonalCell)
                    {
                        style = Style.Parse("bold red");
                    }
                    if (style != null) style.ToMarkup();

                    var cellText = new Text(cellValue, style);

                    cells.Add(cellText);
                }
                table.AddRow(cells.ToArray());
            }
        }
    }
}