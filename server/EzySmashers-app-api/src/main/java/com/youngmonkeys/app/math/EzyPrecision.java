package com.youngmonkeys.app.math;

public final class EzyPrecision {
	public static final double EPSILON = Double.longBitsToDouble(4368491638549381120L);
	public static final double SAFE_MIN = Double.longBitsToDouble(4503599627370496L);
	
	private static final long POSITIVE_ZERO_DOUBLE_BITS = Double.doubleToRawLongBits(0.0D);
	private static final long NEGATIVE_ZERO_DOUBLE_BITS = Double.doubleToRawLongBits(-0.0D);
	private static final int POSITIVE_ZERO_FLOAT_BITS = Float.floatToRawIntBits(0.0F);
	private static final int NEGATIVE_ZERO_FLOAT_BITS = Float.floatToRawIntBits(-0.0F);
	
	public static void main(String[] args) {
		System.out.println(POSITIVE_ZERO_FLOAT_BITS);
		System.out.println("" + NEGATIVE_ZERO_FLOAT_BITS);
	}
	
	public static boolean equals(float x, float y, float eps) {
		return equals(x, y, 1) || Math.abs(y - x) <= eps;
	}
	
	public static boolean equals(float x, float y, int maxUlps) {
		int xInt = Float.floatToRawIntBits(x);
		int yInt = Float.floatToRawIntBits(y);
		boolean isEqual;
		if (((xInt ^ yInt) & -2147483648) == 0) {
			isEqual = Math.abs(xInt - yInt) <= maxUlps;
		} else {
			int deltaPlus;
			int deltaMinus;
			if (xInt < yInt) {
				deltaPlus = yInt - POSITIVE_ZERO_FLOAT_BITS;
				deltaMinus = xInt - NEGATIVE_ZERO_FLOAT_BITS;
			} else {
				deltaPlus = xInt - POSITIVE_ZERO_FLOAT_BITS;
				deltaMinus = yInt - NEGATIVE_ZERO_FLOAT_BITS;
			}
			
			if (deltaPlus > maxUlps) {
				isEqual = false;
			} else {
				isEqual = deltaMinus <= maxUlps - deltaPlus;
			}
		}
		
		return isEqual && !Float.isNaN(x) && !Float.isNaN(y);
	}
	
	public static boolean equals(double x, double y, double eps) {
		return equals(x, y, 1) || Math.abs(y - x) <= eps;
	}
	
	public static boolean equals(double x, double y, int maxUlps) {
		long xInt = Double.doubleToRawLongBits(x);
		long yInt = Double.doubleToRawLongBits(y);
		boolean isEqual;
		if (((xInt ^ yInt) & -9223372036854775808L) == 0L) {
			isEqual = Math.abs(xInt - yInt) <= (long)maxUlps;
		} else {
			long deltaPlus;
			long deltaMinus;
			if (xInt < yInt) {
				deltaPlus = yInt - POSITIVE_ZERO_DOUBLE_BITS;
				deltaMinus = xInt - NEGATIVE_ZERO_DOUBLE_BITS;
			} else {
				deltaPlus = xInt - POSITIVE_ZERO_DOUBLE_BITS;
				deltaMinus = yInt - NEGATIVE_ZERO_DOUBLE_BITS;
			}
			
			if (deltaPlus > (long)maxUlps) {
				isEqual = false;
			} else {
				isEqual = deltaMinus <= (long)maxUlps - deltaPlus;
			}
		}
		
		return isEqual && !Double.isNaN(x) && !Double.isNaN(y);
	}
}
