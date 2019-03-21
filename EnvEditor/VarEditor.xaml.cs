using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EnvEditor
{
    /// <summary>
    /// VarEditor.xaml 的交互逻辑
    /// </summary>
    public partial class VarEditor : Window, INotifyPropertyChanged
    {
        private string originName;
        private string varName;
        private DataTable lines;

        public event PropertyChangedEventHandler PropertyChanged;

        public string VarName
        {
            get
            {
                return this.varName;
            }
            set
            {
                if (this.varName != value)
                {
                    this.varName = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(VarName)));
                }
            }
        }

        public DataTable Lines
        {
            get
            {
                return this.lines;
            }
            set
            {
                if (this.lines != value)
                {
                    this.lines = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Lines)));
                }
            }
        }


        public VarEditor(string name, List<string> lines)
        {
            this.DataContext = this;
            InitializeComponent();

            this.originName = name;
            this.VarName = name;
            DataTable dt = new DataTable();
            dt.Columns.Add("line", typeof(string));
            foreach(var item in lines)
            {
                dt.Rows.Add(item);
            }
            this.Lines = dt;
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach(DataRow item in Lines.Rows)
            {
                sb.Append(item.Field<string>("line"));
                sb.Append(";");
            }
            sb.Remove(sb.Length - 1, 1);
            ((MainWindow)this.Owner).EditingResult(originName, this.VarName, sb.ToString());
            this.Close();
        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            if (dataGridView.SelectedItem != null && dataGridView.SelectedIndex < Lines.Rows.Count)
            {
                Lines.Rows.RemoveAt(dataGridView.SelectedIndex);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
