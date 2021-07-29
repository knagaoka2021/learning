using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeUtility {
    public enum NODE {
        ROOT,
        COMPOSITE,
        SELECTOR,
        SEQUENCE,
        ACTION
    }
    public static class NodeType {
        public static Type GetNodeTypeClass (NODE type) {
            switch (type) {
                case NODE.ROOT:
                    return typeof (RootNode);
                case NODE.COMPOSITE:
                    return typeof (CompositeNode);
                case NODE.SELECTOR:
                    return typeof (SelectorNode);
                case NODE.SEQUENCE:
                    return typeof (SequenceNode);
                case NODE.ACTION:
                    return typeof (ActionNode);
            }
            return typeof (GraphNode);
        }

    }
}