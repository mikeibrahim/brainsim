using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class UKS_Edge
{
    public UKS_Node source;
    public UKS_Node target;
    public UKS_Node relationshipType;

    public UKS_Edge(UKS_Node source, UKS_Node target, UKS_Node relationshipType)
    {
        this.source = source;
        this.target = target;
        this.relationshipType = relationshipType;
    }
}
