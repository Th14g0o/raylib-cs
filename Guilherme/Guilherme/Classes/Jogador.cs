using Guilherme.Interfaces;
using Guilherme.Uteis;
using Raylib_cs;
using System.Numerics;

namespace Guilherme.Classes
{
    public class Pulo
    {
        private int alturaMaxima;
        private int velocidade;
        private int alturaAtual;
        public Pulo(int alturaMaxima, int velocidade)
        {
            this.alturaMaxima = alturaMaxima;
            this.velocidade = velocidade;
            this.alturaAtual = velocidade;
        }

        public int pular()
        {
            if (this.alturaAtual <= this.alturaMaxima + this.velocidade)
            {
                this.alturaAtual += this.velocidade;
                return this.velocidade;
            }
            return 0;
        }

        public void resetar()
        {
            this.alturaAtual = this.velocidade;
        }
    }

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
        private int velocidadeAtual = 0;
        private Pulo pulo = new Pulo(120, 5);

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
            estado.Add(EstadoJogador.Caindo);
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
                    this.spriteAtual = Math.Clamp(this.spriteAtual, 24, 26);

                if (this.estado.Contains(EstadoJogador.Pulando))
                    this.spriteAtual = Math.Clamp(this.spriteAtual, 30, 33);

                if (this.estado.Contains(EstadoJogador.Caindo))
                    this.spriteAtual = Math.Clamp(this.spriteAtual, 33, 36);

                if (!(!this.estado.Contains(EstadoJogador.Agachando) && spriteAtual > 26 && spriteAtual < 29) &&
                    !this.estado.Contains(EstadoJogador.Caindo) &&
                    !this.estado.Contains(EstadoJogador.Pulando))
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
            else if (estado == EstadoJogador.Pulando)
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
            else if (estado == EstadoJogador.Caindo)
            {
                if (this.estado.Contains(EstadoJogador.Andando) || this.estado.Contains(EstadoJogador.Correndo) ||
                    this.estado.Contains(EstadoJogador.Parado) || this.estado.Contains(EstadoJogador.Pulando))
                {
                    this.estado.Remove(EstadoJogador.Andando);
                    this.estado.Remove(EstadoJogador.Correndo);
                    this.estado.Remove(EstadoJogador.Parado);
                    this.estado.Remove(EstadoJogador.Pulando);
                    this.estado.Add(estado);
                    return true;
                }
            }
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
                    this.estado.Contains(EstadoJogador.Agachando) || this.estado.Contains(EstadoJogador.Caindo))
                {
                    this.estado.Remove(EstadoJogador.Correndo);
                    this.estado.Remove(EstadoJogador.Andando);
                    this.estado.Remove(EstadoJogador.Agachando);
                    this.estado.Remove(EstadoJogador.Caindo);

                    this.estado.Add(estado);
                    return true;
                }
            }
            return false;
        }

        private void Movimentacao()
        {
            if (Raylib.IsKeyDown(KeyboardKey.Space))
                this.Pular();

            this.velocidadeAtual = 0;
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
                this.velocidadeAtual = -velocidadeMovimento * velocidadeFator;
                this.direcao = DirecaoMovimento.Esquerda;
                if (velocidadeFator == 1) AddEstado(EstadoJogador.Andando);
            }
            else if (Raylib.IsKeyDown(KeyboardKey.D) || Raylib.IsKeyDown(KeyboardKey.Right))
            {
                this.velocidadeAtual = velocidadeMovimento * velocidadeFator;
                this.direcao = DirecaoMovimento.Direita;
                if (velocidadeFator == 1) AddEstado(EstadoJogador.Andando);
            }
            else
            {
                if (!this.estado.Contains(EstadoJogador.Caindo) && !this.estado.Contains(EstadoJogador.Pulando)) 
                    AddEstado(EstadoJogador.Parado);
            }

            this.posicao.X += this.velocidadeAtual;
        }

        public void Pular()
        {
            if (!this.estado.Contains(EstadoJogador.Pulando) && !this.estado.Contains(EstadoJogador.Caindo))
                AddEstado(EstadoJogador.Pulando);
        }

        public void Pulo()
        {
            if (this.estado.Contains(EstadoJogador.Pulando))
            {
                this.gravidadeAplicada = this.pulo.pular();
                gravidadeAplicada += this.gravidadeAplicada;
                if (this.gravidadeAplicada == 0)
                    AddEstado(EstadoJogador.Caindo);
            }
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
            if (this.estado.Contains(EstadoJogador.Pulando) || this.estado.Contains(EstadoJogador.Caindo)) this.Pulo();
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

        private Rectangle GetSobreposicao(Rectangle a, Rectangle b) 
        {
            float x = Math.Max(a.X, b.X);

            float y = Math.Max(a.Y, b.Y);

            float z = Math.Min(a.X + a.Width, b.X + b.Width);

            float w = Math.Min(a.Y + a.Height, b.Y + b.Height);

            return new Rectangle( x, y, z - x, w - y);
        }

        public void VerificaColisao(ISprite sprite)
        {
            Rectangle jogador = this.CaixaColisao();
            Rectangle bloco = sprite.CaixaColisao();

            if (Raylib.CheckCollisionRecs(jogador, bloco))
            {
                this.AddEstado(EstadoJogador.Parado);
                Rectangle overlap = GetSobreposicao(jogador, bloco);

                if (overlap.Width < overlap.Height)
                {
                    if (jogador.X < bloco.X)
                        this.posicao.X -= overlap.Width + this.velocidadeMovimento;
                    else
                        this.posicao.X += overlap.Width + this.velocidadeMovimento;

                    this.velocidadeAtual = 0;
                }
                else
                {
                    if (jogador.Y < bloco.Y)
                    {
                        this.gravidadeAplicada = 0;
                        this.AddEstado(EstadoJogador.Parado);
                        this.pulo.resetar();
                    }
                    else
                    {
                        this.posicao.Y += overlap.Height;
                        this.gravidadeAplicada = 0;
                    }
                }
            }
            else
            {
                if (this.gravidadeAplicada != 0 && !this.estado.Contains(EstadoJogador.Pulando))
                    this.AddEstado(EstadoJogador.Caindo);
            }
        }

        private void Unload()
        {
            Raylib.UnloadTexture(spriteSheet);
        }

    }
}
