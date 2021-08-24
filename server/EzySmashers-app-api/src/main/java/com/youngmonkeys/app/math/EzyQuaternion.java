package com.youngmonkeys.app.math;


import lombok.Getter;

import java.awt.geom.QuadCurve2D;

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
	
	public EzyQuaternion(EzyQuaternion source) {
		this(source.getX(), source.getY(), source.getZ(), source.getW());
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
	
	/**
	 * @return a normalized (unit-length) quaternion
	 */
	public EzyQuaternion toNormalization() {
		final double n = 1.0 / magnitude();
		final double x = getX() * n;
		final double y = getY() * n;
		final double z = getZ() * n;
		final double w = getW() * n;
		return new EzyQuaternion(x, y, z, w);
	}
	
	public static EzyQuaternion fromAxes(NewVec3 xAxis, NewVec3 yAxis, NewVec3 zAxis) {
		return fromRotationMatrix(xAxis.getX(), yAxis.getX(), zAxis.getX(), xAxis.getY(), yAxis.getY(), zAxis.getY(),
				xAxis.getZ(), yAxis.getZ(), zAxis.getZ());
	}
	
	/**
	 * Creates a rotation with the specified forward and
	 * upwards directions.
	 *
	 * @param forward The direction to look in.
	 * @param upwards The vector that defines in which direction up is.
	 * @return
	 */
	public static EzyQuaternion lookAt(NewVec3 forward, NewVec3 upwards) {
		NewVec3 zAxis = forward.normalize();
		NewVec3 xAxis = upwards.normalize().crossProduct(zAxis);
		NewVec3 yAxis = zAxis.crossProduct(xAxis);
		
		return fromAxes(xAxis, yAxis, zAxis)
				.toNormalization();
	}
	
	public static EzyQuaternion slerp(EzyQuaternion start, EzyQuaternion end,
	                                  double changeAmount) {
		// check for weighting at either extreme
		if (changeAmount == 0.0) {
			return new EzyQuaternion(start);
		} else if (changeAmount == 1.0) {
			return new EzyQuaternion(end);
		}
		EzyQuaternion result = new EzyQuaternion(end);
		
		// Check equality to skip operation.
		if (start.equals(result)) {
			return result;
		}
		
		double dotP = start.dot(result);
		
		if (dotP < 0.0) {
			result = result.negate();
			dotP = -dotP;
		}
		
		double scale0 = 1 - changeAmount;
		double scale1 = changeAmount;
		
		if (1 - dotP > 0.1) {
			final double theta = Math.acos(dotP);
			final double invSinTheta = 1f / Math.sin(theta);
			
			scale0 = Math.sin((1 - changeAmount) * theta) * invSinTheta;
			scale1 = Math.sin(changeAmount * theta) * invSinTheta;
		}
		
		final double x = scale0 * start.getX() + scale1 * result.getX();
		final double y = scale0 * start.getY() + scale1 * result.getY();
		final double z = scale0 * start.getZ() + scale1 * result.getZ();
		final double w = scale0 * start.getW() + scale1 * result.getW();
		
		return new EzyQuaternion(x, y, z, w);
	}
	
	public EzyQuaternion negate() {
		return new EzyQuaternion(
				-getX(),
				-getY(),
				-getZ(),
				-getW()
		);
	}
	
	public EzyQuaternion multiply(double scalar) {
		return new EzyQuaternion(
				getX() * scalar,
				getY() * scalar,
				getZ() * scalar,
				getW() * scalar
		);
	}
	
	/**
	 * @return dot product
	 */
	public double dot(final double x, final double y, final double z, final double w) {
		return getX() * x + getY() * y + getZ() * z + getW() * w;
	}
	
	/**
	 * @return dot product
	 */
	public double dot(EzyQuaternion quaternion) {
		return dot(quaternion.getX(), quaternion.getY(), quaternion.getZ(), quaternion.getW());
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
	
	public String toString() {
		return "EzyQuaternion [X=" + getX() + ", Y=" + getY() + ", Z=" + getZ() + ", W=" + getW() + "]";
	}
}

