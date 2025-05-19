using System;

namespace Minesweeper
{
    public class GameBoard
    {
        public int Width { get; }       // 游戏区域宽度
        public int Height { get; }      // 游戏区域高度
        public int MineCount { get; }   // 地雷总数
        public bool GameOver { get; private set; }

        private readonly bool[,] mines;         // 地雷位置
        private readonly int[,] adjacentMines;  // 周围地雷数
        private readonly bool[,] revealed;      // 已揭开格子
        private readonly bool[,] flagged;       // 标记旗帜

        public GameBoard(int width, int height, int mineCount)
        {
            Width = width;
            Height = height;
            MineCount = mineCount;
            mines = new bool[width, height];
            adjacentMines = new int[width, height];
            revealed = new bool[width, height];
            flagged = new bool[width, height];

            InitializeBoard();
        }

        private void InitializeBoard()
        {
            // 随机布置地雷
            var random = new Random();
            var placed = 0;
            while (placed < MineCount)
            {
                int x = random.Next(Width);
                int y = random.Next(Height);
                if (!mines[x, y])
                {
                    mines[x, y] = true;
                    placed++;
                }
            }

            // 计算每个格子周围地雷数
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    adjacentMines[x, y] = CountAdjacentMines(x, y);
                }
            }
        }

        private int CountAdjacentMines(int x, int y)
        {
            int count = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int nx = x + i;
                    int ny = y + j;
                    if (nx >= 0 && nx < Width && ny >= 0 && ny < Height && mines[nx, ny])
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public void Reveal(int x, int y)
        {
            if (flagged[x, y] || revealed[x, y]) return;

            revealed[x, y] = true;

            if (mines[x, y])
            {
                SetGameOver(true); // 使用 SetGameOver 方法设置 GameOver 状态
                return;
            }

            // 如果周围没有地雷，自动展开相邻区域
            if (adjacentMines[x, y] == 0)
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        int nx = x + i;
                        int ny = y + j;
                        if (nx >= 0 && nx < Width && ny >= 0 && ny < Height && !revealed[nx, ny])
                        {
                            Reveal(nx, ny);
                        }
                    }
                }
            }
        }

        public void ToggleFlag(int x, int y)
        {
            if (!revealed[x, y])
            {
                flagged[x, y] = !flagged[x, y];
            }
        }

        public CellState GetCellState(int x, int y)
        {
            return new CellState(
                IsRevealed: revealed[x, y],
                IsMine: mines[x, y],
                IsFlagged: flagged[x, y],
                AdjacentMines: adjacentMines[x, y]
            );
        }

        // 添加一个公共方法来设置 GameOver 状态
        public void SetGameOver(bool isGameOver)
        {
            GameOver = isGameOver;
        }
    }

    public record CellState(
        bool IsRevealed,
        bool IsMine,
        bool IsFlagged,
        int AdjacentMines
    );
}