using FluentValidation;
using Shared;

namespace DiscreteMathLab2.Domain
{
    public class Figures
    {
        readonly Dictionary<string, Figure> figures = new(FiguresConsts.FIGURE_COUNT);

        private Figures(Dictionary<string, Figure> figures)
        {
            this.figures = figures;
        }

        public int Count => figures.Count;

        public Figure GetBy(EFigures figureName)
        {
            if (IsNot(figures.TryGetValue(figureName.ToString(), out var figure)))
            {
                throw new ArgumentException(nameof(figureName));
            }
            else
            {
                return figure;
            }
        }

        public static ResultFluent<Figures> Create(Dictionary<string, Figure> figures)
        {
            var figuresValidator = new FiguresValidator();
            var validationResult = figuresValidator.Validate(figures);

            if (IsNot(validationResult.IsValid))
            {
                return validationResult;
            }
            else
            {
                return new Figures(figures);
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj is Figures otherFigures)
            {
                return figures.Count == otherFigures.figures.Count &&
                       figures.All(kvp =>
                           otherFigures.figures.TryGetValue(kvp.Key, out var otherFigure) &&
                           EqualityComparer<Figure>.Default.Equals(kvp.Value, otherFigure));
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            foreach (var kvp in figures)
            {
                hash = hash * 31 + kvp.Key.GetHashCode();
                hash = hash * 31 + (kvp.Value?.GetHashCode() ?? 0);
            }
            return hash;
        }
    }

    public class FiguresValidator : AbstractValidator<Dictionary<string, Figure>>
    {
        public FiguresValidator()
        {
            RuleFor(figures => figures.Count).Equal(FiguresConsts.FIGURE_COUNT).WithMessage($"The number of figures should be {FiguresConsts.FIGURE_COUNT}.");
            RuleForEach(figures => figures.Values).SetValidator(new FigureValidator());
            RuleFor(figures => figures.Keys).Must(ContainAllFigureNames).WithMessage("Not all required figures are present.");
        }

        private bool ContainAllFigureNames(ICollection<string> figureNames)
        {
            var requiredNames = Enum.GetNames(typeof(EFigures));
            return requiredNames.All(name => figureNames.Contains(name));
        }
    }

    internal class FiguresConsts
    {
        public static int FIGURE_COUNT = 4;
    }

    public enum EFigures
    {
        A,
        B,
        C,
        D
    }

    public static class EFiguresExtensions
    {
        public static string ToString(this EFigures @enum) => @enum.ToString();
    }
}
