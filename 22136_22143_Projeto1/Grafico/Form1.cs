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
        /*bool esperaPonto;
        bool esperaInicioReta;
        bool esperaFimReta;
        bool esperaInicioCirculo;
        bool esperaFimCirculo;
        bool esperaInicioElipse;
        bool esperaFimElipse;
        bool esperaInicioRetangulo;
        bool esperaFimRetangulo;
        bool esperaInicioPolilinha;
        bool esperaFimPolilinha;*/

        public enum Situacao
        {
            esperaPonto, esperaInicioReta, esperaFimReta, esperaInicioCirculo, esperaFimCirculo,
            esperaInicioPolilinha, esperaFimPolilinha, esperaInicioElipse, esperaFimElipse,
            esperaInicioRetangulo, esperaFimRetangulo, esperaAcao
        }

        Situacao situacaoAtual = new Situacao();

        // uma lista ligada simples para armazenar figuras da classe Ponto
        private ListaSimples<Ponto> figuras = new ListaSimples<Ponto>();

        Color corAtual = Color.Black;
        private static Ponto p1 = new Ponto(0, 0, Color.Black);

        public frmGrafico()
        {
            InitializeComponent();
        }

        // método onde atribuiremos false em todas as variáveis lógicas que indicarão
        // que tipo de clique o programa está esperando no momento
        private void LimparEsperas()
        {
            situacaoAtual = Situacao.esperaAcao;
        }

        // Acessamos o contexto gráfico do PictureBox e percorremos a lista
        // ligada para acessarmos cada uma das figuras nela armazenadas
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

        private void pbAreaDesenho_MouseMove(object sender, MouseEventArgs e)
        {
            stMensagem.Items[3].Text = e.X +","+ e.Y;
        }

        // o botão btnAbrir exibe um diálogo de abertura de arquivo, que permitirá selecionar
        // um arquivo com as figuras geométricas salvas anteriormente pelo aplicativo.
        private void btnAbrir_Click(object sender, EventArgs e)
        {
            if(dlgAbrir.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // lê linhas do arquivo até que este acabe, processa cada uma, determinando
                    // o tipo de figura geométrica que a string lida representa e captura os valores
                    // adicionais que cada tipo de figura possui, de acordo com o tipo da figura

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
                                figuras.InserirAposFim(new NoLista<Ponto>(
                                                       new Reta(xBase, yBase, xFinal, yFinal, cor)));
                                break;

                            case 'c': //circulo
                                int raio = Convert.ToInt32(linha.Substring(30, 5).Trim());
                                figuras.InserirAposFim(new NoLista<Ponto>(
                                                       new Circulo(xBase, yBase, raio, cor)));
                                break;

                            case 'e': //elipse
                                int raioX = Convert.ToInt32(linha.Substring(30, 5).Trim());
                                int raioY = Convert.ToInt32(linha.Substring(35, 5).Trim());
                                figuras.InserirAposFim(new NoLista<Ponto>(
                                                       new Elipse(xBase, yBase, raioX, raioY, cor)));
                                break;

                            case 'r': //retangulo
                                int largura = Convert.ToInt32(linha.Substring(30, 5).Trim());
                                int altura  = Convert.ToInt32(linha.Substring(35, 5).Trim());
                                figuras.InserirAposFim(new NoLista<Ponto>(
                                                       new Retangulo(xBase, yBase, largura, altura, cor)));
                                break;
                        }
                    }

                    arqFiguras.Close();

                    // mudam o título da janela-filha, para o nome do arquivo aberto,
                    // e chama desenhaObjetos, passando a área gráfica de desenho
                    // do painel de desenhos como parâmetro
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
            // caso isso seja verdade, capturamos o ponto
            // em que o mouse estava quando o evento foi disparado
            Polilinha novaPolilinha = new Polilinha();
            switch(situacaoAtual)
            {
                case Situacao.esperaPonto:
                    Ponto p = new Ponto(e.X, e.Y, corAtual);
                    figuras.InserirAposFim(new NoLista<Ponto>(p));
                    p.Desenhar(p.Cor, pbAreaDesenho.CreateGraphics());
                    LimparEsperas();
                    stMensagem.Items[1].Text = "";

                    break;

                case Situacao.esperaInicioReta:
                    p1.Cor = corAtual;
                    p1.X = e.X;
                    p1.Y = e.Y; 
                    situacaoAtual = Situacao.esperaFimReta;
                    stMensagem.Items[1].Text = "Clique no ponto final da reta";

                    break;

                case Situacao.esperaFimReta:
                    Reta r = new Reta(p1.X, p1.Y, e.X, e.Y, corAtual);
                    figuras.InserirAposFim(new NoLista<Ponto>(r));
                    r.Desenhar(r.Cor, pbAreaDesenho.CreateGraphics());
                    LimparEsperas();
                    stMensagem.Items[1].Text = "";

                    break;

                case Situacao.esperaInicioCirculo:
                    p1.Cor = corAtual;
                    p1.X = e.X;
                    p1.Y = e.Y;
                    situacaoAtual = Situacao.esperaFimCirculo;
                    stMensagem.Items[1].Text = "Clique no local da extremidade do círculo";

                    break;

                case Situacao.esperaFimCirculo:
                    int raio = (int)Math.Sqrt(Math.Pow(Math.Abs(e.X - p1.X), 2) + Math.Pow(Math.Abs(e.Y - p1.Y), 2));
                    Circulo c = new Circulo(p1.X, p1.Y, raio, corAtual);
                    figuras.InserirAposFim(new NoLista<Ponto>(c));
                    c.Desenhar(c.Cor, pbAreaDesenho.CreateGraphics());
                    LimparEsperas();
                    stMensagem.Items[1].Text = "";

                    break;

                case Situacao.esperaInicioElipse:
                    p1.Cor = corAtual;
                    p1.X = e.X;
                    p1.Y = e.Y;
                    situacaoAtual = Situacao.esperaFimElipse;
                    stMensagem.Items[1].Text = "Clique na extremidade da elipse";

                    break;

                case Situacao.esperaFimElipse:
                    Elipse elipse = new Elipse(p1.X, p1.Y, e.X < p1.X ? p1.X - e.X : e.X - p1.X, e.Y < p1.Y ? p1.Y - e.Y : e.Y - p1.Y, corAtual);
                    figuras.InserirAposFim(new NoLista<Ponto>(elipse));
                    elipse.Desenhar(elipse.Cor, pbAreaDesenho.CreateGraphics());
                    LimparEsperas();
                    stMensagem.Items[1].Text = "";

                    break;

                case Situacao.esperaInicioRetangulo:
                    p1.Cor = corAtual;
                    p1.X = e.X;
                    p1.Y = e.Y;
                    situacaoAtual = Situacao.esperaFimRetangulo;
                    stMensagem.Items[1].Text = "Clique no canto inferior direito do retângulo";

                    break;

                case Situacao.esperaFimRetangulo:
                    int x = e.X;
                    int y = e.Y;

                    if (e.X < p1.X)
                    {
                        x = p1.X;
                        p1.X = e.X;
                    }

                    if (e.Y < p1.Y)
                    {
                        y = p1.Y;
                        p1.Y = e.Y;
                    }

                    Retangulo retangulo = new Retangulo(p1.X, p1.Y, x - p1.X, y - p1.Y, corAtual);
                    figuras.InserirAposFim(new NoLista<Ponto>(retangulo));
                    retangulo.Desenhar(retangulo.Cor, pbAreaDesenho.CreateGraphics());
                    LimparEsperas();
                    stMensagem.Items[1].Text = "";

                    break;

                case Situacao.esperaInicioPolilinha:
                    p1.X = e.X;
                    p1.Y = e.Y;
                    p1.Cor = corAtual;
                    situacaoAtual = Situacao.esperaFimPolilinha;
                    novaPolilinha.Cor = corAtual;
                    novaPolilinha.X = e.X;
                    novaPolilinha.Y = e.Y;
                    stMensagem.Items[1].Text = "Clique nos outros pontos para formar as linhas. Clique novamente no botão [Polilinha] para encerrar";
                    break;

                case Situacao.esperaFimPolilinha:
                    Reta novaReta = new Reta(p1.X, p1.Y, e.X, e.Y, p1.Cor); //mudar cor
                    novaPolilinha.Linhas.InserirAposFim(novaReta);
                    novaPolilinha.Desenhar(novaPolilinha.Cor, pbAreaDesenho.CreateGraphics());
                    p1.X = e.X;
                    p1.Y = e.Y;
                    break;
            }

        }

        // quando o usuário clicar nesse botão, o programa deverá informar
        // que está esperando que o usuário indique um ponto sobre área de desenho
        private void btnPonto_Click(object sender, EventArgs e)
        {
            stMensagem.Items[1].Text = "Mensagem: Clique em um ponto na área de desenho";
            LimparEsperas();
            situacaoAtual = Situacao.esperaPonto;
        }

        private void btnReta_Click(object sender, EventArgs e)
        {
            stMensagem.Items[1].Text = "Clique no local do ponto inicial da reta";
            LimparEsperas();
            situacaoAtual = Situacao.esperaInicioReta;
        }

        private void btnCirculo_Click(object sender, EventArgs e)
        {
            stMensagem.Items[1].Text = "Clique no local do centro do círculo";
            LimparEsperas();
            situacaoAtual = Situacao.esperaInicioCirculo;
        }

        private void btnElipse_Click(object sender, EventArgs e)
        {
            stMensagem.Items[1].Text = "Clique no local do centro da elipse";
            LimparEsperas();
            situacaoAtual = Situacao.esperaInicioElipse;
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
            // solicita salvamento antes do usuário fechar o programa
            if(MessageBox.Show("Deseja salvar seu desenho antes de sair?", "Sair", MessageBoxButtons.YesNo,
                                                                                   MessageBoxIcon.Question) == DialogResult.Yes) 
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
            situacaoAtual = Situacao.esperaInicioRetangulo;
        }

        private void btnPolilinha_Click(object sender, EventArgs e)
        {
            stMensagem.Items[1].Text = "Selecione o ponto inicial";
            LimparEsperas();
            situacaoAtual = Situacao.esperaInicioPolilinha;

            if (situacaoAtual == Situacao.esperaFimPolilinha)
                LimparEsperas();
        }
    }
}
