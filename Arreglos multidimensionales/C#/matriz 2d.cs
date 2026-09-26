using System;
class TwoDArray
{
    static void Main(string[] args)
    {
        int[,] TwoDimensionalArray =
        {
            {1, 2, 3},
            {4, 5, 6},
            {7, 8, 9}
        };
        
        Console.WriteLine("Los elementos del array son: ");
        for (int i = 0; i < TwoDimensionalArray.GetLength(0); i++)
        {
            for (int j = 0; j < TwoDimensionalArray.GetLength(1); j++)
            {
                Console.Write(TwoDimensionalArray[i, j] + " "); // mostrando los elementos de la fila separados por espacios
            }
            Console.WriteLine(); // ir a la siguiente línea después de mostrar una fila
        }

    }
}