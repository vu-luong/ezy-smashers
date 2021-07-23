using UnityEngine;

public class GenericVariable<T> : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

    public T Value;

    public void SetValue(T value)
    {
        Value = value;
    }

    public void SetValue(GenericVariable<T> value)
    {
        Value = value.Value;
    }
}
