public class MergeSort 
{
    // merge dos subarrays de a[]
    static void merge(int[] a, int l, int m, int r) 
    {
        int a1 = m - l + 1; // tamaño del primer subarray
        int a2 = r - m; // tamaño del segundo subarray

        // crear arrays temporales
        int[] L = new int[a1]; // crea el array temporal
        int[] R = new int[a2]; // crea el array temporal

        // copiar datos a los arrays temporales L[] y R[]
        for (int j = 0; j < a1; j++)
            L[j] = a[l + j];
        for (int k = 0; k < a2; k++) // copiar datos al array temporal
            R[k] = a[m + 1 + k];

        int i = 0; // indice inicial del primer subarray
        int j = 0; // indice inicial del segundo subarray
        int k = l; // indice inicial del subarray mezclado

        // mezclar los arrays temporales de nuevo en a[l..r]
        while (i < a1 && j < a2)// recorrer ambos arrays 
        { 
            if (L[i] <= R[j]) // comparar los elementos de ambos arrays
            { 
                a[k] = L[i]; // copiar el elemento mas pequeño al array original
                i = i + 1; // incrementar el indice del primer array
            } else // si el elemento del segundo array es mas pequeño
            { 
                a[k] = R[j]; // copiar el elemento mas pequeño del array original
                j = j + 1; // incrementar el indice del segundo array
            }
            k = k + 1; // incrementar el indice del array original
        }
        // copiar los elementos restantes de L[], si hay alguno
        while (i < a1) // copiar los elementos restantes del primer array
        { 
            a[k] = L[i]; // copiar el elemento al array original
            i = i + 1; // incrementar el indice del primer array
            k = k + 1; // incrementar el indice del array original
        }
    }

    // l es para el indice izquierdo y r es para el indice derecho del subarray de 'a' a ser ordenado
    static void mergeSort(int[] a, int l, int r) // funcion principal que ordena a[l..r]
    { 
        if (l < r) 
        {
            // igual que (l + r)/2, pero evita el desbordamiento para grandes valores de l y h
            int m = l + (r - l) / 2;
            // ordenar la primera y segunda mitad
            mergeSort(a, l, m); // ordenar la primera mitad
            mergeSort(a, m + 1, r); // ordenar la segunda mitad
            merge(a, l, m, r); // mezclar las dos mitades
        }
    }

    public static void main (String[] args)
    {
        int[] a = {39, 28, 44, 11};
        int s = a.length;

        System.out.println("Antes de ordernar el arreglo: ");
        for (int j = 0; j < s; j++)
            System.out.print(a[j] + " ");

        mergeSort(a, 0, s - 1); // llama a la funcion mergesort

        System.out.println("\nDespues de ordenar el arreglo: ");
        for (int j = 0; j < s; j++)
            System.out.print(a[j] + " ");
    }
    
}
