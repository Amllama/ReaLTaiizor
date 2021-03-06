﻿#region Imports

using System;
using System.Drawing;
using ReaLTaiizor.Util;
using ReaLTaiizor.Manager;
using System.Windows.Forms;
using System.ComponentModel;
using ReaLTaiizor.Enum.Poison;
using ReaLTaiizor.Design.Poison;
using ReaLTaiizor.Drawing.Poison;
using ReaLTaiizor.Extension.Poison;
using ReaLTaiizor.Interface.Poison;

#endregion

namespace ReaLTaiizor.Controls
{
    #region PoisonTile

    [Designer(typeof(PoisonTileDesigner))]
    [ToolboxBitmap(typeof(System.Windows.Forms.Button))]
    public class PoisonTile : System.Windows.Forms.Button, IContainerControl, IPoisonControl
    {
        #region Interface

        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        public event EventHandler<PoisonPaintEventArgs> CustomPaintBackground;
        protected virtual void OnCustomPaintBackground(PoisonPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaintBackground != null)
            {
                CustomPaintBackground(this, e);
            }
        }

        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        public event EventHandler<PoisonPaintEventArgs> CustomPaint;
        protected virtual void OnCustomPaint(PoisonPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaint != null)
            {
                CustomPaint(this, e);
            }
        }

        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        public event EventHandler<PoisonPaintEventArgs> CustomPaintForeground;
        protected virtual void OnCustomPaintForeground(PoisonPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaintForeground != null)
            {
                CustomPaintForeground(this, e);
            }
        }

        private ColorStyle poisonStyle = ColorStyle.Default;
        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        [DefaultValue(ColorStyle.Default)]
        public ColorStyle Style
        {
            get
            {
                if (DesignMode || poisonStyle != ColorStyle.Default)
                {
                    return poisonStyle;
                }

                if (StyleManager != null && poisonStyle == ColorStyle.Default)
                {
                    return StyleManager.Style;
                }

                if (StyleManager == null && poisonStyle == ColorStyle.Default)
                {
                    return PoisonDefaults.Style;
                }

                return poisonStyle;
            }
            set => poisonStyle = value;
        }

        private ThemeStyle poisonTheme = ThemeStyle.Default;
        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        [DefaultValue(ThemeStyle.Default)]
        public ThemeStyle Theme
        {
            get
            {
                if (DesignMode || poisonTheme != ThemeStyle.Default)
                {
                    return poisonTheme;
                }

                if (StyleManager != null && poisonTheme == ThemeStyle.Default)
                {
                    return StyleManager.Theme;
                }

                if (StyleManager == null && poisonTheme == ThemeStyle.Default)
                {
                    return PoisonDefaults.Theme;
                }

                return poisonTheme;
            }
            set => poisonTheme = value;
        }

        private PoisonStyleManager poisonStyleManager = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PoisonStyleManager StyleManager
        {
            get => poisonStyleManager;
            set => poisonStyleManager = value;
        }

        private bool useCustomBackColor = false;
        [DefaultValue(false)]
        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        public bool UseCustomBackColor
        {
            get => useCustomBackColor;
            set => useCustomBackColor = value;
        }

        private bool useCustomForeColor = false;
        [DefaultValue(false)]
        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        public bool UseCustomForeColor
        {
            get => useCustomForeColor;
            set => useCustomForeColor = value;
        }

        private bool useStyleColors = false;
        [DefaultValue(false)]
        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        public bool UseStyleColors
        {
            get => useStyleColors;
            set => useStyleColors = value;
        }

        [Browsable(false)]
        [Category(PoisonDefaults.PropertyCategory.Behaviour)]
        [DefaultValue(false)]
        public bool UseSelectable
        {
            get => GetStyle(ControlStyles.Selectable);
            set => SetStyle(ControlStyles.Selectable, value);
        }

        private Control activeControl = null;
        [Browsable(false)]
        public Control ActiveControl
        {
            get => activeControl;
            set => activeControl = value;
        }

        public bool ActivateControl(Control ctrl)
        {
            if (Controls.Contains(ctrl))
            {
                ctrl.Select();
                activeControl = ctrl;
                return true;
            }

            return false;
        }

        #endregion

        #region Fields

        private bool paintTileCount = true;
        [DefaultValue(true)]
        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        public bool PaintTileCount
        {
            get => paintTileCount;
            set => paintTileCount = value;
        }

        private int tileCount = 0;
        [DefaultValue(0)]
        public int TileCount
        {
            get => tileCount;
            set => tileCount = value;
        }

        [DefaultValue(ContentAlignment.BottomLeft)]
        public new ContentAlignment TextAlign
        {
            get => base.TextAlign;
            set => base.TextAlign = value;
        }

        private Image tileImage = null;
        [DefaultValue(null)]
        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        public Image TileImage
        {
            get => tileImage;
            set => tileImage = value;
        }

        private bool useTileImage = false;
        [DefaultValue(false)]
        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        public bool UseTileImage
        {
            get => useTileImage;
            set => useTileImage = value;
        }

        private ContentAlignment tileImageAlign = ContentAlignment.TopLeft;
        [DefaultValue(ContentAlignment.TopLeft)]
        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        public ContentAlignment TileImageAlign
        {
            get => tileImageAlign;
            set => tileImageAlign = value;
        }

        private PoisonTileTextSize tileTextFontSize = PoisonTileTextSize.Medium;
        [DefaultValue(PoisonTileTextSize.Medium)]
        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        public PoisonTileTextSize TileTextFontSize
        {
            get => tileTextFontSize;
            set { tileTextFontSize = value; Refresh(); }
        }

        private PoisonTileTextWeight tileTextFontWeight = PoisonTileTextWeight.Light;
        [DefaultValue(PoisonTileTextWeight.Light)]
        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        public PoisonTileTextWeight TileTextFontWeight
        {
            get => tileTextFontWeight;
            set { tileTextFontWeight = value; Refresh(); }
        }

        private bool displayFocusBorder = true;
        [DefaultValue(true)]
        [Category(PoisonDefaults.PropertyCategory.Appearance)]
        public bool DisplayFocusBorder
        {
            get => displayFocusBorder;
            set => displayFocusBorder = value;
        }

        private bool isHovered = false;
        private bool isPressed = false;
        private bool isFocused = false;

        #endregion

        #region Constructor

        public PoisonTile()
        {
            SetStyle
            (
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint,
                     true
            );

            TextAlign = ContentAlignment.BottomLeft;
        }

        #endregion

        #region Paint Methods

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                Color backColor = BackColor;

                if (!useCustomBackColor)
                {
                    backColor = PoisonPaint.GetStyleColor(Style);
                }

                if (backColor.A == 255)
                {
                    e.Graphics.Clear(backColor);
                    return;
                }

                base.OnPaintBackground(e);

                OnCustomPaintBackground(new PoisonPaintEventArgs(backColor, Color.Empty, e.Graphics));
            }
            catch
            {
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                if (GetStyle(ControlStyles.AllPaintingInWmPaint))
                {
                    OnPaintBackground(e);
                }

                OnCustomPaint(new PoisonPaintEventArgs(Color.Empty, Color.Empty, e.Graphics));
                OnPaintForeground(e);
            }
            catch
            {
                Invalidate();
            }
        }

        protected virtual void OnPaintForeground(PaintEventArgs e)
        {
            Color foreColor, borderColor;

            borderColor = PoisonPaint.BorderColor.Button.Normal(Theme);

            if (isHovered && !isPressed && Enabled)
            {
                foreColor = PoisonPaint.ForeColor.Tile.Hover(Theme);
            }
            else if (isHovered && isPressed && Enabled)
            {
                foreColor = PoisonPaint.ForeColor.Tile.Press(Theme);
            }
            else if (!Enabled)
            {
                foreColor = PoisonPaint.ForeColor.Tile.Disabled(Theme);
            }
            else
            {
                foreColor = PoisonPaint.ForeColor.Tile.Normal(Theme);
            }

            if (useCustomForeColor)
            {
                foreColor = ForeColor;
            }

            if (isPressed || ((isHovered || isFocused) && displayFocusBorder))
            {
                using (Pen p = new Pen(borderColor))
                {
                    p.Width = 3;
                    Rectangle borderRect = new Rectangle(1, 1, Width - 3, Height - 3);
                    e.Graphics.DrawRectangle(p, borderRect);
                }
            }

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            if (useTileImage)
            {
                if (tileImage != null)
                {
                    Rectangle imageRectangle;
                    switch (tileImageAlign)
                    {
                        case ContentAlignment.BottomLeft:
                            imageRectangle = new Rectangle(new Point(0, Height - TileImage.Height), new Size(TileImage.Width, TileImage.Height));
                            break;
                        case ContentAlignment.BottomCenter:
                            imageRectangle = new Rectangle(new Point(Width / 2 - TileImage.Width / 2, Height - TileImage.Height), new Size(TileImage.Width, TileImage.Height));
                            break;
                        case ContentAlignment.BottomRight:
                            imageRectangle = new Rectangle(new Point(Width - TileImage.Width, Height - TileImage.Height), new Size(TileImage.Width, TileImage.Height));
                            break;
                        case ContentAlignment.MiddleLeft:
                            imageRectangle = new Rectangle(new Point(0, Height / 2 - TileImage.Height / 2), new Size(TileImage.Width, TileImage.Height));
                            break;
                        case ContentAlignment.MiddleCenter:
                            imageRectangle = new Rectangle(new Point(Width / 2 - TileImage.Width / 2, Height / 2 - TileImage.Height / 2), new Size(TileImage.Width, TileImage.Height));
                            break;
                        case ContentAlignment.MiddleRight:
                            imageRectangle = new Rectangle(new Point(Width - TileImage.Width, Height / 2 - TileImage.Height / 2), new Size(TileImage.Width, TileImage.Height));
                            break;
                        case ContentAlignment.TopLeft:
                            imageRectangle = new Rectangle(new Point(0, 0), new Size(TileImage.Width, TileImage.Height));
                            break;
                        case ContentAlignment.TopCenter:
                            imageRectangle = new Rectangle(new Point(Width / 2 - TileImage.Width / 2, 0), new Size(TileImage.Width, TileImage.Height));
                            break;
                        case ContentAlignment.TopRight:
                            imageRectangle = new Rectangle(new Point(Width - TileImage.Width, 0), new Size(TileImage.Width, TileImage.Height));
                            break;
                        default:
                            imageRectangle = new Rectangle(new Point(0, 0), new Size(TileImage.Width, TileImage.Height));
                            break;
                    }

                    e.Graphics.DrawImage(TileImage, imageRectangle);
                }
            }

            if (TileCount > 0 && paintTileCount)
            {
                Size countSize = TextRenderer.MeasureText(TileCount.ToString(), PoisonFonts.TileCount);

                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                TextRenderer.DrawText(e.Graphics, TileCount.ToString(), PoisonFonts.TileCount, new Point(Width - countSize.Width, 0), foreColor);
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            }

            Size textSize = TextRenderer.MeasureText(Text, PoisonFonts.Tile(tileTextFontSize, tileTextFontWeight));

            TextFormatFlags flags = PoisonPaint.GetTextFormatFlags(TextAlign) | TextFormatFlags.LeftAndRightPadding | TextFormatFlags.EndEllipsis;
            Rectangle textRectangle = ClientRectangle;

            if (isPressed)
            {
                textRectangle.Inflate(-4, -12);
            }
            else
            {
                textRectangle.Inflate(-2, -10);
            }

            TextRenderer.DrawText(e.Graphics, Text, PoisonFonts.Tile(tileTextFontSize, tileTextFontWeight), textRectangle, foreColor, flags);

            if (false && isFocused)
            {
                ControlPaint.DrawFocusRectangle(e.Graphics, ClientRectangle);
            }
        }

        #endregion

        #region Focus Methods

        protected override void OnGotFocus(EventArgs e)
        {
            isFocused = true;
            isHovered = true;
            Invalidate();

            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            isFocused = false;
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnLostFocus(e);
        }

        protected override void OnEnter(EventArgs e)
        {
            isFocused = true;
            isHovered = true;
            Invalidate();

            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            isFocused = false;
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnLeave(e);
        }

        #endregion

        #region Keyboard Methods

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                isHovered = true;
                isPressed = true;
                Invalidate();
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            //Remove this code cause this prevents the focus color
            //isHovered = false;
            //isPressed = false;
            Invalidate();

            base.OnKeyUp(e);
        }

        #endregion

        #region Mouse Methods

        protected override void OnMouseEnter(EventArgs e)
        {
            isHovered = true;
            Invalidate();

            base.OnMouseEnter(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isPressed = true;
                Invalidate();
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            isPressed = false;
            Invalidate();

            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (!isFocused)
            {
                isHovered = false;
            }

            Invalidate();

            base.OnMouseLeave(e);
        }

        #endregion

        #region Overridden Methods

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        #endregion
    }

    #endregion
}