using UnityEngine;

public class Rock : MonoBehaviour
{
    public int resourceAmount = 1;
    [SerializeField] private RockCarriable rockCarriable;

    public void Mine(CharacterBase owner)
    {
        Debug.Log("πŸ¿ß √§±ºµ ");
        SpawnResource(owner);
        gameObject.SetActive(false);
    }

    private void SpawnResource(CharacterBase owner)
    {
        Carry carry = owner.GetComponent<Carry>();
        if (carry == null) return;

        RockCarriable rockCarriableGO = Instantiate(rockCarriable);
        carry.TryAdd(rockCarriableGO);
    }
}