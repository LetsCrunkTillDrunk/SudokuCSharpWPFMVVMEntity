using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace WpfMVVMSudoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

      // Обработка кнопки "Новая игра". Создание класса модели и инициализация поля
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BoardModelView board = new BoardModelView(grid1);
            DataContext = board;
            int levelMode = (rbEasy.IsChecked == true) ? 60 : ((rbMaster.IsChecked == true) ? 45 : 30);
            while (levelMode > 0)
            {
                Random r1 = new Random();
                int i = r1.Next(0, 81);
                if (board.Points.ElementAt(i).IsVisible == false)
                {
                    board.Points.ElementAt(i).IsVisible = true;
                    levelMode--;
                    board.Points.ElementAt(i).Current = board.Points.ElementAt(i).Value;
                }
            }
            grid1.UpdateLayout();
            board.PaintButtons();
        }
        //Обработка клика по кнопке поля в судоку. Инкрементация значения
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            if (((Point)b.Tag).IsVisible == true) return;
            b.Foreground = Brushes.Green;
            b.FontWeight = FontWeights.Normal;
            if ((int)b.Content == 9) { b.Content = 1; ((Point)b.Tag).Current = 1; }
            else
            {
                int i = Convert.ToInt32(b.Content);
                i++;
                b.Content = i;
                ((Point)b.Tag).Current = i;
            }
        }
    }
}
