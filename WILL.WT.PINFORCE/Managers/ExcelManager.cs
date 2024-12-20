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
using TS.FW.Wpf.Core;
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
        private ExcelDataList _excelData;

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

    public class ExcelData: ModelBase
    {
        public int Time { get => this.GetValue<int>(); set => this.SetValue(value); }

        // ValueMemberPath
        public double Weight1 { get => this.GetValue<double>(); set => this.SetValue(value); }
        public double Weight2 { get => this.GetValue<double>(); set => this.SetValue(value); }
    }

    public class ExcelDataList : ObservableCollection<ExcelData>
    {
        private const string ROOT = @"..\Data";

        // ChartDisplayViewModel.Instance.ChartData.AddValue(idx, weight1, weight2);
        public void AddValue(int idx, double weight1, double weight2)
        {
            // List Add
            this.Add(new ExcelData() { Time = idx, Weight1 = weight1, Weight2 = weight2 });
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

            //string filePath = $"C:\\Thinksoft\\WILL.WT.PINFORCE\\Data\\{DateTime.Now:yyyyMMdd_HHmmss}_DemoData.xlsx";

            // 상대경로를 쓰면 계속해서 경로 오류가 나는데 이유를 모르겠다
            /*
            string dateTime = $"{DateTime.Now:yyyyMMddHHmmss}";
            string name = "_DemoData.xlsx";
            string filePath = Path.Combine(ROOT, name);
            */

            var fileName = "DemoData";
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

                // 헤더 작성
                worksheet.Cells[1, 1] = "Time";
                worksheet.Cells[1, 2] = "Weight1";
                worksheet.Cells[1, 3] = "Weight2";

                // 데이터 삽입
                int row = 2; // 두 번째 행부터 데이터 시작
                foreach (var data in this)
                {
                    worksheet.Cells[row, 1] = data.Time;
                    worksheet.Cells[row, 2] = data.Weight1;
                    worksheet.Cells[row, 3] = data.Weight2;
                    row++;
                }
                Debug.WriteLine(filePath);

                // 파일 저장
                workbook.SaveAs(filePath);
                workbook.Close();
                excelApp.Quit();

                Debug.WriteLine($"Excel 파일이 성공적으로 저장되었습니다: {filePath}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excel 출력 중 오류 발생: {ex.Message}");
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
