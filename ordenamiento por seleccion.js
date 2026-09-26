function selection(a) //funcion para implementar el algoritmo de seleccion
{
    for (let i = 0; i < a.length; i++) // recorre todo el arreglo
    { 
        let small = i; // índice del elemento más pequeño
        for (let j = i + 1; j < a.length; j++) // encuentra el elemento más pequeño en el arreglo
        { 
            if (a[small] > a[j]) // compara el elemento más pequeño con el siguiente elemento
            { 
                small = j; // actualiza el índice del elemento más pequeño
            }
            // intercambia el elemento más pequeño con el primer elemento
            [a[i], a[small]] = [a[small], a[i]]; // intercambia los elementos
        }
    }
}
function printArr(a) // función para imprimir el array
{ 
    console.log(a.join(" "));
}

const a = [65, 26, 13, 23, 12];

console.log("Arreglo antes de ser ordenado: ");
printArr(a);

selection(a);

console.log("Arreglo despues de ser ordenado: ");
selection(a);
printArr(a);