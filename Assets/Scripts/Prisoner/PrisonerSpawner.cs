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
        Invoke("SpawnPrisoner", 10f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for(int i = 0; i < 3; i++)
                SpawnPrisoner();
        }
    }

    public void SpawnPrisoner()
    {
        if (prisonerPrefab == null || prisonerQueue == null || spawnPoint == null)
            return;

        Prisoner prisoner = Instantiate(prisonerPrefab, spawnPoint.position, spawnPoint.rotation);
        prisoner.JoinQueue(prisonerQueue);
    }
}