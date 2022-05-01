using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace _3D_Engine
{
    /// <summary>
    /// Represents a Matrix.
    /// </summary>
    public class Matrix
    {

        private double[,] data;
        public readonly int[] shape;

        /// <summary>Initializes a new instance of the <see cref="Matrix" /> class.</summary>
        /// <param name="shape">The shape of the Matrix in the form (row, column).</param>
        /// <exception cref="System.ArgumentException">The dimensions of the array must be integers above 0</exception>
        public Matrix(int[] shape)
        {
            if (shape == null || shape[0] <= 0 || shape[1] <= 0)
            {
                throw new ArgumentException("The dimensions of the array must be integers above 0");
            }
            this.data = new double[shape[0], shape[1]];
            this.shape = shape;

        }


        /// <summary>Initializes a new instance of the <see cref="Matrix" /> class.</summary>
        /// <param name="shape">The shape of the Matrix in the form (row, column).</param>
        /// <param name="data">The data that the Matrix will hold.</param>
        /// <exception cref="System.ArgumentException">The dimensions of the array must be integers above 0
        /// or</exception>
        /// <exception cref="System.ArgumentException">The data provided cannot be casted into the shape provided.</exception>
        /// <exception cref="System.ArgumentNullException">Data cannot be null.</exception>
        public Matrix(int[] shape, double[] data)
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
            this.data = new double[shape[0], shape[1]];

            for (int i = 0; i < data.Length; i++)
            {
                int row = i / shape[1];
                int column = i % shape[1];
                this.data[row, column] = data[i];
            }
        }

        /// <summary>
        /// Multiplies by the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Matrix cannot be multiplied by null object.</exception>
        /// <exception cref="System.ArgumentException">These matrices cannot be multiplied as the column count of the first matrix is not equal to the row count on the second matrix.</exception>
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

        /// <summary>
        /// Multiplies by the specified scalar value.
        /// </summary>
        /// <param name="scalar">The scalar.</param>
        /// <returns></returns>
        public Matrix Multiply(double scalar)
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

        /// <summary>
        /// Adds the specified matrix to the current matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Matrix cannot be added to a null object.</exception>
        /// <exception cref="System.ArgumentException">These matrices cannot be added as they do not have the same shape.</exception>
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

        /// <summary>
        /// Subtracts the specified matrix from the current matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Matrix cannot be subtracted by null object.</exception>
        /// <exception cref="System.ArgumentException">These matrices cannot be subtracted as they do not have the same shape.</exception>
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

        /// <summary>
        /// Transposes this instance.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Finds the inverse matrix of this instance using Minors, Cofactors and Adjugates.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.ArithmeticException">This matrix cannot be inversed</exception>
        public Matrix Inverse()
        {
            double determinant = Determinant();
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

        /// <summary>
        /// Finds the Determinant of this instance.
        /// </summary>
        /// <returns></returns>
        public double Determinant() => Determinant(this);

        /// <summary>
        /// Calculates the DotProduct of a row and a column.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        private static double DotProduct(double[] row, double[] column)
        {
            double product = 0;
            for (int i = 0; i < row.Length; i++)
            {
                product += row[i] * column[i];
            }
            return product;
        }

        /// <summary>
        /// Finds the determinant of the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix to find the determinant of.</param>
        /// <returns></returns>
        private static double Determinant(Matrix matrix)
        {
            int sign = -1;
            double determinant = 0f;

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

        /// <summary>
        /// Gets or sets the <see cref="System.Single"/> with the specified row.
        /// </summary>
        /// <value>
        /// The <see cref="System.Single"/>.
        /// </value>
        /// <param name="row">The row.</param>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        public double this[int row, int column]
        {
            get => data[row, column];
            set => data[row, column] = value;
        }

        /// <summary>
        /// Gets the specified row.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <returns></returns>
        /// 
        private double[] GetRow(int row) => Enumerable.Range(0, shape[1])
                .Select(i => data[row, i])
                .ToArray();

        /// <summary>
        /// Gets the specified column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        private double[] GetColumn(int column) => Enumerable.Range(0, shape[0])
                .Select(i => data[i, column])
                .ToArray();

        /// <summary>
        /// Converts the matrix into a formatted string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
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

        /// <summary>
        /// Performs an implicit conversion from <see cref="Matrix"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="a">a.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(Matrix a) => a.ToString();

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Matrix operator +(Matrix a, Matrix b) => a.Add(b);


        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Matrix operator -(Matrix a, Matrix b) => a.Subtract(b);

        /// <summary>
        /// Implements the operator * between two matrices. The resultant matrix will have the same amount of rows as the first matrix and the same amount of columns as the second.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Matrix operator *(Matrix a, Matrix b) => a.Multiply(b);

        /// <summary>
        /// Implements the operator * between a scalar and a matrix.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Matrix operator *(double a, Matrix b) => b.Multiply(a);

    }
}
