using System;
using System.Windows;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using System.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ObservableExtension;  // my github extinsion https://github.com/Sia819/ObservableList

namespace TestProject_WPF.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ObservableList<Type> winList;
        public ObservableList<Type> WinList
        {
            get { return winList; }
            set { Set(ref winList, value, true); }
        }

        private int winList_SelectedIndex;
        public int WinList_SelectedIndex
        {
            get { return winList_SelectedIndex; }
            set { Set(ref winList_SelectedIndex, value); }
        }

        public ICommand GetStart { get; private set; }

        [Obsolete]
        public MainWindowViewModel()
        {
            WinList = new ObservableList<Type>();
            GetStart = new RelayCommand(SelectedWindowStart);

            var assems = Assembly.GetExecutingAssembly().DefinedTypes;
            var result = from a in assems where a.BaseType.Name == "Window" orderby a.Name select a;
            WinList.AddRange(result);
        }

        [STAThread]
        [Obsolete]
        private void SelectedWindowStart()
        {
            new Thread(() =>
            {
                Type listdata;
                int index = WinList_SelectedIndex;  // user selected list of window / 사용자가 선택한 윈도우
                if (index < 0) index = 0;
                listdata = WinList[index];
                var windowClass = Activator.CreateInstance(listdata); // This type can be create Instance of Window / 타입으로 윈도우 인스턴스를 만듦니다.
                Window targetWindow = windowClass as Window;
                targetWindow.ShowDialog();
            })
            {
                ApartmentState = ApartmentState.STA,
                IsBackground = true,
            }.Start();
        }
    }
}
