namespace GraphLib.GraphDomain;

public abstract record NamePrefixs(string Row, string Column);
public record AdjacencyMatrixNamePrefixes : NamePrefixs {
    private AdjacencyMatrixNamePrefixes(string name) : base(name, name) { }

    public static AdjacencyMatrixNamePrefixes Create() {
        return new AdjacencyMatrixNamePrefixes(GraphConsts.NODE_PREFIX + "_");
    }

}

public record IncidentMatrixNamePrefixs : NamePrefixs {
    public IncidentMatrixNamePrefixs(string Row, string Column) : base(Row, Column) { }

}

public record ListsNamePrefixs : NamePrefixs {
    public ListsNamePrefixs(string Row, string Column) : base(Row, Column) {

    }
}
