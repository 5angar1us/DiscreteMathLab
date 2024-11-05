namespace DiscreteMathLab3.UI.InputMatrix.Manual.NAdjacencyMatrixPrompt;

public record CellIndexes(int Row, int Column)
{
    public CellIndexes(CellIndexes cell)
    {
        Row = cell.Row;
        Column = cell.Column;
    }
}
