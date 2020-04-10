using MetroFramework;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestMetroFramework
{
    public partial class FrmTestMetroframework : MetroForm
    {

        BindingList<DetalleGrilla> _lista;

        public FrmTestMetroframework()
        {
            InitializeComponent();
            metroStyleManager1.Theme = MetroFramework.MetroThemeStyle.Dark;
            metroStyleManager1.Style = MetroFramework.MetroColorStyle.Lime;
            PicImagenProducto.Image = RoundCorners(Image.FromFile(@"d:\recibir\7891150043237.png"),500);
        }
        private void FrmTestMetroframework_Load(object sender, EventArgs e)
        {
            CmbComprobante.SelectedIndex = 0;
            CmbConndicionVenta.SelectedIndex = 0;
            CmbListaPrecio.SelectedIndex = 0;
            CmbVendedor.SelectedIndex = 0;

            LoadData();
            GridArticulos.AutoGenerateColumns = false;
            GridArticulos.Columns["ColCodigo"].DataPropertyName = "CodArticulo";
            GridArticulos.Columns["ColDescripcion"].DataPropertyName = "Descripcion";
            GridArticulos.Columns["ColCantidad"].DataPropertyName = "Cantidad";
            GridArticulos.Columns["ColUnitario"].DataPropertyName = "Unitario";
            GridArticulos.Columns["ColDescuento"].DataPropertyName = "Descuento";
            GridArticulos.Columns["ColTotal"].DataPropertyName = "Total";


            GridArticulos.DataSource = _lista;
        }

        private void LoadData()
        {
            _lista = new BindingList<DetalleGrilla>();

            _lista.Add(
                        new DetalleGrilla
                        {
                            CodArticulo = "7891150043237",
                            Descripcion = "ACONDICIONADOR DAVE RECUPERACION EXTREMA x 900 ml",
                            Cantidad = 1,
                            Unitario = 289,
                            Descuento = 0,
                            Total = 289
                        }
            );

            _lista.Add(
                        new DetalleGrilla
                        {
                            CodArticulo = "7891167021013",
                            Descripcion = "SARDINAS EN ACEITE GOMES da COSTA x 125 gr",
                            Cantidad = 3,
                            Unitario = 50,
                            Descuento = 0,
                            Total = 150,
                        }
            );

            _lista.Add(
                        new DetalleGrilla
                        {
                            CodArticulo = "7898024390107",
                            Descripcion = "FERRERO ROCHER BOMBOM CAJA x 12 ud",
                            Cantidad = 2,
                            Unitario = 459,
                            Descuento = 0,
                            Total = 918
                        }
            );

            _lista.Add(
                        new DetalleGrilla
                        {
                            CodArticulo = "8718696658086",
                            Descripcion = "LAMPARA PHILIPS 15W x ud",
                            Cantidad = 3,
                            Unitario = 177,
                            Descuento = 0,
                            Total = 531
                        }
            );

        }

        private Image RoundCorners(Image image, int cornerRadius)
        {
            cornerRadius *= 2;
            Bitmap roundedImage = new Bitmap(image.Width, image.Height);
            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(0, 0, cornerRadius, cornerRadius, 180, 90);
            gp.AddArc(0 + roundedImage.Width - cornerRadius, 0, cornerRadius, cornerRadius, 270, 90);
            gp.AddArc(0 + roundedImage.Width - cornerRadius, 0 + roundedImage.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
            gp.AddArc(0, 0 + roundedImage.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
            using (Graphics g = Graphics.FromImage(roundedImage))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.SetClip(gp);
                g.DrawImage(image, Point.Empty);
            }
            return roundedImage;
        }

        private void GridArticulos_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            DetalleGrilla cust = GridArticulos.Rows[e.RowIndex].DataBoundItem as DetalleGrilla;
            if (cust != null)
            {
                PicImagenProducto.Image = RoundCorners(Image.FromFile($@"d:\recibir\{cust.CodArticulo}.png"), 500);
                pictureBoxRounded1.Image = Image.FromFile($@"d:\recibir\{cust.CodArticulo}.png");

                LblImporteArticulo.Text = string.Format("{0:######0.00}", cust.Unitario);
                LblDescripcionArticulo.Text = string.Format("{0:######0.00}", cust.Cantidad) + " x " + cust.Descripcion.ToUpper();
            }

        }

        private void GridArticulos_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            PicImagenProducto.Image = null;
            LblImporteArticulo.Text = "";
            LblDescripcionArticulo.Text = "";

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            metroStyleManager1.Theme = metroStyleManager1.Theme == MetroFramework.MetroThemeStyle.Dark ? MetroFramework.MetroThemeStyle.Light : MetroFramework.MetroThemeStyle.Dark;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            var m = new Random();
            int next = m.Next(0, 13);
            metroStyleManager1.Style = (MetroColorStyle)next;

        }
    }

    public class DetalleGrilla
    {
        public string CodArticulo { get; set; }
        public string Descripcion { get; set; }
        public Decimal Cantidad { get; set; }
        public Decimal Unitario { get; set; }
        public Decimal Descuento { get; set; }
        public Decimal Total { get; set; }
        public string  RutaImagen { get; set; }
    }
}
