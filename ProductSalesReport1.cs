// ... (other using statements and code remain unchanged)

using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;

namespace ProductSaleReport
{
    public partial class ProductSalesReport1 : DevExpress.XtraReports.UI.XtraReport
    {
        public ObjectDataSource ObjectDataSource => objectDataSource1;

        public ProductSalesReport1()
        {
            InitializeComponent();

            // Ensure the summary function is used in the expression when SummaryRunning is set to Group
            XRLabel summaryLabel = new XRLabel();
            summaryLabel.ExpressionBindings.Add(
                new ExpressionBinding("BeforePrint", "Text", "sumSum([YourFieldName])")
            );
            summaryLabel.Summary = new XRSummary
            {
                Running = SummaryRunning.Group,
                Func = SummaryFunc.Sum,
                IgnoreNullValues = true
            };

            this.Bands[BandKind.Detail].Controls.Add(summaryLabel);
        }
    }
}