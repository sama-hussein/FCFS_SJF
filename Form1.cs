using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FCFS_SJF
{
    public partial class Form1 : Form
    {
        // WITH SAME ARRIVAL TIME
        static int n = 4;
        static float avgTAT, avgWT;
        int []Processes = {1,2,3,4};
        int []BurstTime = new int[n];


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.Columns.Add("Processes", 70);
            listView1.Columns.Add("Burst Time", 70);    // time required by process for execution
            listView1.Columns.Add("Waiting Time", 90);
            listView1.Columns.Add("TAT", 70);            // time spent waiting and execution

        }

        void SJF() {
            //Shortest Job First Scheduling: less burst time --> executed first

            List<int> list = new List<int>();
            list.Add(int.Parse(textBox1.Text));
            list.Add(int.Parse(textBox2.Text));
            list.Add(int.Parse(textBox3.Text));
            list.Add(int.Parse(textBox4.Text));

            list.Sort();

            checkedListBox1.Items.Add(list[0]);
            checkedListBox1.Items.Add(list[1]);
            checkedListBox1.Items.Add(list[2]);
            checkedListBox1.Items.Add(list[3]);

        }

        private void button1_Click(object sender, EventArgs e)
        {
           /* //Processes added to the queue
            checkedListBox1.Items.Add(textBox1.Text);
            checkedListBox1.Items.Add(textBox2.Text);
            checkedListBox1.Items.Add(textBox3.Text);
            checkedListBox1.Items.Add(textBox4.Text); 
           */
            //Burst time of each process
            BurstTime[0] = Convert.ToInt32(textBox1.Text);
            BurstTime[1] = Convert.ToInt32(textBox2.Text);
            BurstTime[2] = Convert.ToInt32(textBox3.Text);
            BurstTime[3] = Convert.ToInt32(textBox4.Text);
            SJF();

            timer1.Start();
            findsAvgTime(Processes,n,BurstTime);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           // textBox5 ---> CPU
            timer1.Interval = Convert.ToInt32(checkedListBox1.Items[0]) * 1000;

            if (checkedListBox1.Items[0].ToString() == textBox1.Text) {
                textBox5.Text = textBox1.Text;
            }
            else if (checkedListBox1.Items[0].ToString() == textBox2.Text)
            {
                timer1.Interval = Convert.ToInt32(textBox2.Text) * 1000;
                textBox5.Text = textBox2.Text;  //CPU is allocated to this process 
            }
            else if (checkedListBox1.Items[0].ToString() == textBox3.Text)
            {
                timer1.Interval = Convert.ToInt32(textBox3.Text) * 1000;
                textBox5.Text = textBox3.Text;
            }
            else if (checkedListBox1.Items[0].ToString() == textBox4.Text)
            {
                timer1.Interval = Convert.ToInt32(textBox4.Text) * 1000;
                textBox5.Text = textBox4.Text;
            }

            checkedListBox1.Items.RemoveAt(0);
            if (checkedListBox1.Items.Count <= 0) {
                MessageBox.Show("Average Waiting Time: " + avgWT + "\n" + "Average TAT:" + avgTAT);
                timer1.Stop();               
            }
        }

        void findsAvgTime(int[] processes, int n, int[] BurstTime) {
            int[] TAT = new int[n];
            int[] Waiting = new int[n];
            int totalWaiting = 0;
            int totalTAT = 0;

            findWaitingTime(processes, n, Waiting, BurstTime);
            findTAT(processes, n, Waiting, BurstTime, TAT);

            for (int i = 1; i < n; i++)
            {
                totalWaiting += Waiting[i];
                totalTAT += TAT[i];
                string[] Row = { (i + 1).ToString(), BurstTime[i].ToString(), Waiting[i].ToString(), TAT[i].ToString() };
                var Item = new ListViewItem(Row);
                listView1.Items.Add(Item); 
            }
            avgWT = totalWaiting / n;   //average waiting time taken by each process
            avgTAT = totalTAT / n;      //average turn around time taken by each process
        }

        void findWaitingTime(int[]processes, int n, int []Waiting, int[]BurstTime) {
            Waiting[0] = 0;          //first process will execute immediately
            for (int i = 1; i < n; i++) {
                Waiting[i] = BurstTime[i - 1] + Waiting[i - 1]; //calculating the waiting time of each process = BT+WT of the previous process
            }
        }

        void findTAT(int[] processes, int n, int[] Waiting, int[] BurstTime, int[]TAT) {
            TAT[0] = BurstTime[0];
            for (int i = 1; i < n; i++) {
                TAT[i] = BurstTime[i] + Waiting[i];
            }
        
        }
    }
}
