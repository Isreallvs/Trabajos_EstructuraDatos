using System.Data;
using System.Runtime.InteropServices;
using System.Diagnostics;
Console.OutputEncoding = System.Text.Encoding.UTF8;

//Configuracion inicial del tablero

int[,] tablero = new int[8, 8];
for (int fila = 0; fila < 8; fila++)  // ciclo externo
{
    for (int columna = 0; columna < 8; columna++) // ciclo interno
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

int filaOrigen = 0;
int columnaOrigen = 0;

TimeSpan tiempoClaras = TimeSpan.FromMinutes(2); //tiempo restante de claras
TimeSpan tiempoOscuras = TimeSpan.FromMinutes(2);
Stopwatch relojTurno = Stopwatch.StartNew(); //mide unicamente el tiempo del jugador que esta jugando

int cursorFila = 0;//filla donde inicia el cursor
int cursorColumna = 0; //columna donde inicia el cursor
bool fichaSeleccionada = false; //indica si el jugador ya escogio una ficha
int filaSeleccionada = -1; //guarda la fila de la ficha seleccionada
int columnaSeleccionada = -1; //guarda la columna de la ficha seleccionada

List <(int fila, int columna)> destinosPosibles = new();//guardara las casillas validas iluminadas

string mensajeEstado = "";//mostrara avisos debajo del tablero

// Bloque de menu inicial 
const string carpetaPartidas = "Partidas";//define la carpeta unica de guardados
Directory.CreateDirectory(carpetaPartidas);//la crea solo si aun no existe

string nombreArchivo = "";
bool iniciarJuego = false; 

while (!iniciarJuego)
{
    Console.WriteLine("--- Bienvenido al juego Damas Inglesas ---");
    Console.WriteLine("¿Qué deseas hacer?");
    Console.WriteLine();
    Console.WriteLine("1. Nueva partida");
    Console.WriteLine("2. Cargar partida guardada");
    Console.WriteLine("3. Historial de partidas guardadas");
    Console.WriteLine("4. Salir");
    Console.WriteLine();


//bloque del nombre del archivo


    string opcionMenu = pedirTecla("Elige una opcion (1, 2, 3 o 4): ", "1", "2", "3", "4");

    if (opcionMenu == "1")//crea una partida nueva sin sobrescribir otra
    {
        bool nombreDisponible = false;//controla que se pida otro nombre si ya existe 

        while (!nombreDisponible)
        {
            Console.WriteLine("Cómo quieres nombrar esta partida? (ej: partida1): ");
            string entradaNombre = Console.ReadLine()!;
            nombreArchivo = limpiarNombreArchivo(entradaNombre);

            string rutaNuevaPartida = Path.Combine(carpetaPartidas, nombreArchivo);
            nombreDisponible = !File.Exists(rutaNuevaPartida);//sera true solo si no existe el archivo

            if (!nombreDisponible)
            {
                Console.WriteLine("Ya existe una partida con ese nombre. Escribe otro nombre.");
            }
        }

        //Guarda el tablero inicial para reservar el nombre desde este momento
        bool seCreoCorrectamente = guardarPartida (nombreArchivo, tablero, turnoActual, debeSeguirComiendo, filaOrigen, columnaOrigen);

        if (seCreoCorrectamente)
        {
            iniciarJuego = true; //sale del menu e inicia la partida nueva
        }
        else
        {
            Console.WriteLine("No se pudo crear el archivo de la nueva partida.");
            Console.WriteLine("Presiona una tecla para volver al menú.");
            Console.ReadKey(true);
        }
        
    }

    else if (opcionMenu == "2")//carga una partida existente
    {
        Console.Write("Qué partida quieres cargar?: ");
        string entradaNombre = Console.ReadLine()!;
        nombreArchivo = limpiarNombreArchivo(entradaNombre);


        string rutaPartida = Path.Combine(carpetaPartidas, nombreArchivo); //une carpeta y archivo en una ruta valida

        if (!File.Exists(rutaPartida)) //revisa si realmente existe el archivo que pidio el jugador
        {
            Console.WriteLine("No se encontro esa partida guardada.");
            Console.WriteLine("Presiona una tecla para volver al menú.");
            Console.ReadKey(true);
        }
        else
        {
            bool seCargoCorrectamente = CargarPartida(rutaPartida, tablero, ref turnoActual, ref debeSeguirComiendo, ref filaOrigen, ref columnaOrigen);

            //revisa si el archivo tenia datos dañados o un formato incorrecto
            if (seCargoCorrectamente)
            {
                iniciarJuego = true;//sale del menu e inicia la partida cargada
            }
            else
            {
                Console.WriteLine("La partida existe, pero sus datos no son válidos.");
                Console.WriteLine("Presiona una tecla para volver al menú.");
                Console.ReadKey(true);
            }
        }
    }
    
    else if (opcionMenu == "3")
    {
        Console.Clear();
        MostrarHistorial(carpetaPartidas);

        Console.WriteLine();
        Console.WriteLine("Presiona una tecla para volver al menú.");
        Console.ReadKey(true);

    }
    else //opcion 4
    {
        Console.WriteLine("Has salido exitosamente. ");
        return;
    }
}


while (juegoActivo) //While ya que no sabemos cuantos turnos va a durar una partida
{
    Console.Clear();

    DibujarTablero(tablero); //mostramos el tablero actual en cada turno

    String colorTurno = (turnoActual == 1) ? "Claras" : "Oscuras";
    Console.WriteLine($"Turno del jugador: {colorTurno}");

    //solo preguntamos el origen si no venimos de captura en cadena
    if (!debeSeguirComiendo)
    {
        Console.WriteLine("Qué ficha quieres mover?");


        filaOrigen = pedirNumero("Fila origen: ");


        columnaOrigen = pedirNumero("Columna origen: ");

    }
    else
    {
        //si si debe seguir comiendo le avisamos con que ficha sigue jugando
        Console.WriteLine($"Sigues comiendo con la ficha en ({filaOrigen}, {columnaOrigen})");
    }

    if (filaOrigen < 0 || filaOrigen > 7 || columnaOrigen < 0 || columnaOrigen > 7)
    {
        Console.WriteLine("Esa coordenada de origen no existe en el tablero.");
        System.Threading.Thread.Sleep(2000);
        continue; //reinicia el turno desde el principio
    }

    //identificamos si la ficha es dama, para saber si hace falta preguntar adelante / atras, o si su direccion ya es fija
    int valorFichaseleccionada = tablero[filaOrigen, columnaOrigen];

    //Validamos la ficha antes de pedir las direcciones del movimiento
    if (valorFichaseleccionada == 0)
    {
        Console.WriteLine("No hay ninguna ficha en esa casilla.");
        System.Threading.Thread.Sleep(2000);
        continue;
    }

    bool esFichaDelTurno = (turnoActual == 1 && (valorFichaseleccionada == 1 || valorFichaseleccionada == 3))
        || (turnoActual == 2 && (valorFichaseleccionada == 2 || valorFichaseleccionada == 4));

    if (!esFichaDelTurno)
    {
        Console.WriteLine("Esa ficha no es tuya, no puedes moverla en este turno.");
        System.Threading.Thread.Sleep(2000);
        continue;
    }

    bool esDamaSeleccionada = (valorFichaseleccionada == 3 || valorFichaseleccionada == 4);

    int filaDir; //guarda hacia que fila se va a mover (-1 o 1)

    if (esDamaSeleccionada)
    {
        //si es dama preguntamos si quiere ir adelante o atras
        string teclaFila = pedirTecla("Deseas mover adelante o atras? (A = Adelante, T = Atras): ", "A", "T");


        int direccionPropia = (turnoActual == 1) ? 1 : -1; //la direccion natural de este jugador (+1 claras, -1 oscuras)

        //si elige adelante usamos su direccion natural, si elige atras usamos la direccion contraria
        filaDir = (teclaFila == "A") ? direccionPropia : -direccionPropia;
    }
    else
    {
        //si es ficha normal su direccion siempre es la misma
        filaDir = (turnoActual == 1) ? 1 : -1;
    }

    //preguntamos si quiere moverse a izquierda o derecg¿ha
    string teclaColumna = pedirTecla("Hacía donde? (I = Izquierda, D = Derecha): ", "I", "D");
    int colDir = (teclaColumna == "I") ? -1 : 1; //si escribe "I" la direccion de columna es -1 (izq), cualquier otra cosa se asume que es D la dejamos en +1 (der)

    //Calculamos el destino automaticamente, segun el origen y direccion elegida

    //Revisamos si en esa diagonal especifica hay una captura disponible (salto de 2)
    int filaCaptura = filaOrigen + (filaDir * 2);
    int columnaCaptura = columnaOrigen + (colDir * 2);

    bool hayCapturaEnEstaDiagonal = false;
    if (filaCaptura >= 0 && filaCaptura <= 7 && columnaCaptura >= 0 && columnaCaptura <= 7)
    {
        int filaIntermedia = (filaOrigen + filaCaptura) / 2;
        int columnaIntermedia = (columnaOrigen + columnaCaptura) / 2;
        int valorIntermedio = tablero[filaIntermedia, columnaIntermedia];

        bool esRivalAqui = (turnoActual == 1 && (valorIntermedio == 2 || valorIntermedio == 4)) || (turnoActual == 2 && (valorIntermedio == 1 || valorIntermedio == 3));

        if (esRivalAqui && tablero[filaCaptura, columnaCaptura] == 0)
        {
            hayCapturaEnEstaDiagonal = true;
        }
    }

    int filaDestino;
    int columnaDestino;

    if (hayCapturaEnEstaDiagonal)
    {
        //si hay captura en esa diagonal la usamos
        filaDestino = filaCaptura;
        columnaDestino = columnaCaptura;
    }
    else
    {
        //si no hay captura probamos movimiento simple (1 casilla)
        filaDestino = filaOrigen + filaDir;
        columnaDestino = columnaOrigen + colDir;
    }


    bool movimientoExitoso = IntentarMover(tablero, filaOrigen, columnaOrigen, filaDestino, columnaDestino, turnoActual); // Probamos el radar con una ficha especifica

    if (movimientoExitoso)
    {
        if (!debeSeguirComiendo)
        {
            turnoActual = (turnoActual == 1) ? 2 : 1; // Nos dice si el turno actual era 1, ahora pasa a ser 2, sino pasa a ser 1
        }
        else
        {
            //si debe seguir comiendo el origen del proximo turno es el destino de este
            filaOrigen = filaDestino;
            columnaOrigen = columnaDestino;
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

        if (juegoActivo)
        {
            bool seGuardoCorrectamente = guardarPartida(nombreArchivo, tablero, turnoActual, debeSeguirComiendo, filaOrigen, columnaOrigen);

            if (seGuardoCorrectamente)
            {
                Console.WriteLine("Partida guardada exitosamente.");
            }
            else
            {
                Console.WriteLine("No se pudo guardar la partida.");
            }

            System.Threading.Thread.Sleep(2000);//esperamos un segundo antes de limpiar y mostrar el siguiente tablero
        }
    }
    if (!movimientoExitoso)
    {
        System.Threading.Thread.Sleep(2000);
    }

}



//Funciones Visuales ("interfaz")
void DibujarTablero(int[,] tablero)
{
    DibujarTableroConCursor( tablero, -1, -1, false, -1, -1, new List<(int fila, int columna)>());
}

void DibujarTableroConCursor(int[,] tablero, int cursorFila, int cursorColumna, bool fichaSeleccionada, int filaSeleccionada, int columnaSeleccionada, List<(int fila, int columna)> destinosPosibles)
{
    Console.ForegroundColor = ConsoleColor.Gray; // color neutro para encabezados
    Console.BackgroundColor = ConsoleColor.Black;// fondo normal
    Console.WriteLine("    0 1 2 3 4 5 6 7");
    Console.WriteLine("  +-----------------");

    for (int fila = 0; fila < 8; fila++)
    {
        Console.ForegroundColor = ConsoleColor.Gray; // número de fila en gris
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Write(fila + " | ");

        for (int columna = 0; columna < 8; columna++)
        {
            int valor = tablero[fila, columna];

            bool esCursor = fila == cursorFila && columna == cursorColumna;
            bool esSeleccionada = fichaSeleccionada && fila == filaSeleccionada && columna == columnaSeleccionada;

            bool esDestinoPosible = destinosPosibles.Contains((fila, columna));

            if (esSeleccionada) //La ficha elegida se resalta en verde
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
            }
            else if (esDestinoPosible) // Los destinos posibles y legales se resaltan en amarillo.
            {
                Console.BackgroundColor = ConsoleColor.DarkYellow;
            }
            else if (esCursor) // El cursor actual se resalta en azul.
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Black;
            }


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

            Console.BackgroundColor = ConsoleColor.Black;//evita que el color se pase a la casilla siguiente
        }
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.WriteLine();
    }

    Console.ResetColor();
}
//Logica del juego (Movimientos y reglas)
bool IntentarMover(int[,] tablero, int filaOrigen, int columnaOrigen, int filaDestino, int columnaDestino, int turnoActual)
{
    //validar que el origen exista en el tablero
    if (filaOrigen < 0 || filaOrigen > 7 || columnaOrigen < 0 || columnaOrigen > 7)
    {
        Console.WriteLine("Esa coordenada de origen no existe en el tablero.");
        
        return false;
    }
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
        if (FichaPuedeComer(tablero, filaDestino, columnaDestino, turnoActual))
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

bool capturaDisponible(int[,] tablero, int turnoActual)
{
    //Recorremos todas las casillas del tablero una por una
    for (int fila = 0; fila < 8; fila++)
    {
        for (int columna = 0; columna < 8; columna++)
        {
            int valor = tablero[fila, columna];

            bool esDeEsteJugador = (turnoActual == 1 && (valor == 1 || valor == 3)) || (turnoActual == 2 && (valor == 2 || valor == 4));

            if (!esDeEsteJugador)
            {
                continue;
            }

            if (FichaPuedeComer(tablero, fila, columna, turnoActual))
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
    int[] direccionesFila;
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
        int[] columnasSalto = { columna - 2, columna + 2 };// las 2 posibles columnas donde puede caer

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
List<(int fila, int columna)> ObtenerDestinosPosibles(int[,] tablero, int filaOrigen, int columnaOrigen, int turnoActual)
{
    List<(int fila, int columna)> destinos = new();//lista de distintos destino que se devolveran

    int valorFicha = tablero[filaOrigen, columnaOrigen]; //lee la ficha seleccionada

    bool esFichaDelTurno = (turnoActual == 1 && (valorFicha == 1 || valorFicha == 3)) || (turnoActual == 2 && (valorFicha == 2 || valorFicha == 4));

    if (!esFichaDelTurno) //no permite calcula movimientos para una ficha rival o casilla vacia
    {
        return destinos;
    }

    bool esDama = valorFicha == 3 || valorFicha == 4;//identifica si puede moverse en ambos sentidos

    int[] direccionesFila; //guarda las direcciones verticales permitidas

    if (esDama)
    {
        direccionesFila = new int[] { 1, -1};//las damas pueden ir adelante y atras

    }
    else
    {
        int direccionNormal = turnoActual == 1 ? 1 : -1; // Dirección de una ficha normal.
        direccionesFila = new int[] { direccionNormal };
    }

    int[] direccionesColumna = new int[] {-1, 1};//izquierda y derecha

    bool hayCapturaObligatoria = capturaDisponible(tablero, turnoActual); // Revisa si el jugador está obligado a comer.

    foreach (int direccionFila in direccionesFila) // Revisa cada dirección vertical válida.
    {
        foreach (int direccionColumna in direccionesColumna) // Revisa izquierda y derecha.
        {
            int filaDestino = filaOrigen + (direccionFila * 2); // Casilla donde caería después de una captura.
            int columnaDestino = columnaOrigen + (direccionColumna * 2);

            if (filaDestino < 0 || filaDestino > 7 || columnaDestino < 0 || columnaDestino > 7)
            {
                continue; // Salta destinos fuera del tablero.
            }

            int filaIntermedia = filaOrigen + direccionFila; // Casilla donde debería estar el rival.
            int columnaIntermedia = columnaOrigen + direccionColumna;

            int valorIntermedio = tablero[filaIntermedia, columnaIntermedia];

            bool esRival =
                (turnoActual == 1 && (valorIntermedio == 2 || valorIntermedio == 4))
                || (turnoActual == 2 && (valorIntermedio == 1 || valorIntermedio == 3));

            if (esRival && tablero[filaDestino, columnaDestino] == 0)
            {
                destinos.Add((filaDestino, columnaDestino)); // Agrega la captura válida.
            }
        }
    }

    if (hayCapturaObligatoria)//si alguien puede comer, solo se muestran capturas
    {
        return destinos;
    }

    foreach (int direccionFila in direccionesFila) // Revisa movimientos simples.
    {
        foreach (int direccionColumna in direccionesColumna)
        {
            int filaDestino = filaOrigen + direccionFila; // Destino de una casilla.
            int columnaDestino = columnaOrigen + direccionColumna;

            bool estaDentroDelTablero =
                filaDestino >= 0 && filaDestino <= 7
                && columnaDestino >= 0 && columnaDestino <= 7;

            if (estaDentroDelTablero && tablero[filaDestino, columnaDestino] == 0)
            {
                destinos.Add((filaDestino, columnaDestino)); // Agrega el movimiento simple válido.
            }
        }
    }

    return destinos; //devuelve todas las casillas iluminables

}

int contarFichas(int[,] tablero, int jugador)
{
    int contador = 0; //empieza en 0 antes de iniciar el recorrido

    for (int fila = 0; fila < 8; fila++)
    {
        for (int columna = 0; columna < 8; columna++)
        {
            int valor = tablero[fila, columna];

            //si el jugador es claras (1) contamos fichas normales y damas (1 y 3)
            //si el jugador es oscura (2) contamos fichas normales y damas (2 y 4)
            bool esDeEsteJugador = (jugador == 1 && (valor == 1 || valor == 3)) || (jugador == 2 && (valor == 2 || valor == 4));

            if (esDeEsteJugador)
            {
                contador++; //le sumamos 1 al contador
            }
        }
    }

    return contador; //regresamos el contador encontrado
}

//detectar si una ficha especifica puede hacer un movimiento simple
bool fichaPuedeMoverse(int[,] tablero, int fila, int columna, int turnoActual)
{
    //identificamos que ficha hay realmente en esa casilla
    int valorFicha = tablero[fila, columna];
    bool esDama = (valorFicha == 3 || valorFicha == 4);


    //Armamos la lista de direcciones de fila
    //si es dama revisa ambas direcciones.
    //si es ficha normal, solo su unica direccion permitida.
    int[] direccionesFila;
    if (esDama)
    {
        direccionesFila = new int[] { 1, -1 }; //Dama puede moverse hacia adelante y atras

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
        int[] columnasPosibles = { columna - 1, columna + 1 }; //las dos diagonales posibles

        //revisamos cada una de las 2 posibles columnas
        foreach (int nuevaColumna in columnasPosibles)
        {
            //validamos que esa casilla de destino exista dentro del tablero
            if (nuevaFila >= 0 && nuevaFila <= 7 && nuevaColumna >= 0 && nuevaColumna <= 7)
            {
                //si esa casilla destino esta vacia, si hay un movimiento simple disponible
                if (tablero[nuevaFila, nuevaColumna] == 0)
                {
                    return true; // encontro al menos una casilla vacia donde moverse
                }
            }
        }
    }

    return false; // si recorrimos todas las direcciones columnas posibles y ninguna sirvio, no puede moverse simple
}

//Detectar si el jugador esta acorralado
bool jugadorEstaAcorralado(int[,] tablero, int turnoActual)
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
            if (FichaPuedeComer(tablero, fila, columna, turnoActual) || fichaPuedeMoverse(tablero, fila, columna, turnoActual))

            {
                return false; // se encontro al menos una ficha con movimiento posible

            }
        }
    }

    return true; // si no hay movimiento posible el jugador esta acorralado

}

//pedimos un numero que sea valido
int pedirNumero(string mensaje)
{
    int resultado;
    bool esValido = false;

    //repetimos el bloque al menos una vez y se sigue repitiendo mientras "esValido" sea falsa
    do
    {
        Console.Write(mensaje);
        string entrada = Console.ReadLine()!;

        //TryParse intenta convertir el texto a numero, si lo logra guarda el valor en resultado y regresa true
        //si no lo logra regresa false
        esValido = int.TryParse(entrada, out resultado);

        if (!esValido)
        {
            Console.WriteLine("Eso no es un numero valido, intenta de nuevo.");

        }

    }
    while (!esValido);

    return resultado;
}

//pedimos una tecla que sea valida
string pedirTecla(string mensaje, params string[] opcionesValidas)
{
    string entrada;
    bool esValida = false;

    do
    {
        Console.Write(mensaje);
        entrada = Console.ReadLine()!.Trim().ToUpper();
        //.Trim() quita espacios de sobra que el usuario haya escrito sin querer
        //.ToUpper convierte lo que este escrito a letras mayusculas

        //Recorremos el arreglo de opciones valida buscando si entrada coincide con alguna
        esValida = false;
        foreach (string opcion in opcionesValidas)
        {
            if (entrada == opcion)
            {
                esValida = true;
            }
        }

        if (!esValida)
        {
            //Armamos un texto tipo "1, 2 o 3" para mostrar las opciones validas en el mensaje de error
            string listaOpciones = string.Join(", ", opcionesValidas);
            Console.WriteLine($"Opción no valida, escribe una de estas: {listaOpciones}");
        }

    }
    while (!esValida);

    return entrada;
}

//bloque para limpiar el nombre del archivo
string limpiarNombreArchivo(string nombreEscrito)
{
    //quitamos espacios de sobra ya sea al inicio o al final y convertimos el texto a minusculas para que sea tratado como el mismo archivo
    string nombreLimpio = nombreEscrito.Trim().ToLower();

    //reemplazamos espacios internos por guiones bajos para evitar nombres de archivos con espacios
    nombreLimpio = nombreLimpio.Replace(" ", "_");

    //si el usuario escribio ".txt" al final del nombre se lo quitamos primero
    if (nombreLimpio.EndsWith(".txt"))
    {
        nombreLimpio = nombreLimpio.Substring(0, nombreLimpio.Length - 4);
    }

    //ahora le agregamos un ".txt" una sola vez para garantizar que se guarde bien
    return nombreLimpio + ".txt";
}

//bloque de cargar partida
bool CargarPartida(string rutaPartida, int[,] tablero, ref int turnoActual, ref bool debeSeguirComiendo, ref int filaOrigen, ref int columnaOrigen)
{
    try //intenta leer y convertir los datos
    {
        string[] lineas = File.ReadAllLines(rutaPartida); //lee todas las lineas del archivo y las guarda en un arreglo

        if (lineas.Length != 11)// verifica que existan 3 lineas de datos generales y 8 filas de tablero
        {
            return false;// detenemos la carga si el formato del archivo no coincide
        }

        bool turnoEsValido = int.TryParse(lineas[0], out int turnoLeido); //convierte la primera linea en el turno guardado

        if (!turnoEsValido || (turnoLeido != 1 && turnoLeido != 2))// revisa que los turnos sean unicamente 1 o 2
        {
            return false;//el archivo no tiene el formato esperado
        }

        bool cadenaEsValida = bool.TryParse(lineas[1], out bool cadenaLeida);//convierte la segunda linea en true o false

        if (!cadenaEsValida)//revisamos que la segunda linea sea un booleano
        {
            return false;//el archivo no tiene el formato esperado
        }

        string[] coordenadas = lineas[2].Split(','); //separa la tercera linea usando la coma

        if (coordenadas.Length != 2) //revisamos que existan exactamente la fila y columna
        {
            return false;
        }

        bool filaEsValida = int.TryParse(coordenadas[0], out int filaLeida); //convierte el primer valor en la fila guardada
        bool columnaEsValida = int.TryParse(coordenadas[1], out int columnaLeida);//convierte el segundo valor en la columna guardada

        if (!filaEsValida || !columnaEsValida)
        {
            return false;//no se carga si las coordenadas no son numeros
        }

        if (filaLeida < 0 || filaLeida > 7 || columnaLeida < 0 || columnaLeida > 7) //comprobamos que las coordenadas estén dentro del tablero.
        {
            return false; //no permite cargar coordenadas fuera del tablero.
        }

        int[,] tableroLeido = new int[8, 8]; //creamos un tablero temporal para validar todo antes de cambiar el tablero real.

        for (int fila = 0; fila < 8; fila++) //recorre las ocho filas que se guardaron.
        {
            string[] valoresFila = lineas[fila + 3].Split(','); //lee una fila del archivo y separa sus valores por comas.

            if (valoresFila.Length != 8) //revisa que cada fila tenga exactamente ocho casillas.
            {
                return false; //el formato no es válido si faltan o sobran casillas.
            }

            for (int columna = 0; columna < 8; columna++) //lo mismo para las columnas
            {
                bool valorEsValido = int.TryParse(valoresFila[columna], out int valorCasilla); //convierte el texto de la casilla en número.

                if (!valorEsValido || valorCasilla < 0 || valorCasilla > 4) //revisa que sea un valor permitido del tablero.
                {
                    return false; //solo se aceptamos 0, 1, 2, 3 o 4.
                }

                tableroLeido[fila, columna] = valorCasilla; // Guarda el valor validado en el tablero temporal.
            }

        }

        turnoActual = turnoLeido; //recuperamos el turno que estaba guardado
        debeSeguirComiendo = cadenaLeida; //recupera si habia una captera en cadena
        filaOrigen = filaLeida;//se recupera la fila de la ficha que estaba jugando
        columnaOrigen = columnaLeida;//se recupera la columna tambien de la ficha que estaba jugando

        for (int fila = 0; fila < 8; fila++) // Recorre las filas del tablero temporal.
        {
            for (int columna = 0; columna < 8; columna++) // Recorre las columnas del tablero temporal.
            {
                tablero[fila, columna] = tableroLeido[fila, columna]; // Copia cada valor validado al tablero real del juego.
            }
        }

        return true; //informa que todos los datos se cargaron correctamente.
    }
    catch //captura errores como archivo inaccesible, vacío o problemas de lectura.
    {
        return false; //informa que no se pudo cargar la partida.
    }
}

bool guardarPartida(string nombreArchivo, int[,] tablero, int turnoActual, bool debeSeguirComiendo, int filaOrigen, int columnaOrigen)
{
    try //intenta crear la carpeta y escribir el archivo
    {
        string carpetaPartidas = "Partidas";// donde se guardaran todas las partidas
        Directory.CreateDirectory(carpetaPartidas);

        string rutaPartida = Path.Combine(carpetaPartidas, nombreArchivo);// unimos la carpeta con el nombre del archivo

        string[] lineas = new string[11];// creamos un arreglo para las 3 lineas generales y las 8 filas del tablero
        lineas[0] = turnoActual.ToString();//guardamos el turno actual en la primera linea
        lineas[1] = debeSeguirComiendo.ToString();//guarda si es verdadero o falso el que siga comiendo en la segunda linea
        lineas[2] = $"{filaOrigen},{columnaOrigen}";// guardamos fila y columna separadas por una coma

        for (int fila = 0; fila < 8; fila++) //recorremos las  8 filas del tablero
        {
            string[] valoresFila = new string[8]; //creamos un arreglo temporal para los ocho valores de esta fila.

            for (int columna = 0; columna < 8; columna++) //recorremos las ocho columnas de la fila actual.
            {
                valoresFila[columna] = tablero[fila, columna].ToString();//convertimos el número de la casilla a texto.
            }

            lineas[fila + 3] = string.Join(",", valoresFila); //unimos los ocho valores con comas y los guarda desde la línea 4.
        }

        File.WriteAllLines(rutaPartida, lineas); //escribimos todas las lineas en el archivo de la partida

        return true;

    }
    catch //atrapa errores que no permiten guardar la partida
    {
        return false;
    }
}


void MostrarHistorial(string carpetaPartidas) //muestra todas las partidas guardadas dentro de la carpeta
{
    string[] rutasPartidas = Directory.GetFiles(carpetaPartidas, "*.txt"); //busca todos los archivos que terminen en .txt
    if (rutasPartidas.Length == 0)//revisa si no se encontro una partida
    {
        Console.WriteLine("Todavia no hay partidas guardadas. ");
        return;
    }

    Array.Sort( rutasPartidas, (ruta1, ruta2) => File.GetLastWriteTime(ruta2) .CompareTo(File.GetLastWriteTime(ruta1)));//ordena de la mas reciente a la mas antigua

    Console.WriteLine ("--- Historial de Partidas ---");
    Console.WriteLine();

    for (int indice = 0; indice < rutasPartidas.Length; indice++) // Recorre cada archivo encontrado.
    {
        string rutaPartida = rutasPartidas[indice]; // Guarda la ruta de la partida actual.

        string nombrePartida = Path.GetFileNameWithoutExtension(rutaPartida); // Obtiene el nombre sin .txt.

        DateTime fechaGuardado = File.GetLastWriteTime(rutaPartida); // Obtiene la fecha del último guardado.

        Console.WriteLine(
            $"{indice + 1}. {nombrePartida} - {fechaGuardado:dd/MM/yyyy HH:mm}"); // Muestra número, nombre y fecha.
    }
}
string FormatearTiempo(TimeSpan tiempo) //convierte timespan a texto mm:ss
{
    if (tiempo < TimeSpan.Zero)//evita mostrar tiempos negativos
    {
        tiempo = TimeSpan.Zero;
    }

    int minutos = (int)tiempo.TotalMinutes;//obtiene los minutos completos
    int segundos = tiempo.Seconds;//obtiene los segundos restantes

    return $"{minutos:00}:{segundos:00}";
}
ConsoleKeyInfo? EsperarTeclarConTiempo(Stopwatch relojTurno, TimeSpan tiempoRestante)
{
    while (!Console.KeyAvailable)//se repite mientras el jugador no presione una tecla
    {
        if (relojTurno.Elapsed >= tiempoRestante) // Revisa si ya gastó todo su tiempo.
        {
            return null; // Informa que se acabó el tiempo.
        }

        System.Threading.Thread.Sleep(50);
    }
    return Console.ReadKey(true); 
}