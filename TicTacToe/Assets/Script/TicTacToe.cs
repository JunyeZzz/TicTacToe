using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TicTacToeAI : MonoBehaviour
{
    public Button[] buttons; // 游戏棋盘上的按钮数组
    public TextMeshProUGUI gameStatusText; // 显示游戏状态的文本（显示回合和输赢结果）
    public GameObject choicePanel; // 玩家选择先后手（X或O）的面板
    public GameObject endGamePanel; // 游戏结束后的面板（重玩/退出）
    public GameObject pauseMenuPanel; // 游戏暂停菜单面板
    public AudioSource moveSFX; // 音效

    private string playerSymbol; // 玩家使用的符号（X或O）
    private string aiSymbol; // AI使用的符号（与玩家相反）
    private int movesCount = 0; // 当前已进行的回合数
    private bool playerTurn; // 是否为玩家回合

    // 游戏初始化
    private void Start()
    {
        choicePanel.SetActive(true);
        endGamePanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        foreach (var button in buttons)
            button.interactable = false;

        // 从PlayerPrefs中读取并设置音效音量
        float savedVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        moveSFX.volume = savedVolume;

        // 预加载音效以防止首次播放延迟
        moveSFX.Play();
        moveSFX.Stop();
    }


    private void Update()
    {   // 检测ESC键以显示或隐藏暂停菜单
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }
    
    // 玩家选择符号（X或O）
    public void PlayerChoosesSymbol(string symbol)
    {
        playerSymbol = symbol;
        aiSymbol = (symbol == "X") ? "O" : "X";
        playerTurn = (playerSymbol == "X");

        choicePanel.SetActive(false);
        // 激活棋盘上的按钮，准备开始游戏
        foreach (var button in buttons)
        {
            button.interactable = true;
            button.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }

        movesCount = 0;

        if (!playerTurn)
        {
            gameStatusText.text = "电脑回合";
            Invoke("AIFirstMove", 0.5f);
        }
        else
            gameStatusText.text = "你的回合";
        // 为按钮添加点击事件
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => PlayerMove(index));
        }
    }
    // 玩家回合
    void PlayerMove(int index)
    {
        if (!playerTurn || !buttons[index].interactable) return;

        buttons[index].GetComponentInChildren<TextMeshProUGUI>().text = playerSymbol;
        buttons[index].interactable = false;
        moveSFX.Play();
        movesCount++;

        if (CheckWin(playerSymbol))
        {
            gameStatusText.text = "你赢了！";
            Invoke("EndGame", 1f);
        }
        else if (movesCount == 9)
        {
            gameStatusText.text = "平局";
            Invoke("EndGame", 1f);
        }
        else
        {
            playerTurn = false;
            gameStatusText.text = "电脑回合";
            Invoke("AIMove", 0.5f);
        }
    }
    // AI首回合，随机选择一个优先位置（优化性能）
    void AIFirstMove()
    {
        int[] bestFirstMoves = { 0, 2, 4, 6, 8 };
        int move = bestFirstMoves[Random.Range(0, bestFirstMoves.Length)];
        MakeAIMove(move);
    }
    //AI回合
    void AIMove()
    {
        int move = FindBestMove();
        MakeAIMove(move);
    }
    // 执行AI的回合
    void MakeAIMove(int move)
    {
        buttons[move].GetComponentInChildren<TextMeshProUGUI>().text = aiSymbol;
        buttons[move].interactable = false;
        moveSFX.Play();
        movesCount++;

        if (CheckWin(aiSymbol))
        {
            gameStatusText.text = "电脑胜利";
            Invoke("EndGame", 1f);
        }
        else if (movesCount == 9)
        {
            gameStatusText.text = "平局";
            Invoke("EndGame", 1f);
        }
        else
        {
            playerTurn = true;
            gameStatusText.text = "你的回合";
        }
    }
    
    // 游戏结束处理，显示结束面板
    void EndGame()
    {
        DisableButtons();
        endGamePanel.SetActive(true);
    }
    
    // 再来一局
    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 返回主菜单
    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // 切换暂停菜单的显示状态
    public void TogglePauseMenu()
    {
        pauseMenuPanel.SetActive(!pauseMenuPanel.activeSelf);
    }

    // 从暂停菜单继续游戏
    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
    }

    // 从暂停菜单重新开始游戏
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // AI寻找最优解
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

    // MiniMax算法核心逻辑
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

    // 检测是否胜利
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

    // 检测是否平局
    bool IsDraw()
    {
        foreach (var button in buttons)
            if (button.interactable) return false;
        return true;
    }

    // 禁用棋盘上的所有按钮
    void DisableButtons()
    {
        foreach (var button in buttons)
            button.interactable = false;
    }
}
