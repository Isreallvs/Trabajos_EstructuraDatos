using System;

class InsertionSort
{
    static void MetodoDeInsercion(int[] a)
    {
        for (int i = 1; i < a.Length; i++)
        {
            int temp = a[i];
            // Mueve los elementos mayores que temp
            // a una posición más adelante de su posición actual
            int j = i - 1;
            while (j >= 0 && temp < a[j])
            {
                a[j + 1] = a[j];
                j = j - 1;
            }
            a[j + 1] = temp;
        }
    }

    static void PrintArr(int[] a) // función para imprimir el array
    {
        foreach (int i in a)
            Console.Write(i + " ");
    }

    static void Main(string[] args)
    {
        int[] a = { 70, 15, 2, 51, 60 };

        Console.WriteLine("Antes de ordenar los elementos del arreglo: ");
        PrintArr(a);

        MetodoDeInsercion(a);

        Console.WriteLine("\nDespués de ordenar los elementos del arreglo: ");
        PrintArr(a);
    }
}