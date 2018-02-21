using System;
using System.Collections.Generic;

namespace FinanceMonkey
{
    class InstanceSize
    {
        private readonly IDictionary<string, IList<string>> _models = new Dictionary<string, IList<string>>
        {
            { "t2", new List<string>() {
                "nano",
                "micro",
                "small",
                "medium",
                "large",
                "xlarge",
                "2xlarge"
            } }
        };

        public string InstanceClass { get; }
        public string InstanceModel { get; }

        public InstanceSize(string instanceSize)
        {
            var stringParts = instanceSize.Split(".");
            if (stringParts.Length != 2) { throw new Exception("Invalid instance size given. Should be `class.model`, e.g. `t2.micro`"); }
            InstanceClass = stringParts[0];
            InstanceModel = stringParts[1];
        }

        public InstanceSize(string instanceClass, string instanceModel)
        {
            InstanceClass = instanceClass;
            InstanceModel = instanceModel;
        }

        public InstanceSize Downsize()
        {
            if (IsSmallest) throw new Exception($"Currently {InstanceClass}.{InstanceModel} - can't get any smaller than that!");
            var currentSizeIndex = _models[InstanceClass].IndexOf(InstanceModel);
            var nextSizeDown = _models[InstanceClass][currentSizeIndex - 1];
            return new InstanceSize(InstanceClass, nextSizeDown);
        }

        public bool IsSmallest => _models[InstanceClass].IndexOf(InstanceModel) == 0;

        public override string ToString()
        {
            return $"{InstanceClass}.{InstanceModel}";
        }
    }
}
