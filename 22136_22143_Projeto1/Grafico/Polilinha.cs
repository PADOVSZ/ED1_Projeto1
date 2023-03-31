using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafico
{
    class Polilinha : Ponto
    {
        ListaSimples<Reta> linhas;
        Color cor;

        public Polilinha(int x1, int y1, Color cor) : base(x1, y1, cor)
        {
            this.cor = cor;
            linhas = new ListaSimples<Reta>();
        }

        public Polilinha() : base(0 , 0, Color.Black)
        {
            linhas = new ListaSimples<Reta>();
            cor = Color.Black;
        }

        public ListaSimples<Reta> Linhas
        {
            get => linhas;
            set => linhas = value;
        }

        public Color Cor
        {
            get => cor;
            set => cor = value; 
        }

        public override void Desenhar(Color cor, Graphics g)
        {
            Pen p = new Pen(cor);
            linhas.IniciarPercursoSequencial();

            while(linhas.PodePercorrer())
                g.DrawLine(p, linhas.Atual.Info.X, linhas.Atual.Info.Y, linhas.Atual.Info.PontoFinal.X, linhas.Atual.Info.PontoFinal.Y);
        }

        public override string ToString()
        {
            string arq = "";
            linhas.IniciarPercursoSequencial();
            while(linhas.PodePercorrer())
            {
                arq += linhas.Atual.Info.ToString() + "\n";
            }

            return arq;
        }
    }
}
