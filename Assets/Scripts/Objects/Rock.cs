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
        SpawnResource(owner);
        SetEnable(false);
        StartCoroutine(CoRespawn(respawnTime));
    }

    private void SpawnResource(CharacterBase owner)
    {
        Carry carry = owner.GetComponent<Carry>();
        if (carry == null) return;

        RockCarriable rockCarriableGO = Instantiate(rockCarriable, transform);
        carry.TryAdd(rockCarriableGO);
    }

    private void SetEnable(bool enable)
    {
        collider.enabled = enable;
        renderer.enabled = enable;
    }

    IEnumerator CoRespawn(float time)
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        SetEnable(true);
    }
}