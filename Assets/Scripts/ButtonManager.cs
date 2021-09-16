using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : SingletonBaseClass<ButtonManager>
{
    private bool isFaster;
    
    /// <summary>
    /// Faster the game
    /// </summary>
    /// <param name="fasterText">Faster text</param>
    public void FasterGame(Text fasterText)
    {
        isFaster = !isFaster;
        fasterText.text = isFaster ? "2x" : "1x";
        Time.timeScale = isFaster ? 2 : 1;
    }
    
    /// <summary>
    /// Pause the game
    /// </summary>
    /// <param name="pausedPanel">Paused panel</param>
    public void PauseGame(GameObject pausedPanel)
    {
        Time.timeScale = 0f;
        pausedPanel.SetActive(true);
    }

    /// <summary>
    /// Resume the game
    /// </summary>
    /// <param name="pausedPanel">Paused panel</param>
    public void ResumeGame(GameObject pausedPanel)
    {
        Time.timeScale = 1f;
        pausedPanel.SetActive(false);
    }
    
    /// <summary>
    /// Restart game when is over
    /// </summary>
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}