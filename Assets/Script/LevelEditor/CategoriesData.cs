using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CategoriesEditor")]
[SerializeField]
public class CategoriesData : ScriptableObject
{
    [Header("CATEGORIES")]
    public List<Categorie> categorie;
}

[System.Serializable]
public class Categorie
{
    [Header("CATEGORIE NAME")]
    public Categories WhichCategorie;
    [Header("CATEGORIE OBJECTS")]
    public List<CategorieObjects> categorieObjects;
}

[System.Serializable]
public class CategorieObjects
{
    public GameObject gObject;
}
