using System.ComponentModel;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace AeroHockey.Game;

public class Game
{
    private PlayingField _playingField;
    private RenderWindow _renderWindow;

    private Text leftScoreText;
    private Text rightScoreText;
    private Font textFont;
    private string solutionPath;
    private string localFontPath = "\\Fonts\\ARIAL.TTF";

    private float rightRacketMoveDirection = 0;
    private float leftRacketMoveDirection = 0;

    private int leftScore = 0;
    private int rightScore = 0;

    private bool leftJustScored = false;
    private bool rightJustScored = false;
    
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
        solutionPath = GetSolutionPath();
        
        _playingField.Initialize();

        _renderWindow = new RenderWindow(new VideoMode(_playingField.Width, _playingField.Height), "AeroHockey");
        _renderWindow.Closed += WindowClosed;
        
        textFont = new Font(solutionPath + localFontPath);

        InitText(ref leftScoreText);
        InitText(ref rightScoreText);

        leftScoreText.Position = new Vector2f(25, 20);
        rightScoreText.Position = new Vector2f(_playingField.Width - 10, 20);
    }

    private void InitText(ref Text text)
    {
        text = new Text(leftScore.ToString(), textFont, 30);
        text.FillColor = new Color(120, 120, 120);
        text.Origin = new Vector2f(15, 15);
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

        bool scoredGoal = HandleGateCollisions(ball);

        if (!scoredGoal)
        {
            if (IsLeftBorderCollision(ball) || IsRightBorderCollision(ball)) // Left Right borders
                direction.X = -direction.X;

            if (ballPosition.Y - ball.Radius < 0 || ballPosition.Y + ball.Radius > _playingField.Height) // Top Bottom borders
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
        return ball.Position.X + ball.Radius > _playingField.Width;
    }

    private bool IsWithinGateHeight(CircleShape ball)
    {
        return ball.Position.Y > _playingField.TopGateBorder && ball.Position.Y < _playingField.BottomGateBorder;
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
        if (leftJustScored)
        {
            leftScore++;
            leftJustScored = false;
            leftScoreText.DisplayedString = leftScore.ToString();

            ResetField();
        }
        
        if (rightJustScored)
        {
            rightScore++;
            rightJustScored = false;
            rightScoreText.DisplayedString = rightScore.ToString();

            ResetField();
        }
    }

    private void ResetField()
    {
        _playingField = new PlayingField();
        _playingField.Initialize();
    }

    private void Output()
    {
        _renderWindow.Clear(new Color(20, 20, 20));

        foreach (var shape in _playingField.ShapesToDisplay)
        {
            _renderWindow.Draw(shape);
        }
        
        _renderWindow.Draw(leftScoreText);
        _renderWindow.Draw(rightScoreText);
        
        _renderWindow.Display();
        
        Thread.Sleep(1);
    }
    
    void WindowClosed(object sender, EventArgs e)
    {
        RenderWindow w = (RenderWindow)sender;
        w.Close();
    }
    
    static string? GetSolutionPath()
    {
        string? currentDirectory = Directory.GetCurrentDirectory();

        while (!string.IsNullOrEmpty(currentDirectory))
        {
            if (Directory.GetFiles(currentDirectory, "*.sln").Length > 0)
            {
                return currentDirectory;
            }

            currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
        }

        return null;
    }
}