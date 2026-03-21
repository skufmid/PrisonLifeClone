using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class CharacterBase : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    protected CharacterController controller;
    protected Vector2 input;

    protected virtual void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    protected virtual void Update()
    {
        Move();
    }

    // 입력은 외부에서 주입 (Player, AI 모두 대응 가능)
    public virtual void SetInput(Vector2 inputValue)
    {
        input = inputValue;
    }

    protected virtual void Move()
    {
        Vector3 move = new Vector3(input.x, 0, input.y);

        if (move.magnitude > 1f)
            move.Normalize();

        controller.Move(move * moveSpeed * Time.deltaTime);

        Rotate(move);
    }

    protected virtual void Rotate(Vector3 move)
    {
        if (move != Vector3.zero)
        {
            transform.forward = move;
        }
    }
}