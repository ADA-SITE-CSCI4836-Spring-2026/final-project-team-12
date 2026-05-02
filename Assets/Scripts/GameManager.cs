using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

        // Borrow time with B
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

    public void BorrowTime()
    {
        if (gameOver)
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
        if (!gameOver)
            currentTime += amount;
    }

    public void RemoveTime(float amount)
    {
        if (gameOver)
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
        if (!gameOver)
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
        if (gameOver)
            return;

        if (keysCollected >= keysNeeded)
            WinGame();
        else
            gameMessage = "Need more Time Keys!";
    }

    private void WinGame()
    {
        gameOver = true;
        playerWon = true;
        gameMessage = "You escaped Time Debt!";
    }

    private void LoseGame()
    {
        gameOver = true;
        playerWon = false;
        gameMessage = "Time Debt collected you!";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        style.normal.textColor = Color.white;

        GUI.Label(new Rect(20, 20, 500, 50), "TIME: " + Mathf.CeilToInt(currentTime), style);
        GUI.Label(new Rect(20, 60, 500, 50), "DEBT: " + debt + "/" + maxDebt, style);
        GUI.Label(new Rect(20, 100, 500, 50), "KEYS: " + keysCollected + "/" + keysNeeded, style);
        GUI.Label(new Rect(20, 140, 900, 50), "MESSAGE: " + gameMessage, style);
    }
}