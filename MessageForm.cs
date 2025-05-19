using System;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class MessageForm : Form
    {
        public MessageForm(string message, string title)
        {
            // InitializeComponent();
            this.Text = title;
            this.Size = new Size(300, 150);
            this.StartPosition = FormStartPosition.CenterParent;

            Label messageLabel = new Label
            {
                Text = message,
                Font = new Font("Times New Roman", 12),
                AutoSize = true,
                Location = new Point(50, 30)
            };

            Button okButton = new Button
            {
                Text = "OK",
                Size = new Size(80, 30),
                Location = new Point(110, 80)
            };
            okButton.Click += (s, e) => this.Close();

            this.Controls.Add(messageLabel);
            this.Controls.Add(okButton);
        }
    }
}