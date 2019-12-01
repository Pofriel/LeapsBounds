using Godot;
using System;
using System.Collections.Generic;

public class RootScene : Node2D
{
    public PackedScene BombScene = GD.Load<PackedScene>(
        "res://Bomb.tscn"
    );

    public PackedScene EnemyScene = GD.Load<PackedScene>("res://Enemy.tscn");

    public Player MyPlayer;

    public int Score;

    public List<Bomb> PlayerBombs; 

    public List<Enemy> Enemies;

    public Timer EnemyTimer;

    private int _enemiesCreated = 0;

    private Label _scoreLabel;

    public override void _Ready()
    {
        Score = 0;
        PlayerBombs = new List<Bomb>();
        Enemies = new List<Enemy>();
        MyPlayer = GetNode<Player>("Player");
        EnemyTimer = GetNode<Timer>("EnemyTimer");
        _scoreLabel = GetNode<Label>("CanvasLayer/Score");
        EnemyTimer.Connect("timeout", this, nameof(_OnSpawnEnemy));
        MyPlayer.Connect("ProtonLeap", this, nameof(_OnProtonLeap));
        MyPlayer.Connect("CreateBomb", this, nameof(_OnCreateBomb));
        MyPlayer.Connect("PlayerKilled", this, nameof(_OnPlayerKilled));
        _OnSpawnEnemy();
    }

    private void _OnSpawnEnemy() {
        var enemy = EnemyScene.Instance() as Enemy;
        Enemies.Add(enemy);
        AddChild(enemy);
        _enemiesCreated++;
        if (_enemiesCreated % 5 == 0) {
            EnemyTimer.WaitTime = Mathf.Max(EnemyTimer.WaitTime / 2, 1);
        }
        enemy.GlobalPosition = new Vector2(100, 50);
    }

    private void _OnProtonLeap(bool isActive) {
        EnemyTimer.Paused = isActive;
        if (Enemies.Count > 0) {
            foreach(var enemy in Enemies) {
                if (enemy != null) {
                    enemy.Freeze = isActive;
                }
                
            }
        }
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
        _OnProtonLeap(true);
    }

    private void _OnEnemyKilled() {
        Score++;
        _scoreLabel.Text = Score.ToString();
    }

    private void _OnBombBoom(Bomb exploder) {
        PlayerBombs.Remove(exploder);
    }

}
