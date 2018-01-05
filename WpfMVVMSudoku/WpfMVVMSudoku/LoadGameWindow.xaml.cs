using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for LoadGameWindow.xaml
    /// </summary>
    /// 
    //Окно загрузки игры
    public partial class LoadGameWindow : Window
    {
        private ObservableCollection<string> names = new ObservableCollection<string>();
        //Ссылка на контекст для работы с БД
        private UserContext db;
       
        public ObservableCollection<String> SaveNames {
            get { return names; }
            set { names = value; } }

        public int? Selected { get; set; }
        public LoadGameWindow(List<string>n, UserContext db)
        {
            InitializeComponent();
            
            foreach(var i in n)
            {
                SaveNames.Add(i);
            }
            this.db = db;
            listBox1.ItemsSource = SaveNames;
            
        }
        //Загрузка. Просто инициализация свойства класса, загрузка происходит в классе модели
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                string s = listBox1.SelectedItem.ToString();
                Selected = db.Saves.Where(i => i.Name == s).Select(j => j.Id).FirstOrDefault();
                this.DialogResult = true;
                this.Close();
            }
        }
        //Удаление сохранения и записей БД

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                string s = listBox1.SelectedItem.ToString();
                int? id = db.Saves.Where(i => i.Name == s).Select(j=> j.Id).FirstOrDefault();
                if (id != null)
                {
                    db.Points.RemoveRange(db.Points.Where(i => i.IdSave == id));
                    db.Saves.RemoveRange(db.Saves.Where(i => i.Id == id));
                    db.SaveChangesAsync();
                    SaveNames.RemoveAt(listBox1.SelectedIndex);
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
