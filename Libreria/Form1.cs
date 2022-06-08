using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ViewModels;

namespace Libreria
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetAllLibros();
        }

        private async void GetAllLibros()
        {
            using(var client=new HttpClient())
            {
                using(var response = await client.GetAsync("https://localhost:44369/Api/Libros")){

                    if (response.IsSuccessStatusCode)
                    {
                        string LibroJsonString = await response.Content.ReadAsStringAsync();
                        dataGridView1.DataSource = JsonConvert.DeserializeObject<List<LibrosViewModel>>(LibroJsonString).ToList();
                    }
                    else
                    {
                        MessageBox.Show("Test");
                        return;
                    }
                }
            }
        }

        private async void AddLibro()
        {
            LibrosViewModel oLibro = new LibrosViewModel()
            {
                ISBN = txtISBN.Text,
                Autor = txtAutor.Text,
                Editorial = txtEditorial.Text,
                Temas = txtTemas.Text,
                Titulo = txtTitulo.Text
            };

            using(var client= new HttpClient())
            {
                using (var response = await client.GetAsync("https://localhost:44369/Api/Libros"));

                var serializedLibro = JsonConvert.SerializeObject(oLibro);
                var content = new StringContent(serializedLibro, Encoding.UTF8, "application/json");
                var result = await client.PostAsync("https://localhost:44369/Api/Libros",content);
            }
            Limpiar();
            GetAllLibros();
        }

        private void Limpiar()
        {
            txtAutor.Clear();
            txtEditorial.Clear();
            txtTemas.Clear();
            txtTitulo.Clear();
            txtISBN.Clear();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            AddLibro();
        }
    }
}
