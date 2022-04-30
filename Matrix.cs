using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Engine
{
    public class Matrix
    {
        private float[,] data;
        public readonly int[] shape;
        public Matrix(int[] shape)
        {
            if (shape == null || shape[0] <= 0 || shape[1] <= 0)
            {
                throw new ArgumentException("The dimensions of the array must be integers above 0");
            }
            data = new float[shape[0], shape[1]];
            this.shape = shape;
            
        }

        public Matrix Multiply(Matrix Matrix2)
        {
            if (Matrix2 == null)
            {
                throw new ArgumentNullException("Matrix cannot be multiplied by null object");
            }
            else if (this.shape[1] != Matrix2.shape[0])
            {
                throw new ArgumentException("These matrices cannot be multiplied as the column count of the first matrix is not equal to the row count on the second matrix");
            }

            int[] ResultShape = {this.shape[0], Matrix2.shape[1]};
            Matrix ResultMatrix = new Matrix();
        }

        private float DotProduct(int[] row, int[] column)
        {
            int product = 0;
            for (int i = 0; i < row.Length; i++)
            {
                product += row[i] * column[i];
            }
            return product;
        }
    }
}
