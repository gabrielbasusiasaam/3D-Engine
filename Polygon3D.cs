using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Engine
{
    abstract class Polygon3D
    {
        private Matrix position;
        private double[] rotation;
        private List<int[]> surfaces;
        private List<int[]> edges;
        private List<Matrix> vertices;
        private Display display;
        public Matrix Position { get => position; set => position = value; }
        public double[] Rotation { get => rotation; set => rotation = value; }
        public List<int[]> Surfaces { get => surfaces; set => surfaces = value; }
        public List<int[]> Edges { get => edges; set => edges = value; }
        public List<Matrix> Vertices { get => vertices; set => vertices = value; }
        internal Display Display { get { return display; } set { display = value; } }

        public Polygon3D(Display display)
        {
            this.display = display;
            position = new Matrix(new[] { 3, 1 });
            rotation = new double[] { 0, 0, 0 };
            vertices = new List<Matrix> { };
            edges = new List<int[]> { };
            surfaces = new List<int[]> { };
        }

        abstract public void Draw();
        abstract public void Draw(List<Color> surfaceColors);
        abstract public void Rotate(double[] rotation);
        abstract public void SetPosition(Matrix newPosition);
        abstract public void SetRotation(double[] newRotation);

    }
}
