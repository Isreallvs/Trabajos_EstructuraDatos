const ThreeDimensionalArray = [
    [   [1, 2, 3],
        [4, 5, 6],
        [7, 8, 9]
    ],
    [ [10, 11, 12],
      [13, 14, 15],
      [16, 17, 18]
    ],    
];

console.log("Los elementos del array son: ");
for (const block of ThreeDimensionalArray) {
    for (const row of block) {
        let line = " ";
    for (const element of row) {
        line += element + " ";
    }
    console.log(line.trim());
    }
    
    
}