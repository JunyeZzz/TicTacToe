using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// 游戏控制器脚本，负责游戏逻辑、玩家操作和UI界面更新
public class GameController : MonoBehaviour
{
    public Button[] buttons;                   // 游戏棋盘上的按钮数组
    public TextMeshProUGUI gameStatusText;     // 显示游戏状态的文本（回合信息和游戏结果）
    public GameObject choicePanel;             // 玩家选择先后手（X或O）的面板
    public GameObject endGamePanel;            // 游戏结束后的面板（重玩/退出）
    public AudioSource moveSFX;                // 移动音效

    private string playerSymbol;               // 玩家使用的符号（X或O）
    private string aiSymbol;                   // AI使用的符号（与玩家相反）
    private int movesCount = 0;                // 当前已进行的回合数
    private bool playerTurn;                   // 是否为玩家回合

    private AIController aiController;         // 引用AI控制器脚本

    // 游戏初始化
    private void Start()
    {
        aiController = GetComponent<AIController>();
        choicePanel.SetActive(true);
        endGamePanel.SetActive(false);
        foreach (var button in buttons)
            button.interactable = false;

        float savedVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        moveSFX.volume = savedVolume;

        moveSFX.Play();
        moveSFX.Stop();
    }

    // 玩家选择符号（X或O）
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
        gameStatusText.text = playerTurn ? "你的回合" : "电脑回合";

        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => PlayerMove(index));
        }

        if (!playerTurn)
            Invoke("AIFirstTurn", 0.5f);

    }

    // 玩家回合
    void PlayerMove(int index)
    {
        if (!playerTurn || !buttons[index].interactable) return;

        buttons[index].GetComponentInChildren<TextMeshProUGUI>().text = playerSymbol;
        buttons[index].interactable = false;
        moveSFX.Play();
        movesCount++;

        if (aiController.CheckWin(buttons, playerSymbol))
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

    // AI回合
    void AIMove()
    {
        int move = aiController.FindBestMove(buttons, aiSymbol, playerSymbol);
        buttons[move].GetComponentInChildren<TextMeshProUGUI>().text = aiSymbol;
        buttons[move].interactable = false;
        moveSFX.Play();
        movesCount++;

        if (aiController.CheckWin(buttons, aiSymbol))
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

    // AI 首回合，调用 AI 控制器获取快速开局落点
    void AIFirstTurn()
    {
        int move = aiController.GetFirstSmartMove(buttons, aiSymbol, playerSymbol);
        AIMoveAt(move);
    }
    
    // 执行指定位置的AI移动（用于首回合）
    void AIMoveAt(int index)
    {
        buttons[index].GetComponentInChildren<TextMeshProUGUI>().text = aiSymbol;
        buttons[index].interactable = false;
        moveSFX.Play();
        movesCount++;

        if (aiController.CheckWin(buttons, aiSymbol))
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
        foreach (var button in buttons)
            button.interactable = false;
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
}
