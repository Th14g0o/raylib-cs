using Guilherme.Interfaces;
using Raylib_cs;
using System.Numerics;

namespace Guilherme.Classes
{
    class CorpoFisico : ISprite
    {
        private Rectangle areaColisao;
        private Color cor;    
        public  CorpoFisico(int x, int y, int largura, int altura, Color cor) {
            areaColisao = new Rectangle(x, y, largura, altura);
            this.cor = cor; 
        }       
        
        public void Update()
        {
            Raylib.DrawRectangle((int)this.areaColisao.X, (int)this.areaColisao.Y, (int)this.areaColisao.Width, (int)this.areaColisao.Height, this.cor);
        }

        public Rectangle CaixaColisao()
        {
            return areaColisao;
        }
    }
}
