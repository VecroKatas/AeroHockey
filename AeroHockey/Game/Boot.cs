using SFML.Graphics;
using SFML.Window;

namespace AeroHockey.Game;

public class Boot
{
    private RenderWindow _renderWindow;

    public Boot()
    {
        _renderWindow = new RenderWindow(new VideoMode(PlayingField.Width, PlayingField.Height), "AeroHockey");
    }

    public void StartGame()
    {
        Game game = new Game(_renderWindow);
        game.StartGame();
    }
}