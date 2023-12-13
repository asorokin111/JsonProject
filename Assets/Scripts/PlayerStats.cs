public class PlayerStats
{
    public int Str = 0;
    public int Dex = 0;
    public int Int = 0;
    public int LeftoverPoints = 0;

    public PlayerStats(int strength, int dexterity, int intelligence, int leftoverPoints)
    {
        Str = strength;
        Dex = dexterity;
        Int = intelligence;
        LeftoverPoints = leftoverPoints;
    }

    public PlayerStats() : this(0, 0, 0, 15)
    { }
}
