using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NPCInteraction : MonoBehaviour
{
    [Header("NPC 종류")]
    public string npcType;

    [Header("이동할 미니게임 씬 이름")]
    public string miniGameSceneName;

    [Header("UI")]
    public TextMeshProUGUI interactText;

    [Header("효과음")]
    public AudioClip enterMiniGameSound;

    private bool playerInRange = false;
    private bool isEntering = false;
    private AudioSource playerAudioSource;

    void Start()
    {
        if (interactText != null)
            interactText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F) && !isEntering)
        {
            StartCoroutine(EnterMiniGame());
        }
    }

    IEnumerator EnterMiniGame()
    {
        isEntering = true;

        PlayerPrefs.SetString("CurrentNPC", npcType);

        if (interactText != null)
            interactText.gameObject.SetActive(false);

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerAudioSource = player.GetComponent<AudioSource>();

            if (playerAudioSource != null && enterMiniGameSound != null)
            {
                playerAudioSource.PlayOneShot(enterMiniGameSound);
            }
            else
            {
                Debug.LogWarning("Player AudioSource 또는 Enter 효과음이 비어 있음");
            }
        }

        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene(miniGameSceneName);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (interactText != null)
            {
                interactText.gameObject.SetActive(true);
                interactText.text = "F 키를 눌러 놀래키기";
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactText != null)
                interactText.gameObject.SetActive(false);
        }
    }
}