using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Similar to store
public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    [SerializeField]
    private GameObject _inventoryItemPrefab;
    [SerializeField]
    private RectTransform _inventoryViewContent;
    private List<GameObject> _spawnedItems;
    [Header("Vertical offset variables")]

    [SerializeField]
    [Tooltip("By how much the Y of the next item is offset. Default is -200 (200 down).")]
    private int _offsetDelta = -200;

    [SerializeField]
    [Tooltip("The value the content size is increased by for every item in the list")]
    private int _sizeIncreasePerItem = 70;

    private int _verticalItemOffset = -50;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        _spawnedItems = new List<GameObject>();
    }

    private void Start()
    {
        DisplayInventoryUI();
    }

    private void InitializeItemSlot(InventoryItem item)
    {
        var spawned = Instantiate(_inventoryItemPrefab, Vector3.zero, Quaternion.identity, parent: _inventoryViewContent.transform);
        _spawnedItems.Add(spawned);
        var rectTransform = spawned.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, _verticalItemOffset);
        InitSpawnedObject(spawned, item);
        _verticalItemOffset += _offsetDelta;
    }

    private void InitSpawnedObject(GameObject spawned, InventoryItem item)
    {
        TextMeshProUGUI nameText, amountText;
        nameText = spawned.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        amountText = spawned.transform.Find("AmountText").GetComponent<TextMeshProUGUI>();
        nameText.text = item.name;
        amountText.text = "Amount: " + item.amount;
    }

    private void DisplayInventoryUI()
    {
        _inventoryViewContent.sizeDelta = new Vector2(_inventoryViewContent.sizeDelta.x,
            _inventoryViewContent.sizeDelta.y + _sizeIncreasePerItem * PersistentData.Instance.Inventory.list.Count);
        foreach (InventoryItem item in PersistentData.Instance.Inventory.list)
        {
            InitializeItemSlot(item);
        }
    }
}
