using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    #region Event Declarations
    public delegate void BoughtAction();
    public static event BoughtAction OnItemBought;
    #endregion

    #region Variables
    public static Store Instance;

    [Tooltip("Items in the store")]
    public SerializableList<StoreItem> items;

    [Tooltip("Items to be added to the store when nothing else is there")]
    public List<StoreItem> defaultItems;

    [Tooltip("GameObjects in the UI")]
    private List<GameObject> _spawnedItems;

    [Tooltip("The prefab to be spawned in the store menu")]
    [SerializeField]
    private GameObject _itemPrefab;

    [Tooltip("The RectTransform component of the main ScrollView's Content")]
    [SerializeField]
    private RectTransform _scrollViewContent;

    [Header("Vertical offset variables")]

    [SerializeField]
    [Tooltip("By how much the Y of the next item is offset. Default is -200 (200 down).")]
    private int _offsetDelta = -200;

    [SerializeField]
    [Tooltip("The value the content size is increased by for every item in the list")]
    private int _sizeIncreasePerItem = 200;

    private int _verticalItemOffset = -100;
    private int _initialVerticalOffset = -100;

    private const string _jsonFilename = "store_items.json";
    private const string _defaultItemsFilename = "default_store_items.json";
    private string _jsonPath;
    private string _defaultItemsPath;
    #endregion

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
        _jsonPath = Application.persistentDataPath + "/" + _jsonFilename;
        _defaultItemsPath = Application.dataPath + "/" + _defaultItemsFilename;
        _spawnedItems = new List<GameObject>();
        _initialVerticalOffset = _verticalItemOffset;
    }

    private void Start()
    {
        GetItemsFromJson();
        //ShowItemsInUI();
    }

    private void OnEnable()
    {
        ShopInteraction.OnStoreOpen += ShowItemsInUI;
    }

    #region Json Functions
    private void GetItemsFromJson()
    {
        if (FileExistsAndNotEmpty(_jsonPath))
        {
            string jsonData = File.ReadAllText(_jsonPath);
            items = JsonUtility.FromJson<SerializableList<StoreItem>>(jsonData);
        }
        else
        {
            InitDefaultItems();
            SaveItemsToJson();
        }
        _scrollViewContent.sizeDelta = new Vector2(_scrollViewContent.sizeDelta.x,
            _scrollViewContent.sizeDelta.y + _sizeIncreasePerItem * items.list.Count(item => item.amount > 0)); // TODO: fix this
    }

    private bool FileExistsAndNotEmpty(string path)
    {
        if (!File.Exists(path)) return false;
        var info = new FileInfo(path);
        string dummyJson = JsonUtility.ToJson(new SerializableList<StoreItem>());
        return info.Length > dummyJson.Length;
    }
    
    private void SaveItemsToJson()
    {
        string jsonData = JsonUtility.ToJson(items);
        File.WriteAllText(_jsonPath, jsonData);
    }

    private void InitDefaultItems()
    {
        items = new SerializableList<StoreItem>();
        if (FileExistsAndNotEmpty(_defaultItemsPath))
        {
            string data = File.ReadAllText(_defaultItemsPath);
            items = JsonUtility.FromJson<SerializableList<StoreItem>>(data);
            return;
        }
        else
        {
            foreach (StoreItem item in defaultItems)
            {
                AddItemNoDuplicates(items.list, item);
            }
            var newDefaultList = new SerializableList<StoreItem>();
            newDefaultList.list = defaultItems;
            File.WriteAllText(_defaultItemsPath, JsonUtility.ToJson(newDefaultList));
        }
    }
    #endregion

    #region UI-related functions

    public void ShowItemsInUI()
    {
        var itemList = items.list;
        foreach (StoreItem item in itemList)
        {
            if (item.amount <= 0) continue;
            InitializeItemSlot(item);
        }
    }

    private void InitializeItemSlot(StoreItem item)
    {
        var spawned = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity, parent: _scrollViewContent.transform);
        _spawnedItems.Add(spawned);
        var rectTransform = spawned.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, _verticalItemOffset);

        InitSpawnedObject(spawned, item);

        _verticalItemOffset += _offsetDelta;
    }

    private void InitSpawnedObject(GameObject spawned, StoreItem item)
    {
        Button buyButton;
        buyButton = spawned.transform.Find("BuyButton").GetComponent<Button>();
        buyButton.interactable = PersistentData.Instance.Gold >= item.price;
        buyButton.onClick.AddListener(delegate { BuyButtonClicked(item); });

        TextMeshProUGUI nameText, priceText, amountText;
        nameText = spawned.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        priceText = spawned.transform.Find("PriceText").GetComponent<TextMeshProUGUI>();
        amountText = spawned.transform.Find("AmountText").GetComponent<TextMeshProUGUI>();

        nameText.text = item.name;
        priceText.text = "Price: " + item.price;
        amountText.text = "Amount: " + item.amount;
    }

    public void DestroySpawnedItems()
    {
        foreach (GameObject item in _spawnedItems)
        {
            Destroy(item);
        }
        _verticalItemOffset = _initialVerticalOffset;
    }
    #endregion

    #region Inventory manipulation functions
    public void AddInventoryItem(StoreItem item)
    {
        InventoryItem invItem = (InventoryItem)item;
        invItem.amount = 1;
        var inventory = PersistentData.Instance.Inventory.list;
        AddItemNoDuplicates(inventory, invItem);
    }

    public void AddStoreItem(StoreItem item)
    {
        AddItemNoDuplicates(items.list, item);
    }

    private void AddItemNoDuplicates(List<InventoryItem> list, InventoryItem itemToAdd)
    {
        bool foundDuplicate = false;
        foreach (var existingItem in list)
        {
            if (existingItem.name == itemToAdd.name)
            {
                foundDuplicate = true;
                ++existingItem.amount;
                break;
            }
        }
        if (!foundDuplicate)
        {
            list.Add(itemToAdd);
        }
    }

    private void AddItemNoDuplicates(List<StoreItem> list, StoreItem itemToAdd)
    {
        bool foundDuplicate = false;
        foreach (var existingItem in list)
        {
            if (existingItem.name == itemToAdd.name)
            {
                foundDuplicate = true;
                existingItem.amount += itemToAdd.amount;
                break;
            }
        }
        if (!foundDuplicate)
        {
            list.Add(itemToAdd);
        }
    }
    #endregion

    public void BuyButtonClicked(StoreItem itemToBuy)
    {
        if (PersistentData.Instance.Gold < itemToBuy.price) return;
        --itemToBuy.amount;
        PersistentData.Instance.Gold -= itemToBuy.price;
        AddInventoryItem(itemToBuy);
        PersistentData.Instance.WriteToJson();
        OnItemBought?.Invoke();
        DestroySpawnedItems(); // FIXME: Not optimal
        ShowItemsInUI();
        SaveItemsToJson();
    }
}
