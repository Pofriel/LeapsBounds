using Godot;
using System;

public class Enemy : KinematicBody2D
{

    float TimeWalkin = 0;

    Vector2 Velocity = new Vector2();
    
    [Export]
    public float XSpeed = 25;

    [Export]
    public float WalkLimit = 10;
    
    public override void _Ready()
    {
        // choose random direction.
        var dirVector = new Vector2();
        var random = new Random();
        var dirInt = random.Next(0, 3);
        if (dirInt == 0) {
            dirVector.x = 1;
        } else if (dirInt == 1) {
            dirVector.x = -1;
        } else if (dirInt == 2) {
            dirVector.y = 1;
        } else {
            dirVector.y = -1;
        }

         Velocity = dirVector * XSpeed;
    }

    [Export]
    public bool Freeze = false;

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (!Freeze) {
            var collision = MoveAndCollide(Velocity * delta);
            if (collision != null)
            {          
                var reflect = collision.Remainder.Bounce(collision.Normal);
                Velocity = Velocity.Bounce(collision.Normal);
                MoveAndCollide(reflect);
            }

        }
        else {
            MoveAndCollide(new Vector2(0, 0));
        }
        
    }
}
