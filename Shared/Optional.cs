namespace Shared;
[Serializable]
public class Optional<T> {
    private bool m_HasValue;
    private T? m_Value;

    public Optional(T value = default) {
        m_HasValue = true;
        m_Value = value;
    }

    private Optional() {
        m_HasValue = false;
        m_Value = default;
    }

    public bool HasValue {
        get {
            return m_HasValue;
        }
    }

    public bool IsEmpty {
        get {
            return IsNot(m_HasValue);
        }
    }

    public T Value {
        get {

            if (IsNot(m_HasValue)) {
                throw new InvalidOperationException(
                    "Optional<T> object must have a value.");
            }
            return m_Value;
        }
        set {
            m_HasValue = true;
            m_Value = value;
        }
    }


    public T GetValueOrDefault(T defaultValue) {
        return m_HasValue ? m_Value : defaultValue;
    }

    public T GetValueOrThrow() {
        if (IsNot(m_HasValue)) {
            throw new InvalidOperationException("Optional<T> object must have a value.");
        }
        return m_Value;
    }

    public void Reset() {
        m_HasValue = false;
        m_Value = default;
    }

    public static explicit operator T(Optional<T> optional) {
        return optional.Value;
    }

    public static implicit operator Optional<T>(T value) {
        return new Optional<T>(value);
    }

    public static implicit operator bool(Optional<T> optional) {
        return optional.m_HasValue;
    }

    public static bool operator true(Optional<T> optional) {
        return optional.m_HasValue;
    }

    public static bool operator false(Optional<T> optional) {
        return !optional.m_HasValue;
    }

    public static Optional<T> Empty() {
        return new Optional<T>();
    }
}
