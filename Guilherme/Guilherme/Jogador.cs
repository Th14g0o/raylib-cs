using Raylib_cs;
using System.Numerics;

namespace Guilherme
{
    internal class Jogador
    {
        public List<EstadoJogador> estado;

        public Texture2D spriteSheet = Raylib.LoadTexture("~/Conteudos/SpriteSheets/Jogador.png");

        public Rectangle framePosicionamento;

        public const float linha = 0;
        public float coluna;
        public Vector2 spriteTamanho;
        public Vector2 spriteTamanhoPosicionamento;

        public float animacaoVelocidade;
        public float velocidadeMovimento;

        public Jogador(float x, float y)
        {
            this.estado = new List<EstadoJogador>();
            this.estado.Add(EstadoJogador.Parado);
            this.coluna = 0;
            this.spriteTamanho = new Vector2(64, 64);
            this.spriteTamanhoPosicionamento = new Vector2(100, 100);
            this.animacaoVelocidade = 0.3f;
            this.velocidadeMovimento = 0.3f;
            this.framePosicionamento = new Rectangle(new Vector2(x, y), this.spriteTamanhoPosicionamento);
        }

        public Rectangle FrameAtual()
        {
            return new Rectangle(new Vector2(this.coluna * spriteTamanho.X, linha * spriteTamanho.Y), spriteTamanho);
        }

        public void Animar()
        {
            this.coluna += this.animacaoVelocidade;
            if (this.estado.Contains(EstadoJogador.Parado) && this.estado.Count < 2 && (int)this.coluna > 7) this.coluna = 0;
            else if (this.estado.Contains(EstadoJogador.Andando) && this.estado.Count < 2 && (int)this.coluna > 15) this.coluna = 8;
            else if (this.estado.Contains(EstadoJogador.Correndo) && this.estado.Count < 2 && (int)this.coluna > 23) this.coluna = 16;
        }

        public bool AddEstado(EstadoJogador estado)
        {
            if (this.estado.Contains(estado)) return true;
            else if (estado == EstadoJogador.Andando)
            {
                if (this.estado.Contains(EstadoJogador.Parado))
                {
                    this.estado.Remove(EstadoJogador.Parado);
                    this.estado.Add(estado);
                    return true;
                }
            }
            else if (estado == EstadoJogador.Parado)
            {
                if (this.estado.Contains(EstadoJogador.Correndo) ||
                    this.estado.Contains(EstadoJogador.Andando) ||
                    this.estado.Contains(EstadoJogador.Agachando) ||
                    this.estado.Contains(EstadoJogador.Caindo) ||
                    this.estado.Contains(EstadoJogador.Atacando))
                {
                    this.estado.Remove(EstadoJogador.Correndo);
                    this.estado.Remove(EstadoJogador.Andando);
                    this.estado.Remove(EstadoJogador.Agachando);
                    this.estado.Remove(EstadoJogador.Caindo);
                    this.estado.Remove(EstadoJogador.Atacando);
                    this.estado.Add(estado);
                    return true;
                }
            }
            return false;
        }

        public void Movimento()
        {
            float velocidade = 0;
            if (this.AddEstado(EstadoJogador.Andando))
            {
                
                if (Raylib.IsKeyDown(KeyboardKey.A) || Raylib.IsKeyDown(KeyboardKey.Left))
                {
                    velocidade = this.velocidadeMovimento * -1;
                }
                else if (Raylib.IsKeyDown(KeyboardKey.D) || Raylib.IsKeyDown(KeyboardKey.Right))
                {
                    velocidade += this.velocidadeMovimento;
                }
                this.framePosicionamento.X += velocidade;

            }
            if (velocidade == 0) this.AddEstado(EstadoJogador.Parado);
            else AddEstado(EstadoJogador.Andando);
        }

        public void desenhar()
        {
            this.Movimento();
            this.Animar();
            Raylib.DrawTexturePro(
                this.spriteSheet,
                this.FrameAtual(),
                this.framePosicionamento,
                Vector2.Zero,
                0,
                Color.White
            );
        }
    }
}
