using System.Collections.Generic;
using EC;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "EC/Item")]
public class BaseItemData : ScriptableObject
{
    // DO NOT INSTANTIATE
    // ALL Interactable scriptables inherit from this base one

    [Header("Base Item Configuration"), Space(5)]
    [Tooltip("The name of this item.")]
    [SerializeField] protected string itemName;
    [Tooltip("The prefab asset of this item.")]
    [SerializeField] protected GameObject prefab;
    [Tooltip("The type of interactable this is.")]
    [SerializeField] protected ItemType type;
    [Tooltip("Description text when examining this item.")]
    [SerializeField] protected List<string> descriptionTextList;


    public ItemType GetItemType()
    {
        return type;
    }

    public List<string> GetDescription()
    {
        return descriptionTextList;
    }

    public GameObject GetPrefab()
    {
        return prefab;
    }

    public string GetName()
    {
        return itemName;
    }
}
