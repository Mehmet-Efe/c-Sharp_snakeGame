using System.Runtime.CompilerServices;
using static System.Formats.Asn1.AsnWriter;

namespace littleSnake
{
    public partial class Form1 : Form
    {
        int width = 15, height = 15;
        int bounds = 35;
        int score = 0, bestScore = 0;
        int foodX, foodY;

        bool canMove = true;

        snake snake;

        char direction = 'w';

        gameboard[,] g;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            startGame();
        }

        public void startGame()
        {
            int w = 0, h = 0;
            timer1.Interval = 350;
            score = 0;
            this.Text = "Snake - Score: " + score + " Best Score: " + bestScore;
            direction = 'w';
            snake = new snake();
            g = new gameboard[width, height];
            for (int x = 0; x < height; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    g[x, y] = new gameboard();
                    g[x, y].setIsFood(false);
                    g[x, y].BackColor = Color.LightGray;
                    g[x, y].SetBounds(w, h, bounds, bounds);
                    g[x, y].setBodyPart(false);
                    this.Controls.Add(g[x, y]);
                    w += bounds;
                }
                h += bounds;
                w = 0;
            }
            snake.addLast(new body(8, 7));
            g[8, 7].setBodyPart(true);
            snake.addLast(new body(9, 7));
            g[9, 7].setBodyPart(true);
            snake.addLast(new body(10, 7));
            g[10, 7].setBodyPart(true);
            createFood();
            drawSnake();
            timer1.Enabled = true;
        }

        private void createFood()
        {
            Random rnd = new Random();
            bool valid = true;
            int x, y;
            while (valid)
            {
                x = rnd.Next(0, width);
                y = rnd.Next(0, height);
                if (!g[x, y].getBodyPart())
                {
                    valid = false;
                    foodX = x;
                    foodY = y;
                    g[x, y].setIsFood(true);
                }
            }
        }

        public void drawSnake()
        {
            moveSnake();
            int[,] snakeLocation = snake.getSnake();
            foreach (gameboard g in g)
            {
                g.BackColor = Color.LightGray;
            }
            for (int x = snake.getLength() - 1; x >= 0; x--)
            {
                if (x == snake.getLength() - 1)
                {
                    g[snakeLocation[x, 0], snakeLocation[x, 1]].BackColor = Color.DarkRed;
                }
                else
                {
                    g[snakeLocation[x, 0], snakeLocation[x, 1]].BackColor = Color.Blue;
                }
            }
            g[foodX, foodY].BackColor = Color.Orange;
        }

        private void changeDirection(object sender, KeyPressEventArgs e)
        {
            if (canMove)
            {
                char direction = e.KeyChar;
                if (direction == 'w' && this.direction != 's') { this.direction = direction; }
                else if (direction == 'a' && this.direction != 'd') { this.direction = direction; }
                else if (direction == 'd' && this.direction != 'a') { this.direction = direction; }
                else if (direction == 's' && this.direction != 'w') { this.direction = direction; }
            }
            canMove = false;
        }

        private void moveSnake()
        {
            body tmp = snake.getTail().previous;
            body head = snake.getHead();
            body tail = snake.getTail();
            int[,] snakeLocation = snake.getSnake();
            int moveX;
            int moveY;
            g[tail.getX(), tail.getY()].setBodyPart(false);
            while (tmp != head)
            {
                moveX = tmp.getX();
                moveY = tmp.getY();
                tmp.next.setX(moveX);
                tmp.next.setY(moveY);
                tmp = tmp.previous;
            }
            tmp.next.setX(tmp.getX());
            tmp.next.setY(tmp.getY());
            g[tmp.getX(), tmp.getY()].setBodyPart(true);
            if (direction == 'a') { tmp.setY(tmp.getY() - 1); }
            else if (direction == 's') { tmp.setX(tmp.getX() + 1); }
            else if (direction == 'd') { tmp.setY(tmp.getY() + 1); }
            else if (direction == 'w') { tmp.setX(tmp.getX() - 1); }
            isGameOver(head.getX(), head.getY());
            try
            {
                eatFood(head, tail);
            }
            catch (Exception e)
            {
                if (head.getX() < 0) { head.setX(height - 1); }
                else if (head.getX() > height - 1) { head.setX(0); ; }
                else if (head.getY() < 0) { head.setY(width - 1); ; }
                else if (head.getY() > width - 1) { head.setY(0); ; }
                eatFood(head, tail);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            drawSnake();
            if (isGameOver(snake.getHead().getX(), snake.getHead().getY()))
            {
                timer1.Enabled = false;
                if (score > bestScore)
                {
                    bestScore = score;
                    MessageBox.Show("Game Over\nCongratulations! New Best Score: " + bestScore);
                }
                else
                {
                    MessageBox.Show("Game over\n\nYour Score is: " + score);
                }
                foreach (gameboard g in g)
                {
                    Controls.Remove(g);
                }
                startGame();
            }
            canMove = true;
        }

        private bool isGameOver(int x, int y)
        {
            bool isOver = false;
            try
            {
                if (g[x, y].getBodyPart())
                {
                    isOver = true;
                }
            }
            catch (Exception e)
            {
                if (x < 0) { x = height - 1; }
                else if (x > height) { x = 0; }
                else if (y < 0) { x = width; }
                else if (y > width) { x = 0; }
            }
            return isOver;
        }

        private void eatFood(body head, body tail)
        {
            if (g[head.getX(), head.getY()].getIsFood())
            {
                snake.addLast(new body(tail.getX(), tail.getY()));
                g[head.getX(), head.getY()].setIsFood(false);
                createFood();
                score++;
                this.Text = "Snake - Score: " + score + " Best Score: " + bestScore;
                timer1.Interval -= 5;
            }
        }
    }
}