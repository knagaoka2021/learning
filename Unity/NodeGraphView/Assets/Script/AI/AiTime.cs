using System.Collections;
using System.Collections.Generic;
using AiDesign;
using UnityEngine;

public class AiTime : AiBase {
    public float WaitTime { get; set; }

    public override void OnInit () {
        NodeState = STATE.RUNNING;
        StartCoroutine ("DoSomething", WaitTime);
    }
    public override STATE OnUpdate () {
        if (NodeState == STATE.NONE) {
            OnInit ();
        }
        return NodeState;
    }
    private IEnumerator DoSomething (float time) {
        yield return new WaitForSeconds (time);
        this.NodeState = STATE.SUCCSESS;
    }
}