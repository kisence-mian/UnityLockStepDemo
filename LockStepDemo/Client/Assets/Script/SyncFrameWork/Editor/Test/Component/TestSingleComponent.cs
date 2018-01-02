using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSingleComponent : MomentSingletonComponent
{
    public int testValue = 0;

    public override MomentSingletonComponent DeepCopy()
    {
        TestSingleComponent tc = HeapObjectPool<TestSingleComponent>.GetObject();// new TestSingleComponent();
        tc.testValue = testValue;

        return tc;
    }
}
