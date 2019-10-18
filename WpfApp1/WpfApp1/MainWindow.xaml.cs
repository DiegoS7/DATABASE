using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb; //Agregamos libreria OleDB
using System.Data; //Agregamos Manejo de datos

namespace WpfApp1
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OleDbConnection con; //Agregamos la conexion
        DataTable dt; //Agregar la tabla
        public MainWindow()
        {
            InitializeComponent();
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\AlumnosDB.mdb";
            MostrarDatos();
        }

        //Mostramos los registros de la tabla
        private void MostrarDatos()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from Celphone";
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            gvDatos.ItemsSource = dt.AsDataView();
            if (dt.Rows.Count > 0)
            {
                lbContenido.Visibility = System.Windows.Visibility.Hidden;
                gvDatos.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lbContenido.Visibility = System.Windows.Visibility.Visible;
                gvDatos.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void LimpiarTodo()
        {
            txtId.Text = "";
            txtModel.Text = "";
            cbCompania.SelectedIndex = 0;
            txtProblema.Text = "";
            txtComentario.Text = "";
            btnNuevo.Content = "Nuevo";
            txtId.IsEnabled = true;
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;

            if (txtId.Text != "")
            {
                if (txtId.IsEnabled == true)
                {
                    if (cbCompania.Text != "Selecciona Compania")
                    {
                        cmd.CommandText = "insert into Celphone(Id,Modelo,Compania,Problema,Comentario) " +
                            "Values(" + txtId.Text + ",'" + txtModel.Text + "','" + cbCompania.Text + "'," + txtProblema.Text + ",'" + txtComentario.Text + "')";
                        cmd.ExecuteNonQuery();
                        MostrarDatos();
                        MessageBox.Show("Alumno agregado correctamente...");
                        LimpiarTodo();

                    }
                    else
                    {
                        MessageBox.Show("Favor de seleccionar el genero....");
                    }
                }
                else
                {
                    cmd.CommandText = "update Celphone set Modelo='" + txtModel.Text + "',Compania='" + cbCompania.Text + "',Problema=" + txtProblema.Text + ",Comentario='" + txtComentario.Text + "' where Id=" + txtId.Text;
                    cmd.ExecuteNonQuery();
                    MostrarDatos();
                    MessageBox.Show("Datos del alumno Actualizados...");
                    LimpiarTodo();
                }
            }
            else
            {
                MessageBox.Show("Favor de poner el ID de un Alumno.......");
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];
                txtId.Text = row["Id"].ToString();
                txtModel.Text = row["Modelo"].ToString();
                cbCompania.Text = row["Compania"].ToString();
                txtProblema.Text = row["Problema"].ToString();
                txtComentario.Text = row["Comentario"].ToString();
                txtId.IsEnabled = false;
                btnNuevo.Content = "Actualizar";
            }
            else
            {
                MessageBox.Show("Favor de Seleccionar un alumno...");
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];
                OleDbCommand cmd = new OleDbCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.Connection = con;
                cmd.CommandText = "delete from Celphone where Id=" + row["Id"].ToString();
                cmd.ExecuteNonQuery();
                MostrarDatos();
                MessageBox.Show("Alumno eliminado correctamente...");
                LimpiarTodo();
            }
            else
            {
                MessageBox.Show("Favor de Seleccionar un alumno...");
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarTodo();
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
