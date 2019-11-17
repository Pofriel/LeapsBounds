using Godot;
using System;

public class Player : Godot.KinematicBody2D
{
    
    [Export]
    public int Speed = 100;

    Vector2 velocity = new Vector2();

    public bool SelectionMode = false;

    public PackedScene IndicatorScene = GD.Load<PackedScene>("res://SelectionIndicator.tscn");

    public SelectionIndicator Indicator;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _Input(InputEvent @event) 
    {
        if (@event.IsActionPressed("enter")) {
            if (!SelectionMode) {
                _StartSelectionMode();
            } else {
                _StopSelectionMode();
            }
        }
        
    }

    private void _StartSelectionMode() {
        SelectionMode = true;
        MoveAndSlide(new Vector2(0, 0)); 
        Indicator = IndicatorScene.Instance() as SelectionIndicator;
        AddChild(Indicator);
    }

    private void _StopSelectionMode() {
        GlobalPosition = new Vector2(Indicator.GlobalPosition.x, Indicator.GlobalPosition.y);
        RemoveChild(Indicator);
        Indicator.QueueFree();
        SelectionMode = false;
    }

    public void GetInput() {
        if (!SelectionMode) {
            velocity = new Vector2();

            var spriteNode = GetNode<Sprite>("Sprite");

            if (Input.IsActionPressed("right")) {
                spriteNode.FlipH = false;
                velocity.x += 1;
            }

            if (Input.IsActionPressed("left")) {
                spriteNode.FlipH = true;
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
        
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        GetInput();
        velocity = MoveAndSlide(velocity);    
    }
}
