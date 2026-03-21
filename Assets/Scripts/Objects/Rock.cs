using UnityEngine;

public class Rock : MonoBehaviour
{
    public int resourceAmount = 1;

    public void Mine(CharacterBase owner)
    {
        Debug.Log("¹ÙÀ§ Ã¤±¼µÊ");
        SpawnResource();
        gameObject.SetActive(false);
    }

    private void SpawnResource()
    {
        Debug.Log($"ÀÚ¿ø »ý¼º: {resourceAmount}");
    }
}