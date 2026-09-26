using System;
class SelectionSort
{
    static void Selection(int[] a) // función para implementar el algoritmo de selección
    {
        for (int i = 0; i < a.Length; i++) // recorre todo el arreglo
        {
            int small = i; // índice del elemento más pequeño
            for (int j = i + 1; j < a.Length; j++) // encuentra el elemento más pequeño en el arreglo
            {
                if (a[small] > a[j]) // compara el elemento más pequeño con el siguiente elemento
                {
                    small = j; // actualiza el índice del elemento más pequeño
                }
                // intercambia el elemento más pequeño con el primer elemento
                int temp = a[i];
                a[i] = a[small];
                a[small] = temp; // intercambia los elementos
            }
        }
    }

    static void PrintArr(int[] a)
    {
        foreach (int i in a )
            Console.Write(i + " ");
    }

    static void Main(string[] args)
    {
        int[] a = { 65, 26, 13, 23, 12};

        Console.WriteLine("Arreglo antes de ser ordenado: ");
        PrintArr(a);
        Selection(a);
        Console.WriteLine("\nArreglo después de ser ordenado: ");
        Selection(a);
        PrintArr(a);
    }
    
}