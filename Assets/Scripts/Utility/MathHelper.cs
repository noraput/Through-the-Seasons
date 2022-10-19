public static class MathHelper {
    public static bool IsBetween (this float num, float lower, float upper, bool inclusive = false) {
        return inclusive
            ? lower <= num && num <= upper
            : lower < num && num < upper;
    }
}