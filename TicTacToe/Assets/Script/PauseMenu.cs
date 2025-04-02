using UnityEngine;
using UnityEngine.SceneManagement;

// 暂停菜单控制器，负责暂停、继续、重启或退出游戏
public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuPanel; // 暂停菜单面板

    void Update()
    {
        // 检测 ESC 键，切换暂停菜单显示
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    // 切换暂停菜单
    public void TogglePauseMenu()
    {
        pauseMenuPanel.SetActive(!pauseMenuPanel.activeSelf);
    }

    // 恢复游戏
    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
    }

    // 重新开始游戏
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 返回主菜单
    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}