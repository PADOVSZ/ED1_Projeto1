using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing; // declarações e classes necessárias para desenho gráfico
using System.Runtime.ConstrainedExecution;

namespace Grafico
{
    class Ponto // implementar IComparable
    {
        // atributos que serão utilizados
        private int x, y;
        private Color cor;
        // private ListaSimples<Ponto> figuras = new ListaSimples<Ponto>();

        // construtor da classe Ponto
        public Ponto(int cX, int cY, Color qualCor)
        {
            x = cX; 
            y = cY;
            cor = qualCor;
        }

        // getters e setters dos atributos
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        public Color Cor
        {
            get { return cor; }
            set { cor = value; }
        }

        // desenha o ponto no local indicado pelas coordenadas cartesianas do ponto
        // e pintado na cor indicada pelo atributo cor
        public virtual void Desenhar(Color cor, Graphics g)
        {
            Pen pen = new Pen(cor);
            g.DrawLine(pen, x, y, x, y);
        }
    }
}