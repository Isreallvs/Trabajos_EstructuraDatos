#include <iostream>
using namespace std;

void printRow(int fila[], int c)
{
    cout << "[";
    for (int i = 0; i < c; i++)
    {
        cout << fila[i];
        if (i < c - 1)
            cout << ", ";
    }
    cout << "]" << endl;
}

int main ()
{
    int r = 3;
    int c = 3;
    int arr[3][3] = {0}; // creamos una matriz vacia

    int TwoDArr[3][3] = {
        {1, 2, 3},
        {4, 5, 6},
        {7, 8, 9}
    };

    for (int x = 0; x < r * c; x++)
    {
        int fila = x % r;
        int columna = x / r;
        arr[fila][columna] = TwoDArr[x / c][x % c];
    }

    cout << "Los elementos del array bidimensional son:" << endl;
    for (int i = 0; i < r; i++)
        printRow(TwoDArr[i], c);

    cout << "\nLa matriz ordenada por columnas es:" << endl;
    for (int i = 0; i < r; i++)
        printRow(arr[i], c);

    return 0;
}