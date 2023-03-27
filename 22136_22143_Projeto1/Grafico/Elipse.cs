using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafico
{
    class Elipse : Circulo //herda todos os atributos de um circulo. obs: seria mais fácil fazer o inverso. Circulo herdar de elipse.
    {
        private int raio2;

        public int Raio2
        {
            get { return raio2; }
            set { raio2 = value; }
        }

        public Elipse(int centroX, int centroY, int raioX, int raioY, Color novaCor) : base(centroX, centroY, raioX, novaCor)
        {
            this.raio2 = raioY;
        }

        public override void Desenhar(Color corDesenho, Graphics g) //desenha elipse
        {
            Pen pen = new Pen(corDesenho);
            g.DrawEllipse(pen, base.X - base.Raio, base.Y - raio2, 2 * base.Raio, 2 * raio2); //acho q dá certo...
        }

        public override string ToString()
        {
            return transformaString("e", 5) +
                transformaString(base.X, 5) +
                transformaString(base.Y, 5) +
                transformaString(Cor.R, 5) +
                transformaString(Cor.G, 5) +
                transformaString(Cor.B, 5) +
                transformaString(base.Raio, 5) +
                transformaString(raio2, 5);
        }
        // fazer a classe herdada de Ponto, descrita na página 5 do pdf
    }
}
