﻿#region Imports

using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

#endregion

namespace ReaLTaiizor
{
    #region HopeSwitch

    public class HopeSwitch : System.Windows.Forms.CheckBox
    {
        #region Variables

        Timer AnimationTimer = new Timer { Interval = 1 };
        int PointAnimationNum = 3;

        #endregion

        #region Settings
        #endregion

        #region Events

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            AnimationTimer.Start();
        }

        protected override void OnResize(EventArgs e)
        {
            Height = 20; Width = 40;
            Invalidate();
        }

        #endregion

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var graphics = pevent.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            graphics.InterpolationMode = InterpolationMode.High;
            graphics.Clear(Parent.BackColor);

            var backRect = new GraphicsPath();
            backRect.AddArc(new RectangleF(0.5f, 0.5f, Height - 1, Height - 1), 90, 180);
            backRect.AddArc(new RectangleF(Width - Height + 0.5f, 0.5f, Height - 1, Height - 1), 270, 180);
            backRect.CloseAllFigures();

            graphics.FillPath(new SolidBrush(Checked ? BackColor : HopeColors.OneLevelBorder), backRect);
            graphics.FillEllipse(new SolidBrush(Color.White), new RectangleF(PointAnimationNum, 2, 16, 16));
        }

        public HopeSwitch()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
            Height = 20; Width = 42;
            AnimationTimer.Tick += new EventHandler(AnimationTick);
            Cursor = Cursors.Hand;
            BackColor = HopeColors.PrimaryColor;
        }

        void AnimationTick(object sender, EventArgs e)
        {
            if (Checked)
            {
                if (PointAnimationNum < 21)
                {
                    PointAnimationNum += 2;
                    Invalidate();
                }
            }
            else if (PointAnimationNum > 3)
            {
                PointAnimationNum -= 2;
                Invalidate();
            }
        }
    }

    #endregion
}