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

    private void SpawnResourceToCarry(CharacterBase owner)
    {
        Carry carry = owner.GetComponent<Carry>();
        if (carry == null) return; // 여기에 carry가 null이면 TileStack으로 이동하게

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