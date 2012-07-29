using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpaceInvaders2
{
    /// <summary>
    /// Interaction logic for Bomb.xaml
    /// </summary>
    public partial class Bomb : UserControl
    {
        public Bomb()
        {
            InitializeComponent();
        }
        //the bomb has a method that moves it down each move.
        public void MoveBomb(int intBombSpeed)
        {
            Canvas.SetTop(this,Canvas.GetTop(this)+intBombSpeed);

        }
    }
}
