using UnityEngine;
using System.Collections;
using System;

namespace BT
{
    //BT precondition is used to check if a BTNode can be entered
    //inherit from BTNode means it can be used as a normal node as well
    //it is useful when you need to check some condition to end some logics
    public abstract class BTPrecondition : BTNode
    {
        public BTPrecondition() : base(null) { }

        //override to provide the condition check
        public abstract bool Check(); //abstract fun, has to be implemented by derived class

        //functions as a node
        public override BTResult Tick()
        {
            bool success = Check();
            if (success)
            {
                return BTResult.Ended;
            }
            else
                return BTResult.Running;
        }
    }
        //a precondition that uses database
    public abstract class BTPreconditionUseDB:BTPrecondition
    {
        protected string _dataToCheck;
        protected int _dataIDToCheck;

        public BTPreconditionUseDB(string dataToCheck)
        {
            this._dataToCheck = dataToCheck;
        }

        public override void Activate(Database database)
        {
            base.Activate(database);
            _dataIDToCheck = database.GetDataID(_dataToCheck);
        }

    }

    //used to check if the float data in the database is less than/ equal to/ greater than the data passed in through constructor
    public class BTPreconditionFloat:BTPreconditionUseDB
    {
        public float rhs;
        private FloatFunction func;

        public BTPreconditionFloat(string dataToCheck, float rhs, FloatFunction func):base(dataToCheck)
        {
            this.rhs = rhs;
            this.func = func;
        }

        public override bool Check()
        {
            float lhs = database.GetData<float>(_dataIDToCheck);
            switch(func)
            {
                case FloatFunction.LessThan:
                    return lhs < rhs;
                case FloatFunction.GreaterThan:
                    return lhs > rhs;
                case FloatFunction.EqualTo:
                    return lhs == rhs;
            }
            return false;
        }

        public enum FloatFunction
        {
            LessThan = 1,
            GreaterThan = 2,
            EqualTo = 3,
        }
    }

    //used to check if the bool data in database is equal to the data passed in through constructor
    public class BTPreconditionBool : BTPreconditionUseDB
    {
        public bool rhs;

        public BTPreconditionBool(string dataToCheck, bool rhs):base(dataToCheck)
        {
            this.rhs = rhs;
        }

        public override bool Check()
        {
            bool lhs = database.GetData<bool>(_dataIDToCheck);
            return lhs == rhs;
        }
    }
    
    //used to check if the bool data in database is null
    public class BTPreconditionNull : BTPreconditionUseDB
    {
        private NullFunction func;

        public BTPreconditionNull(string dataToCheck, NullFunction func):base(dataToCheck)
        {
            this.func = func;
        }

        public override bool Check()
        {
            object lhs = database.GetData<object>(_dataIDToCheck);

            if (func == NullFunction.NotNull)
                return lhs != null;
            else
                return lhs == null;
        }

        public enum NullFunction { NotNull=1, Null = 2,}
    }

    public enum CheckType { Same = 1, Different = 2,}

}


