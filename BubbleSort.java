public class BubbleSort 
{
    static void bubblesort(int[] a)
    {
        int s = a.length;
        //Iterando por todos los elementos del array

        for (int i = 0; i < s; i++) 
        {
            boolean isSwapped = false;
            // Los últimos i elementos ya están en su lugar correspondiente.
            for (int j = 0; j < s - i - 1; j++)
            {
                //intercambiando si el elemento encontrado es mayor que el siguiente
                if (a[j] > a[j + 1])
                {
                    int temp = a[j];
                    a[j] = a[j + 1];
                    a[j + 1] = temp;
                    isSwapped = true;
                }
            }
            if (!isSwapped)
                break;
        }
    }

    public static void main(String[] args)
    {
        int[] a= {15, 16, 11, 13, 14};

        System.out.println("Antes de ordenar los elementos del array son: ");
        for (int j : a)
            System.out.print(j + " ");

        bubblesort(a);

        System.out.println("\nDespues de ordenar los elementos del array son: ");
        for (int j : a)
            System.out.print(j + " ");
    }
}
