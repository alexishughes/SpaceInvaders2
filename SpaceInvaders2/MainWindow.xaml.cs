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
using System.Windows.Threading;
using System.Threading;

namespace SpaceInvaders2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //setting up the variables
        DispatcherTimer myTimer = new DispatcherTimer();
        //int refresh rate is th number of milliseconds between each iteration of the game loop.
        int intRefreshRate = 20;
        //blnleft, blnright, blnspace represent whether the keys are being pressed and are turned 
        //on and off (true/false) by the keyup keydown events in mainWindow
        bool blnLeft = false;
        bool blnRight = false;
        bool blnSpace = false;
        //bln game started inicates whether the game is in play
        bool blnGameStarted = false;
        //intscore is the current score
        int intScore = 0;
        //intHighScore is the highest score of the session.
        int intHighScore=0;
        //intLives is the number of lives the play has left. (note this could be done by counting the Lives list of images)
        int intLives= 5;
        //dblMaxLeft and dblMaxRight find the furthest left and furthest right of the aliens and gets their GetLeft property 
        //(the aliens don't change direction until the outside one hits the edge of the screen)
        double dblMaxLeft;
        double dblMinLeft;
        //blnMoveInvDown is true when the aliens reach the edge of the screen when they move down
        bool blnMoveInvDown = false;
        //dblInvSpeed is the number of pixels each alien moves each tick
        double dblInvSpeed = 4;
        // dblInvMove is how many pixels they move down when they reach the edgeof the screen.
        double dblInvMove = 4;
        // dblCurrentleft is the GetLeft property of the gunner.
        double dblCurrentLeft = 0;
        // dblGunnerSpeed is the number of pixel the gunner moves per tick.
        double dblGunnerSpeed = 10;
        //int aniCount is the number of ticks between changing the image of the invaders
        int intAniCount = 0;

        int intSwitch = 15;
        //intBulletRegcharge is the number of ticks until you can fire another bullet.
        int intBulletRecharge=7;
        //intBulletFired is the number of ticks since the last bullet was fired.
        int intBulletFired=0;
        //intBulletSpeed is the number of pixels the bullet moves each time
        int intBulletSpeed = 10;
        //intBombSpeed is the number of pixels the bomb moves each tick
        int intBombSpeed = 2;
        //intBombRate. Every tick, each invader picks a random number between 0 and intBombRate. 
        //if this number is 1 then the invader creates a new bomb instance and drops it.
        int intBombRate = 1000;
        //intLevel is the current level (initially 1)
        int intLevel;
        //intBonus is the number of bonus points granted for finishing the level.
        int intBonus;
        //initialize bitmaps for the user controls.

        //bitshielda,b,c are the shield element (a small rectangle) that degrades with the number of hits.
        BitmapImage bitshielda = new BitmapImage();
        BitmapImage bitshieldb = new BitmapImage();
        BitmapImage bitshieldc = new BitmapImage();

        //bitInv1a..3b are 2step animations for 3 types of invaders.
        BitmapImage bitInv1a = new BitmapImage();
        BitmapImage bitInv1b = new BitmapImage();
        BitmapImage bitInv2a = new BitmapImage();
        BitmapImage bitInv2b = new BitmapImage();
        BitmapImage bitInv3a = new BitmapImage();
        BitmapImage bitInv3b = new BitmapImage();
        BitmapImage bitBullet = new BitmapImage();
        BitmapImage bitGunner = new BitmapImage();
        



        //random number generator
        Random randomObj = new Random();
        //Text lists of objects for the game aliens bombs and bullets
        List<Invader> Aliens = new List<Invader>();
        List<Bomb> Bombs = new List<Bomb>();
        List<Bullet> Bullets = new List<Bullet>();
        //because you cannot remove items from a list whilst you are working through it a dead list is created
        //the dead objects are then removed from the list after all the hit tests have completed.
        List<Invader> DeadAliens = new List<Invader>();
        List<Bullet> DeadBullets = new List<Bullet>();
        List<Bomb> DeadBombs = new List<Bomb>();
        //the shields are actually made of many rectangles which each have 3 lives and are contained in a list.
        List<shieldElement> Shields = new List<shieldElement>();
        List<shieldElement> DeadShieldElements = new List<shieldElement>();
        //finally the spare gunner ships are stored in this list to make it easy to remove the newest one.
        List<Image> Lives = new List<Image>();



        public MainWindow()
        {   
            //draw the MainWindow
            InitializeComponent();
            
            //these set up bitmap variables for all the graphics the program will use from the embedded .png files
            bitshielda.BeginInit();
            bitshielda.UriSource = new Uri("shielda.png", UriKind.Relative);
            bitshielda.EndInit();
            bitshieldb.BeginInit();
            bitshieldb.UriSource = new Uri("shieldb.png", UriKind.Relative);
            bitshieldb.EndInit();
            bitshieldc.BeginInit();
            bitshieldc.UriSource = new Uri("shieldc.png", UriKind.Relative);
            bitshieldc.EndInit();

            bitInv1a.BeginInit();
            bitInv1a.UriSource = new Uri("inv1a.png", UriKind.Relative);
            bitInv1a.EndInit();


            bitInv1b.BeginInit();
            bitInv1b.UriSource = new Uri("inv1b.png", UriKind.Relative);
            bitInv1b.EndInit();


            bitInv2a.BeginInit();
            bitInv2a.UriSource = new Uri("inv2a.png", UriKind.Relative);
            bitInv2a.EndInit();


            bitInv2b.BeginInit();
            bitInv2b.UriSource = new Uri("inv2b.png", UriKind.Relative);
            bitInv2b.EndInit();


            bitInv3a.BeginInit();
            bitInv3a.UriSource = new Uri("inv3a.png", UriKind.Relative);
            bitInv3a.EndInit();


            bitInv3b.BeginInit();
            bitInv3b.UriSource = new Uri("inv3b.png", UriKind.Relative);
            bitInv3b.EndInit();

            bitBullet.BeginInit();
            bitBullet.UriSource = new Uri("bullet.png", UriKind.Relative);
            bitBullet.EndInit();

            bitGunner.BeginInit();
            bitGunner.UriSource = new Uri("gunner.png", UriKind.Relative);
            bitGunner.EndInit();

            //adds a new Event Handler for myTimer
            myTimer.Tick += new EventHandler(myTimer_Elaspsed);
            myTimer.Interval = TimeSpan.FromMilliseconds(intRefreshRate);

           
            //initialize the game
            drawAliens();
            drawSpareGunners();
            drawShields();





        }
        //every time the myTimer has a tick event, call the gameLoop() method
        private void myTimer_Elaspsed(object source, EventArgs e)
        {
            gameLoop();
        }
        //The startGame() method hides the Game Over and press space to begin labels and initializes the game board.
        private void startGame()
        {   lblGameOver.Visibility=Visibility.Hidden;
            lblPressSpace.Visibility = Visibility.Hidden;
            myTimer.Start();
            intScore = 0;
            intLevel = 1;
            intLives = 5;
            lblScore.Content = "Score : " + intScore.ToString();
            lblLevel.Content = "Level : " + intLevel.ToString();
            // a random number is generated between 0 and BombRate for each alien at each clock tick. 
            // If the number is a 1 a bomb is dropped
            // when the player goes up a level BombRate is reduced by 10% so there are more bombs.

            intBombRate = 1000;

            //dblInvSpeed is the number of pixels the alien moves each loop.
            dblInvSpeed = 4;
            //if the game is being re-started all invaders, spare ships and shields will be re-drawn.
            foreach (Invader thisInvader in Aliens)
            { cnvSpace.Children.Remove(thisInvader); }
            Aliens.Clear();
            drawAliens();
            foreach (Image thisLife in Lives)
            { cnvSpace.Children.Remove(thisLife); }
            Lives.Clear();
            drawSpareGunners();
            foreach (shieldElement thisShieldElement in Shields)
            { cnvSpace.Children.Remove(thisShieldElement); }
            Shields.Clear();
            drawShields();
        }
        //stops the game
        private void stopGame()
        {
            myTimer.Stop();
        }
        //this is the main game loop
        private void gameLoop()
        {
            moveAliens();
            moveGunner();


            //only allows a bullet to be fired if it is a certain number of 
            // timer ticks after the last bullet was fired
            if ((blnSpace)&&(intBulletFired==intBulletRecharge))
            { fireBullet();
            intBulletFired=0;
            }
            // increments intBulletFired if it is below the threashold.
            if (intBulletFired<intBulletRecharge){intBulletFired++;}

            //move bullets each bullet will be moved up a certain number of pixels
            moveBullets();
            //invaders drop bombs 
            invadersDropBombs();
            //move bombs
            moveBombs();
            //runs through the various hit tests
            doHitTests();
            //deletes bullets that have gone off the top of the screen
            loseBulletsOffScreen();
            //deletes bombs that have gone off the bottom of the screen
            loseBombsOffScreen();
            //animates the aliens
            if (intAniCount == intSwitch)
            { animateAliens(1); }
            if (intAniCount == intSwitch * 2)
            { animateAliens(2); intAniCount = 0; }

            intAniCount++;
        }
        //the method the draw the aliens at the start of the game
        private void drawAliens()
        {
            //first the embedded .png files are loaded into bitmap variables
            BitmapImage bitInv1a = new BitmapImage();
            bitInv1a.BeginInit();
            bitInv1a.UriSource = new Uri("inv1a.png", UriKind.Relative);
            bitInv1a.EndInit();

            BitmapImage bitInv1b = new BitmapImage();
            bitInv1b.BeginInit();
            bitInv1b.UriSource = new Uri("inv1b.png", UriKind.Relative);
            bitInv1b.EndInit();

            BitmapImage bitInv2a = new BitmapImage();
            bitInv2a.BeginInit();
            bitInv2a.UriSource = new Uri("inv2a.png", UriKind.Relative);
            bitInv2a.EndInit();

            BitmapImage bitInv2b = new BitmapImage();
            bitInv2b.BeginInit();
            bitInv2b.UriSource = new Uri("inv2b.png", UriKind.Relative);
            bitInv2b.EndInit();

            BitmapImage bitInv3a = new BitmapImage();
            bitInv3a.BeginInit();
            bitInv3a.UriSource = new Uri("inv3a.png", UriKind.Relative);
            bitInv3a.EndInit();

            BitmapImage bitInv3b = new BitmapImage();
            bitInv3b.BeginInit();
            bitInv3b.UriSource = new Uri("inv3b.png", UriKind.Relative);
            bitInv3b.EndInit();

            BitmapImage bitBullet = new BitmapImage();
            bitBullet.BeginInit();
            bitBullet.UriSource = new Uri("bullet.png", UriKind.Relative);
            bitBullet.EndInit();

            //five rows of aliens
            for (int j = 0; j < 5; j++)
            {
                //15 aliens per row
                for (int i = 0; i < 15; i++)
                {
                    //instanciates a new user control of the type Invader
                    Invader thisInvader = new Invader();

                    //using modular division aliens are created in 3 types
                    switch (j % 3)
                    {
                        case (0): thisInvader.image1.Source = bitInv1a; thisInvader.lives = 1; thisInvader.points = 100; thisInvader.flavour = 1; break;
                        case (1): thisInvader.image1.Source = bitInv2a; thisInvader.lives = 2; thisInvader.points = 200; thisInvader.flavour = 2; break;
                        case (2): thisInvader.image1.Source = bitInv3a; thisInvader.lives = 3; thisInvader.points = 300; thisInvader.flavour = 3; break;
                    }

                    //invader is added to the screen
                    cnvSpace.Children.Add(thisInvader);
                    //a reference to the invader is added to the typed list aliens
                    Aliens.Add(thisInvader);
                    //a horizontal gap of 5px between invaders (dimesnion is 30*30) plus an offset of 5px
                    Canvas.SetLeft(thisInvader, i * 35 + 5);
                    //vertical gap if 5px plus 40px offset
                    Canvas.SetTop(thisInvader, j * 35 + 40);
                    //z-index is set up so that game over will appear above the sprite
                    Canvas.SetZIndex(thisInvader, 0);

                }
            }
        }
        //the animation routine: when called with either intStep == 1 or 2
        private void animateAliens(int intStep)
        {
            foreach(Invader thisInvader in Aliens)
                if (intStep == 1)
                {
                    switch (thisInvader.flavour)
                    {
                        case (1): thisInvader.image1.Source = bitInv1a; break;
                        case (2): thisInvader.image1.Source = bitInv2a; break;
                        case (3): thisInvader.image1.Source = bitInv3a; break;

                    }
                }
                else
                {
                    switch (thisInvader.flavour)
                    {
                        case (1): thisInvader.image1.Source = bitInv1b; break;
                        case (2): thisInvader.image1.Source = bitInv2b; break;
                        case (3): thisInvader.image1.Source = bitInv3b; break;

                    }
                }
        }

        //draws the spare lives
        private void drawSpareGunners()
        {
            //four spare gunners
            for (int i = 0; i < 4; i++)
            {
                //handled as a list of images
                Image imgThisLife = new Image();
                imgThisLife.Source = bitGunner;
                imgThisLife.Width = 40;
                imgThisLife.Height = 20;
                //adds gunner life image to canvas and a reference is added to the lives list.
                cnvSpace.Children.Add(imgThisLife);
                Lives.Add(imgThisLife);
                Canvas.SetLeft(imgThisLife, i * 50 + 10);
                Canvas.SetTop(imgThisLife, 565);

            }
        }

        //The draw shields method
        private void drawShields()
        {
            //there are 4 shield 0 to 3
            for (int intShieldNo = 0; intShieldNo < 4; intShieldNo++)
            {
                //each shield has 4 rows 0 to 3
                for (int row = 0; row < 4; row++)
                {
                    //each row has 11 shield elements which are user controls
                    for (int x = 0; x < 11; x++)
                    {
                        //provided the element is not in the cut out area at the bottom centre of the shield,
                        //create the shield element
                        if (!((row>1)&&(x>2)&&(x<8)))
                        {
                        shieldElement thisShieldElement = new shieldElement();
                        Shields.Add(thisShieldElement);
                        cnvSpace.Children.Add(thisShieldElement);
                        //135 offset, shields at 125px interval, each element 7px width.
                        Canvas.SetLeft(thisShieldElement, 135+intShieldNo*125+x*7);
                        //475 offset from top of screen 10 height
                        Canvas.SetTop(thisShieldElement, 475+row*10);
                        }

                    }
                }



            }
        }

        //when a shield element gets hit it loses 1 of its 3 lives and gets redrawn
        private void reDrawShield(shieldElement thisShieldElement)
        {
            switch (thisShieldElement.intLives)
            {
                case (3): thisShieldElement.imgShield.Source = bitshielda; break;
                case (2): thisShieldElement.imgShield.Source = bitshieldb; break;
                case (1): thisShieldElement.imgShield.Source = bitshieldc; break;

            }
        }


        //the move aliens routine
        private void moveAliens()
        {
            //the first thing to do is find the GetLeft position of the leftmost and rightmost aliens.

            //so MaxLeft is initiallially set to zero then every time an alien is to the right of it it is reset ot that value.
            dblMaxLeft = 0;
            //similarly minleft is set to a number further right than any alien
            dblMinLeft = 800;

            //we run through all the invaders on the screen and if the aliens position is greater than maxleft, maxleft is updated
            //if less than current minleft, minleft is updated
            foreach (Invader thisInvader in Aliens)
            {

                    if (dblMaxLeft < Canvas.GetLeft(thisInvader))
                    { dblMaxLeft = Canvas.GetLeft(thisInvader); }
                    if (dblMinLeft > Canvas.GetLeft(thisInvader))
                    { dblMinLeft = Canvas.GetLeft(thisInvader); }

            }

            //if the column is going off the screen to the right the speed is reversed and a flag (blnMoveInvDown) is set to true so that next move the aliens are also lowered
            if (dblMaxLeft > 700)
            {
                dblInvMove = -dblInvSpeed;
                blnMoveInvDown = true;
            }

            //similarly if the column has reached the left of the screen speed is set to positive and the MoveDown flag set to true
            if (dblMinLeft < 0)
            {
                dblInvMove = dblInvSpeed;
                blnMoveInvDown = true;
            }
            //scrolls through all the invaders on the screen
            foreach (UIElement thisInvader in Aliens)
            {
                    //moves to the left or right appropriately
                    Canvas.SetLeft(thisInvader, Canvas.GetLeft(thisInvader) + dblInvMove);
                    
                    //if at screen edge moves down
                    if (blnMoveInvDown)
                    {
                        Canvas.SetTop(thisInvader, Canvas.GetTop(thisInvader) + 10);
                        if (Canvas.GetTop(thisInvader) > 445) { gameOver(); }
                    }


            }
            //once all invaders are moved move down flag set back to false if it was used
            blnMoveInvDown = false;
        }

        //routine to move the gunner
        private void moveGunner()
        {
            //using currentleft as a variable the gunner's p[osition is taken
            dblCurrentLeft = Canvas.GetLeft(imgGunner);
            // blnLeft and blnRight are switched on and off by the KeyUp and KeyDown events on the Main Window
            // here i am putting code in so that the gunner just stops if both left and right are pressed simultaneously.
            if (blnLeft && !blnRight) { dblCurrentLeft -= dblGunnerSpeed; }

            if (!blnLeft && blnRight) { dblCurrentLeft += dblGunnerSpeed; }

            //limits the gunner to the screen dimensions
            if (dblCurrentLeft < 0) { dblCurrentLeft = 0; }
            if (dblCurrentLeft > 685) { dblCurrentLeft = 685; }
            //resets the position of the gunner sprite
            Canvas.SetLeft(imgGunner, dblCurrentLeft);

        }
        //the idea behind this is that every game tick each invader has a 1 in 1000 chance of dropping a bomb
        //the chances of dropping get higher for each new level.
        private void invadersDropBombs()
        {
            //so run through all the invaders
            foreach (Invader thisInvader in Aliens)
            {   
                //pick a random integer between 0 and intBombRate and if it is a 1 drop the bomb
                if (randomObj.Next(intBombRate) == 1)
                {
                    //instanciate a new bomb and add it to the canvas and add a reference to it to the list Bombs.
                    Bomb newBomb = new Bomb();

                    cnvSpace.Children.Add(newBomb);
                    Bombs.Add(newBomb);
                    //set the position of the bomb to be that of the alien that dropped it
                    Canvas.SetLeft(newBomb, Canvas.GetLeft(thisInvader)+15);
                    Canvas.SetTop(newBomb, Canvas.GetTop(thisInvader)+30);
                }
            }
            
        }
        //here is the fire bullet method
        private void fireBullet()
        {
            //instanciate a new bullet user control
            Bullet newBullet = new Bullet();
            //put it on the screen andd add refernece to the bullets list
            cnvSpace.Children.Add(newBullet);
            Bullets.Add(newBullet);
            //set its position to that of the centre of the gunner
            Canvas.SetLeft(newBullet, Canvas.GetLeft(imgGunner) + imgGunner.ActualWidth*.5);
            Canvas.SetTop(newBullet, Canvas.GetTop(imgGunner));
        }
        //here is the move bullets method
        private void moveBullets()
        {
            //runs through all the bullets currently on the screen and moves them 1 by 1
            foreach (Bullet thisBullet in Bullets)
            {
                //the bullet user control actually has a method to move it defined within the control
                thisBullet.MoveBullet(intBulletSpeed);
            }

        }
        //here is the move bombs method
        private void moveBombs()
        {
            //runs through the list of bombs actively on the screen and calls the MoveBomb method in the user control
            foreach (Bomb thisBomb in Bombs)
            {
                thisBomb.MoveBomb(intBombSpeed);
            }
        }
        //this is the list all all the hit tests that must be performed and what to do about them it is run every game tick.
        private void doHitTests()
        {
            //First bullets hitting invaders
            /////////////////////////////////////

            //runs through all bullets and for each one sees if it has hit an invader
            foreach(Bullet thisBullet in Bullets)
            {
                foreach(Invader thisInvader in Aliens)
                {
                    if (BulletInvaderHitTest(thisBullet,thisInvader))
                
                    // if the invader has been hit it loses a life and its opacity fades if it is out of lives it is added
                    // to the dead aliens list and you get points for it
                        
                    {
                    thisInvader.lives --;
                    thisInvader.Opacity -= .2;
                    if (thisInvader.lives==0)
                    {
                        DeadAliens.Add(thisInvader);
                        intScore+= thisInvader.points;
                        lblScore.Content = "Score : " + intScore.ToString();
                    }
                        //the bullet is added to dead bullets if it has hit anything
                    DeadBullets.Add(thisBullet);
                }

                }
            }

            //the dead bullets are then removed from the list and from the screen
            //this method is neccessary because the contents of the typed list cannot be altered whilst a foreach loop is running on it.
            foreach (Bullet thisBullet in DeadBullets)
            {
                Bullets.Remove(thisBullet);
                cnvSpace.Children.Remove(thisBullet);
            }
            //the dead bullets list is then cleared to avoid deleting them twice.
            DeadBullets.Clear();

            //the same routine is run for the aliens with the addition that if all the aliens are killed the winLevel mothod is called
            foreach (Invader thisInvader in DeadAliens)
            {
                Aliens.Remove(thisInvader);
                cnvSpace.Children.Remove(thisInvader);
            }
            DeadAliens.Clear();
            if (Aliens.Count() == 0)
            {
                winLevel();
            }


            //The next hit test is for bombs hitting the gunner
            ////////////////////////////////////////////////////

            foreach (Bomb thisBomb in Bombs)
            {
                if (BombGunnerHitTest(thisBomb))

                {
                    loseLife();
                    DeadBombs.Add(thisBomb);
                }
            }
            
            foreach(Bomb thisBomb in DeadBombs)
            {
                Bombs.Remove(thisBomb);
                cnvSpace.Children.Remove(thisBomb);
            }
            DeadBombs.Clear();


            //The next hittest is for bombs hitting the shields
            ///////////////////////////////////////////////////

            //runs through all bombs on screen
            foreach (Bomb thisBomb in Bombs)
                //as there are rather a lot of shield elements it only runs if the bomb is in the vicinity
                if (Canvas.GetTop(thisBomb) > 450)
                {
                    {
                        //runs through each shield element
                        foreach (shieldElement thisShieldElement in Shields)
                        {
                            //if the element is hit it loses a life
                            if (BombShieldElementHitTest(thisBomb, thisShieldElement))
                            {
                                thisShieldElement.intLives--;
                                //and the bomb is added to the dead bombs list to be deleted later
                                DeadBombs.Add(thisBomb);

                                // if the shield element is dead it is removed else it is redrawn with a new graphic
                                if (thisShieldElement.intLives == 0)
                                { DeadShieldElements.Add(thisShieldElement); }
                                else { reDrawShield(thisShieldElement); }
                            }
                        }

                    }
                }

            //as before the dead bombs and dead shield elements are removed. One slight problem is that a bomb can kill more than 1 shield element at a time.
            foreach (Bomb thisBomb in DeadBombs)
            {
                Bombs.Remove(thisBomb);
                cnvSpace.Children.Remove(thisBomb);
            }
            DeadBombs.Clear();
            foreach (shieldElement thisShieldElement in DeadShieldElements)
            {
                Shields.Remove(thisShieldElement);
                cnvSpace.Children.Remove(thisShieldElement);
            }
            DeadShieldElements.Clear();

            //the next hit test is for bullets hitting shield elements to make a pillarbox
            ////////////////////////////////////////////////////////////////////////////////

            foreach (Bullet thisBullet in Bullets)
                
            {
                foreach (shieldElement thisShieldElement in Shields)
                {
                    // if it hits the shield element loses a life and the bullet gets added to the deadbullets list
                    if (BulletShieldElementHitTest(thisBullet,thisShieldElement))
                    {
                    thisShieldElement.intLives--;
                    DeadBullets.Add(thisBullet);

                        //if the shield element is dead it is removed or else it is redrawn with damage.
                        if (thisShieldElement.intLives == 0)
                        { DeadShieldElements.Add(thisShieldElement); }
                        else { reDrawShield(thisShieldElement); }
                    }
                }
           }
        
            // dead bullets and shield elements are de;eted from screen and active lists as before
            foreach (Bullet thisBullet in DeadBullets)
            {
                Bullets.Remove(thisBullet);
                cnvSpace.Children.Remove(thisBullet);
            }
            DeadBullets.Clear();

            foreach (shieldElement thisShieldElement in DeadShieldElements)
            {
                Shields.Remove(thisShieldElement);
                cnvSpace.Children.Remove(thisShieldElement);
            }
            DeadShieldElements.Clear();

    

   

        }

        //Now here are the actual hit tests


        //Bullets hit invaders
        private bool BulletInvaderHitTest(Bullet thisBullet, Invader thisInvader)
        {
            //two rectangles are defined: one around the bullet, one around the invader
            Rect recBullet = new Rect(Canvas.GetLeft(thisBullet), Canvas.GetTop(thisBullet), thisBullet.ActualWidth, thisBullet.ActualHeight);
            Rect recInvader = new Rect(Canvas.GetLeft(thisInvader), Canvas.GetTop(thisInvader), thisInvader.ActualWidth, thisInvader.ActualHeight);
            //The intersectsWith method is called on recBullet with the argument recInvader which returns true if the rectangles overlap.
            //This result is returned to the condition statement calling the method and it is similar to a function in visual basic.
            return recBullet.IntersectsWith(recInvader);
        }
        //Bombs hit Gunners
        private bool BombGunnerHitTest(Bomb thisBomb)
        {
            Rect recBomb = new Rect(Canvas.GetLeft(thisBomb), Canvas.GetTop(thisBomb), thisBomb.ActualWidth, thisBomb.ActualHeight);
            Rect recGunner = new Rect(Canvas.GetLeft(imgGunner), Canvas.GetTop(imgGunner), imgGunner.ActualWidth, imgGunner.ActualHeight);
            return recBomb.IntersectsWith(recGunner);
        }
        //Bombs Hit shield elements
        private bool BombShieldElementHitTest(Bomb thisBomb, shieldElement thisShieldElement)
        {
            Rect recBomb = new Rect(Canvas.GetLeft(thisBomb), Canvas.GetTop(thisBomb), thisBomb.ActualWidth, thisBomb.ActualHeight);
            Rect recShieldElement = new Rect(Canvas.GetLeft(thisShieldElement), Canvas.GetTop(thisShieldElement), 10, 20);
            return recBomb.IntersectsWith(recShieldElement);
        }
        //bullets hit shield elements
        private bool BulletShieldElementHitTest(Bullet thisBullet, shieldElement thisShieldElement)
        {
            Rect recBullet = new Rect(Canvas.GetLeft(thisBullet), Canvas.GetTop(thisBullet), thisBullet.ActualWidth, thisBullet.ActualHeight);
            Rect recShieldElement = new Rect(Canvas.GetLeft(thisShieldElement), Canvas.GetTop(thisShieldElement), 10, 20);
            return recBullet.IntersectsWith(recShieldElement);
        }

        //in this method all bullets that have flown off then top of the screen are deleted.
        private void loseBulletsOffScreen()
        {
            foreach(Bullet thisBullet in Bullets)
                if (Canvas.GetTop(thisBullet) < 0)
             { 
                DeadBullets.Add(thisBullet); 
             }
            foreach (Bullet thisBullet in DeadBullets)
            {
                Bullets.Remove(thisBullet);
                cnvSpace.Children.Remove(thisBullet);
            }
            DeadBullets.Clear();
        }
        //bombs off the sceen area are deleted
        private void loseBombsOffScreen()
        {
            foreach(Bomb thisBomb in Bombs)
            {
                if (Canvas.GetTop(thisBomb) >543)
                {
                    DeadBombs.Add(thisBomb);
                }
            }
            foreach(Bomb thisBomb in DeadBombs)
            {
                Bombs.Remove(thisBomb);
                cnvSpace.Children.Remove(thisBomb);
            }
            DeadBombs.Clear();


        }



        //Now the KeyDown, KeyUp event handlers

        //This handler reads the key down event 

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //sets blnSpace to true when the spacebar is pressed 
            //and also starts the game if it has not already started.
            if (e.Key == Key.Space)
            {
                blnSpace = true;
                if (blnGameStarted == false)
                {
                    startGame();
                    blnGameStarted = true;
                }

            }
            //sets blnLeft to true if left arrow pressed
            if (e.Key == Key.Left)
            {
                blnLeft = true;
            }
            //sets blnright to true if right arrow pressed
            if (e.Key == Key.Right)
            {
                blnRight = true;
            }
            //stops the game if you press q.
            if (e.Key == Key.Q)
            {
                stopGame();
            }
        }

        //the KeyUp event handler sets the boolean key variables to false when the key is released.
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                blnSpace = false;
            }
            if (e.Key == Key.Left)
            {
                blnLeft = false;
            }
            if (e.Key == Key.Right)
            {
                blnRight = false;
            }


        }

        //when the ship loses a life
        private void loseLife()
        {
                //decrements the intLives variable and exits if all lives are gone
                intLives --;
                if (intLives == 0)
                { gameOver(); }
                else
                    //deletes one of the spare lives (assuming you have one to spare)
                {
                    Image thisLife = Lives.Last();
                    Lives.Remove(thisLife);
                    cnvSpace.Children.Remove(thisLife);
                }
        


        }

        //gameOver updates the high score if you have exceeded it and displays messages
        private void gameOver()
        {
            if (intScore > intHighScore)
            {
                intHighScore = intScore;
                lblHighScore.Content = "High Score : " + intHighScore.ToString();
            }
                lblGameOver.Visibility=Visibility.Visible;
                lblPressSpace.Visibility=Visibility.Visible;
                blnGameStarted=false;
                stopGame(); 
        }
        //winLevel - stops the game timer and displays level completed message
        private void winLevel()
        {
            myTimer.Stop();
            lblLevelCompleted.Visibility = Visibility.Visible;
            intBonus = intLevel * 1000;
            lblLevelCompleted.Content = ("Level "+intLevel.ToString()+" completed!!! Bonus "+intBonus.ToString()+ "!!!");
            //stops the game for 2 seconds to allow message to be read
            Thread.Sleep(2000);
            //hides messages again and updates score
            lblLevelCompleted.Visibility = Visibility.Hidden;
            //adds bonus points and updates score label
            intScore += intBonus;
            lblScore.Content = "Score : " + intScore.ToString();
            //increases level and updates label
            intLevel++;
            lblLevel.Content = "Level : " + intLevel.ToString();
            //increases invader speed by 10%
            dblInvSpeed += dblInvSpeed * .1;
            //decreases the odds of each invader dropping a bomb by 10%
            intBombRate -= (intBombRate/10);
            //draws a new set of aliens
            drawAliens();
            //removes the shields
            foreach (shieldElement thisShieldElement in Shields)
            { cnvSpace.Children.Remove(thisShieldElement); }
            Shields.Clear();
            //and redraws new intact ones
            drawShields();
            //starts the game back up.
            myTimer.Start();


            
            
            
        }
        //Click event for help item in help menu
        private void help_Click(object sender, RoutedEventArgs e)
        {
            //instantiates a new window of the help type.
            help h = new help();
            h.ShowDialog();
        }
        //click event for about item in the help menu
        private void about_Click(object sender, RoutedEventArgs e)
        {
            //opens an about menu.
            about a = new about();
            a.ShowDialog();

        }



  



    }

    }






