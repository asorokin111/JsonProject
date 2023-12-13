// StoreItem, but without the price
[System.Serializable]
public class InventoryItem
{
    public string name = string.Empty;
    public int amount = 0;

    public InventoryItem(string name, int amount)
    {
        this.name = name;
        this.amount = amount;
    }
    public InventoryItem(StoreItem storeItem)
    {
        name = storeItem.name;
        amount = storeItem.amount;
    }
}
