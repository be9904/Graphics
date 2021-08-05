﻿using System.Collections.Generic;
using System.Linq;
using UnityEditor.GraphToolsFoundation.Overdrive;

namespace UnityEditor.ShaderGraph.GraphUI.Utilities
{
    public static class NodeModelExtensions
    {
        public static IEnumerable<IEdgeModel> GetIncomingEdges(this IPortNodeModel nodeModel)
        {
            return nodeModel
                .Ports
                .Where(port => port.Direction == PortDirection.Input)
                .SelectMany(port => port.GetConnectedEdges());
        }
    }
}
