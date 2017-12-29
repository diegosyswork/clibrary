using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SysWork.Controls
{
    public partial class TextBoxWithHint : TextBox
    {
        private string _hint = "";
        public string Hint
        {
            get { return _hint; }
            set { _hint = value; }
        }

        private bool _hintActivo = true;
        public bool HintActivo
        {
            get { return _hintActivo; }
            set { _hintActivo = value; }
        }

        public TextBoxWithHint()
        {
            this._hintActivo = true;

            this.Text = _hint;
            this.ForeColor = Color.Gray;

            GotFocus += (source, e) =>
            {
                RemoverHint();
            };

            LostFocus += (source, e) =>
            {
                MostrarHint();
            };

        }

        public void RemoverHint()
        {
            if (this._hintActivo)
            {
                this._hintActivo = false;
                this.Text = "";
                this.ForeColor = Color.Black;
            }
        }

        public void MostrarHint()
        {
            if (!this._hintActivo && string.IsNullOrEmpty(this.Text) || ForeColor == Color.Gray)
            {
                this._hintActivo = true;
                this.Text = _hint;
                this.ForeColor = Color.Gray;
            }
        }

        public void AplicarHint(string newText)
        {
            Hint = newText;
            MostrarHint();
        }
    }
}
