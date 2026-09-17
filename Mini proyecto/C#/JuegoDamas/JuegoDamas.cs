using System.Data;

//Configuracion inicial del tablero

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

//Ejecucion de prueba del juego

DibujarTablero(tablero); //Imprime el tablero original

Console.WriteLine("Qué ficha quieres mover?");
Console.Write("Fila: ");

int filaElegida = int.Parse(Console.ReadLine()!); //Readline nos lee el texto pero int.parse lo convierte a numero

Console.Write("Columna: ");
int colElegida = int.Parse(Console.ReadLine()!);

RevisarMovimientos(tablero, filaElegida, colElegida); // Probamos el radar con una ficha especifica

DibujarTablero(tablero); //Dibujamos el tablero de nuevo para ver el resultado del movimiento


//Funciones Visuales ("interfaz")
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


//Logica del juego (Movimientos y reglas)
void RevisarMovimientos (int[,] tablero, int fila, int columna)
{
    if (tablero[fila, columna] == 1 || tablero[fila, columna] == 2)
    {
        int direccion = (tablero[fila, columna] == 1) ? 1 : -1;
        int nuevaFila = fila + direccion;
        int[] columnasPosibles = { columna - 1, columna + 1};

        foreach (int nuevaColumna in columnasPosibles)
        {
            if (nuevaFila >= 0 && nuevaFila <= 7 && nuevaColumna >= 0 && nuevaColumna <= 7)
            {
                if (tablero[nuevaFila, nuevaColumna] == 0)
                {
                    Console.WriteLine($"Movimiento válido a: ({nuevaFila}, {nuevaColumna})");

                    //Como el radar vio que esta vacio (0), mueve la ficha
                    RealizarMovimiento(tablero, fila, columna, nuevaFila, nuevaColumna);

                    //ponemos un break para que solo haga un movimiento
                    break;
                }
                else
                {
                    Console.WriteLine($"Casilla ({nuevaFila}, {nuevaColumna}) ocupada.");

                }

            }
        }
    }
}

//Motor de movimiento
void RealizarMovimiento(int[,] tablero, int filaOrigen, int colOrigen, int filaDestino, int colDestino)
{
    tablero[filaDestino, colDestino] = tablero[filaOrigen, colOrigen]; //Copia el numero de origen y lo pega en la casilla de destino
    tablero[filaOrigen, colOrigen] = 0; //Le da el valor de 0 a la casilla vieja para no clonar el numero que se copio

    Console.WriteLine($"Movimiento realizado exitosamente");
}