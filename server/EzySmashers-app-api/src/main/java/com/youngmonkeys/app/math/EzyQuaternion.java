package com.youngmonkeys.app.math;


import lombok.Getter;

import java.util.Queue;

/**
 * This class implements Quaternion (Hamilton's hyper-complex numbers).
 */
public final class EzyQuaternion {
	
	// To determine if two Quaternions are close enough to be considered equal.
	public static final double ALLOWED_DEVIANCE = 0.00000001;
	
	@Getter
	private double x;
	@Getter
	private double y;
	@Getter
	private double z;
	@Getter
	private double w;
	
	public EzyQuaternion(double x, double y, double z, double w) {
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
	}
	
	public static EzyQuaternion fromRotationMatrix(final double m00, final double m01, final double m02, final double m10,
	                                               final double m11, final double m12, final double m20, final double m21, final double m22) {
		// Trace = sum of the diagonal elements;
		final double t = m00 + m11 + m22;
		
		// Protect the division by s by ensuring that s>=1
		double x, y, z, w;
		if (t >= 0) { // |w| >= .5
			double s = Math.sqrt(t + 1); // |s|>=1 ...
			w = 0.5 * s;
			s = 0.5 / s; // so this division isn't bad
			x = (m21 - m12) * s;
			y = (m02 - m20) * s;
			z = (m10 - m01) * s;
		} else if (m00 > m11 && m00 > m22) {
			double s = Math.sqrt(1.0 + m00 - m11 - m22); // |s|>=1
			x = s * 0.5; // |x| >= .5
			s = 0.5 / s;
			y = (m10 + m01) * s;
			z = (m02 + m20) * s;
			w = (m21 - m12) * s;
		} else if (m11 > m22) {
			double s = Math.sqrt(1.0 + m11 - m00 - m22); // |s|>=1
			y = s * 0.5; // |y| >= .5
			s = 0.5 / s;
			x = (m10 + m01) * s;
			z = (m21 + m12) * s;
			w = (m02 - m20) * s;
		} else {
			double s = Math.sqrt(1.0 + m22 - m00 - m11); // |s|>=1
			z = s * 0.5; // |z| >= .5
			s = 0.5 / s;
			x = (m02 + m20) * s;
			y = (m21 + m12) * s;
			w = (m10 - m01) * s;
		}
		
		return new EzyQuaternion(x, y, z, w);
	}
	
	public EzyQuaternion toNormalization() {
		final double n = 1.0 / magnitude();
		final double x = getX() * n;
		final double y = getY() * n;
		final double z = getZ() * n;
		final double w = getW() * n;
		return new EzyQuaternion(x, y, z, w);
	}
	
	/**
	 * @return magnitude of this quaternion
	 */
	public double magnitude() {
		final double magnitudeSQ = magnitudeSquared();
		if (magnitudeSQ == 1.0) {
			return 1.0;
		}
		
		return Math.sqrt(magnitudeSQ);
	}
	
	/**
	 * @return squared magnitude of this quaternion
	 */
	public double magnitudeSquared() {
		return getW() * getW() + getX() * getX() + getY() * getY() + getZ() * getZ();
	}
}

