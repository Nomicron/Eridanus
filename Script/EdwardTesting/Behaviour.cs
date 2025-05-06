using Godot;
using System;

public partial class Behaviour : CharacterBody3D
{
    // Exporterade variabler för anpassning i editorn
    [Export] public AudioStream WalkingSound;
    [Export] public float MoveSpeed = 3.0f;
    [Export] public float Acceleration = 5.0f;
    [Export] public float DetectionDistance = 15.0f;
    [Export(PropertyHint.Range, "0,180")] public float FieldOfView = 90f;
    [Export] public float MinMoveDelay = 2.0f;
    [Export] public float MaxMoveDelay = 5.0f;
    [Export] public float PowerOutageChance = 0.2f;
    [Export] public CharacterBody3D Player; // Tilldelas i editorn eller via kod
    
    // Privata variabler
    private bool activated = false;
    private bool playerCanSee = false;
    private AudioStreamPlayer3D audioPlayer;
    private Node3D visualNode;
    private AnimationPlayer animationPlayer;
    private Vector3[] lightPositions;
    private int currentLightIndex = 0;
    private Random random = new Random();
    private Vector3 targetPosition;
    private Vector3 velocity = Vector3.Zero;
    private bool isMoving = false;
    private double timeUntilNextMove = 0;

    public override void _Ready()
    {
        // Initiera ljudspelare
        audioPlayer = new AudioStreamPlayer3D();
        AddChild(audioPlayer);
        audioPlayer.Stream = WalkingSound;
        audioPlayer.UnitSize = 10f;
        
        // Hämta referenser till noder
        visualNode = GetNode<Node3D>("Visual");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        
        // Hitta alla ljuskällor i scenen
        FindLightPositions();
        
        // Dölj monster från början
        visualNode.Visible = false;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!activated) return;
        
        // Uppdatera spelarens syn
        UpdatePlayerVision();
        
        if (isMoving)
        {
            MoveTowardsTarget((float)delta);
        }
        else
        {
            // Räknar ner tills nästa rörelse
            timeUntilNextMove -= delta;
            if (timeUntilNextMove <= 0)
            {
                ChooseNewTarget();
            }
        }
    }

    private void FindLightPositions()
    {
        // Hämta alla noder i ljusgruppen
        var lightNodes = GetTree().GetNodesInGroup("light_sources");
        lightPositions = new Vector3[lightNodes.Count];
        
        for (int i = 0; i < lightNodes.Count; i++)
        {
            if (lightNodes[i] is Node3D lightNode)
            {
                lightPositions[i] = lightNode.GlobalPosition;
            }
        }
        
        if (lightPositions.Length == 0)
        {
            GD.PrintErr("Inga ljuskällor hittades! Använd gruppen 'light_sources' på dina lampor.");
            lightPositions = new Vector3[] { GlobalPosition };
        }
    }

    private void UpdatePlayerVision()
    {
        if (Player == null) 
        {
            GD.PrintErr("Spelaren är inte tilldelad!");
            return;
        }
        
        // Kontrollera avstånd
        float distanceToPlayer = GlobalPosition.DistanceTo(Player.GlobalPosition);
        if (distanceToPlayer > DetectionDistance)
        {
            playerCanSee = false;
            return;
        }
        
        // Kontrollera synlinje
        var spaceState = GetWorld3D().DirectSpaceState;
        var query = PhysicsRayQueryParameters3D.Create(
            Player.GlobalPosition,
            GlobalPosition,
            CollisionMask,
            exclude: new Godot.Collections.Array<Rid> { Player.GetRid(), GetRid() });
        
        var result = spaceState.IntersectRay(query);
        
        bool hasClearLineOfSight = result.Count == 0;
        
        // Kontrollera synfält (använder spelarens forward-vektor)
        Vector3 dirToMonster = (GlobalPosition - Player.GlobalPosition).Normalized();
        Vector3 playerForward = -Player.GlobalTransform.Basis.Z;
        
        float angle = Mathf.RadToDeg(Mathf.Acos(dirToMonster.Dot(playerForward)));
        
        playerCanSee = hasClearLineOfSight && angle < FieldOfView;
    }

    private void MoveTowardsTarget(float delta)
    {
        if (playerCanSee)
        {
            // Spelaren ser oss - avbryt rörelse
            isMoving = false;
            velocity = Vector3.Zero;
            animationPlayer.Play("idle");
            return;
        }
        
        Vector3 direction = (targetPosition - GlobalPosition).Normalized();
        float distance = GlobalPosition.DistanceTo(targetPosition);
        
        if (distance > 0.5f)
        {
            // Accelerera mot målet
            velocity = velocity.Lerp(direction * MoveSpeed, Acceleration * delta);
            
            // Rotera mot målet (endast på Y-axeln)
            LookAt(new Vector3(targetPosition.X, GlobalPosition.Y, targetPosition.Z), Vector3.Up);
            
            // Utför rörelsen med kollisionshantering
            Velocity = velocity;
            MoveAndSlide();
            
            // Spela gånganimation
            animationPlayer.Play("walk");
        }
        else
        {
            // Nått målet
            isMoving = false;
            velocity = Vector3.Zero;
            animationPlayer.Play("idle");
            ScheduleNextMove();
        }
    }

    private void ChooseNewTarget()
    {
        if (playerCanSee) return;
        
        // Slumpmässigt strömavbrott (blanda ljuspositioner)
        if (random.NextDouble() < PowerOutageChance)
        {
            ShuffleLightPositions();
        }
        
        // Välj en ny ljuskälla (inte den nuvarande)
        int newIndex;
        do {
            newIndex = random.Next(lightPositions.Length);
        } while (newIndex == currentLightIndex && lightPositions.Length > 1);
        
        currentLightIndex = newIndex;
        targetPosition = lightPositions[currentLightIndex];
        isMoving = true;
        
        // Spela ljudeffekt
        audioPlayer.Play();
    }

    private void ShuffleLightPositions()
    {
        // Fisher-Yates shuffle algoritm
        for (int i = lightPositions.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            Vector3 temp = lightPositions[i];
            lightPositions[i] = lightPositions[j];
            lightPositions[j] = temp;
        }
    }

    private void ScheduleNextMove()
    {
        timeUntilNextMove = random.NextSingle() * (MaxMoveDelay - MinMoveDelay) + MinMoveDelay;
    }

    public void Activate()
    {
        activated = true;
        visualNode.Visible = true;
        
        // Startposition vid närmaste ljuskälla
        if (lightPositions != null && lightPositions.Length > 0)
        {
            currentLightIndex = FindClosestLightIndex();
            GlobalPosition = lightPositions[currentLightIndex];
        }
        
        ScheduleNextMove();
    }

    private int FindClosestLightIndex()
    {
        int closestIndex = 0;
        float closestDistance = float.MaxValue;
        
        for (int i = 0; i < lightPositions.Length; i++)
        {
            float distance = GlobalPosition.DistanceTo(lightPositions[i]);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }
        
        return closestIndex;
    }

    private void OnDetectionAreaBodyEntered(Node3D body)
    {
        if (body is CharacterBody3D playerBody && !activated && playerBody == Player)
        {
            Activate();
        }
    }
}