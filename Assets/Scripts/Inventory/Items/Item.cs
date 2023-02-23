using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemSO m_item;
    public bool IsEquipped;

    private void OnTriggerEnter(Collider other)
    {
        if (IsEquipped) return;
        Inventory inventory = other.GetComponent<Inventory>();
        if (inventory != null)
        {
            bool pickUp = inventory.TryPickUpItem(m_item);
            if (pickUp) Destroy(this.gameObject);
        }
    }
}
