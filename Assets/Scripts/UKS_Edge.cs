// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// [Serializable]
// public class UKS_Edge
// {
//     public string label;
//     [SerializeReference] public UKS_Node source;
//     [SerializeReference] public UKS_Node target;
//     [SerializeReference] public UKS_Node relationshipType;

//     public UKS_Edge(UKS_Node source, UKS_Node target, UKS_Node relationshipType)
//     {
//         this.label = source.label + " -> " + relationshipType.label + " -> " + target.label;
//         this.source = source;
//         this.target = target;
//         this.relationshipType = relationshipType;
//     }

//     // public override bool Equals(object obj)
//     // {
//     //     if (obj == null || GetType() != obj.GetType()) return false;
//     //     UKS_Edge other = (UKS_Edge)obj;
//     //     return source.Equals(other.source) && target.Equals(other.target) && relationshipType.Equals(other.relationshipType);
//     // }

//     // public override int GetHashCode() => HashCode.Combine(source, target, relationshipType);

//     // public override string ToString() => relationshipType.label;
// }
