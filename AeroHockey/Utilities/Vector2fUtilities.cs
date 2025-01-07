using SFML.System;

namespace AeroHockey;

public static class Vector2fUtilities
{
    public static Vector2f NormalizeVector2f(this Vector2f vector)
    {
        float magnitude = vector.X * vector.X + vector.Y * vector.Y;
        
        magnitude = MathF.Sqrt(magnitude);
        
        if (magnitude == 0)
        {
            Console.WriteLine("Cannot normalize a zero vector.");
            return new Vector2f(1, 0);
        }

        return new Vector2f(vector.X / magnitude, vector.Y / magnitude);
    }
}