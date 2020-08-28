using MetroFramework;
using MetroFramework.Forms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
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
        }
        private void FrmTestMetroframework_Load(object sender, EventArgs e)
        {
            CmbComprobante.SelectedIndex = 0;
            CmbCondicionVenta.SelectedIndex = 0;
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

            LblImporteTotal.Text = _lista.Sum(a => a.Total).ToString("$ ###,###,##0.00");
        }

        private void LoadData()
        {
            _lista = new BindingList<DetalleGrilla>();

            _lista.Add(
                        new DetalleGrilla
                        {CodArticulo = "7891150043237",Descripcion = "ACONDICIONADOR DAVE RECUPERACION EXTREMA x 900 ml",                            Cantidad = 1,
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
                            Unitario = 55.123456M,
                            Descuento = 0,
                            Total = 165.370368M
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
            Bitmap roundedImage = new Bitmap(image.Width, image.Height);

            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddEllipse(0, 0, image.Width, image.Height);

            using (Graphics g = Graphics.FromImage(roundedImage))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.SetClip(graphicsPath);
                g.DrawImage(image, Point.Empty);
            }
            return roundedImage;
        }

        private void GridArticulos_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            DetalleGrilla cust = GridArticulos.Rows[e.RowIndex].DataBoundItem as DetalleGrilla;
            if (cust != null)
            {
                PicImagenArticuloPOS.Image = RoundCorners(Image.FromFile($@"d:\recibir\{cust.CodArticulo}.png"), 500);

                LblImporteArticuloPOS.Text = string.Format("{0:###,###,##0.00}", cust.Unitario);
                LblDescripcionArticuloPOS.Text = string.Format("{0:###,###,##0.00}", cust.Cantidad) + " x " + cust.Descripcion.ToUpper();
            }
        }

        private void GridArticulos_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            PicImagenArticuloPOS.Image = null;
            LblImporteArticuloPOS.Text = "";
            LblDescripcionArticuloPOS.Text = "";

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

        private void TxtCodigoCliente_Validating(object sender, CancelEventArgs e)
        {
            errorProvider1.SetError(TxtCodigoCliente, "errr");
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
