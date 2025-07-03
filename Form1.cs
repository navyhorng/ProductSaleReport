using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ProductSaleReport
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            btnGenerate.Click += new System.EventHandler(OnClickGenerate);

        }
        private List<SaleDto> GetSaleData(DateTime startDate, DateTime endDate)
        {
            var sales = new List<SaleDto>();
            string cnnstr = @"server=MSI\NNNNN;database=productdb;trusted_connection=true;encrypt=false";
            string query = @"
                  SELECT PRODUCTCODE, PRODUCTNAME, QUANTITY, UNITPRICE, SALEDATE
                  FROM PRODUCTSALES
                  WHERE SALEDATE >= @STARTDATE AND SALEDATE < DATEADD(day, 1, @ENDDATE)";
            
            try
            {
                SqlConnection conn = new SqlConnection(cnnstr);
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@STARTDATE", startDate.Date);
                cmd.Parameters.AddWithValue("@ENDDATE", endDate.Date);
                conn.Open();

                

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sales.Add(new SaleDto
                    {
                        ProductCode = reader["PRODUCTCODE"].ToString(),
                        ProductName = reader["PRODUCTNAME"].ToString(),
                        Quantity = Convert.ToInt32(reader["QUANTITY"]),
                        UnitPrice = Convert.ToDecimal(reader["UNITPRICE"]),
                        SaleDate = Convert.ToDateTime(reader["SALEDATE"])
                    });
                }

            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                MessageBox.Show("Error fetching data. See logs/errors.txt");

            }
            return sales;
        }
        private void OnClickGenerate(object sender, EventArgs e)
        {
            var data = GetSaleData(dtpStart.Value.Date, dtpEnd.Value.Date);
            
            if (data.Count == 0)
            {
                MessageBox.Show($"No sales data found", "warning");
                return;
            }
            MessageBox.Show(data.Count.ToString());//check data record
            var report = new ProductSalesReport1();
            report.ObjectDataSource.DataSource = data;
            //report.DataMember = string.Empty;

            var tool = new DevExpress.XtraReports.UI.ReportPrintTool(report);
            tool.ShowPreviewDialog();
        }

       
    }
}
