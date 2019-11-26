using Godot;
using System;
using System.Collections.Generic;

public class RootScene : Node2D
{
    public PackedScene BombScene = GD.Load<PackedScene>(
        "res://Bomb.tscn"
    );
    public Enemy MyEnemy;

    public Player MyPlayer;

    public List<Bomb> PlayerBombs; 
    public override void _Ready()
    {
        PlayerBombs = new List<Bomb>();
        MyEnemy = GetNode<Enemy>("Enemy");
        MyPlayer = GetNode<Player>("Player");
        MyPlayer.Connect("ProtonLeap", this, nameof(_OnProtonLeap));
        MyPlayer.Connect("CreateBomb", this, nameof(_OnCreateBomb));
    }

    private void _OnProtonLeap(bool isActive) {
        MyEnemy.Freeze = isActive;
        if (PlayerBombs.Count > 0) {
            foreach (var bomb in PlayerBombs) {
                bomb.Freeze = isActive;
            }
            
        }
    }

    private void _OnCreateBomb(Vector2 position) {
        var bomb = BombScene.Instance() as Bomb;
        AddChild(bomb);
        bomb.GlobalPosition = position;
        PlayerBombs.Add(bomb);
        bomb.Connect("Boom", this, nameof(_OnBombBoom));
    }

    private void _OnBombBoom(Bomb exploder) {
        PlayerBombs.Remove(exploder);
    }

}
