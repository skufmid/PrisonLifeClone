using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

        RockCarriable rockCarriableGO = Instantiate(rockCarriable);
        carry.TryStart(rockCarriableGO);
    }
}