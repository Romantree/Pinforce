using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.FW.Wpf.Core;

namespace WILL.WT.PINFORCE.Models.Config
{
    public class MotionDataModel : DataModelBase
    {
        // public string Name { get; private set; }

        // 단위 : mm/s
        public int ReadyPosMoveSpeed { get => this.GetValue<int>(); set => this.SetValue(value); }

        // 생성자
        // public MotionDataModel(string name) => this.Name = name; // ViewModel에서 입력한 "NAME"
    }
}

