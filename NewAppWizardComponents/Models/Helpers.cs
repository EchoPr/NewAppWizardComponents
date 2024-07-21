

namespace NewAppWizardComponents;
internal class Helpers
{
}

public interface IValueProperty
{
    object Value { get; set; }
}

public class ValueProperty<T> : IValueProperty
{
    public T value { get; set; }

    public ValueProperty(T value_)
    {
        value = value_;
    }

    object IValueProperty.Value
    {
        get => value;
        set => this.value = (T)value;
    }
}


