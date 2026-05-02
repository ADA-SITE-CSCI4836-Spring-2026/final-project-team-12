using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GamePhase
    {
        MainMenu,
        Instructions,
        Playing,
        Finished
    }

    [Header("Game Phase")]
    public GamePhase currentPhase = GamePhase.MainMenu;

    [Header("Game Values")]
    public float currentTime = 30f;
    public int debt = 0;
    public int maxDebt = 5;
    public int keysCollected = 0;
    public int keysNeeded = 3;

    [Header("Debt Effects")]
    public GameObject timeCollector;
    public GameObject[] debtTraps;

    [Header("Game State")]
    public bool gameOver = false;
    public bool playerWon = false;
    public string gameMessage = "";

    private float drainMultiplier = 1f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentPhase = GamePhase.MainMenu;

        if (timeCollector != null)
            timeCollector.SetActive(false);

        foreach (GameObject trap in debtTraps)
        {
            if (trap != null)
                trap.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            RestartGame();

        if (currentPhase != GamePhase.Playing)
            return;

        if (Input.GetKeyDown(KeyCode.B))
            BorrowTime();

        if (gameOver)
            return;

        currentTime -= Time.deltaTime * drainMultiplier;

        if (currentTime <= 0)
        {
            currentTime = 0;
            LoseGame();
        }
    }

    public bool IsGamePlaying()
    {
        return currentPhase == GamePhase.Playing && !gameOver;
    }

    public void GoToInstructions()
    {
        currentPhase = GamePhase.Instructions;
        gameMessage = "";
    }

    public void StartGame()
    {
        currentPhase = GamePhase.Playing;
        gameMessage = "Collect 3 Time Cores and escape!";
    }

    public void BorrowTime()
    {
        if (!IsGamePlaying())
            return;

        if (debt >= maxDebt)
        {
            gameMessage = "MAX DEBT!";
            return;
        }

        currentTime += 10f;
        debt++;

        gameMessage = "Borrowed +10 seconds. Debt increased.";

        ApplyDebtEffects();
    }

    public void AddTime(float amount)
    {
        if (IsGamePlaying())
            currentTime += amount;
    }

    public void RemoveTime(float amount)
    {
        if (!IsGamePlaying())
            return;

        currentTime -= amount;

        if (currentTime <= 0)
        {
            currentTime = 0;
            LoseGame();
        }
    }

    public void AddKey()
    {
        if (IsGamePlaying())
            keysCollected++;
    }

    private void ApplyDebtEffects()
    {
        if (debt >= 2)
        {
            foreach (GameObject trap in debtTraps)
            {
                if (trap != null)
                    trap.SetActive(true);
            }

            gameMessage = "Debt Level 2: Extra traps activated!";
        }

        if (debt >= 3 && timeCollector != null)
        {
            timeCollector.SetActive(true);
            gameMessage = "Debt Level 3: Time Collector is hunting you!";
        }

        if (debt >= 4)
        {
            drainMultiplier = 1.5f;
            gameMessage = "Debt Level 4: Time drains faster!";
        }

        if (debt >= 5)
        {
            drainMultiplier = 2f;
            gameMessage = "Debt Level 5: Final debt crisis!";
        }

        if (timeCollector != null)
        {
            TimeCollector collector = timeCollector.GetComponent<TimeCollector>();

            if (collector != null)
                collector.SetSpeedByDebt(debt);
        }
    }

    public void TryWin()
    {
        if (!IsGamePlaying())
            return;

        if (keysCollected >= keysNeeded)
            WinGame();
        else
            gameMessage = "Need more Time Cores!";
    }

    private void WinGame()
    {
        gameOver = true;
        playerWon = true;
        currentPhase = GamePhase.Finished;
        gameMessage = "You escaped Time Debt!";
    }

    private void LoseGame()
    {
        gameOver = true;
        playerWon = false;
        currentPhase = GamePhase.Finished;
        gameMessage = "Time Debt collected you!";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}