using UnityEngine;

public class Rock : MonoBehaviour, IInteractable
{
    public int resourceAmount = 1; // 생성할 바위 자원량

    public void Interact(CharacterBase character)
    {
        Mine miner = character.GetComponent<Mine>();

        if (miner != null)
        {
            miner.StartMining(this);
        }
    }

    public void Mine(CharacterBase owner)
    {
        Debug.Log($"바위 채굴됨, 채굴자 {owner.gameObject.ToString()}");

        SpawnResource();
        gameObject.SetActive(false);
    }

    private void SpawnResource()
    {
        // TODO: 바위 → 자원 생성 (수갑 재료, 돈 등)
        Debug.Log($"자원 생성: {resourceAmount}");
    }
}