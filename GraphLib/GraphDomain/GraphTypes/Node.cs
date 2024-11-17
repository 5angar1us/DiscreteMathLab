namespace GraphLib.GraphDomain.GraphTypes;

public class Node : IEquatable<Node>, IComparable<Node> {
    public int Number { get; init; }

    public Node(int number) {
        Number = number;
    }


    public override string ToString() {
        return Number.ToString();
    }

    public override int GetHashCode() {
        return HashCode.Combine(Number);
    }

    public bool Equals(Node other) {
        return Number.Equals(other.Number);
    }

    public override bool Equals(object obj) {
        if (obj is Node) {
            return Equals((Node)obj);
        }

        return false;
    }

    public int CompareTo(Node other) {
        if (other == null) {
            return 1;
        }

        return Number.CompareTo(other.Number);
    }


}
