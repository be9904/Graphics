﻿using GtfPlayground.DataModel;
using UnityEditor.GraphToolsFoundation.Overdrive;
using UnityEngine.UIElements;

namespace GtfPlayground.GraphElements
{
    public class DataNodeTextPart : BaseModelUIPart
    {
        public DataNodeTextPart(string name, IGraphElementModel model, IModelUI ownerElement,
            string parentClassName) : base(name, model, ownerElement, parentClassName)
        {
        }

        public override VisualElement Root => label;
        private TextElement label;

        protected override void BuildPartUI(VisualElement parent)
        {
            if (m_Model is not DataNodeModel)
                return;

            label = new Label("");
            label.style.marginTop = 4;
            label.style.marginBottom = 4;
            label.style.marginLeft = 4;
            label.style.marginRight = 4;

            parent.Add(label);
        }

        protected override void UpdatePartFromModel()
        {
            if (m_Model is not DataNodeModel dataNode)
                return;

            label.text = $"intValue = {dataNode.intValue}\n" +
                         $"floatValue = {dataNode.floatValue}";
        }
    }
}