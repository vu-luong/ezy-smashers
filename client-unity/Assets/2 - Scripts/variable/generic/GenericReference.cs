using System;

[Serializable]
public class GenericReference<T>
{
    public bool UseConstant = true;
    public T ConstantValue;
    public GenericVariable<T> Variable;

    public GenericReference() { }

    public GenericReference(T value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public T Value
    {
        get
        {
            return UseConstant ? ConstantValue : Variable.Value;
        }
    }

    public static implicit operator T(GenericReference<T> reference)
    {
        return reference.Value;
    }
}
