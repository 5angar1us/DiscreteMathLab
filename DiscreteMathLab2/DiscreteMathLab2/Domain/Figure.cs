using FluentValidation;
using Shared;
using System.Numerics;

namespace DiscreteMathLab2.Domain
{
    public class FigureValidator : AbstractValidator<Figure>
    {
        public FigureValidator()
        {
            When(x => x.IsCircle, () =>
            {
                RuleFor(x => x.Radius).GreaterThan(0).When(x => x.IsCircle).WithMessage("Radius must be greater than 0 for circles.");
            }).Otherwise(() =>
            {
                RuleFor(x => x.Width).GreaterThan(0).When(x => !x.IsCircle).WithMessage("Width must be greater than 0 for rectangles.");
                RuleFor(x => x.Height).GreaterThan(0).When(x => !x.IsCircle).WithMessage("Height must be greater than 0 for rectangles.");
            });
        }
    }


    public class Figure
    {
        public bool IsCircle { get; private set; }
        public float X0 { get; }
        public float Y0 { get; }
        public float Width { get; }
        public float Height { get; }
        public float Radius { get; }

        private Figure(bool isCircle, float x0, float y0, float width = 0.0f, float height = 0.0f, float radius = 0.0f)
        {
            IsCircle = isCircle;
            X0 = x0;
            Y0 = y0;

            // Only for rectangles
            Width = width;
            Height = height;

            // Only for circles
            Radius = radius;
        }

        public bool Contains(Point point)
        {
            float x = point.X;
            float y = point.Y;

            if (IsCircle)
            {
                // Distance formula (check if within radius)
                var isPointInCircle = Vector2.DistanceSquared(new Vector2(x, y), new Vector2(X0, Y0)) <= Radius * Radius;
                return isPointInCircle;
            }
            else
            {
                float leftBorder = X0 - Width / 2;
                float rightBorder = X0 + Width / 2;
                float topBorder = Y0 + Height / 2;
                float bottomBorder = Y0 - Height / 2;

                return leftBorder <= x && x <= rightBorder && bottomBorder <= y && y <= topBorder;
            }
        }

        public static ResultFluent<Figure> CreateCircle(float x0, float y0, float radius)
        {
            return Validate(new Figure(isCircle: true, x0, y0, radius: radius));
        }

        public static ResultFluent<Figure> CreateRectangle(float x0, float y0, float width, float height)
        {
            return Validate(new Figure(isCircle: false, x0, y0, width: width, height: height));
        }

        public static ResultFluent<Figure> CreateCircle(Point centerPoint, float radius)
        {
            return CreateCircle(centerPoint.X, centerPoint.Y, radius: radius);
        }

        public static ResultFluent<Figure> CreateRectangle(Point centerPoint, float width, float height)
        {
            return CreateRectangle(centerPoint.X, centerPoint.Y, width, height);
        }

        private static ResultFluent<Figure> Validate(Figure figure)
        {
            var figureValidator = new FigureValidator();
            var validationResult = figureValidator.Validate(figure);

            if (IsNot(validationResult.IsValid))
            {
                return validationResult;
            }
            else
            {
                return figure;
            }
        }
    }


}
