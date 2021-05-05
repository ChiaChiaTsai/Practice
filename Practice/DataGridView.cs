using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Practice.Properties;

namespace Practice
{
    public partial class DataGridView : Form
    {
        public DataGridView()
        {
            InitializeComponent();
        }
        NorthwindEntities dbcontext = new NorthwindEntities();


        private void button1_Click(object sender, EventArgs e)
        {
            var years = this.dbcontext.Orders.Select(n => n.OrderDate.Value.Year);
            var q = from p in this.dbcontext.Order_Details.AsEnumerable()
                    group p by p.Order.OrderDate.Value.Year into g
                    select new { Year=g.Key, Sum = $"{ g.Sum(n => n.UnitPrice * n.Quantity * (1 - (decimal)n.Discount)):c2}" };
            this.dataGridView1.DataSource = q.ToList();
            this.dataGridView1.DefaultCellStyle.Font = new Font("新細明體", 14);
            this.chart1.DataSource = q.ToList();
            this.chart1.Series[0].XValueMember = "Year";
            this.chart1.Series[0].YValueMembers = "Sum";
            this.chart1.Series[0].IsValueShownAsLabel = true;
            this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int Y1 = 0;



            if (e.ColumnIndex == 0)
            {
                Y1 = (int)this.dataGridView1.Rows[e.RowIndex].Cells["Year"].Value;
                var TotalS = this.dbcontext.Order_Details.AsEnumerable().Where(n => n.Order.OrderDate.Value.Year == Y1).Sum(n => n.UnitPrice * n.Quantity * (1 - (decimal)n.Discount));
                Getgrid(Y1, TotalS);
            }



        }

        private void Getgrid(int y1,decimal TotalS)
        {
            var q = from p in this.dbcontext.Order_Details.AsEnumerable()
                    group p by new { Year = p.Order.OrderDate.Value.Year, CID = p.Product.CategoryID } into g
                    where g.Key.Year == y1
                    orderby g.Key.CID
                    select new { Year = g.Key.Year, CategoryID = g.Key.CID, Sum = $"{ g.Sum(n => n.UnitPrice * n.Quantity * (1 - (decimal)n.Discount)):c2}" ,Percent= $"{ g.Sum(n => n.UnitPrice * n.Quantity * (1 - (decimal)n.Discount)) / TotalS:P1}" };
            this.dataGridView2.DataSource = q.ToList();
            this.chart1.DataSource = q.ToList();
            this.chart1.Series[0].XValueMember = "CategoryID";
            this.chart1.Series[0].YValueMembers = "Sum";
            this.chart1.Series[0].IsValueShownAsLabel = false;
            this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            
        }
    }
}
