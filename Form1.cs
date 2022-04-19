using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myGame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.KeyPreview = true;
            // 设置窗体属性,使其可以接收键盘消息
        }

        World world = new World();  //声明出world世界
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            world.KeyPress(e.KeyChar);
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            world.NextRound();  //world世界到下一个时刻

            if (world.bGameOver)
            {
                label1.Text = "GAME OVER";
                timer1.Stop();
            }
            pictureBox1.BackgroundImage = world.Show();//显示这个世界
            pictureBox1.Refresh();  //刷新

        }


        private void button1_Click(object sender, EventArgs e)
        {   
            timer1.Start();
            //FSky.Audio.Play("audio\\MOVE.WAV");       //插入音乐
            label1.Text = "游戏中...";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();

            //this.Text = "Game Over";
            label1.Text = "暂停...";
        }

    }
}
