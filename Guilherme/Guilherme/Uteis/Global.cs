namespace Guilherme.Uteis
{
    public enum EstadoJogador
    {
        Parado,
        Andando,
        Correndo,
        Pulando,
        Caindo,
        Levantando,
        Agachando,
        Apanhando,
        Atacando,
        Bloqueando,
    }

    public enum DirecaoMovimento
    {
        Nenhum,
        Esquerda,
        Direita,
        Baixo,
        Cima
    }

    public static class Global
    {
        public static int larguraTela = 1000;
        public static int alturaTela = 800;
        public static int FPS = 60;
    }
}
