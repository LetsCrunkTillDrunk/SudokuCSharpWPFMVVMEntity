using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WpfMVVMSudoku
{
    /// <summary>
    /// Interaction logic for SaveGameWindow.xaml
    /// </summary>
    /// 
    //Окно сохранения игры. По сути только запрашивает идентификатор для сохранения у игрока и сохраняет в своем свойстве
    public partial class SaveGameWindow : Window
    {
        public string SaveName { get; set; }
        public SaveGameWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (tbSave.Text != String.Empty)
            {
                SaveName = tbSave.Text;
                this.DialogResult = true;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
