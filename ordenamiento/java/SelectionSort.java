package java;
public class SelectionSort {
    static void selection (int[] a)
    {
        for (int i = 0; i < a.length; i++)
        { // recorre todo el arreglo
            int small = i; // índice del elemento más pequeño
            for (int j = i + 1; j < a.length; j++) 
            { // encuentra el elemento más pequeño en el arreglo
                if (a[small] > a[j]) 
                { // compara el elemento más pequeño con el siguiente elemento
                    small = j; // actualiza el índice del elemento más pequeño
                }
                // intercambia el elemento más pequeño con el primer elemento
                int temp = a[i];
                a[i] = a[small];
                a[small] = temp; // intercambia los elementos
            }
        }
    }

    static void printArr (int[] a)
    {
        for (int i : a)
                System.out.print(i + " ");
    }

    public static void main(String[] args)
    {
        int[] a = {65, 26, 13, 23, 12};

        System.out.println("Arreglo antes de ser ordenado: ");
        printArr(a);

        selection(a);

        System.out.println("\nArreglo despues de ser ordenado: ");
        selection(a);
        printArr(a);
    }
}
