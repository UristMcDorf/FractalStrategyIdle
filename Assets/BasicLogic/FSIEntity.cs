using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSIEntity : MonoBehaviour
{
    [SerializeField]
    protected string _sysName;
    public string SysName
    {
        get
        {
            return _sysName;
        }
    }

    [SerializeField]
    protected string _inGameName;
    public string InGameName
    {
        get
        {
            return _inGameName;
        }
    }

    [TextArea]
    [SerializeField]
    protected string _description;
    public string Description
    {
        get
        {
            return _description;
        }
    }

	// Use this for initialization
	protected virtual void Start()
    {
        name = SysName;
        //_inGameName = SysName; //TODO: move to loading from xml
	}

    protected virtual void Update()
    {
        
    }
}
