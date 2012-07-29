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
    /// Interaction logic for Invader.xaml
    /// </summary>
    public partial class Invader : UserControl
    {
        //the class invader has three properties: flavour (which kind of alioen it is)
        //lives (how many lives it has)
        //points (how many points you get for killing it)
        //it does not contain a method for moving it as the invaders are moved as a whole.
        public int flavour = 1;
        public int lives = 1;
        public int points = 10;
        public Invader()
        {
            InitializeComponent();
        }




    }
}
