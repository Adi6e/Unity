using System;

public static class Utils {
    public static void Shuffle<T> (T[] array) {
        var rng = new Random();
        int n = array.Length;
        while (n > 1) {
            int k = rng.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}
