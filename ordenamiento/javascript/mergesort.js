//merge dos subarrays de a[]
function merge (a, l, m, r)
{
    const a1 = m - l + 1;
    const a2 = r - m;

    //crear arrays temporales
    const L = new Array(a1).fill(0);
    const R = new Array(a2).fill(0);

    // copiar datos a los arrays temporales L[] y R[]
    for (let j = 0; j < a1; j++)
        L[j] = a[l + j];
    for (let k = 0; k < a2; k++) // copiar datos al array temporal
        R[k] = a[m + 1 + k];

    let i = 0; // indice inicial del primer subarray
    let j = 0; // indice inicial del segundo subarray
    let k = l; // indice inicial del subarray mezclado

    // mezclar los arrays temporales de nuevo en a[l..r]
    while (i < a1 && j < a2) { // recorrer ambos arrays
        if (L[i] <= R[j]) { // comparar los elementos de ambos arrays
            a[k] = L[i]; // copiar el elemento mas pequeño al array original
            i = i + 1; // incrementar el indice del primer array
        } else { // si el elemento del segundo array es mas pequeño
            a[k] = R[j]; // copiar el elemento mas pequeño del array original
            j = j + 1; // incrementar el indice del segundo array
        }
        k = k + 1; // incrementar el indice del array original
    }
    // copiar los elementos restantes de L[], si hay alguno
    while (i < a1) { // copiar los elementos restantes del primer array
        a[k] = L[i]; // copiar el elemento al array original
        i = i + 1; // incrementar el indice del primer array
        k = k + 1; // incrementar el indice del array original
    }
}

// l es para el indice izquierdo y r es para el indice derecho del subarray de 'a' a ser ordenado
function mergeSort(a, l, r) // funcion principal que ordena a[l..r]
{ 
    if (l < r) {
        // igual que (l + r)/2, pero evita el desbordamiento para grandes valores de l y h
        const m = l + Math.floor((r - l) / 2);
        // ordenar la primera y segunda mitad
        mergeSort(a, l, m); // ordenar la primera mitad
        mergeSort(a, m + 1, r); // ordenar la segunda mitad
        merge(a, l, m, r); // mezclar las dos mitades
    }
}

//codigo para probar la implementacion de mergesort
const a = [39, 28, 44, 11];
const s = a.length;

console.log("Antes de ordenar el arreglo: ");
console.log(a.join(" "));

mergeSort(a, 0, s - 1);

console.log("Despues de ordenar el arreglo: ");
console.log(a.join(" "));