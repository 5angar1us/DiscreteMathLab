public class Point {
    public float X { get; }
    public float Y { get; }

    private Point(float x, float y) {
        X = x;
        Y = y;
    }

    public static Point Create(float x, float y) {
        return new Point(x, y);
    }


}
