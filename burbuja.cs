using System;

class BubbleSort
{
    static void Bubblesort(int[]a)
    {
        int s = a.Length;
        // Iterando por todos los elementos del array
        for (int i = 0; i < s; i++)
        {
            bool isSwapped = false;
            // Los últimos i elementos ya están en su lugar correspondiente.
            for (int j = 0; j < s - i - 1; j++)
            {
                // Intercambiando si el elemento encontrado es mayor que el siguiente
                if (a[j] > a[j + 1])
                {
                    int temp = a[j];
                    a[j] = a[j + 1];
                    a[j + 1] = temp;
                    isSwapped = true;
                }
            }
            if (!isSwapped )
                break;
        }
    }
    static void Main(string[] args)
    {
        int [] a = {15, 16, 11, 13, 14 };

        Console.WriteLine("Antes de ordenar los elementos del array son: ");
        foreach (int j in a)
            Console.Write(j + " ");
        
        Bubblesort(a);

        Console.WriteLine("\nDespues de ordenar los elementos del array son: ");
        foreach (int j in a)
            Console.Write(j + " ");
    }
}    