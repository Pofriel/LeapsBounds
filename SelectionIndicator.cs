using Godot;
using System;

public class SelectionIndicator : Node2D
{

    public Sprite Sprite;

    public Vector2 SpriteSize;

    public override void _Input(InputEvent @event) 
    {
        Vector2 newPosition = new Vector2(Position.x, Position.y);
        if (@event.IsActionPressed("left")) {
            newPosition.x = newPosition.x - (SpriteSize.x / 2);
        } else if (@event.IsActionPressed("right")) {
            newPosition.x = newPosition.x + (SpriteSize.x / 2);
        } else if (@event.IsActionPressed("down")) {
            newPosition.y = newPosition.y + SpriteSize.y;
        } else if (@event.IsActionPressed("up")) {
            newPosition.y = newPosition.y - SpriteSize.y;
        }
        

        Position = newPosition; 

    }

    public override void _Ready()
    {
        Sprite = GetNode<Sprite>("Sprite");
        SpriteSize = Sprite.GetTexture().GetSize();

        var animation = GetNode<AnimationPlayer>("AnimationPlayer");
        animation.Play("Idle");
    }
}
