using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class MainForm : Form
    {
        private const int CellSize = 30;  // 每个格子的大小
        // private const int BoardWidth = 10; // 游戏区域宽度
        // private const int BoardHeight = 10; // 游戏区域高度
        // private const int MineCount = 5; // 地雷数量
        //
        //先设置默认值
        private int BoardWidth=10; // 游戏区域宽度
        private int BoardHeight=10; // 游戏区域高度
        private int MineCount=15 ; // 地雷数量

        private GameBoard gameBoard=null!;
        private Button[,] cellButtons=null!;
        private Button restartButton=null!;    // 重新开始按钮
        private Button easyButton=null!;      // 简单模式按钮
        private Button normalButton=null!;    // 普通模式按钮
        private Button hardButton=null!;      // 困难模式按钮
    
        // 计时器和 Stopwatch
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();
        private Label timeLabel = null!; // 计时器标签

        public MainForm()
        {
            InitializeComponent();
            this.Icon = new Icon("ttt.ico"); // 设置窗体图标
            // 初始化计时器
            _timer.Interval = 100; // 每 100 毫秒更新一次
            _timer.Tick += Timer_Tick;
            
            InitializeGame();
        }

        private void InitializeGame()
        {
            // 初始化游戏面板
            gameBoard = new GameBoard(BoardWidth, BoardHeight, MineCount);
            cellButtons = new Button[BoardWidth, BoardHeight];    // 格子按钮数组

            // 创建按钮网格
            for (int x = 0; x < BoardWidth; x++)
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    var button = new Button
                    {
                        Size = new Size(CellSize, CellSize),
                        Location = new Point(x * CellSize, y * CellSize+80),    //Y轴坐标+80,为重新开始按键添加空间
                        Tag = new Point(x, y),
                        Font = new Font("Times New Roman", 12)
                    };

                    button.MouseUp += CellButton_MouseUp;
                    cellButtons[x, y] = button;
                    Controls.Add(button);
                }
            }

            // 计算按钮的宽度和位置
            int buttonAreaWidth = BoardWidth * CellSize; // 游戏区域的宽度
            int buttonWidth = (buttonAreaWidth - 50) / 4; // 按钮宽度（留出 50 像素的边距）
            int buttonHeight = 30; // 按钮高度
            int buttonSpacing = 10; // 按钮之间的间距


            // 创建重新开始按钮
            restartButton = new Button
            {
                Text = "Retry",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(10, 5),
                Font = new Font("Times New Roman", 12)
            };
            restartButton.Click += RestartButton_Click;
            Controls.Add(restartButton);

            //创建三种不同难度的模式
            easyButton = new Button  
            {  
                Text = "easy",  
                Size = new Size(buttonWidth, buttonHeight),  
                Location = new Point(restartButton.Right + buttonSpacing, 5),  
                Font = new Font("Times New Roman", 10)  
            };
            easyButton.Click += EasyButton_Click;
            Controls.Add(easyButton);

            normalButton = new Button  
            {              
                Text = "normal",  
                Size = new Size(buttonWidth, buttonHeight),        
                Location = new Point(easyButton.Right + buttonSpacing, 5),  
                Font = new Font("Times New Roman", 10)  
            };
            normalButton.Click += NormalButton_Click;
            Controls.Add(normalButton);


            hardButton = new Button  
            {  
                Text = "hard",  
                Size = new Size(buttonWidth, buttonHeight), 
                Location = new Point(normalButton.Right + buttonSpacing, 5),  
                Font = new Font("Times New Roman", 10)  
            };
            hardButton.Click += HardButton_Click;
            Controls.Add(hardButton);
            

            int timeLabelX = (buttonAreaWidth-100)/ 2; // 按钮中心位置
            int timeLabelY = buttonHeight+15; // 时间标签位置
            // 计时器标签
            timeLabel = new Label
            {
                Text = "00:00:00",
                Font = new Font("Times New Roman", 12),
                Location = new Point(timeLabelX, timeLabelY),
                Size = new Size(100, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(timeLabel);



            // 设置窗体大小
            ClientSize = new Size(BoardWidth * CellSize, BoardHeight * CellSize+80);
        }
        
        private void RestartGame()
        {
            // 重置计时器
            _stopwatch.Reset();
            timeLabel.Text = "00:00:00";

            //移除所有格子按钮
            if(cellButtons!=null)
            {
                int row=cellButtons.GetLength(0);
                int col=cellButtons.GetLength(1);
            for(int x = 0; x < row; x++)
            {
                for(int y = 0; y < col; y++)
                {
                    if(cellButtons[x,y]!=null)
                    {
                    Controls.Remove(cellButtons[x, y]);
                    }
                }
            }
                 Controls.Remove(restartButton);
                 Controls.Remove(easyButton);
                 Controls.Remove(normalButton);
                 Controls.Remove(hardButton);
                 Controls.Remove(timeLabel);
            }
            //重新初始化游戏面板
            InitializeGame();
            UpdateUI();
        }

        private void RestartButton_Click(object? sender, EventArgs e)
        {
            RestartGame();
        }

        private void EasyButton_Click(object? sender, EventArgs e)
        {
            BoardWidth = 10; // 游戏区域宽度
            BoardHeight = 10; // 游戏区域高度
            MineCount = 15; // 地雷数量
            RestartGame();
        }

        private void NormalButton_Click(object? sender, EventArgs e)
        {
            BoardWidth = 16; // 游戏区域宽度    
            BoardHeight = 16; // 游戏区域高度
            MineCount = 40; // 地雷数量
            RestartGame();
        }
       
       private void HardButton_Click(object? sender, EventArgs e)
        {
            BoardWidth = 20; // 游戏区域宽度
            BoardHeight = 20; // 游戏区域高度
            MineCount = 99; // 地雷数量
            RestartGame();
        }          


        private void UpdateUI()
        {
            for (int x = 0; x < BoardWidth; x++)
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    var state = gameBoard.GetCellState(x, y);
                    var button = cellButtons[x, y];

                    if (state.IsRevealed)
                    {
                        button.BackColor = Color.LightGray;
                        button.Text = state.IsMine ? "💣" : 
                                    state.AdjacentMines > 0 ? state.AdjacentMines.ToString() : "";
                    }
                    else
                    {
                        button.BackColor = SystemColors.Control;
                        button.Text = state.IsFlagged ? "🚩" : "";
                    }
                }
            }
        }

        private void CellButton_MouseUp(object? sender, MouseEventArgs e)
        {
            if (sender == null) return;
            if (gameBoard.GameOver) return;

            var button = (Button)sender;
            
            if (button.Tag == null) return;
            var location = (Point)button.Tag;

            int x = location.X;
            int y = location.Y;

            if (e.Button == MouseButtons.Left)
            {
                //第一次点击时启动计时器
                if (!_stopwatch.IsRunning)
                {
                    _stopwatch.Start();
                    _timer.Start();
                }

                gameBoard.Reveal(x, y);
                if (gameBoard.GameOver)
                {
                    _stopwatch.Stop();
                    _timer.Stop();

                    var messageForm = new MessageForm("游戏结束！你踩到地雷了！", "Game Over");
                    messageForm.ShowDialog(this); // 居中显示对话框
                    // RevealAllMines();
                    UpdateUI();
                    RevealAllMines();
                    return;
                }
                
                if(CheckWin()){

                    _stopwatch.Stop();
                    _timer.Stop();
                    
                    var messageForm = new MessageForm("恭喜！你赢了！", "You Win!");
                    messageForm.ShowDialog(this); // 居中显示
                    // RevealAllMines();
                    gameBoard.SetGameOver(true);
                    UpdateUI();
                    RevealAllMines();
                    return;
                    
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                gameBoard.ToggleFlag(x, y);
            }

            UpdateUI();
        }

        private bool CheckWin()
        {
            for (int x = 0; x < BoardWidth; x++)
            {
                for (int y = 0; y < BoardHeight; y++)
                {    

                    if (!gameBoard.GetCellState(x, y).IsRevealed&&!gameBoard.GetCellState(x, y).IsMine)
                    {
                        return false;    //有未翻开的空白格子，则游戏未结束
                    }
                }
            }
            return true;
        }

        private void RevealAllMines()
        {
            for (int x = 0; x < BoardWidth; x++)
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    var state = gameBoard.GetCellState(x, y);
                    if (state.IsMine)
                    {  
                        // 所有地雷都被揭开
                        cellButtons[x,y].BackColor = Color.LightGray;
                        cellButtons[x, y].Text = "💣";
                    }
                }
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            timeLabel.Text = _stopwatch.Elapsed.ToString("hh\\:mm\\:ss");
        }
        
    }
}