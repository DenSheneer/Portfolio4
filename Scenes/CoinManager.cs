using Godot;
using System;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;

public partial class CoinManager : Node
{
    private List<Point_Of_Interest> _pointsOfInterest = new List<Point_Of_Interest>();

    [Export] AudioStreamPlayer3D _audioStream;
    [Export] PackedScene _coinScene;
    public Action<int> ScoreChanged;

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
        if (_initialized)
            increaseScore();

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

    private void increaseScore()
    {
        _audioStream.Play();
        _collectedCoins++;
        ScoreChanged?.Invoke(_collectedCoins);
        GD.Print($"{_collectedCoins} coins");
    }
}
