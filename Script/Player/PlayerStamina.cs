using Godot;
using System;

public partial class PlayerStamina : Node
{
    [Export] public float Stamina { get; private set; }
    [Export] private float staminaMax = 100.0f;
    [Export] private float sprintDepletion = 0.175f;
    [Export] private float jumpDepletion = 25.0f;
    [Export] private float staminaReplenishment = 7.5f;
    [Export] private TextureRect vignette;

    private bool cooldown;
    private float oldStamina;
    private float cooldownTimer;
    private float cooldownDur = 6.5f;

    public override void _Ready()
    {
        Stamina = staminaMax;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!cooldown)
        {
            if (oldStamina == Stamina && Stamina < staminaMax)
            {
                Stamina += staminaReplenishment * (float)delta;

                GD.Print(Stamina);
            }
        }

        oldStamina = Stamina;

        if (Stamina <= 1.5f)
        {
            cooldown = true;
        }

        if (cooldown && cooldownTimer < cooldownDur)
        {
            cooldownTimer += (float)delta;
        }
        else if (cooldown && cooldownTimer >= cooldownDur)
        {
            cooldown = false;
            cooldownTimer = 0;
        }

        vignette.SelfModulate = new Color(vignette.SelfModulate.R, vignette.SelfModulate.G, vignette.SelfModulate.B, 1 - Stamina / 100);
    }

    public bool CanSprint()
    {
        if (!cooldown)
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
        else
        {
            return false;
        }
    }

    public bool CanJump()
    {
        if (!cooldown)
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
        else
        {
            return false;
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
