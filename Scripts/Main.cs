using Godot;
using System;
using System.Collections.Generic;

public partial class Main : Node3D
{
	[Export] Node3D target;
    [Export] CoinManager _coinManager;

	private List<Enemy> _enemies = new List<Enemy>();
	private List<Point_Of_Interest> _pointsOfInterest = new List<Point_Of_Interest>();
	public override void _Ready()
    {
        GatherPointOfInterest();
        GetAndInitializeEnemyObjects();
        InitializeCoinManager();
    }

    private void InitializeCoinManager()
    {
        if (_coinManager == null) { return; }
        _coinManager.Initialize(_pointsOfInterest);
    }

    private void GatherPointOfInterest()
    {
        var group = GetTree().GetNodesInGroup("PointsOfInterest");
        foreach (var node in group)
        {
            if (node is Point_Of_Interest)
            {
                _pointsOfInterest.Add(node as Point_Of_Interest);
            }
        }
    }
    private void GetAndInitializeEnemyObjects()
    {
        var group = GetTree().GetNodesInGroup("Enemy");
        foreach (var node in group)
        {
            if (node is Enemy)
            {
                var enemy = (Enemy)node;
                _enemies.Add(enemy);
                enemy.PointsOfInterest = _pointsOfInterest;
                enemy.FindNewTarget();

            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		
	}
}
