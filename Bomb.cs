using Godot;
using System;

public class Bomb : Area2D
{

    public bool Freeze = false;

    Timer TimerNode;

    AnimationPlayer AnimPlayer;

    [Signal]
    public delegate void Boom(Bomb exploder);

    [Signal]
    public delegate void PlayerKilled();

    [Signal]
    public delegate void EnemyKilled();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AnimPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        AnimPlayer.Play("Idle");

        TimerNode = GetNode<Timer>("Timer");
        TimerNode.Connect("timeout", this, nameof(_OnTimerTimeout));
        
        this.Connect("body_entered", this, nameof(_OnDetectCollide));

        var startTimer = GetNode<Timer>("StartTimer");
        startTimer.Connect("timeout", this, nameof(_OnStartTimerTimeout));
    }

    // Adds a delay for detecting collisions with player since they occupy the same space on creation.
    private void _OnStartTimerTimeout() {
        this.SetCollisionMaskBit(0, true);
    }

    private void _OnDetectCollide(PhysicsBody2D body) {
        if (!Freeze) {

            // Enemy
            if (body.GetCollisionLayerBit(1)) {
                body.QueueFree();
                EmitSignal("EnemyKilled");
            } else if (body.GetCollisionLayerBit(0)) {
                EmitSignal("PlayerKilled");
            }

            EmitSignal("Boom", this);
            QueueFree();
        }
    }

    private void _OnTimerTimeout() {
        if (!Freeze) {
            EmitSignal("Boom", this);
            QueueFree();
        }
    }

    public override void _Process(float delta)
    {
        TimerNode.Paused = Freeze;
        if (Freeze && AnimPlayer.IsPlaying()) {
            AnimPlayer.Stop();
        } else if (!Freeze && !AnimPlayer.IsPlaying()) {
            AnimPlayer.Play("Idle");
        }
    }
}
