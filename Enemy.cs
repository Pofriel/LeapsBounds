using Godot;
using System;

public class Enemy : KinematicBody2D
{

    float TimeWalkin = 0;

    Vector2 Velocity = new Vector2();
    
    [Export]
    float XSpeed = 25;

    [Export]
    float WalkLimit = 10;
    
    public override void _Ready()
    {
        Velocity = new Vector2(XSpeed, 0);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        TimeWalkin += delta;
        if (TimeWalkin > WalkLimit) {
            XSpeed *= -1;
            TimeWalkin = 0;
        }
        Velocity = new Vector2(XSpeed, 0);
        Velocity = MoveAndSlide(Velocity);
    }
}
