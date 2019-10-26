using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Tetris
{
    public partial class View : UserControl
    {
        Model model;
        public View(Model model)
        {
            InitializeComponent();

            this.model = model;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Monitor.Enter(model.PlayGroundImage);
            e.Graphics.DrawImage(model.PlayGroundImage, 2, 2);

            this.Invalidate();
            Monitor.Exit(model.PlayGroundImage);

            ControlPaint.DrawBorder3D(e.Graphics, e.ClipRectangle, Border3DStyle.Sunken);
        }
    }
}
