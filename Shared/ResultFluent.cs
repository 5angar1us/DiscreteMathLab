using FluentValidation.Results;

namespace Shared;

public class ResultFluent<T> {
    public required T Value { get; init; }
    public required ValidationResult ValidationResult { get; init; }
    public bool IsSuccess { get; init; }

    public bool IsFailure { get { return !IsSuccess; } }


    private ResultFluent() { }

    public static ResultFluent<T> Success(T value) {
        return new ResultFluent<T> {
            IsSuccess = true,
            Value = value,
            ValidationResult = new ValidationResult()
        };
    }

    public static ResultFluent<T> Failure(ValidationResult validationResult) {
        if (validationResult == null)
            throw new ArgumentNullException(nameof(validationResult), "ValidationResult cannot be null on failure.");

        return new ResultFluent<T> {
            IsSuccess = false,
            Value = default(T),
            ValidationResult = validationResult
        };
    }

    public ResultFluent<T> OnSuccess(Action<T> action) {
        if (IsSuccess && action != null) {
            action(Value);
        }
        return this;
    }

    // Метод для выполнения действия в случае неудачи
    public ResultFluent<T> OnFailure(Action<ValidationResult> action) {
        if (!IsSuccess && action != null) {
            action(ValidationResult);
        }
        return this;
    }

    // Метод для преобразования результата
    public ResultFluent<K> Map<K>(Func<T, K> map) {
        if (IsSuccess) {
            return ResultFluent<K>.Success(map(Value));
        }
        else {
            return ResultFluent<K>.Failure(ValidationResult);
        }
    }

    public ResultFluent<K> Map<K>(Func<T, ResultFluent<K>> map) {
        if (IsSuccess) {
            try {
                return map(Value);
            }
            catch (Exception ex) {
                // Обработка исключения, возможно, создание ValidationResult
                var validationFailure = new ValidationFailure("", ex.Message);
                var validationResult = new ValidationResult(new[] { validationFailure });
                return ResultFluent<K>.Failure(validationResult);
            }
        }
        else {
            return ResultFluent<K>.Failure(ValidationResult);
        }
    }

    public T? GetValueOrDefault() {
        return IsSuccess ? Value : default;
    }

    public static implicit operator ResultFluent<T>(ValidationResult error) => Failure(error);

    public static implicit operator ResultFluent<T>(T value) => Success(value);

    public static ResultFluent<T> FromValidationResult(ValidationResult error) => Failure(error);

    public static ResultFluent<T> FromValue(T value) => Success(value);

}
