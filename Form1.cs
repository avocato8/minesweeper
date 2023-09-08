using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bomb
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void gameStart(TextBox tBstringNum, TextBox tBbombNum, bool endlessMode)
        {
            int stringNum = int.Parse(tBstringNum.Text);
            int bombNum = int.Parse(tBbombNum.Text);
            bool extraGame = endlessMode;


            if (stringNum <= bombNum)
            {
                MessageBox.Show("지뢰의 개수가 전체 버튼 개수보다 같거나 많습니다.");
            }
            else if(stringNum == 0 || bombNum == 0)
            {
                MessageBox.Show("0은 처리할 수 없습니다.");
            }
            else
            {
                Form2 Form2 = new Form2(stringNum, bombNum, endlessMode);
                Form2.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            gameStart(textBox1, textBox2, false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            gameStart(textBox1, textBox2, true);
        }
    }
}
