using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 主菜单脚本，包含开始游戏、设置音效音量和退出游戏功能
public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel; // 设置面板对象
    public GameObject aiDifficultyPanel; // 难度选择面板
    public Slider sfxVolumeSlider; // 控制音效音量的滑动条

    // 初始化主菜单时，加载并应用已保存的音量设置
    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("SFXVolume", 1f); // 从玩家偏好设置读取音量，默认值为1
        sfxVolumeSlider.value = savedVolume;                       // 设置滑动条到已保存的音量值
        AudioListener.volume = savedVolume;                        // 应用音量设置
    }

    // 人机对战按钮点击，打开难度选择面板
    public void OpenAIDifficultyPanel()
    {
        aiDifficultyPanel.SetActive(true);
    }

    // 选择简单AI，保存设置并进入游戏
    public void PlayEasyMode()
    {
        PlayerPrefs.SetInt("AI_EASY", 1);
        SceneManager.LoadScene("Game");
    }

    // 选择困难AI，保存设置并进入游戏
    public void PlayHardMode()
    {
        PlayerPrefs.SetInt("AI_EASY", 0);
        SceneManager.LoadScene("Game");
    }

    // "设置" 按钮调用，打开设置面板
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    // "关闭设置" 按钮调用，关闭设置面板
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    // "退出游戏" 按钮调用，退出游戏应用
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game!"); // 在Unity编辑器中测试退出功能时打印信息
    }

    // 音效音量滑动条变化时调用，更新并保存音量设置
    public void SetSFXVolume(float volume)
    {
        AudioListener.volume = volume;             // 实时更新音量
        PlayerPrefs.SetFloat("SFXVolume", volume); // 保存音量设置
    }
}