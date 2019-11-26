using Godot;
using System;

public class Bomb : Node2D
{

    public bool Freeze = false;

    Timer TimerNode;

    AnimationPlayer AnimPlayer;

    [Signal]
    public delegate void Boom(Bomb exploder);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AnimPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        AnimPlayer.Play("Idle");

        TimerNode = GetNode<Timer>("Timer");
        TimerNode.Connect("timeout", this, nameof(_OnTimerTimeout));
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
