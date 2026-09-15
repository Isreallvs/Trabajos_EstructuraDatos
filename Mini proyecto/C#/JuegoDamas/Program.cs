using System.Data;

int[,] tablero = new int [8, 8];
for (int fila = 0; fila < 8; fila++)  // ciclo externo
{
    for (int columna = 0; columna < 8; columna ++) // ciclo interno
    {
        if ((fila + columna) % 2 != 0) // solo casillas oscuras
        {
            if (fila < 3)
            {
                tablero[fila, columna] = 1; // fichas claras (empiezan el juego)
            }
            else if (fila > 4)
            {
                tablero[fila, columna] = 2; // ficha oscura
            }
        }
    }
}

DibujarTablero(tablero);

void DibujarTablero(int[,] tablero)
{
    for (int fila = 0; fila < 8; fila ++)
    {
        for (int columna = 0; columna < 8; columna ++)
        {
            char simbolo = '.'; // Casilla vacia

            if (tablero[fila, columna] == 1) simbolo = 'b';
            else if (tablero[fila, columna] == 2) simbolo = 'r';
            else if (tablero[fila, columna] == 3) simbolo = 'B';
            else if (tablero[fila, columna] == 4) simbolo = 'R';

            Console.Write(simbolo + " ");  //espacio para separar columnas 
        }
        Console.WriteLine(); // salto de linea al terminar cada fila
    }
}

RevisarMovimientos(tablero, 2, 1);

void RevisarMovimientos (int[,] tablero, int fila, int columna)
{
    if (tablero[fila, columna] == 1 || tablero[fila, columna] == 2)
    {
        int direccion;

        if (tablero[fila, columna] == 1)
        {
            direccion = 1;
        }
        else
        {
            direccion = -1;
        }

        int nuevaFila = fila + direccion;
        int[] columnasPosibles = { columna - 1, columna + 1};

        foreach (int nuevaColumna in columnasPosibles)
        {
            if (nuevaFila >= 0 && nuevaFila <= 7 && nuevaColumna >= 0 && nuevaColumna <= 7)
            {
                if (tablero[nuevaFila, nuevaColumna] == 0)
                {
                    Console.WriteLine($"Movimiento válido a: ({nuevaFila}, {nuevaColumna})");
                }
                else
                {
                    Console.WriteLine($"Casilla ({nuevaFila}, {nuevaColumna}) ocupada, no se puede mover ahí");

                }
            }
        }
    }
}