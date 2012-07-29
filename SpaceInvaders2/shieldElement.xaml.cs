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
    /// Interaction logic for shieldElement.xaml
    /// </summary>
    public partial class shieldElement : UserControl
    {
        //the shiled element has a property called intLives it can be hit 3 times beofre it eventually disappears.
        public int intLives = 3;
        public shieldElement()
        {
            InitializeComponent();
        }


    }
}
