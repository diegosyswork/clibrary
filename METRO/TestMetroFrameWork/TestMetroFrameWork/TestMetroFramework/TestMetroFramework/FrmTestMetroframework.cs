using MetroFramework;
using MetroFramework.Forms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using SysWork.MetroControls.MetroToolbars;

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
            GridArticulos.Columns["ColLista"].DataPropertyName = "Unitario";
            GridArticulos.Columns["ColCantidad"].DataPropertyName = "Cantidad";
            GridArticulos.Columns["ColUnitario"].DataPropertyName = "Unitario";
            GridArticulos.Columns["ColDescuento"].DataPropertyName = "Descuento";
            GridArticulos.Columns["ColTotal"].DataPropertyName = "Total";

            GridArticulos.DataSource = _lista;

            LblImporteTotal.Text = _lista.Sum(a => a.Total).ToString("$ ###,###,##0.00");

            metroToolbarCRUD1.Theme = metroStyleManager1.Theme == MetroThemeStyle.Dark ? MetroTheme.Dark : MetroTheme.Light;
            metroToolbarDisplaySettings1.Theme = metroToolbarCRUD1.Theme;

            this.UseEnterKeyToValidate = true;
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

        private void TxtCodigoCliente_Validating(object sender, CancelEventArgs e)
        {
            errorProvider1.SetError(TxtCodigoCliente, "Error en la carga del cliente");
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            //TestDialog t = new TestDialog();
            //t.ShowDialog();

            MetroMessageBox.Show(this,"No hay conexion con AFIP, reintenta en unos momentos", "Aviso al operador", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MetroMessageBox.Show(this, "No hay conexion con AFIP, reintenta en unos momentos", "Aviso al operador", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            MetroMessageBox.Show(this, "No hay conexion con AFIP, reintenta en unos momentos", "Aviso al operador", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            MetroMessageBox.Show(this, "No hay conexion con AFIP, reintenta en unos momentos", "Aviso al operador", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            MetroMessageBox.Show(this, "No hay conexion con AFIP, reintenta en unos momentos", "Aviso al operador", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);
            MetroMessageBox.Show(this, "No hay conexion con AFIP, reintenta en unos momentos", "Aviso al operador", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand);
            MetroMessageBox.Show(this, "No hay conexion con AFIP, reintenta en unos momentos", "Aviso al operador", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
            MetroMessageBox.Show(this, "No hay conexion con AFIP, reintenta en unos momentos", "Aviso al operador", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

        }

        private void metroToolbarDisplaySettings1_ActionSelected(object sender, MetroToolbarDisplaySettingsClickEventArgs e)
        {
            switch (e.Action)
            {
                case MetroToolbarDisplaySettingsAction.ChangeTheme:
                    metroStyleManager1.Theme = metroStyleManager1.Theme == MetroThemeStyle.Dark ? MetroFramework.MetroThemeStyle.Light : MetroFramework.MetroThemeStyle.Dark;
                    metroToolbarCRUD1.Theme = metroStyleManager1.Theme == MetroThemeStyle.Dark ? MetroTheme.Dark : MetroTheme.Light;
                    metroToolbarDisplaySettings1.Theme = metroToolbarCRUD1.Theme;
                    break;
                case MetroToolbarDisplaySettingsAction.ChangeStyle:
                        var m = new Random();
                        int next = m.Next(0, 13);
                        var style = (MetroColorStyle)next;
                        metroStyleManager1.Style = style;
                    break;
                default:
                    break;
            }
        }

        private void metroToolbarCRUD1_ActionSelected(object sender, MetroToolbarCRUDlickEventArgs e)
        {
            switch (e.Action)
            {
                case MetroToolbarCRUDAction.New:
                    var f = new FrmTestPrincipal();
                    f.Show();
                    break;
                case MetroToolbarCRUDAction.Delete:
                    break;
                case MetroToolbarCRUDAction.Refresh:
                    break;
                case MetroToolbarCRUDAction.Search:
                    break;
                case MetroToolbarCRUDAction.ImportExport:
                    break;
                case MetroToolbarCRUDAction.Report:
                    var r = new FrmTestReport();
                    r.SetAppearance(this.metroStyleManager1.Theme, this.metroStyleManager1.Style);
                    r.Show();
                    break;
                case MetroToolbarCRUDAction.Initialize:
                    break;
                case MetroToolbarCRUDAction.Save:
                    break;
                case MetroToolbarCRUDAction.Exit:
                    break;
                default:
                    break;
            }
        }

        private void TxtCodigoCliente_ButtonClick(object sender, EventArgs e)
        {
            MetroMessageBox.Show(this, "Seleccionaste el cliente", "Carga de comprobantes de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    public class DetalleGrilla
    {
        public string CodArticulo { get; set; }
        public string Descripcion { get; set; }
        public string CodLista { get; set; }
        public Decimal Cantidad { get; set; }
        public Decimal Unitario { get; set; }
        public Decimal Descuento { get; set; }
        public Decimal Total { get; set; }
        public string  RutaImagen { get; set; }
    }
}
