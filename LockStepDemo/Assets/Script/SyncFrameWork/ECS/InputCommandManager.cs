using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCommandManager
{
    static SyncStatus s_syncStatus;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public enum SyncStatus
    {
        Client,
        Service,
        Local,
    }
}
