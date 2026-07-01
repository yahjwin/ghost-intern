using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCInteractable : MonoBehaviour
{
    public string npcType;
    public GameObject interactText;
    public NPCSpawner spawner;

    private bool playerInRange = false;

    void Start()
    {
        if (interactText != null)
        {
            interactText.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(npcType + " 미니게임 시작");

            if (interactText != null)
            {
                interactText.SetActive(false);
            }

            if (spawner != null)
            {
                spawner.DecreaseNPCCount();
            }

            if (npcType == "Student")
            {
                SceneManager.LoadScene("JumpScareScene");
            }
            else if (npcType == "Worker")
            {
                SceneManager.LoadScene("WireGameScene");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (interactText != null)
            {
                interactText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactText != null)
            {
                interactText.SetActive(false);
            }
        }
    }
}