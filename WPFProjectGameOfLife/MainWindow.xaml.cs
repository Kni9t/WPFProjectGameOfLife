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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPFProjectGameOfLife
{
    public partial class MainWindow : Window
    {
        public const int Edge = 10; // Длина ребра куба клетки
        int MapHeight, MapWidth;
        bool[][] Map; // Height на Width
        int StringCheck(int id)
        {
            if (id >= MapHeight) return 0;
            if (id < 0) return (MapHeight - 1);
            return id;
        }
        int ColumCheck(int id)
        {
            if (id >= MapWidth) return 0;
            if (id < 0) return (MapWidth - 1);
            return id;
        }
        void UpdateLogic()
        {
            bool[][] Buf = new bool[MapHeight][];

            for (int i = 0; i < MapHeight; i++)
            {
                Buf[i] = new bool[MapWidth];

                for (int j = 0; j < MapWidth; j++)
                {
                    int bufcount = 0;

                    if (Map[StringCheck(i - 1)][ColumCheck(j - 1)]) bufcount++;
                    if (Map[StringCheck(i - 1)][ColumCheck(j)]) bufcount++;
                    if (Map[StringCheck(i - 1)][ColumCheck(j + 1)]) bufcount++;

                    if (Map[StringCheck(i)][ColumCheck(j - 1)]) bufcount++;
                    if (Map[StringCheck(i)][ColumCheck(j + 1)]) bufcount++;

                    if (Map[StringCheck(i + 1)][ColumCheck(j - 1)]) bufcount++;
                    if (Map[StringCheck(i + 1)][ColumCheck(j)]) bufcount++;
                    if (Map[StringCheck(i + 1)][ColumCheck(j + 1)]) bufcount++;

                    if (Map[i][j] == false) // Стандарт
                    {
                        if (bufcount == 3) Buf[i][j] = true;
                        else Buf[i][j] = false;
                    }
                    else
                    {
                        if ((bufcount == 2) || (bufcount == 3)) Buf[i][j] = true;
                        else Buf[i][j] = false;
                    }
                }
            }

            Map = Buf;
        }
        void UpdatePrint()
        {
            Polygon bufPolygon;
            PointCollection bufPointCollection;

            MainScreen.Children.Clear();

            for (int i = 0; i < MapHeight; i++)
                for (int j = 0; j < MapWidth; j++)
                {
                    if (Map[i][j])
                    {
                        bufPolygon = new Polygon();
                        bufPointCollection = new PointCollection();

                        bufPointCollection.Add(new Point(Edge * j, Edge * i));
                        bufPointCollection.Add(new Point(Edge * j, Edge * i + Edge));
                        bufPointCollection.Add(new Point(Edge * j + Edge, Edge * i + Edge));
                        bufPointCollection.Add(new Point(Edge * j + Edge, Edge * i));
                        bufPolygon.Points = bufPointCollection;
                        bufPolygon.Fill = Brushes.White;

                        MainScreen.Children.Add(bufPolygon);
                    }
                }
            GC.Collect();
        }

        public MainWindow()
        {
            InitializeComponent();
            MapHeight = (int)(Window.Height / Edge);
            MapWidth = (int)(Window.Width / Edge);
            MainScreen.Background = Brushes.Black;

            Map = new bool[MapHeight][];

            for (int i = 0; i < MapHeight; i++)
            {
                Map[i] = new bool[MapWidth];

                for (int j = 0; j < MapWidth; j++)
                {
                    Map[i][j] = false;
                }
            }
            DispatcherTimer Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 33);
            Timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateLogic();
            UpdatePrint();
        }

        private void MouseClick(object sender, MouseEventArgs e)
        {
            Random R = new Random();

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                int PositionX = (int)e.GetPosition(this).X / Edge, PositionY = (int)e.GetPosition(this).Y / Edge;

                if (R.Next(0, 2) == 1) Map[StringCheck(PositionY - 1)][ColumCheck(PositionX - 1)] = true;
                if (R.Next(0, 2) == 1) Map[StringCheck(PositionY - 1)][ColumCheck(PositionX)] = true;
                if (R.Next(0, 2) == 1) Map[StringCheck(PositionY - 1)][ColumCheck(PositionX + 1)] = true;

                if (R.Next(0, 2) == 1) Map[StringCheck(PositionY)][ColumCheck(PositionX - 1)] = true;
                if (R.Next(0, 2) == 1) Map[StringCheck(PositionY)][ColumCheck(PositionX)] = true;
                if (R.Next(0, 2) == 1) Map[StringCheck(PositionY)][ColumCheck(PositionX + 1)] = true;

                if (R.Next(0, 2) == 1) Map[StringCheck(PositionY + 1)][ColumCheck(PositionX - 1)] = true;
                if (R.Next(0, 2) == 1) Map[StringCheck(PositionY + 1)][ColumCheck(PositionX)] = true;
                if (R.Next(0, 2) == 1) Map[StringCheck(PositionY + 1)][ColumCheck(PositionX + 1)] = true;
            }

            if (e.RightButton == MouseButtonState.Pressed)
            {
                int PositionX = (int)e.GetPosition(this).X / Edge, PositionY = (int)e.GetPosition(this).Y / Edge;

                if (R.Next(0, 2) == 1) Map[StringCheck(PositionY)][ColumCheck(PositionX)] = true;
            }
        }
    }
}
