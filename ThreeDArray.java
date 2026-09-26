public class ThreeDArray {
    public static void main(String[] args){
        int [][][] ThreeDimensionalArray = {
            {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9}
            },
            {
                {10, 11, 12},
                {13, 14, 15},
                {16, 17, 18}
            }
        };

        System.out.println("Los elementos del array son: ");
        for (int[][] TwoDimensionalArray : ThreeDimensionalArray) {
            for (int[] row : TwoDimensionalArray) {
                for (int element : row) {
                    System.out.print(element + " "); // mostrar los elementos de la fila separados por espacios
                }
            }
            System.out.println(); // ir a la siguiente línea después de mostrar un bloque 2D
        }
    }
}
