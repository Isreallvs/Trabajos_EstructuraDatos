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
    Console.WriteLine("    0 1 2 3 4 5 6 7"); //Encabezado con numero de las columnas
    Console.WriteLine("  +-----------------");
    for (int fila = 0; fila < 8; fila ++)
    {
        Console.Write(fila + " | "); //numero de fila al inicio del renglon

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
void RevisarMovimientos(int[,] tablero, int fila, int columna)
{
    if (tablero[fila, columna] == 1 || tablero[fila, columna] == 2)
    {
        int direccion = (tablero[fila, columna] == 1) ? 1 : -1;
        int rival = (tablero[fila, columna] == 1) ? 2 : 1; // Identificamos la ficha contraria

        int nuevaFila = fila + direccion;
        int[] columnasPosibles = { columna - 1, columna + 1 };

        foreach (int nuevaColumna in columnasPosibles)
        {
            // primero revisamos si podemos comer (saltar 2 casillas)
            int filaSalto = fila + (direccion * 2);
            int columnaSalto = (nuevaColumna < columna) ? columna - 2 : columna + 2;

            // validar que el salto caiga dentro del tablero (0 a 7)
            if (filaSalto >= 0 && filaSalto <= 7 && columnaSalto >= 0 && columnaSalto <= 7)
            {
                // si la casilla al lado tiene un rival Y la casilla tras él está vacía (0)
                if (tablero[nuevaFila, nuevaColumna] == rival && tablero[filaSalto, columnaSalto] == 0)
                {
                    RealizarMovimiento(tablero, fila, columna, filaSalto, columnaSalto);
                    return; // Salimos para completar solo esta acción
                }
            }

            // si no hay quien comer hacemos movimiento simple (1 casilla)
            if (nuevaFila >= 0 && nuevaFila <= 7 && nuevaColumna >= 0 && nuevaColumna <= 7)
            {
                if (tablero[nuevaFila, nuevaColumna] == 0)
                {
                    RealizarMovimiento(tablero, fila, columna, nuevaFila, nuevaColumna);
                    return;
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

    if (Math.Abs(filaDestino - filaOrigen) == 2)// Si el movimiento fue de 2 filas de distancia siginifica que salto y comio
    {
        int filaComida = (filaOrigen + filaDestino) / 2; //Calcula la casilla intermedia
        int colComida = (colOrigen + colDestino) / 2;
        tablero[filaComida, colComida] = 0; //borra la ficha del rival
        Console.WriteLine($"Ficha rival comida en ({filaComida}, {colComida})");
    }
    Console.WriteLine($"Movimiento realizado exitosamente");
}