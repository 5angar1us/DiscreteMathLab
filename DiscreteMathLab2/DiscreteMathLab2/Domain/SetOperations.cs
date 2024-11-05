namespace DiscreteMathLab2.Domain;

public static class SetOperations
{
    public static bool IsPointInSet(Point point, Figures figures)
    {

        bool in_A = figures.GetBy(EFigures.A).Contains(point);
        bool in_B = figures.GetBy(EFigures.B).Contains(point);
        bool in_C = figures.GetBy(EFigures.C).Contains(point);
        bool in_D = figures.GetBy(EFigures.D).Contains(point);


        // f = (A ∩ (¬B ∪ D)) ∪ C 
        bool f = in_A && (IsNot(in_B) || in_D )|| in_C;

        return f;
    }
}
