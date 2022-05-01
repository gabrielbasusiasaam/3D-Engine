using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Engine
{
    internal class Camera
    {
        Matrix displaySurface = new Matrix(new[] { 4, 1 }, new[] { 0.5, 0.5, 0.5, 1 });
        Matrix position;
        Matrix orientation;
        Matrix windowDimensions;
        double near;
        double far;
        double fov;


        /// <summary>Initializes a new instance of the <see cref="Camera" /> class.</summary>
        /// <param name="windowDimensions">The dimensions of the window the point is projected on .</param>
        /// <param name="position">The position of the Camera.</param>
        /// <param name="orientation">The orientation of the Camera is Tait-Bryan angles.</param>
        /// <param name="near">The near distance of the viewing frustrum. Typically this is set to 0.1;</param>
        /// <param name="far">The far distance of the viewing frustrum.
        /// Typically this is set to 100;</param>
        /// <param name="fov">The fov of the camera.</param>
        public Camera(Matrix windowDimensions, Matrix position, Matrix orientation, double near, double far, double fov)
        {
            this.position = position;
            this.orientation = orientation;
            this.windowDimensions = windowDimensions;
            this.near = near;
            this.far = far;
            this.fov = fov;
        }



        /// <summary>Projects the specified point onto the camera's viewing surface.</summary>
        /// <param name="point">The point to be projected.</param>
        /// <returns>The homegeneous position of the point.</returns>
        public Matrix Convert3d(Matrix point)
        {
            double rotX = orientation[0, 0];
            double cosX = Math.Cos(rotX);
            double sinX = Math.Sin(rotX);

            double rotY = orientation[1, 0];
            double cosY = Math.Cos(rotY);
            double sinY = Math.Sin(rotY);

            double rotZ = orientation[2, 0]; 
            double cosZ = Math.Cos(rotZ);
            double sinZ = Math.Sin(rotZ);

            double aspect = windowDimensions[0, 0] / windowDimensions[1, 0];
            double p00 = 1d / (aspect * Math.Tan(fov / 2));
            double p11 = 1d / Math.Tan(fov / 2);
            double p22 = -(far + near) / (far - near);
            double p23 = -(2 * far * near) / (far - near);

            double eX = displaySurface[0,0];
            double eY = displaySurface[1,0];
            double eZ = displaySurface[2,0];

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

            Matrix projectionMatrix = new Matrix(new[] { 4, 4 },
                                                 new[] { p00,   0,   0,   0,
                                                           0, p11,   0,   0,
                                                           0,   0, p22, p23,
                                                           0,   0,  -1,   0 });
            Matrix matrixD = projectionMatrix * (rotationMatrixX * (rotationMatrixY * (rotationMatrixZ * (point - position))));
            Matrix matrixE = new Matrix(new[] { 4, 4 },
                                        new[] { 1, 0, eX / eZ, 0,
                                                0, 1, eY / eZ, 0,
                                                0, 0,  1 / eZ, 0,
                                                0, 0,       0, 1 });
            Matrix matrixF = matrixE * matrixD;
            Matrix returnMatrix = new Matrix(new[] { 3, 1 },
                                             new[] { matrixF[0, 0] / matrixF[2, 0],
                                                     matrixF[1, 0] / matrixF[2, 0],
                                                     matrixD[4, 0] });
            return returnMatrix;
        }
    }
}
