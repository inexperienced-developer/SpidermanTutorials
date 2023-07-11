using UnityEngine;
using UnityEngine.Events;

public class InventoryEventsSO : ScriptableObject
{
    public event UnityAction<ItemSO, int> ItemPickup;
    public event UnityAction<ItemSO, ItemSO, int> ItemEquip;

    public void OnItemPickup(ItemSO item, int index)
    {
        if(ItemPickup != null) ItemPickup.Invoke(item, index);
    }

    public void OnItemEquip(ItemSO item1,  ItemSO item2, int index) => ItemEquip?.Invoke(item1, item2, index);
}
