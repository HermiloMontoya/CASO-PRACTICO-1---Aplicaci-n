using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionDeGastos
{
    public partial class FPrincipal : Form
    {

        public class Gasto
        {
            public DateTime Fecha { get; set; }
            public string NombreDelGasto { get; set; }
            public decimal TotalDelGasto { get; set; }
        }

        private List<Gasto> listaGastos = new List<Gasto>();


        public FPrincipal()
        {
            InitializeComponent();

            mostrarBaseDeDatos.DataSource = listaGastos;

            mostrarBaseDeDatos.Columns[0].Width = 160; // Fecha
            mostrarBaseDeDatos.Columns[1].Width = 200; // Nombre
            mostrarBaseDeDatos.Columns[2].Width = 130; // Total
        }

        //UTILIZAMOS UN METODO DE CODIGO ASCII PARA QUE SOLO SE PERMITA CARACTERES NUMERICOS PARA NUMERO ENTEROS O DECIMALES.
        private void SoloNumeros(KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 45 || (e.KeyChar >= 58 && e.KeyChar <= 255)))
            {
                MessageBox.Show("Solo se permite caracteres numericos y caracter de '.' para decimal.", "Caractere invalido", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void txtTotalGasto_KeyPress(object sender, KeyPressEventArgs e)
        {
            SoloNumeros(e);
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
           
            if (string.IsNullOrWhiteSpace(txtNombreGasto.Text) || string.IsNullOrWhiteSpace(txtTotalGasto.Text))
            {
                MessageBox.Show("ERROR: Ingrese el nombre y el total del gasto.");
                return;
            }

          
            if (!decimal.TryParse(txtTotalGasto.Text, out decimal total))
            {
                MessageBox.Show("ERROR: Ingrese un total de gasto válido.");
                return;
            }

           
            DateTime fecha = txtFechaAgregar.Value;

           
            Gasto nuevoGasto = new Gasto
            {
                Fecha = fecha,
                NombreDelGasto = txtNombreGasto.Text,
                TotalDelGasto = total
            };

            
            listaGastos.Add(nuevoGasto);

            LimpiarTxt();

            ActualizarDataGridView();
            calcularTodo(listaGastos);
        }

        private void LimpiarTxt()
        {
            txtNombreGasto.Clear();
            txtTotalGasto.Clear();
        }

        private void ActualizarDataGridView()
        {     
            mostrarBaseDeDatos.DataSource = null; 
            mostrarBaseDeDatos.DataSource = listaGastos;

            mostrarBaseDeDatos.DataSource = listaGastos;

            mostrarBaseDeDatos.Columns[0].Width = 160; // Fecha
            mostrarBaseDeDatos.Columns[1].Width = 200; // Nombre
            mostrarBaseDeDatos.Columns[2].Width = 130; // Total
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            DateTime fechaBusqueda = txtFechaMostrar.Value.Date;

            List<Gasto> gastosFiltrados = listaGastos.FindAll(g => g.Fecha.Date == fechaBusqueda);

            mostrarBaseDeDatos.DataSource = gastosFiltrados;

            calcularSelecion(); //Metodo que calcula el gasto por fecha de selecion de los registros.
        }

        private void btnTodo_Click(object sender, EventArgs e)
        {
            ActualizarDataGridView();
            calcularTodo(listaGastos); //Metodo que calcula el gasto de todos los registros.
            
        }

        private void calcularTodo(List<Gasto> gastos)
        {
           
            decimal total = gastos.Sum(g => g.TotalDelGasto);
            lbGastos.Text = total.ToString();

            int registros = mostrarBaseDeDatos.RowCount;
            lbRegistros.Text = registros.ToString();
        }

        private void calcularSelecion()
        {
            decimal total = 0;

            if (mostrarBaseDeDatos.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in mostrarBaseDeDatos.Rows)
                {
                    Gasto gasto = (Gasto)row.DataBoundItem;

                    total += gasto.TotalDelGasto;
                }
            }

            int registros = mostrarBaseDeDatos.RowCount;
            lbRegistros.Text = registros.ToString();
            lbGastos.Text = total.ToString();
        }
    }
}
