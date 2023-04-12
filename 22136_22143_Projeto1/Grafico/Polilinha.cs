using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafico
{
    // será feita uma lista de pontos, os quais estarão ligados, formando a polilinha
    class Polilinha : Ponto
    {
        // atributos que serão utilizados
        ListaSimples<Ponto> listaPonto;
        Color cor;

        // getters e setters
        public ListaSimples<Ponto> ListaPonto
        {
            get => listaPonto;
            set => listaPonto = value;
        }

        public Color Cor
        {
            get => cor;
            set => cor = value;
        }

        // construtores de classe
        public Polilinha(int x1, int y1, Color cor) : base(x1, y1, cor)
        {
            this.cor = cor;
            listaPonto = new ListaSimples<Ponto>();
        }

        // efetua o desenho da polilinha baseado na lista ligada.
        // as retas serão formadas a partir do clique do usuário, pois será feito um ponto
        // (o ponto final de uma reta) e esse ponto será adicionada na lista. quando o usuário
        // fizer um double click, o desenho da polilinha será encerrado
        public override void Desenhar(Color cor, Graphics g)
        {
            Pen p = new Pen(cor);
            listaPonto.IniciarPercursoSequencial();
            g.DrawLine(p, base.X, base.Y, listaPonto.Atual.Info.X, listaPonto.Atual.Info.Y);

            while(listaPonto.PodePercorrer())
            {   //talvez dÊ erro pois proximo do ultimo é nulo
                if(listaPonto.Atual.Prox != null)
                    g.DrawLine(p, listaPonto.Atual.Info.X, listaPonto.Atual.Info.Y, listaPonto.Atual.Prox.Info.X, listaPonto.Atual.Prox.Info.Y);
            }
        }

        // uma forma de armazenamento da figura por meio de código
        // para que possa ser salvo e futuramente acessado novamente
        public override string ToString()
        {
            string arq = transformaString("m", 5) +
                         transformaString(base.X, 5) +
                         transformaString(base.Y, 5) +
                         transformaString(Cor.R, 5) +
                         transformaString(Cor.G, 5) +
                         transformaString(Cor.B, 5) +
                         transformaString(listaPonto.QuantosNos(), 5);

            listaPonto.IniciarPercursoSequencial();
            while(listaPonto.PodePercorrer())
            {
                arq +=
                transformaString(listaPonto.Atual.Info.X, 5) +
                transformaString(listaPonto.Atual.Info.Y, 5);
            }

            return arq;
        }
    }
}
