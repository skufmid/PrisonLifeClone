using UnityEngine;

public class Player : CharacterBase
{
    protected override void Update()
    {
        base.Update();

        HandleInput();
    }

    private void HandleInput()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        SetInput(new Vector2(h, v));
    }
}