using Godot;
using System;

public class RootScene : Node2D
{
   

    
    public PackedScene BombScene = GD.Load<PackedScene>(
        "res://Bomb.tscn"
    );
    public Enemy MyEnemy;

    public Player MyPlayer;

    public Bomb PlayerBomb; 
    public override void _Ready()
    {
        MyEnemy = GetNode<Enemy>("Enemy");
        MyPlayer = GetNode<Player>("Player");
        MyPlayer.Connect("ProtonLeap", this, nameof(_OnProtonLeap));
        MyPlayer.Connect("CreateBomb", this, nameof(_OnCreateBomb));
    }

    private void _OnProtonLeap(bool isActive) {
        MyEnemy.Freeze = isActive;
    }

    private void _OnCreateBomb(Vector2 position) {
        PlayerBomb = BombScene.Instance() as Bomb;
        AddChild(PlayerBomb);
        PlayerBomb.GlobalPosition = position;
    }

}
