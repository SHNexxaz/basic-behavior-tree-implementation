using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BT
{
    //BTPrioritySelector selects the first successfully evaluated child as the active child
    public class BTPrioritySelector : BTNode
    {
        private BTNode _activeChild;

        public BTPrioritySelector(BTPrecondition precondition = null) : base(precondition) { }

        protected override bool DoEvaluate()
        {
            foreach (BTNode child in children)
            {
                if (child.Evaluate())
                {
                    if (_activeChild != null && _activeChild != child)
                        _activeChild.Clear();
                
                    //select the first successfully evaluated child
                    _activeChild = child;
                    return true;
                }
            }
            _activeChild = null;
            return false;
        }

        public override void Clear()
        {
            if(_activeChild != null)
            {
                _activeChild.Clear();
                _activeChild = null;
            }
        }

        public override BTResult Tick()
        {
            if (_activeChild == null)
                return BTResult.Ended;

            BTResult result = _activeChild.Tick();
            if(result!=BTResult.Running)
            {
                _activeChild.Clear();
                _activeChild = null;
            }
            return result;
        }





    }
}


