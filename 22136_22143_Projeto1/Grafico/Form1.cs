using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grafico
{
    public partial class frmGrafico : Form
    {
        private ListaSimples<Ponto> figuras = new ListaSimples<Ponto>();
        public frmGrafico()
        {
            InitializeComponent();
        }

        private void frmGrafico_Load(object sender, EventArgs e)
        {

        }
    }
}
