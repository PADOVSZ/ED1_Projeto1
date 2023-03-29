using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafico
{
    class Retangulo : Ponto
    {
        int largura;
        int altura;
        Color cor;

        public int Largura
        {
            get => largura;
            set => largura = value;
        }

        public int Altura
        {
            get => altura;
            set => altura = value;
        }

        public Color Cor
        {
            get => cor;
            set => cor = value;
        }

        public Retangulo(int xInicial, int yInicial, int altura, int largura, Color cor) : 
                         base(xInicial, yInicial, cor)
        {
            this.altura = altura;
            this.largura = largura;
            this.cor = cor;
        }

        public override void Desenhar(Color cor, Graphics g)
        {
            Pen p = new Pen(cor);
            g.DrawRectangle(p, base.X, base.Y, largura, altura);
        }

        public override string ToString()
        {
            return transformaString("r", 5) +
                   transformaString(base.X, 5) +
                   transformaString(base.Y, 5) +
                   transformaString(Cor.R, 5) +
                   transformaString(Cor.G, 5) +
                   transformaString(Cor.B, 5) +
                   transformaString(this.largura, 5) +
                   transformaString(this.altura, 5);
        }
    }
}
