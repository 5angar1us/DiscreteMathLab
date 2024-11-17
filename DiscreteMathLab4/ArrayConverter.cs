namespace DiscreteMathLab4;

public static class ArrayConverter {

    public static int[][] ConvertToJaggedArray(int[,] adjMatrix) {
        int rows = adjMatrix.GetLength(0);
        int cols = adjMatrix.GetLength(1);

        int[][] matrix = new int[rows][];

        for (int i = 0; i < rows; i++) {
            matrix[i] = new int[cols];
            for (int j = 0; j < cols; j++) {
                matrix[i][j] = adjMatrix[i, j];
            }
        }

        return matrix;
    }
}