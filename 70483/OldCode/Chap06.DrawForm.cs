using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
namespace Chap6
{
    public partial class DrawForm : Form
    {
        Graphics g;
        Button b;
        public DrawForm()
        {
            InitializeComponent();
        }

        public void Draw1()
        {
            if (b == null)
            {
                b = new Button();
                b.Parent = this;
                b.Location = new Point(15, 15);
            }
            else
            {
                b.Location = new Point(b.Top + 15, b.Left + 15);
                b.Text = "Testing Button";
                b.ForeColor = Color.OrangeRed;
                b.BackColor = Color.Silver;
            }
            
        }

        public void SetDblBuff()
        {
            this.DoubleBuffered = !this.DoubleBuffered;
        }

        public void Draw2()
        {
            
            if (g == null)
            {
                g = this.CreateGraphics();
            }
            g.Clear(Color.Olive);
            g.DrawEllipse(Pens.Tan, new Rectangle(0, 0, this.Width-10, this.Height / 5));
            g.DrawIcon(SystemIcons.Exclamation, new Rectangle(this.Width / 2, this.Height / 2, this.Width / 4, this.Height / 4));
            g.DrawIconUnstretched(SystemIcons.WinLogo, new Rectangle(0, this.Height / 2, this.Width / 4, this.Height / 4));
            Brush b = new System.Drawing.Drawing2D.HatchBrush(HatchStyle.DiagonalBrick , Color.Maroon);
            Pen p = new Pen(b, 3);
            p.StartCap = LineCap.Triangle;
            p.EndCap = LineCap.RoundAnchor;
            g.DrawLine(p, new Point(5, this.Height / 2), new Point(this.Width-20, this.Height / 2));
            g.DrawArc(p, new RectangleF(new Point(this.Width / 2, this.Height / 2), new Size(100, 100)),(float)13.3, (float)180);
            

        }
        //Filled shapes!
        public void Draw3()
        {
            g = this.CreateGraphics();
            g.Clear(SystemColors.Window);
            Point[] po = new Point[] 
            {   new Point(10,10),
                new Point(10,100),
                new Point(50,65),
                new Point(100,100),
                new Point(85,40)};
            Brush b = new System.Drawing.Drawing2D.HatchBrush(HatchStyle.Cross , Color.DarkGreen );
            Pen p = new Pen(b, 3);
            g.DrawPolygon(p, po);
            //Point[] p1 = new Point[] { po[0], po[3]};
            //byte[] b1 = new byte[]{(byte)PathPointType.Start , (byte)PathPointType.CloseSubpath };
            //GraphicsPath gp = new GraphicsPath(p1,b1,FillMode.Winding);
            //Brush fill = new System.Drawing.Drawing2D.PathGradientBrush(gp);
            Brush solid = new SolidBrush(Color.IndianRed);
            g.FillPolygon(solid,po);

            Pen p1 = new Pen(Color.Maroon, 3);
            Brush b1 = new LinearGradientBrush(new Point(151, 151), new Point(250, 250), Color.GhostWhite, Color.Red);
            for (int x = 0;x<po.Length;x++)
            {
                po[x].X += 150;
                po[x].Y += 150;
            }
            g.DrawPolygon(p1, po);
            g.FillPolygon(b1, po);
        }
        //strings
        public void Draw4()
        {
            g = this.CreateGraphics();
            g.Clear(SystemColors.Window);
            //Brush b = new System.Drawing.Drawing2D.
            Brush b = new System.Drawing.Drawing2D.LinearGradientBrush(new Point (0,0), new Point(40,(int)(this.Width *.8)),Color.GhostWhite, Color.Navy) ;
            g.DrawString("Hello Nurse!", new Font("Arial", 36, FontStyle.Bold), b, new PointF(0, (float)(this.Height * .08)));
        }

        public void Draw5()
        {
            g = this.CreateGraphics();
            g.SetClip(g);
            g.TranslateClip(50, 50);
        }
    }
}