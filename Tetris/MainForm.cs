using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace Tetris
{
    public partial class MainForm : Form
    {
        Model model;
        View view;
        Thread game;
        public MainForm()
        {
            InitializeComponent();
            //comboBox1.DataSource = Enum.GetValues(typeof(TetroColor));

            model = new Model();
            view = new View(model);
            view.PreviewKeyDown += new PreviewKeyDownEventHandler(view_PreviewKeyDown);
            view.KeyUp += new KeyEventHandler(view_KeyUp);
            view.Location = new Point(13, 13);
            this.Controls.Add(view);

            game = new Thread(model.Play);

            game.Start();

        }

        void view_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
                model.StopSpeedUp();
        }

        void view_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    model.RotateCurrentTetro();
                    break;

                case Keys.Right:
                    model.MoveCurrentTetroRight();
                    break;

                case Keys.Left:
                    model.MoveCurrentTetroLeft();
                    break;

                case Keys.Down:
                    model.SpeedUp();
                    break;

                case Keys.Space:
                    model.Finish();
                    break;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (game != null && game.IsAlive)
                game.Abort();
        }
    }
}

/*
        private void btn_update_Click(object sender, EventArgs e)
        {
            //using (Graphics gfx = Graphics.FromImage(gameFieldBmp))
            //{
            //    for (int x = 0; x < width; x++)
            //    {
            //        for (int y = 0; y < height; y++)
            //        {
            //            if (gameField[y, x])
            //            {
            //                gfx.DrawImage(Tetris.Properties.Resources.tiles,
            //                  x * 20, y * 20,
            //                  new Rectangle(20, 0, 20, 20),
            //                  GraphicsUnit.Pixel);
            //            }
            //            else
            //            {
            //                gfx.DrawImage(Tetris.Properties.Resources.tiles,
            //                x * 20, y * 20,
            //                new Rectangle(140, 0, 20, 20),
            //                GraphicsUnit.Pixel);
            //            }
            //        }
            //    }

            //}

            //this.view1.Invalidate();
        }

        private void btn_rotate_Click(object sender, EventArgs e)
        {
            //t.Rotate();
            //btn_update.PerformClick();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //t.Color = (TetroColor)Enum.Parse(typeof(TetroColor),comboBox1.SelectedValue.ToString());
        }
        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                    model.RotateCurrentTetro();
                    break;

                case Keys.Right:
                    model.MoveCurrentTetroRight();
                    break;

                case Keys.Left:
                    model.MoveCurrentTetroTetro();
                    break;

                case Keys.Down:
                    model.SpeedUp();
                    break;

                case Keys.Space:
                    model.Finish();
                    break;
                    return true;
            }
            return base.IsInputKey(keyData);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.Up:
                    model.RotateCurrentTetro();
                    break;

                case Keys.Right:
                    model.MoveCurrentTetroRight();
                    break;

                case Keys.Left:
                    model.MoveCurrentTetroTetro();
                    break;

                case Keys.Down:
                    model.SpeedUp();
                    break;

                case Keys.Space:
                    model.Finish();
                    break;
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void btn_fall_Click(object sender, EventArgs e)
        {
            //t.Fall();
            //btn_update.PerformClick();
        }
*/