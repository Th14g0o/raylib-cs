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

        Raylib.SetTargetFPS(60);

        jogador = new Jogador();
        jogador.Init();


        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            Rodar();

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}