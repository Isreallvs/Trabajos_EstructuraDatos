using System;
class FlattenArray
{
    static void Main(string[] args)
    {
        int r = 3, c = 3;
        int[] arr = new int[r * c];

        // Matriz inicializada y luego se le asigna un valor
        int[,] TwoDArr = {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 9 }
        }; // almacenar elementos en un array unidimensional ordenados por filas

        int k = 0;
        for (int x = 0; x < r; x++)
        {
            for (int y = 0; y < c; y++)
            {
                k = x * r + y;
                arr[k] = TwoDArr[x, y];
                k = k + 1;
            }
        }

        Console.WriteLine("Los elementos del array bidimensional son: ");
        for (int x = 0; x < r; x++)
        {
            for (int y = 0; y < c; y++)
            {
                Console.Write(TwoDArr[x, y] + " "); // mostrar los elementos de la fila separados por espacios
            }
            Console.WriteLine(); // ir a la siguiente línea después de mostrar una fila
        }

        Console.WriteLine("\nLos elementos del array unidimensional son: ");
        // imprimir los elementos del array unidimensional
        for (int x = 0; x < r; x++)
        {
            for (int y = 0; y < c; y++)
            {
                Console.Write(arr[x * r + y] + " "); // mostrar los elementos de la fila separados por espacios
            }
        }

    }
}