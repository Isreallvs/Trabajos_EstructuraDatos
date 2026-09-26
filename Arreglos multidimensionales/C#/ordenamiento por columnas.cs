using System;
using System.Text;

class MatrixTranspose
{
    static string FormatRow(int[] fila)
    {
        StringBuilder sb = new StringBuilder("[");
        for (int i = 0; i < fila.Length; i++)
        {
            sb.Append(fila[i]);
            if (i < fila.Length - 1)
                sb.Append(", ");
        }
        sb.Append("]");
        return sb.ToString();
    }

    static void Main(string[] args)
    {
        int r = 3;
        int c = 3;
        int[][] arr = new int[r][];
        for (int i = 0; i < r; i++)
            arr[i] = new int[c]; // creamos una matriz vacia

        int[][] TwoDArr = {
            new int[] { 1, 2, 3 },
            new int[] { 4, 5, 6 },
            new int[] { 7, 8, 9 }
        };

        for (int x = 0; x < r * c; x++)
        {
            int fila = x % r;
            int columna = x / r;
            arr[fila][columna] = TwoDArr[x / c][x % c];
        }

        Console.WriteLine("Los elementos del array bidimensional son:");
        foreach (int[] fila in TwoDArr)
            Console.WriteLine(FormatRow(fila));

        Console.WriteLine("\nLa matriz ordenada por columnas es:");
        foreach (int[] fila in arr)
            Console.WriteLine(FormatRow(fila));
    }
}