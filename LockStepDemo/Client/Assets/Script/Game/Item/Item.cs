using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : PoolObject
{
    public int m_itemId = 0;
    public string m_itemName = "";
    public string m_effectID = "";

	// Update is called once per frame
	void Update () {
		
	}

    public override void OnCreate()
    {
        base.OnCreate();
    }

    public override void OnFetch()
    {
        base.OnFetch();
    }

    public override void OnRecycle()
    {
        base.OnRecycle();
    }

    public void OnPickUp()
    {
        EffectManager.ShowEffect(m_effectID, transform.position, 1f);
    }
}
