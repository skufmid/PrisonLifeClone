using UnityEngine;

public class PrisonerSpawner : MonoBehaviour
{
    [SerializeField] private Prisoner prisonerPrefab;
    [SerializeField] private PrisonerQueue prisonerQueue;
    [SerializeField] private Transform spawnPoint;

    [ContextMenu("Spawn Prisoner")]

    private void Awake()
    {
        Invoke("SpawnPrisoner", 1f);
        Invoke("SpawnPrisoner", 2f);
        Invoke("SpawnPrisoner", 3f);
    }

    private void SpawnPrisoner()
    {
        if (prisonerPrefab == null || prisonerQueue == null || spawnPoint == null)
            return;

        Prisoner prisoner = Instantiate(prisonerPrefab, spawnPoint.position, spawnPoint.rotation);
        prisoner.JoinQueue(prisonerQueue);
    }
}