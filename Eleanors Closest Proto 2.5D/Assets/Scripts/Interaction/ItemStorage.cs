using UnityEngine;

public class ItemStorage : MonoBehaviour
{
    /*
    Single Responsibility: This script is responsible for tracking the item an interactable has and displauying in-scene
    */

    [Header("Item Configuration"), Space(5)]
    [Tooltip("A ref to a BaseItem object this interactable is storing.Assin in Inspector.")]
    [SerializeField] private BaseItemData m_StoredItemData = null;
    [Tooltip("A transform where item objects will be displayed in-game when storing an item on this interactable. Assign in Inspector or at run time.")]
    [SerializeField] private Transform m_ItemDisplayPoint;
    private GameObject m_InstancedVisual;

    private void Start()
    {
        // Instantiate thg item under the display point pos
        TryInstantiateStoredItem();
    }

    private void TryInstantiateStoredItem()
    {
        // Find the item this is storing and display it display point
        if (m_StoredItemData == null) return;

        if (m_ItemDisplayPoint == null)
        {
            Debug.LogError("Item Display Point not assigned!");
            return;
        }

        // If we already instantiated a visual, clean it up (optional, depends on your usage)
        if (m_InstancedVisual != null)
        {
            Destroy(m_InstancedVisual);
            m_InstancedVisual = null;
        }

        // Obtain the prefab/visual GameObject from the item data
        GameObject prefab = m_StoredItemData.GetPrefab();
        if (prefab == null)
        {
            Debug.LogWarning($"[{nameof(ItemStorage)}] Stored item {m_StoredItemData.name} has no prefab assigned.");
            return;
        }

        // Approach A: use Instantiate with parent, which keeps local transform
        m_InstancedVisual = Instantiate(prefab, m_ItemDisplayPoint.position, m_ItemDisplayPoint.rotation, m_ItemDisplayPoint);

        Debug.Log($"Item Display Complete on {this.gameObject.name}");
    }

    public void SetStoredItemData(BaseItemData newItem)
    {
        m_StoredItemData = newItem;
        TryInstantiateStoredItem();
    }
    
    public void ClearStoredItemData()
    {
        m_StoredItemData = null;
        if (m_InstancedVisual != null)
        {
            Destroy(m_InstancedVisual);
            m_InstancedVisual = null;
        }
    }

    public BaseItemData GetStoredItemData()
    {
        return m_StoredItemData;
    }
}
