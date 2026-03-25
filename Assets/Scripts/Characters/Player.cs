using UnityEngine;

public class Player : CharacterBase
{
    [SerializeField] Joystick joystick;
    protected override void Update()
    {
        base.Update();

        HandleInput();
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