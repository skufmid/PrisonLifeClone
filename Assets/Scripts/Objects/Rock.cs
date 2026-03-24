using System.Collections;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public int resourceAmount = 1;

    [SerializeField] private float respawnTime = 3f;
    [SerializeField] private RockCarriable rockCarriable;

    private Collider collider;
    private Renderer renderer;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();
    }

    public void Mine(CharacterBase owner)
    {
        SpawnResourceToCarry(owner);
        SetEnable(false);
        StartCoroutine(CoRespawn(respawnTime));
    }

    public bool TryMineToTileStack(TileStack tileStack)
    {
        if (tileStack == null) return false;
        if (rockCarriable == null) return false;
        if (tileStack.IsFull) return false;

        RockCarriable rockCarriableGO = Instantiate(rockCarriable, transform);
        if (!tileStack.TryAdd(rockCarriableGO))
        {
            Destroy(rockCarriableGO.gameObject);
        }

        SetEnable(false);
        StartCoroutine(CoRespawn(respawnTime));
        return true;
    }


    private void SpawnResourceToCarry(CharacterBase owner)
    {
        Carry carry = owner.GetComponent<Carry>();
        if (carry == null) return;

        RockCarriable rockCarriableGO = Instantiate(rockCarriable, transform);
        bool added = carry.TryAdd(rockCarriableGO);

        if (!added)
        {
            Destroy(rockCarriableGO.gameObject);
        }
    }

    private void SetEnable(bool enable)
    {
        if (collider != null) collider.enabled = enable;
        if (renderer != null) renderer.enabled = enable;
    }

    private IEnumerator CoRespawn(float time)
    {
        while (time > 0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        SetEnable(true);
    }
}