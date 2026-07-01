using UnityEngine;
using UnityEngine.SceneManagement;

public class GoHomeTrigger : MonoBehaviour
{
    public GameObject goHomeIcon;

    private bool playerInside = false;

    void Start()
    {
        bool canGoHome = PlayerPrefs.GetInt("CanGoHome", 0) == 1;

        Debug.Log("CanGoHome = " + canGoHome);

        if (canGoHome)
        {
            goHomeIcon.SetActive(true);
            Debug.Log("퇴근 아이콘 활성화");
        }
        else
        {
            goHomeIcon.SetActive(false);
            gameObject.SetActive(false);

            Debug.Log("퇴근 불가능 상태");
        }
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("퇴근하기 실행");

            SceneManager.LoadScene("EndScene");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("무언가 트리거 진입 : " + other.name);

        if (other.CompareTag("Player"))
        {
            playerInside = true;

            Debug.Log("플레이어 퇴근 구역 진입");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            Debug.Log("플레이어 퇴근 구역 이탈");
        }
    }
}