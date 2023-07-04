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
using System.Windows.Threading;

namespace Pairs
{
    public static class ObservableCollectionExtensions
    {
        private static readonly Random _random = new Random();

        public static void Shuffle<T>(this ObservableCollection<T> collection)
        {
            int n = collection.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                T value = collection[k];
                collection[k] = collection[n];
                collection[n] = value;
            }
        }
    }

    public static class ListExtensions
    {
        private static readonly Random _random = new Random();

        public static void ListShuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
    public partial class GameWindow : Window
    {
        int level;
        int rows;
        int cols;
        private int clickCounter;
        public GameWindow(int rows, int cols, int columnCount)
        {
            InitializeComponent();
            setText1();
            gameTime.Start();
            CreateGameMatrix(rows, cols);
            timer = 150;
            CustomGameTimer.Text = "You have " + timer + " seconds left";
            this.level = 1;
            gameTime.Interval = TimeSpan.FromSeconds(1);
            gameTime.Tick += TimeDecrease;
            this.rows = rows;
            this.cols = cols;
            this.columnCount = columnCount;
        }

        ObservableCollection<BitmapImage> gameImages = new ObservableCollection<BitmapImage>
        {
            new BitmapImage(new Uri(@"/Images/3.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/Images/6.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/Images/9.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/Images/24.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/Images/25.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/Images/59.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/Images/94.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/Images/157.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/Images/197.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/Images/214.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/Images/245.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/Images/260.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/Images/306.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/Images/359.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/Images/373.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/Images/470.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/Images/483.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/Images/426.png", UriKind.Relative)),


        };

        List<BitmapImage> gridList;

        private void CreateGameMatrix(int n_row, int m_col)
        {
            clickCounter = 0;
            gameImages.Shuffle();
            ObservableCollection<BitmapImage> randomisedImages = new ObservableCollection<BitmapImage>(gameImages.Take(n_row * m_col / 2));
            gridList = randomisedImages.Concat(randomisedImages).ToList();
            gridList.ListShuffle();
            CustomGameGrid.Children.Clear();
            CustomGameGrid.RowDefinitions.Clear();
            CustomGameGrid.ColumnDefinitions.Clear();

            for (int it = 0; it < n_row + m_col; it++)
            {
                CustomGameGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                CustomGameGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }

            CustomGameGrid.HorizontalAlignment = HorizontalAlignment.Center;
            CustomGameGrid.VerticalAlignment = VerticalAlignment.Center;

            for (int rows = 0; rows < n_row; rows++)
            {
                for (int cols = 0; cols < m_col; cols++)
                {
                    Button button = new Button();
                    button.Click += new RoutedEventHandler(ImageButtonClick);

                    button.Width = 50;
                    button.Height = 50;

                    Grid.SetRow(button, rows);
                    Grid.SetColumn(button, cols);

                    CustomGameGrid.Children.Add(button);
                }
            }
        }
        private bool _isFirstTile = true;
        private Tuple<Button, string> _firstButton;
        private Tuple<Button, string> _secondButton;
        private int columnCount;

        private async void ImageButtonClick(object sender, RoutedEventArgs e)
        {

            Button button = (Button)sender;
            int row = Grid.GetRow(button);
            int column = Grid.GetColumn(button);

            BitmapImage image = gridList[row * columnCount + column];
            Image buttonImage = new Image();
            buttonImage.Source = image;

            button.Content = buttonImage;

            if (_isFirstTile)
            {
                _firstButton = new Tuple<Button, string>(button, buttonImage.Source.ToString());
                _isFirstTile = false;
            }

            else if (button != _firstButton.Item1)
            {
                _secondButton = new Tuple<Button, string>(button, buttonImage.Source.ToString());

                if (_firstButton.Item2 != null && _secondButton.Item2 != null)
                {
                    if (_firstButton.Item2.Equals(_secondButton.Item2) == true)
                    {
                        await Task.Delay(500);

                        _firstButton.Item1.Visibility = Visibility.Hidden;
                        _secondButton.Item1.Visibility = Visibility.Hidden;
                        clickCounter += 2;
                    }
                    else
                    {
                        await Task.Delay(500);

                        _firstButton.Item1.Content = new Image();
                        _secondButton.Item1.Content = new Image();
                    }

                    _isFirstTile = true;
                }
            }
            if (clickCounter == gridList.Count)
            {
                if (level == 3)
                {
                    await Task.Delay(500);
                    gameTime.Stop();
                    MessageBox.Show("Congrats,you won !", "", MessageBoxButton.OK);
                    this.Close();
                }
                if (level == 2)
                {
                    setText3();
                    await Task.Delay(500);
                    CreateGameMatrix(rows, cols);
                    level = 3;
                }
                if (level == 1)
                {
                    setText2();
                    await Task.Delay(500);
                    CreateGameMatrix(rows, cols);
                    level = 2;
                }
            }
        }

        private void setText1()
        {
            CustomGameBlockLevel.Text = "Level 1";
        }

        private void setText2()
        {
            CustomGameBlockLevel.Text = "Level 2";
        }

        private void setText3()
        {
            CustomGameBlockLevel.Text = "Level 3";
        }

        private int timer = 150;
        DispatcherTimer gameTime = new DispatcherTimer();
        private void TimeDecrease(object sender, EventArgs e)
        {
            timer--;
            CustomGameTimer.Text = $"You have {timer} seconds left";
            if (timer == 0)
            {
                gameTime.Stop();
                MessageBox.Show("You lost !", "", MessageBoxButton.OK);
                this.Close();
            }
        }

    }
}
