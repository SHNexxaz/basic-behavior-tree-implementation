using UnityEngine;
using System.Collections;

namespace BT
{
    //BTSequence evaluates the current active child, or the first child(if no active child)
    //if passed the evaluation, BTSequence ticks the current active child, or the first child (if no active child avaliable)
    //and if it's result is BTEnded, then change the active child to the next one
    public class BTSequence : BTNode
    {
        private BTNode _activeChild;
        private int _activeIndex = -1;

        public BTSequence(BTPrecondition precondition = null) : base(precondition) { }

        protected override bool DoEvaluate()
        {
            if (_activeChild != null)
            {
                bool result = _activeChild.Evaluate();
                if (!result)
                {
                    _activeChild.Clear();
                    _activeChild = null;
                    _activeIndex = -1;
                }
                return result;
            }
            else
                return children[0].Evaluate();
        }



        public override BTResult Tick()
        {
            //first time
            if(_activeChild == null)
            {
                _activeChild = children[0];
                _activeIndex = 0;
            }

            BTResult result = _activeChild.Tick();
            if(result == BTResult.Ended)
            {
                //current active node has finished
                _activeIndex++;
                if(_activeIndex >= children.Count) //sequence has finished
                {
                    _activeChild.Clear();
                    _activeChild = null;
                    _activeIndex = -1;
                }
                else //next node
                {
                    _activeChild.Clear();
                    _activeChild = children[_activeIndex];
                    result = BTResult.Running;
                }
            }
            return result;
        }

        public override void Clear()
        {
            if(_activeChild!= null)
            {
                _activeChild = null;
                _activeIndex = -1;
            }

            foreach (BTNode child in children)
                child.Clear();
        }
    }
}


