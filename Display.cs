using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Engine
{
    internal class Display
    {
        public Graphics displayGraphics;
        public List<Polygon3D> graphicsObjects;
        public Camera camera;
        Color clearColor;
        double near = 0.1;
        double far = 100;
        double fov = 45 * (Math.PI / 180);

        public Display(Graphics graphics, Matrix windowDimensions, Color clearColor)
        {
            this.clearColor = clearColor;
            displayGraphics = graphics;
            graphicsObjects = new List<Polygon3D>();
            Matrix position = new Matrix(new[] { 4, 1 }, new[] { 0d, 0d, 0d, 0d });
            Matrix orientation = new Matrix(new[] { 3, 1 }, new[] { 0d, 0d, 0d });
            camera = new Camera(windowDimensions, position, orientation, near, far, fov);
        }

        public void Update()
        {
            displayGraphics.Clear(clearColor);
            if (graphicsObjects.Count > 0) graphicsObjects.ForEach(x => x.Draw());
        }

        public void DrawLine(Matrix point1, Matrix point2, Color color)
        {
            Pen pen = new Pen(color);
            Matrix pointProjected1 = camera.Convert3d(point1);
            Matrix pointProjected2 = camera.Convert3d(point2);

            if (pointProjected1[2, 0] < near && pointProjected2[2, 0] < near) return;

            if (pointProjected1[2, 0] >= near && pointProjected2[2, 0] >= near)
            {
                Point newPoint1 = new Point((int)pointProjected1[0, 0], (int)pointProjected1[1, 0]);
                Point newPoint2 = new Point((int)pointProjected2[0, 0], (int)pointProjected2[1, 0]);
                displayGraphics.DrawLine(pen, newPoint1, newPoint2);
                return;
            };

            Matrix stationaryPoint2D = pointProjected1;
            Matrix stationaryPoint3D = point1;
            Matrix clippingPoint2D = pointProjected2;
            Matrix clippingPoint3D = point2;
            if (clippingPoint2D[2, 0] > stationaryPoint2D[2, 0])
            {
                (clippingPoint3D, stationaryPoint3D) = (stationaryPoint3D, clippingPoint3D);
                (clippingPoint2D, stationaryPoint2D) = (stationaryPoint2D, clippingPoint2D);
            }
            Matrix newPoint3D = Clip(clippingPoint2D, clippingPoint3D, stationaryPoint2D, stationaryPoint3D);
            Matrix newPoint2D = camera.Convert3d(newPoint3D);

            Point displayPoint1 = new Point((int)stationaryPoint2D[0, 0], (int)stationaryPoint2D[1, 0]);
            Point displayPoint2 = new Point((int)newPoint2D[0, 0], (int)newPoint2D[1, 0]);

            displayGraphics.DrawLine(pen, displayPoint1, displayPoint2);
            return;
        }

        public void DrawSurface(List<Matrix> points, Color color)
        {
            SolidBrush brush = new SolidBrush(color);
            List<Matrix> points2D = points.Select(x => camera.Convert3d(x)).ToList();
            if (points2D.All(x => x[2, 0] < near)) return;

            List<Point> clippedPoints2D = new List<Point>();
            for (int i = 0; i < points2D.Count; i++)
            {
                Matrix point1 = points2D[i];
                Matrix point2 = points2D[(i + 1) % points2D.Count];
                if (point2[2, 0] >= near)
                {
                    Point point = new Point((int)point2[0, 0], (int)point2[1, 0]);
                    clippedPoints2D.Add(point);
                    continue;
                }
                Matrix newPoint = Clip(point1, points[i], point2, points[(i + 1) % points.Count]);
                Matrix newPoint2D = camera.Convert3d(newPoint);
                Point displayPoint = new Point((int)newPoint2D[0, 0], (int)newPoint2D[1, 0]);
                clippedPoints2D.Add(displayPoint);
            }

            displayGraphics.FillPolygon(brush, clippedPoints2D.ToArray());
        }
        private Matrix Clip(Matrix clippingPoint2D, Matrix clippingPoint3D, Matrix stationaryPoint2D, Matrix stationaryPoint3D)
        {
            double w1 = stationaryPoint2D[2, 0];
            double w2 = clippingPoint2D[2, 0];
            double n = (w1 - near) / (w1 - w2);
            double xc = (n * stationaryPoint3D[0, 0]) + ((1 - n) * clippingPoint3D[0, 0]);
            double yc = (n * stationaryPoint3D[1, 0]) + ((1 - n) * clippingPoint3D[1, 0]);
            double zc = (n * stationaryPoint3D[2, 0]) + ((1 - n) * clippingPoint3D[2, 0]);
            double wc = near;
            
            Matrix newPoint3D = new Matrix(new[] { 4, 1 },
                                             new[] { xc, yc, zc, wc });

            return newPoint3D;
        }
    }

}
