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
            set { Set(ref winList, value); }
        }

        private int winList_SelectedIndex;
        public int WinList_SelectedIndex
        {
            get { return winList_SelectedIndex; }
            set { Set(ref winList_SelectedIndex, value); }
        }

        private string displayText;
        public string DisplayText
        {
            get { return displayText; }
            set { Set(ref displayText, value); }
        }

        private string labelText;
        public string LabelText
        {
            get { return labelText; }
            set { Set(ref labelText, value); }
        }

        private string buttonText;
        public string ButtonText
        {
            get { return buttonText; }
            set { Set(ref buttonText, value); }
        }


        public ICommand ProgramStart_Click { get; private set; }
        public ICommand Hyperlink_Click { get; private set; }

        private bool IsDisplayingEnglish = true;

        [Obsolete]
        public MainWindowViewModel()
        {
            WinList = new ObservableList<Type>();
            ProgramStart_Click = new RelayCommand(SelectedWindowStart);
            Hyperlink_Click = new RelayCommand(LanguageChange);
            LanguageChange();

            var assems = Assembly.GetExecutingAssembly().DefinedTypes;
            var result = from a in assems where a.BaseType.Name == "Window" && a.Name != "MainWindow" orderby a.Name select a;
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

        private void LanguageChange()
        {
            if (IsDisplayingEnglish)
            {
                LabelText = "Need English?";
                DisplayText = "이 프로젝트에 Window.xaml을 추가하면,\n자동으로 모든 Window가 아래 리스트에 표시됩니다. \n(프로젝트 - 추가 - 새 항목 - Window.xaml)";
                ButtonText = "프로그램 시작";
                IsDisplayingEnglish = false;
            }
            else
            {
                LabelText = "Need Koeran?";
                DisplayText = "If you just Add Window.xaml in this Project,\nautomatically added in list your Window.\n(Project - Add - New Item - Window.xaml)";
                ButtonText = "Program Start";
                IsDisplayingEnglish = true;
            }
        }
    }
}
