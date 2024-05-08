using System.Collections.Generic;

[System.Serializable]
public class SaveData<T>
{
    public List<T> list;
    public SaveData(List<T> data)
    {
        this.list = data;
    }
}
