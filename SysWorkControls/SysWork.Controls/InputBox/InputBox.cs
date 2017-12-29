using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysWork.Controls.InputBox
{
    public enum EIcono 
    {
        consulta,
        prohibido,
        cancelar,
        refresh,
        ok,
        exclamacion,
        custom
    }

    public class InputBox
    {
        private static FrmInputBox f;
        private static int originalTxtInputHeight;
        
        private InputBox() 
        {
        }

        private static void InicializaFormulario() 
        {
            f = new FrmInputBox();
            originalTxtInputHeight = f.txtInput.Height;
        }

        public static DialogResult Show(string title, string prompt, string defaultText, int maxLines, EIcono icono)
        {

            Title = title;
            Prompt = prompt;
            Text = defaultText;
            MaxLines = maxLines;
            Icono = icono;

            return Show();
        }
        public static DialogResult Show(string title, string prompt, EIcono icono)
        {

            Title = title;
            Prompt = prompt;
            Icono = icono;

            return Show();
        }

        public static DialogResult Show() 
        {
            InicializaFormulario();

            if (_maxLines > 1)
                f.AcceptButton = null;
            
            f.txtInput.Text = _text;
            f.Text = _title;
            f.lblPrompt.Text = _prompt;

            f.txtInput.Multiline = (_maxLines > 1);
            f.txtInput.ScrollBars = (_maxLines > 1) ? System.Windows.Forms.ScrollBars.Vertical : System.Windows.Forms.ScrollBars.None;
            f.txtInput.Height = originalTxtInputHeight * _maxLines;

            switch (_icono)
            {
                case EIcono.consulta:
                    f.pictureBox1.Image = f.imgLst.Images[0];
                    break;
                case EIcono.prohibido:
                    f.pictureBox1.Image = f.imgLst.Images[1];
                    break;
                case EIcono.cancelar:
                    f.pictureBox1.Image = f.imgLst.Images[2];
                    break;
                case EIcono.refresh:
                    f.pictureBox1.Image = f.imgLst.Images[3];
                    break;
                case EIcono.ok:
                    f.pictureBox1.Image = f.imgLst.Images[4];
                    break;
                case EIcono.exclamacion:
                    f.pictureBox1.Image = f.imgLst.Images[5];
                    break;
                case EIcono.custom:
                    f.pictureBox1.Image = _customImage;
                    break;
                default:
                    f.pictureBox1.Image = f.imgLst.Images[0];
                    break;

            }
            
            f.ShowInTaskbar = false;
            
            f.ShowDialog();

            _text = f.txtInput.Text;
            f.txtInput.Text = "";

            f.Dispose();

            return f.DialogResult;
        }


        private static string _title;
        public static string Title 
        {
            get {return _title;}
            set 
            {
                _title = value;
            }
        }

        private static string _prompt;
        public static string Prompt
        {
            get { return _prompt; }
            set 
            { 
                _prompt = value;
            } 
        }
        private static string _text;
        public static string Text
        {
            get 
            {
                return _text;
            }
            set 
            { 
                _text = value;
            } 
        }

        private static int _maxLines = 1;
        public static int MaxLines 
        { 
            get{return _maxLines;}
            set
            {
                if (value <= 0 || value > 20)
                    throw new ArgumentOutOfRangeException("MaxLines","el rango valido es entre 1 y 20");

                _maxLines = value;
            }
        }

        private static EIcono _icono = EIcono.consulta;
        public static EIcono Icono
        {
            set
            {
                _icono = value;
            }
        }
        private static System.Drawing.Image _customImage = null;
        public static System.Drawing.Image CustomImage 
        {
            set 
            {
                _customImage = value;
            }
        }

    }
}
