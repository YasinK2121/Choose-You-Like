using System.Collections.Generic;

[System.Serializable]
public class LevelData
{
    public List<TrueObject> trueObject = new List<TrueObject>();
    public List<FalseObject> falseObject = new List<FalseObject>();
    public bool toggle;
}

[System.Serializable]
public class TrueObject
{
    public Categories categories;
    public int gObjectCount;
    public bool toggle;
}

[System.Serializable]
public class FalseObject
{
    public Categories categories;
    public int gObjectCount;
    public bool toggle;
}

public enum Categories { Clothes,Food, FoodNotFruit,Technology,Vegetables};
