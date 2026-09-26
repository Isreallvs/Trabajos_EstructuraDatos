public class MatrixTranspose {
    
    static String formatRow(int[] fila) {
        StringBuilder sb = new StringBuilder("[");
        for (int i = 0; i < fila.length; i++) {
            sb.append(fila[i]);
            if (i < fila.length - 1)
                sb.append(", ");
        }
        sb.append("]");
        return sb.toString();
    }

    public static void main(String[] args) {
        int r = 3;
        int c = 3;
        int[][] arr = new int[r][c]; // creamos una matriz vacia

        int[][] TwoDArr = {
            {1, 2, 3},
            {4, 5, 6},
            {7, 8, 9}
        };

        for (int x = 0; x < r * c; x++) {
            int fila = x % r;
            int columna = x / r;
            arr[fila][columna] = TwoDArr[x / c][x % c];
        }

        System.out.println("Los elementos del array bidimensional son:");
        for (int[] fila : TwoDArr)
            System.out.println(formatRow(fila));

        System.out.println("\nLa matriz ordenada por columnas es:");
        for (int[] fila : arr)
            System.out.println(formatRow(fila));
    }
}
