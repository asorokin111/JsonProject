[System.Serializable]
public class StoreItem
{
    public string name = string.Empty;
    public int price = 0;
    public int amount = 0;
    public static implicit operator InventoryItem(StoreItem i) => new InventoryItem(i.name, i.amount);
    public StoreItem(string name, int price, int amount)
    {
        this.name = name;
        this.price = price;
        this.amount = amount;
    }
}
