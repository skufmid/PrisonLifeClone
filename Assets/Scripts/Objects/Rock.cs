using UnityEngine;

public class Rock : MonoBehaviour
{
    public int resourceAmount = 1;
    [SerializeField] private RockCarriable rockCarriable;

    public void Mine(CharacterBase owner)
    {
        SpawnResource(owner);
        gameObject.SetActive(false);
    }

    private void SpawnResource(CharacterBase owner)
    {
        Carry carry = owner.GetComponent<Carry>();
        if (carry == null) return;

        RockCarriable rockCarriableGO = Instantiate(rockCarriable, transform);
        carry.TryAdd(rockCarriableGO);
    }
}