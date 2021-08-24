package com.youngmonkeys.app.math;

import com.tvd12.gamebox.math.Vec3;

public class NewVec3 extends Vec3 {
	public static final double EPSILON = Double.longBitsToDouble(4368491638549381120L);
	
	public NewVec3() {
	}
	
	public NewVec3(Vec3 v) {
		super(v);
	}
	
	public NewVec3(float[] array) {
		super(array);
	}
	
	public NewVec3(float x, float y, float z) {
		super(x, y, z);
	}
	
	public static NewVec3 crossProduct(Vec3 u, Vec3 v) {
		float newX = u.getY() * v.getZ() - u.getZ() * v.getY();
		float newY = u.getZ() * v.getX() - u.getX() * v.getZ();
		float newZ = u.getX() * v.getY() - u.getY() * v.getX();
		return new NewVec3(newX, newY, newZ);
	}
	
	public NewVec3 crossProduct(Vec3 v) {
		return crossProduct(this, v);
	}
	
	public double lengthSquared() {
		return (this.x * this.x + this.y * this.y + this.z * this.z);
	}
	
	public static NewVec3 multiply(Vec3 vec, double value) {
		return new NewVec3(
				(float) (vec.x * value),
				(float) (vec.y * value),
				(float) (vec.z * value)
		);
	}
	
	public NewVec3 normalize() {
		final double lengthSq = lengthSquared();
		if (Math.abs(lengthSq) > EPSILON) {
			return multiply(this, 1.0 / Math.sqrt(lengthSq));
		}
		
		return new NewVec3(0f, 0f, 0f);
	}
	
}
