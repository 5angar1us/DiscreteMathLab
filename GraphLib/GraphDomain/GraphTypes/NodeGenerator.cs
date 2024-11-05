namespace DiscreteMathLab3.GraphDomain.GraphTypes
{
    public static class NodeGenerator
    {
        public static HashSet<Node> GenerateNodes(int number)
        {
            if (number < 1)
            {
                throw new Exception(
                    string.Format(
                        "argument 'number' must be a positive integer. Value given {0}",
                        number
                    )
                );
            }

            var nodes = Enumerable.Range(1, number).Select(CreateNode).ToHashSet();

            return nodes;
        }

        public static Node CreateNode(int index)
        {
            return new Node(index);
        }
    }


}
