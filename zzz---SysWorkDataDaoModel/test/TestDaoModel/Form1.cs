using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestDaoModel.Entities;

namespace TestDaoModel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            string connectionString = "Data Source = 54.207.67.88; Initial Catalog = UPC; User ID = SA; Password =@#!SqlAws58125812$%&;";
            DaoProducto daoProducto = new DaoProducto(connectionString,"Productos");


            daoProducto.GetListByCriteria(x => (x.idProducto == 16446) || (x.idProducto == 16445));


        }
    }
}
