using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.world
{
    public static class StaticParameters
    {
        // Static Game-wide Parameters
        public const int ScreenWidth = 1280;
        public const int screenHeight = 720;

        public const int SpeedFactor = 200;

        // Static Level Parameters
        public static readonly Vector2D NorthSpawn = new Vector2D(ScreenWidth /2 , 10);
        public static readonly Vector2D SouthSpawn = new Vector2D(ScreenWidth /2, screenHeight - 10);

        public static readonly Vector2D TopLeftNodePos = new Vector2D(ScreenWidth / 2, screenHeight - 10);
        public static readonly Vector2D BottomRightNodePos = new Vector2D(ScreenWidth / 2, screenHeight - 10);

        // Static File Locations
        // Sprites
        public const string PlayerSpritePath = "Content/PlayerAgent 12-02-2021 04-44-13.png";
    }
}
