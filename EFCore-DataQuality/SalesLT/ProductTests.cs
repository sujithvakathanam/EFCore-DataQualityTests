using NUnit.Framework;
using EFCore.DataAccess.Data;
using EFCore_DataQuality.Utils;

namespace EFCore_DataQuality.SalesLT
{
    [TestFixture]
    public class ProductTests
    {
        private string? _connectionString;
        
        [SetUp]
        public void Setup()
        {
            _connectionString = DBHelper.GetConnectionString();
        }

        /* RowCount*/
        [Test]
        public void ProductRowCountGreaterThanZero()
        {
            using var context = new AdventureWorksLT2016Context(_connectionString);
            var actualCount = context.Products.GetRowCount();
            Assert.That(actualCount, Is.GreaterThan(0));
        }

        /* Duplicate Check*/
        [Test]
        public void NoDuplicateProductNumber()
        {
            using var context = new AdventureWorksLT2016Context(_connectionString);
            var duplicateProducts = context.Products
                .GroupBy(p => p.ProductNumber)
                .Where(g => g.Count() > 1)
                .Select(g => new
                            {
                                ProductNumber = g.Key,
                                DuplicateCount = g.Count()
                            })
                .ToList();
            Assert.That(duplicateProducts.Count, Is.EqualTo(0));
        }

        /* NullCheck*/
        [Test]
        public void ProductCategoryIdIsNotNull()
        {
            using var context = new AdventureWorksLT2016Context(_connectionString);
            var product = context.Products.FirstOrDefault(product => product.ProductCategoryId == null || product.ProductCategoryId.Equals(""));
            Assert.That(product, Is.Null);
        }
    }
}
