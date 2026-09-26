//funcion para intercambiar dos elementos en el arreglo
function swap(a, j, k) // intercambia los elementos
{ 
    [a[j], a[k]] = [a[k], a[j]]; // intercambia los elementos
}

// Función para hacer la partición del arreglo
function partition(a, l, h) 
{
    // Selecciona el elemento pivote
    const pvt = a[h];
    // j es el índice de los elementos que son menores que
    // pivot y también indica la posición correcta del pivot encontrado hasta este momento
    let j = l - 1;
    // Recorre a[l..h-1] y mueve todos los elementos menores
    // al lado izquierdo del pivote.
    // los elementos de l a j son más pequeños después de cada iteración
    for (let k = l; k < h; k++) // recorre el arreglo
    { 
        // Si el elemento actual es menor que el pivote
        if (a[k] < pvt) // compara el elemento actual con el pivote
        { 
            j++; // incrementa el indice del elemento más pequeño
            swap(a, j, k); // intercambia los elementos
        }
    }
    // Mover el pivote después de elementos más pequeños y devolverlo a su posición
    swap(a, j + 1, h); // intercambia el pivote con el elemento siguiente al último elemento más pequeño
    return j + 1; // devuelve el indice del pivote
}

// Implementación de la función QuickSort
function qckSort(a, l, h) // función principal de QuickSort
{ 
    if (l < h) // si el indice izquierdo es menor que el derecho
    { 
        // pi es el índice de partición, regresa el índice del pivote
        const pi = partition(a, l, h); // particiona el arreglo
        // llamadas recursivas para los elementos menores
        // y mayores o iguales a los elementos
        qckSort(a, l, pi - 1); // llamada recursiva para los elementos menores que el pivote
        qckSort(a, pi + 1, h); // llamada recursiva para los elementos mayores o iguales que el pivote
    }
}

//codigo para probar la implementacion de quicksort
const a = [10, 7, 8, 9, 1, 5];
const size = a.length;

console.log("El arreglo antes de ordenarlo: ");
console.log(a.join(" "));

qckSort(a, 0, size - 1);

console.log("El arreglo despues de ordenarlo: ");
console.log(a.join(" "));