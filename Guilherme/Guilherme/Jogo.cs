using Guilherme.Classes;
using Guilherme.Uteis;
using Raylib_cs;

namespace Guilherme;

internal static class Jogo
{

    public static Jogador jogador;

    public static void Rodar()
    {
        jogador.Update();
    }

    // STAThread is required if you deploy using NativeAOT on Windows
    // See https://github.com/raylib-cs/raylib-cs/issues/301
    [System.STAThread]
    public static void Main()
    {
        Raylib.InitWindow(Global.larguraTela, Global.alturaTela, "Guilherme");

        Raylib.SetTargetFPS(Global.FPS);

        jogador = new Jogador(Global.larguraTela / 2, (float)(Global.alturaTela * 0.8));

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