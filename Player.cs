using Godot;
using System;

public class Player : Godot.KinematicBody2D
{
    
    [Export]
    public int Speed = 200;

    Vector2 velocity = new Vector2();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void GetInput() {
        velocity = new Vector2();

        if (Input.IsActionPressed("right")) {
            velocity.x += 1;
        }

        if (Input.IsActionPressed("left")) {
            velocity.x -= 1;
        }

        if (Input.IsActionPressed("down")) {
            velocity.y += 1;
        }

        if (Input.IsActionPressed("up")) {
            velocity.y -= 1;
        }

        var animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        if (velocity.Length() > 0) {
            animationPlayer.Play("Walk");
        } else {
            animationPlayer.Play("Idle");
        }
       

        velocity = velocity.Normalized() * Speed;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        GetInput();
        velocity = MoveAndSlide(velocity);    
    }
}
