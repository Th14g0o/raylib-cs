using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guilherme
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

    public static class Global
    {
        

        public static int larguraTela = 1000;
        public static int alturaTela = 800;
    }
}
