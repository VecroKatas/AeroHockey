using SFML.Graphics;
using SFML.System;

namespace AeroHockey.Game;

public class PlayingField
{
    public uint Width { get; private set; } = 800;
    public uint Height { get; private set; } = 400;
    
    public uint BallRadius { get; private set; } = 10;
    public uint BallSpeed { get; private set; } = 10;
    
    private float TopGateBorder { get; set; }
    private float BottomGateBorder { get; set; }
    
    public CircleShape Ball { get; set; }
    public Vector2f BallMoveDirection { get; set; } = new Vector2f(1, .7f);

    private Random _random = new Random();
    
    public PlayingField(){}

    public void Initialize()
    {
        InitializeBall();
    }

    private void InitializeBall()
    {
        Ball = new CircleShape(BallRadius);
        Ball.Origin = new Vector2f(BallRadius, BallRadius);
        Ball.Position = new Vector2f(Width / 2, Height / 2);
        Ball.FillColor = new Color(200, 200, 200);

        BallMoveDirection = new Vector2f(GetRandomFloat(), GetRandomFloat());
    }

    private float GetRandomFloat()
    {
        return (float)_random.Next(-100, 101) / 100;
    }

    public void MoveBall()
    {
        Ball.Position += BallMoveDirection * BallSpeed;
    }
}