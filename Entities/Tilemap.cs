using Raylib_cs;
using AntsShooter.Systems;
using System.Numerics;
using AntsShooter.Entities;

namespace AntsShooter.Entities
{
    public class Tilemap : Entity
    {
        private const int TILE_SIZE = 32;
        private const int ROWS_NUMBER = (Globals.MAP_WIDTH + Globals.VECTUAL_SCREEN_WIDTH/2) / TILE_SIZE;
        private const int COLS_NUMBER = 5;

        private Texture2D topTileTexture = Raylib.LoadTexture("assets/tiles/top_tile.png");
        private Texture2D tileTexture = Raylib.LoadTexture("assets/tiles/tile.png");

        public Tilemap() : base()
        {

        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw()
        {
            base.Draw();

            for (int i = 0; i < COLS_NUMBER; i++)
            {
                for (int j = 0; j < ROWS_NUMBER; j++)
                {
                    if (i == 0)
                    {
                        Raylib.DrawTexture(
                            topTileTexture,
                            (int)(TILE_SIZE * j),
                            (int)(Globals.GROUND_LEVEL),
                            Color.White
                        );
                    }
                    else
                    {
                        Raylib.DrawTexture(
                            tileTexture,
                            (int)(TILE_SIZE * j),
                            (int)(Globals.GROUND_LEVEL + TILE_SIZE * i),
                            Color.White
                        );
                    }
                }
            }
        }
    }
}
