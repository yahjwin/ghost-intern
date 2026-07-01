using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("공포 에너지")]
    public int fearEnergy = 0;
    public int maxFearEnergy = 300;

    [Header("UI")]
    public TextMeshProUGUI fearEnergyText;

    [Header("NPC")]
    public GameObject studentNPC;
    public GameObject workerNPC;

    public bool studentCleared = false;
    public bool workerCleared = false;

    private bool guardSpawned = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        LoadGameData();
        CheckMiniGameResult();
        UpdateFearEnergyUI();
    }

    void LoadGameData()
    {
        fearEnergy = PlayerPrefs.GetInt("FearEnergy", 0);

        studentCleared =
            PlayerPrefs.GetInt("StudentAlreadyCleared", 0) == 1;

        workerCleared =
            PlayerPrefs.GetInt("WorkerAlreadyCleared", 0) == 1;
    }

    void SaveGameData()
    {
        PlayerPrefs.SetInt("FearEnergy", fearEnergy);
        PlayerPrefs.SetInt("StudentAlreadyCleared", studentCleared ? 1 : 0);
        PlayerPrefs.SetInt("WorkerAlreadyCleared", workerCleared ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void AddFearEnergy(int amount)
    {
        fearEnergy += amount;

        if (fearEnergy > maxFearEnergy)
            fearEnergy = maxFearEnergy;

        SaveGameData();
        UpdateFearEnergyUI();

        if (fearEnergy >= maxFearEnergy)
        {
            GoBackToGhostCompany();
        }
    }

    public void StudentClear()
    {
        if (studentCleared)
            return;

        studentCleared = true;
        AddFearEnergy(100);

        SaveGameData();
        CheckGuardCondition();
    }

    public void WorkerClear()
    {
        if (workerCleared)
            return;

        workerCleared = true;
        AddFearEnergy(100);

        SaveGameData();
        CheckGuardCondition();
    }

    void CheckMiniGameResult()
    {
        if (PlayerPrefs.GetInt("StudentClear", 0) == 1)
        {
            StudentClear();
            PlayerPrefs.SetInt("StudentClear", 0);
        }

        if (PlayerPrefs.GetInt("StudentNPCGone", 0) == 1)
        {
            if (studentNPC != null)
                studentNPC.SetActive(false);
        }

        if (PlayerPrefs.GetInt("WorkerClear", 0) == 1)
        {
            WorkerClear();
            PlayerPrefs.SetInt("WorkerClear", 0);
        }

        if (PlayerPrefs.GetInt("WorkerNPCGone", 0) == 1)
        {
            if (workerNPC != null)
                workerNPC.SetActive(false);
        }

        SaveGameData();
    }

    void CheckGuardCondition()
    {
        if (studentCleared &&
            workerCleared &&
            !guardSpawned)
        {
            guardSpawned = true;
            Debug.Log("경비원 등장!");
        }
    }

    void UpdateFearEnergyUI()
    {
        if (fearEnergyText != null)
        {
            fearEnergyText.text =
                "공포 에너지 : " + fearEnergy + " / " + maxFearEnergy;
        }
    }

    void GoBackToGhostCompany()
    {
        Debug.Log("퇴근 가능!");
    }
}