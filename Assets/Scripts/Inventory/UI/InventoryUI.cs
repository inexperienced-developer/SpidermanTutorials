using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private Inventory m_playerInventory;
    [SerializeField] private GameObject m_slotPrefab;
    [SerializeField] private GameObject m_inventoryPanel;
    [SerializeField] private Button m_inventoryToggle;
    private List<Button> m_Items = new List<Button>();

    // Start is called before the first frame update
    void Awake()
    {
        m_playerInventory = Inventory.Instance;
        //Fill up with empty slots
        for(int i = 0; i < m_playerInventory.NumSlots; i++)
        {
            GameObject slot = Instantiate(m_slotPrefab, m_inventoryPanel.transform);
            slot.GetComponentsInChildren<Image>(true)[1].gameObject.SetActive(false);
            slot.GetComponent<Button>().interactable = false;
            m_Items.Add(slot.GetComponent<Button>());
        }
        m_inventoryToggle.onClick.AddListener(() => m_inventoryPanel.SetActive(!m_inventoryPanel.activeSelf));
    }

    private void OnEnable()
    {
        m_playerInventory.OnItemPickup += OnItemPickup;
        m_playerInventory.OnItemEquip += OnItemEquip;
    }



    private void OnDisable()
    {
        m_playerInventory.OnItemPickup -= OnItemPickup;
        m_playerInventory.OnItemEquip -= OnItemEquip;
    }

    private void OnItemPickup(ItemSO obj, int index)
    {
        Button btn = m_Items[index].GetComponent<Button>();
        Image img = m_Items[index].GetComponentsInChildren<Image>(true)[1];
        btn.interactable = true;
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => m_playerInventory.EquipItem(index));
        img.sprite = obj.Sprite;
        img.gameObject.SetActive(true);
    }

    private void OnItemEquip(ItemSO obj1, ItemSO obj2, int index)
    {
        Button btn = m_Items[index].GetComponent<Button>();
        Image img = m_Items[index].GetComponentsInChildren<Image>(true)[1];
        if(obj2 == null)
        {
            img.gameObject.SetActive(false);
            btn.interactable = false;
            return;
        }
        img.sprite = obj2.Sprite;
        btn.interactable = true;
    }
}
