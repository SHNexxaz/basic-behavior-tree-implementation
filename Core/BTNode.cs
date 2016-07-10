using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BT
{
    //BT node is the base of any nodes in BT framework
    public abstract class BTNode
    {
        public string name;

        protected List<BTNode> _children;
        public List<BTNode> children { get { return _children; } }

        //precondition is used to check if the node can be entered
        public BTPrecondition precondition;

        public Database database;

        //cool down function
        public float interval = 0;
        private float _lastTimeEvaluated = 0;

        public bool activated;

        public BTNode() : this(null) { }

        public BTNode(BTPrecondition precondition)
        {
            this.precondition = precondition;
        }

        public virtual void Activate (Database database)
        {
            if (activated)
                return;
            this.database = database;
            //activate precondition node
            if(precondition != null)
            {
                precondition.Activate(database);
            }
            //activate children nodes
            if(_children != null)
            {
                foreach (BTNode child in _children)
                    child.Activate(database);
            }
            activated = true;
        }

        public bool Evaluate()
        {
            bool coolDownOK = CheckTimer();
            return activated && coolDownOK && (precondition == null || precondition.Check()) && DoEvaluate();
        }

        protected virtual bool DoEvaluate()
        {
            return true;
        }

        public virtual BTResult Tick() { return BTResult.Ended; }

        public virtual void Clear() { }
    
        public virtual void AddChild(BTNode aNode)
        {
            if(_children == null)
            {
                _children = new List<BTNode>();
            }
            if(aNode!=null)
            {
                _children.Add(aNode);
            }
        }

        public virtual void RemoveChild (BTNode aNode)
        {
            if (_children != null && aNode != null)
                _children.Remove(aNode);
        }

        private bool CheckTimer()
        {
            if(Time.time - _lastTimeEvaluated > interval)
            {
                _lastTimeEvaluated = Time.time;
                return true;
            }
            return false;
        }
        


    }

    public enum BTResult
    {
        Ended = 1, Running = 2,
    }
}
