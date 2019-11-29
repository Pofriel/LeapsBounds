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

    public int Score;

    public List<Bomb> PlayerBombs; 
    public override void _Ready()
    {
        Score = 0;
        PlayerBombs = new List<Bomb>();
        MyEnemy = GetNode<Enemy>("Enemy");
        MyPlayer = GetNode<Player>("Player");
        MyPlayer.Connect("ProtonLeap", this, nameof(_OnProtonLeap));
        MyPlayer.Connect("CreateBomb", this, nameof(_OnCreateBomb));
        MyPlayer.Connect("PlayerKilled", this, nameof(_OnPlayerKilled));
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
        bomb.Connect("PlayerKilled", this, nameof(_OnPlayerKilled));
        bomb.Connect("EnemyKilled", this, nameof(_OnEnemyKilled));
        bomb.Monitoring = true;
    }

    private void _OnPlayerKilled() {
        // Do some game over type work here.
        MyPlayer.SelectionMode = true;
    }

    private void _OnEnemyKilled() {
        Score++;
        GD.Print("Score: " + Score.ToString());
    }

    private void _OnBombBoom(Bomb exploder) {
        PlayerBombs.Remove(exploder);
    }

}
