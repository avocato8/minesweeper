using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace bomb
{
    public partial class Form2 : Form
    {
        private int stringNum, bombNum;
        private bool endlessMode;
        private List<int> bombList = new List<int>();
        List<Button> buttons = new List<Button>();
        private int sec, milisec;
        private double remainedString, remainedBomb;
        private static double savedProbability = 1;
        private double probability = savedProbability;
        

        public Form2(int stringNum, int bombNum, bool endlessMode)
        {
            InitializeComponent();
            this.stringNum = stringNum;
            this.bombNum = bombNum;
            this.endlessMode = endlessMode;
            this.remainedString = stringNum;
            this.remainedBomb = bombNum;
            this.sec = stringNum - 1;
            this.milisec = 100;

            timer1.Start();

            makeStringButton(stringNum);
            chooseBomb(bombNum);

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            milisec--;
            if (milisec < 0)
            {
                milisec = 100;
                sec--;
            }
            label1.Text = String.Format($"{sec:D2} : {milisec:D2}");

            if (sec < 0)
            {
                label2.Text = "폭탄";
                timer1.Stop();
                MessageBox.Show("폭탄이 터졌습니다.\r\n" + probability);
            }
        }
        private void makeStringButton(int stringNum)
        {
            for (int i = 0; i < stringNum; i++)
            {
                Button button = new Button();
                button.Text = $"{i + 1} 번째 선";
                button.Tag = i + 1;
                button.Location = new Point(10, 10 + i * 30);
                button.Click += Button_Click;
                buttons.Add(button);
                this.Controls.Add(button);
            }
        }
        private void chooseBomb(int bombNum)
        {
            Random random = new Random();

            while (bombList.Count < bombNum)
            {
                int randomLine = random.Next(1, stringNum + 1);
                if (!bombList.Contains(randomLine))
                {
                    bombList.Add(randomLine);
                }
            }
        }
        private void examineBomb()
        {
            foreach (Button a in buttons)
            {
                if (a.Enabled == true)
                {
                    if (!bombList.Contains((int)a.Tag))
                        return;
                }
            }
            label2.Text = "";
            Thread.Sleep(500);
            timer1.Stop();
            DialogResult gameOver = MessageBox.Show("지뢰를 모두 해체했습니다\r\n 확률 : "+ probability * 100 + "%",
                "게임종료", MessageBoxButtons.OK);

            if(endlessMode && gameOver == DialogResult.OK)
            {
                
                InitializeComponent();
                IncreaseDifficulty(ref probability, ref savedProbability);
            }
            else if(gameOver == DialogResult.OK)
            {
                probability = 1;
                this.Close();
            }
        }
        private void IncreaseDifficulty(ref double probability, ref double savedProbabilty)
        {
            savedProbability = probability;
            stringNum++;
            bombNum++;
            remainedString = stringNum;
            remainedBomb = bombNum;
            sec = stringNum - 1;
            milisec = 100;

            this.Close();
            Form2 newForm = new Form2(stringNum, bombNum, endlessMode);
            newForm.Show();
            probability = savedProbability;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            clickedButton.Enabled = false;
            

            if (bombList.Contains((int)clickedButton.Tag))
            {
                label2.Text = "";
                Thread.Sleep(500);
                label2.Text = "쾅";
                pictureBox1.Show();
                timer1.Stop();
                Thread.Sleep(500);

                if(probability == 1)
                {
                    probability = 0;
                }
                DialogResult gameOver = MessageBox.Show("폭탄이 터졌습니다.\r\n 확률 : " + probability * 100 + "%", "게임종료", MessageBoxButtons.OK);
                probability = 1;

                if (gameOver == DialogResult.OK)
                {
                    this.Close();
                }
            }
            else
            {
                label2.Text = "";
                Thread.Sleep(500);
                label2.Text = "터지지 않았습니다.";

                //확률 계산
                probability = probability * (remainedString - remainedBomb) / remainedString;
                remainedString--;

                examineBomb();
            }
        }
    }
}
