using DiscreteMathLab3.GraphDomain;
using Shared;

namespace DiscreteMathLab3.UI.InputMatrix.FromFile
{
    internal class ParserMatrixFromFile
    {
        public ResultFluent<AdjacencyMatrix> Parse(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            int rowSize = lines.Length;
            int[][] matrix = new int[rowSize][];

            for (int i = 0; i < rowSize; i++)
            {
                int[] elements = lines[i].Split(' ').Select(int.Parse).ToArray();
                if (elements.Length != rowSize)
                    throw new ArgumentException("The file does not contain a square matrix.");

                matrix[i] = elements;
            }

            return AdjacencyMatrix.Create(matrix, AdjacencyMatrixNamePrefixes.Create());
        }
    }
}
