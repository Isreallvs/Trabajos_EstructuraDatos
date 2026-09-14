const TwoDimensionalArray = [
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
];

console.log("Los elementos del array son: ");
for (const row of TwoDimensionalArray) {
    let line = " ";
    for (const element of row) {
        line += element + " ";
    }
    console.log(line.trim());
}