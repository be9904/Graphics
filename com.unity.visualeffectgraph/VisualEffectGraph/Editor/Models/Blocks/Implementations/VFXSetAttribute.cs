using System;
using System.Linq;
using System.Collections.Generic;

namespace UnityEditor.VFX
{
    [VFXInfo(category = "Attribute")]
    class VFXSetAttribute : VFXBlock
    {
        [VFXSetting]
        [StringProvider(typeof(AttributeProvider))]
        public string attribute = VFXAttribute.All.First();

        public override string name { get { return "SetAttribute"; } }
        public override VFXContextType compatibleContexts { get { return VFXContextType.kInitAndUpdateAndOutput; } }
        public override VFXDataType compatibleData { get { return VFXDataType.kParticle; } }
        public override IEnumerable<VFXAttributeInfo> attributes
        {
            get
            {
                return new List<VFXAttributeInfo>() { new VFXAttributeInfo(currentAttribute, VFXAttributeMode.Write) };
            }
        }
        static private string GenerateLocalAttributeName(string name)
        {
            return name[0].ToString().ToUpper() + name.Substring(1);
        }

        public override string source
        {
            get
            {
                var attribute = currentAttribute;
                return string.Format("{0} = {1};", attribute.name, GenerateLocalAttributeName(attribute.name));
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            UpdateInputFromSettings();
        }

        protected override void Invalidate(VFXModel model, InvalidationCause cause)
        {
            base.Invalidate(model, cause);
            if (cause == InvalidationCause.kSettingChanged || inputSlots.Count == 0)
            {
                UpdateInputFromSettings();
            }
        }

        private VFXAttribute currentAttribute
        {
            get
            {
                return VFXAttribute.Find(attribute, VFXAttributeLocation.Current);
            }
        }

        private void UpdateInputFromSettings()
        {
            var attribute = currentAttribute;
            var expression = new VFXAttributeExpression(attribute);
            var localAttributeName = GenerateLocalAttributeName(attribute.name);
            if (inputSlots.Count > 0)
            {
                if (inputSlots[0].name == localAttributeName)
                {
                    return;
                }
            }

            AddSlot(VFXSlot.Create(new VFXProperty(VFXExpression.TypeToType(expression.valueType), localAttributeName), VFXSlot.Direction.kInput));
            if (inputSlots.Count == 2)
            {
                CopyLink(inputSlots[0], inputSlots[1]);
                inputSlots[0].UnlinkAll(false);
                RemoveSlot(inputSlots[0]);
            }

            if (inputSlots.Count >= 2)
                throw new Exception("Unexpected behavior in VFXSetAttribute");
        }
    }
}
