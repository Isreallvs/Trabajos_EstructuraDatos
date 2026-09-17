using System.Data;
Console.OutputEncoding = System.Text.Encoding.UTF8;

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

// Turnos y movimientos
int turnoActual = 1; //Empieza en 1 porque las fichas claras empiezan el juego

bool juegoActivo = true; //Se usa como interruptor, para saber si el juego debe seguir corriendo

bool debeSeguirComiendo = false; //para saber si el mismo jugador debe seguir jugando

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
        if (!debeSeguirComiendo)
        {
            turnoActual = (turnoActual == 1) ? 2 : 1; // Nos dice si el turno actual era 1, ahora pasa a ser 2, sino pasa a ser 1
        }
        
    }
    
}



//Funciones Visuales ("interfaz")
void DibujarTablero(int[,] tablero)
{
    Console.ForegroundColor = ConsoleColor.Gray; // color neutro para encabezados
    Console.WriteLine("    0 1 2 3 4 5 6 7");
    Console.WriteLine("  +-----------------");

    for (int fila = 0; fila < 8; fila++)
    {
        Console.ForegroundColor = ConsoleColor.Gray; // número de fila en gris
        Console.Write(fila + " | ");

        for (int columna = 0; columna < 8; columna++)
        {
            int valor = tablero[fila, columna];

            char simbolo = '.';

            if (valor == 1)
            {
                Console.ForegroundColor = ConsoleColor.White;
                simbolo = '●';
            }
            else if (valor == 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                simbolo = '●';
            }
            else if (valor == 3)
            {
                Console.ForegroundColor = ConsoleColor.White;
                simbolo = '♛';
            }
            else if (valor == 4)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                simbolo = '♛';
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray; 
            }

            Console.Write(simbolo + " ");
        }

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine();
    }

    Console.ForegroundColor = ConsoleColor.Gray;
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

    // checamos si hay una captura obligatoria en todo el tablero
    bool capturaObligatoria = capturaDisponible(tablero, turnoActual);


    if (capturaObligatoria && esMovimientoSimple)
    {
        //Si hay captura obligatoria y el jugador intenta movimiento simple, lo rehazamos
        Console.WriteLine("Puedes comer una ficha del rival, estas obligado a comer.");
        return false;
    }

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
        Console.WriteLine($"Comiste una ficha en ({filaComida}, {columnaComida})");
        
        //preguntamos si desde la nueva posicion puede seguir comiendo
        if (FichaPuedeComer (tablero, filaDestino, columnaDestino, turnoActual))
        {
            Console.WriteLine("Puedes seguir comiendo con la misma ficha. Vuelve a mover.");
            debeSeguirComiendo = true; //Avisamos que el jugador deber repetir turno

        }
        else
        {
            debeSeguirComiendo = false; //el turno pasa al otro jugador normalmente

        }
        return true;
    }
    else if (esMovimientoSimple)
    {
        RealizarMovimiento(tablero, filaOrigen, columnaOrigen, filaDestino, columnaDestino);
        Console.WriteLine("Movimiento realizado.");

        debeSeguirComiendo = false;

        return true;
    }
    else
    {
        Console.WriteLine("Ese movimiento no es válido no es diagonal correcta.");
        return false;
    }

}

//Motor de movimiento
void RealizarMovimiento(int[,] tablero, int filaOrigen, int colOrigen, int filaDestino, int colDestino)
{
    tablero[filaDestino, colDestino] = tablero[filaOrigen, colOrigen]; //Copia el numero de origen y lo pega en la casilla de destino
    tablero[filaOrigen, colOrigen] = 0; //Le da el valor de 0 a la casilla vieja para no clonar el numero que se copio

}

//Captura obligatoria disponible

bool capturaDisponible(int [,] tablero, int turnoActual)
{
    //Recorremos todas las casillas del tablero una por una
    for (int fila = 0; fila < 8; fila++)
    {
        for (int columna = 0; columna < 8; columna++)
        {
            //Si no es turno de la ficha de este jugar la saltamos
            if (tablero[fila, columna] != turnoActual)
            {
                continue;
            }

            if (FichaPuedeComer( tablero, fila, columna, turnoActual))
            {
                return true;
            }            
        }
    }
    return false; //Si ya recorrimos todas las casillas y no se regresa un true es porque no hay capturas disponibles
}

//Funcion para ver si una ficha especifica puede comer

bool FichaPuedeComer(int[,] tablero, int fila, int columna, int turnoActual)
{
    int direccion = (turnoActual == 1) ? 1 : -1;
    int rival = (turnoActual == 1) ? 2 : 1;

    int filaSalto = fila + (direccion * 2); // 2 filas de distancia, asi salta al comer
    int[] columnasSalto = {columna -2, columna + 2};// las 2 posibles columnas donde puede caer

    foreach (int columnaSalto in columnasSalto)
    {
        //validamos la existencia de la casilla donde va a caer
        if (filaSalto >= 0 && filaSalto <= 7 && columnaSalto >= 0 && columnaSalto <= 7)
        {
            //punto medio es la casilla intermedia donde deberia estar el rival
            int filaIntermedia = (fila + filaSalto) / 2;
            int columnaIntermedia = (columna + columnaSalto) / 2;

            //Si en la intermedia hay un rival, y donde va a caer esta vacia, si se puede comer
            if (tablero[filaIntermedia, columnaIntermedia] == rival && tablero[filaSalto, columnaSalto] == 0)
            {
                return true; // Se encuentra al menos una captura disponible, no es necesario seguir buscando

            }
        }
    }

    return false;
}