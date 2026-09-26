function formatRow(fila){
    return "[" + fila.join(",") + "]";

}

const r = 3;
const c = 3;
const arr = Array.from({length: r}, () => new Array(c).fill(0));

const TwoDArr = [
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
];

for (let x = 0; x < r * c; x++) {
    const fila = x % r;
    const columna = Math.floor(x / r);
    arr[fila][columna] = TwoDArr[Math.floor(x / c)][x % c];
}

console.log("Los elementos del array bidimensional son:");
for (const fila of TwoDArr)
    console.log(formatRow(fila));

console.log("\nLa matriz ordenada por columnas es:");
for (const fila of arr)
    console.log(formatRow(fila));