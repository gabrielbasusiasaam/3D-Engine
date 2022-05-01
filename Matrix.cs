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
            if (data == null)
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

            int[] ResultShape = { this.shape[0], matrix.shape[1] };
            Matrix resultMatrix = new Matrix(ResultShape);

            for (int row = 0; row < this.shape[0]; row++)
            {
                for (int col = 0; col < matrix.shape[1]; col++)
                {
                    resultMatrix[row, col] = DotProduct(GetRow(row), matrix.GetColumn(col));
                }
            }
            return resultMatrix;
        }

        public Matrix Multiply(float scalar)
        {

            Matrix resultMatrix = new Matrix(shape);

            for (int row = 0; row < shape[0]; row++)
            {
                for (int col = 0; col < shape[1]; col++)
                {
                    resultMatrix[row, col] = scalar * this[row, col];
                }
            }

            return resultMatrix;
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

            Matrix resultMatrix = new Matrix(this.shape);

            for (int row = 0; row < this.shape[0]; row++)
            {
                for (int col = 0; col < this.shape[0]; col++)
                {
                    resultMatrix[row, col] = this[row, col] + matrix[row, col];
                }
            }
            return resultMatrix;
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

            Matrix resultMatrix = new Matrix(this.shape);

            for (int row = 0; row < this.shape[0]; row++)
            {
                for (int col = 0; col < this.shape[0]; col++)
                {
                    resultMatrix[row, col] = this[row, col] - matrix[row, col];
                }
            }
            return resultMatrix;
        }

        public Matrix Transpose()
        {
            int[] newShape = { shape[1], shape[0] };
            Matrix resultMatrix = new Matrix(newShape);
            for (int row = 0; row < this.shape[0]; row++)
            {
                for (int col = 0; col < this.shape[1]; col++)
                {
                    resultMatrix[col, row] = this[row, col];
                }
            }
            return resultMatrix;
        }

        public Matrix Inverse()
        {
            float determinant = Determinant();
            if (determinant == 0 || shape[0] != shape[1])
            {
                throw new ArithmeticException("This matrix cannot be inversed");
            }
            int sign = 1;
            Matrix minorsMatrix = new Matrix(shape);
            for (int row = 0; row < shape[0]; row++)
            {
                for (int col = 0; col < shape[1]; col++)
                {
                    int position = 0;
                    int[] newShape = { shape[0] - 1, shape[1] - 1 };
                    Matrix determinantMatrix = new Matrix(newShape);
                    for (int minorRow = 0; minorRow < shape[0]; minorRow++)
                    {
                        for (int minorCol = 0; minorCol < shape[1]; minorCol++)
                        {
                            if (minorRow != row && minorCol != col)
                            {
                                determinantMatrix[position / newShape[1], position % newShape[0]] = this[minorRow, minorCol];
                                position++;
                            }
                        }
                    }
                    minorsMatrix[row, col] = Determinant(determinantMatrix) * sign;
                    sign *= -1;
                }
            }
            Matrix adjugate = minorsMatrix.Transpose();

            return adjugate.Multiply(1f / determinant);
        }

        public float Determinant() => Determinant(this);

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

        public override string ToString()
        {
            string formattedMatrix = "[";
            int longest = 0;
            for (int row = 0; row < shape[0]; row++)
            {
                for (int col = 0; col < shape[1]; col++)
                {
                    longest = Math.Max(longest, this[row, col].ToString().Length);
                }
            }

            for (int row = 0; row < shape[0]; row++)
            {
                for (int col = 0; col < shape[1]; col++)
                {
                    int leadingSpaces = 2;
                    int trailingSpaces = longest - this[row, col].ToString().Length;
                    if (row == 0 && col == 0)
                    {
                        leadingSpaces = 1;
                    }
                    if (this[row, col] < 0)
                    {
                        leadingSpaces -= 1;
                        trailingSpaces += 1;
                    }
                    formattedMatrix += new String(' ', leadingSpaces) + String.Format("{0},", this[row, col]) + new String(' ', trailingSpaces);
                }
                if (row != shape[0] - 1) 
                {
                    formattedMatrix = formattedMatrix.TrimEnd() + "\n";
                } else
                {
                    formattedMatrix = formattedMatrix.TrimEnd();
                    formattedMatrix = formattedMatrix.Remove(formattedMatrix.Length - 1) + " ]";
                }
            }
            return formattedMatrix;
        }

        public static implicit operator string(Matrix a) => a.ToString();

        public static Matrix operator +(Matrix a, Matrix b) => a.Add(b);

        public static Matrix operator -(Matrix a, Matrix b) => a.Subtract(b);
        
        public static Matrix operator *(Matrix a, Matrix b) => a.Multiply(b);

        public static Matrix operator *(float a, Matrix b) => b.Multiply(a);

        public static Matrix operator *(Matrix a, float b) => a.Multiply(b);

    }
}
