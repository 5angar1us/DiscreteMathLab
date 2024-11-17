using DiscreteMathLab2.Domain;
using DiscreteMathLab2.UI.InputFigures.FromFile.Parser.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared;
namespace DiscreteMathLab2.UI.InputFigures.FromFile.Parser;

public class ParserFiguresFromFile {

    public Dictionary<string, ResultFluent<Figure>> Parse(string filePath) {
        using StreamReader reader = new StreamReader(filePath);
        string json = reader.ReadToEnd();

        if (string.IsNullOrWhiteSpace(json)) {
            throw new FileEmptyException();
        }

        var data = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(json);

        Dictionary<string, ResultFluent<Figure>> figures = [];
        foreach (var (name, jObject) in data) {
            figures[name] = GetFigureFromJObject(jObject);
        }

        return figures;
    }

    public ResultFluent<Figure> GetFigureFromJObject(JObject jObject) {
        string type = jObject.GetValue<string>("type");


        var isCircle = type switch {
            "circle" => true,
            "rectangle" => false,
            _ => throw new IncorrectFigureTypeException(type)
        };


        if (isCircle) {
            float x0 = jObject.GetValue<float>("x0");
            float y0 = jObject.GetValue<float>("y0");
            float radius = jObject.GetValue<float>("radius");

            return Figure.CreateCircle(x0, y0, radius);
        }
        else {
            float x0 = jObject.GetValue<float>("x0");
            float y0 = jObject.GetValue<float>("y0");
            float width = jObject.GetValue<float>("width");
            float height = jObject.GetValue<float>("height");

            return Figure.CreateRectangle(x0, y0, width, height);
        }
    }
}

static class JsonParserExtention {

    public static T GetValue<T>(this JObject jObject, string propertyName, Func<T>? defaultValue = null) {
        if (jObject == null) {
            throw new ArgumentNullException(nameof(jObject));
        }

        if (jObject.TryGetValue(propertyName, out JToken token)) {
            try {
                return token.Value<T>();
            }
            catch (FormatException) {
                throw new JsonPropertyException($"Неверный формат для свойства '{propertyName}'.");
            }
        }
        else if (defaultValue != null) {
            return defaultValue();
        }
        else {
            throw new JsonPropertyException($"Отсутствует свойство '{propertyName}' в JSON.");
        }
    }

}
