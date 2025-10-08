using System.Collections.Generic;
using EC;
using UnityEngine;

public class BaseItem : MonoBehaviour, IItem
{
    /*
    Single Responsibility: This script is responsible for storing base data of the item and actions you can do with them
    - Store BaseItemData scriptable
    - Examine Action
    - Use Action 
    */

    [Tooltip("The scriptable asset for this item. Assign in Editor.")]
    [SerializeField] protected BaseItemData m_BaseItemData;
    [Tooltip("Enum for the type of item this is.")]
    [SerializeField] protected ItemType m_Type;
    [Tooltip("The description for this item when being examined")]
    [SerializeField] protected List<string> m_DescriptionTextList;

    private void Awake()
    {
        m_Type = m_BaseItemData.GetItemType();
        m_DescriptionTextList = m_BaseItemData.GetDescription();
    }

    public void Use()
    {
        Debug.Log($"Using {this.gameObject.name}");
    }   

    public void Examine()
    {
        Debug.Log($"Examining {this.gameObject.name}");
    }

}
