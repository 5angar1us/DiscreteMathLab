using System.Text;
using Spectre.Console;
using static Shared.BooleanUtils;

public class TruthTable
{
    public bool A { get; set; }
    public bool B { get; set; }
    public bool C { get; set; }
    public bool D { get; set; }
    public bool E { get; set; }
    public bool F { get; set; }


    public IEnumerable<bool> GetAll()
    {
        yield return A;
        yield return B;
        yield return C;
        yield return D;
        yield return E;
        yield return F;
    }
}

public class DNFBuilder
{
    private StringBuilder _builder = new();
    private string _termSeparator { get; init; }
    private string _elementSeparator { get; init; }
    private Func<bool, bool> _DNFUpdater { get; init; }
    private int _count = 0;

    public DNFBuilder(string separator, string separator2, Func<bool, bool> dNF)
    {
        _termSeparator = separator;
        _elementSeparator = separator2;
        _DNFUpdater = dNF;
    }

    public void AppendTerm(TruthTable row)
    {
        if (_builder.Length > 0)
        {
            _builder.Append($" {_termSeparator} ");
        }

        if (_count == 3)
        {
            _builder.Append(Environment.NewLine);
            _count = 0;
        }

        _builder.Append(CreateTerm(row));
        _count++;
    }


    private string CreateTerm(TruthTable row)
    {
        StringBuilder term = new StringBuilder();
        term.Append('(');

        term.Append(FormatValue(row.A, "a"));
        term.Append($" {_elementSeparator} ");

        term.Append(FormatValue(row.B, "b"));
        term.Append($" {_elementSeparator} ");

        term.Append(FormatValue(row.C, "c"));
        term.Append($" {_elementSeparator} ");

        term.Append(FormatValue(row.D, "d"));
        term.Append($" {_elementSeparator} ");

        term.Append(FormatValue(row.E, "e"));

        term.Append(')');

        return term.ToString();
    }

    private string FormatValue(bool value, string name)
    {
        var updatedValue = _DNFUpdater(value);
        return updatedValue ? name : $"!{name}";
    }

    public override string ToString()
    {
        return _builder.ToString();
    }
}

class Program
{
    static void Main(string[] args)
    {
        List<TruthTable> truthTable = GenerateTruthTable();

        DNFBuilder sdnfBuilder = new DNFBuilder("|", "&", (value) => value);
        DNFBuilder sknfBuilder = new DNFBuilder("&", "|",  IsNot);

        foreach (var row in truthTable)
        {
            var isSDNF = row.F;
            if (isSDNF)
            {
                sdnfBuilder.AppendTerm(row);
            }
            else
            {
                sknfBuilder.AppendTerm(row);
            }
        }

        RenderTruthTable(truthTable);

        AnsiConsole.MarkupLine($"Совершенная дизъюнктивная нормальная форма (СДНФ): {Environment.NewLine}{sdnfBuilder.ToString()}");
        AnsiConsole.MarkupLine($"Совершенная конъюнктивная нормальная форма (СКНФ): {Environment.NewLine}{sknfBuilder.ToString()}");
    }

    static List<TruthTable> GenerateTruthTable()
    {
        List<TruthTable> table = new List<TruthTable>();

        // bit is 0 or 1
        // int = 4 byte = 32 bit
        // 2^5 = 32 комбинаций
        for (int i = 0; i < 32; i++) 
        {
            bool a = getBitValue(i, 4);
            bool b = getBitValue(i, 3);
            bool c = getBitValue(i, 2);
            bool d = getBitValue(i, 1);
            bool e = getBitValue(i, 0);

            // f = (a ∧ b) ∨ ¬c ∨ (¬d ∧ e)
            bool f = (a && b) || !c || (!d && e);

            table.Add(new TruthTable
            {
                A = a,
                B = b,
                C = c,
                D = d,
                E = e,
                F = f
            });
        }
        return table;
    }

    static bool getBitValue(int sourceInt, int bitIndex)
    {
        var targetBitOffset = (int)Math.Pow(2, bitIndex);
        var bitValue = sourceInt & targetBitOffset;

        var isBitValueTrue = bitValue != 0;

        return isBitValueTrue;
    }

    static void RenderTruthTable(List<TruthTable> table)
    {
        var tableDisplay = new Table();

        tableDisplay.AddColumn("a");
        tableDisplay.AddColumn("b");
        tableDisplay.AddColumn("c");
        tableDisplay.AddColumn("d");
        tableDisplay.AddColumn("e");
        tableDisplay.AddColumn("f");

        foreach (var row in table)
        {
            var rowValues = row.GetAll().Select(BoolToInt);

            tableDisplay.AddRow(
               rowValues.ToArray()
            );
        }

        AnsiConsole.Write(tableDisplay);
    }

    static string BoolToInt(bool value)
    {
        return value ? "1" : "0";
    }
}

