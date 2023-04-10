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
        // atributos que serão utilizados
        int largura;
        int altura;
        Color cor;

        // getters e setters
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

        // construtor da classe
        public Retangulo(int xInicial, int yInicial, int largura, int altura, Color cor) : 
                    base(xInicial, yInicial, cor)
        {
            this.altura = altura;
            this.largura = largura;
            this.cor = cor;
        }

        // efetua o desenho de um quadrilátero a partir de dois pontos, o que
        // indicara o seu início em uma ponta oposta ao outro ponto. a partir
        // do primeiro e segundo pontos serão desenhadas retas (altura e largura)
        // que se encontrarão e formarão os lados paralelos do quadrilátero
        public override void Desenhar(Color cor, Graphics g)
        {
            Pen p = new Pen(cor);
            g.DrawRectangle(p, base.X, base.Y, largura, altura);
        }

        // uma forma de armazenamento da figura por meio de código
        // para que possa ser salvo e futuramente acessado novamente
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
