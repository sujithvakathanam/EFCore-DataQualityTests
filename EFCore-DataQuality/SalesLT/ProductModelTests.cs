using NUnit.Framework;
using EFCore.DataAccess.Data;
using EFCore_DataQuality.Utils;

namespace EFCore_DataQuality.SalesLT
{
    [TestFixture]
    public class ProductModelTests
    {
        private string? _connectionString;
        
        [SetUp]
        public void Setup()
        {
            _connectionString = DBHelper.GetConnectionString();
        }


        /* RowCount*/
        [Test]
        public void ProductModelRowCountGreaterThanZero()
        {
            using var context = new AdventureWorksLT2016Context(_connectionString);
            var actualCount = context.ProductModels.GetRowCount();
            Assert.That(actualCount, Is.GreaterThan(0));
        }

        /* NullCheck*/
        [Test]
        public void ProductModelNameIsNotNull()
        {
            using var context = new AdventureWorksLT2016Context(_connectionString);
            var product = context.ProductModels.FirstOrDefault(productModel => productModel.Name == null || productModel.Name.Equals(""));
            Assert.That(product, Is.Null);
        }
    }
}
