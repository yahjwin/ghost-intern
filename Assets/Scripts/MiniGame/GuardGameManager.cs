using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuardGameManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI countText;
    public TextMeshProUGUI warningText;
    public TextMeshProUGUI resultText;

    [Header("경비원 스프라이트")]
    public SpriteRenderer guardSpriteRenderer;
    public Sprite guardBackSprite;
    public Sprite guardFrontSprite;

    [Header("귀신 스프라이트")]
    public SpriteRenderer ghostSpriteRenderer;
    public Sprite ghostBackSprite;

    [Header("귀신 둥둥 효과")]
    public Transform ghostFloatTarget;
    public float floatSpeed = 3f;
    public float floatHeight = 0.15f;

    [Header("사운드")]
    public AudioSource sfxSource;
    public AudioClip invisibleSound;
    public AudioClip metalFailSound;
    public AudioClip witchLaughSound;

    private Vector3 ghostStartPosition;

    private int successCount = 0;
    private bool isGameEnded = false;

    private float[] reactionTimes =
    {
        2.0f,
        1.7f,
        1.4f,
        1.1f,
        0.8f
    };

    void Start()
    {
        if (warningText != null)
            warningText.gameObject.SetActive(false);

        if (resultText != null)
            resultText.gameObject.SetActive(false);

        SetGuardBack();
        SetGhostBack();

        if (ghostFloatTarget != null)
            ghostStartPosition = ghostFloatTarget.localPosition;

        UpdateCountText();

        StartCoroutine(GameLoop());
    }

    void Update()
    {
        FloatGhost();
    }

    void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    void FloatGhost()
    {
        if (ghostFloatTarget == null)
            return;

        Vector3 pos = ghostStartPosition;
        pos.y += Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        ghostFloatTarget.localPosition = pos;
    }

    IEnumerator GameLoop()
    {
        while (successCount < 5)
        {
            float randomLookBackTime = Random.Range(0.5f, 4f);
            yield return new WaitForSeconds(randomLookBackTime);

            SetGuardFront();

            yield return new WaitForSeconds(0.2f);

            if (warningText != null)
                warningText.gameObject.SetActive(true);

            float timer = reactionTimes[successCount];
            bool success = false;

            while (timer > 0)
            {
                timer -= Time.deltaTime;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    success = true;
                    break;
                }

                yield return null;
            }

            if (success)
            {
                PlaySFX(invisibleSound);

                if (warningText != null)
                    warningText.gameObject.SetActive(false);

                if (resultText != null)
                {
                    resultText.text = "투명화 성공!";
                    resultText.gameObject.SetActive(true);
                }

                successCount++;
                UpdateCountText();

                yield return new WaitForSeconds(0.8f);

                if (resultText != null)
                    resultText.gameObject.SetActive(false);

                SetGuardBack();
            }
            else
            {
                FailGame();
                yield break;
            }
        }

        WinGame();
    }

    void SetGuardBack()
    {
        if (guardSpriteRenderer != null && guardBackSprite != null)
            guardSpriteRenderer.sprite = guardBackSprite;
    }

    void SetGuardFront()
    {
        if (guardSpriteRenderer != null && guardFrontSprite != null)
            guardSpriteRenderer.sprite = guardFrontSprite;
    }

    void SetGhostBack()
    {
        if (ghostSpriteRenderer != null && ghostBackSprite != null)
            ghostSpriteRenderer.sprite = ghostBackSprite;
    }

    void UpdateCountText()
    {
        if (countText != null)
            countText.text = "회피 성공 " + successCount + " / 5";
    }

    void WinGame()
    {
        if (isGameEnded)
            return;

        isGameEnded = true;
        StartCoroutine(WinRoutine());
    }

    IEnumerator WinRoutine()
    {
        PlaySFX(witchLaughSound);

        int fearEnergy = PlayerPrefs.GetInt("FearEnergy", 0);
        fearEnergy += 100;

        PlayerPrefs.SetInt("FearEnergy", fearEnergy);
        PlayerPrefs.SetInt("GuardAlreadyCleared", 1);
        PlayerPrefs.SetInt("CanGoHome", 1);
        PlayerPrefs.Save();

        if (warningText != null)
            warningText.gameObject.SetActive(false);

        if (resultText != null)
        {
            resultText.text = "퇴근 성공!\n공포 에너지 +100";
            resultText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene("VillageScene");
    }

    void FailGame()
    {
        if (isGameEnded)
            return;

        isGameEnded = true;
        StartCoroutine(FailRoutine());
    }

    IEnumerator FailRoutine()
    {
        PlaySFX(metalFailSound);

        PlayerPrefs.SetInt("FearEnergy", 0);

        PlayerPrefs.SetInt("StudentAlreadyCleared", 0);
        PlayerPrefs.SetInt("WorkerAlreadyCleared", 0);
        PlayerPrefs.SetInt("GuardAlreadyCleared", 0);

        PlayerPrefs.SetInt("StudentNPCGone", 0);
        PlayerPrefs.SetInt("WorkerNPCGone", 0);

        PlayerPrefs.SetInt("CanGoHome", 0);
        PlayerPrefs.Save();

        SetGuardFront();

        if (warningText != null)
            warningText.gameObject.SetActive(false);

        if (resultText != null)
        {
            resultText.text = "들켰다!\n공포 에너지와 업무 진행도 초기화";
            resultText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene("VillageScene");
    }
}