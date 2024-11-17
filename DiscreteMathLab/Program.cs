using System.Text;
using DiscreteMathLab;
using Spectre.Console;
using static Shared.BooleanUtils;


class Program
{
    static void Main(string[] args)
    {
        IEnumerable<TruthTableItem> truthTable = GenerateTruthTable();

        DNFBuilder sdnfBuilder = new DNFBuilder("|", "&", (value) => value);
        DNFBuilder sknfBuilder = new DNFBuilder("&", "|", IsNot);

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

        Render.View(truthTable.ToList());

        AnsiConsole.MarkupLine($"Совершенная дизъюнктивная нормальная форма (СДНФ): {Environment.NewLine}{sdnfBuilder.ToString()}");
        AnsiConsole.MarkupLine($"Совершенная конъюнктивная нормальная форма (СКНФ): {Environment.NewLine}{sknfBuilder.ToString()}");
    }

    static IEnumerable<InputVariables> CreateInputVariables()
    {
        // bit is 0 or 1
        // int = 4 byte = 32 bit
        // 2^5 = 32 комбинаций
        int border = (int)Math.Pow(2, InputVariables.variablesCount);

        for (int i = 0; i < border; i++)
        {
            yield return new InputVariables(i);
        }
    }

    static IEnumerable<TruthTableItem> GenerateTruthTable()
    {
        var inputVariables = CreateInputVariables();

        var truthTable = inputVariables.Select(x =>
        {
            var a = x.A;
            var b = x.B;
            var c = x.C;
            var d = x.D;
            var e = x.E;

            // f = (a ∧ b) ∨ ¬c ∨ (¬d ∧ e)
            bool f = (a && b) || !c || (!d && e);

            return new TruthTableItem
            {
                A = a,
                B = b,
                C = c,
                D = d,
                E = e,
                F = f
            };
        });

        return truthTable;
    }

    
}

