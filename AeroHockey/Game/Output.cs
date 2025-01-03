using SFML.Graphics;
using SFML.System;

namespace AeroHockey.Game;

public class Output
{private PlayingField _playingField;
    private RenderWindow _renderWindow;

    private Text leftScoreText;
    private Text rightScoreText;
    private Font textFont;
    private string solutionPath;
    private string localFontPath = "\\Fonts\\ARIAL.TTF";

    public Output(PlayingField playingField, RenderWindow renderWindow)
    {
        _playingField = playingField;
        _renderWindow = renderWindow;
    }

    public void Initialize()
    {
        solutionPath = GetSolutionPath();
        
        _renderWindow.Closed += WindowClosed;
        
        textFont = new Font(solutionPath + localFontPath);

        InitText(ref leftScoreText);
        InitText(ref rightScoreText);

        leftScoreText.Position = new Vector2f(25, 20);
        rightScoreText.Position = new Vector2f(PlayingField.Width - 10, 20);
    }

    private void InitText(ref Text text)
    {
        text = new Text("0", textFont, 30);
        text.FillColor = new Color(120, 120, 120);
        text.Origin = new Vector2f(15, 15);
    }

    public bool IsWindowOpen()
    {
        return _renderWindow.IsOpen;
    }

    public void Display()
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

    public void UpdateScores(string leftScore, string rightScore)
    {
        leftScoreText.DisplayedString = leftScore;
        rightScoreText.DisplayedString = rightScore;
    }
    
    string? GetSolutionPath()
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
    
    void WindowClosed(object sender, EventArgs e)
    {
        RenderWindow w = (RenderWindow)sender;
        w.Close();
    }
}