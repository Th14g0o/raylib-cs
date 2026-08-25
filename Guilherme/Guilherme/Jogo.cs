using Guilherme.Classes;
using Guilherme.Interfaces;
using Guilherme.Uteis;
using Raylib_cs;

namespace Guilherme;

internal static class Jogo
{
    public const int gravidade = -5;
    public static Jogador jogador;
    public static List<ISprite> sprites;

    public static void Rodar()
    {
        jogador.GravidadeAplicada(gravidade);
        foreach (ISprite sprite in sprites) 
        {
            sprite.Update();
            jogador.VerificaColisao(sprite);
        }
  
        jogador.Update();
    }

    public static void Cenario()
    {
        sprites = new List<ISprite>();
        for (int i = 0; i < 6; i++)
        {
            int tamX = 150;
            int tamY = 50;
            CorpoFisico a = new CorpoFisico(tamX * i, (int)(Global.alturaTela * 0.6 + jogador.spriteTam.Y - tamY / 2), tamX, tamY / 2, Color.Green);
            CorpoFisico b = new CorpoFisico(tamX * i, (int)(Global.alturaTela * 0.6 + jogador.spriteTam.Y), tamX, tamY, Color.DarkBrown);
            sprites.Add(a);
            sprites.Add(b);

        }

        for (int i = 0; i < 4; i++)
        {
            int tamX = 150;
            int tamY = 50;
            if (i % 2 == 0){
                CorpoFisico a = new CorpoFisico(tamX * i, (int)(Global.alturaTela * 0.4 + jogador.spriteTam.Y - tamY / 2), tamX, tamY / 2, Color.Green);
                CorpoFisico b = new CorpoFisico(tamX * i, (int)(Global.alturaTela * 0.4 + jogador.spriteTam.Y), tamX, tamY, Color.DarkBrown);
                sprites.Add(a);
                sprites.Add(b);
            }
        }
    }

    // STAThread is required if you deploy using NativeAOT on Windows
    // See https://github.com/raylib-cs/raylib-cs/issues/301
    [System.STAThread]
    public static void Main()
    {
        Raylib.InitWindow(Global.larguraTela, Global.alturaTela, "Guilherme");

        Raylib.SetTargetFPS(Global.FPS);

        jogador = new Jogador(Global.larguraTela / 2, 0);
        Cenario();

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Blue);

            Rodar();

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}