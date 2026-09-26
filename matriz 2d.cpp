#include  <iostream>
using namespace std;

int main()
{
    int TwoDimensionalArray[3][3] = {
        {1, 2, 3},
        {4, 5, 6},
        {7, 8, 9}
    };

    cout<< "Los elementos del array son: "<< endl;
    for (int i = 0; i < 3; i++)
    {
        for (int j = 0; j < 3; j++)
        {
            cout << TwoDimensionalArray[i][j] << " "; // mostrando los elementos de la fila separados por espacios
        }
        cout << endl; // ir a la siguiente línea después de mostrar una fila
    }

    return 0;
}