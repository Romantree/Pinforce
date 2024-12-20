using System.Runtime.Serialization;
using WILL.WT.PINFORCE.Managers;
using TS.FW.Wpf.v2.Core;

namespace WILL.WT.PINFORCE.Models
{
    [DataContract]
    public abstract class IRecipeModel : IModel
    {
        [DataMember]
        public RecipeType Type { get; set; }

        [DataMember]
        public int No { get => this.GetValue<int>(); set => this.SetValue(value); }

        [DataMember]
        public string Name { get => this.GetValue<string>(); set => this.SetValue(value); }

        public string LastWriteTime { get => this.GetValue<string>(); set => this.SetValue(value); }

        public string CreationTime { get => this.GetValue<string>(); set => this.SetValue(value); }

        public bool IsSelcted { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public IRecipeModel(RecipeType type) => this.Type = type;

        public override string ToString() => $"{No},{Type},{Name}";
    }
}
