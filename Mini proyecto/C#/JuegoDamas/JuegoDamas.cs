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

int turnoActual = 1; //Empieza en 1 porque las fichas claras empiezan el juego

bool juegoActivo = true; //Se usa como interruptor, para saber si el juego debe seguir corriendo

while (juegoActivo) //While ya que no sabemos cuantos turnos va a durar una partida
{
    DibujarTablero(tablero); //mostramos el tablero actual en cada turno

    String colorTurno = (turnoActual == 1) ? "Claras" : "Oscuras";
    Console.WriteLine($"Turno del jugador: {colorTurno}");


    Console.WriteLine("Qué ficha quieres mover?");
    Console.Write("Fila origen: ");

    int filaOrigen= int.Parse(Console.ReadLine()!); //Readline nos lee el texto pero int.parse lo convierte a numero

    Console.Write("Columna origen: ");
    int columnaOrigen = int.Parse(Console.ReadLine()!);

    Console.WriteLine("A donde la quieres mover?");
    Console.Write("Fila destino: ");
    int filaDestino = int.Parse(Console.ReadLine()!);
    Console.Write("Columna destino: ");
    int columnaDestino = int.Parse(Console.ReadLine()!);

    bool movimientoExitoso = IntentarMover(tablero, filaOrigen, columnaOrigen, filaDestino, columnaDestino, turnoActual); // Probamos el radar con una ficha especifica

    if (movimientoExitoso)
    {
        turnoActual = (turnoActual == 1) ? 2 : 1; // Nos dice si el turno actual era 1, ahora pasa a ser 2, sino pasa a ser 1
    }
    
}



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
            int valor = tablero[fila, columna];

            if (valor == 1 || valor == 3)
            {
                Console.ForegroundColor = ConsoleColor.White; //fichas claras de color blanco
            }
            else if (valor == 2 || valor == 4)
            {
                Console.ForegroundColor = ConsoleColor.Red; //fichas oscuras color rojo
            }
            char simbolo = '.'; // Casilla vacia

            if (valor == 1) simbolo = '●';
            else if (valor == 2) simbolo = '●';
            else if (valor == 3) simbolo = '♛';
            else if (valor == 4) simbolo = '♛';

            Console.Write(simbolo + " ");  //espacio para separar columnas 
        }
        Console.WriteLine(); // salto de linea al terminar cada fila
    }
}


//Logica del juego (Movimientos y reglas)
bool IntentarMover(int[,] tablero, int filaOrigen, int columnaOrigen, int filaDestino, int columnaDestino, int turnoActual)
{
    if (tablero[filaOrigen, columnaOrigen] == 0) //Validamos que exista una ficha en el origen
    {
        Console.WriteLine("No hay ninguna ficha en esa casilla.");
        return false;
    }

    // Si es turno de las claras (1), no puede mover una ficha oscura (2), y viceversa.
    if (tablero[filaOrigen, columnaOrigen] != turnoActual)
    {
        Console.WriteLine("Esa ficha no es tuya, no puedes moverla en este turno.");
        return false;
    }

    if (filaDestino < 0 || filaDestino > 7 || columnaDestino < 0 || columnaDestino > 7)// validar que el destino este dentro del tablero
    {
        Console.WriteLine("Ese destino está fuera del tablero.");
        return false;
    }

    if (tablero[filaDestino, columnaDestino] != 0) //Validar que no haya fichas en el destino
    {
        Console.WriteLine("Esa casilla ya está ocupada.");
        return false;
    }

    //Calcular direccion segun el color
    int direccion = (turnoActual == 1) ? 1 : -1; // claras avanzan +1, oscuras -1

    int distanciaFila = filaDestino - filaOrigen;
    int distanciaColumna = columnaDestino - columnaOrigen;

    // Valida si el movimiento es simple (1 casilla en diagonal)
    bool esMovimientoSimple = (distanciaFila == direccion) && (distanciaColumna == 1 || distanciaColumna == -1);

    //Valida si el movimiento es una captura (2 casillas en diagonal, saltando rival)
    bool esCaptura = (distanciaFila == direccion * 2) && (distanciaColumna == 2 || distanciaColumna == -2);

    if (esCaptura)
    {
        // La ficha rival debe estar justo a la mitad del salto
        int filaComida = (filaOrigen + filaDestino) / 2;
        int columnaComida = (columnaOrigen + columnaDestino) / 2;

        int rival = (turnoActual == 1) ? 2 : 1;

        if (tablero[filaComida, columnaComida] != rival)
        {
            Console.WriteLine("No hay ficha rival para comer en ese salto.");
            return false;
        }

        // Todo válido: ejecutamos el movimiento y quitamos la ficha comida
        RealizarMovimiento(tablero, filaOrigen, columnaOrigen, filaDestino, columnaDestino);
        tablero[filaComida, columnaComida] = 0;
        Console.WriteLine($"¡Comiste una ficha en ({filaComida}, {columnaComida})!");
        return true;
    }
    else if (esMovimientoSimple)
    {
        RealizarMovimiento(tablero, filaOrigen, columnaOrigen, filaDestino, columnaDestino);
        Console.WriteLine("Movimiento realizado.");
        return true;
    }
    else
    {
        Console.WriteLine("Ese movimiento no es válido (no es diagonal correcta).");
        return false;
    }
}

//Motor de movimiento
void RealizarMovimiento(int[,] tablero, int filaOrigen, int colOrigen, int filaDestino, int colDestino)
{
    tablero[filaDestino, colDestino] = tablero[filaOrigen, colOrigen]; //Copia el numero de origen y lo pega en la casilla de destino
    tablero[filaOrigen, colOrigen] = 0; //Le da el valor de 0 a la casilla vieja para no clonar el numero que se copio

}