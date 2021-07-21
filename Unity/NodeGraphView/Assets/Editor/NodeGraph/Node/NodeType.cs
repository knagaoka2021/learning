using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeUtility {
    public enum NODE {
        ROOT,
        COMPOSITE,
        ACTION
    }
    public static class NodeType {
        public static Type GetNodeTypeClass (NODE type) {
            switch (type) {
                case NODE.ROOT:
                    break;
                case NODE.COMPOSITE:
                    return typeof (CompositeNode);
                case NODE.ACTION:
                    return typeof (ActionNode);
            }
            return typeof (GraphNode);
        }

    }
}