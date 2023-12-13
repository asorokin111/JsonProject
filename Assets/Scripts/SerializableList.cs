using System.Collections.Generic;

[System.Serializable]
public class SerializableList<T>
{
    public List<T> list;
    public SerializableList()
    {
        list = new List<T>();
    }
}
