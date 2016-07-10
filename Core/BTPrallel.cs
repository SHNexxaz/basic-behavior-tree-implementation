using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BT
{
    //BTParallel evaluates all children, if any of them fails the evaluation, BTParallel fails
    //BTParallel ticks all children, if
    //1. ParallelFunction.And: ends when all children ends
    //2. ParallelFunction.Or: ends when any of the children ends
    //NOTE: order of child node added does matter
    public class BTParallel : BTNode
    {
        protected List<BTResult> _result;
        protected ParallelFunction _func;

        public BTParallel(ParallelFunction func) : this(func, null) { }

        public BTParallel (ParallelFunction func, BTPrecondition precondition):base(precondition)
        {
            _result = new List<BTResult>();
            this._func = func;
        }

        public override BTResult Tick()
        {
            int endingResultCount = 0;

            for(int i=0;i<children.Count;i++)
            {
                if(_func == ParallelFunction.And)
                {
                    if (_result[i] == BTResult.Running)
                        _result[i] = children[i].Tick();
                    if (_result[i] != BTResult.Running)
                        endingResultCount++;
                }
                else
                {
                    if (_result[i] == BTResult.Running)
                        _result[i] = children[i].Tick();
                    if (_result[i] != BTResult.Running)
                    {
                        ResetResult();
                        return BTResult.Ended;
                    }                                            
                }
            }

            if(endingResultCount == children.Count)
            {
                //only apply to AND func
                ResetResult();
                return BTResult.Ended;
            }
            return BTResult.Running;
        }

        protected override bool DoEvaluate()
        {
            foreach(BTNode child in children)
            {
                if(!child.Evaluate())
                {
                    return false;
                }
            }
            return true;
        }

        public override void Clear()
        {
            ResetResult();
            foreach(BTNode child in children)
            {
                child.Clear();
            }
        }

        private void ResetResult()
        {
            for (int i=0;i<_result.Count;i++)
            {
                _result[i] = BTResult.Running;
            }
        }

        public override void AddChild(BTNode aNode)
        {
            base.AddChild(aNode);
            _result.Add(BTResult.Running);
        }

        public override void RemoveChild(BTNode aNode)
        {
            int index = _children.IndexOf(aNode);
            _result.RemoveAt(index);
            base.RemoveChild(aNode);
        }

        public enum ParallelFunction
        { And = 1,
          Or = 2,
        }

        

    }
}


