using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace littleSnake
{
    class body
    {
        int x, y;
        public body next;
        public body previous;

        public body(int x, int y)
        {
            this.x = x;
            this.y = y;
            next = null;
            previous = null;
        }

        public int getX() { return x; }
        public int getY() { return y; }

        public void setX(int x) { this.x = x; }

        public void setY(int y) { this.y = y; }
    }

    class snake
    {
        body head;
        body tail;
        int lenght = 0;

        public snake()
        {
            head = null;
            tail = null;
        }

        public void addLast(body newBody)
        {
            if (head == null)
            {
                head = newBody;
                tail = newBody;
            }
            else
            {
                tail.next = newBody;
                newBody.previous = tail;
                tail = newBody;
            }
            lenght++;
        }

        public int getLength()
        {
            return lenght;
        }

        public int[,] getSnake()
        {
            int[,] bodyLocations = new int[lenght, 2];
            int L = lenght;
            body tmp = head;
            while (tmp != null)
            {
                L--;
                bodyLocations[L, 0] = tmp.getX();
                bodyLocations[L, 1] = tmp.getY();
                tmp = tmp.next;
            }
            return bodyLocations;
        }


        public body getTail()
        {
            return tail;
        }

        public body getHead()
        {
            return head;
        }
    }
}
