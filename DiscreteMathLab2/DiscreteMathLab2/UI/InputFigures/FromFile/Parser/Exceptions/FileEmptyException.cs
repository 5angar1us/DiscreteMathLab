namespace DiscreteMathLab2.UI.InputFigures.FromFile.Parser.Exceptions;

public class FileEmptyException : ParseException {
    public FileEmptyException() : base("Файл пуст или содержит только пробелы.") { }

}
