using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BT
{
    //BTAction is the base class for behavior node
    //it can add/ remove child
    //override the following to build a behavior(optional)
    //-enter
    //-execute
    //-exit
    //-clear
  
    public class BTAction : BTNode
    {
        private enum BTActionStatus { Ready = 1, Running = 2,}

        private BTActionStatus _status = BTActionStatus.Ready;

        public BTAction(BTPrecondition precondition = null) : base(precondition) { }

        protected virtual void Enter() { }

        protected virtual void Exit() { }

        protected virtual BTResult Execute()
        {
            return BTResult.Running;
        }

        public override void Clear()
        {
            if(_status != BTActionStatus.Ready)
            {
                //not clear yet
                Exit();
                _status = BTActionStatus.Ready;
            }
        }

        public override BTResult Tick()
        {
            BTResult result = BTResult.Ended;
            if(_status == BTActionStatus.Ready)
            {
                Enter();
                _status = BTActionStatus.Running;
            }
            if(_status == BTActionStatus.Running)
            {
                result = Execute();
                if(result != BTResult.Running)
                {
                    Exit();
                    _status = BTActionStatus.Ready;
                }

            }

            return result;
        }

        public override void AddChild(BTNode aNode)
        {
            Debug.LogError("BTAction: Cannot add a node into BTAction.");
        }

        public override void RemoveChild(BTNode aNode)
        {
            Debug.LogError("BTAction: Cannot remove a node into BTAction.");
        }

    }
}


