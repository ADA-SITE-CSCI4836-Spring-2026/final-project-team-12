using UnityEngine;

public class GameUI : MonoBehaviour
{
    private GUIStyle titleStyle;
    private GUIStyle normalStyle;
    private GUIStyle messageStyle;
    private GUIStyle centerStyle;
    private GUIStyle smallStyle;
    private GUIStyle buttonStyle;

    private Texture2D whiteTexture;

    private void Start()
    {
        whiteTexture = Texture2D.whiteTexture;
    }

    private void SetupStyles()
    {
        if (titleStyle != null)
            return;

        titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 42;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        titleStyle.normal.textColor = Color.white;

        normalStyle = new GUIStyle(GUI.skin.label);
        normalStyle.fontSize = 20;
        normalStyle.fontStyle = FontStyle.Bold;
        normalStyle.normal.textColor = Color.white;

        messageStyle = new GUIStyle(GUI.skin.label);
        messageStyle.fontSize = 20;
        messageStyle.fontStyle = FontStyle.Bold;
        messageStyle.normal.textColor = Color.yellow;

        smallStyle = new GUIStyle(GUI.skin.label);
        smallStyle.fontSize = 18;
        smallStyle.normal.textColor = Color.white;
        smallStyle.wordWrap = true;

        centerStyle = new GUIStyle(GUI.skin.label);
        centerStyle.fontSize = 38;
        centerStyle.fontStyle = FontStyle.Bold;
        centerStyle.alignment = TextAnchor.MiddleCenter;
        centerStyle.normal.textColor = Color.white;

        buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 26;
        buttonStyle.fontStyle = FontStyle.Bold;
    }

    private void OnGUI()
    {
        if (GameManager.Instance == null)
            return;

        SetupStyles();

        GameManager gm = GameManager.Instance;

        if (gm.currentPhase == GameManager.GamePhase.MainMenu)
        {
            DrawMainMenu(gm);
            return;
        }

        if (gm.currentPhase == GameManager.GamePhase.Instructions)
        {
            DrawInstructions(gm);
            return;
        }

        if (gm.currentPhase == GameManager.GamePhase.Playing)
        {
            DrawTopLeftPanel(gm);
            DrawWarnings(gm);
            return;
        }

        if (gm.currentPhase == GameManager.GamePhase.Finished)
        {
            DrawTopLeftPanel(gm);
            DrawGameOverScreen(gm);
        }
    }

    private void DrawMainMenu(GameManager gm)
    {
        GUI.color = new Color(0f, 0f, 0f, 0.75f);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), whiteTexture);
        GUI.color = Color.white;

        GUI.Label(new Rect(0, Screen.height / 2 - 180, Screen.width, 80), "TIME DEBT", titleStyle);

        GUIStyle subtitle = new GUIStyle(centerStyle);
        subtitle.fontSize = 22;
        subtitle.normal.textColor = Color.yellow;

        GUI.Label(
            new Rect(0, Screen.height / 2 - 100, Screen.width, 60),
            "Borrow time now. Pay for it later.",
            subtitle
        );

        if (GUI.Button(new Rect(Screen.width / 2 - 120, Screen.height / 2, 240, 60), "START", buttonStyle))
        {
            gm.GoToInstructions();
        }

        GUIStyle bottom = new GUIStyle(smallStyle);
        bottom.alignment = TextAnchor.MiddleCenter;

        GUI.Label(
            new Rect(0, Screen.height - 70, Screen.width, 40),
            "A game about time, debt, risk and escape.",
            bottom
        );
    }

    private void DrawInstructions(GameManager gm)
    {
        GUI.color = new Color(0f, 0f, 0f, 0.78f);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), whiteTexture);
        GUI.color = Color.white;

        GUI.Label(new Rect(0, 60, Screen.width, 70), "HOW TO PLAY", titleStyle);

        GUIStyle instructionStyle = new GUIStyle(smallStyle);
        instructionStyle.fontSize = 22;
        instructionStyle.alignment = TextAnchor.UpperLeft;

        float boxWidth = 760;
        float boxHeight = 330;
        float boxX = Screen.width / 2 - boxWidth / 2;
        float boxY = 150;

        GUI.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);
        GUI.DrawTexture(new Rect(boxX, boxY, boxWidth, boxHeight), whiteTexture);
        GUI.color = Color.white;

        string instructions =
            "Goal:\n" +
            "Collect 3 Time Cores and reach the green portal.\n\n" +
            "Controls:\n" +
            "WASD / Arrow Keys = Move\n" +
            "Shift = Dash, but it costs 3 seconds\n" +
            "B = Borrow +10 seconds\n" +
            "R = Restart\n\n" +
            "Warning:\n" +
            "Every time you borrow time, your debt increases.\n" +
            "Higher debt activates traps, summons the Time Collector, and makes time drain faster.";

        GUI.Label(new Rect(boxX + 35, boxY + 25, boxWidth - 70, boxHeight - 50), instructions, instructionStyle);

        

        if (GUI.Button(new Rect(Screen.width - 280, Screen.height - 100, 240, 60), "PLAY", buttonStyle))
{
    gm.StartGame();
}
    }

    private void DrawTopLeftPanel(GameManager gm)
    {
        GUI.color = new Color(0f, 0f, 0f, 0.65f);
        GUI.DrawTexture(new Rect(15, 15, 310, 175), whiteTexture);
        GUI.color = Color.white;

        GUIStyle panelTitle = new GUIStyle(normalStyle);
        panelTitle.fontSize = 26;

        GUI.Label(new Rect(30, 25, 280, 35), "TIME DEBT", panelTitle);

        int timeValue = Mathf.CeilToInt(gm.currentTime);
        GUI.Label(new Rect(30, 65, 250, 30), "TIME: " + timeValue, normalStyle);

        float timePercent = Mathf.Clamp01(gm.currentTime / 60f);
        DrawBar(new Rect(30, 95, 260, 18), timePercent, GetTimeColor(gm.currentTime));

        GUI.Label(new Rect(30, 120, 250, 30), "DEBT: " + gm.debt + " / " + gm.maxDebt, normalStyle);

        float debtPercent = (float)gm.debt / gm.maxDebt;
        DrawBar(new Rect(30, 148, 260, 18), debtPercent, Color.red);

        GUI.Label(new Rect(30, 167, 280, 30), "TIME CORES: " + gm.keysCollected + " / " + gm.keysNeeded, normalStyle);
    }

    private void DrawWarnings(GameManager gm)
    {
        if (!string.IsNullOrEmpty(gm.gameMessage))
        {
            GUI.Label(new Rect(350, 25, 850, 40), gm.gameMessage, messageStyle);
        }

        if (gm.currentTime <= 10f)
        {
            GUI.color = new Color(1f, 0f, 0f, 0.25f);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), whiteTexture);
            GUI.color = Color.white;

            GUI.Label(new Rect(0, 80, Screen.width, 80), "TIME IS ALMOST GONE!", centerStyle);
        }

        if (gm.debt >= 4)
        {
            GUI.color = new Color(0.7f, 0f, 0f, 0.18f);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), whiteTexture);
            GUI.color = Color.white;
        }
    }

    private void DrawGameOverScreen(GameManager gm)
    {
        GUI.color = new Color(0f, 0f, 0f, 0.75f);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), whiteTexture);
        GUI.color = Color.white;

        string title = gm.playerWon ? "YOU ESCAPED TIME DEBT!" : "TIME DEBT COLLECTED YOU!";

        GUI.Label(new Rect(0, Screen.height / 2 - 100, Screen.width, 80), title, centerStyle);
        GUI.Label(new Rect(0, Screen.height / 2 - 20, Screen.width, 80), "Press R to Restart", centerStyle);
    }

    private void DrawBar(Rect rect, float percent, Color fillColor)
    {
        GUI.color = Color.black;
        GUI.DrawTexture(rect, whiteTexture);

        GUI.color = fillColor;
        GUI.DrawTexture(new Rect(rect.x + 3, rect.y + 3, (rect.width - 6) * percent, rect.height - 6), whiteTexture);

        GUI.color = Color.white;
    }

    private Color GetTimeColor(float time)
    {
        if (time <= 10f)
            return Color.red;

        if (time <= 20f)
            return Color.yellow;

        return Color.green;
    }
}