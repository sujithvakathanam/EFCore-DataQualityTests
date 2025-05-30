using NUnit.Framework;
using EFCore.DataAccess.Data;
using EFCore_DataQuality.Utils;

namespace EFCore_DataQuality.SalesLT
{
    [TestFixture]
    public class SalesOrderDetailTests
    {
        private string? _connectionString;

        [SetUp]
        public void Setup()
        {
            _connectionString = DBHelper.GetConnectionString();
        }

        /* RowCount*/
        [Test]
        public void SalesOrderDetailRowCountGreaterThanZero()
        {
            using var context = new AdventureWorksLT2016Context(_connectionString);
            var actualCount = context.SalesOrderDetails.GetRowCount();
            Assert.That(actualCount, Is.GreaterThan(0));
        }

        /* Regression Test- Historical data checks*/
        [Test]
        public void ValidateTotalPrice()
        {
            var expectedTotalPrice = 713.796000;
            using var context = new AdventureWorksLT2016Context(_connectionString);
            var totalPrice = context.SalesOrderDetails
                .Where(salesOrderDetail =>
                salesOrderDetail.SalesOrderId == 71774)
                .GroupBy(sodetail => sodetail.SalesOrderId)
                .Select(sr => sr.Sum(s => s.LineTotal))
                .FirstOrDefault();
            Assert.That(totalPrice, Is.EqualTo(expectedTotalPrice));
        }

        /* Regression Test-To check the financial values across multiple rows from a resultant table */
        [TestCaseSource(nameof(GetExpectedRowData))]
        public void ValidateLineTotal(ExpectedRow expectedRow)
        {
            using var context = new AdventureWorksLT2016Context(_connectionString);
            var salesOrderDetailItems = context.SalesOrderDetails
                    .Where(sod => sod.SalesOrderId == expectedRow.SalesOrderID && sod.SalesOrderDetailId == expectedRow.SalesOrderDetailID)
                    .Join(
                        context.Products,
                        sod => sod.ProductId,
                        p => p.ProductId,
                        (sod, p) => new
                        {
                            sod.SalesOrderId,
                            sod.SalesOrderDetailId,
                            sod.ProductId,
                            sod.UnitPrice,
                            sod.LineTotal,
                            p.Name,
                            p.Color
                        }
                    )
                    .OrderBy(result => result.SalesOrderId)
                    .ToList();
            
            Assert.That(salesOrderDetailItems[0].SalesOrderDetailId, Is.EqualTo(expectedRow.SalesOrderDetailID));
            Assert.That(salesOrderDetailItems[0].SalesOrderId, Is.EqualTo(expectedRow.SalesOrderID));
            Assert.That(salesOrderDetailItems[0].ProductId, Is.EqualTo(expectedRow.ProductID));
            Assert.That(salesOrderDetailItems[0].UnitPrice, Is.EqualTo(expectedRow.UnitPrice));
            Assert.That(salesOrderDetailItems[0].LineTotal, Is.EqualTo(expectedRow.LineTotal));
            Assert.That(salesOrderDetailItems[0].Name, Is.EqualTo(expectedRow.Name));
            Assert.That(salesOrderDetailItems[0].Color, Is.EqualTo(expectedRow.Color));
        }

        public static IEnumerable<TestCaseData> GetExpectedRowData()
        {
            yield return new TestCaseData(new ExpectedRow { SalesOrderID = 71774, SalesOrderDetailID = 110562, ProductID = 836, UnitPrice = 356.898m, LineTotal = 356.898000m, Name = "ML Road Frame-W - Yellow, 48", Color = "Yellow" });
            yield return new TestCaseData(new ExpectedRow { SalesOrderID = 71783, SalesOrderDetailID = 110741, ProductID = 836, UnitPrice = 356.898m, LineTotal = 1427.592000m, Name = "ML Road Frame-W - Yellow, 48", Color = "Yellow" });
            yield return new TestCaseData(new ExpectedRow { SalesOrderID = 71797, SalesOrderDetailID = 111081, ProductID = 836, UnitPrice = 356.898m, LineTotal = 1427.592000m, Name = "ML Road Frame-W - Yellow, 48", Color = "Yellow" });
            yield return new TestCaseData(new ExpectedRow { SalesOrderID = 71915, SalesOrderDetailID = 113089, ProductID = 836, UnitPrice = 356.898m, LineTotal = 1070.694000m, Name = "ML Road Frame-W - Yellow, 48", Color = "Yellow" });
        }

        public class ExpectedRow
        {
            public int SalesOrderID { get; set; }
            public int SalesOrderDetailID { get; set; }
            public int ProductID { get; set; }

            public decimal UnitPrice { get; set; }

            public decimal LineTotal { get; set; }

            public string Name { get; set; }

            public string Color { get; set; }
        }
    }
}
