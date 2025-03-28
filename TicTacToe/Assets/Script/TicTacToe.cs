using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TicTacToeAI : MonoBehaviour
{
    public Button[] buttons;
    public TextMeshProUGUI gameStatusText;
    public GameObject choicePanel;

    private string playerSymbol;
    private string aiSymbol;
    private int movesCount = 0;
    private bool playerTurn;

    private void Start()
    {
        choicePanel.SetActive(true);
        foreach (var button in buttons)
            button.interactable = false;
    }

    public void PlayerChoosesSymbol(string symbol)
    {
        playerSymbol = symbol;
        aiSymbol = (symbol == "X") ? "O" : "X";
        playerTurn = (playerSymbol == "X");

        choicePanel.SetActive(false);

        foreach (var button in buttons)
        {
            button.interactable = true;
            button.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }

        movesCount = 0;

        if (!playerTurn)
        {
            gameStatusText.text = "AI's Turn";
            Invoke("AIFirstMove", 0.5f);
        }
        else
            gameStatusText.text = "Your Turn";

        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => PlayerMove(index));
        }
    }

    void PlayerMove(int index)
    {
        if (!playerTurn || !buttons[index].interactable) return;

        buttons[index].GetComponentInChildren<TextMeshProUGUI>().text = playerSymbol;
        buttons[index].interactable = false;
        movesCount++;

        if (CheckWin(playerSymbol))
        {
            gameStatusText.text = "You Win!";
            DisableButtons();
        }
        else if (movesCount == 9)
        {
            gameStatusText.text = "It's a Draw!";
        }
        else
        {
            playerTurn = false;
            gameStatusText.text = "AI's Turn";
            Invoke("AIMove", 0.5f);
        }
    }

    void AIFirstMove()
    {
        int[] bestFirstMoves = { 0, 2, 4, 6, 8 };
        int move = bestFirstMoves[Random.Range(0, bestFirstMoves.Length)];
        MakeAIMove(move);
    }

    void AIMove()
    {
        int move = FindBestMove();
        MakeAIMove(move);
    }

    void MakeAIMove(int move)
    {
        buttons[move].GetComponentInChildren<TextMeshProUGUI>().text = aiSymbol;
        buttons[move].interactable = false;
        movesCount++;

        if (CheckWin(aiSymbol))
        {
            gameStatusText.text = "AI Wins!";
            DisableButtons();
        }
        else if (movesCount == 9)
        {
            gameStatusText.text = "It's a Draw!";
        }
        else
        {
            playerTurn = true;
            gameStatusText.text = "Your Turn";
        }
    }

    int FindBestMove()
    {
        int bestScore = int.MinValue;
        int bestMove = 0;

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].interactable)
            {
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = aiSymbol;
                buttons[i].interactable = false;

                int score = MiniMax(false);

                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                buttons[i].interactable = true;

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = i;
                }
            }
        }
        return bestMove;
    }

    int MiniMax(bool isMaximizing)
    {
        if (CheckWin(aiSymbol)) return 1;
        if (CheckWin(playerSymbol)) return -1;
        if (IsDraw()) return 0;

        int bestScore = isMaximizing ? int.MinValue : int.MaxValue;
        string currentSymbol = isMaximizing ? aiSymbol : playerSymbol;

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].interactable)
            {
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentSymbol;
                buttons[i].interactable = false;

                int score = MiniMax(!isMaximizing);

                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                buttons[i].interactable = true;

                bestScore = isMaximizing ? Mathf.Max(score, bestScore) : Mathf.Min(score, bestScore);
            }
        }
        return bestScore;
    }

    bool CheckWin(string symbol)
    {
        int[,] winConditions = new int[,]
        {
            {0,1,2}, {3,4,5}, {6,7,8},
            {0,3,6}, {1,4,7}, {2,5,8},
            {0,4,8}, {2,4,6}
        };

        for (int i = 0; i < winConditions.GetLength(0); i++)
        {
            if (buttons[winConditions[i, 0]].GetComponentInChildren<TextMeshProUGUI>().text == symbol &&
                buttons[winConditions[i, 1]].GetComponentInChildren<TextMeshProUGUI>().text == symbol &&
                buttons[winConditions[i, 2]].GetComponentInChildren<TextMeshProUGUI>().text == symbol)
                return true;
        }
        return false;
    }

    bool IsDraw()
    {
        foreach (var button in buttons)
            if (button.interactable) return false;
        return true;
    }

    void DisableButtons()
    {
        foreach (var button in buttons)
            button.interactable = false;
    }
}
