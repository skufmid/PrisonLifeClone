using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class ActionTileBase : MonoBehaviour
{
    [Header("Action Loop")]
    [SerializeField] private float actionInterval = 0.05f;

    private readonly HashSet<CharacterBase> charactersInTile = new();
    private Coroutine coActionLoop;
    private WaitForSeconds cachedWait;

    protected virtual void Awake()
    {
        cachedWait = new WaitForSeconds(actionInterval);
    }

    protected virtual void Reset()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = true;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        CharacterBase character = other.GetComponent<CharacterBase>();
        if (character == null) return;

        if (charactersInTile.Add(character))
        {
            OnCharacterEnter(character);

            if (coActionLoop == null)
                coActionLoop = StartCoroutine(CoActionLoop());
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        CharacterBase character = other.GetComponent<CharacterBase>();
        if (character == null) return;

        if (charactersInTile.Remove(character))
        {
            OnCharacterExit(character);

            if (charactersInTile.Count == 0 && coActionLoop != null)
            {
                StopCoroutine(coActionLoop);
                coActionLoop = null;
            }
        }
    }

    protected virtual void OnDisable()
    {
        if (coActionLoop != null)
        {
            StopCoroutine(coActionLoop);
            coActionLoop = null;
        }

        charactersInTile.Clear();
    }

    private IEnumerator CoActionLoop()
    {
        while (charactersInTile.Count > 0)
        {
            foreach (CharacterBase character in charactersInTile)
            {
                if (character == null) continue;
                ProcessCharacter(character);
            }

            yield return cachedWait;
        }

        coActionLoop = null;
    }

    protected virtual void OnCharacterEnter(CharacterBase character) { }
    protected virtual void OnCharacterExit(CharacterBase character) { }

    protected abstract void ProcessCharacter(CharacterBase character);
}