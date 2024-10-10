

[System.Serializable]

public class GameData
{
    public int money;
    
    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load

    public GameData() 
    {
        this.money = 0;
    }
}
