using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace customButton
{
    public class Design : Button
    {
        private int borderSize = 0;
        private int borderRadius = 40;
        private Color borderColor = Color.FromArgb(32, 32, 32);

        //Properties
        public int BorderSize
        {
            get { return borderSize; }
            set { borderSize = value; Invalidate(); }
        }

        public int BorderRadius
        {
            get { return borderRadius; }
            set { borderRadius = (value <= Height) ? value:Height; Invalidate(); }
        }

        public Color TextClor
        {
            get { return this.ForeColor; }
            set { this.ForeColor = value; }
        }

        //Constructor
        public Design()
        {
            Size = new Size(100, 40);
            FlatAppearance.BorderSize = 0;
            FlatStyle = FlatStyle.Flat;
            BackColor = Color.Black;
            ForeColor = Color.White;

            Resize += new EventHandler(Button_Resize);

        }

        private void Button_Resize(object sender, EventArgs e)
        {
            if (borderRadius > Height)
            {
                borderRadius = Height;
            }
        }

        //methods
        private GraphicsPath gp(RectangleF rect, float radius)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.StartFigure();
            gp.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            gp.AddArc(rect.Width - radius, rect.Y, radius, radius, 270, 90);
            gp.AddArc(rect.Width - radius, rect.Height - radius, radius, radius, 0, 90);
            gp.AddArc(rect.X, rect.Height - radius, radius, radius, 90, 90);
            gp.CloseFigure();
            return gp;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF rectSurface = new RectangleF(0, 0, Width, Height);
            RectangleF rectBorder = new RectangleF(1, 1, Width - 0.9F, Height - 1);

            if (borderRadius > 1)
            {
                using (GraphicsPath gpSurface = gp(rectSurface, BorderRadius))
                using (GraphicsPath gpBorder = gp(rectBorder, BorderRadius - 1))
                using (Pen penSurface = new Pen(this.Parent.BackColor, 2))
                using (Pen penBorder = new Pen(borderColor, BorderSize))
                {
                    penBorder.Alignment = PenAlignment.Center;
                    Region = new Region(gpSurface);
                    pevent.Graphics.DrawPath(penBorder, gpSurface);

                    if (borderSize >= 1)
                    {
                        pevent.Graphics.DrawPath(penBorder, gpBorder);
                    }
                }
            }
            else
            {
                Region = new Region(rectSurface);
                if (borderSize >= 1)
                    using (Pen penBorder = new Pen(borderColor, BorderSize))
                    {
                        penBorder.Alignment = PenAlignment.Inset;
                        pevent.Graphics.DrawRectangle(penBorder, 0, 0, Width - 1, Height - 1);
                    }
            }
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Parent.BackColorChanged += new EventHandler(Container_BackColorChanged);
        }

        private void Container_BackColorChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                Invalidate();
        }
    }
}
