using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AISystemsLab1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < 3; i++)
                dataGridView1.Rows.Add();
            for (int i = 0; i < 4; i++)
                dataGridView2.Rows.Add();
            for (int i = 0; i < 4; i++)
                dataGridView3.Rows.Add();
            dataGridView1.Rows[0].Cells[0].Value = "0";
            dataGridView1.Rows[1].Cells[0].Value = "0,5";
            dataGridView1.Rows[2].Cells[0].Value = "1";
            dataGridView2.Rows[0].Cells[0].Value = "0";
            dataGridView2.Rows[1].Cells[0].Value = "0,5";
            dataGridView2.Rows[2].Cells[0].Value = "1";
            dataGridView2.Rows[3].Cells[0].Value = "0,2";
            dataGridView3.Rows[0].Cells[0].Value = "0";
            dataGridView3.Rows[1].Cells[0].Value = "0,5";
            dataGridView3.Rows[2].Cells[0].Value = "1";
            dataGridView3.Rows[3].Cells[0].Value = "0,2";
            chart1.Series.Clear();
            chart1.Series.Add("Множество A");
            chart1.Series.Add("Множество B");
            chart1.Series.Add("Множество C");
            foreach (var item in chart1.Series)
            {
                item.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = (TextBox)e.Control;
            tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
        }

        private void dataGridView2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = (TextBox)e.Control;
            tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
        }

        void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar)) && !((e.KeyChar == ',')))
            {
                if (e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
            }
        }

        private void sum_Click(object sender, EventArgs e)
        {
            try
            {
                var a = fillA();
                var b = fillB();
                AddSetLevel(a, 0.2);
                var ans = SumSets(a, b);
                fillC(ans);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void subtract_Click(object sender, EventArgs e)
        {
            try
            {
                var a = fillA();
                var b = fillB();
                AddSetLevel(a, 0.2);
                var ans = SubtractSets(a, b);
                fillC(ans);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void multiply_Click(object sender, EventArgs e)
        {
            try
            {
                var a = fillA();
                var b = fillB();
                AddSetLevel(a, 0.2);
                var ans = MultiplySets(a, b);
                fillC(ans);
            }
            catch (Exception)
            {
                throw;
            }           
        }

        private void divide_Click(object sender, EventArgs e)
        {
            try
            {
                var a = fillA();
                var b = fillB();
                AddSetLevel(a, 0.2);
                if (!TryDivide(b)) 
                    throw new Exception("На 0 делить нельзя!");
                else
                {
                    var ans = DivideSets(a, b);
                    fillC(ans);
                }
            }
            catch (Exception)
            {
                throw;
            }            
        }
        private double[,] fillA()
        {
            double[,] a = new double[4, 3];
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (dataGridView1.Rows[i].Cells[j].Value == null)
                            throw new Exception("Не все ячейки заполнены!");
                        if (double.TryParse(dataGridView1.Rows[i].Cells[j].Value.ToString(), out double val))
                        {
                            a[i, j] = val;
                        }
                        else
                        {
                            throw new Exception("Не все ячейки - числа!");
                        }

                    }
                }
                a[3, 0] = 0.2;
                a[3, 1] = 0;
                a[3, 2] = 0;
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return a;
        }
        private double[,] fillB()
        {
            double[,] b = new double[4, 3];
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (dataGridView2.Rows[i].Cells[j].Value == null)
                            throw new Exception("Не все ячейки заполнены!");
                        if (double.TryParse(dataGridView2.Rows[i].Cells[j].Value.ToString(), out double val))
                        {
                            b[i, j] = val;
                        }
                        else
                        {
                            throw new Exception("Не все ячейки - числа!");
                        }

                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return b;
        }
        private void fillC(double[,] c)
        {
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        dataGridView3.Rows[i].Cells[j].Value = c[i, j];

                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void compare_Click(object sender, EventArgs e)
        {
            var a = fillA();
            var b = fillB();
            richTextBox1.Text = "Множество А " + GetCompare(a, b) + " множества B";
        }
        private void BuildGraph(string name, DataGridView dataGridView)
        {
            chart1.Series[name].Points.Clear();
            var points = new List<(double, double)>();
            try
            {
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.Cells[1].Value == null || row.Cells[2].Value == null)
                        throw new Exception("Не все ячейки заполнены!");
                    if (double.TryParse(row.Cells[1].Value.ToString(), out double X1) && double.TryParse(row.Cells[2].Value.ToString(), out double X2) && double.TryParse(row.Cells[0].Value.ToString(), out double Y))
                    {
                        points.Add((X1, Y));
                        points.Add((X2, Y));
                    }
                    else
                    {
                        throw new Exception("Не все ячейки - числа!");
                    }
                }
                points.Sort((x, y) => y.Item1.CompareTo(x.Item1));
                foreach (var point in points)
                {
                    chart1.Series[name].Points.AddXY(point.Item1, point.Item2);
                }

            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buildA_Click(object sender, EventArgs e)
        {
            BuildGraph("Множество A", dataGridView1);
        }

        private void buildB_Click(object sender, EventArgs e)
        {
            BuildGraph("Множество B", dataGridView2);
        }

        private void buildC_Click(object sender, EventArgs e)
        {
            BuildGraph("Множество C", dataGridView3);
        }

        private void clear_Click(object sender, EventArgs e)
        {
            foreach (var item in chart1.Series)
            {
                item.Points.Clear();
            }
        }
        public static string GetCompare(double[,] a, double[,] b)
        {
            if (Compare(a, b) == 1) { return (" < != <= "); }
            else if (Compare(a, b) == 2) { return (" > != >= "); }
            else if (Compare(a, b) == 3) { return (" = "); }
            return "?";
        }

        public static int Compare(double[,] a, double[,] b)
        {
            double asum = (a[0, 1] + a[0, 2] + a[1, 1] + a[1, 2] + a[2, 1] + a[2, 2]);
            double bsum = (b[0, 1] + b[0, 2] + b[1, 1] + b[1, 2] + b[2, 1] + b[2, 2] + b[3, 1] + b[3, 2]);
            bool sravup = (((1f / (3)) * asum) > ((1f / (4)) * bsum));
            bool sraveq = (((1f / (3)) * asum) == ((1f / (4)) * bsum));

            if ((sravup == false) && (sraveq == false)) { return 1; }// a < b a!=b
            else if ((sravup == true) && (sraveq == false)) { return 2; }// a > b a!=b
            else { return 3; }// a = b
        }

        public static double[,] MultiplySets(double[,] a, double[,] b)
        {
            double[,] ans = a;

            for (int i = 0; i < a.GetLength(0); i++)
            {
                ans[i, 1] = a[i, 1] * b[i, 1];
                ans[i, 2] = a[i, 2] * b[i, 2];
            }
            return ans;
        }

        public static bool TryDivide(double[,] b)
        {

            for (int i = 1; i < b.GetLength(0); i++)
            {
                for (int j = 0; j < b.GetLength(1); j++)
                {
                    if (b[i, j] == 0) 
                        return false;
                }
            }
            return true;
        }
        public static double[,] DivideSets(double[,] a, double[,] b)
        {
            double[,] ans = a;

            for (int i = 0; i < a.GetLength(0); i++)
            {
                ans[i, 1] = a[i, 1] / b[i, 2];
                ans[i, 2] = a[i, 2] / b[i, 1];
            }
            return ans;
        }
        static double[,] SumSets(double[,] setA, double[,] setB)
        {
            double[,] setC = new double[4, 3];

            setC[0, 0] = setB[0, 0];
            setC[1, 0] = setB[1, 0];
            setC[2, 0] = setB[2, 0];
            setC[3, 0] = setB[3, 0];

            for (int i = 0; i < setC.GetLength(0); i++)
            {
                setC[i, 1] = setA[i, 1] + setB[i, 1];
                setC[i, 2] = setA[i, 2] + setB[i, 2];
            }

            return setC;
        }

        static double[,] SubtractSets(double[,] setA, double[,] setB)
        {
            double[,] setC = new double[4, 3];

            setC[0, 0] = setB[0, 0];
            setC[1, 0] = setB[1, 0];
            setC[2, 0] = setB[2, 0];
            setC[3, 0] = setB[3, 0];

            for (int i = 0; i < setC.GetLength(0); i++)
            {
                setC[i, 1] = setA[i, 1] - setB[i, 2];
                setC[i, 2] = setA[i, 2] - setB[i, 1];
            }

            return setC;
        }
        static double IntersectionX(double x1, double y1, double x2, double y2, double newLevel)
        {
            if (x2 == x1) //вертикаль
            {
                return x1;
            }
            else
            {
                double kA = (y2 - y1) / (x2 - x1);
                double bA = -(x1 * y2 - x2 * y1) / (x2 - x1);
                double b2 = newLevel;
                double k2 = 0;

                return (b2 - bA) / (kA - k2);
            }
        }

        static void AddSetLevel(double[,] set, double newLevel)
        {
            double x1 = set[0, 1];
            double y1 = set[0, 0];
            double x2 = set[1, 1];
            double y2 = set[1, 0];

            double newXleft;
            double newXright;

            newXleft = IntersectionX(x1, y1, x2, y2, newLevel);
            x1 = set[0, 2];
            y1 = set[0, 0];
            x2 = set[1, 2];
            y2 = set[1, 0];

            newXright = IntersectionX(x1, y1, x2, y2, newLevel);

            set[set.GetLength(0) - 1, 0] = newLevel;
            set[set.GetLength(0) - 1, 1] = newXleft;
            set[set.GetLength(0) - 1, 2] = newXright;

        }
    }
}
