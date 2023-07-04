using Pairs;
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
    public partial class MainWindow  : Window
    {

        int rows;
        int cols;
        public MainWindow()
        {
            InitializeComponent();


        }

        private void submitInputButton_Click(object sender, RoutedEventArgs e)
        {
            rows = int.Parse(RowsTextBox.Text);
            cols = int.Parse(ColumnsTextBox.Text);
            int columnCount = cols;

            if ((rows * cols) % 2 == 0)
            {
               GameWindow game = new GameWindow(rows, cols, columnCount);
                game.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Please introduce an even number of cards !", "", MessageBoxButton.OK);
            }

        }


    }
}
