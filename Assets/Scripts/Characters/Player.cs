using UnityEngine;

public class Player : CharacterBase
{
    public bool IsMovable { get; set; } = true;

    [SerializeField] Joystick joystick;
    protected override void Update()
    {
        if (!IsMovable) return;

        HandleInput();
        base.Update();
    }

    private void HandleInput()
    {
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");
        float h = joystick.Horizontal;
        float v = joystick.Vertical;

        SetInput(new Vector2(h, v));
    }
}