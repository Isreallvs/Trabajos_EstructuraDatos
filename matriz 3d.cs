using System;
class ThreeDArray
{
    static void Main(string[] args)
    {
        int [,,] ThreeDimensionalArray = {
            {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9}
            },
            {
            {10, 11, 12},
            {13, 14, 15},
            {16, 17, 18}  
            }
        };

        Console.WriteLine("Los elementos del array son: ");
        for (int i = 0; i < ThreeDimensionalArray.GetLength(0); i++)
        {
            for (int j = 0; j < ThreeDimensionalArray.GetLength(1); j++)
            {
                for (int k = 0; k < ThreeDimensionalArray.GetLength(2); k++)
                {
                    Console.Write(ThreeDimensionalArray[i, j, k] + " "); // mostrar los elementos de la fila separados por espacios
                }
            }
            Console.WriteLine(); // ir a la siguiente línea después de mostrar un bloque 2D
        }
    }
}