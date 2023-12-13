public class DataObject // For writing to json because of unity's limitations
{
    public string Name;
    public int HP;
    public long Gold;
    public int Str;
    public int Dex;
    public int Int;
    public int LeftoverPoints;
    public SerializableList<InventoryItem> Inventory;

    public DataObject(string name, int hp, long gold, int strength, int dexterity, int intelligence, int left, SerializableList<InventoryItem> inventory)
    {
        Name = name;
        HP = hp;
        Gold = gold;
        Str = strength;
        Dex = dexterity;
        Int = intelligence;
        LeftoverPoints = left;
        Inventory = inventory;
    }
}
