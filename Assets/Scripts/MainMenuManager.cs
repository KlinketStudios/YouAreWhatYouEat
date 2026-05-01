using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayButtonClicked()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitButtongClicked()
    {
        Application.Quit();
    }
}
