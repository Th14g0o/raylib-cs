using Raylib_cs;
using System.Collections.Generic;

namespace Navinha;

internal static class Jogo
{
    private static List<Inimigo> inimigos;
    private static Placar placar;
    private static Nave jogador;

    [System.STAThread]

    private static List<Inimigo> GerarInimigo()
    {
        List <Inimigo> inimigosGerados = new List <Inimigo>();
        int qtd = placar.GetPontos() / 2500 + 1;
        if (qtd > 5) qtd = 5;
        qtd = qtd - inimigos.Where(i => !i.destruir).ToList().Count;
        for (int i = 0; i < qtd; i++)
            inimigosGerados.Add(new Inimigo(Random.Shared.Next(1, qtd+1), Random.Shared.Next(1, qtd + 1)));
        return inimigosGerados;
    }


    public static void Jogando()
    {
        Raylib.ClearBackground(Color.Black);

        placar.Contar();
        jogador.Desenhar();

        List<Inimigo> inimigosAdi = new List<Inimigo>();
        foreach (Inimigo i in inimigos)
        {
            i.Desenhar();
            if (i.destruir) inimigosAdi.AddRange(GerarInimigo());
        }
        inimigos.RemoveAll(i => i.destruir);
        inimigos.AddRange(inimigosAdi);
        foreach (Inimigo i in inimigos)
        {
            if (Raylib.CheckCollisionRecs(i.colisao, jogador.colisao))
            {
                i.Colidiu();
                ReiniciarJogo();
            }
            foreach (Bala b in jogador.balas)
            {
                if (Raylib.CheckCollisionRecs(i.colisao, b.colisao))
                {
                    i.Colidiu();
                    b.Colidiu();
                }

            }
        }
    }

    public static void ReiniciarJogo()
    {
        inimigos = new List<Inimigo>();
        jogador = new Nave();
        placar.Resetar();
        inimigos.Add(new Inimigo());
    }

    public static void Main()
    {
        inimigos = new List<Inimigo>();
        jogador = new Nave();
        placar = new Placar(10, 5);
        inimigos.Add(new Inimigo());

        Raylib.InitWindow(Constantes.LarguraTela, Constantes.AlturaTela, "Pseudo jogo de Navinha");        

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();

            Jogando();
            
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}