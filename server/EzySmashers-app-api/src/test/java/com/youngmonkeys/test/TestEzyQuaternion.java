package com.youngmonkeys.test;

import com.youngmonkeys.app.math.EzyQuaternion;
import com.youngmonkeys.app.math.NewVec3;
import org.testng.annotations.Test;

public class TestEzyQuaternion {
	
	@Test
	public void testLookAt() {
		final NewVec3 direction = new NewVec3(1f, 0f, 1f);
		final EzyQuaternion quat = EzyQuaternion.lookAt(direction,
				new NewVec3(0, 1, 0));
		System.out.println("quat = " + quat);
	}
	
	@Test
	public void testSlerp() {
		final EzyQuaternion start1 = new EzyQuaternion(0.0000, 0.0000, 0.0000, 1.0000);
		final EzyQuaternion end1 = new EzyQuaternion(0.0000, 1.0000, 0.0000, 0.0000);
		
		System.out.println(EzyQuaternion.slerp(start1, end1, 0.5f));
		
		final EzyQuaternion start2 = new EzyQuaternion(0.0000, 0.9808, 0.0000, 0.1951);
		final EzyQuaternion end2 = new EzyQuaternion(0.0000, 0.9239, 0.0000, -0.3827);
		
		System.out.println(EzyQuaternion.slerp(start2, end2, 0.5f));
	}
}
