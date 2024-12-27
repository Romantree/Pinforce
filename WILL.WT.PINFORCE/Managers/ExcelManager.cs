using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Util;
using TS.FW;
using TS.FW.Wpf.Core;
using TS.FW.Wpf.v2.Core;
using WILL.WT.PINFORCE.Controls.Chart;
using WILL.WT.PINFORCE.Controls.Vision;

using Excel = Microsoft.Office.Interop.Excel;

namespace WILL.WT.PINFORCE.Managers
{
    public class ExcelManager
    {
        // 생성자에서 초기화
        private ExcelManager()
        {
            _excelRawData = new ExcelRawDataList();
            _excelData = new ExcelDataList();
        }

        // 싱글톤 인스턴스
        private static ExcelManager _instance;

        // 싱글톤 인스턴스 반환
        public static ExcelManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ExcelManager();
                }
                return _instance;
            }
        }

        // ExcelManager의 DataList
        private ExcelRawDataList _excelRawData;
        private ExcelDataList _excelData;

        public ExcelRawDataList ExcelRawData
        {
            get => _excelRawData;
            set
            {
                _excelRawData = value;
                OnPropertyChanged(nameof(ExcelRawData));
            }
        }

        public ExcelDataList ExcelData
        {
            get => _excelData;
            set
            {
                _excelData = value;
                OnPropertyChanged(nameof(ExcelData));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ExcelData: IModel
    {
        public string Time { get => this.GetValue<string>(); set => this.SetValue(value); }
        public double ZPos { get => this.GetValue<double>(); set => this.SetValue(value); }
        public double OD { get => this.GetValue<double>(); set => this.SetValue(value); }

        // ValueMemberPath
        public double Weight1 { get => this.GetValue<double>(); set => this.SetValue(value); }
        public double Weight2 { get => this.GetValue<double>(); set => this.SetValue(value); }
        public double Weight3 { get => this.GetValue<double>(); set => this.SetValue(value); }
        public double Weight4 { get => this.GetValue<double>(); set => this.SetValue(value); }
        public double Weight5 { get => this.GetValue<double>(); set => this.SetValue(value); }
        public double Weight6 { get => this.GetValue<double>(); set => this.SetValue(value); }
    }

    public class ExcelRawDataList : ObservableCollection<ExcelData> // 주기적으로 작성되는 ExcelList
    {
        private const string ROOT = @"..\Data\1st_Log";

        // ChartDisplayViewModel.Instance.ChartData.AddValue(idx, weight1, weight2);
        public void AddValue(double zPos, double od, double weight1, double weight2)
        {
            // List Add
            this.Add(new ExcelData() { Time = DateTime.Now.ToString("HH::mm::ss.FFF"), ZPos = zPos, OD = od, Weight1 = weight1, Weight2 = weight2 });
        }

        // ChartDisplayViewModel.Instance.ChartData.ClearValue();
        public void ClearValue()
        {
            // List Clear
            this.Clear();
        }

        public void SaveExcel()
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            // string filePath = $"C:\\Thinksoft\\WILL.WT.PINFORCE\\Data\\{DateTime.Now:yyyyMMdd_HHmmss}_1st_DemoData.xlsx";

            // 상대경로를 쓰면 계속해서 경로 오류가 나는데 이유를 모르겠다
            /*
            string dateTime = $"{DateTime.Now:yyyyMMddHHmmss}";
            string name = "_DemoData.xlsx";
            string filePath = Path.Combine(ROOT, name);
            */

            var fileName = AP.Proc.Main.WorkItem.Rcp + "_PinForce_1st";
            var filePath = Path.Combine(Path.GetFullPath(ROOT), $"{DateTime.Now:yyyyMMdd_HHmmss}_{fileName}.xlsx");

            try
            {
                // 폴더가 존재하지 않으면 생성
                if (Directory.Exists(ROOT) == false) Directory.CreateDirectory(ROOT);

                // Excel 애플리케이션 시작
                excelApp = new Excel.Application();
                workbook = excelApp.Workbooks.Add();
                worksheet = (Excel.Worksheet)workbook.ActiveSheet;
                excelApp.DisplayAlerts = false; // 팝업 끄기
                worksheet.Columns.AutoFit(); // 열 너비 자동으로

                // 헤더 작성
                worksheet.Cells[1, 1] = "Time";
                worksheet.Cells[1, 2] = "Probe Z-Pos(㎜)";
                worksheet.Cells[1, 3] = "O/D(㎛)";
                worksheet.Cells[1, 4] = "Loadcell #1(g)";
                worksheet.Cells[1, 5] = "Loadcell #2(g)";

                // 데이터 삽입
                int row = 2; // 두 번째 행부터 데이터 시작

                foreach (var data in this)
                {
                    worksheet.Cells[row, 1] = data.Time;
                    worksheet.Cells[row, 2] = data.ZPos;
                    worksheet.Cells[row, 3] = data.OD;
                    worksheet.Cells[row, 4] = data.Weight1;
                    worksheet.Cells[row, 5] = data.Weight2;
                    row++;
                }
                // 가장 마지막 행에 Recipe 이름 작성
                worksheet.Cells[row, 1] = string.Format("Recipe Name : {0}", AP.Proc.Main.WorkItem.Rcp.Name);

                // 파일 저장
                workbook.SaveAs(filePath);
                workbook.Close();
                excelApp.Quit();

                Logger.Write($"1st Raw Data Excel Save Succeed : {filePath}");
            }
            catch (Exception ex)
            {
                Logger.Write($"1st Raw Data Excel Save Failed : {ex.Message}");
            }
            finally
            {
                // Excel 프로세스 종료 및 리소스 해제
                if (worksheet != null) Marshal.ReleaseComObject(worksheet);
                if (workbook != null) Marshal.ReleaseComObject(workbook);
                if (excelApp != null)
                {
                    excelApp.Quit();
                    Marshal.ReleaseComObject(excelApp);
                }

                worksheet = null;
                workbook = null;
                excelApp = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();

                // List Clear
                this.Clear();
            }
        }
    }

    public class ExcelDataList : ObservableCollection<ExcelData> // 가공된 데이터를 작성하는 ExcelList
    {
        private const string ROOT = @"..\Data\2nd_Log";

        // ChartDisplayViewModel.Instance.ChartData.AddValue(idx, weight1, weight2);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="zPos">Z-Axis의 위치</param>
        /// <param name="od">O/D</param>
        /// <param name="avg1">Loadcell #1의 Contact 이후의 평균값</param>
        /// <param name="avg2">Loadcell #2의 Contact 이후의 평균값</param>
        /// <param name="tot1">Loadcell #1의 Contact 포함 평균값</param>
        /// <param name="tot2">Loadcell #2의 Contact 포함 평균값</param>
        /// <param name="rel1">Loadcell #1의 가장 마지막 값(Release 직전)</param>
        /// <param name="rel2">Loadcell #2의 가장 마지막 값(Release 직전)</param>
        public void AddValue(double zPos, double od, double avg1, double avg2, double tot1, double tot2, double rel1, double rel2)
        {
            // List Add
            this.Add(new ExcelData() { Time = DateTime.Now.ToString("HH::mm::ss.FFF"), ZPos = zPos, OD = od, Weight1 = avg1, Weight2 = avg2, Weight3 = tot1, Weight4 = tot2, Weight5 = rel1, Weight6 = rel2 });
        }

        // ChartDisplayViewModel.Instance.ChartData.ClearValue();
        public void ClearValue()
        {
            // List Clear
            this.Clear();
        }

        public void SaveExcel()
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            // string filePath = $"C:\\Thinksoft\\WILL.WT.PINFORCE\\Data\\{DateTime.Now:yyyyMMdd_HHmmss}_2nd_DemoData.xlsx";

            // 상대경로를 쓰면 계속해서 경로 오류가 나는데 이유를 모르겠다
            /*
            string dateTime = $"{DateTime.Now:yyyyMMddHHmmss}";
            string name = "_DemoData.xlsx";
            string filePath = Path.Combine(ROOT, name);
            */

            var fileName = AP.Proc.Main.WorkItem.Rcp + "_PinForce_2nd";
            var filePath = Path.Combine(Path.GetFullPath(ROOT), $"{DateTime.Now:yyyyMMdd_HHmmss}_{fileName}.xlsx");

            try
            {
                // 폴더가 존재하지 않으면 생성
                if (Directory.Exists(ROOT) == false) Directory.CreateDirectory(ROOT);

                // Excel 애플리케이션 시작
                excelApp = new Excel.Application();
                workbook = excelApp.Workbooks.Add();
                worksheet = (Excel.Worksheet)workbook.ActiveSheet;
                excelApp.DisplayAlerts = false; // 팝업 끄기
                worksheet.Columns.AutoFit(); // 열 너비 자동으로

                // 헤더 작성
                worksheet.Cells[1, 1] = "Time";
                worksheet.Cells[1, 2] = "O/D(㎛)";
                
                worksheet.Cells[1, 3] = "Loadcell #1 AVG.(g)";
                worksheet.Cells[1, 4] = "Loadcell #2 AVG.(g)";
                worksheet.Cells[1, 5] = "Loadcell #1 Total(g)";
                worksheet.Cells[1, 6] = "Loadcell #2 Total(g)";
                worksheet.Cells[1, 7] = "Loadcell #1 Release(g)";
                worksheet.Cells[1, 8] = "Loadcell #2 Release(g)";

                // 데이터 삽입
                int row = 2; // 두 번째 행부터 데이터 시작

                foreach (var data in this)
                {

                    worksheet.Cells[row, 1] = data.Time;
                    worksheet.Cells[row, 2] = data.OD;
                    worksheet.Cells[row, 3] = data.Weight1;
                    worksheet.Cells[row, 4] = data.Weight2;
                    worksheet.Cells[row, 5] = data.Weight3;
                    worksheet.Cells[row, 6] = data.Weight4;
                    worksheet.Cells[row, 7] = data.Weight5;
                    worksheet.Cells[row, 8] = data.Weight6;
                    row++;
                }
                // 가장 마지막 행에 Recipe 이름 작성
                worksheet.Cells[row, 1] = string.Format("Recipe Name : {0}", AP.Proc.Main.WorkItem.Rcp.Name);

                // 파일 저장
                workbook.SaveAs(filePath);
                workbook.Close();
                excelApp.Quit();

                Logger.Write($"2nd Raw Data Excel Save Succeed : {filePath}");
            }
            catch (Exception ex)
            {
                Logger.Write($"1st Raw Data Excel Save Failed : {ex.Message}");
            }
            finally
            {
                // Excel 프로세스 종료 및 리소스 해제
                if (worksheet != null) Marshal.ReleaseComObject(worksheet);
                if (workbook != null) Marshal.ReleaseComObject(workbook);
                if (excelApp != null)
                {
                    excelApp.Quit();
                    Marshal.ReleaseComObject(excelApp);
                }

                worksheet = null;
                workbook = null;
                excelApp = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();

                // List Clear
                this.Clear();
            }
        }
    }
}
