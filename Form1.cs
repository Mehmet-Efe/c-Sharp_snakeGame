using System.Runtime.CompilerServices;
using static System.Formats.Asn1.AsnWriter;

namespace littleSnake
{
    public partial class Form1 : Form
    {

        // width is panel count for gameBoard
        //height is panel count for gameBoard
        //bounds is panel bounds for gameBoard
        int width = 16, height = 16;
        int bounds = 35;

        bool bot = false;
        bool botDirX = false, botDirY = false;

        int score = 0, bestScore = 0;


        int foodX, foodY;

        //needed to avoid some bugy moves
        bool canMove = true;

        //creating snake object
        snake snake;

        char direction;

        //creating gameBoard object
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
            if (bot) { direction = 'd'; }
            else { direction = 'w'; }
            //int w and h => panel coordinates for gameBoard
            int w = 0, h = 0;

            //timer default internal ms
            if (bot)
            {
                timer1.Interval = 1;
            }
            else
            {
                timer1.Interval = 250;
            }
            score = 0;

            //giving text for form
            this.Text = "Snake - Score: " + score + " Best Score: " + bestScore;

            //Default direction
            /*direction = 'w';*/

            //Start snake object
            snake = new snake();

            //strating gameBoard object
            g = new gameboard[width, height];

            //creating and adding every panel to Form
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

            //snake start position
            snake.addLast(new body(8, 7));
            g[8, 7].setBodyPart(true);
            snake.addLast(new body(9, 7));
            g[9, 7].setBodyPart(true);
            snake.addLast(new body(10, 7));
            g[10, 7].setBodyPart(true);
            createFood();
            drawSnake();

            //starting timer
            timer1.Enabled = true;
        }

        private void createFood()
        {
            Random rnd = new Random();

            //valid checks if it is valid to insert food x and y coordinates
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

            int[,] snakeLocation = snake.getSnake();

            //set snake colors
            for (int x = snake.getLength() - 1; x >= 0; x--)
            {
                if (x == snake.getLength() - 1)
                {
                    //set backColor darkRed for head
                    g[snakeLocation[x, 0], snakeLocation[x, 1]].BackColor = Color.DarkRed;
                }
                else
                {
                    //other bodyPart is blue
                    g[snakeLocation[x, 0], snakeLocation[x, 1]].BackColor = Color.Blue;
                }
            }
            //set backColor orange for food
            g[foodX, foodY].BackColor = Color.Orange;
        }

        private void botMove(int x, int y)
        {
            if (y == width - 1 && !botDirX)
            {
                direction = 's';
                botDirX = true;
            }
            else if (botDirX)
            {
                direction = 'a';
                botDirX = false;
            }
            else if (y == 0 && !botDirY)
            {
                botDirY = true;
                direction = 's';
            }
            else if (botDirY)
            {
                botDirY = false;
                direction = 'd';
            }
            moveSnake();
        }

        private void moveSnake()
        {

            body tmp = snake.getTail().previous;
            body head = snake.getHead();
            body tail = snake.getTail();

            //getting only snake bodyPart coordinates
            int[,] snakeLocation = snake.getSnake();
            int moveX;
            int moveY;
            g[tail.getX(), tail.getY()].setBodyPart(false);
            g[tail.getX(), tail.getY()].BackColor = Color.LightGray;
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
            /*isGameOver(head.getX(), head.getY());*/

            //i dont preffer wall for side of the board
            //snake will teleport other side of the board
            //if gameBoard array get outOfBoundException then teleport the head other side
            //also check for food teleporting x,y coordinates is i for not
            //if do not check other snake did not eat food(bug)
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

        private void timer1_Tick(object sender, EventArgs e)//reDraw all buttons and checking game is over or not
        {
            if (bot)
            {
                botMove(snake.getHead().getX(), snake.getHead().getY());
            }
            else
            {
                moveSnake();
            }
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

                //remove all existing gameBoard object to start new game
                startNewGame();
            }
            drawSnake();
            canMove = true;
        }

        private void startNewGame()
        {
            timer1.Enabled = false;
            foreach (gameboard g in g)
            {
                Controls.Remove(g);
            }
            startGame();

        }

        private bool isGameOver(int x, int y)
        {
            bool isOver = false;

            //try catch is using for if snake go pass gameBoard border then check other side of the gameBoard
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
                else if (y < 0) { x = width - 1; }
                else if (y > width) { x = 0; }
            }
            return isOver;
        }

        //tail using for linkedList addLast method
        //head using for head current position is food or not
        private void eatFood(body head, body tail)
        {
            if (g[head.getX(), head.getY()].getIsFood())
            {
                snake.addLast(new body(tail.getX(), tail.getY()));
                g[head.getX(), head.getY()].setIsFood(false);
                createFood();
                score++;
                this.Text = "Snake - Score: " + score + " Best Score: " + bestScore;

                //speed up game after eating
                if (!bot)
                {
                    timer1.Interval -= 5;
                }
            }
        }

        private void changeDirection(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.C && !bot)
            {
                bot = true;
                startNewGame();
            }
            else if (e.KeyData == Keys.C && bot)
            {
                bot = false;
                startNewGame();
            }
            if (canMove)//needed for some bugy movement
            {
                int direction = e.KeyValue;

                //cant turn 180 degree 
                if ((direction == 87 || direction == 38) && this.direction != 's') { this.direction = 'w'; }
                else if ((direction == 65 || direction == 37) && this.direction != 'd') { this.direction = 'a'; }
                else if ((direction == 68 || direction == 39) && this.direction != 'a') { this.direction = 'd'; }
                else if ((direction == 83 || direction == 40) && this.direction != 'w') { this.direction = 's'; }
            }

            //set True in timer1_Tick method
            canMove = false;
        }
    }
}