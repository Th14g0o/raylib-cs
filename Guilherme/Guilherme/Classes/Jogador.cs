using Guilherme.Interfaces;
using Guilherme.Uteis;
using Raylib_cs;
using System.Numerics;

namespace Guilherme.Classes
{
    public class Jogador : SpriteColisao
    {
        private Texture2D spriteSheet;
        private Vector2 posicao;
        private Rectangle frameAtual;
        private int spriteAtual;
        private int frameContador;
        private int frameVelocidade;

        private const int frameVelocidadeMaxima = 30;
        private const int frameVelocidadeMinima = 1;

        private List<EstadoJogador> estado;
        private int velocidadeMovimento;
        private DirecaoMovimento direcao;
        private Vector2 spriteTam;

        public Jogador(float x = 0f, float y = 0f)
        {
            Init();
            Reposicionar(x, y);
        }

        private void Reposicionar(float x, float y)
        {
            posicao = new Vector2(x - spriteTam.X, y);
        }

        private void Init()
        {
            spriteSheet = Raylib.LoadTexture("Conteudos/SpriteSheets/Jogador.png");
            estado = new List<EstadoJogador>();
            estado.Add(EstadoJogador.Parado);
            direcao = DirecaoMovimento.Direita;
            spriteTam = new Vector2(128, 128);
            velocidadeMovimento = 5;

            posicao = new(0f, 0f);
            frameAtual = new(0.0f, 0.0f, (float)spriteSheet.Width / 94, spriteSheet.Height);
            spriteAtual = 0;

            frameContador = 0;
            frameVelocidade = 16;
        }

        private void PassarFrames()
        {
            frameContador++;

            if (frameContador >= Global.FPS / frameVelocidade)
            {
                frameContador = 0;
                spriteAtual++;

                if (estado.Contains(EstadoJogador.Parado) && estado.Count < 2 && spriteAtual > 7) spriteAtual = 0;
                else if (estado.Contains(EstadoJogador.Andando) && estado.Count < 2 && (spriteAtual > 15 || spriteAtual < 8)) spriteAtual = 8;
                else if (estado.Contains(EstadoJogador.Correndo) && estado.Count < 2 && (spriteAtual > 23 || spriteAtual < 16)) spriteAtual = 16;

                frameAtual.X = spriteAtual * frameAtual.Width;
            }

            frameVelocidade = Math.Clamp(frameVelocidade, frameVelocidadeMinima, frameVelocidadeMaxima);
        }

        private bool AddEstado(EstadoJogador estado)
        {
            if (this.estado.Contains(estado)) return true;
            else if (estado == EstadoJogador.Correndo)
            {
                if (this.estado.Contains(EstadoJogador.Andando) || this.estado.Contains(EstadoJogador.Parado))
                {
                    this.estado.Remove(EstadoJogador.Andando);
                    this.estado.Remove(EstadoJogador.Parado);
                    this.estado.Add(estado);
                    return true;
                }
            }
            else if (estado == EstadoJogador.Andando)
            {
                if (this.estado.Contains(EstadoJogador.Parado) || this.estado.Contains(EstadoJogador.Correndo))
                {
                    this.estado.Remove(EstadoJogador.Parado);
                    this.estado.Remove(EstadoJogador.Correndo);
                    this.estado.Add(estado);

                    return true;
                }
            }
            else if (estado == EstadoJogador.Parado)
            {
                if (this.estado.Contains(EstadoJogador.Correndo) ||
                    this.estado.Contains(EstadoJogador.Andando) ||
                    this.estado.Contains(EstadoJogador.Agachando) ||
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

        private void Movimentacao()
        {
            float velocidade = 0;
            int velocidadeFator = 1;

            if (Raylib.IsKeyDown(KeyboardKey.LeftShift))
            {
                AddEstado(EstadoJogador.Correndo);
                velocidadeFator = 2;
            }

            if (Raylib.IsKeyDown(KeyboardKey.A) || Raylib.IsKeyDown(KeyboardKey.Left))
            {
                velocidade = -velocidadeMovimento * velocidadeFator;
                direcao = DirecaoMovimento.Esquerda;
                if (velocidadeFator == 1) AddEstado(EstadoJogador.Andando);
            }
            else if (Raylib.IsKeyDown(KeyboardKey.D) || Raylib.IsKeyDown(KeyboardKey.Right))
            {
                velocidade = velocidadeMovimento * velocidadeFator;
                direcao = DirecaoMovimento.Direita;
                if (velocidadeFator == 1) AddEstado(EstadoJogador.Andando);
            }
            else
            {
                AddEstado(EstadoJogador.Parado);
            }

            posicao.X += velocidade;
        }

        public Rectangle CaixaColisao()
        {
            return new Rectangle(posicao.X, posicao.Y, spriteTam.X, spriteTam.Y);
        }

        public void Update()
        {
            Movimentacao();
            PassarFrames();

            Rectangle frameExibido = frameAtual;
            if (direcao == DirecaoMovimento.Esquerda)
                frameExibido.Width = -frameExibido.Width;

            Rectangle spriteExibida = new Rectangle(posicao.X, posicao.Y, spriteTam.X, spriteTam.Y);
            Raylib.DrawTexturePro(spriteSheet, frameExibido, spriteExibida, Vector2.Zero, 0, Color.White);
        }

        private void Unload()
        {
            Raylib.UnloadTexture(spriteSheet);
        }

    }
}
