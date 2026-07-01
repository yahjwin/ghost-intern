using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                Debug.LogError("BGMManager에 AudioSource가 없습니다!");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}