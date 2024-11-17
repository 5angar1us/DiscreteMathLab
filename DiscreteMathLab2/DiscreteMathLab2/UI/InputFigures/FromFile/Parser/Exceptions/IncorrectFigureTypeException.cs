namespace DiscreteMathLab2.UI.InputFigures.FromFile.Parser.Exceptions;

public class IncorrectFigureTypeException : ParseException {
    public IncorrectFigureTypeException(string type) : base($"Неизвестный тип фигуры: {type}") {

    }
}
