using Raylib_cs;
using System.Numerics;

namespace Navinha
{
    class Inimigo
    {
        public Rectangle colisao;
        private float velocidade;
        private Color cor = new Color(215, 232, 215, 255);
        public  bool destruir = false;

        public Inimigo(int fatorAltura = 1, int fatorVelocidade = 1, float velocidade = 0.1f)
        {
            int x = Random.Shared.Next(0, Constantes.LarguraTela + 1 - (int)this.colisao.Width);
            int y = -(int)this.colisao.Height * fatorAltura;
            Vector2 posicao = new Vector2(x, y);
            Vector2 tamanho = new Vector2(25, 25);
            this.colisao = new Rectangle(posicao, tamanho);
            this.velocidade = velocidade * fatorVelocidade;
        }

        public void Colidiu()
        {
            this.destruir = true;
        }

        public void Descendo()
        {
            this.colisao.Y += this.velocidade;
            if (this.colisao.Y > Constantes.AlturaTela + 1) this.destruir = true;
        }


        public void Desenhar()
        {
            this.Descendo();
            Raylib.DrawRectangle((int)this.colisao.X, (int)this.colisao.Y, (int)this.colisao.Width, (int)this.colisao.Height, this.cor);
        }
    }
}
