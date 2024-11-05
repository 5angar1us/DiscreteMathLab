namespace GraphLib.GraphDomain.GraphTypes;

public class Edge : IEquatable<Edge>, IComparable<Edge>
{
    public Node From { get; init; }

    public Node To { get; init; }

    public bool IsDirected { get; init; }

    public Edge(Node from, Node to, bool isDirected = false)
    {
        if (from.Equals(to))
        {
            throw new Exception("Edge nodes must be different.");
        }

        From = from;
        To = to;
        IsDirected = isDirected;
    }

    public bool IsSame(Edge edge)
    {
        if (
            From.Equals(edge.From) && To.Equals(edge.To)
            || From.Equals(edge.To) && To.Equals(edge.From)
        )
        {
            return true;
        }

        return false;
    }

    public bool IsSame(Node n1, Node n2)
    {
        if (From.Equals(n1) && To.Equals(n2) || From.Equals(n2) && To.Equals(n1))
        {
            return true;
        }

        return false;
    }

    public HashSet<Node> Nodes()
    {
        var nodes = new HashSet<Node>
        {
            From,
            To
        };
        return nodes;
    }

    public Node GetOther(Node n)
    {
        if (From.Equals(n))
        {
            return To;
        }
        else if (To.Equals(n))
        {
            return From;
        }
        else
        {
            throw new Exception("Cannot get other node to a non-incident node edge pair.");
        }
    }
    public override string ToString()
    {
        return "(" + From + ", " + To + ")";
    }

    public bool Equals(Edge? other)
    {
        if (other == null) return false;

        return From == other.From && To == other.To;
    }

    public override bool Equals(object obj)
    {
        if (obj is Edge)
        {
            return Equals((Edge)obj);
        }

        return false;
    }

    public override int GetHashCode()
    {
        var h = (From.GetHashCode() ^ 38334421) + To.GetHashCode() * 11;
        return h;
    }

    public int CompareTo(Edge other)
    {
        if (other == null)
            return 1;

        var fromComparison = From.CompareTo(other.From);
        if (fromComparison != 0)
            return fromComparison;

        return To.CompareTo(other.To);
    }
}
