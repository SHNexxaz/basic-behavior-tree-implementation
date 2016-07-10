using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//store data from local nodes and cross tree nodes
//Nodes can read the data inside a database by using a string or an index of the data
//index access is prefered for efficiency's sake

public class Database : MonoBehaviour {

    //_database & _dataNames are 1 to 1 relationship
    private List<object> _database = new List<object>();
    private List<string> _dataNames = new List<string>();

    //using string to get data
    public T GetData<T> (string dataName)
    {
        int dataID = IndexOfDataID(dataName);
        if (dataID == -1)
            Debug.LogError("Database: Data for " + dataName + "does not exist!");

        return (T)_database[dataID];
    }

   //use dataID to get data
   public T GetData<T> (int dataID)
    {
        return (T)_database[dataID];
    }

    public void SetData<T> (string dataName, T data)
    {
        int dataID = IndexOfDataID(dataName);
        _database[dataID] = (object)data;
    }

    public void SetData<T> (int dataID, T data)
    {
        _database[dataID] = (object)data;
    }

    public int GetDataID(string dataName)
    {
        int dataID = IndexOfDataID(dataName);
        if(dataID == -1)
        {
            _dataNames.Add(dataName);
            _database.Add(null);
            dataID = _dataNames.Count - 1;
        }
        return dataID;
    }

    private int IndexOfDataID(string dataName)
    {
        for (int i=0;i<_dataNames.Count;++i)
        {
            if (_dataNames[i].Equals(dataName))
                return i;
        }
        return -1;
    }

    public bool ContainsData (string dataName)
    {
        return IndexOfDataID(dataName) != -1;
    }
}
