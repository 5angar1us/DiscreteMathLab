using System.Text;

namespace DiscreteMathLab;


public class TruthTableItem {
    public bool A { get; set; }
    public bool B { get; set; }
    public bool C { get; set; }
    public bool D { get; set; }
    public bool E { get; set; }
    public bool F { get; set; }


    public IEnumerable<bool> GetAll() {
        yield return A;
        yield return B;
        yield return C;
        yield return D;
        yield return E;
        yield return F;
    }
}

public class InputVariables {
    public InputVariables(int bits) {
        if (bits > Math.Pow(2, bits)) {
            throw new ArgumentException(nameof(bits));
        }

        Bits = bits;
    }

    private int Bits { get; set; }

    public static int variablesCount = 5;

    public bool A {
        get { return getBitValue(4); }
    }

    public bool B {
        get { return getBitValue(3); }
    }

    public bool C {
        get { return getBitValue(2); }
    }

    public bool D {
        get { return getBitValue(1); }
    }

    public bool E {
        get { return getBitValue(0); }
    }

    private bool getBitValue(int bitIndex) {
        var targetBitOffset = (int)Math.Pow(2, bitIndex);
        var bitValue = Bits & targetBitOffset;

        var isBitValueTrue = bitValue != 0;

        return isBitValueTrue;
    }

}

public class DNFBuilder {
    private StringBuilder _builder = new();
    private string _termSeparator { get; init; }
    private string _elementSeparator { get; init; }
    private Func<bool, bool> _DNFUpdater { get; init; }
    private int _count = 0;

    public DNFBuilder(string separator, string separator2, Func<bool, bool> dNF) {
        _termSeparator = separator;
        _elementSeparator = separator2;
        _DNFUpdater = dNF;
    }

    public void AppendTerm(TruthTableItem row) {
        if (_builder.Length > 0) {
            _builder.Append($" {_termSeparator} ");
        }

        if (_count == 3) {
            _builder.Append(Environment.NewLine);
            _count = 0;
        }

        _builder.Append(CreateTerm(row));
        _count++;
    }


    private string CreateTerm(TruthTableItem row) {
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

    private string FormatValue(bool value, string name) {
        var updatedValue = _DNFUpdater(value);
        return updatedValue ? name : $"!{name}";
    }

    public override string ToString() {
        return _builder.ToString();
    }
}
