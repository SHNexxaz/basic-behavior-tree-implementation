using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BT;

//1. initiate values in the database for the children to use
//2. initiate BT _root
//3. some actions & preconditions that will be used later
//4. add children nodes
//5. activate the _root, including the children nodes initialization


public class BTTree : MonoBehaviour {

    protected BTNode _root = null;

    [HideInInspector]
    public Database database;

    [HideInInspector]
    public bool isRunning = true;

    public const string RESET = "Rest";
    private static int _resetID;

    void Awake()
    {
        Init();
        _root.Activate(database);
    }

    void Update()
    {
        if (!isRunning)
            return;

        if(database.GetData<bool>(RESET))
        {
            Reset();
            database.SetData<bool>(RESET, false);
        }

        //iterate the BT tree
        if(_root.Evaluate())
        {
            _root.Tick();
        }
    }

    protected virtual void Init()
    {
        database = GetComponent<Database>();
        if(database == null)
        {
            database = gameObject.AddComponent<Database>();
        }

        _resetID = database.GetDataID(RESET);
        database.SetData<bool>(_resetID, false);
    }

    void OnDestory()
    {
        if (_root != null)
            _root.Clear();
    }

    protected void Reset()
    {
        _root.Clear();
    }
}
