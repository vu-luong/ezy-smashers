package com.youngmonkeys.app.math;


import lombok.Getter;

import java.util.function.BiPredicate;
import java.util.function.ToDoubleFunction;

/**
 * This class implements Quaternion (Hamilton's hyper-complex numbers).
 */
public final class EzyQuaternion {
	
	// The number of dimensions for the vector part of the quaternion.
	private static final int VECTOR_DIMS = 3;
	private static final String EXCEPTION_NORM_PREFIX = "Invalid norm: ";
	
	// For enabling specialized method
	private final Type type;
	
	// First component (scalar part)
	@Getter
	private final double w;
	// Second component (first vector part)
	@Getter
	private final double x;
	// Third component (second vector part).
	@Getter
	private final double y;
	// Fourth component (third vector part).
	@Getter
	private final double z;
	
	private enum Type {
		DEFAULT(Default.NORMSQ,
				Default.NORM,
				Default.IS_UNIT),
		NORMALIZED(Normalized.NORM,
				Normalized.NORM,
				Normalized.IS_UNIT),
		POSITIVE_POLAR_FORM(Normalized.NORM,
				Normalized.NORM,
				Normalized.IS_UNIT);
		
		private final ToDoubleFunction<EzyQuaternion> normSq;
		private final ToDoubleFunction<EzyQuaternion> norm;
		private final BiPredicate<EzyQuaternion, Double> testIsUnit;
		
		private static final class Default {
			static final ToDoubleFunction<EzyQuaternion> NORMSQ = q ->
					q.w * q.w + q.x * q.x + q.y * q.y + q.z * q.z;
			
			private static final ToDoubleFunction<EzyQuaternion> NORM = q ->
					Math.sqrt(NORMSQ.applyAsDouble(q));
			
			private static final BiPredicate<EzyQuaternion, Double> IS_UNIT = (q, eps) ->
					EzyPrecision.equals(NORM.applyAsDouble(q), 1d, eps);
			
		}
		
		private static final class Normalized {
			static final ToDoubleFunction<EzyQuaternion> NORM = q -> 1;
			static final BiPredicate<EzyQuaternion, Double> IS_UNIT = (q, eps) -> true;
		}
		
		Type(ToDoubleFunction<EzyQuaternion> normSq,
		     ToDoubleFunction<EzyQuaternion> norm,
		     BiPredicate<EzyQuaternion, Double> isUnit) {
			this.normSq = normSq;
			this.norm = norm;
			this.testIsUnit = isUnit;
		}
		
		double normSq(EzyQuaternion q) {
			return normSq.applyAsDouble(q);
		}
		
		double norm(EzyQuaternion q) {
			return norm.applyAsDouble(q);
		}
		
		boolean isUnit(EzyQuaternion q, double eps) {
			return testIsUnit.test(q, eps);
		}
	}
	
	private EzyQuaternion(Type type,
	                      final double w,
	                      final double x,
	                      final double y,
	                      final double z) {
		this.type = type;
		this.w = w;
		this.x = x;
		this.y = y;
		this.z = z;
	}
	
	private EzyQuaternion(Type type, EzyQuaternion q) {
		this.type = type;
		w = q.w;
		x = q.x;
		y = q.y;
		z = q.z;
	}
	
	public static EzyQuaternion of(final double w,
	                               final double x,
	                               final double y,
	                               final double z) {
		return new EzyQuaternion(Type.DEFAULT, w, x, y, z);
	}
	
	public static EzyQuaternion of(final double scalar,
	                               final double[] v) {
		if (v.length != VECTOR_DIMS) {
			throw new IllegalArgumentException("Size of array must equal 3");
		}
		
		return of(scalar, v[0], v[1], v[2]);
	}
	
	public static EzyQuaternion of(final double[] v) {
		return of(0, v);
	}
	
	/**
	 * Returns the polar form of the quaternion.
	 *
	 * @return the unit quaternion with positive scalar part.
	 */
	public EzyQuaternion positivePolarForm() {
		switch (type) {
			case POSITIVE_POLAR_FORM:
				return this;
			case NORMALIZED:
				return w >= 0 ?
						new EzyQuaternion(Type.POSITIVE_POLAR_FORM, this) :
						new EzyQuaternion(Type.POSITIVE_POLAR_FORM, negate());
			case DEFAULT:
				return w >= 0 ?
						normalize() :
						negate().normalize();
			default:
				throw new IllegalStateException();
		}
	}
	
	public EzyQuaternion negate() {
		switch (type) {
			case POSITIVE_POLAR_FORM:
			case NORMALIZED:
				return new EzyQuaternion(Type.NORMALIZED, -w, -x, -y, -z);
			case DEFAULT:
				return new EzyQuaternion(Type.DEFAULT, -w, -x, -y, -z);
			default:
				throw new IllegalStateException();
		}
	}
	
	public EzyQuaternion normalize() {
		switch (type) {
			case NORMALIZED:
			case POSITIVE_POLAR_FORM:
				return this;
			case DEFAULT:
				final double norm = norm();
				
				if (norm < EzyPrecision.SAFE_MIN ||
						!Double.isFinite(norm)) {
					throw new IllegalStateException(EXCEPTION_NORM_PREFIX + norm);
				}
				
				final EzyQuaternion unit = divide(norm);
				return w >= 0 ?
						new EzyQuaternion(Type.POSITIVE_POLAR_FORM, unit) :
						new EzyQuaternion(Type.NORMALIZED, unit);
			default:
				throw new IllegalStateException();
		}
	}
	
	public EzyQuaternion divide(final double alpha) {
		return of(
				w / alpha,
				x / alpha,
				y / alpha,
				z / alpha
		);
	}
	
	public double norm() {
		return type.norm(this);
	}
	
	public static double dot(final EzyQuaternion q1,
	                         final EzyQuaternion q2) {
		return q1.w * q2.w +
				q1.x * q2.x +
				q1.y * q2.y +
				q1.z * q2.z;
	}
	
	public double dot(final EzyQuaternion q) {
		return dot(this, q);
	}
	
	public String toString() {
		return "[" + this.w + " " + this.x + " " + this.y + " " + this.z + "]";
	}
	
	public static EzyQuaternion fromRotationMatrix(final double m00, final double m01, final double m02, final double m10,
	                                        final double m11, final double m12, final double m20, final double m21, final double m22) {
		// the trace is the sum of the diagonal elements; see
		// http://mathworld.wolfram.com/MatrixTrace.html
		final double t = m00 + m11 + m22;
		
		// we protect the division by s by ensuring that s>=1
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
		
		return of(w, x, y, z).positivePolarForm();
	}
	
	public static EzyQuaternion fromAxes(final NewVec3 xAxis, final NewVec3 yAxis, final NewVec3 zAxis) {
		return fromRotationMatrix(xAxis.getX(), yAxis.getX(), zAxis.getX(), xAxis.getY(), yAxis.getY(), zAxis.getY(),
				xAxis.getZ(), yAxis.getZ(), zAxis.getZ());
	}
	
	public static EzyQuaternion lookAt(final NewVec3 direction, final NewVec3 up) {
		NewVec3 zAxis = direction.normalize();
		NewVec3 xAxis = up.normalize().crossProduct(zAxis);
//		NewVec3 xAxis = zAxis.crossProduct(up.normalize());
		NewVec3 yAxis = zAxis.crossProduct(xAxis);
		return fromAxes(xAxis, yAxis, zAxis).positivePolarForm();
	}
	
	public static void main(String[] args) {
		EzyQuaternion start = EzyQuaternion.of(0, 0, 0, -1);
//		EzyQuaternion end = EzyQuaternion.of(new double[]{-1, 0, 0}).dot(EzyQuaternion.of(new double[]{0, 1, 0}));
//		System.out.println(start.toString());
//		System.out.println(end.toString());
//		Slerp slerp = new Slerp(start, end);
//		System.out.println(slerp.apply(0.3).toString());
//
//		System.out.println("Hello World!");
		
//		double val = EzyQuaternion.of(new double[]{-1, 0, -1}).dot(EzyQuaternion.of(new double[]{0, 1, 0}));
//		EzyQuaternion eq1 = EzyQuaternion.of(new double[]{-1, 0, -1});
//		EzyQuaternion eq2 = EzyQuaternion.of(new double[]{0, 1, 0});
		NewVec3 v1 = new NewVec3(80, 30, 10);
		NewVec3 v2 = new NewVec3(0, 1, 0);
		EzyQuaternion res = EzyQuaternion.lookAt(v1, v2);
		System.out.println("LookAt = " + res);
	}
}

