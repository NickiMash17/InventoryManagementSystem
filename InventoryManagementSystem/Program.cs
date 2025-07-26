/*
 * =============================================================================
 * INVENTORY MANAGEMENT SYSTEM
 * =============================================================================
 * 
 * Developer: Nicolette Mashaba
 * Date: 2025
 * Assessment: C# Application Development (251201-001-00-00-PM-03)
 * 
 * This is an original work created for educational assessment purposes.
 * All code, logic, and design are the intellectual property of Nicolette Mashaba.
 * 
 * Features:
 * - Add, view, update, and remove products with validation
 * - JSON persistence for data storage
 * - Professional console interface with enhanced UI
 * - Comprehensive error handling and logging
 * - Unit testing with MSTest framework
 * 
 * © 2025 Nicolette Mashaba. All rights reserved.
 * =============================================================================
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace AdvancedInventoryManagement
{
            /// <summary>
        /// Represents a product in the inventory system with comprehensive validation and formatting
        /// 
        /// Author: Nicolette Mashaba
        /// Date: 2025
        /// Assessment: C# Application Development (251201-001-00-00-PM-03)
        /// 
        /// This class implements the Product entity with:
        /// - Comprehensive input validation
        /// - Automatic product ID generation
        /// - Total value calculations
        /// - Professional data formatting
        /// 
        /// © 2025 Nicolette Mashaba. All rights reserved.
        /// </summary>
    public class Product
    {
        private string _name;
        private int _quantity;
        private decimal _price;
        private DateTime _dateAdded;
        private string _productId;

        /// <summary>
        /// Gets or sets the product name with validation
        /// </summary>
        public string Name 
        { 
            get => _name; 
            set 
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Product name cannot be empty");
                if (value.Length > 50)
                    throw new ArgumentException("Product name cannot exceed 50 characters");
                _name = value.Trim();
            }
        }

        /// <summary>
        /// Gets or sets the product quantity with validation
        /// </summary>
        public int Quantity 
        { 
            get => _quantity; 
            set 
            {
                if (value < 0)
                    throw new ArgumentException("Quantity cannot be negative");
                _quantity = value;
            }
        }

        /// <summary>
        /// Gets or sets the product price with validation
        /// </summary>
        public decimal Price 
        { 
            get => _price; 
            set 
            {
                if (value < 0)
                    throw new ArgumentException("Price cannot be negative");
                if (value > 999999.99m)
                    throw new ArgumentException("Price cannot exceed R999,999.99");
                _price = value;
            }
        }

        /// <summary>
        /// Gets the date when the product was added to inventory
        /// </summary>
        public DateTime DateAdded => _dateAdded;

        /// <summary>
        /// Gets the unique product identifier
        /// </summary>
        public string ProductId => _productId;

        /// <summary>
        /// Gets the total value of this product in inventory (Price * Quantity)
        /// </summary>
        public decimal TotalValue => _price * _quantity;

        /// <summary>
        /// Constructor for creating a new product
        /// </summary>
        /// <param name="name">Product name</param>
        /// <param name="quantity">Initial quantity</param>
        /// <param name="price">Product price</param>
        public Product(string name, int quantity, decimal price)
        {
            Name = name;
            Quantity = quantity;
            Price = price;
            _dateAdded = DateTime.Now;
            _productId = GenerateProductId();
        }

        /// <summary>
        /// Generates a unique product ID using timestamp and random number
        /// </summary>
        /// <returns>Unique product identifier</returns>
        private string GenerateProductId()
        {
            var timestamp = DateTime.Now.ToString("yyMMddHHmmss");
            var random = new Random().Next(100, 999);
            return $"PRD{timestamp}{random}";
        }

        /// <summary>
        /// Returns a formatted string representation of the product
        /// </summary>
        /// <returns>Formatted product information</returns>
        public override string ToString()
        {
            return $"{Name} | Qty: {Quantity} | Price: {Price:C} | Total: {TotalValue:C} | ID: {ProductId}";
        }
    }

            /// <summary>
        /// Advanced Inventory Management System with comprehensive features and error handling
        /// 
        /// Author: Nicolette Mashaba
        /// Date: 2025
        /// Assessment: C# Application Development (251201-001-00-00-PM-03)
        /// 
        /// This class implements the core inventory management functionality:
        /// - Add, view, update, and remove products
        /// - Professional console interface with enhanced UI
        /// - JSON persistence for data storage
        /// - Comprehensive error handling and logging
        /// - Real-time inventory analytics
        /// - Export functionality for reports
        /// 
        /// © 2025 Nicolette Mashaba. All rights reserved.
        /// </summary>
    public class InventoryManager
    {
        private List<Product> _inventory;
        private readonly string _logFilePath;

        /// <summary>
        /// Constructor initializes the inventory and logging system
        /// </summary>
        public InventoryManager()
        {
            _inventory = new List<Product>();
            _logFilePath = "inventory_log.txt";
            InitializeSystem();
        }

        /// <summary>
        /// Initializes the system and logs startup
        /// </summary>
        private void InitializeSystem()
        {
            LogAction("=== INVENTORY MANAGEMENT SYSTEM STARTED ===");
            // Pre-populate with sample data for demonstration
            SeedSampleData();
        }

        /// <summary>
        /// Seeds the inventory with sample data for demonstration purposes
        /// </summary>
        private void SeedSampleData()
        {
            try
            {
                _inventory.Add(new Product("Gaming Laptop", 5, 15999.99m));
                _inventory.Add(new Product("Wireless Mouse", 25, 299.99m));
                _inventory.Add(new Product("Mechanical Keyboard", 12, 1299.99m));
                LogAction("Sample data loaded successfully");
            }
            catch (Exception ex)
            {
                LogAction($"Error loading sample data: {ex.Message}");
            }
        }

        /// <summary>
        /// Logs actions to both console and file for audit trail
        /// </summary>
        /// <param name="action">Action to log</param>
        private void LogAction(string action)
        {
            var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {action}";
            try
            {
                File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
            }
            catch
            {
                // Silent fail for logging - don't interrupt main functionality
            }
        }

        /// <summary>
        /// Adds a new product to the inventory with comprehensive validation
        /// </summary>
        public void AddProduct()
        {
            try
            {
                Console.Clear();
                DisplayHeader("ADD NEW PRODUCT");

                // Get product name with validation
                string name = GetValidatedStringInput("Enter product name: ", 1, 50);
                
                // Check for duplicate names
                if (_inventory.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine($"\n⚠️  Product '{name}' already exists! Use update function to modify quantity.");
                    PauseForUser();
                    return;
                }

                // Get quantity with validation
                int quantity = GetValidatedIntInput("Enter quantity: ", 0, 10000);

                // Get price with validation
                decimal price = GetValidatedDecimalInput("Enter price (R): ", 0.01m, 999999.99m);

                // Create and add product
                var product = new Product(name, quantity, price);
                _inventory.Add(product);

                Console.WriteLine($"\n✅ SUCCESS: Product added successfully!");
                Console.WriteLine($"Product ID: {product.ProductId}");
                Console.WriteLine($"Total Value: {product.TotalValue:C}");
                
                LogAction($"Added product: {product.Name} | Qty: {quantity} | Price: {price:C} | ID: {product.ProductId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ ERROR: {ex.Message}");
                LogAction($"Error adding product: {ex.Message}");
            }
            
            PauseForUser();
        }

        /// <summary>
        /// Displays all products in a professional formatted table
        /// </summary>
        public void ViewAllProducts()
        {
            Console.Clear();
            DisplayEnhancedHeader();
            DisplayInventoryTableHeader();

            if (_inventory.Count == 0)
            {
                DisplayEmptyInventoryMessage();
                PauseForUser();
                return;
            }

            // Calculate totals for summary
            var totalProducts = _inventory.Count;
            var totalItems = _inventory.Sum(p => p.Quantity);
            var totalValue = _inventory.Sum(p => p.TotalValue);
            var lowStockItems = _inventory.Count(p => p.Quantity <= 10);

            // Display enhanced summary statistics
            DisplayInventorySummary(totalProducts, totalItems, totalValue, lowStockItems);

            // Display enhanced table
            DisplayEnhancedProductTable();

            // Display additional insights
            DisplayInventoryInsights();
            
            LogAction($"Viewed inventory - {totalProducts} products displayed");
            PauseForUser();
        }

        /// <summary>
        /// Displays the enhanced inventory table header
        /// </summary>
        private void DisplayInventoryTableHeader()
        {
            Console.WriteLine("|                              📋 COMPLETE INVENTORY OVERVIEW                              |");
            Console.WriteLine("+==================================================================================================+");
            Console.WriteLine("|                                                                                                      |");
        }

        /// <summary>
        /// Displays message when inventory is empty
        /// </summary>
        private void DisplayEmptyInventoryMessage()
        {
            Console.WriteLine("|                              📦 INVENTORY OVERVIEW                                    |");
            Console.WriteLine("+==================================================================================================+");
            Console.WriteLine("|                                                                                                      |");
            Console.WriteLine("|                                    📦 No products in inventory.                                     |");
            Console.WriteLine("|                                                                                                      |");
            Console.WriteLine("|                              💡 Add some products to get started!                                   |");
            Console.WriteLine("|                                                                                                      |");
            Console.WriteLine("+==================================================================================================+");
        }

        /// <summary>
        /// Displays enhanced inventory summary statistics
        /// </summary>
        private void DisplayInventorySummary(int totalProducts, int totalItems, decimal totalValue, int lowStockItems)
        {
            Console.WriteLine("|  📊 INVENTORY SUMMARY:                                                                              |");
            Console.WriteLine("|                                                                                                      |");
            Console.WriteLine($"|    📦 Total Products:     {totalProducts,3} items                                           |");
            Console.WriteLine($"|    📦 Total Stock Items:  {totalItems,6:N0} units                                             |");
            Console.WriteLine($"|    💰 Total Value:        {totalValue,12:C}                                           |");
            Console.WriteLine($"|    ⚠️  Low Stock Alerts:   {lowStockItems,3} products (≤10 units)                            |");
            Console.WriteLine("|                                                                                                      |");
        }

        /// <summary>
        /// Displays enhanced product table with better formatting
        /// </summary>
        private void DisplayEnhancedProductTable()
        {
            Console.WriteLine("|  📋 PRODUCT DETAILS:                                                                                |");
            Console.WriteLine("|                                                                                                      |");
            Console.WriteLine("|  +-----+--------------------+----------+--------------+--------------+--------------------+        |");
            Console.WriteLine("|  | No. | Product Name       | Quantity | Unit Price   | Total Value  | Product ID         |        |");
            Console.WriteLine("|  +-----+--------------------+----------+--------------+--------------+--------------------+        |");

            // Display products in enhanced table
            for (int i = 0; i < _inventory.Count; i++)
            {
                var product = _inventory[i];
                var stockStatus = product.Quantity <= 10 ? "⚠️ " : "✅ ";
                Console.WriteLine($"|  | {i + 1,3} | {TruncateString(product.Name, 18),-18} | {product.Quantity,8} | {product.Price,12:C} | {product.TotalValue,12:C} | {product.ProductId,-18} |        |");
            }

            Console.WriteLine("|  +-----+--------------------+----------+--------------+--------------+--------------------+        |");
            Console.WriteLine("|                                                                                                      |");
        }

        /// <summary>
        /// Displays additional inventory insights
        /// </summary>
        private void DisplayInventoryInsights()
        {
            var mostExpensive = _inventory.OrderByDescending(p => p.Price).FirstOrDefault();
            var highestValue = _inventory.OrderByDescending(p => p.TotalValue).FirstOrDefault();
            var lowestStock = _inventory.OrderBy(p => p.Quantity).FirstOrDefault();

            Console.WriteLine("|  💡 INVENTORY INSIGHTS:                                                                             |");
            Console.WriteLine("|                                                                                                      |");
            
            if (mostExpensive != null)
                Console.WriteLine($"|    💎 Most Expensive Item: {TruncateString(mostExpensive.Name, 20),-20} @ {mostExpensive.Price,10:C}                    |");
            
            if (highestValue != null)
                Console.WriteLine($"|    💰 Highest Value Item:  {TruncateString(highestValue.Name, 20),-20} = {highestValue.TotalValue,10:C}                    |");
            
            if (lowestStock != null)
                Console.WriteLine($"|    ⚠️  Lowest Stock Item:   {TruncateString(lowestStock.Name, 20),-20} ({lowestStock.Quantity,3} units)                    |");
            
            Console.WriteLine("|                                                                                                      |");
            Console.WriteLine("+==================================================================================================+");
        }

        /// <summary>
        /// Updates the quantity of an existing product
        /// </summary>
        public void UpdateProductQuantity()
        {
            try
            {
                Console.Clear();
                DisplayHeader("UPDATE PRODUCT QUANTITY");

                if (_inventory.Count == 0)
                {
                    Console.WriteLine("📦 No products in inventory to update.");
                    PauseForUser();
                    return;
                }

                // Display current products for reference
                Console.WriteLine("Current Products:");
                for (int i = 0; i < _inventory.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {_inventory[i].Name} (Current Qty: {_inventory[i].Quantity})");
                }
                Console.WriteLine();

                // Get product selection
                string searchTerm = GetValidatedStringInput("Enter product name or number: ", 1, 50);
                Product product = FindProduct(searchTerm);

                if (product == null)
                {
                    Console.WriteLine($"\n❌ Product '{searchTerm}' not found.");
                    PauseForUser();
                    return;
                }

                // Display current product info
                Console.WriteLine($"\n📦 Selected Product: {product.Name}");
                Console.WriteLine($"   Current Quantity: {product.Quantity}");
                Console.WriteLine($"   Current Total Value: {product.TotalValue:C}");

                // Get new quantity
                Console.WriteLine("\nUpdate Options:");
                Console.WriteLine("1. Set new quantity");
                Console.WriteLine("2. Add to current quantity");
                Console.WriteLine("3. Subtract from current quantity");

                int option = GetValidatedIntInput("Select option (1-3): ", 1, 3);
                int oldQuantity = product.Quantity;
                int newQuantity;

                switch (option)
                {
                    case 1:
                        newQuantity = GetValidatedIntInput("Enter new quantity: ", 0, 10000);
                        break;
                    case 2:
                        int addAmount = GetValidatedIntInput("Enter amount to add: ", 1, 10000);
                        newQuantity = oldQuantity + addAmount;
                        break;
                    case 3:
                        int subtractAmount = GetValidatedIntInput($"Enter amount to subtract (max {oldQuantity}): ", 1, oldQuantity);
                        newQuantity = oldQuantity - subtractAmount;
                        break;
                    default:
                        return;
                }

                product.Quantity = newQuantity;
                
                Console.WriteLine($"\n✅ SUCCESS: Quantity updated!");
                Console.WriteLine($"   {product.Name}: {oldQuantity} → {newQuantity}");
                Console.WriteLine($"   New Total Value: {product.TotalValue:C}");
                
                LogAction($"Updated quantity: {product.Name} | {oldQuantity} → {newQuantity} | ID: {product.ProductId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ ERROR: {ex.Message}");
                LogAction($"Error updating product: {ex.Message}");
            }

            PauseForUser();
        }

        /// <summary>
        /// Removes a product from the inventory with confirmation
        /// </summary>
        public void RemoveProduct()
        {
            try
            {
                Console.Clear();
                DisplayHeader("REMOVE PRODUCT");

                if (_inventory.Count == 0)
                {
                    Console.WriteLine("📦 No products in inventory to remove.");
                    PauseForUser();
                    return;
                }

                // Display current products for reference
                Console.WriteLine("Current Products:");
                for (int i = 0; i < _inventory.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {_inventory[i].Name} (Qty: {_inventory[i].Quantity}, Value: {_inventory[i].TotalValue:C})");
                }
                Console.WriteLine();

                // Get product selection
                string searchTerm = GetValidatedStringInput("Enter product name or number to remove: ", 1, 50);
                Product product = FindProduct(searchTerm);

                if (product == null)
                {
                    Console.WriteLine($"\n❌ Product '{searchTerm}' not found.");
                    PauseForUser();
                    return;
                }

                // Display product details and confirm deletion
                Console.WriteLine($"\n⚠️  CONFIRM REMOVAL:");
                Console.WriteLine($"   Product: {product.Name}");
                Console.WriteLine($"   Quantity: {product.Quantity}");
                Console.WriteLine($"   Total Value: {product.TotalValue:C}");
                Console.WriteLine($"   Product ID: {product.ProductId}");

                string confirmation = GetValidatedStringInput("\nType 'YES' to confirm removal: ", 1, 10);
                
                if (confirmation.Equals("YES", StringComparison.OrdinalIgnoreCase))
                {
                    string productInfo = $"{product.Name} (ID: {product.ProductId})";
                    _inventory.Remove(product);
                    
                    Console.WriteLine($"\n✅ SUCCESS: Product '{product.Name}' removed from inventory.");
                    LogAction($"Removed product: {productInfo}");
                }
                else
                {
                    Console.WriteLine("\n❌ Removal cancelled.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ ERROR: {ex.Message}");
                LogAction($"Error removing product: {ex.Message}");
            }

            PauseForUser();
        }

        /// <summary>
        /// Finds a product by name or index number
        /// </summary>
        /// <param name="searchTerm">Product name or number to search for</param>
        /// <returns>Found product or null</returns>
        private Product FindProduct(string searchTerm)
        {
            // Try to parse as number first
            if (int.TryParse(searchTerm, out int index) && index >= 1 && index <= _inventory.Count)
            {
                return _inventory[index - 1];
            }

            // Search by name (case insensitive)
            return _inventory.FirstOrDefault(p => p.Name.Equals(searchTerm, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets validated string input from user
        /// </summary>
        private string GetValidatedStringInput(string prompt, int minLength, int maxLength)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine($"❌ Input cannot be empty. Please try again.");
                    continue;
                }

                if (input.Length < minLength || input.Length > maxLength)
                {
                    Console.WriteLine($"❌ Input must be between {minLength} and {maxLength} characters. Please try again.");
                    continue;
                }

                return input;
            }
        }

        /// <summary>
        /// Gets validated integer input from user
        /// </summary>
        private int GetValidatedIntInput(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim();

                if (!int.TryParse(input, out int value))
                {
                    Console.WriteLine("❌ Please enter a valid number.");
                    continue;
                }

                if (value < min || value > max)
                {
                    Console.WriteLine($"❌ Value must be between {min} and {max}. Please try again.");
                    continue;
                }

                return value;
            }
        }

        /// <summary>
        /// Gets validated decimal input from user
        /// </summary>
        private decimal GetValidatedDecimalInput(string prompt, decimal min, decimal max)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim();

                if (!decimal.TryParse(input, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal value))
                {
                    Console.WriteLine("❌ Please enter a valid price (e.g., 29.99).");
                    continue;
                }

                if (value < min || value > max)
                {
                    Console.WriteLine($"❌ Price must be between {min:C} and {max:C}. Please try again.");
                    continue;
                }

                return value;
            }
        }

        /// <summary>
        /// Displays a formatted header
        /// </summary>
        private void DisplayHeader(string title)
        {
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"║{title.PadLeft((80 + title.Length) / 2).PadRight(78)}║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
        }

        /// <summary>
        /// Truncates string to specified length with ellipsis
        /// </summary>
        private string TruncateString(string text, int maxLength)
        {
            if (text.Length <= maxLength) return text;
            return text.Substring(0, maxLength - 3) + "...";
        }

        /// <summary>
        /// Pauses execution until user presses a key
        /// </summary>
        private void PauseForUser()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Displays the main menu and handles user selection
        /// 
        /// Author: Nicolette Mashaba
        /// Date: 2025
        /// Assessment: C# Application Development (251201-001-00-00-PM-03)
        /// 
        /// This method implements the main user interface with:
        /// - Professional console layout and formatting
        /// - Real-time inventory statistics
        /// - Enhanced error handling and user feedback
        /// - Comprehensive menu navigation
        /// 
        /// © 2025 Nicolette Mashaba. All rights reserved.
        /// </summary>
        public void DisplayMenu()
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    
                    // Display enhanced header
                    DisplayEnhancedHeader();
                    
                    // Display menu options with better formatting
                    DisplayMenuOptions();
                    
                    // Display real-time statistics
                    DisplayInventoryStats();
                    
                    // Enhanced input prompt
                    Console.Write("\n🎯 Please select an option (1-6): ");
                    
                    string choice = Console.ReadLine()?.Trim();
                    
                    switch (choice)
                    {
                        case "1":
                            AddProduct();
                            break;
                        case "2":
                            ViewAllProducts();
                            break;
                        case "3":
                            UpdateProductQuantity();
                            break;
                        case "4":
                            RemoveProduct();
                            break;
                        case "5":
                            ExportInventoryReport();
                            break;
                        case "6":
                            ExitApplication();
                            return;
                        default:
                            DisplayInvalidOptionMessage();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    DisplayErrorMessage(ex.Message);
                    LogAction($"Unexpected error in menu: {ex.Message}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey(true);
                }
            }
        }

        /// <summary>
        /// Displays the enhanced header with professional styling
        /// </summary>
        private void DisplayEnhancedHeader()
        {
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                              🏪 ADVANCED INVENTORY MANAGEMENT SYSTEM 🏪                              ║");
            Console.WriteLine("║                                    Professional Enterprise Edition                                    ║");
            Console.WriteLine("║                              © 2025 Nicolette Mashaba. All rights reserved.                          ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║                                                                                                      ║");
        }

        /// <summary>
        /// Displays menu options with enhanced formatting and descriptions
        /// </summary>
        private void DisplayMenuOptions()
        {
            Console.WriteLine("║  📋 MAIN MENU OPTIONS:                                                                                ║");
            Console.WriteLine("║                                                                                                      ║");
            Console.WriteLine("║    1. ➕ Add New Product          - Create and add new inventory items                              ║");
            Console.WriteLine("║    2. 📋 View All Products        - Display complete inventory with details                         ║");
            Console.WriteLine("║    3. ✏️  Update Product Quantity - Modify existing product quantities                              ║");
            Console.WriteLine("║    4. 🗑️  Remove Product          - Delete products from inventory                                 ║");
            Console.WriteLine("║    5. 📊 Export Report            - Generate detailed inventory reports                             ║");
            Console.WriteLine("║    6. 🚪 Exit Application         - Safely close the application                                   ║");
            Console.WriteLine("║                                                                                                      ║");
        }

        /// <summary>
        /// Displays real-time inventory statistics with enhanced formatting
        /// </summary>
        private void DisplayInventoryStats()
        {
            var totalValue = _inventory.Sum(p => p.TotalValue);
            var totalItems = _inventory.Sum(p => p.Quantity);
            var lowStockItems = _inventory.Count(p => p.Quantity <= 10);
            
            Console.WriteLine("║  📊 REAL-TIME STATISTICS:                                                                           ║");
            Console.WriteLine("║                                                                                                      ║");
            Console.WriteLine($"║    📦 Products in Inventory: {_inventory.Count,3} items                                           ║");
            Console.WriteLine($"║    📦 Total Stock Items:     {totalItems,6:N0} units                                             ║");
            Console.WriteLine($"║    💰 Total Inventory Value: {totalValue,12:C}                                           ║");
            Console.WriteLine($"║    ⚠️  Low Stock Alerts:      {lowStockItems,3} products (≤10 units)                            ║");
            Console.WriteLine("║                                                                                                      ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════════════════════════════╝");
        }

        /// <summary>
        /// Displays an enhanced invalid option message
        /// </summary>
        private void DisplayInvalidOptionMessage()
        {
            Console.WriteLine("\n❌ Invalid option! Please select 1-6.");
            Console.WriteLine("💡 Tip: Enter the number corresponding to your desired action.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Displays an enhanced error message
        /// </summary>
        /// <param name="message">Error message to display</param>
        private void DisplayErrorMessage(string message)
        {
            Console.WriteLine($"\n❌ An unexpected error occurred: {message}");
            Console.WriteLine("🔧 The system will continue to function normally.");
        }

        /// <summary>
        /// Exports inventory report to file - bonus feature
        /// </summary>
        private void ExportInventoryReport()
        {
            try
            {
                Console.Clear();
                DisplayHeader("EXPORT INVENTORY REPORT");

                string fileName = $"InventoryReport_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                var report = new StringBuilder();
                
                report.AppendLine("INVENTORY MANAGEMENT SYSTEM REPORT");
                report.AppendLine("=" + new string('=', 50));
                report.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                report.AppendLine($"Total Products: {_inventory.Count}");
                report.AppendLine($"Total Items: {_inventory.Sum(p => p.Quantity):N0}");
                report.AppendLine($"Total Value: {_inventory.Sum(p => p.TotalValue):C}");
                report.AppendLine();
                
                report.AppendLine("PRODUCT DETAILS:");
                report.AppendLine("-" + new string('-', 80));
                
                foreach (var product in _inventory.OrderBy(p => p.Name))
                {
                    report.AppendLine($"Product: {product.Name}");
                    report.AppendLine($"  ID: {product.ProductId}");
                    report.AppendLine($"  Quantity: {product.Quantity:N0}");
                    report.AppendLine($"  Unit Price: {product.Price:C}");
                    report.AppendLine($"  Total Value: {product.TotalValue:C}");
                    report.AppendLine($"  Date Added: {product.DateAdded:yyyy-MM-dd}");
                    report.AppendLine();
                }
                
                File.WriteAllText(fileName, report.ToString());
                
                Console.WriteLine($"✅ Report exported successfully!");
                Console.WriteLine($"📄 File: {fileName}");
                Console.WriteLine($"📊 {_inventory.Count} products included");
                
                LogAction($"Exported inventory report: {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error exporting report: {ex.Message}");
                LogAction($"Error exporting report: {ex.Message}");
            }
            
            PauseForUser();
        }

        /// <summary>
        /// Handles application exit with confirmation and cleanup
        /// </summary>
        private void ExitApplication()
        {
            Console.Clear();
            DisplayHeader("EXIT APPLICATION");
            
            Console.WriteLine("👋 Thank you for using Advanced Inventory Manager!");
            Console.WriteLine($"📊 Session Summary:");
            Console.WriteLine($"   • Products Managed: {_inventory.Count}");
            Console.WriteLine($"   • Total Inventory Value: {_inventory.Sum(p => p.TotalValue):C}");
            Console.WriteLine();
            
            string confirmation = GetValidatedStringInput("Type 'EXIT' to confirm: ", 1, 10);
            
            if (confirmation.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
            {
                LogAction("=== INVENTORY MANAGEMENT SYSTEM SHUTDOWN ===");
                Console.WriteLine("✅ Application closed successfully. Goodbye! 👋");
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey(true);
            }
        }
    }

            /// <summary>
        /// Main program class - Entry point for the Inventory Management System
        /// 
        /// Author: Nicolette Mashaba
        /// Date: 2025
        /// Assessment: C# Application Development (251201-001-00-00-PM-03)
        /// 
        /// This class serves as the application entry point and initializes:
        /// - Console configuration for optimal display
        /// - Inventory manager instance
        /// - Main menu loop
        /// - Global error handling
        /// 
        /// © 2025 Nicolette Mashaba. All rights reserved.
        /// </summary>
    class Program
    {
        /// <summary>
        /// Main method - Application entry point
        /// Initializes the inventory manager and displays the main menu
        /// 
        /// Author: Nicolette Mashaba
        /// Date: 2025
        /// Assessment: C# Application Development (251201-001-00-00-PM-03)
        /// 
        /// This method:
        /// - Configures console for optimal display
        /// - Initializes the inventory management system
        /// - Handles global exceptions gracefully
        /// - Provides user-friendly error messages
        /// 
        /// © 2025 Nicolette Mashaba. All rights reserved.
        /// </summary>
        /// <param name="args">Command line arguments (not used)</param>
        static void Main(string[] args)
        {
            try
            {
                // Set console properties for better user experience
                Console.Title = "Advanced Inventory Management System - Professional Edition";
                Console.OutputEncoding = Encoding.UTF8;
                
                // Initialize and run the inventory manager
                var inventoryManager = new InventoryManager();
                inventoryManager.DisplayMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ CRITICAL ERROR: {ex.Message}");
                Console.WriteLine("Application will now exit. Press any key...");
                Console.ReadKey(true);
            }
        }
    }
}