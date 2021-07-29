using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUtil;

namespace AiDesign {
    public enum BEHAVIOR {
        NONE,
        TIME,
        MOVE,
        STATUS
    }
    public enum STATE {
        NONE,
        RUNNING,
        SUCCSESS,
        FAILURE
    }

    public class AiBase : MonoBehaviour {
        public string Name { get; set; }
        public BEHAVIOR Behavior { get; set; }
        public CALCULATE Calculate { get; set; }
        public STATE NodeState { get; set; }
        public virtual void OnInit () {
            NodeState = STATE.RUNNING;
        }
        public virtual STATE OnUpdate () {

            if (NodeState == STATE.NONE) {
                OnInit ();
            }

            return NodeState;
        }
    }
}