package com.youngmonkeys.app.math;

import java.awt.geom.QuadCurve2D;
import java.util.function.DoubleFunction;

/**
 * Perform spherical linear interpolation
 */
public class Slerp implements DoubleFunction<EzyQuaternion> {
	/**
	 * Threshold max value for the dot product.
	 * If the quaternion dot product is greater than this value (i.e. the
	 * quaternions are very close to each other), then the quaternions are
	 * linearly interpolated instead of spherically interpolated.
	 */
	private static final double MAX_DOT_THRESHOLD = 0.9995;
	// Start of the interpolation
	private final EzyQuaternion start;
	
	// End of the interpolation
	private final EzyQuaternion end;
	
	// Linear or spherical interpolation algorithm
	private final DoubleFunction<EzyQuaternion> algo;
	
	public Slerp(EzyQuaternion start,
	             EzyQuaternion end) {
		this.start = start.positivePolarForm();
		final EzyQuaternion e = end.positivePolarForm();
		double dot = this.start.dot(e);
		
		// If the dot product is negative, then the interpolation won't follow the shortest
		// angular path between the two quaternions. In this case, invert the end quaternion
		// to produce an equivalent rotation that will give us the path we want.
		if (dot < 0) {
			dot = -dot;
			this.end = e.negate();
		} else {
			this.end = e;
		}
		
		algo = dot > MAX_DOT_THRESHOLD ?
				new Linear() :
				new Spherical(dot);
	}
	
	@Override
	public EzyQuaternion apply(double t) {
		if (t == 0) {
			return start;
		} else if (t == 1) {
			return end.positivePolarForm();
		}
		
		return algo.apply(t);
	}
	
	private class Linear implements DoubleFunction<EzyQuaternion> {
		@Override
		public EzyQuaternion apply(double t) {
			final double f = 1 - t;
			return EzyQuaternion.of(
					f * start.getW() + t * end.getW(),
					f * start.getX() + t * end.getX(),
					f * start.getY() + t * end.getY(),
					f * start.getZ() + t * end.getZ()
			).positivePolarForm();
		}
	}
	
	private class Spherical implements DoubleFunction<EzyQuaternion> {
		private final double theta;
		private final double sinTheta;
		
		Spherical(double dot) {
			theta = Math.acos(dot);
			sinTheta = Math.sin(theta);
		}
		
		@Override
		public EzyQuaternion apply(double t) {
			final double f1 = Math.sin((1 - t) * theta) / sinTheta;
			final double f2 = Math.sin(t * theta) / sinTheta;
			
			return EzyQuaternion.of(
					f1 * start.getW() + f2 * end.getW(),
					f1 * start.getX() + f2 * end.getX(),
					f1 * start.getY() + f2 * end.getY(),
					f1 * start.getZ() + f2 * end.getZ()
			).positivePolarForm();
		}
	}
}
