public class GameManager
{
    private static GameManager instance;

    private GameManager()
    {

    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }

            return instance;
        }
    }

}
