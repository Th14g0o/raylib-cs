using Guilherme.Interfaces;
using Guilherme.Uteis;
using Raylib_cs;
using System.Numerics;

namespace Guilherme.Classes
{
    public class Jogador : ISprite
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
        public Vector2 spriteTam;
        private int gravidadeAplicada;

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
            gravidadeAplicada = 0;
        }

        private void PassarFrames()
        {
            this.frameContador++;

            if (this.frameContador >= Global.FPS / this.frameVelocidade)
            {
                this.frameContador = 0;
                this.spriteAtual++;

                if (this.estado.Contains(EstadoJogador.Agachando))
                {
                    if (this.spriteAtual < 24) this.spriteAtual = 24;
                    else if (this.spriteAtual > 26) this.spriteAtual = 26;
                }

                if (!(!this.estado.Contains(EstadoJogador.Agachando) && spriteAtual > 26 && spriteAtual < 29))
                {
                    if (this.estado.Contains(EstadoJogador.Parado) && this.estado.Count < 2 && this.spriteAtual > 7) 
                        this.spriteAtual = 0;
                    else if (this.estado.Contains(EstadoJogador.Andando) && this.estado.Count < 2 && (this.spriteAtual > 15 || this.spriteAtual < 8)) 
                        this.spriteAtual = 8;
                    else if (this.estado.Contains(EstadoJogador.Correndo) && this.estado.Count < 2 && (this.spriteAtual > 23 || this.spriteAtual < 16)) 
                        this.spriteAtual = 16;
                }

                this.frameAtual.X = this.spriteAtual * this.frameAtual.Width;
            }

            this.frameVelocidade = Math.Clamp(this.frameVelocidade, frameVelocidadeMinima, frameVelocidadeMaxima);
        }

        private bool AddEstado(EstadoJogador estado)
        {
            if (this.estado.Contains(estado)) return true;
            else if (estado == EstadoJogador.Agachando)
            {
                if (this.estado.Contains(EstadoJogador.Andando) || this.estado.Contains(EstadoJogador.Correndo) ||
                    this.estado.Contains(EstadoJogador.Parado))
                {
                    this.estado.Remove(EstadoJogador.Andando);
                    this.estado.Remove(EstadoJogador.Correndo);
                    this.estado.Remove(EstadoJogador.Parado);
                    this.estado.Add(estado);
                    return true;
                }
            }
            else if (estado == EstadoJogador.Correndo)
            {
                if (this.estado.Contains(EstadoJogador.Andando) || this.estado.Contains(EstadoJogador.Parado) ||
                    this.estado.Contains(EstadoJogador.Agachando))
                {
                    this.estado.Remove(EstadoJogador.Andando);
                    this.estado.Remove(EstadoJogador.Parado);
                    this.estado.Remove(EstadoJogador.Agachando);
                    this.estado.Add(estado);
                    return true;
                }
            }
            else if (estado == EstadoJogador.Andando)
            {
                if (this.estado.Contains(EstadoJogador.Parado) || this.estado.Contains(EstadoJogador.Correndo) ||
                    this.estado.Contains(EstadoJogador.Agachando))
                {
                    this.estado.Remove(EstadoJogador.Parado);
                    this.estado.Remove(EstadoJogador.Correndo);
                    this.estado.Remove(EstadoJogador.Agachando);
                    this.estado.Add(estado);

                    return true;
                }
            }
            else if (estado == EstadoJogador.Parado)
            {
                if (this.estado.Contains(EstadoJogador.Correndo) || this.estado.Contains(EstadoJogador.Andando) ||
                    this.estado.Contains(EstadoJogador.Agachando))
                {
                    this.estado.Remove(EstadoJogador.Correndo);
                    this.estado.Remove(EstadoJogador.Andando);
                    this.estado.Remove(EstadoJogador.Agachando);

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

            if (Raylib.IsKeyDown(KeyboardKey.LeftShift) || Raylib.IsKeyDown(KeyboardKey.RightShift))
            {
                AddEstado(EstadoJogador.Correndo);
                velocidadeFator = 2;
            }

            if (Raylib.IsKeyDown(KeyboardKey.S) || Raylib.IsKeyDown(KeyboardKey.Down))
            {
                AddEstado(EstadoJogador.Agachando);
            }
            else if (Raylib.IsKeyDown(KeyboardKey.A) || Raylib.IsKeyDown(KeyboardKey.Left))
            {
                velocidade = -velocidadeMovimento * velocidadeFator;
                this.direcao = DirecaoMovimento.Esquerda;
                if (velocidadeFator == 1) AddEstado(EstadoJogador.Andando);
            }
            else if (Raylib.IsKeyDown(KeyboardKey.D) || Raylib.IsKeyDown(KeyboardKey.Right))
            {
                velocidade = velocidadeMovimento * velocidadeFator;
                this.direcao = DirecaoMovimento.Direita;
                if (velocidadeFator == 1) AddEstado(EstadoJogador.Andando);
            }
            else
            {
                AddEstado(EstadoJogador.Parado);
            }

            this.posicao.X += velocidade;
        }

        public Rectangle SpriteDimensionada()
        {
            return new Rectangle(this.posicao.X, this.posicao.Y, this.spriteTam.X, this.spriteTam.Y);
        }

        public Rectangle CaixaColisao()
        {
            Rectangle caixa = new Rectangle((int)(this.posicao.X + 45), (int)this.posicao.Y + 40, (int)(this.spriteTam.X * 0.3), (int)(this.spriteTam.Y - 70));
            //Raylib.DrawRectangle((int)caixa.X, (int)caixa.Y, (int)caixa.Width, (int)caixa.Height, Color.Green);
            return caixa;
        }

        public void Update()
        {
            this.Gravidade();
            this.Movimentacao();
            this.PassarFrames();

            Rectangle frameExibido = frameAtual;
            if (this.direcao == DirecaoMovimento.Esquerda)
                frameExibido.Width = -frameExibido.Width;

            Raylib.DrawTexturePro(this.spriteSheet, frameExibido, this.SpriteDimensionada(), Vector2.Zero, 0, Color.White);
        }

        private void Gravidade()
        {
            this.posicao.Y -= this.gravidadeAplicada;
        }

        public void GravidadeAplicada(int gravidadeAplicada)
        {
            this.gravidadeAplicada = gravidadeAplicada;
        }

        private void Unload()
        {
            Raylib.UnloadTexture(spriteSheet);
        }

    }
}
