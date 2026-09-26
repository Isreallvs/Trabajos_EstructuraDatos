#include <iostream>
using namespace std;

void selection(int a[], int s) //funcion para implementar el algoritmo de sleccion
{
    for (int i = 0; i < s; i++) // recorre todo el arreglo
    {
        int small = i; // índice del elemento más pequeño
        for (int j = i + 1; j < s; j++) // encuentra el elemento más pequeño en el arreglo
        {
            if (a[small] > a[j]) // compara el elemento más pequeño con el siguiente elemento
            {
                small = j; // actualiza el índice del elemento más pequeño
            }
            // intercambia el elemento más pequeño con el primer elemento
            swap(a[i], a[small]); // intercambia los elementos
        }
    }
}

void printArr(int a[], int s) 
{
    for (int i = 0; i < s; i++) // recorre todo el arreglo
        cout << a[i] << " "; // imprime el elemento
}

int main()
{
    int a[] = { 65, 26, 13, 23, 12};
    int s = sizeof(a) / sizeof(a[0]);

    cout<< "Arreglo antes de ser ordenado: "<< endl;
    printArr(a, s);

    selection(a, s);

    cout << "\nArreglo despues de ser ordenado: " << endl;
    selection(a, s);
    printArr(a, s);

    return 0;
}