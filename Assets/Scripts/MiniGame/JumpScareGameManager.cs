using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class JumpScareGameManager : MonoBehaviour
{
    [Header("UI")]
    public RectTransform barArea;
    public RectTransform successZone;
    public RectTransform pointer;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI successText;
    public TextMeshProUGUI failText;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI effectText;

    public GameObject resultPanel;
    public TextMeshProUGUI resultText;

    [Header("학생 이미지")]
    public Image studentImage;
    public Sprite studentBackSprite;
    public Sprite studentSurprisedSprite;
    public float surprisedTime = 0.7f;

    [Header("게임 설정")]
    public float gameTime = 20f;
    public int targetSuccessCount = 10;
    public int maxFailCount = 3;

    [Header("게이지 설정")]
    public float pointerSpeed = 300f;
    public float speedIncreaseAmount = 35f;

    public float minZoneWidth = 50f;
    public float maxZoneWidth = 150f;
    public float zoneShrinkAmount = 8f;

    [Header("사운드")]
    public AudioSource sfxSource;
    public AudioClip booSound;        // 맞출 때 BOO
    public AudioClip crySound;        // 놓칠 때 울음
    public AudioClip failScreamSound; // 최종 실패 비명
    public AudioClip witchLaughSound; // 최종 성공 마녀 웃음

    private float currentTime;
    private int successCount = 0;
    private int failCount = 0;

    private float direction = 1f;
    private bool isGameEnded = false;

    private Coroutine studentFaceCoroutine;

    private string[] statusMessages =
    {
        "초등학생이 주변을 두리번거린다...",
        "초등학생이 휴대폰을 보고 있다...",
        "초등학생이 친구를 찾고 있다...",
        "초등학생이 방심하고 있다...",
        "지금이 놀래킬 기회일지도...?"
    };

    void Start()
    {
        currentTime = gameTime;

        if (resultPanel != null)
            resultPanel.SetActive(false);

        if (effectText != null)
            effectText.gameObject.SetActive(false);

        if (studentImage != null && studentBackSprite != null)
            studentImage.sprite = studentBackSprite;

        CreateRandomSuccessZone();
        UpdateUI();

        InvokeRepeating(nameof(ChangeStatusMessage), 0f, 2f);
    }

    void Update()
    {
        if (isGameEnded)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            FailGame();
            return;
        }

        MovePointer();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckTiming();
        }

        UpdateUI();
    }

    void MovePointer()
    {
        float halfBarWidth = barArea.rect.width / 2f;
        Vector2 pos = pointer.anchoredPosition;

        pos.x += direction * pointerSpeed * Time.deltaTime;

        if (pos.x >= halfBarWidth)
        {
            pos.x = halfBarWidth;
            direction = -1f;
        }
        else if (pos.x <= -halfBarWidth)
        {
            pos.x = -halfBarWidth;
            direction = 1f;
        }

        pointer.anchoredPosition = pos;
    }

    void CheckTiming()
    {
        float pointerX = pointer.anchoredPosition.x;
        float zoneX = successZone.anchoredPosition.x;
        float zoneHalfWidth = successZone.rect.width / 2f;

        bool isSuccess =
            pointerX >= zoneX - zoneHalfWidth &&
            pointerX <= zoneX + zoneHalfWidth;

        if (isSuccess)
        {
            successCount++;

            PlaySFX(booSound);

            ShowSurprisedStudent();

            pointerSpeed += speedIncreaseAmount;

            maxZoneWidth -= zoneShrinkAmount;
            if (maxZoneWidth < minZoneWidth)
                maxZoneWidth = minZoneWidth;

            ShowEffect("BOO!");
            SetStatus("깜짝 놀랐다!");

            if (successCount >= targetSuccessCount)
            {
                ClearGame();
                return;
            }
        }
        else
        {
            failCount++;

            PlaySFX(crySound);

            ShowEffect("들켰다!");
            SetStatus("눈치챘다...");

            if (failCount >= maxFailCount)
            {
                FailGame();
                return;
            }
        }

        CreateRandomSuccessZone();
        UpdateUI();
    }

    void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    void ShowSurprisedStudent()
    {
        if (studentImage == null || studentSurprisedSprite == null || studentBackSprite == null)
            return;

        if (studentFaceCoroutine != null)
            StopCoroutine(studentFaceCoroutine);

        studentFaceCoroutine = StartCoroutine(StudentFaceRoutine());
    }

    IEnumerator StudentFaceRoutine()
    {
        studentImage.sprite = studentSurprisedSprite;

        yield return new WaitForSeconds(surprisedTime);

        if (!isGameEnded)
            studentImage.sprite = studentBackSprite;
    }

    void CreateRandomSuccessZone()
    {
        float barWidth = barArea.rect.width;
        float randomWidth = Random.Range(minZoneWidth, maxZoneWidth);

        successZone.sizeDelta =
            new Vector2(randomWidth, successZone.sizeDelta.y);

        float halfBarWidth = barWidth / 2f;
        float halfZoneWidth = randomWidth / 2f;

        float randomX = Random.Range(
            -halfBarWidth + halfZoneWidth,
            halfBarWidth - halfZoneWidth
        );

        successZone.anchoredPosition =
            new Vector2(randomX, successZone.anchoredPosition.y);
    }

    void UpdateUI()
    {
        timerText.text = "TIME : " + Mathf.CeilToInt(currentTime);
        successText.text = "성공 : " + successCount + " / " + targetSuccessCount;
        failText.text = "실패 : " + failCount + " / " + maxFailCount;
    }

    void ChangeStatusMessage()
    {
        if (isGameEnded)
            return;

        int randomIndex = Random.Range(0, statusMessages.Length);
        SetStatus(statusMessages[randomIndex]);
    }

    void SetStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;
    }

    void ShowEffect(string message)
    {
        if (effectText == null)
            return;

        effectText.gameObject.SetActive(true);
        effectText.text = message;

        CancelInvoke(nameof(HideEffect));
        Invoke(nameof(HideEffect), 0.5f);
    }

    void HideEffect()
    {
        if (effectText != null)
            effectText.gameObject.SetActive(false);
    }

    void ClearGame()
    {
        isGameEnded = true;

        CancelInvoke(nameof(ChangeStatusMessage));
        CancelInvoke(nameof(HideEffect));

        int fearEnergy = PlayerPrefs.GetInt("FearEnergy", 0);
        fearEnergy += 100;

        PlayerPrefs.SetInt("FearEnergy", fearEnergy);
        PlayerPrefs.SetInt("StudentAlreadyCleared", 1);
        PlayerPrefs.SetInt("StudentNPCGone", 1);
        PlayerPrefs.Save();

        if (effectText != null)
            effectText.gameObject.SetActive(false);

        if (studentImage != null && studentSurprisedSprite != null)
            studentImage.sprite = studentSurprisedSprite;

        SetStatus("초등학생이 완전히 놀랐다!");

        ShowResult(true);
    }

    void FailGame()
    {
        isGameEnded = true;

        CancelInvoke(nameof(ChangeStatusMessage));
        CancelInvoke(nameof(HideEffect));

        PlayerPrefs.SetInt("StudentAlreadyCleared", 0);
        PlayerPrefs.SetInt("StudentNPCGone", 0);
        PlayerPrefs.Save();

        if (effectText != null)
            effectText.gameObject.SetActive(false);

        if (studentImage != null && studentBackSprite != null)
            studentImage.sprite = studentBackSprite;

        SetStatus("초등학생이 도망갔다...");

        ShowResult(false);
    }

    void ShowResult(bool isClear)
    {
        if (resultPanel != null)
            resultPanel.SetActive(true);

        if (resultText == null)
            return;

        if (isClear)
        {
            PlaySFX(witchLaughSound);
            resultText.text = "성공!\n공포 에너지 +100";
        }
        else
        {
            PlaySFX(failScreamSound);
            resultText.text = "실패...\n다른 학생을 찾아보자";
        }

        Invoke(nameof(ReturnToVillage), 2.0f);
    }

    void ReturnToVillage()
    {
        SceneManager.LoadScene("VillageScene");
    }
}