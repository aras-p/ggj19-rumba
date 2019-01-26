public class Game
{
    static Game m_Instance;
    public static Game instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new Game();
            return m_Instance;
        }
    }

    public int score = 0;
}
