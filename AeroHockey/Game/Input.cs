using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace AeroHockey.Game;

public class Input
{
    private RenderWindow _renderWindow;

    public Input(RenderWindow renderWindow)
    {
        _renderWindow = renderWindow;
    }

    public void Initialize()
    {
        
    }

    public void DispatchEvents()
    {
        _renderWindow.DispatchEvents();
    }

    public Vector2f GetLeftRacketDirection()
    {
        if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            return new Vector2f(-1f, 0);
        
        if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            return new Vector2f(1f, 0);
        
        return new Vector2f(0, 0);
    }
    
    public Vector2f GetRightRacketDirection()
    {
        if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
            return new Vector2f(-1f, 0);
        
        if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
            return new Vector2f(1f, 0);
        
        return new Vector2f(0, 0);
    }
}