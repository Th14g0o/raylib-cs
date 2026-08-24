using Raylib_cs;
using System.Numerics;

namespace Guilherme
{
    internal class Jogador
    {
        public List<EstadoJogador> estado;

        private Texture2D scarfy;
        private Vector2 position;
        private Rectangle frameRec;
        private int currentFrame;
        private int framesCounter;
        private int framesSpeed;

        public const int MaxFrameSpeed = 15;
        public const int MinFrameSpeed = 1;

        public void Init()
        {
            // NOTE: Textures MUST be loaded after Window initialization (OpenGL context is required)
            scarfy = Raylib.LoadTexture("Conteudos/SpriteSheets/Jogador.png");        // Texture loading

            position = new(350.0f, 280.0f);
            frameRec = new(0.0f, 0.0f, (float)scarfy.Width / 94, (float)scarfy.Height);
            currentFrame = 0;

            framesCounter = 0;
            framesSpeed = 8;            // Number of spritesheet frames shown by second
        }

        public void Update()
        {
            // Update
            //----------------------------------------------------------------------------------
            framesCounter++;

            if (framesCounter >= (60 / framesSpeed))
            {
                framesCounter = 0;
                currentFrame++;

                if (currentFrame > 5)
                {
                    currentFrame = 0;
                }

                frameRec.X = (float)currentFrame * (float)scarfy.Width / 6;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Right))
            {
                framesSpeed++;
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.Left))
            {
                framesSpeed--;
            }

            framesSpeed = Math.Clamp(framesSpeed, MinFrameSpeed, MaxFrameSpeed);
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            Raylib.DrawTexture(scarfy, 15, 40, Color.White);
            Raylib.DrawRectangleLines(15, 40, scarfy.Width, scarfy.Height, Color.Lime);
            Raylib.DrawRectangleLines(
                15 + (int)frameRec.X,
                40 + (int)frameRec.Y,
                (int)frameRec.Width,
                (int)frameRec.Height,
                Color.Red
            );

            Raylib.DrawText("FRAME SPEED: ", 165, 210, 10, Color.DarkGray);
            Raylib.DrawText($"{framesSpeed:D2} FPS", 575, 210, 10, Color.DarkGray);
            Raylib.DrawText("PRESS RIGHT/LEFT KEYS to CHANGE SPEED!", 290, 240, 10, Color.DarkGray);

            for (var i = 0; i < MaxFrameSpeed; i++)
            {
                if (i < framesSpeed)
                {
                    Raylib.DrawRectangle(250 + 21 * i, 205, 20, 20, Color.Red);
                }
                Raylib.DrawRectangleLines(250 + 21 * i, 205, 20, 20, Color.Maroon);
            }

            Raylib.DrawTextureRec(scarfy, frameRec, position, Color.White);  // Draw part of the texture

            Raylib.DrawText("(c) Scarfy sprite by Eiden Marsal", Global.larguraTela - 200, Global.alturaTela - 20, 10, Color.Gray);

            //----------------------------------------------------------------------------------
        }

        public void Unload()
        {
            Raylib.UnloadTexture(scarfy);       // Texture unloading
        }

    }
}
