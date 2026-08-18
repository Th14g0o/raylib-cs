using Raylib_cs;
using System.Numerics;

namespace Navinha
{
    
    class Nave
    {
        public Rectangle colisao;
        private Color cor = new Color(5, 152, 255, 255);
        private float velocidade;
        private float velocidadeX; 
        public List<Bala> balas = new List<Bala>();
        private const int tempoDisparoMaximo = 800;
        private int tempoDisparo = 0;
        

        public Nave()
        {
            Vector2 tam = new Vector2(30, 30);
            Vector2 pos = new Vector2(Constantes.LarguraTela / 2, Constantes.AlturaTela - tam.Y - tam.Y / 2);
            this.colisao = new Rectangle(pos, tam);

            this.velocidade = 0.3f;
            this.velocidadeX = 0;
            this.balas = new List<Bala>();
            
        }
        
        public void Movimento() {
            if (Raylib.IsKeyDown(KeyboardKey.A) || Raylib.IsKeyDown(KeyboardKey.Left))
            {
                this.velocidadeX = this.velocidade * -1;
            }
            else if (Raylib.IsKeyDown(KeyboardKey.D) || Raylib.IsKeyDown(KeyboardKey.Right))
            {
                this.velocidadeX = this.velocidade;;
            }
            else
            {
                this.velocidadeX = 0;
            }

            if (!(this.velocidadeX > 0 && this.colisao.X + this.colisao.Width + this.velocidade > Constantes.LarguraTela) &&
                !(this.velocidadeX < 0 && this.colisao.X - this.velocidade < 0))
                this.colisao.X += this.velocidadeX;
        }

        public void Disparo()
        {
            if ((Raylib.IsKeyDown(KeyboardKey.Enter) || Raylib.IsMouseButtonDown(MouseButton.Left)) && this.tempoDisparo >= tempoDisparoMaximo)
            {
                Bala b = new Bala(new Vector2(this.colisao.X + this.colisao.Width / 2, this.colisao.Y));
                this.balas.Add(b);
                this.tempoDisparo = 0;
            }
            this.tempoDisparo += 1;
            foreach (Bala b in this.balas)
                b.Desenhar();
            this.balas.RemoveAll(bala => bala.destruir);
        }

        public void Desenhar()
        {
            this.Movimento();
            this.Disparo();
            Raylib.DrawRectangle((int) this.colisao.X, (int) this.colisao.Y, (int) this.colisao.Width, (int) this.colisao.Height, this.cor);
        }

    }
}
