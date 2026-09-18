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

        //revisar si alguien se quedo sin fichas
        int fichasClaras = contarFichas(tablero, 1);
        int fichasOscuras = contarFichas(tablero, 2);

        if (fichasClaras == 0)
        {
            DibujarTablero(tablero); //mostramos el tablero final
            Console.WriteLine("Las Oscuras ganan. Ya no hay mas fichas claras.");
            juegoActivo = false; //detenemos el while
        }
        else if (fichasOscuras == 0)
        {
            DibujarTablero(tablero);
            Console.WriteLine("Las Claras ganan. Ya no hay mas fichas oscuras.");
            juegoActivo = false;
        }

        //revisar si el jugar que sigue esta acorralado
        else if (jugadorEstaAcorralado(tablero, turnoActual))
        {
            DibujarTablero(tablero);
            string ganador = (turnoActual == 1) ? "Oscuras" : "Claras"; //gana el que no esta acorralado
            Console.WriteLine($"Las {ganador} ganan. El otro jugador quedo acorralado/sin movimientos posibles.");
            juegoActivo = false;
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
    // identificamos si la ficha del origen es del jugador en turno
    int valorFicha = tablero[filaOrigen, columnaOrigen];// comparamos si es ficha o si es Dama (1, 2 para fichas) (3, 4 para Damas)
    bool esFichaDelTurno = (turnoActual == 1 && (valorFicha == 1 || valorFicha == 3)) || (turnoActual == 2 && (valorFicha == 2 || valorFicha == 4));

    
    if (!esFichaDelTurno)
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

    bool esDama = (valorFicha == 3 || valorFicha == 4);

    int distanciaFila = filaDestino - filaOrigen;
    int distanciaColumna = columnaDestino - columnaOrigen;

    bool esMovimientoSimple;
    bool esCaptura;

    if (esDama)
    {
        esMovimientoSimple = (distanciaFila == 1 || distanciaFila == -1) && (distanciaColumna == 1 || distanciaColumna == -1);
        esCaptura = (distanciaFila == 2 || distanciaFila == -2) && (distanciaColumna == 2 || distanciaColumna == -2);
    }
    else
    {
        //Calcular direccion segun el color
    int direccion = (turnoActual == 1) ? 1 : -1; // claras avanzan +1, oscuras -1

    // Valida si el movimiento es simple (1 casilla en diagonal)
    esMovimientoSimple = (distanciaFila == direccion) && (distanciaColumna == 1 || distanciaColumna == -1);

    //Valida si el movimiento es una captura (2 casillas en diagonal, saltando rival)
    esCaptura = (distanciaFila == direccion * 2) && (distanciaColumna == 2 || distanciaColumna == -2);
    }
    
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

        int valorComida = tablero[filaComida, columnaComida];
        bool esRival = (turnoActual == 1 && (valorComida == 2 || valorComida == 4)) || (turnoActual == 2 && (valorComida == 1 || valorComida == 3));


        if (!esRival)
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

    //revisar coronación
    if (tablero[filaDestino, colDestino] == 1 && filaDestino == 7) //si la ficha que se movio es 1(clara) y llega a la ultima fila (7) se corona
    {
        tablero[filaDestino, colDestino] = 3; //3 es dama clara
        Console.WriteLine("La ficha se coronó a Dama.");
    }
    else if (tablero[filaDestino, colDestino] == 2 && filaDestino == 0) //si la ficha que se movio es 2(oscura) y llega a la ultima fila (0) se corona
    {
        tablero[filaDestino, colDestino] = 4; //4 es dama oscura
        Console.WriteLine("La ficha se coronó a Dama.");
    }
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
    //identificamos que ficha hay realmente en la casilla
    int valorFicha = tablero[fila, columna];
    bool esDama = (valorFicha == 3 || valorFicha == 4);

    //armamos la lista de direccions de fila a revisar
    int [] direccionesFila;
    if (esDama)
    {
        direccionesFila = new int[] { 1, -1 }; //la dama puede saltar hacia adelante y hacia atras

    }
    else
    {
        int direccionFija = (turnoActual == 1) ? 1 : -1;
        direccionesFila = new int[] { direccionFija }; //ficha normal su unica direccion permitida

    }

    //recorremos cada direccion posible de fila
    foreach (int direccion in direccionesFila)
    {
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

                int valorIntermedio = tablero[filaIntermedia, columnaIntermedia];

                //comprobar si lo que hay en medio es rival (ficha normal o Dama) 

                bool esRival = (turnoActual == 1 && (valorIntermedio == 2 || valorIntermedio == 4)) || (turnoActual == 2 && (valorIntermedio == 1 || valorIntermedio == 3));

                if (esRival && tablero[filaSalto, columnaSalto] == 0)
                {
                    return true;
                }          
            }
        }
    
       
    }

    return false;
}

int contarFichas(int[,] tablero, int jugador)
{
    int contador = 0; //empieza en 0 antes de iniciar el recorrido

    for (int fila = 0; fila < 8; fila++)
    {
        for( int columna = 0; columna < 8; columna++)
        {
            int valor = tablero[fila, columna];

            //si el jugador es claras (1) contamos fichas normales y damas (1 y 3)
            //si el jugador es oscura (2) contamos fichas normales y damas (2 y 4)
            bool esDeEsteJugador = (jugador == 1 && (valor == 1 || valor == 3)) || (jugador == 2 && (valor == 2 || valor == 4));

            if(esDeEsteJugador)
            {
                contador++; //le sumamos 1 al contador
            }
        }
    }

    return contador; //regresamos el contador encontrado
}

//detectar si una ficha especifica puede hacer un movimiento simple
bool fichaPuedeMoverse (int[,] tablero, int fila, int columna, int turnoActual)
{
    //identificamos que ficha hay realmente en esa casilla
    int valorFicha = tablero[fila, columna];
    bool esDama = (valorFicha == 3 || valorFicha == 4);


    //Armamos la lista de direcciones de fila
    //si es dama revisa ambas direcciones.
    //si es ficha normal, solo su unica direccion permitida.
    int [] direccionesFila;
    if (esDama)
    {
        direccionesFila = new int [] { 1, -1 }; //Dama puede moverse hacia adelante y atras

    }
    else
    {
        int direccionFija = (turnoActual == 1) ? 1 : -1;
        direccionesFila = new int[] { direccionFija }; //ficha normal con direccion unica
    }
    // recorremos cada direccion posible de fila (1 vuelta si es normal, 2 vueltas si es dama)
    foreach (int direccion in direccionesFila)
    {
        int nuevaFila = fila + direccion;
        int [] columnasPosibles = { columna - 1, columna + 1 }; //las dos diagonales posibles

        //revisamos cada una de las 2 posibles columnas
        foreach ( int nuevaColumna in columnasPosibles)
        {
            //validamos que esa casilla de destino exista dentro del tablero
            if (nuevaFila >= 0 && nuevaFila <= 7 && nuevaColumna >= 0 && nuevaColumna <= 7)
            {   
                //si esa casilla destino esta vacia, si hay un movimiento simple disponible
                if (tablero [nuevaFila, nuevaColumna] == 0)
                {
                    return true; // encontro al menos una casilla vacia donde moverse
                }
            }
        }
    }

    return false; // si recorrimos todas las direcciones columnas posibles y ninguna sirvio, no puede moverse simple
}

//Detectar si el jugador esta acorralado
bool jugadorEstaAcorralado (int[,] tablero, int turnoActual)
{
    //recorremos todas las casillas una por una
    for (int fila = 0; fila < 8; fila++)
    {
        for (int columna = 0; columna < 8; columna++)
        {
            int valor = tablero[fila, columna];

            //revisamos si la casilla tiene una ficha del jugador en turno
            bool esDeEsteJugador = (turnoActual == 1 && (valor == 1 || valor == 3)) || (turnoActual == 2 && (valor == 2 || valor == 4));

            //si no es ficha del jugador en turno la saltamos y seguimos con la siguiente casilla
            if (!esDeEsteJugador)
            {
                continue;
            }

            //si la ficha si puede comer o moverse simple el jugador si tiene una opcion disponible, no esta acorralado
            if (FichaPuedeComer(tablero, fila, columna, turnoActual) || fichaPuedeMoverse (tablero, fila, columna, turnoActual))

            {
                return false; // se encontro al menos una ficha con movimiento posible

            }
        }
    }

    return true; // si no hay movimiento posible el jugador esta acorralado
}