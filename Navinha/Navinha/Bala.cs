using Raylib_cs;
using System.Numerics;

namespace Navinha
{
    class Bala
    {
        public Rectangle colisao;
        private float velocidade;
        private Color cor = new Color(255, 152, 2, 255);
        public bool destruir = false;

        public Bala(Vector2 posicaoParam, float velocidade = 0.3f)
        {
            Vector2 tam = new Vector2(10, 10);
            Vector2 posicao = posicaoParam;
            posicao.Y -= tam.Y;
            posicao.X -= tam.X / 2;
            this.colisao = new Rectangle(posicao, tam);

            this.destruir = false;
            this.velocidade = velocidade;
        }

        public void Colidiu()
        {
            this.destruir = true;
        }

        public void Desenhar()
        {
            this.colisao.Y -= this.velocidade;
            Raylib.DrawRectangle((int)this.colisao.X, (int)this.colisao.Y, (int)this.colisao.Width, (int)this.colisao.Height, this.cor);
            if (this.colisao.Y - this.colisao.Height < 0) this.destruir = true;
        }
    }
}
