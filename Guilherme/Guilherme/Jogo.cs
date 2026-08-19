using Raylib_cs;

namespace Guilherme;

internal static class Jogo
{

    public static Jogador jogador = new Jogador(0, 0);

    public static void Rodar()
    {
        jogador.desenhar();
    }

    // STAThread is required if you deploy using NativeAOT on Windows
    // See https://github.com/raylib-cs/raylib-cs/issues/301
    [System.STAThread]
    public static void Main()
    {
        Raylib.InitWindow(Global.larguraTela, Global.alturaTela, "Guilherme");

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