using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI endingText;
    public GameObject restartButton;

    void Start()
    {
        restartButton.SetActive(false);
        StartCoroutine(EndingRoutine());
    }

    IEnumerator EndingRoutine()
    {
        int fearEnergy = PlayerPrefs.GetInt("FearEnergy", 300);

        endingText.text =
            "오늘도 무사히 업무 완료!\n\n" +
            "학생을 놀래키고\n" +
            "직장인을 정전시키고\n" +
            "경비원을 따돌렸다.\n\n" +
            "획득한 공포 에너지\n" +
            fearEnergy + " / 300";

        yield return new WaitForSeconds(3f);

        endingText.text =
            "내일도 열심히 일하자...";

        yield return new WaitForSeconds(3f);

        endingText.text =
            "제작\n" +
            "조예진\n\n" +
            "성신여대";

        yield return new WaitForSeconds(2f);

        restartButton.SetActive(true);
    }

    public void RestartGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        SceneManager.LoadScene("StartScene");
    }
}