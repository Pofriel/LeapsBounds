using Godot;
using System;

public class RootScene : Node2D
{
   
    public Enemy MyEnemy;

    public Player MyPlayer;
    public override void _Ready()
    {
        MyEnemy = GetNode<Enemy>("Enemy");
        MyPlayer = GetNode<Player>("Player");
        MyPlayer.Connect("ProtonLeap", this, nameof(_OnProtonLeap));
    }

    private void _OnProtonLeap(bool isActive) {
        MyEnemy.Freeze = isActive;
    }

}
