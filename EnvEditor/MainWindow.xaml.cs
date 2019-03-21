using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace EnvEditor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        EnvService envService = new EnvService();
        private ObservableCollection<EnvVar> envList;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            this.DataContext = this;
            EnvList = new ObservableCollection<EnvVar>();

            InitializeComponent();
            //this.EnvList = envService.getEnvs();

        }



        public ObservableCollection<EnvVar> EnvList
        {
            get
            {
                return envList;
            }
            set
            {
                if (envList != value)
                {
                    envList = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(EnvList)));
                    }
                }
            }
        }

        private void BtnLoadCurrent_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("当前未保存修改将会丢失，是否继续？", "警告", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            if (result != MessageBoxResult.OK)
            {
                return;
            }
            this.EnvList = new ObservableCollection<EnvVar>(envService.GetEnvs());
        }

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("该操作将会覆盖现有环境变量，建议备份当前系统环境变量后再进行操作。是否继续？", "警告", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            if (result == MessageBoxResult.OK)
            {
                this.envService.SetEnvs(EnvList.ToList());
            }
        }

        private void OpenVarEditor(object sender, MouseButtonEventArgs e)
        {
            EnvVar var = (EnvVar)((ListViewItem)sender).Content;
            List<string> lines = var.Value.Split(';').ToList();
            VarEditor editorWindow = new VarEditor(var.Key, lines);
            editorWindow.Owner = this;
            editorWindow.ShowDialog();
        }

        public void EditingResult(string originName, string newName, string value)
        {
            if (originName == null || originName == "")
            {
                envList.Add(new EnvVar(newName, value));
            }
            else
            {
                var envVar = envList.First(delegate (EnvVar item)
                   {
                       return item.Key == originName;
                   });
                if (envVar != null)
                {
                    envVar.Key = newName;
                    envVar.Value = value;
                }
            }
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(EnvList)));
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "EnviromentVariablesFile(*.env)|*.env"
            };
            var result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = saveFileDialog.OpenFile();
                formatter.Serialize(stream, EnvList);
                stream.Close();
            }

        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("当前未保存修改将会丢失，是否继续？", "警告", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            if (result != MessageBoxResult.OK)
            {
                return;
            }
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "EnviromentVariablesFile(*.env)|*.env"
            };
            var res = openFileDialog.ShowDialog();
            if (res == true)
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = openFileDialog.OpenFile();
                EnvList = (ObservableCollection<EnvVar>)formatter.Deserialize(stream);
                stream.Close();
            }
        }

        private void AddVar(object sender, RoutedEventArgs e)
        {
            var newVar = new EnvVar("新建环境变量", "value");
            if (EnvList == null)
            {
                EnvList = new ObservableCollection<EnvVar>();
            }
            EnvList.Add(newVar);
        }

        private void DeleteVar(object sender, RoutedEventArgs e)
        {
            if (EnvListView.SelectedItem != null)
            {
                var result = MessageBox.Show("确定要删除该环境变量吗？", "警告", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
                if (result == MessageBoxResult.OK)
                {
                    EnvList.RemoveAt(EnvListView.SelectedIndex);
                }

            }
        }
    }
}
