using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Navinha
{
    class Placar
    {
        private float pontos;
        private int pontosMaximo;
        private float velocidade;
        private Vector2 posicao;

        public int GetPontos()
        {
            return (int)this.pontos;
        }

        public Placar(float x, float y, float velocidade = 0.05f)
        {
            this.pontosMaximo = 0;
            this.pontos = 0;
            this.velocidade = velocidade;
            this.posicao = new Vector2(x,y);
        }

        public void Resetar() {
            if (this.pontos > 0) this.pontosMaximo = (int)this.pontos; 
            this.pontos = 0;
        }

        public void Contar()
        {
            this.pontos += this.velocidade;
            this.Mostrar();
        }

        public void Mostrar()
        {
            Raylib.DrawText($"Pontos: {(int)this.pontos}".PadLeft(5), (int)this.posicao.X, (int)this.posicao.Y, 20, Color.Yellow);
        }

    }
}
