/**
 * MetroSet UI - MetroSet UI Framewrok
 * 
 * The MIT License (MIT)
 * Copyright (c) 2017 Narwin, https://github.com/N-a-r-w-i-n
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in the 
 * Software without restriction, including without limitation the rights to use, copy, 
 * modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the 
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using MetroSet_UI.Controls;
using MetroSet_UI.Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MetroSet_UI.Forms
{
    public class MetroSetMessageBox : MetroSetForm
    {

        #region Internal vars

        private Size _buttonSize;
        private MetroDefaultSetButton _okButton;
        private MetroDefaultSetButton _yesButton;
        private MetroDefaultSetButton _noButton;
        private MetroDefaultSetButton _cancelButton;
        private MetroDefaultSetButton _retryButton;
        private MetroDefaultSetButton _abortButton;
        private MetroDefaultSetButton metroDefaultSetButton1;
        private MetroSetDivider metroSetDivider1;
        private MetroSetControlBox metroSetControlBox1;
        private MetroDefaultSetButton _ignoreButton;

        #endregion

        #region Properties

        /// <summary>
        /// Get or sets the parent form.
        /// </summary>
        private Form OwnerForm { get; set; }

        /// <summary>
        /// Gets or sets the content of the message.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the title of the content
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the MessageBoxButtons.
        /// </summary> 
        public MessageBoxButtons Buttons { get; set; }

        /// <summary>
        /// Gets or sets the MessageBoxIcon.
        /// </summary>
        public new MessageBoxIcon Icon { get; set; }

        /// <summary>
        /// Gets or sets the BackgroundColor
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private new static Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the BorderColor
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private new static Color BorderColor { get; set; }

        /// <summary>
        /// Gets or sets the ForegroundColor
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private static Color ForegroundColor { get; set; }


        #endregion

        #region Constructor

        /// <summary>
        /// The Constructor.
        /// </summary>
        private MetroSetMessageBox() : base()
        {
            Font = MetroSetFonts.Regular(9.5f);
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            _buttonSize = new Size(95, 32);
            ApplyTheme();
            EvaluateControls();
            AddControls();
            //HideControls();
        }

        /// <summary>
        /// Here we set the buttons properties value.
        /// </summary>
        private void EvaluateControls()
        {
            EvaluateOkeyButton();

            EvaluateYesButton();

            EvaluateNoButton();

            EvaluateCancelButton();

            EvaluateRetryButton();

            EvaluateAbortButton();

            EvaluateIgnoreButton();
        }

        /// <summary>
        /// Adding the controls just to be exist in form but we don't need them all at the moment.
        /// </summary>
        private void AddControls()
        {
            Controls.Add(_okButton);
            Controls.Add(_yesButton);
            Controls.Add(_noButton);
            Controls.Add(_cancelButton);
            Controls.Add(_retryButton);
            Controls.Add(_abortButton);
            Controls.Add(_ignoreButton);
        }

        /// <summary>
        /// Set the required properties values and click event of retry button.
        /// </summary>
        private void EvaluateRetryButton()
        {
            _retryButton = new MetroDefaultSetButton
            {
                Text = "Retry",
                Size = _buttonSize,
                Visible = false
            };
            _retryButton.Click += RetryButton_Click;
        }

        /// <summary>
        /// Set the required properties values and click event of cancel button.
        /// </summary>
        private void EvaluateCancelButton()
        {
            _cancelButton = new MetroDefaultSetButton
            {
                Text = "Cancel",
                Size = _buttonSize,
                Visible = false
            };
            _cancelButton.Click += CancelButton_Click;
        }

        /// <summary>
        /// Set the required properties values and click event of no button.
        /// </summary>
        private void EvaluateNoButton()
        {
            _noButton = new MetroDefaultSetButton
            {
                Text = "No",
                Size = _buttonSize,
                Visible = false
            };
            _noButton.Click += NoButton_Click;
        }

        /// <summary>
        /// Set the required properties values and click event of yes button.
        /// </summary>
        private void EvaluateYesButton()
        {
            _yesButton = new MetroDefaultSetButton
            {
                Text = "Yes",
                Size = _buttonSize,
                Visible = false
            };
            _yesButton.Click += YesButton_Click;
        }

        /// <summary>
        /// Set the required properties values and click event of ok button.
        /// </summary>
        private void EvaluateOkeyButton()
        {
            _okButton = new MetroDefaultSetButton
            {
                Text = "Ok",
                Size = _buttonSize,
                Visible = false
            };
            _okButton.Click += OkButton_Click;
        }

        /// <summary>
        /// Set the required properties values and click event of abort button.
        /// </summary>
        private void EvaluateAbortButton()
        {
            _abortButton = new MetroDefaultSetButton
            {
                Text = "Abort",
                Size = _buttonSize,
                Visible = false

            };
            _abortButton.Click += AbortButton_Click;
        }

        /// <summary>
        /// Set the required properties values and click event of ignore button.
        /// </summary>
        private void EvaluateIgnoreButton()
        {
            _ignoreButton = new MetroDefaultSetButton
            {
                Text = "Ignore",
                Size = _buttonSize,
                Visible = false
            };
            _ignoreButton.Click += IgnoreButton_Click;
        }

        #endregion

        #region Events

        /// <summary>
        /// Handling the retry button click.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void RetryButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Retry;
        }

        /// <summary>
        /// Handling the cancel button click.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Handling the no button click.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void NoButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }

        /// <summary>
        /// Handling the yes button click.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void YesButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
        }

        /// <summary>
        /// Handling the okey button click.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void OkButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Handling the abort button click.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void AbortButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
        }

        /// <summary>
        /// Handling the ignore button click.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void IgnoreButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Ignore;
        }

        /// <summary>
        /// When the user just provides the content of message to appear.
        /// </summary>
        /// <param name="form">The Form that messagebox will be showed from.</param>
        /// <param name="content">The Content of the message.</param>
        /// <returns>The MessageBox with just the content and an ok button.</returns>
        public static DialogResult Show(MetroSetForm form, string content)
        {
            return Show(form, content, form.Text, MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        /// <summary>
        ///  When the user provides the content of message and the message title to appear.
        /// </summary>
        /// <param name="form">The Form that messagebox will be showed from.</param>
        /// <param name="content">The Content of the message.</param>
        /// <param name="caption">The MesageBox title.</param>
        /// <returns>The MessageBox with the content and title and an ok button.</returns>
        public static DialogResult Show(MetroSetForm form, string content, string caption)
        {
            return Show(form, content, caption, MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        /// <summary>
        /// When the user provides the content of message and the message title and also which type of buttons to appear.
        /// </summary>
        /// <param name="form">The Form that messagebox will be showed from.</param>
        /// <param name="content">The Content of the message.</param>
        /// <param name="caption">The MesageBox title.</param>
        /// <param name="buttons">The Type of buttons to appear.</param>
        /// <returns>The MessageBox with the content and title and provided button(s) type.</returns>
        public static DialogResult Show(MetroSetForm form, string content, string caption, MessageBoxButtons buttons)
        {
            return Show(form, content, caption, buttons, MessageBoxIcon.None);
        }

        /// <summary>
        /// When the user provides the content of message and the message title and also which type message and buttons to appear.
        /// </summary>
        /// <param name="form">The Form that messagebox will be showed from.</param>
        /// <param name="content">The Content of the message.</param>
        /// <param name="caption">The MesageBox title.</param>
        /// <param name="buttons">The Type of buttons to appear.</param>
        /// <param name="icon">The MessageBox type.</param>
        /// <returns>The MessageBox with the content and title and provided button(s) and type.</returns>
        public static DialogResult Show(MetroSetForm form, string content, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            var msgBox = new MetroSetMessageBox
            {
                OwnerForm = form ?? throw new ArgumentNullException("MetroSetMessageBox requires a form, use 'this' as the first parameter in the place you use MetroSetMessageBox."),
                Content = content,
                Caption = caption,
                Buttons = buttons,
                Size = new Size(form.Width - 2, (form.Height / 3) - 1),
                Location = new Point(form.Location.X, (form.Height / 2) - 1)
            };

            if (icon == MessageBoxIcon.Error || icon == MessageBoxIcon.Stop)
            {
                BackgroundColor = Color.FromArgb(210, 50, 45);
                BorderColor = Color.FromArgb(210, 50, 45);
                ForegroundColor = Color.Black;
            }

            else if (icon == MessageBoxIcon.Information)
            {
                BackgroundColor = Color.FromArgb(60, 180, 218);
                BorderColor = Color.FromArgb(60, 180, 218);
                ForegroundColor = Color.White;
            }

            else if (icon == MessageBoxIcon.Question)
            {
                BackgroundColor = Color.FromArgb(70, 165, 70);
                BorderColor = Color.FromArgb(70, 165, 70);
                ForegroundColor = Color.Black;
            }

            else if (icon == MessageBoxIcon.Exclamation || icon == MessageBoxIcon.Warning)
            {
                BackgroundColor = Color.FromArgb(237, 156, 40);
                BorderColor = Color.FromArgb(237, 156, 40);
                ForegroundColor = Color.Black;
            }

            else if (icon == MessageBoxIcon.None || icon == MessageBoxIcon.Asterisk || icon == MessageBoxIcon.Hand)
            {
                BackgroundColor = Color.White;
                BorderColor = Color.FromArgb(65, 177, 225);
                ForegroundColor = Color.Black;
            }

            return msgBox.ShowDialog();
        }

        /// <summary>
        /// Here we handle the user provided buttons appearance.
        /// </summary>
        /// <returns>The MessageBox with provided buttons.</returns>
        protected new DialogResult ShowDialog()
        {

            var buttonHeight = Height - 45;
            var firstButton = (Width - _buttonSize.Width) - 10;
            var secondButoon = (Width - (_buttonSize.Width * 2)) - 20;
            switch (Buttons)
            {
                case MessageBoxButtons.OK:
                    _okButton.Location = new Point(firstButton, buttonHeight);
                    _okButton.Visible = true;
                    break;

                case MessageBoxButtons.OKCancel:
                    _okButton.Location = new Point(secondButoon, buttonHeight);
                    _okButton.Visible = true;
                    _cancelButton.Location = new Point(firstButton, buttonHeight);
                    _cancelButton.Visible = true;
                    break;

                case MessageBoxButtons.YesNo:
                    _yesButton.Location = new Point(secondButoon, buttonHeight);
                    _yesButton.Visible = true;
                    _noButton.Location = new Point(firstButton, buttonHeight);
                    _noButton.Visible = true;
                    break;

                case MessageBoxButtons.YesNoCancel:
                    _yesButton.Location = new Point((Width - (_buttonSize.Width * 3)) - 30, buttonHeight);
                    _yesButton.Visible = true;
                    _noButton.Location = new Point(secondButoon, buttonHeight);
                    _noButton.Visible = true;
                    _cancelButton.Location = new Point(firstButton, buttonHeight);
                    _cancelButton.Visible = true;
                    break;

                case MessageBoxButtons.RetryCancel:
                    _retryButton.Location = new Point(secondButoon, buttonHeight);
                    _retryButton.Visible = true;
                    _cancelButton.Location = new Point(firstButton, buttonHeight);
                    _cancelButton.Visible = true;
                    break;

                case MessageBoxButtons.AbortRetryIgnore:
                    _abortButton.Location = new Point((Width - (_buttonSize.Width * 3)) - 30, buttonHeight);
                    _abortButton.Visible = true;
                    _retryButton.Location = new Point(secondButoon, buttonHeight);
                    _retryButton.Visible = true;
                    _ignoreButton.Location = new Point(firstButton, buttonHeight);
                    _ignoreButton.Visible = true;
                    break;

                default:
                    _okButton.Location = new Point(firstButton, buttonHeight);
                    _okButton.Visible = true;
                    break;
            }
            return base.ShowDialog();
        }

        #endregion

        #region Draw Dialog

        protected override void OnPaint(PaintEventArgs e)
        {
            var G = e.Graphics;
            G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var rect = new Rectangle(0, ((OwnerForm.Height - (OwnerForm.Height / 2)) / 250), OwnerForm.Width - 3, (OwnerForm.Height / 3) - 3);

            using (var bg = new SolidBrush(BackgroundColor))
            {
                using (var CTNT = new SolidBrush(ForegroundColor))
                {
                    using (var p = new Pen(BorderColor))
                    {
                        G.FillRectangle(bg, rect);
                        G.DrawString(Caption, Font, CTNT, new PointF(rect.X + 10, rect.Y + 10));
                        G.DrawString(Content, Font, CTNT, new PointF(rect.X + 10, rect.Y + 50));
                        G.DrawRectangle(p, rect);
                    }
                }
            }
        }

        #endregion

        private void InitializeComponent()
        {
            this.metroDefaultSetButton1 = new MetroSet_UI.Controls.MetroDefaultSetButton();
            this.metroSetDivider1 = new MetroSet_UI.Controls.MetroSetDivider();
            this.metroSetControlBox1 = new MetroSet_UI.Controls.MetroSetControlBox();
            this.SuspendLayout();
            // 
            // metroDefaultSetButton1
            // 
            this.metroDefaultSetButton1.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.metroDefaultSetButton1.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.metroDefaultSetButton1.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.metroDefaultSetButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.metroDefaultSetButton1.HoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.metroDefaultSetButton1.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.metroDefaultSetButton1.HoverTextColor = System.Drawing.Color.White;
            this.metroDefaultSetButton1.Location = new System.Drawing.Point(48, 122);
            this.metroDefaultSetButton1.Name = "metroDefaultSetButton1";
            this.metroDefaultSetButton1.NormalBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.metroDefaultSetButton1.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.metroDefaultSetButton1.NormalTextColor = System.Drawing.Color.Black;
            this.metroDefaultSetButton1.PressBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.metroDefaultSetButton1.PressColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.metroDefaultSetButton1.PressTextColor = System.Drawing.Color.White;
            this.metroDefaultSetButton1.Size = new System.Drawing.Size(159, 82);
            this.metroDefaultSetButton1.Style = MetroSet_UI.Design.Style.Light;
            this.metroDefaultSetButton1.StyleManager = null;
            this.metroDefaultSetButton1.TabIndex = 0;
            this.metroDefaultSetButton1.Text = "metroDefaultSetButton1";
            this.metroDefaultSetButton1.ThemeAuthor = "Narwin";
            this.metroDefaultSetButton1.ThemeName = "MetroLite";
            // 
            // metroSetDivider1
            // 
            this.metroSetDivider1.Location = new System.Drawing.Point(37, 218);
            this.metroSetDivider1.Name = "metroSetDivider1";
            this.metroSetDivider1.Orientation = MetroSet_UI.Enums.DividerStyle.Horizontal;
            this.metroSetDivider1.Size = new System.Drawing.Size(117, 4);
            this.metroSetDivider1.Style = MetroSet_UI.Design.Style.Light;
            this.metroSetDivider1.StyleManager = null;
            this.metroSetDivider1.TabIndex = 1;
            this.metroSetDivider1.Text = "metroSetDivider1";
            this.metroSetDivider1.ThemeAuthor = "Narwin";
            this.metroSetDivider1.ThemeName = "MetroLite";
            this.metroSetDivider1.Thickness = 1;
            // 
            // metroSetControlBox1
            // 
            this.metroSetControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.metroSetControlBox1.CloseHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.metroSetControlBox1.CloseHoverForeColor = System.Drawing.Color.White;
            this.metroSetControlBox1.CloseNormalForeColor = System.Drawing.Color.Gray;
            this.metroSetControlBox1.DisabledForeColor = System.Drawing.Color.DimGray;
            this.metroSetControlBox1.Location = new System.Drawing.Point(41, 249);
            this.metroSetControlBox1.MaximizeBox = true;
            this.metroSetControlBox1.MaximizeHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.metroSetControlBox1.MaximizeHoverForeColor = System.Drawing.Color.Gray;
            this.metroSetControlBox1.MaximizeNormalForeColor = System.Drawing.Color.Gray;
            this.metroSetControlBox1.MinimizeBox = true;
            this.metroSetControlBox1.MinimizeHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.metroSetControlBox1.MinimizeHoverForeColor = System.Drawing.Color.Gray;
            this.metroSetControlBox1.MinimizeNormalForeColor = System.Drawing.Color.Gray;
            this.metroSetControlBox1.Name = "metroSetControlBox1";
            this.metroSetControlBox1.Size = new System.Drawing.Size(100, 25);
            this.metroSetControlBox1.Style = MetroSet_UI.Design.Style.Light;
            this.metroSetControlBox1.StyleManager = null;
            this.metroSetControlBox1.TabIndex = 2;
            this.metroSetControlBox1.Text = "metroSetControlBox1";
            this.metroSetControlBox1.ThemeAuthor = "Narwin";
            this.metroSetControlBox1.ThemeName = "MetroLite";
            // 
            // MetroSetMessageBox
            // 
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Controls.Add(this.metroSetControlBox1);
            this.Controls.Add(this.metroSetDivider1);
            this.Controls.Add(this.metroDefaultSetButton1);
            this.Name = "MetroSetMessageBox";
            this.ResumeLayout(false);

        }
    }
}
