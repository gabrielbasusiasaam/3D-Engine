using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Engine
{

    internal class Cuboid : Polygon3D
    {
        double dx, dy, dz;
        public Cuboid(Display display, Matrix position, double[] orientation, double dx, double dy, double dz) : base(display)
        {
            double x = position[0, 0];
            double y = position[1, 0];
            double z = position[2, 0];
            Position = position;
            Rotation = orientation;
            this.dx = dx;
            this.dy = dy;
            this.dz = dz;
            Vertices = new List<Matrix>
            {
                new Matrix(new[]{4, 1}, new[] { x, y, z, 1}),
                new Matrix(new[]{4, 1}, new[] { x, y + dy, z, 1}),
                new Matrix(new[]{4, 1}, new[] { x, y + dy, z + dz, 1}),
                new Matrix(new[]{4, 1}, new[] { x, y, z + dz, 1}),
                new Matrix(new[]{4, 1}, new[] { x + dx, y, z + dz, 1}),
                new Matrix(new[]{4, 1}, new[] { x + dx, y, z, 1}),
                new Matrix(new[]{4, 1}, new[] { x + dx, y + dy, z, 1}),
                new Matrix(new[]{4, 1}, new[] { x + dx, y + dy, z + dz, 1})
            };

            Edges = new List<int[]>
            {
                new[] { 0, 1 }, new[] { 0, 3 }, new[] { 0, 5 },
                new[] { 1, 2 }, new[] { 1, 6 },
                new[] { 2, 3 }, new[] { 2, 7 },
                new[] { 3, 4 },
                new[] { 4, 5 }, new[] { 4, 7 },
                new[] { 5, 6 },
                new[] { 6, 7 }
            };

            Surfaces = new List<int[]>
            {
                new[] { 0, 1, 6, 5 },
                new[] { 0, 1, 2, 3 },
                new[] { 0, 3, 4, 5 },
                new[] { 1, 2, 7, 6 },
                new[] { 2, 3, 4, 7 },
                new[] { 5, 6, 7, 4 }
            };

        }

        public override void Draw()
        {
            foreach(var surface in Surfaces)
            {
                Display.DrawSurface(surface.Select(x => Vertices[x]).ToList(), Color.Red);
            }
        }
        public override void Draw(List<Color> surfaceColors)
        {
            if (surfaceColors.Count != Surfaces.Count) throw new ArgumentException("The number of colors provided does not equal the number of surfaces");
            Surfaces = Surfaces.OrderByDescending(surface => surface.Sum(x => Distance(Display.camera.position, Vertices[x]))).ToList();
            for (int i = 0; i < Surfaces.Count; i++)
            {
                Display.DrawSurface(Surfaces[i].Select(x => Vertices[x]).ToList(), surfaceColors[i]);
            }
        }
        public override void Rotate(double[] rotation)
        {
            double[] newRotation = new[] { Rotation[0] + rotation[0], Rotation[1] + rotation[1], Rotation[2] + rotation[2] };
            SetRotation(newRotation);
        }

        public override void SetPosition(Matrix newPosition)
        {
            Position = newPosition;
            SetRotation(Rotation);
        }

        public override void SetRotation(double[] newRotation)
        {
            double x = Position[0, 0];
            double y = Position[1, 0];
            double z = Position[2, 0];

            Matrix centre = new Matrix(new[] { 4, 1 }, new[] {x + (dx / 2), y + (dy / 2), z + (dz / 2), 0});

            double rotX = newRotation[0];
            double cosX = Math.Cos(rotX);
            double sinX = Math.Sin(rotX);

            double rotY = newRotation[1];
            double cosY = Math.Cos(rotY);
            double sinY = Math.Sin(rotY);

            double rotZ = newRotation[2];
            double cosZ = Math.Cos(rotZ);
            double sinZ = Math.Sin(rotZ);

            Matrix rotationMatrixZ = new Matrix(new[] { 4, 4 },
                                                new[] {  cosZ, sinZ, 0, 0,
                                                        -sinZ, cosZ, 0, 0,
                                                            0,    0, 1, 0,
                                                            0,    0, 0, 1 });
            Matrix rotationMatrixY = new Matrix(new[] { 4, 4 },
                                                new[] { cosY, 0, -sinY, 0,
                                                        0,    1,     0, 0,
                                                        sinY, 0,  cosY, 0,
                                                        0,    0,     0, 1 });
            Matrix rotationMatrixX = new Matrix(new[] { 4, 4 },
                                                new[] { 1,    0,     0, 0,
                                                        0,  cosX, sinX, 0,
                                                        0, -sinX, cosX, 0,
                                                        0,     0,    0, 1 });

            Vertices = new List<Matrix>
            {
                new Matrix(new[]{4, 1}, new[] { x, y, z, 1}),
                new Matrix(new[]{4, 1}, new[] { x, y + dy, z, 1}),
                new Matrix(new[]{4, 1}, new[] { x, y + dy, z + dz, 1}),
                new Matrix(new[]{4, 1}, new[] { x, y, z + dz, 1}),
                new Matrix(new[]{4, 1}, new[] { x + dx, y, z + dz, 1}),
                new Matrix(new[]{4, 1}, new[] { x + dx, y, z, 1}),
                new Matrix(new[]{4, 1}, new[] { x + dx, y + dy, z, 1}),
                new Matrix(new[]{4, 1}, new[] { x + dx, y + dy, z + dz, 1})
            }.Select(x => rotationMatrixX * (rotationMatrixY * (rotationMatrixZ * (x - centre))) + centre).ToList();
            Rotation = newRotation;
        }
        private static double Distance(Matrix point1, Matrix point2)
        {
            double dx = point1[0, 0] - point2[0, 0];
            double dy = point1[1, 0] - point2[1, 0];
            double dz = point1[2, 0] - point2[2, 0];
            return Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2) + Math.Pow(dx, 2));
        }

    }
}
