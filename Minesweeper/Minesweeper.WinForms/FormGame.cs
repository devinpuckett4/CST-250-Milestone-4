using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Minesweeper.BLL;
using Minesweeper.Models;
using Microsoft.VisualBasic; // for InputBox

namespace Minesweeper.WinForms
{
    public partial class FormGame : Form
    {
        private readonly IBoardOperations _ops;
        private readonly BoardModel _board;
        private Panel pnlGrid;
        private Label lblStatus;
        private Label lblTimer;
        private Label lblRewards;
        private Button btnPeek;
        private Button btnClose;
        private System.Windows.Forms.Timer _gameTimer;
        private readonly int _cellSize;

        public FormGame(IBoardOperations ops, BoardModel board)
        {
            _ops = ops ?? throw new ArgumentNullException(nameof(ops));
            _board = board ?? throw new ArgumentNullException(nameof(board));
            int n = _board.Size;
            _cellSize = n <= 9 ? 44 : n <= 14 ? 38 : n <= 19 ? 32 : 28;

            Text = $"Minesweeper - {n}x{n}";
            StartPosition = FormStartPosition.CenterScreen;
            int gridW = _cellSize * n;
            int gridH = _cellSize * n;
            ClientSize = new Size(Math.Max(560, gridW + 40), Math.Max(620, gridH + 180));

            // 1. Create the status label FIRST
            lblStatus = new Label
            {
                Text = "Good luck! Left-click reveal • Right-click flag",
                AutoSize = true,
                Location = new Point(20, 20),
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                BackColor = Color.Transparent   // <-- important
            };

            // 2. Create the grid panel
            pnlGrid = new Panel
            {
                Location = new Point(20, 60),
                Size = new Size(gridW + 2, gridH + 2),
                BackColor = Color.Gainsboro
            };

            // 3. Add the panel **after** the label (order in Controls matters)
            Controls.Add(lblStatus);   // background layer
            Controls.Add(pnlGrid);     // foreground layer 

            lblTimer = new Label { Text = "Time: 0s", AutoSize = true, Location = new Point(pnlGrid.Right - 150, 20), Font = new Font("Segoe UI", 11f, FontStyle.Bold), ForeColor = Color.DarkBlue };
            lblRewards = new Label { Text = "Peeks: 0", AutoSize = true, Location = new Point(20, pnlGrid.Bottom + 20), Font = new Font("Segoe UI", 11f) };
            btnPeek = new Button { Text = "Use Peek", Size = new Size(100, 34), Location = new Point(20, lblRewards.Bottom + 10) };
            btnPeek.Click += BtnPeek_Click;
            btnClose = new Button { Text = "Close", Size = new Size(100, 34), Location = new Point(btnPeek.Right + 20, btnPeek.Top) };
            btnClose.Click += (s, e) => Close();

            _gameTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _gameTimer.Tick += GameTimer_Tick;
            _gameTimer.Start();

            Controls.AddRange(new Control[] { lblTimer, lblRewards, btnPeek, btnClose });

            BuildButtons(n);
            RefreshBoard();
        }

        private Font CellFont => new Font("Segoe UI Emoji", Math.Max(8f, _cellSize * 0.35f), FontStyle.Bold);

        private void BuildButtons(int n)
        {
            pnlGrid.Controls.Clear();
            for (int r = 0; r < n; r++)
            {
                for (int c = 0; c < n; c++)
                {
                    var btn = new Button
                    {
                        Name = $"btn_{r}_{c}",
                        Size = new Size(_cellSize, _cellSize),
                        Location = new Point(c * _cellSize, r * _cellSize),
                        Text = "?",
                        Tag = (r, c),
                        Margin = Padding.Empty,
                        Padding = new Padding(0, -2, 0, 2), // Shift text up to prevent bottom cutoff
                        FlatStyle = FlatStyle.Flat,
                        Font = CellFont,
                        TextAlign = ContentAlignment.MiddleCenter,
                        UseCompatibleTextRendering = true // Better text rendering
                    };
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = Color.Silver;
                    btn.UseVisualStyleBackColor = false;
                    btn.Click += OnCellClick;
                    btn.MouseUp += OnCellMouseUp;
                    pnlGrid.Controls.Add(btn);
                }
            }
        }

        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            var elapsed = DateTime.UtcNow - _board.StartTime;
            lblTimer.Text = $"Time: {(int)elapsed.TotalSeconds}s";
        }

        private void OnCellMouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            if (sender is not Button b || b.Tag is not ValueTuple<int, int> tag) return;
            if (!_board.Cells[tag.Item1, tag.Item2].IsVisited)
            {
                _ops.ToggleFlag(_board, tag.Item1, tag.Item2);
                RefreshBoard();
            }
        }

        private void OnCellClick(object? sender, EventArgs e)
        {
            if (sender is not Button b || b.Tag is not ValueTuple<int, int> tag) return;
            int r = tag.Item1, c = tag.Item2;

            int prevRewards = _board.RewardsRemaining;
            _ops.RevealCell(_board, r, c);

            if (_board.RewardsRemaining > prevRewards)
                MessageBox.Show("You found a special Hint reward! You can now peek one hidden cell safely.",
                                "Reward Found!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            var state = _ops.DetermineGameState(_board);

            if (state == GameState.Lost)
            {
                lblStatus.Text = "Boom! You lost";
                RevealAllBombs();
                DisableAllCells();
                _gameTimer.Stop();
                lblTimer.ForeColor = Color.Red;
            }
            else if (state == GameState.Won)   // WIN BRANCH
            {
                // 1. Stop timer
                _gameTimer.Stop();

                // 2. Calculate score
                int score = _ops.DetermineFinalScore(_board);

                // 3. Show win message
                lblStatus.Text = $"You won! Score: {score}";
                lblStatus.ForeColor = Color.DarkGreen;
                lblStatus.Font = new Font(lblStatus.Font, FontStyle.Bold);

                // 4. Disable the whole board
                DisableAllCells();

                //  hide the message after 3 seconds
                var hideTimer = new System.Windows.Forms.Timer { Interval = 3000 };
                hideTimer.Tick += (s, ev) =>
                {
                    lblStatus.Visible = false;
                    hideTimer.Stop();
                    hideTimer.Dispose();
                };
                hideTimer.Start();
            }

            RefreshBoard();
        }
        private void BtnPeek_Click(object? sender, EventArgs e)
        {
            if (_board.RewardsRemaining <= 0)
            {
                MessageBox.Show("No peeks available!", "Peek", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string input = Interaction.InputBox("Enter row,col to peek (e.g. 5,8):", "Use Peek Reward", "");
            if (string.IsNullOrWhiteSpace(input)) return;

            var parts = input.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2 || !int.TryParse(parts[0].Trim(), out int row) || !int.TryParse(parts[1].Trim(), out int col))
            {
                MessageBox.Show("Invalid format. Use row,col", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string result = _ops.UseRewardPeek(_board, row, col);
            MessageBox.Show(result, "Peek Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshBoard();
        }

        private void RefreshBoard()
        {
            int n = _board.Size;
            for (int r = 0; r < n; r++)
                for (int c = 0; c < n; c++)
                    RefreshCell(r, c);

            lblRewards.Text = $"Peeks: {_board.RewardsRemaining}";
            btnPeek.Enabled = _board.GameState == GameState.StillPlaying && _board.RewardsRemaining > 0;
        }

        private static Color ColorForNumber(int num) => num switch
        {
            1 => Color.RoyalBlue,
            2 => Color.ForestGreen,
            3 => Color.Firebrick,
            4 => Color.MidnightBlue,
            5 => Color.Maroon,
            6 => Color.Teal,
            7 => Color.Black,
            8 => Color.DimGray,
            _ => Color.Black
        };

        private void RefreshCell(int r, int c)
        {
            if (pnlGrid.Controls[$"btn_{r}_{c}"] is not Button btn) return;
            var cell = _board.Cells[r, c];
            var state = _board.GameState;

            // default appearance
            btn.Text = "";
            btn.BackColor = cell.IsVisited ? Color.White : Color.LightGray;
            btn.ForeColor = Color.Black;
            btn.Enabled = state == GameState.StillPlaying && !cell.IsVisited;

            // overrides
            if (cell.IsFlagged)
            {
                btn.Text = "🚩";
                btn.BackColor = Color.PaleGoldenrod;
            }

            if (!cell.IsVisited && state == GameState.Lost && cell.IsBomb)
            {
                btn.Text = "💣";
                btn.BackColor = Color.Red;
            }

            if (cell.IsVisited)
            {
                if (cell.IsBomb)
                {
                    btn.Text = "💣";
                    btn.BackColor = Color.Red;
                    btn.ForeColor = Color.White;
                }
                else if (cell.NumberOfBombNeighbors > 0)
                {
                    btn.Text = cell.NumberOfBombNeighbors.ToString();
                    btn.ForeColor = ColorForNumber(cell.NumberOfBombNeighbors);
                }
            }

            // wrong flag on loss
            if (state == GameState.Lost && cell.IsFlagged && !cell.IsBomb)
            {
                btn.Text = "❌";
                btn.BackColor = Color.DarkOrange;
            }

            if (!cell.IsVisited && !cell.IsFlagged && state == GameState.StillPlaying)
                btn.Text = "?";
        }

        private void RevealAllBombs()
        {
            int n = _board.Size;
            for (int r = 0; r < n; r++)
                for (int c = 0; c < n; c++)
                    if (_board.Cells[r, c].IsBomb)
                        _board.Cells[r, c].IsVisited = true;
        }

        private void DisableAllCells()
        {
            foreach (Control ctl in pnlGrid.Controls)
                if (ctl is Button b) b.Enabled = false;
        }
    }
}