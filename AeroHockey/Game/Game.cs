using System.Diagnostics;
using SFML.Graphics;
using SFML.System;

namespace AeroHockey.Game;

public class Game
{
    private PlayingField _playingField;
    private Input _input;
    private Output _output;

    private Vector2f rightRacketDirection = new Vector2f(0, 0);
    private Vector2f leftRacketDirection = new Vector2f(0, 0);

    private int leftScore = 0;
    private int rightScore = 0;

    private bool leftJustScored = false;
    private bool rightJustScored = false;

    private Stopwatch _stopwatch;
    private long _deltaTime;
    
    public Game(RenderWindow renderWindow)
    {
        _playingField = new PlayingField();
        _input = new Input(renderWindow);
        _output = new Output(_playingField, renderWindow);
        _stopwatch = new Stopwatch();
    }

    public void StartGame()
    {
        Initialization();

        GameLoop();
    }

    private void Initialization()
    {
        _playingField.Initialize();
        
        _output.Initialize();
        
        _stopwatch.Start();
    }

    private void GameLoop()
    {
        while (GameRunning())
        {
            Input();
            
            Physics();
            
            Logic();
            
            Output();
        }
    }

    private bool GameRunning()
    {
        return _output.IsWindowOpen();
    }

    private void Input()
    {
        _input.DispatchEvents();

        leftRacketDirection = _input.GetLeftRacketDirection();
        rightRacketDirection = _input.GetRightRacketDirection();
    }

    private void Physics()
    {
        MeasureDeltaTime();
        
        MoveBall();

        MoveRackets();
        
        HandleCollisions();
    }

    private void MeasureDeltaTime()
    {
        _deltaTime = _stopwatch.ElapsedMilliseconds;
        _stopwatch.Restart();
    }

    private void MoveBall()
    {
        _playingField.MoveBall(_deltaTime);
    }

    private void MoveRackets()
    {
        _playingField.MoveRacket(1, leftRacketDirection.X, _deltaTime);
        _playingField.MoveRacket(2, rightRacketDirection.X, _deltaTime);
    }

    private void HandleCollisions()
    {
        CircleShape ball = _playingField.Ball;
        
        Vector2f ballPosition = ball.Position;
        Vector2f direction = _playingField.BallMoveDirection;

        RectangleShape leftRacket = _playingField.LeftRacket;
        RectangleShape rightRacket = _playingField.RightRacket;

        bool scoredGoal = HandleGateCollisions(ball);

        if (!scoredGoal)
        {
            if (IsLeftBorderCollision(ball) || IsRightBorderCollision(ball)) // Left Right borders
                direction.X = -direction.X;

            if (ballPosition.Y - ball.Radius < 0 || ballPosition.Y + ball.Radius > PlayingField.Height) // Top Bottom borders
                direction.Y = -direction.Y;

            direction = HandleRacketCollision(leftRacket, ball, direction);
            direction = HandleRacketCollision(rightRacket, ball, direction);

            _playingField.BallMoveDirection = direction;
        }
    }

    private bool HandleGateCollisions(CircleShape ball)
    {
        if (IsWithinGateHeight(ball))
        {
            if (IsLeftBorderCollision(ball))
            {
                rightJustScored = true;
                return true;
            }
            
            if (IsRightBorderCollision(ball))
            {
                leftJustScored = true;
                return true;
            }
        }

        return false;
    }

    private bool IsLeftBorderCollision(CircleShape ball)
    {
        return ball.Position.X - ball.Radius < 0;
    }
    
    private bool IsRightBorderCollision(CircleShape ball)
    {
        return ball.Position.X + ball.Radius > PlayingField.Width;
    }

    private bool IsWithinGateHeight(CircleShape ball)
    {
        return ball.Position.Y > _playingField.TopGateBorder && ball.Position.Y < _playingField.BottomGateBorder;
    }

    private Vector2f HandleRacketCollision(RectangleShape racket, CircleShape ball, Vector2f direction)
    {
        if (ball.Position.X > racket.Position.X - racket.Size.X / 2 && ball.Position.X < racket.Position.X + racket.Size.X / 2) // Vertical collision
        {
            if (ball.Position.Y + ball.Radius > racket.Position.Y - racket.Size.Y / 2 && ball.Position.Y + ball.Radius < racket.Position.Y + racket.Size.Y / 2)
                direction.Y = -MathF.Abs(direction.Y);     // Top collision
            else if (ball.Position.Y - ball.Radius > racket.Position.Y - racket.Size.Y / 2 && ball.Position.Y - ball.Radius < racket.Position.Y + racket.Size.Y / 2)
                direction.Y = MathF.Abs(direction.Y); // Bottom collision
        }
        else if (ball.Position.Y > racket.Position.Y - racket.Size.Y / 2 && ball.Position.Y < racket.Position.Y + racket.Size.Y / 2) // Horizontal collision
        {
            if (ball.Position.X - ball.Radius > racket.Position.X - racket.Size.X / 2 && ball.Position.X - ball.Radius < racket.Position.X + racket.Size.X / 2)
                direction.X = MathF.Abs(direction.X);     // Left collision
            else if (ball.Position.X + ball.Radius > racket.Position.X - racket.Size.X / 2 && ball.Position.X + ball.Radius < racket.Position.X + racket.Size.X / 2)
                direction.X = -MathF.Abs(direction.X); // Right collision
        }

        return direction;
    }

    private void Logic()
    {
        if (leftJustScored)
        {
            leftScore++;
            leftJustScored = false;

            ResetField();
        }
        
        if (rightJustScored)
        {
            rightScore++;
            rightJustScored = false;

            ResetField();
        }
    }

    private void ResetField()
    {
        _playingField.Reset();
    }

    private void Output()
    {
        _output.UpdateScores(leftScore.ToString(), rightScore.ToString());
        _output.Display();
        
        Thread.Sleep(1);
    }
}