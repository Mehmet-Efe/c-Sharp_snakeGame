using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace littleSnake
{
    internal class gameboard : Panel
    {

        //Give black line to every Panel
        public gameboard()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw 
                | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (SolidBrush brush = new SolidBrush(BackColor))
                e.Graphics.FillRectangle(brush, ClientRectangle);
            e.Graphics.DrawRectangle(Pens.Black, 0, 0, ClientSize.Width+1 , ClientSize.Height+1);
        }

        //creating needed required variables
        private bool isFood;
        private bool isBodyPart;

        public void setIsFood(bool isFood) { this.isFood = isFood; }

        public bool getIsFood() { return  this.isFood; }

        public void setBodyPart(bool isBodyPart)
        {
            this.isBodyPart = isBodyPart;
        }

        public bool getBodyPart()
        {
            return isBodyPart;
        }


    }
}
