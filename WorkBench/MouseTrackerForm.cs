using System;
using System.Windows.Forms;

public class MouseTrackerForm : Form
{
    private Label lblPosition;
    private System.Windows.Forms.Timer timer;

    public MouseTrackerForm()
    {
        Size = new System.Drawing.Size(400, 80);
        lblPosition = new Label { Dock = DockStyle.Top, Font = new System.Drawing.Font("Arial", 16) };
        Controls.Add(lblPosition);
        this.TopMost = true;
        timer = new System.Windows.Forms.Timer { Interval = 50 }; // Update every 50ms
        timer.Tick += Timer_Tick;
        timer.Start();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        var pos = Cursor.Position;
        lblPosition.Text = $"Mouse Position: X={pos.X}, Y={pos.Y}";
    }
}

