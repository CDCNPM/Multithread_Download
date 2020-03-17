using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;


namespace SampleProject
{
    public partial class FormGetDataWeb : Form
    {
        public FormGetDataWeb()
        {
            InitializeComponent();
        }

        private void FormGetDataWeb_Load(object sender, EventArgs e)
        {

        }

        public DataTable convert_HTML_to_DataTable(string url)
        {

            var web = new HtmlWeb();
            var doc = web.Load(url);

            var nodes = doc.DocumentNode.SelectNodes("//table/thead/tr");
            var bodys = doc.DocumentNode.SelectNodes("//table/tbody/tr");
            var table = new DataTable("MyTable");

            var headers = nodes[0]
                .Elements("th")
                .Select(th => th.InnerText.Trim());
            foreach (var header in headers)
            {
                table.Columns.Add(header);
            }

            var rows = bodys.Select(tr => tr
                .Elements("td")
                .Select(td => td.InnerText.Trim())
                .ToArray());
            foreach (var row in rows)
            {
                table.Rows.Add(row);
            }
            return table;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = convert_HTML_to_DataTable(txtUrl.Text);
            dataGridView1.DataSource = dt;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
