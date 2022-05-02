using System.ComponentModel;
using System.Diagnostics;

namespace _3D_Engine
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        Display display;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.DoubleBuffered = true;

            display = new Display(this.CreateGraphics(), new Matrix(new[] {2, 1}, new double[] {this.Width, this.Height}), Color.Black);
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 16;
            timer.Tick +=  (object sender, EventArgs e) => { this.Refresh(); };
            timer.Start();

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            display.camera.orientation[2, 0] += Math.PI / 180;
            display.displayGraphics = e.Graphics;
            display.Update();
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            display.camera.windowDimensions = new Matrix(new[] { 2, 1 }, new double[] { this.Width, this.Height });
        }

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /// 
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Desktop;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
    }
}