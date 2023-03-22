using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafico
{
    class Circulo : Ponto
    {
        // herda o ponto central (x, y) da classe Ponto
        int raio;

        // getter e setter
        public int Raio
        {
            get { return raio; }
            set { raio = value; }
        }

        // construtor da classe
        public Circulo(int xCentro, int yCentro, int novoRaio, Color novaCor) :
                  base(xCentro, yCentro, novaCor) // construtor de Ponto(x,y)
        {
            raio = novoRaio;
        }

        public void setRaio(int novoRaio)
        {
            raio = novoRaio;
        }

        // efetua o desenho de um círculo a partir do ponto central do mesmo
        public override void Desenhar(Color corDesenho, Graphics g)
        {
            Pen pen = new Pen(corDesenho);
            g.DrawEllipse(pen, base.X - raio, base.Y - raio, // centro - raio
                                        2 * raio, 2 * raio); // centro + raio
        }
    }
}