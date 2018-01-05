using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WpfMVVMSudoku
{
    public class BoardModelView:INotifyPropertyChanged
    {
        private UserContext db;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Point> Points { get; set; }

        private SudokuCommand saveCommand;
        private SudokuCommand loadCommand;
        private SudokuCommand checkCommand;


        public System.Windows.Controls.Grid grid1 { get; set; }
        //Команда сохранения игры
        public SudokuCommand SaveCommand
        {
            get
            {
                return saveCommand ?? (saveCommand = new SudokuCommand((obj) =>
                {
                    SaveGameWindow sgw = new SaveGameWindow();
                    
                    bool? result = sgw.ShowDialog();
                    string name = sgw.SaveName;
                    if (result == true)
                    {
                        Save save = new Save { Name = name };
                        db.Saves.Add(save);
                        db.SaveChanges();
                        Save saveId = db.Saves.Where(i => i.Name == name).FirstOrDefault();
                        foreach (Point p in Points)
                        {
                            p.IdSave = saveId.Id;
                        }
                        db.Points.AddRange(Points);
                        db.SaveChanges();
                    }
                },
                obj =>
                {
                    return true;
                }
          ));
            }
        }
        //Соответственно, загрузка сохраненной игры
        public SudokuCommand LoadCommand
        {
            get
            {
                return loadCommand ?? (loadCommand = new SudokuCommand((obj) =>
                {
                    List<string> names = db.Saves.Select(i => i.Name).Distinct().ToList<string>();
                    LoadGameWindow lgw = new LoadGameWindow(names, db);
                    bool? result = lgw.ShowDialog();
                    int? saveId = lgw.Selected;
                    if (saveId == null) return;
                    if (result == true)
                    {
                        var pnts = db.Points.Where(i => i.IdSave == saveId).ToList();

                        foreach (var p in Points)
                        {
                            int x = p.PosX;
                            int y = p.PosY;
                            var savedItem = pnts.Where(i => i.PosX == x && i.PosY == y).FirstOrDefault();
                            p.Current = savedItem.Current;
                            p.IsVisible = savedItem.IsVisible;
                            p.Value = savedItem.Value;

                        }
                        PaintButtons();

                    }
                },
                obj =>
                {
                    return true;
                }
          ));
            }
        }

        //Проверка поля. В начале проверяются валидность значений ячеек. Потом совпадения по горизонтали, вертикали и по клеткам
        public SudokuCommand CheckCommand
        {
            get
            {
                return checkCommand ?? (checkCommand = new SudokuCommand((obj) =>
                {
                    var buttons = FindVisualChildren<Button>(grid1);
                    foreach (var button in buttons)
                    {
                        Point p = (Point)button.Tag;
                        Point p1 = Points.Where(l1 => l1.PosX == p.PosX && l1.PosY == p.PosY).FirstOrDefault();
                        if(p.Current == 0)
                        {
                            button.Foreground = Brushes.Red;
                            button.FontWeight = FontWeights.ExtraBold;
                            Animation(button);
                            return;
                        }
                    }
                    for (int i = 0; i < 9; i++)
                    {
                        var rowPoints = Points.Where(k => k.PosX == i).ToList();
                        for (int j = 0; j < 9; j++)
                        {
                            for (int k = j + 1; k < 9; k++)
                            {
                                if (rowPoints[j].Current == rowPoints[k].Current)
                                {
                                    var b1 = buttons.Where(l1 => ((Point)l1.Tag).PosX == rowPoints[j].PosX && ((Point)l1.Tag).PosY == rowPoints[j].PosY).FirstOrDefault();
                                    var b2 = buttons.Where(l1 => ((Point)l1.Tag).PosX == rowPoints[k].PosX && ((Point)l1.Tag).PosY == rowPoints[k].PosY).FirstOrDefault();
                                    if (b1 != null && b2 != null)
                                    {
                                        MakeRed(b1, b2);
                                        Animation(b1, b2);
                                        return;
                                    }

                                }
                            }
                        }
                    }
                    for (int i = 0; i < 9; i++)
                    {
                        var colPoints = Points.Where(k => k.PosY == i).ToList();
                        for (int j = 0; j < 9; j++)
                        {
                            for (int k = j + 1; k < 9; k++)
                            {
                                if (colPoints[j].Current == colPoints[k].Current)
                                {
                                    var b1 = buttons.Where(l1 => ((Point)l1.Tag).PosX == colPoints[j].PosX && ((Point)l1.Tag).PosY == colPoints[j].PosY).FirstOrDefault();
                                    var b2 = buttons.Where(l1 => ((Point)l1.Tag).PosX == colPoints[k].PosX && ((Point)l1.Tag).PosY == colPoints[k].PosY).FirstOrDefault();
                                    if (b1 != null && b2 != null)
                                    {
                                        MakeRed(b1, b2);
                                        Animation(b1, b2);
                                        return;
                                    }

                                }
                            }
                        }
                    }

                    var squarePoints = Points.Where(k => k.PosX >= 0 && k.PosX <=2 && k.PosY >=0 && k.PosY<=2).ToList();
                    for (int j = 0; j < 6; j++)
                    {
                        for (int k = j + 1; k < 6; k++)
                        {
                            if (squarePoints[j].Current == squarePoints[k].Current)
                            {
                                var b1 = buttons.Where(l1 => ((Point)l1.Tag).PosX == squarePoints[j].PosX && ((Point)l1.Tag).PosY == squarePoints[j].PosY).FirstOrDefault();
                                var b2 = buttons.Where(l1 => ((Point)l1.Tag).PosX == squarePoints[k].PosX && ((Point)l1.Tag).PosY == squarePoints[k].PosY).FirstOrDefault();
                                if (b1 != null && b2 != null)
                                {
                                    MakeRed(b1, b2);
                                    Animation(b1, b2);
                                    return;
                                }

                            }
                        }
                    }


                    var squarePoints1 = Points.Where(k => k.PosX >= 0 && k.PosX <= 2 && k.PosY >= 3 && k.PosY <= 5).ToList();
                    for (int j = 0; j < 6; j++)
                    {
                        for (int k = j + 1; k < 6; k++)
                        {
                            if (squarePoints1[j].Current == squarePoints1[k].Current)
                            {
                                var b1 = buttons.Where(l1 => ((Point)l1.Tag).PosX == squarePoints1[j].PosX && ((Point)l1.Tag).PosY == squarePoints1[j].PosY).FirstOrDefault();
                                var b2 = buttons.Where(l1 => ((Point)l1.Tag).PosX == squarePoints1[k].PosX && ((Point)l1.Tag).PosY == squarePoints1[k].PosY).FirstOrDefault();
                                if (b1 != null && b2 != null)
                                {
                                    MakeRed(b1, b2);
                                    Animation(b1, b2);
                                    return;
                                }

                            }
                        }
                    }

                    var squarePoints2 = Points.Where(k => k.PosX >= 0 && k.PosX <= 2 && k.PosY >= 6 && k.PosY <= 8).ToList();
                    for (int j = 0; j < 6; j++)
                    {
                        for (int k = j + 1; k < 6; k++)
                        {
                            if (squarePoints2[j].Current == squarePoints2[k].Current)
                            {
                                var b1 = buttons.Where(l1 => ((Point)l1.Tag).PosX == squarePoints2[j].PosX && ((Point)l1.Tag).PosY == squarePoints2[j].PosY).FirstOrDefault();
                                var b2 = buttons.Where(l1 => ((Point)l1.Tag).PosX == squarePoints2[k].PosX && ((Point)l1.Tag).PosY == squarePoints2[k].PosY).FirstOrDefault();
                                if (b1 != null && b2 != null)
                                {
                                    MakeRed(b1, b2);
                                    Animation(b1, b2);
                                    return;
                                }

                            }
                        }
                    }

                    var squarePoints3 = Points.Where(k => k.PosX >= 3 && k.PosX <= 5 && k.PosY >= 0 && k.PosY <= 2).ToList();
                    for (int j = 0; j < 6; j++)
                    {
                        for (int k = j + 1; k < 6; k++)
                        {
                            if (squarePoints3[j].Current == squarePoints3[k].Current)
                            {
                                var b1 = buttons.Where(l1 => ((Point)l1.Tag).PosX == squarePoints3[j].PosX && ((Point)l1.Tag).PosY == squarePoints3[j].PosY).FirstOrDefault();
                                var b2 = buttons.Where(l1 => ((Point)l1.Tag).PosX == squarePoints3[k].PosX && ((Point)l1.Tag).PosY == squarePoints3[k].PosY).FirstOrDefault();
                                if (b1 != null && b2 != null)
                                {
                                    MakeRed(b1, b2);
                                    Animation(b1, b2);
                                    return;
                                }

                            }
                        }
                    }

                    var squarePoints4 = Points.Where(k => k.PosX >= 3 && k.PosX <= 5 && k.PosY >= 3 && k.PosY <= 5).ToList();
                    for (int j = 0; j < 6; j++)
                    {
                        for (int k = j + 1; k < 6; k++)
                        {
                            if (squarePoints4[j].Current == squarePoints4[k].Current)
                            {
                                var b1 = buttons.Where(l1 => ((Point)l1.Tag).PosX == squarePoints4[j].PosX && ((Point)l1.Tag).PosY == squarePoints4[j].PosY).FirstOrDefault();
                                var b2 = buttons.Where(l1 => ((Point)l1.Tag).PosX == squarePoints4[k].PosX && ((Point)l1.Tag).PosY == squarePoints4[k].PosY).FirstOrDefault();
                                if (b1 != null && b2 != null)
                                {
                                    MakeRed(b1, b2);
                                    Animation(b1, b2);
                                    return;
                                }

                            }
                        }
                    }

                    var squarePoints5 = Points.Where(k => k.PosX >= 3 && k.PosX <= 5 && k.PosY >= 6 && k.PosY <= 8).ToList();
                    for (int j = 0; j < 6; j++)
                    {
                        for (int k = j + 1; k < 6; k++)
                        {
                            if (squarePoints5[j].Current == squarePoints5[k].Current)
                            {
                                var b1 = buttons.Where(l1 => ((Point)l1.Tag).PosX == squarePoints5[j].PosX && ((Point)l1.Tag).PosY == squarePoints5[j].PosY).FirstOrDefault();
                                var b2 = buttons.Where(l1 => ((Point)l1.Tag).PosX == squarePoints5[k].PosX && ((Point)l1.Tag).PosY == squarePoints5[k].PosY).FirstOrDefault();
                                if (b1 != null && b2 != null)
                                {
                                    MakeRed(b1, b2);
                                    Animation(b1, b2);
                                    return;
                                }

                            }
                        }
                    }

                    var squarePoints6 = Points.Where(k => k.PosX >= 6 && k.PosX <= 8 && k.PosY >= 0 && k.PosY <= 2).ToList();
                    for (int j = 0; j < 6; j++)
                    {
                        for (int k = j + 1; k < 6; k++)
                        {
                            if (squarePoints6[j].Current == squarePoints6[k].Current)
                            {
                                var b1 = buttons.Where(l1 => ((Point)l1.Tag).PosX == squarePoints6[j].PosX && ((Point)l1.Tag).PosY == squarePoints6[j].PosY).FirstOrDefault();
                                var b2 = buttons.Where(l1 => ((Point)l1.Tag).PosX == squarePoints6[k].PosX && ((Point)l1.Tag).PosY == squarePoints6[k].PosY).FirstOrDefault();
                                if (b1 != null && b2 != null)
                                {
                                    MakeRed(b1, b2);
                                    Animation(b1, b2);
                                    return;
                                }

                            }
                        }
                    }

                    var squarePoints7 = Points.Where(k => k.PosX >= 6 && k.PosX <= 8 && k.PosY >= 3 && k.PosY <= 5).ToList();
                    for (int j = 0; j < 6; j++)
                    {
                        for (int k = j + 1; k < 6; k++)
                        {
                            if (squarePoints7[j].Current == squarePoints7[k].Current)
                            {
                                var b1 = buttons.Where(l1 => ((Point)l1.Tag).PosX == squarePoints7[j].PosX && ((Point)l1.Tag).PosY == squarePoints7[j].PosY).FirstOrDefault();
                                var b2 = buttons.Where(l1 => ((Point)l1.Tag).PosX == squarePoints7[k].PosX && ((Point)l1.Tag).PosY == squarePoints7[k].PosY).FirstOrDefault();
                                if (b1 != null && b2 != null)
                                {
                                    MakeRed(b1, b2);
                                    Animation(b1, b2);
                                    return;
                                }

                            }
                        }
                    }
                    var squarePoints8 = Points.Where(k => k.PosX >= 6 && k.PosX <= 8 && k.PosY >= 6 && k.PosY <= 8).ToList();
                    for (int j = 0; j < 6; j++)
                    {
                        for (int k = j + 1; k < 6; k++)
                        {
                            if (squarePoints8[j].Current == squarePoints8[k].Current)
                            {
                                var b1 = buttons.Where(l1 => ((Point)l1.Tag).PosX == squarePoints8[j].PosX && ((Point)l1.Tag).PosY == squarePoints8[j].PosY).FirstOrDefault();
                                var b2 = buttons.Where(l1 => ((Point)l1.Tag).PosX == squarePoints8[k].PosX && ((Point)l1.Tag).PosY == squarePoints8[k].PosY).FirstOrDefault();
                                if (b1 != null && b2 != null)
                                {
                                    MakeRed(b1, b2);
                                    Animation(b1, b2);
                                    return;
                                }

                            }
                        }
                    }

                    MessageBox.Show("Паззл собран!");

                },
                obj =>
                {
                    return true;
                }
          ));
            }
        }
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        
        public BoardModelView(System.Windows.Controls.Grid mv)
        {
            grid1 = mv;
            db = new UserContext();
            Points = new ObservableCollection<Point>();
            SudokuBoard board = new SudokuBoard();
            int[,] b = board.GetBoard();
            
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Points.Add(new Point { PosX = i, PosY = j, Value = b[i, j], Current = 0, IsVisible = false, IdSave = 0 });
                }
            }
        }
        //Раскраска квадратов поля
        public void PaintButtons()
        {
            grid1.BeginInit();
            int count = 0;
            int count2 = 0;
            bool flag = true;
            foreach (Button b in FindVisualChildren<Button>(grid1))
            {
                if (flag)
                {
                    if (count >= 0)
                    {
                        b.Background = Brushes.White;

                    }
                    else
                    {
                        b.Background = Brushes.Gray;
                    }
                }
                else
                {
                    if (count >= 0)
                    {
                        b.Background = Brushes.Gray;
                    }
                    else
                    {
                        b.Background = Brushes.White;
                    }
                }
                count++;
                count2++;
                if (count == 3) { count = -3; }
                if (count2 == 9)
                { count2 = (flag == true) ? 0 : -9; flag = !flag; }

            }
            foreach (Button b in FindVisualChildren<Button>(grid1))
            {
                bool p = ((Point)b.Tag).IsVisible;
                if (p == true)
                {
                    b.Foreground = Brushes.DarkBlue;
                    b.FontWeight = FontWeights.ExtraBold;
                }
                else
                {
                    b.Foreground = Brushes.Green;
                    b.FontWeight = FontWeights.Normal;
                }
            }
            grid1.EndInit();
            grid1.UpdateLayout();
        }
        //Служебная функция для получения кнопок поля
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
        //Небольшая анимация
        private void Animation(Button b, Button b1 = null)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = b.FontSize;
            da.To = 20;
            da.Duration = TimeSpan.FromSeconds(1);
            da.AutoReverse = true;
            da.RepeatBehavior = new RepeatBehavior(3);
            if (b1 != null)
            {
                b1.BeginAnimation(Button.FontSizeProperty, da);
            }
            b.BeginAnimation(Button.FontSizeProperty, da);
        }
        private void MakeRed(Button b1, Button b2)
        {
            if (((Point)b1.Tag).IsVisible == false)
            {
                b1.Foreground = Brushes.Red;
                b1.FontWeight = FontWeights.ExtraBold;
            }
            if (((Point)b2.Tag).IsVisible == false)
            {
                b2.Foreground = Brushes.Red;
                b2.FontWeight = FontWeights.ExtraBold;
            }
        }
    }
}
