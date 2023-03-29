using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grafico
{
    public partial class frmGrafico : Form
    {
        bool esperaPonto;
        bool esperaInicioReta;
        bool esperaFimReta;
        bool esperaInicioCirculo;
        bool esperaFimCirculo;
        bool esperaInicioElipse;
        bool esperaFimElipse;
        bool esperaInicioRetangulo;
        bool esperaFimRetangulo;
        private ListaSimples<Ponto> figuras = new ListaSimples<Ponto>();
        Color corAtual = Color.Black;
        private static Ponto p1 = new Ponto(0, 0, Color.Black);

        public frmGrafico()
        {
            InitializeComponent();
        }

        private void LimparEsperas()
        {
            esperaPonto = false;
            esperaInicioReta = false;
            esperaFimReta = false;
        }

        private void pbAreaDesenho_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics; // acessa contexto gráfico
            figuras.IniciarPercursoSequencial();
            while (figuras.PodePercorrer())
            {
                Ponto figuraAtual = figuras.Atual.Info;
                figuraAtual.Desenhar(figuraAtual.Cor, g);
            }
        }

        private void btnPonto_Click(object sender, EventArgs e)
        {
            stMensagem.Items[1].Text = "Mensagem: Clique em um ponto na área de desenho";
            LimparEsperas();
            esperaPonto = true;
        }

        private void pbAreaDesenho_MouseMove(object sender, MouseEventArgs e)
        {
            stMensagem.Items[3].Text = e.X +","+ e.Y;
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            if(dlgAbrir.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader arqFiguras = new StreamReader(dlgAbrir.FileName);
                    string linha = arqFiguras.ReadLine();
                    double xInfEsq_Screen = Convert.ToDouble(linha.Substring(0, 5).Trim());
                    double yInfEsq_Screen = Convert.ToDouble(linha.Substring(5, 5).Trim());
                    double xSupDir_Screen = Convert.ToDouble(linha.Substring(10, 5).Trim());
                    double ySupDir_Screen = Convert.ToDouble(linha.Substring(15, 5).Trim());

                    while(!arqFiguras.EndOfStream)
                    {
                        linha = arqFiguras.ReadLine();
                        string tipo = linha.Substring(0, 5).Trim();
                        int xBase = Convert.ToInt32(linha.Substring(5, 5).Trim());
                        int yBase = Convert.ToInt32(linha.Substring(10, 5).Trim());
                        int corR = Convert.ToInt32(linha.Substring(15, 5).Trim());
                        int corG = Convert.ToInt32(linha.Substring(20, 5).Trim());
                        int corB = Convert.ToInt32(linha.Substring(25, 5).Trim());
                        Color cor = new Color();
                        cor = Color.FromArgb(255, corR, corG, corB);

                        switch(tipo[0])
                        {
                            case 'p': //ponto
                                figuras.InserirAposFim(new NoLista<Ponto>(new Ponto(xBase, yBase, cor)));
                                break;

                            case 'l': //reta
                                int xFinal = Convert.ToInt32(linha.Substring(30, 5).Trim());
                                int yFinal = Convert.ToInt32(linha.Substring(35, 5).Trim());
                                figuras.InserirAposFim(
                                    new NoLista<Ponto>(
                                        new Reta(xBase, yBase, xFinal, yFinal, cor)));
                                break;

                            case 'c': //circulo
                                int raio = Convert.ToInt32(linha.Substring(30, 5).Trim());
                                figuras.InserirAposFim(
                                    new NoLista<Ponto>(
                                        new Circulo(xBase, yBase, raio, cor)));
                                break;

                            case 'e': //elipse
                                int raioX = Convert.ToInt32(linha.Substring(30, 5).Trim());
                                int raioY = Convert.ToInt32(linha.Substring(35, 5).Trim());
                                figuras.InserirAposFim(
                                    new NoLista<Ponto>(
                                        new Elipse(xBase, yBase, raioX, raioY, cor)));
                                break;
                        }
                    }

                    arqFiguras.Close();
                    this.Text = dlgAbrir.FileName;
                    pbAreaDesenho.Invalidate();
                }
                catch (IOException)
                {
                    MessageBox.Show("Erro de leitura no arquivo");
                }
            }
        }

        private void pbAreaDesenho_MouseClick(object sender, MouseEventArgs e)
        {
            if(esperaPonto)
            {
                Ponto p = new Ponto(e.X, e.Y, corAtual);
                figuras.InserirAposFim(new NoLista<Ponto>(p));
                p.Desenhar(p.Cor, pbAreaDesenho.CreateGraphics());
                esperaPonto = false;
                stMensagem.Items[1].Text = "";
            }
            else
            if(esperaInicioReta)
            {
                p1.Cor = corAtual;
                p1.X = e.X;
                p1.Y = e.Y;
                esperaInicioReta = false;
                esperaFimReta = true;
                stMensagem.Items[1].Text = "Clique no ponto final da reta";
            }
            else
                if(esperaFimReta)
            {
                Reta r = new Reta(p1.X, p1.Y, e.X, e.Y, corAtual);
                figuras.InserirAposFim(new NoLista<Ponto>(r));
                r.Desenhar(r.Cor, pbAreaDesenho.CreateGraphics());
                esperaFimReta = false;
                stMensagem.Items[1].Text = "";
            }
            else
                if(esperaInicioCirculo)
            {
                p1.Cor = corAtual;
                p1.X = e.X;
                p1.Y = e.Y;
                esperaInicioCirculo = false;
                esperaFimCirculo = true;
                stMensagem.Items[1].Text = "Clique no local da extremidade do círculo";
            }
            else
                if(esperaFimCirculo)
            {
                Circulo c = new Circulo(p1.X, p1.Y, (e.X - p1.X) < 0 ? p1.X - e.X : e.X - p1.X, corAtual);
                figuras.InserirAposFim(new NoLista<Ponto>(c));
                c.Desenhar(c.Cor, pbAreaDesenho.CreateGraphics());
                esperaFimCirculo = false;
                stMensagem.Items[1].Text = "";
            }
            else
                if(esperaInicioElipse)
            {
                p1.Cor = corAtual;
                p1.X = e.X;
                p1.Y = e.Y;
                esperaInicioElipse = false;
                esperaFimElipse = true;
                stMensagem.Items[1].Text = "Clique na extremidade da elipse";
            }
            else
                if(esperaFimElipse)
            {
                Elipse elipse = new Elipse(p1.X, p1.Y, e.X < p1.X ? p1.X - e.X : e.X - p1.X, e.Y < p1.Y ? p1.Y - e.Y : e.Y - p1.Y, corAtual);
                figuras.InserirAposFim(new NoLista<Ponto>(elipse));
                elipse.Desenhar(elipse.Cor, pbAreaDesenho.CreateGraphics());
                esperaFimElipse = false;
                stMensagem.Items[1].Text = "";
            }
            else
                if(esperaInicioRetangulo)
            {
                p1.Cor = corAtual;
                p1.X = e.X;
                p1.Y = e.Y;
                esperaInicioRetangulo = false;
                esperaFimRetangulo = true;
                stMensagem.Items[1].Text = "Clique no canto inferior direito do retângulo";
            }
            else
                if(esperaFimRetangulo)
            {
                int x = e.X;
                int y = e.Y;

                if ()
                    p1.X = x;

                Retangulo retangulo = new Retangulo(p1.X, p1.Y, (p1.X > e.X) ? p1.X - e.X : e.X - p1.X, (p1.Y > e.Y) ? p1.Y - e.Y : e.Y - p1.Y, corAtual);
                figuras.InserirAposFim(new NoLista<Ponto>(retangulo));
            }
        }

        private void btnReta_Click(object sender, EventArgs e)
        {
            stMensagem.Items[1].Text = "Clique no local do ponto inicial da reta";
            LimparEsperas();
            esperaInicioReta = true;
        }

        private void btnCirculo_Click(object sender, EventArgs e)
        {
            stMensagem.Items[1].Text = "Clique no local do centro do círculo";
            LimparEsperas();
            esperaInicioCirculo = true;
        }

        private void btnElipse_Click(object sender, EventArgs e)
        {
            stMensagem.Items[1].Text = "Clique no local do centro da elipse";
            LimparEsperas();
            esperaInicioElipse = true;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmGrafico_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("Deseja salvar seu desenho antes de sair?", "Sair", MessageBoxButtons.YesNo) == DialogResult.Yes) 
            {
                Salvar();
            }
        }

        private void Salvar()
        {
            if (dlgSalvar.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamWriter arq = new StreamWriter(dlgSalvar.FileName);
                    arq.WriteLine("0".PadLeft(10, '0') +
                        pbAreaDesenho.Width.ToString().PadLeft(5, '0') +
                        pbAreaDesenho.Height.ToString().PadLeft(5, '0'));
                    figuras.IniciarPercursoSequencial();
                    while (figuras.PodePercorrer())
                    {
                        arq.WriteLine(figuras.Atual.Info.ToString());
                    }
                    arq.Close();
                    stMensagem.Items[1].Text = $"Arquivo salvo em {dlgSalvar.FileName}";
                }
                catch (IOException)
                {
                    MessageBox.Show("Erro ao salvar figuras");
                }
            }
        }

        private void btnRetangulo_Click(object sender, EventArgs e)
        {
            stMensagem.Items[1].Text = "Clique no canto superior esquerdo do retângulo";
            LimparEsperas();
            esperaInicioRetangulo = true;
        }
    }
}
