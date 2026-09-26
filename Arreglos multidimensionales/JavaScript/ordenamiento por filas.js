const r = 3, c = 3;
const arr = new Array(r * c).fill(0);

// Matriz inicializada y luego se le asigna un valor
const TwoDArr = [
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
]; // almacenar elementos en un array unidimensional ordenados por filas

let k = 0;
for (let x = 0; x < r; x++) {
    for (let y = 0; y < c; y++) {
        k = x * r + y;
        arr[k] = TwoDArr[x][y];
        k = k + 1;
    }
}
console.log("Los elementos del array bidimensional son: ");
for (let x = 0; x < r; x++) {
    let line = "";
    for (let y = 0; y < c; y++) {
        line += TwoDArr[x][y] + " "; // mostrar los elementos de la fila separados por espacios
    }
    console.log(line); // ir a la siguiente línea después de mostrar una fila
}

console.log("\nLos elementos del array unidimensional son: ");
// imprimir los elementos del array unidimensional
let line = "";
for (let x = 0; x < r; x++) {
    for (let y = 0; y < c; y++) {
        line += arr[x * r + y] + " "; // mostrar los elementos de la fila separados por espacios
    }
}
console.log(line);