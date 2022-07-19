﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.world
{
    public static class StaticParameters
    {
        // Static Universal Parameters
        public const double InfinityD = double.MaxValue;

        // Static Game-wide Parameters
        public const int ScreenWidth = 1280;
        public const int ScreenHeight = 720;

        public const int SpeedFactor = 200;
        public const int DefaultNodeDistance = 20;

        public static readonly Random RandomGenerator = new Random();

        // Static Level Parameters
        public const int MaxHorizontalNodes = (ScreenWidth / StaticParameters.DefaultNodeDistance) - 1; // No nodes on border of the level, with counter starting at 1 this results in -2, for borders on both sides.
        public const int MaxVerticalNodes = (ScreenHeight / StaticParameters.DefaultNodeDistance) - 1;

        public static readonly Vector2D NorthSpawn = new Vector2D(ScreenWidth /2 , DefaultNodeDistance);
        public static readonly Vector2D SouthSpawn = new Vector2D(ScreenWidth /2, ScreenHeight - DefaultNodeDistance);

        public static readonly Vector2D TopLeftNodePos = new Vector2D(DefaultNodeDistance, DefaultNodeDistance);
        public static readonly Vector2D BottomRightNodePos = new Vector2D(ScreenWidth - DefaultNodeDistance, ScreenHeight - DefaultNodeDistance);

        // Static File Locations
        // Sprites
        public const string PlayerSpritePath = "Content/PlayerAgent 12-02-2021 04-44-13.png";
    }
}
