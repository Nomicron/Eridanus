using Godot;
using System;

public partial class PlayerStamina : Node
{
    [Export] public float Stamina { get; private set; }
    [Export] private float staminaMax = 100;
    [Export] private float sprintDepletion = 0.50f;
    [Export] private float jumpDepletion = 10f;
    [Export] private float staminaReplenishment = 0.1f;

    private float oldStamina;

    public override void _Ready()
    {
        Stamina = staminaMax;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (oldStamina == Stamina && Stamina < staminaMax)
        {
            if (Stamina + staminaReplenishment > staminaMax)
            {
                Stamina = 100;
            }
            else
            {
                Stamina += staminaReplenishment;
            }
            
            GD.Print(Stamina);
        }

        oldStamina = Stamina;
    }

    public bool CanSprint()
    {
        float check = Stamina;
        check -= sprintDepletion;

        GD.Print(Stamina);

        if (check < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool CanJump()
    {
        float check = Stamina;
        check -= jumpDepletion;

        GD.Print(Stamina);

        if (check < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void Sprint()
    {
        Stamina -= sprintDepletion;
    }

    public void Jump()
    {
        Stamina -= jumpDepletion;
    }
}
