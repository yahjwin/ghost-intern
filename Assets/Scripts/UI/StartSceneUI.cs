using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneUI : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject loadingPanel;
    public GameObject howToPanel;

    public void StartGame()
    {
        StartCoroutine(LoadVillage());
    }

    IEnumerator LoadVillage()
    {
        mainMenu.SetActive(false);
        loadingPanel.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("VillageScene");
    }

    public void OpenHowTo()
    {
        mainMenu.SetActive(false);
        howToPanel.SetActive(true);
    }

    public void CloseHowTo()
    {
        howToPanel.SetActive(false);
        mainMenu.SetActive(true);
    }
}