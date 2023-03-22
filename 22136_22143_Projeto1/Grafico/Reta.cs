using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using System.Runtime.ConstrainedExecution;

namespace Grafico
{
    class Reta : Ponto
    {
        // herda (x, y) da classe Ponto, que são as coordenadas
        // do ponto inicial da reta; também herda a cor e, em
        // seguida define o ponto final:

        private Ponto pontoFinal;

        // getter e setter
        internal Ponto PontoFinal
        {
            get { return pontoFinal;  }
            set { pontoFinal = value; }
        }

        // construtor da classe:
        // recebe como parâmetros os componentes x1, y1 e x2, y2 coordenadas dos pontos inicial
        // e final da reta sendo criada, bem como sua cor e cria um objeto Reta
        // usando o construtor herdado da classe Ponto
        public Reta(int x1, int y1, int x2, int y2, Color novaCor) : base(x1, y1, novaCor)
        {
            pontoFinal = new Ponto(x2, y2, novaCor);
        }

        // efetua o desenho de uma linha reta entre(x1, y1) e (x2, y2) usando o método DrawLine()
        public override void Desenhar(Color corDesenho, Graphics g)
        {
            Pen pen = new Pen(corDesenho);
            g.DrawLine(pen, base.X, base.Y, // ponto inicial
                            pontoFinal.X, pontoFinal.Y);
        }
    }
}
