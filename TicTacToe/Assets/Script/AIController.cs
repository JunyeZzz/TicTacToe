using UnityEngine;
using UnityEngine.UI;
using TMPro;

// AI 控制器，包含 AI 的最优走法算法（MiniMax）和胜负判断
public class AIController : MonoBehaviour
{
    // AI 首回合逻辑，优先选择角或中间位置
    public int GetFirstSmartMove(Button[] buttons, string aiSymbol, string playerSymbol)
    {
        int[] bestFirstMoves = { 0, 2, 4, 6, 8 };
        foreach (int move in bestFirstMoves)
        {
            if (buttons[move].interactable)
                return move;
        }
        return FindBestMove(buttons, aiSymbol, playerSymbol);
    }


    // 复杂AI，获取当前棋盘上 AI 的最优解
    public int FindBestMove(Button[] buttons, string aiSymbol, string playerSymbol)
    {
        int bestScore = int.MinValue;
        int bestMove = 0;

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].interactable)
            {
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = aiSymbol;
                buttons[i].interactable = false;

                int score = MiniMax(buttons, false, aiSymbol, playerSymbol);

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

    // MiniMax 算法核心逻辑（递归）
    int MiniMax(Button[] buttons, bool isMaximizing, string aiSymbol, string playerSymbol)
    {
        if (CheckWin(buttons, aiSymbol)) return 1;
        if (CheckWin(buttons, playerSymbol)) return -1;
        if (IsDraw(buttons)) return 0;

        int bestScore = isMaximizing ? int.MinValue : int.MaxValue;
        string currentSymbol = isMaximizing ? aiSymbol : playerSymbol;

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].interactable)
            {
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentSymbol;
                buttons[i].interactable = false;

                int score = MiniMax(buttons, !isMaximizing, aiSymbol, playerSymbol);

                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                buttons[i].interactable = true;

                bestScore = isMaximizing ? Mathf.Max(score, bestScore) : Mathf.Min(score, bestScore);
            }
        }
        return bestScore;
    }

    // 简单AI：随机选择一个可用的位置
    public int GetRandomMove(Button[] buttons)
    {
        System.Collections.Generic.List<int> availableMoves = new System.Collections.Generic.List<int>();

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].interactable)
            {
                availableMoves.Add(i);
            }
        }

        if (availableMoves.Count == 0)
            return 0; // 兜底处理

        int randomIndex = Random.Range(0, availableMoves.Count);
        return availableMoves[randomIndex];
    }

    // 检查胜利
    public bool CheckWin(Button[] buttons, string symbol)
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

    // 检查平局
    bool IsDraw(Button[] buttons)
    {
        foreach (var button in buttons)
            if (button.interactable) return false;
        return true;
    }
}
