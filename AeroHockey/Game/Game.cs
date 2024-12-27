using System.ComponentModel;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace AeroHockey.Game;

public class Game
{
    private PlayingField _playingField;
    private RenderWindow _renderWindow;

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
    }

    private void Physics()
    {
        MoveBall();

        HandleCollisions();
    }

    private void MoveBall()
    {
        _playingField.MoveBall();
    }

    private void HandleCollisions()
    {
        CircleShape ball = _playingField.Ball;
        Vector2f position = ball.Position;
        Vector2f direction = _playingField.BallMoveDirection;

        if (position.X - ball.Radius < 0 || position.X + ball.Radius > _playingField.Width)
            direction.X = -direction.X;
        
        if (position.Y - ball.Radius < 0 || position.Y + ball.Radius > _playingField.Height)
            direction.Y = -direction.Y;

        _playingField.BallMoveDirection = direction;
    }

    private void Logic()
    {
        
    }

    private void Output()
    {
        _renderWindow.Clear(new Color(20, 20, 20));
        
        _renderWindow.Draw(_playingField.Ball);
        _renderWindow.Display();
        
        Thread.Sleep(1);
    }
    
    void WindowClosed(object sender, EventArgs e)
    {
        RenderWindow w = (RenderWindow)sender;
        w.Close();
    }
}