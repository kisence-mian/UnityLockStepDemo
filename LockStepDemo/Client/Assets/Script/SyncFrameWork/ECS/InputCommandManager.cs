using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCommandManager
{
    static SyncStatus s_syncStatus;

    //输入缓冲
    //消息缓冲

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public enum SyncStatus
    {
        Client,
        Service,
        Local,
    }
}
