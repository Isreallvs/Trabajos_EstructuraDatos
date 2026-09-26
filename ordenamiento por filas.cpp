#include <iostream>
using namespace std;

int main()
{
    int r = 3, c = 3;
    int arr[9];

    // Matriz inicializada y luego se le asigna un valor
    int TwoDArr[3][3] = {
        {1, 2, 3},
        {4, 5, 6},
        {7, 8, 9}
    }; // almacenar elementos en un array unidimensional ordenados por filas

    int k = 0;
    for (int x = 0; x < r; x++)
    {
        for (int y = 0; y < c; y++)
        {
            k = x * r + y;
            arr[k] = TwoDArr[x][y];
            k = k + 1;
        }
    }

    cout<< "Los elementos del arrau bidimensional son: "<< endl;
    for (int x = 0; x < r; x++)
    {
        for (int y = 0; y < c; y++)
        {
            cout << TwoDArr[x][y] << " "; // mostrar los elementos de la fila separados por espacios
        }
        cout << endl; // ir a la siguiente línea después de mostrar una fila
    }

    cout << "\nLos elementos del array unidimensional son: "<< endl;
    // imprimir los elementos del array unidimensional
    for (int x = 0; x < r; x++)
    {
        for (int y = 0; y < c; y++)
        {
            cout << arr[x * r + y] << " "; // mostrar los elementos de la fila separados por espacios
        }
    }

    return 0;
}