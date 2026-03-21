using System.Collections;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Mine : MonoBehaviour
{
    public MineTool defaultTool;   // 곡괭이
    public MineTool currentTool;   // 현재 장착 도구

    private Coroutine miningCoroutine;
    private CharacterBase owner;

    private Rock targetRock;

    private const int ROCK_LAYER = 1 << 6;

    private void Awake()
    {
        owner = GetComponent<CharacterBase>();

        if (currentTool == null)
            currentTool = defaultTool;
    }
    private void OnTriggerEnter(Collider other)
    {
        Rock rock = other.GetComponent<Rock>();

        if (rock != null && miningCoroutine == null)
        {
            rock.Interact(owner);
        }
    }

    public void StartMining(Rock rock)
    {
        if (rock == null) return;

        Debug.Log("채굴 시작");
        targetRock = rock;
        miningCoroutine = StartCoroutine(CoMining());
    }

    public void StopMining()
    {
        Debug.Log("채굴 종료");
        if (miningCoroutine == null) return;

        StopCoroutine(miningCoroutine);
        miningCoroutine = null;
    }

    private IEnumerator CoMining()
    {
        while (true)
        {
            // 타겟이 없으면 종료
            if (targetRock == null)
            {
                StopMining();
                yield break;
            }

            yield return new WaitForSeconds(currentTool.interval);

            MineOnce();
        }
    }

    private void MineOnce()
    {
        Vector3 center = transform.position + transform.forward * currentTool.range * 0.5f;

        Collider[] hits = Physics.OverlapSphere(center, currentTool.range, ROCK_LAYER);

        int minedCount = 0;

        foreach (var hit in hits)
        {
            Rock rock = hit.GetComponent<Rock>();
            if (rock != null)
            {
                rock.Mine(owner);
                minedCount++;
            }

            // 최대 채굴량 제한으로 겹쳐진거 동시에 캐는 문제 예방
            if (minedCount >= currentTool.maxMineAmount)
                break;
        }

        if (minedCount == 0) targetRock = null;
    }
}