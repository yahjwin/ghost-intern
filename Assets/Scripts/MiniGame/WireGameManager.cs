using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WireGameManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI phaseText;
    public TextMeshProUGUI sequenceText;
    public TextMeshProUGUI currentInputText;

    public GameObject resultPanel;
    public TextMeshProUGUI resultText;

    [Header("암전 UI")]
    public Image blackoutImage;
    public TextMeshProUGUI blackoutText;
    public float blackoutFadeTime = 1.2f;
    public float blackoutWaitTime = 2.5f;

    [Header("패널 조각 날리기")]
    public GameObject wireProjectilePrefab;
    public Transform firePoint;

    public Transform redTarget;
    public Transform blueTarget;
    public Transform yellowTarget;
    public Transform greenTarget;
    public Transform blackTarget;
    public Transform purpleTarget;

    [Header("Sparkle 효과")]
    public GameObject sparklePrefab;

    [Header("패널 조각 색 Material")]
    public Material redMaterial;
    public Material blueMaterial;
    public Material yellowMaterial;
    public Material greenMaterial;
    public Material blackMaterial;
    public Material purpleMaterial;

    [Header("게임 설정")]
    public float memorizeTime = 3f;
    public float inputTime = 15f;
    public int totalRound = 3;

    [Header("연출 설정")]
    public float flyTime = 0.7f;
    public float arcHeight = 1.2f;
    public float roundDelay = 1.5f;
    public float clearBeforeBlackoutDelay = 0.5f;

    [Header("박힘 연출")]
    public Vector3 stuckScale = new Vector3(0.25f, 0.25f, 0.8f);
    public Vector3 stuckRotation = new Vector3(90f, 0f, 0f);
    public float stickForwardOffset = 0.08f;

    [Header("사운드")]
    public AudioSource sfxSource;
    public AudioClip electricSound;
    public AudioClip failScreamSound;
    public AudioClip witchLaughSound;

    private int currentRound = 1;
    private float currentTime;

    private bool isInputPhase = false;
    private bool isGameEnded = false;
    private bool isFailing = false;

    private List<string> wireNames = new List<string>
    {
        "빨강", "파랑", "노랑", "초록", "검정", "보라"
    };

    private List<string> answerSequence = new List<string>();
    private List<string> playerInput = new List<string>();

    void Start()
    {
        if (resultPanel != null)
            resultPanel.SetActive(false);

        if (blackoutImage != null)
        {
            blackoutImage.gameObject.SetActive(true);

            Color c = blackoutImage.color;
            c.a = 0f;
            blackoutImage.color = c;

            blackoutImage.gameObject.SetActive(false);
        }

        if (blackoutText != null)
            blackoutText.gameObject.SetActive(false);

        StartRound();
    }

    void Update()
    {
        if (isGameEnded || isFailing)
            return;

        if (isInputPhase)
        {
            currentTime -= Time.deltaTime;

            if (timerText != null)
                timerText.text = "TIME : " + Mathf.CeilToInt(currentTime);

            if (currentTime <= 0)
            {
                currentTime = 0;
                StartCoroutine(FailRoutine());
                return;
            }

            CheckKeyboardInput();
        }
    }

    void StartRound()
    {
        if (isGameEnded)
            return;

        isInputPhase = false;
        playerInput.Clear();

        int sequenceLength = currentRound + 3;
        answerSequence = GenerateRandomSequence(sequenceLength);

        if (roundText != null)
            roundText.text = "ROUND " + currentRound + " / " + totalRound;

        if (timerText != null)
            timerText.text = "TIME : " + inputTime;

        if (phaseText != null)
            phaseText.text = "정답 순서를 기억하세요!";

        if (sequenceText != null)
            sequenceText.text = GetSequenceString(answerSequence);

        if (currentInputText != null)
            currentInputText.text = "현재 입력 : -";

        StartCoroutine(MemorizeRoutine());
    }

    IEnumerator MemorizeRoutine()
    {
        yield return new WaitForSeconds(memorizeTime);

        if (isGameEnded || isFailing)
            yield break;

        if (sequenceText != null)
            sequenceText.text = "???";

        if (phaseText != null)
            phaseText.text = "번호키 1~6으로 전선을 입력하세요!";

        currentTime = inputTime;
        isInputPhase = true;
    }

    List<string> GenerateRandomSequence(int length)
    {
        List<string> temp = new List<string>(wireNames);
        List<string> result = new List<string>();

        for (int i = 0; i < length; i++)
        {
            int randomIndex = Random.Range(0, temp.Count);
            result.Add(temp[randomIndex]);
            temp.RemoveAt(randomIndex);
        }

        return result;
    }

    void CheckKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectWire("빨강");
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectWire("파랑");
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectWire("노랑");
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectWire("초록");
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectWire("검정");
        if (Input.GetKeyDown(KeyCode.Alpha6)) SelectWire("보라");
    }

    public void SelectWire(string wireName)
    {
        if (!isInputPhase || isGameEnded || isFailing)
            return;

        int index = playerInput.Count;

        if (wireName != answerSequence[index])
        {
            StartCoroutine(FailRoutine());
            return;
        }

        playerInput.Add(wireName);

        PlaySFX(electricSound);

        if (currentInputText != null)
            currentInputText.text = "현재 입력 : " + GetSequenceString(playerInput);

        SpawnFlyingPanelPiece(wireName);

        if (playerInput.Count >= answerSequence.Count)
        {
            RoundClear();
        }
    }

    void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    IEnumerator FailRoutine()
    {
        if (isFailing || isGameEnded)
            yield break;

        isFailing = true;
        isInputPhase = false;

        PlaySFX(failScreamSound);

        yield return new WaitForSeconds(1.2f);

        FailGame();
    }

    void SpawnFlyingPanelPiece(string wireName)
    {
        if (wireProjectilePrefab == null || firePoint == null)
        {
            Debug.LogWarning("WireProjectilePrefab 또는 FirePoint가 연결되지 않았습니다.");
            return;
        }

        Transform target = GetTarget(wireName);
        Material material = GetMaterial(wireName);

        if (target == null)
        {
            Debug.LogWarning(wireName + " Target이 연결되지 않았습니다.");
            return;
        }

        GameObject piece = Instantiate(
            wireProjectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        piece.transform.localScale = stuckScale;

        Renderer renderer = piece.GetComponent<Renderer>();
        if (renderer != null && material != null)
            renderer.material = material;

        StartCoroutine(FlyPanelPieceRoutine(piece.transform, target));
    }

    IEnumerator FlyPanelPieceRoutine(Transform piece, Transform target)
    {
        Vector3 startPosition = piece.position;
        Vector3 finalPosition = target.position + target.forward * stickForwardOffset;

        float elapsed = 0f;

        while (elapsed < flyTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / flyTime);

            Vector3 pos = Vector3.Lerp(startPosition, finalPosition, t);
            pos.y += Mathf.Sin(t * Mathf.PI) * arcHeight;

            piece.position = pos;
            piece.Rotate(0f, 0f, 720f * Time.deltaTime);

            yield return null;
        }

        piece.position = finalPosition;
        piece.rotation = target.rotation * Quaternion.Euler(stuckRotation);

        if (sparklePrefab != null)
        {
            GameObject sparkle = Instantiate(
                sparklePrefab,
                finalPosition,
                Quaternion.identity
            );

            Destroy(sparkle, 1.5f);
        }

        Rigidbody rb = piece.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        Collider col = piece.GetComponent<Collider>();
        if (col != null)
            col.enabled = false;
    }

    Transform GetTarget(string wireName)
    {
        switch (wireName)
        {
            case "빨강": return redTarget;
            case "파랑": return blueTarget;
            case "노랑": return yellowTarget;
            case "초록": return greenTarget;
            case "검정": return blackTarget;
            case "보라": return purpleTarget;
            default: return null;
        }
    }

    Material GetMaterial(string wireName)
    {
        switch (wireName)
        {
            case "빨강": return redMaterial;
            case "파랑": return blueMaterial;
            case "노랑": return yellowMaterial;
            case "초록": return greenMaterial;
            case "검정": return blackMaterial;
            case "보라": return purpleMaterial;
            default: return null;
        }
    }

    void RoundClear()
    {
        isInputPhase = false;

        if (currentRound >= totalRound)
        {
            ClearGame();
        }
        else
        {
            if (phaseText != null)
                phaseText.text = "전력 회로 " + currentRound + "단계 차단 성공!";

            if (sequenceText != null)
                sequenceText.text = "다음 라운드 준비...";

            if (currentInputText != null)
                currentInputText.text = "현재 입력 : -";

            currentRound++;
            Invoke(nameof(StartRound), roundDelay);
        }
    }

    void ClearGame()
    {
        if (isGameEnded)
            return;

        PlaySFX(witchLaughSound);

        isGameEnded = true;
        isInputPhase = false;

        int fearEnergy = PlayerPrefs.GetInt("FearEnergy", 0);
        fearEnergy += 100;

        PlayerPrefs.SetInt("FearEnergy", fearEnergy);
        PlayerPrefs.SetInt("WorkerAlreadyCleared", 1);
        PlayerPrefs.SetInt("WorkerNPCGone", 1);
        PlayerPrefs.Save();

        StartCoroutine(BlackoutClearRoutine());
    }

    IEnumerator BlackoutClearRoutine()
    {
        if (phaseText != null)
            phaseText.text = "전력 차단 완료...";

        if (sequenceText != null)
            sequenceText.text = "";

        if (currentInputText != null)
            currentInputText.text = "";

        if (timerText != null)
            timerText.text = "";

        if (resultPanel != null)
            resultPanel.SetActive(false);

        yield return new WaitForSeconds(clearBeforeBlackoutDelay);

        if (blackoutImage != null)
        {
            blackoutImage.gameObject.SetActive(true);

            float elapsed = 0f;
            Color c = blackoutImage.color;
            c.a = 0f;
            blackoutImage.color = c;

            while (elapsed < blackoutFadeTime)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / blackoutFadeTime);

                c.a = Mathf.Lerp(0f, 1f, t);
                blackoutImage.color = c;

                yield return null;
            }

            c.a = 1f;
            blackoutImage.color = c;
        }

        if (blackoutText != null)
        {
            blackoutText.gameObject.SetActive(true);
            blackoutText.text = "암전 성공!";
        }

        yield return new WaitForSeconds(blackoutWaitTime);

        ReturnToVillage();
    }

    void FailGame()
    {
        if (isGameEnded)
            return;

        isGameEnded = true;
        isInputPhase = false;

        PlayerPrefs.SetInt("WorkerAlreadyCleared", 0);
        PlayerPrefs.SetInt("WorkerNPCGone", 0);
        PlayerPrefs.Save();

        ShowResult(false);
    }

    void ShowResult(bool isClear)
    {
        if (phaseText != null)
            phaseText.text = "";

        if (sequenceText != null)
            sequenceText.text = "";

        if (currentInputText != null)
            currentInputText.text = "";

        if (timerText != null)
            timerText.text = "";

        if (resultPanel != null)
            resultPanel.SetActive(true);

        if (resultText != null)
        {
            if (isClear)
                resultText.text = "전력 차단 성공!\n공포 에너지 +100";
            else
                resultText.text = "차단 실패...\n다른 직장인을 찾아보자";
        }

        Invoke(nameof(ReturnToVillage), 2f);
    }

    string GetSequenceString(List<string> sequence)
    {
        if (sequence.Count == 0)
            return "-";

        return string.Join(" > ", sequence);
    }

    void ReturnToVillage()
    {
        SceneManager.LoadScene("VillageScene");
    }
}