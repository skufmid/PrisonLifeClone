using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class ActionTileBase : MonoBehaviour
{
    protected virtual void Reset()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = true;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        CharacterBase character = other.GetComponent<CharacterBase>();
        if (character == null) return;

        OnCharacterEnter(character);
    }

    protected abstract void OnCharacterEnter(CharacterBase character);
}