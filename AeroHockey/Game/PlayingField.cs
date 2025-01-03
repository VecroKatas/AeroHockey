﻿using SFML.Graphics;
using SFML.System;

namespace AeroHockey.Game;

public class PlayingField
{
    public static readonly uint Width = 800;
    public static readonly uint Height = 400;
    
    private uint BallRadius = 10;
    private int BallSpeed = 4;

    public float TopGateBorder { get; private set; } = 125;
    public float BottomGateBorder { get; private set; } = 275;
    
    public CircleShape Ball { get; private set; }
    public Vector2f BallMoveDirection { get; set; } = new Vector2f(1, .7f);
    
    public RectangleShape RightRacket { get; private set; }
    public RectangleShape LeftRacket { get; private set; }
    private readonly int RacketWidth = 10;
    private readonly int RacketHeight = 100;
    private readonly int RacketSpeed = 5;

    private RectangleShape topLeftGateBorder;
    private RectangleShape bottomLeftGateBorder;
    private RectangleShape topRightGateBorder;
    private RectangleShape bottomRightGateBorder;

    public List<Shape> ShapesToDisplay;

    private Random _random = new Random();

    public PlayingField()
    {
        ShapesToDisplay = new List<Shape>();
    }

    public void Initialize()
    {
        InitializeGates();
        
        InitializeBall();

        InitializeRackets();
    }

    public void Reset()
    {
        ShapesToDisplay.Clear();
        Initialize();
    }

    private void InitializeGates()
    {
        InitGateBorder(ref topLeftGateBorder);
        InitGateBorder(ref bottomLeftGateBorder);
        InitGateBorder(ref topRightGateBorder);
        InitGateBorder(ref bottomRightGateBorder);
        
        topLeftGateBorder.Position = new Vector2f(0, TopGateBorder);
        bottomLeftGateBorder.Position = new Vector2f(0, BottomGateBorder);
        topRightGateBorder.Position = new Vector2f(Width, TopGateBorder);
        bottomRightGateBorder.Position = new Vector2f(Width, BottomGateBorder);
    }

    private void InitGateBorder(ref RectangleShape border)
    {
        border = new RectangleShape(new Vector2f(10, 6));
        border.Origin = new Vector2f(5, 3);
        border.FillColor = new Color(120, 120, 120);
        
        ShapesToDisplay.Add(border);
    }

    private void InitializeBall()
    {
        Ball = new CircleShape(BallRadius);
        Ball.Origin = new Vector2f(BallRadius, BallRadius);
        Ball.Position = new Vector2f(Width / 2, Height / 2);
        Ball.FillColor = new Color(200, 200, 200);

        Vector2f randomDirection = new Vector2f(GetRandomFloat(), GetRandomFloat());
        BallMoveDirection = randomDirection.NormalizeVector2f();
        
        ShapesToDisplay.Add(Ball);
    }

    private void InitializeRackets()
    {
        RightRacket = InitRacket(RightRacket);
        LeftRacket = InitRacket(LeftRacket);
        
        RightRacket.Position = new Vector2f(Width - 100, Height / 2);
        LeftRacket.Position = new Vector2f(100, Height / 2);
    }

    private RectangleShape InitRacket(RectangleShape racket)
    {
        racket = new RectangleShape(new Vector2f(RacketWidth,RacketHeight));
        racket.Origin = new Vector2f(5, 50);
        racket.FillColor = new Color(160, 160, 160);
        
        ShapesToDisplay.Add(racket);

        return racket;
    }

    private float GetRandomFloat()
    {
        return (float)_random.Next(-100, 101) / 100;
    }

    public void MoveBall()
    {
        Ball.Position += BallMoveDirection * BallSpeed;
    }

    public void MoveRacket(int racketIndex, float direction)
    {
        float delta = direction * RacketSpeed;
        if (racketIndex == 1 && IsRacketWithinBorders(LeftRacket, direction))
        {
            LeftRacket.Position += new Vector2f(0, delta);
        }
        else if (racketIndex == 2 && IsRacketWithinBorders(RightRacket, direction))
        {
            RightRacket.Position += new Vector2f(0, delta);
        }
    }

    private bool IsRacketWithinBorders(RectangleShape racket, float delta)
    {
        return racket.Position.Y + RacketHeight / 2 + delta < Height && racket.Position.Y - RacketHeight / 2 + delta > 0;
    }
}