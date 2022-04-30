using System.Diagnostics;

namespace _3D_Engine
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            float[] matrix_data = { 1f, 1f, 1f,
                                    2f, 2f, 4f,
                                    3f, 2f, 6f};
            float[] matrix_data2 = { 1f, 3f, 5f };
            int[] shape = { 3, 3 };
            int[] shape2 = { 3, 1 };
            Matrix matrix = new Matrix(shape, matrix_data);
            Matrix matrix2 = new Matrix(shape2, matrix_data2);
            Matrix result = matrix.Multiply(matrix2);
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}