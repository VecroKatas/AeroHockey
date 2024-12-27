using System.ComponentModel;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace AeroHockey.Game;



public class Game
{
    private PlayingField _playingField;
    private RenderWindow _renderWindow;

    private float rightRacketMoveDirection = 0;
    private float leftRacketMoveDirection = 0;
    
    public Game()
    {
        _playingField = new PlayingField();
    }

    public void StartGame()
    {
        Initialization();

        GameLoop();
    }

    private void Initialization()
    {
        _playingField.Initialize();

        _renderWindow = new RenderWindow(new VideoMode(_playingField.Width, _playingField.Height), "AeroHockey");
        _renderWindow.Closed += WindowClosed;
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
        return _renderWindow.IsOpen;
    }

    private void Input()
    {
        _renderWindow.DispatchEvents();
        
        if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            leftRacketMoveDirection = -1;
        else if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            leftRacketMoveDirection = 1;
        else
            leftRacketMoveDirection = 0;
        
        if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
            rightRacketMoveDirection = -1;
        else if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
            rightRacketMoveDirection = 1;
        else
            rightRacketMoveDirection = 0;
    }

    private void Physics()
    {
        MoveBall();

        MoveRackets();
        
        HandleCollisions();
    }

    private void MoveRackets()
    {
        _playingField.MoveRacket(1, rightRacketMoveDirection);
        _playingField.MoveRacket(2, leftRacketMoveDirection);
    }

    private void MoveBall()
    {
        _playingField.MoveBall();
    }

    private void HandleCollisions()
    {
        CircleShape ball = _playingField.Ball;
        
        Vector2f ballPosition = ball.Position;
        Vector2f direction = _playingField.BallMoveDirection;

        RectangleShape leftRacket = _playingField.LeftRacket;
        RectangleShape rightRacket = _playingField.RightRacket;

        if (ballPosition.X - ball.Radius < 0 || ballPosition.X + ball.Radius > _playingField.Width) // Left Right borders
            direction.X = -direction.X;
        
        if (ballPosition.Y - ball.Radius < 0 || ballPosition.Y + ball.Radius > _playingField.Height) // Top Bottom borders
            direction.Y = -direction.Y;

        direction = HandleRacketCollision(leftRacket, ball, direction);
        direction = HandleRacketCollision(rightRacket, ball, direction);

        _playingField.BallMoveDirection = direction;
    }

    private Vector2f HandleRacketCollision(RectangleShape racket, CircleShape ball, Vector2f direction)
    {
        // Horizontal collision
        if (ball.Position.Y > racket.Position.Y - racket.Size.Y / 2 && ball.Position.Y < racket.Position.Y + racket.Size.Y / 2)
        {
            // Left collision
            if (ball.Position.X - ball.Radius > racket.Position.X - racket.Size.X / 2 && ball.Position.X - ball.Radius < racket.Position.X + racket.Size.X / 2)
                direction.X = -direction.X;
            
            // Right collision
            if (ball.Position.X + ball.Radius > racket.Position.X - racket.Size.X / 2 && ball.Position.X + ball.Radius < racket.Position.X + racket.Size.X / 2)
                direction.X = -direction.X;
        }
        
        // Vertical collision
        if (ball.Position.X > racket.Position.X - racket.Size.X / 2 && ball.Position.X < racket.Position.X + racket.Size.X / 2)
        {
            // Top collision
            if (ball.Position.Y + ball.Radius > racket.Position.Y - racket.Size.Y / 2 && ball.Position.Y + ball.Radius < racket.Position.Y + racket.Size.Y / 2)
                direction.Y = -direction.Y;
            
            // Bottom collision
            if (ball.Position.Y - ball.Radius > racket.Position.Y - racket.Size.Y / 2 && ball.Position.Y - ball.Radius < racket.Position.Y + racket.Size.Y / 2)
                direction.Y = -direction.Y;
        }

        return direction;
    }

    private void Logic()
    {
        
    }

    private void Output()
    {
        _renderWindow.Clear(new Color(20, 20, 20));

        foreach (var shape in _playingField.ShapesToDisplay)
        {
            _renderWindow.Draw(shape);
        }
        
        _renderWindow.Display();
        
        Thread.Sleep(1);
    }
    
    void WindowClosed(object sender, EventArgs e)
    {
        RenderWindow w = (RenderWindow)sender;
        w.Close();
    }
}