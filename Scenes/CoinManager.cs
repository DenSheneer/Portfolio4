using Godot;
using System;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;

public partial class CoinManager : Node
{
    private List<Point_Of_Interest> _pointsOfInterest = new List<Point_Of_Interest>();

    [Export] PackedScene _coinScene;

    private int _collectedCoins = 0;
    private bool _initialized = false;

    public void Initialize(List<Point_Of_Interest> pointOfInterests)
    {
        _pointsOfInterest = pointOfInterests;
        makeNewCoin();
        _initialized = true;
    }

    private void makeNewCoin()
    {
        if (_initialized) { _collectedCoins++; GD.Print($"{_collectedCoins} coins"); }

        var coin = _coinScene.Instantiate<Coin>();
        AddChild(coin);
        var rnd = new Random();
        if (_pointsOfInterest.Count > 0)
        {
            var randomPointLoc = _pointsOfInterest[rnd.Next(0, _pointsOfInterest.Count)].Transform.Origin;
            coin.GlobalPosition = randomPointLoc;
            coin.OnCollected += makeNewCoin;
        }
    }
}
