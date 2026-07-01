using System.Collections;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject studentPrefab;
    public GameObject workerPrefab;
    public GameObject guardPrefab;

    public Transform player;

    [Header("일반 NPC 설정")]
    public int maxNPCCount = 6;
    public float spawnInterval = 3f;

    public Vector2 spawnRangeX = new Vector2(-40f, 40f);
    public Vector2 spawnRangeZ = new Vector2(-40f, 40f);

    public float minDistanceFromPlayer = 15f;

    [Header("경비원 설정")]
    public Vector2 guardSpawnRangeX = new Vector2(-15f, 15f);
    public Vector2 guardSpawnRangeZ = new Vector2(-15f, 15f);
    public float guardMinDistanceFromPlayer = 5f;

    private int currentNPCCount = 0;
    private GameObject currentGuard;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        HandleGuardSpawn();
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (currentNPCCount < maxNPCCount)
            {
                SpawnRandomNPC();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnRandomNPC()
    {
        GameObject prefabToSpawn;

        int randomType = Random.Range(0, 2);

        if (randomType == 0)
        {
            prefabToSpawn = studentPrefab;
        }
        else
        {
            prefabToSpawn = workerPrefab;
        }

        Vector3 spawnPosition = GetRandomSpawnPosition();

        GameObject npc = Instantiate(
            prefabToSpawn,
            spawnPosition,
            Quaternion.identity
        );

        currentNPCCount++;

        NPCInteractable interactable =
            npc.GetComponent<NPCInteractable>();

        if (interactable != null)
        {
            interactable.spawner = this;
        }
    }

    void HandleGuardSpawn()
    {
        int fearEnergy = PlayerPrefs.GetInt("FearEnergy", 0);

        bool guardCleared =
            PlayerPrefs.GetInt("GuardAlreadyCleared", 0) == 1;

        if (guardCleared)
            return;

        if (fearEnergy >= 200 && currentGuard == null)
        {
            Vector3 spawnPosition = GetRandomGuardSpawnPosition();

            currentGuard = Instantiate(
                guardPrefab,
                spawnPosition,
                Quaternion.identity
            );

            Debug.Log("경비원 생성!");
        }

        if (fearEnergy < 200 && currentGuard != null)
        {
            Destroy(currentGuard);
            currentGuard = null;

            Debug.Log("경비원 제거!");
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPosition;

        int maxTryCount = 30;
        int tryCount = 0;

        do
        {
            float x = Random.Range(spawnRangeX.x, spawnRangeX.y);
            float z = Random.Range(spawnRangeZ.x, spawnRangeZ.y);

            spawnPosition = new Vector3(x, 1f, z);

            tryCount++;

        } while (
            player != null &&
            Vector3.Distance(spawnPosition, player.position) < minDistanceFromPlayer &&
            tryCount < maxTryCount
        );

        return spawnPosition;
    }

    Vector3 GetRandomGuardSpawnPosition()
    {
        Vector3 spawnPosition;

        int maxTryCount = 30;
        int tryCount = 0;

        do
        {
            float x = Random.Range(guardSpawnRangeX.x, guardSpawnRangeX.y);
            float z = Random.Range(guardSpawnRangeZ.x, guardSpawnRangeZ.y);

            spawnPosition = new Vector3(x, 1f, z);

            tryCount++;

        } while (
            player != null &&
            Vector3.Distance(spawnPosition, player.position) < guardMinDistanceFromPlayer &&
            tryCount < maxTryCount
        );

        return spawnPosition;
    }

    public void DecreaseNPCCount()
    {
        currentNPCCount--;

        if (currentNPCCount < 0)
        {
            currentNPCCount = 0;
        }
    }
}