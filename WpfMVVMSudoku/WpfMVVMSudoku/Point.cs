using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMVVMSudoku
{
    //Модель клетки судоку
    public class Point:INotifyPropertyChanged
    {
        //Координаты клетки. Нужны для правильной инициализации поля при загрузке сохранения
        private int posX;
        private int posY;
        //Правильное значение клетки
        private int value;
        //Текущее значение 
        private int current;
        //Является ли клетка изначально открытой игроку
        private bool isVisible;

     
        public int PosX {
            get { return posX; }
            set { posX = value; OnPropertyChanged("PosX");
            }
        }
        public int PosY {
            get { return posY; }
            set { posY = value; OnPropertyChanged("PosY"); }
        }
        public int Value {
            get { return value; }
            set { this.value = value; OnPropertyChanged("Value"); }
        }
        public int Current {
            get { return current; }
            set { current = value; OnPropertyChanged("Current"); }
        }

        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; OnPropertyChanged("IsVisible"); }
        }

        public int Id { get; set; }
        public int? IdSave { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string arg)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(arg));
        }


    }
}
