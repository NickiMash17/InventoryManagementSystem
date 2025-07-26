using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdvancedInventoryManagement;

[TestClass]
public class InventoryManagerTests
{
    private InventoryManager _inventoryManager;

    [TestInitialize]
    public void Setup()
    {
        _inventoryManager = new InventoryManager();
    }

    [TestMethod]
    public void Product_ValidInput_CreatesSuccessfully()
    {
        // Arrange & Act
        var product = new Product("Laptop", 10, 999.99m);
        
        // Assert
        Assert.AreEqual("Laptop", product.Name);
        Assert.AreEqual(10, product.Quantity);
        Assert.AreEqual(999.99m, product.Price);
        Assert.IsNotNull(product.ProductId);
    }

    [TestMethod]
    public void Product_InvalidName_ThrowsException()
    {
        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => 
            new Product("", 10, 999.99m));
    }

    [TestMethod]
    public void Product_InvalidQuantity_ThrowsException()
    {
        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => 
            new Product("Laptop", -5, 999.99m));
    }

    [TestMethod]
    public void Product_InvalidPrice_ThrowsException()
    {
        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => 
            new Product("Laptop", 10, -100m));
    }

    [TestMethod]
    public void Product_TotalValue_CalculatesCorrectly()
    {
        // Arrange
        var product = new Product("Laptop", 5, 1000m);
        
        // Act & Assert
        Assert.AreEqual(5000m, product.TotalValue);
    }
}