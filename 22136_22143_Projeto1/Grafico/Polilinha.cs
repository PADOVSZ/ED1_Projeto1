using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafico
{
    // será feita uma lista de retas, as quais estarão ligadas, formando a polilinha
    class Polilinha : Ponto
    {
        // atributos que serão utilizados
        ListaSimples<Reta> linhas;
        Color cor;

        // getters e setters
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

        // construtores de classe
        public Polilinha(int x1, int y1, Color cor) : 
                    base(x1, y1, cor)
        {
            this.cor = cor;
            linhas = new ListaSimples<Reta>();
        }

        public Polilinha() : base(0 , 0, Color.Black)
        {
            linhas = new ListaSimples<Reta>();
            cor = Color.Black;
        }

        // efetua o desenho da polilinha baseado na lista ligada.
        // as retas serão formadas a partir do clique do usuário, pois será feito um ponto
        // (o ponto final de uma reta) e essa reta será adicionada na lista. quando o usuário
        // fizer um double click, o desenho da polilinha será encerrado
        public override void Desenhar(Color cor, Graphics g)
        {
            Pen p = new Pen(cor);
            linhas.IniciarPercursoSequencial();

            while(linhas.PodePercorrer())
                g.DrawLine(p, linhas.Atual.Info.X, linhas.Atual.Info.Y, 
                           linhas.Atual.Info.PontoFinal.X, linhas.Atual.Info.PontoFinal.Y);
        }

        // uma forma de armazenamento da figura por meio de código
        // para que possa ser salvo e futuramente acessado novamente
        public override string ToString()
        {
            string arq = "";
            linhas.IniciarPercursoSequencial();
            while(linhas.PodePercorrer())
            {
                arq += linhas.Atual.Info.ToString();
            }

            return arq;
        }

        public Polilinha(Polilinha polilinha) : 
                    this(polilinha.X, polilinha.Y, polilinha.Cor)
        {}
    }
}
