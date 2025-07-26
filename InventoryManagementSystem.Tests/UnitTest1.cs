/*
 * =============================================================================
 * INVENTORY MANAGEMENT SYSTEM - UNIT TESTS
 * =============================================================================
 * 
 * Developer: Nicolette Mashaba
 * Date: 2025
 * Assessment: C# Application Development (251201-001-00-00-PM-03)
 * 
 * This test suite validates the core functionality of the Inventory Management System.
 * All tests are designed to ensure proper validation, error handling, and data integrity.
 * 
 * Test Coverage:
 * - Product creation and validation
 * - Input validation for all data types
 * - Error handling for invalid inputs
 * - Total value calculations
 * - Product ID generation
 * 
 * © 2025 Nicolette Mashaba. All rights reserved.
 * =============================================================================
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdvancedInventoryManagement;

/// <summary>
/// Comprehensive test suite for the Inventory Management System
/// 
/// Author: Nicolette Mashaba
/// Date: 2025
/// Assessment: C# Application Development (251201-001-00-00-PM-03)
/// 
/// This test class validates all core functionality including:
/// - Product creation and validation
/// - Input validation for all data types
/// - Error handling for invalid inputs
/// - Total value calculations
/// - Product ID generation
/// 
/// © 2025 Nicolette Mashaba. All rights reserved.
/// </summary>
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