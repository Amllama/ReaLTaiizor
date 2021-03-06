﻿#region Imports

using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Drawing2D;

#endregion

namespace ReaLTaiizor.Controls
{
    #region SkyCheckBox

    [DefaultEvent("CheckedChanged")]
    public class SkyCheckBox : Control
    {
        #region " Control Help - MouseState & Flicker Control"
        private Point mouse = new Point(0, 0);

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            mouse = e.Location;
        }
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        private bool _Checked;
        public bool Checked
        {
            get => _Checked;
            set
            {
                _Checked = value;
                CheckedChanged?.Invoke(this);
                Invalidate();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Height = 14;
        }

        protected override void OnClick(EventArgs e)
        {
            if (mouse.X <= Height - 1 || mouse.Y <= Width - 1)
            {
                Checked = !Checked;
                Invalidate();
            }

            base.OnClick(e);
        }

        public event CheckedChangedEventHandler CheckedChanged;
        public delegate void CheckedChangedEventHandler(object sender);
        #endregion

        #region Variables
        private SmoothingMode _SmoothingType = SmoothingMode.HighQuality;

        private Color _BoxBGColorA = Color.FromArgb(245, 245, 245);
        private Color _BoxBGColorB = Color.FromArgb(231, 231, 231);
        private Color _BoxBorderColorA = Color.FromArgb(189, 189, 189);
        private Color _BoxBorderColorB = Color.FromArgb(252, 252, 252);
        private Color _BoxBorderColorC = Color.FromArgb(168, 168, 168);
        private Color _CheckedColor = Color.FromArgb(220, 27, 94, 137);
        #endregion

        #region Settings
        public SmoothingMode SmoothingType
        {
            get => _SmoothingType;
            set
            {
                _SmoothingType = value;
                Invalidate();
            }
        }

        public Color BoxBGColorA
        {
            get => _BoxBGColorA;
            set => _BoxBGColorA = value;
        }

        public Color BoxBGColorB
        {
            get => _BoxBGColorB;
            set => _BoxBGColorB = value;
        }

        public Color BoxBorderColorA
        {
            get => _BoxBorderColorA;
            set => _BoxBorderColorA = value;
        }

        public Color BoxBorderColorB
        {
            get => _BoxBorderColorB;
            set => _BoxBorderColorB = value;
        }

        public Color BoxBorderColorC
        {
            get => _BoxBorderColorC;
            set => _BoxBorderColorC = value;
        }

        public Color CheckedColor
        {
            get => _CheckedColor;
            set => _CheckedColor = value;
        }
        #endregion

        public SkyCheckBox() : base()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
            Font = new Font("Verdana", 6.75f, FontStyle.Bold);
            BackColor = Color.Transparent;
            ForeColor = Color.FromArgb(27, 94, 137);
            Size = new Size(90, 16);
            DoubleBuffered = true;
            Cursor = Cursors.Hand;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);

            Rectangle checkBoxRectangle = new Rectangle(0, 0, Height - 1, Height - 1);
            G.SmoothingMode = SmoothingType;

            G.Clear(Parent.FindForm().BackColor);

            LinearGradientBrush bodyGrad = new LinearGradientBrush(checkBoxRectangle, BoxBGColorA, BoxBGColorB, 90);
            G.FillRectangle(bodyGrad, bodyGrad.Rectangle);
            G.DrawRectangle(new Pen(BoxBorderColorA), new Rectangle(0, 0, Height - 1, Height - 2));
            G.DrawRectangle(new Pen(BoxBorderColorB), new Rectangle(1, 1, Height - 3, Height - 4));
            G.DrawLine(new Pen(BoxBorderColorC), new Point(1, Height - 1), new Point(Height - 2, Height - 1));

            if (Checked)
            {
                Rectangle chkPoly = new Rectangle(checkBoxRectangle.X + checkBoxRectangle.Width / 4, checkBoxRectangle.Y + checkBoxRectangle.Height / 4, checkBoxRectangle.Width / 2, checkBoxRectangle.Height / 2);
                Point[] Poly =
                {
                    new Point(chkPoly.X, chkPoly.Y + chkPoly.Height / 2),
                    new Point(chkPoly.X + chkPoly.Width / 2, chkPoly.Y + chkPoly.Height),
                    new Point(chkPoly.X + chkPoly.Width, chkPoly.Y)
                };
                G.SmoothingMode = SmoothingType;

                G.DrawString("a", new Font("Marlett", 10.75f), new SolidBrush(CheckedColor), new Rectangle(-2, -1, Width - 1, Height - 1), new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Near
                });
            }

            G.DrawString(Text, Font, new SolidBrush(ForeColor), new Point(16, 0), new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Near
            });

            e.Graphics.DrawImage(B, 0, 0);
            G.Dispose();
            B.Dispose();

        }
    }

    #endregion
}