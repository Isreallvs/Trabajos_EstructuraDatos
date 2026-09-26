public class TwoDArray {
    public static void main (String[] args){
        int[][] TwoDimensionalArray = {
            {1, 2, 3},
            {4, 5, 6},
            {7, 8, 9}
        };

        System.out.println("Los elementos del array son: ");
        for (int[] row : TwoDimensionalArray){
            for (int element : row){
                System.out.print(element + " ");

            }
            System.out.println();
        }
    }
}
