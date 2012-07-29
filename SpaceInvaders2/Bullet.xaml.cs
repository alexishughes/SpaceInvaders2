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
    /// Interaction logic for Bullet.xaml
    /// </summary>
    public partial class Bullet : UserControl
    {
        public Bullet()
        {
            InitializeComponent();
        }
        //the bullet has a moveBullet method which moves it up by the number of pixels specified
        public void MoveBullet(int intBulletSpeed)
        {
            Canvas.SetTop(this, Canvas.GetTop(this) - intBulletSpeed);
        }
    }
}
