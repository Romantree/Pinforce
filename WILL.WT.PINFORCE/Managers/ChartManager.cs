using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using TS.FW.Wpf.Core;
using TS.FW.Wpf.v2.Core;
using WILL.WT.PINFORCE.Managers;
using static Infragistics.Shared.DynamicResourceString;

namespace WILL.WT.PINFORCE.Managers
{
    // ChartManager의 File 구성 : ChartData, ChartDataList
    // Instance는 ViewModel에 있음
    public class ChartManager { }

    public class ChartData : IModel
    {
            // <ig:XamDataChart.Axes>
            // <ig:CategoryXAxis x:Name="xAxis"  Label="{}{Label}" ItemsSource="{Binding }"/>
            public double Label { get => this.GetValue<double>(); set => this.SetValue(value); }

            // ValueMemberPath
            public double Weight1 { get => this.GetValue<double>(); set => this.SetValue(value); }
            public double Weight2 { get => this.GetValue<double>(); set => this.SetValue(value); }        
    }

    public class ChartDataList : ObservableCollection<ChartData>
    {
        private bool IsDesignMode
        {
            get
            {
                return DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject());
            }
        }

        public ChartDataList()
        {

        }

        // ChartDisplayViewModel.Instance.ChartData.AddValue(idx, weight1, weight2);
        public void AddValue(double od, double weight1, double weight2)
        {
            // List Add
            if (Application.Current.Dispatcher.CheckAccess())
            {
                // UI 스레드에서 실행 중이므로 바로 추가
                this.Add(new ChartData() { Label = od, Weight1 = weight1, Weight2 = weight2 });
            }
            else
            {
                // UI 스레드가 아니므로 Dispatcher를 통해 추가
                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.Add(new ChartData() { Label = od, Weight1 = weight1, Weight2 = weight2 });
                });
            }
        }

        // ChartDisplayViewModel.Instance.ChartData.ClearValue();
        public void ClearValue()
        {
            // List Clear
            if (Application.Current.Dispatcher.CheckAccess())
            {
                // UI 스레드에서 실행 중이므로 바로 추가
                this.Clear();
            }
            else
            {
                // UI 스레드가 아니므로 Dispatcher를 통해 추가
                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.Clear();
                });
            }
        }
    }
}
