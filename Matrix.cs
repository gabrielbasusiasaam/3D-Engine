using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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
            this.data = new float[shape[0], shape[1]];
            this.shape = shape;
            
        }

        public Matrix(int[] shape, float[] data)
        {
            if (shape == null || shape[0] <= 0 || shape[1] <= 0)
            {
                throw new ArgumentException("The dimensions of the array must be integers above 0");
            }
            if(data == null)
            {
                throw new ArgumentNullException("Data cannot be null.");
            }
            if (data.Length != shape[0] * shape[1])
            {
                throw new ArgumentException(String.Format("Data cannot be casted to shape ({0}, {1})", shape[0], shape[1]));
            }
            this.shape = shape;
            this.data = new float[shape[0], shape[1]];

            for (int i = 0; i < data.Length; i++)
            {
                int row = i / shape[1];
                int column = i % shape[1];
                this.data[row, column] = data[i];
                Debug.WriteLine(String.Format("({0}, {1})", row, column));
            }
            Debug.WriteLine(data.Length);


        }

        public Matrix Multiply(Matrix matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException("Matrix cannot be multiplied by null object.");
            }
            else if (this.shape[1] != matrix.shape[0])
            {
                throw new ArgumentException("These matrices cannot be multiplied as the column count of the first matrix is not equal to the row count on the second matrix.");
            }

            int[] ResultShape = {this.shape[0], matrix.shape[1]};
            Matrix ResultMatrix = new Matrix(ResultShape);
            Debug.WriteLine("{0}, {1}", ResultMatrix.shape[0], ResultMatrix.shape[1]);
            for (int row = 0; row < this.shape[0]; row++)
            {
                for (int col = 0; col < matrix.shape[1]; col++)
                {
                    ResultMatrix[row, col] = DotProduct(GetRow(row), matrix.GetColumn(col));
                }
            }
            return ResultMatrix;
        }

        public Matrix Add(Matrix matrix) 
        {
            if (matrix == null)
            {
                throw new ArgumentNullException("Matrix cannot be added to a null object.");
            }
            else if (this.shape != matrix.shape)
            {
                throw new ArgumentException("These matrices cannot be added as they do not have the same shape.");
            }

            Matrix ResultMatrix = new Matrix(this.shape);

            for (int row = 0; row < this.shape[0]; row++)
            {
                for (int col = 0; col < this.shape[0]; col++)
                {
                    ResultMatrix[row, col] = this[row, col] + matrix[row, col];
                }
            }
            return ResultMatrix;
        }

        public Matrix Subtract(Matrix matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException("Matrix cannot be subtracted by null object.");
            }
            else if (this.shape != matrix.shape)
            {
                throw new ArgumentException("These matrices cannot be subtracted as they do not have the same shape.");
            }

            Matrix ResultMatrix = new Matrix(this.shape);

            for (int row = 0; row < this.shape[0]; row++)
            {
                for (int col = 0; col < this.shape[0]; col++)
                {
                    ResultMatrix[row, col] = this[row, col] - matrix[row, col];
                }
            }
            return ResultMatrix;
        }

        public float Determinant() => Determinant(this);

        public float this[int row, int column]
        {
            get => data[row, column];
            set => data[row, column] = value;
        }

        private float[] GetRow(int row) => Enumerable.Range(0, shape[1])
                .Select(i => data[row, i])
                .ToArray();

        private float[] GetColumn(int column) => Enumerable.Range(0, shape[0])
                .Select(i => data[i, column])
                .ToArray();

        private static float DotProduct(float[] row, float[] column)
        {
            float product = 0;
            for (int i = 0; i < row.Length; i++)
            {
                product += row[i] * column[i];
            }
            return product;
        }

        private static float Determinant(Matrix matrix)
        {
            int sign = -1;
            float determinant = 0f;

            if (matrix.shape[0] == 1 && matrix.shape[1] == 1)
            {
                return matrix[0, 0];
            }
            if (matrix.shape[0] == 2 && matrix.shape[1] == 2)
            {
                return (matrix[0, 0] * matrix[1, 1]) - (matrix[0, 1] * matrix[1, 0]);
            }
            for (int i = 0; i < matrix.shape[1]; i++)
            {
                int position = 0;
                int[] new_shape = { matrix.shape[0] - 1, matrix.shape[1] - 1 }; 
                Matrix matrix1 = new Matrix(new_shape);
                for (int row = 1; row <= new_shape[0]; row++)
                {
                    for (int col = 0; col < new_shape[1]; col++)
                    {
                        if (col != i)
                        {
                            matrix1[position / matrix1.shape[1], position % matrix1.shape[0]] = matrix[row, col];
                            position++;
                        }
                    }
                }
                determinant += Determinant(matrix1) * sign;
                sign *= -1;
            }
            return determinant;
        }
    }
}
