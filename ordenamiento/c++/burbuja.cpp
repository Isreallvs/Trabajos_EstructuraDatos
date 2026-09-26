#include <iostream>
using namespace std;

void bubblesort(int a[], int s)
{
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
                swap(a[j], a[j + 1]);
                isSwapped = true;
            }
        }
        if (!isSwapped)
            break;
    }
}
int main ()
{
    int a[] = {15, 16, 11, 13, 14};
    int s = sizeof (a) / sizeof(a[0]);

    cout << "Antes de ordenar los elementos del arrray son: "<< endl;
    for (int j = 0; j < s; j++)
        cout << a[j] << " ";

    bubblesort(a, s);

    cout << "\nDespues de ordenar los elementos del array son: "<< endl;
    for (int j = 0; j < s; j++)
        cout << a[j] << " ";

    return 0;


}   

