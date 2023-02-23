using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[DefaultExecutionOrder(-99)]
public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    private void Awake()
    {
        Items = new ItemSO[NumSlots];
        Instance = this;
    }

    [SerializeField] private int m_numSlots;
    public int NumSlots => m_numSlots;
    public ItemSO[] Items { get; private set; }

    private ItemSO m_equippedItem;
    private GameObject m_equippedItemVisual;
    [SerializeField] private Transform m_equipPoint;

    public event Action<ItemSO, int> OnItemPickup;
    public event Action<ItemSO, ItemSO, int> OnItemEquip;

    public bool TryPickUpItem(ItemSO item)
    {
        for(int i = 0; i < Items.Length; i++)
        {
            if (Items[i] == null)
            {
                Items[i] = item;
                OnItemPickup?.Invoke(item, i);
                return true;
            }
        }
        return false;
    }

    public void EquipItem(int index)
    {
        //If this is null what the hell are we even doing?
        if (Items[index] == null) return;
        //Swap items
        if (m_equippedItem != null)
        {
            ItemSO equipping = Items[index];
            Items[index] = m_equippedItem;
            m_equippedItem = equipping;
            Destroy(m_equippedItemVisual);
        }
        //If no item just equip
        else
        {
            m_equippedItem = Items[index];
            Items[index] = null;
        }
        m_equippedItemVisual = Instantiate(m_equippedItem.Prefab, m_equipPoint);
        m_equippedItemVisual.transform.localPosition = Vector3.zero;
        m_equippedItemVisual.transform.localRotation = Quaternion.identity;
        m_equippedItemVisual.GetComponent<Item>().IsEquipped = true;
        OnItemEquip?.Invoke(m_equippedItem, Items[index], index);
    }
}
