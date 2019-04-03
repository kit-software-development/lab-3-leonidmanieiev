using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escape_Button
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            // center button
            int cX = (this.Width - pushMeButton.Width) / 2;
            int cY = (this.ClientRectangle.Height - pushMeButton.Height) / 2;
            pushMeButton.Location = new Point(cX, cY);

            this.MouseMove += new MouseEventHandler(mainForm_MouseMove);
            pushMeButton.Click += new EventHandler(pushMeButton_Click);
        }
        
        private void mainForm_MouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            // Point(0, 0) of button
            Point btnLoc = pushMeButton.FindForm().PointToClient(
                pushMeButton.Parent.PointToScreen(pushMeButton.Location));
            
            // Central point of button
            Point btnCenterLoc = new Point(btnLoc.X + pushMeButton.Width / 2,
                                           btnLoc.Y + pushMeButton.Height / 2);

            // Min dist between btn and cursor before we move btn
            const double minDist = 100.0;

            // if cursor is close enough to button
            if (distanceToButtonCenter(mouseEventArgs, btnCenterLoc) < minDist)
            {
                moveButton(mouseEventArgs, btnCenterLoc, btnLoc);
            }
        }
        
        // Move btn by X, Y offsets according to cursor quadrant
        private void moveButton(MouseEventArgs mouseEventArgs, Point btnCenterLoc, Point btnLoc)
        {
            Tuple<int, int> moveDist = calcMoveDist(mouseEventArgs, btnCenterLoc);
            int moveByX = moveDist.Item1, moveByY = moveDist.Item2;

            // cursor in I quadrant 
            if (
                btnCenterLoc.X <= mouseEventArgs.X &&
                btnCenterLoc.Y >= mouseEventArgs.Y &&
                isValidMove(btnLoc, -moveByX, moveByY)
               )
            {
                pushMeButton.Location = new Point(btnLoc.X - moveByX, btnLoc.Y + moveByY);
            }
            // cursor in II quadrant
            else if (
                     btnCenterLoc.X >= mouseEventArgs.X &&
                     btnCenterLoc.Y >= mouseEventArgs.Y &&
                     isValidMove(btnLoc, moveByX, moveByY)
                    )
            {
                pushMeButton.Location = new Point(btnLoc.X + moveByX, btnLoc.Y + moveByY);
            }
            // cursor in III quadrant 
            else if (
                     btnCenterLoc.X >= mouseEventArgs.X &&
                     btnCenterLoc.Y <= mouseEventArgs.Y &&
                     isValidMove(btnLoc, moveByX, -moveByY)
                    )
            {
                pushMeButton.Location = new Point(btnLoc.X + moveByX, btnLoc.Y - moveByY);
            }
            // cursor in IV quadrant
            else if (
                     btnCenterLoc.X <= mouseEventArgs.X &&
                     btnCenterLoc.Y <= mouseEventArgs.Y &&
                     isValidMove(btnLoc, -moveByX, -moveByY)
                    )
            {
                pushMeButton.Location = new Point(btnLoc.X - moveByX, btnLoc.Y - moveByY);
            }
        }

        // if it`s valid to move button
        private bool isValidMove(Point btnLoc, int moveByX, int moveByY)
        {
            int newX = btnLoc.X + moveByX, 
                newY = btnLoc.Y + moveByY,
                newW = newX + pushMeButton.Width, 
                newH = newY + pushMeButton.Height;

            // check boundaries
            if (
                newX < 0 ||
                newY < 0 ||
                newW > this.ClientRectangle.Width ||
                newH > this.ClientRectangle.Height
               )
            {
                return false;
            }

            return true;
        }

        // values to add to coords of button for move
        private Tuple<int, int> calcMoveDist(MouseEventArgs mouseEventArgs, Point btnCenterLoc)
        {
            // distance between corresponding coordinates of cursor and button
            double distByX = Math.Abs(mouseEventArgs.X - btnCenterLoc.X),
                   distByY = Math.Abs(mouseEventArgs.Y - btnCenterLoc.Y);

            // magic const value
            const double mul = 100.0;

            // calculate values to add to coords of button
            double moveByX = (int)distByX == 0 ? 0.0 : mul * Math.Exp(-Math.Log(distByX)),
                   moveByY = (int)distByY == 0 ? 0.0 : mul * Math.Exp(-Math.Log(distByY));

            /* when cursor is close to axis, moveBy* will be too large
             * so we reduce it manualy according to some consts */
            moveByX = moveByX > mul / 3 ? Math.Log(moveByX, 50) : moveByX;
            moveByY = moveByY > mul / 3 ? Math.Log(moveByX, 50) : moveByY;

            return Tuple.Create((int)moveByX, (int)moveByY);
        }

        // Distance from cursor to center of button
        private double distanceToButtonCenter(MouseEventArgs mouseEventArgs, Point btnCenterLoc)
        {
            return Math.Sqrt(Math.Pow(btnCenterLoc.X - mouseEventArgs.X, 2) + 
                             Math.Pow(btnCenterLoc.Y - mouseEventArgs.Y, 2));
        }

        private void pushMeButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Поздравляем! Вы смогли нажать на кнопку!");
            this.Close();
        }
    }
}
