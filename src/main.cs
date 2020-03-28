using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

using Cell.Automata;
using Cell.Typedefs;


public class App {
  public static void Main(string[] args) {
    if (args.Length != 1 && args.Length != 2) {
      Console.WriteLine("Usage: northwind <input dataset> [<saved dataset copy>]");
      return;
    }

    // Creating the automaton
    Northwind northwind = new Northwind();

    string path = args[0];

    if (File.Exists(path)) {
      // Loading the initial state
      using (Stream stream = new FileStream(args[0], FileMode.Open)) {
        northwind.Load(stream);
      }
    }
    else if (Directory.Exists(path)) {
      // Importing the data from the CSV files
      ImportCsvData(northwind, args[0]);
    }
    else {
      Console.WriteLine("No such file or directory: {0}", path);
      return;
    }

    RunQueries(northwind);

    if (args.Length == 2) {
      // Saving the final state to the indicated file
      using (Stream stream = new FileStream(args[1], FileMode.Create)) {
        northwind.Save(stream);
      }
    }
  }

  //////////////////////////////////////////////////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////

  private static void ImportCsvData(Northwind northwind, string path) {
    ReadCategories(northwind, path);
    ReadSuppliers(northwind, path);
    ReadRegions(northwind, path);
    ReadTerritories(northwind, path);
    ReadShippers(northwind, path);
    ReadCustomers(northwind, path);
    ReadEmployees(northwind, path);
    ReadEmployeeTerritories(northwind, path);
    ReadProducts(northwind, path);
    ReadOrders(northwind, path);
    ReadOrderDetails(northwind, path);
  }


  private static void ReadCategories(Northwind northwind, string path) {
    CsvReader reader = new CsvReader(File.ReadAllBytes(path + "/Categories.csv"));

    reader.SkipLine();
    while (!reader.Eof()) {
      long id = reader.ReadLong();
      reader.Skip(';');
      string name = reader.ReadString();
      reader.Skip(';');
      string description = reader.ReadString();
      reader.SkipLine();

      northwind.AddCategory(id: id, name: name, description: description);
    }
  }


  private static void ReadSuppliers(Northwind northwind, string path) {
    CsvReader reader = new CsvReader(File.ReadAllBytes(path + "/Suppliers.csv"));

    reader.SkipLine();
    while (!reader.Eof()) {
      long id = reader.ReadLong();
      reader.Skip(';');
      string companyName = reader.ReadString();
      reader.Skip(';');
      string contactName = reader.ReadString();
      reader.Skip(';');
      string contactTitle = reader.ReadString();
      reader.Skip(';');
      string address = reader.ReadString();
      reader.Skip(';');
      string city = reader.ReadString();
      reader.Skip(';');
      string regionName = reader.ReadString();
      reader.Skip(';');
      string postalCode = reader.ReadString();
      reader.Skip(';');
      string country = reader.ReadString();
      reader.Skip(';');
      string phone = reader.ReadString();
      reader.Skip(';');
      string fax = reader.ReadString();
      reader.Skip(';');
      string homePage = reader.ReadString();
      reader.NextLine();

      northwind.AddSupplier(
        id:           id,
        companyName:  companyName,
        contactName:  contactName,
        contactTitle: contactTitle,
        address:      address,
        city:         city,
        regionName:   regionName,
        postalCode:   postalCode,
        country:      country,
        phone:        phone,
        fax:          fax,
        homePage:     homePage
      );
    }
  }

  private static void ReadRegions(Northwind northwind, string path) {
    CsvReader reader = new CsvReader(File.ReadAllBytes(path + "/Region.csv"));

    reader.SkipLine();
    while (!reader.Eof()) {
      long id = reader.ReadLong();
      reader.Skip(';');
      string description = reader.ReadString().Trim();
      reader.SkipLine();

      northwind.AddRegion(id: id, description: description);
    }
  }

  private static void ReadTerritories(Northwind northwind, string path) {
    CsvReader reader = new CsvReader(File.ReadAllBytes(path + "/Territories.csv"));

    reader.SkipLine();
    while (!reader.Eof()) {
      string idStr = reader.ReadString();
      reader.Skip(';');
      string description = reader.ReadString().Trim();
      reader.Skip(';');
      long region = reader.ReadLong();
      reader.SkipLine();

      int id = Convert.ToInt32(idStr);

      northwind.AddTerritory(id: id, description: description, region: region);
    }
  }

  private static void ReadShippers(Northwind northwind, string path) {
    CsvReader reader = new CsvReader(File.ReadAllBytes(path + "/Shippers.csv"));

    reader.SkipLine();
    while (!reader.Eof()) {
      long id = reader.ReadLong();
      reader.Skip(';');
      string companyName = reader.ReadString();
      reader.Skip(';');
      string phone = reader.ReadString();
      reader.SkipLine();

      northwind.AddShipper(id: id, companyName: companyName, phone: phone);
    }
  }

  private static void ReadCustomers(Northwind northwind, string path) {
    CsvReader reader = new CsvReader(File.ReadAllBytes(path + "/Customers.csv"));

    reader.SkipLine();
    while (!reader.Eof()) {
      string id = reader.ReadString();
      reader.Skip(';');
      string companyName = reader.ReadString();
      reader.Skip(';');
      string contactName = reader.ReadString();
      reader.Skip(';');
      string contactTitle = reader.ReadString();
      reader.Skip(';');
      string address = reader.ReadString();
      reader.Skip(';');
      string city = reader.ReadString();
      reader.Skip(';');
      string regionName = reader.ReadString();
      reader.Skip(';');
      string postalCode = reader.ReadString();
      reader.Skip(';');
      string country = reader.ReadString();
      reader.Skip(';');
      string phone = reader.ReadString();
      reader.Skip(';');
      string fax = reader.ReadString();
      reader.NextLine();

      northwind.AddCustomer(
        id:           id,
        companyName:  companyName,
        contactName:  contactName,
        contactTitle: contactTitle,
        address:      address,
        city:         city,
        regionName:   regionName,
        postalCode:   postalCode,
        country:      country,
        phone:        phone,
        fax:          fax
      );
    }
  }


  private static void ReadEmployees(Northwind northwind, string path) {
    CsvReader reader = new CsvReader(File.ReadAllBytes(path + "/Employees.csv"));

    List<(long, long)> reportHierarchy = new List<(long, long)>();

    reader.SkipLine();
    while (!reader.Eof()) {
      long id = reader.ReadLong();
      reader.Skip(';');
      string lastName = reader.ReadString();
      reader.Skip(';');
      string firstName = reader.ReadString();
      reader.Skip(';');
      string title = reader.ReadString();
      reader.Skip(';');
      string titleOfCourtesy = reader.ReadString();
      reader.Skip(';');
      DateTime birthDate = Convert.ToDateTime(reader.ReadString());
      reader.Skip(';');
      DateTime hireDate = Convert.ToDateTime(reader.ReadString());
      reader.Skip(';');
      string address = reader.ReadString();
      reader.Skip(';');
      string city = reader.ReadString();
      reader.Skip(';');
      string regionName = reader.ReadString();
      reader.Skip(';');
      string postalCode = reader.ReadString();
      reader.Skip(';');
      string country = reader.ReadString();
      reader.Skip(';');
      string homePhone = reader.ReadString();
      reader.Skip(';');
      string extension = reader.ReadString();
      reader.Skip(';');
      string _ = reader.ReadString();
      reader.Skip(';');
      string notes = reader.ReadString();
      reader.Skip(';');
      long reportsTo = reader.ReadLong();
      reader.Skip(';');
      string photoPath = reader.ReadString();
      reader.Skip(';');
      double salary = reader.ReadDouble();
      reader.NextLine();

      if (reportsTo != 0)
        reportHierarchy.Add((id, reportsTo));

      northwind.AddEmployee(
        id:               id,
        lastName:         lastName,
        firstName:        firstName,
        title:            title,
        titleOfCourtesy:  titleOfCourtesy,
        birthDate:        birthDate,
        hireDate:         hireDate,
        address:          address,
        city:             city,
        regionName:       regionName,
        postalCode:       postalCode,
        country:          country,
        homePhone:        homePhone,
        extension:        extension,
        notes:            notes,
        // reportsTo:        reportsTo,
        photoPath:        photoPath,
        salary:           salary
      );
    }

    northwind.SetReportsTo(reportHierarchy.ToArray());
  }

  private static void ReadEmployeeTerritories(Northwind northwind, string path) {
    CsvReader reader = new CsvReader(File.ReadAllBytes(path + "/EmployeeTerritories.csv"));

    reader.SkipLine();
    while (!reader.Eof()) {
      long employeeId = reader.ReadLong();
      reader.Skip(';');
      int territoryId = Convert.ToInt32(reader.ReadString());
      reader.SkipLine();

      northwind.AssignTerritory(employeeId: employeeId, territoryId: territoryId);
    }
  }

  private static void ReadProducts(Northwind northwind, string path) {
    CsvReader reader = new CsvReader(File.ReadAllBytes(path + "/Products.csv"));

    reader.SkipLine();
    while (!reader.Eof()) {
      long id = reader.ReadLong();
      reader.Skip(';');
      string productName = reader.ReadString();
      reader.Skip(';');
      long supplierId = reader.ReadLong();
      reader.Skip(';');
      long categoryId = reader.ReadLong();
      reader.Skip(';');
      string quantityPerUnit = reader.ReadString();
      reader.Skip(';');
      double unitPrice = reader.ReadDouble();
      reader.Skip(';');
      long unitsInStock = reader.ReadLong();
      reader.Skip(';');
      long unitsOnOrder = reader.ReadLong();
      reader.Skip(';');
      long reorderLevel = reader.ReadLong();
      reader.Skip(';');
      long discontinued = reader.ReadLong();
      reader.NextLine();

      Debug.Assert(discontinued == 0 | discontinued == 1);

      northwind.AddProduct(
        id:                id,
        productName:      productName,
        supplierId:       supplierId,
        categoryId:       categoryId,
        quantityPerUnit: quantityPerUnit,
        unitPrice:        unitPrice,
        unitsInStock:    unitsInStock,
        unitsOnOrder:    unitsOnOrder,
        reorderLevel:     reorderLevel,
        discontinued:      discontinued != 0
      );
    }
  }

  private static void ReadOrders(Northwind northwind, string path) {
    CsvReader reader = new CsvReader(File.ReadAllBytes(path + "/Orders.csv"));

    reader.SkipLine();
    while (!reader.Eof()) {
      long id = reader.ReadLong();
      reader.Skip(';');
      string customerId = reader.ReadString();
      reader.Skip(';');
      long employeeId = reader.ReadLong();
      reader.Skip(';');
      DateTime orderDate = Convert.ToDateTime(reader.ReadString());
      reader.Skip(';');
      DateTime requiredDate = Convert.ToDateTime(reader.ReadString());
      reader.Skip(';');
      string shippedDateStr = reader.ReadString();
      reader.Skip(';');
      long shipVia = reader.ReadLong();
      reader.Skip(';');
      double freight = reader.ReadDouble();
      reader.Skip(';');
      string shipName = reader.ReadString();
      reader.Skip(';');
      string shipAddress = reader.ReadString();
      reader.Skip(';');
      string shipCity = reader.ReadString();
      reader.Skip(';');
      string shipRegion = reader.ReadString();
      reader.Skip(';');
      string shipPostalCode = reader.ReadString();
      reader.Skip(';');
      string shipCountry = reader.ReadString();
      reader.NextLine();

      DateTime? shippedDate = shippedDateStr != "" ? Convert.ToDateTime(shippedDateStr) : (DateTime?) null;

      northwind.AddOrder(
        id:             id,
        customerId:     customerId,
        employeeId:     employeeId,
        orderDate:      orderDate,
        requiredDate:   requiredDate,
        shippedDate:    shippedDate,
        shipVia:        shipVia,
        freight:        freight,
        shipName:       shipName,
        shipAddress:    shipAddress,
        shipCity:       shipCity,
        shipRegion:     shipRegion,
        shipPostalCode: shipPostalCode,
        shipCountry:    shipCountry
      );
    }
  }

  private static void ReadOrderDetails(Northwind northwind, string path) {
    CsvReader reader = new CsvReader(File.ReadAllBytes(path + "/OrderDetails.csv"));

    reader.SkipLine();
    while (!reader.Eof()) {
      long orderId = reader.ReadLong();
      reader.Skip(';');
      long productId = reader.ReadLong();
      reader.Skip(';');
      double itemUnitPrice = reader.ReadDouble();
      reader.Skip(';');
      long quantity = reader.ReadLong();
      reader.Skip(';');
      double discount = reader.ReadDouble();
      reader.NextLine();

      northwind.AddOrderDetail(
        orderId:        orderId,
        productId:      productId,
        itemUnitPrice:  itemUnitPrice,
        quantity:       quantity,
        discount:       discount
      );
    }
  }

  //////////////////////////////////////////////////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////

  private static void RunQueries(Northwind northwind) {
    // (long, string)[] Regions();
    foreach (var (regionId, name) in northwind.Regions())
      Console.WriteLine("{0}  {1,-8}", regionId, name);
    Console.WriteLine();

    // (string, string)[] SortedEmployeesNames();
    foreach (var (firstName, lastName) in northwind.SortedEmployeesNames())
      Console.WriteLine("{0,-8}  {1,-9}", firstName, lastName);
    Console.WriteLine();

    // (long, double)[] OrdersSubtotals();
    foreach (var (orderId, subtotal) in northwind.OrdersSubtotals())
      Console.WriteLine("{0}  {1,8:####0.00}", orderId, subtotal);
    Console.WriteLine();

    // (DateTime, long, double)[] ShippedOrdersSubtotal();
    foreach (var (date, orderId, subtotal) in northwind.ShippedOrdersSubtotal())
      Console.WriteLine("{0:yyyy-MM-dd}  {1}  {2,8:####0.00}", date, orderId, subtotal);
    Console.WriteLine();

    // (long, string, string, double)[] TotalSalesPerProductWithCategory();
    foreach (var (categoryId, categoryName, productName, subtotal) in northwind.TotalSalesPerProductWithCategory())
      Console.WriteLine("{0}  {1,-14}  {2,-32}  {3,9:#####0.00}", categoryId, categoryName, productName, subtotal);
    Console.WriteLine();

    // (long, string, string, double)[] TotalSalesPerProductWithCategory(DateTime, DateTime);
    foreach (var (categoryId, categoryName, productName, subtotal) in northwind.TotalSalesPerProductWithCategory(new DateTime(1997, 1, 1), new DateTime(1997, 12, 31)))
      Console.WriteLine("{0}  {1,-14}  {2,-32}  {3,8:####0.00}", categoryId, categoryName, productName, subtotal);
    Console.WriteLine();

    // (long, string, (string, double)[])[] TotalSalesPerProductByCategory();
    foreach (var (categoryId, categoryName, productsSales) in northwind.TotalSalesPerProductByCategory()) {
      Console.WriteLine("{0}  {1,-14}", categoryId, categoryName);
      foreach (var (productName, subtotal) in productsSales)
        Console.WriteLine("  {0,-32}  {1,9:#####0.00}", productName, subtotal);
    }
    Console.WriteLine();

    // (string, double)[] AboveAveragePriceProducts();
    foreach (var (productName, price) in northwind.AboveAveragePriceProducts())
      Console.WriteLine("{0,-31}  {1,6:##0.00}", productName, price);
    Console.WriteLine();

    // (string, string, long, double)[] SalesByProductAndQuarter(long);
    foreach (var (categoryName, productName, quarter, subtotal) in northwind.SalesByProductAndQuarter(1997))
      Console.WriteLine("{0,-14}  {1,-32}  {2}  {3,8:####0.00}", categoryName, productName, quarter, subtotal);
    Console.WriteLine();

    // (string, (string, double[])[])[] SalesByCategoryProductAndQuarter(long);
    foreach (var (categoryName, productsSales) in northwind.SalesByCategoryProductAndQuarter(1997)) {
      Console.WriteLine("{0,-14}", categoryName);
      foreach (var (productName, subtotals) in productsSales) {
        Console.Write("  {0,-32}", productName);
        for (int i=0 ; i < subtotals.Length ; i++)
          Console.Write("  {0,8:####0.00}", subtotals[i]);
        Console.WriteLine();
      }
    }
    Console.WriteLine();

    // (string, double)[] SalesByCategory(DateTime, DateTime);
    foreach (var (categoryName, subtotal) in northwind.SalesByCategory(new DateTime(1997, 1, 1), new DateTime(1997, 12, 31)))
      Console.WriteLine("{0,-14}  {1,9:#####0.00}", categoryName, subtotal);
    Console.WriteLine();


    // (string, string, long, double, double, double, double)[] QuarterlyOrdersByProductCustomerYear(DateTime, DateTime);
    foreach (var (productName, companyName, year, s1, s2, s3, s4) in northwind.QuarterlyOrdersByProductCustomerYear(new DateTime(1997, 1, 1), new DateTime(1997, 12, 31)))
      Console.WriteLine("{0,-32}  {1,-34}  {2}  {3,8:####0.00}  {4,7:###0.00}  {5,7:###0.00}  {6,7:###0.00}", productName, companyName, year, s1, s2, s3, s4);
    Console.WriteLine();

    // (string, (string, (long, double[])[])[])[] QuarterlyOrdersByProductCustomerYear();
    foreach (var (productName, salesByCustomer) in northwind.QuarterlyOrdersByProductCustomerYear()) {
      Console.WriteLine("{0,-32}", productName);
      foreach (var (companyName, salesByYear) in salesByCustomer) {
        bool firstYear = true;
        foreach (var (year, subtotals) in salesByYear) {
          if (firstYear)
            Console.Write("  {0,-34}", companyName);
          else
            Console.Write("                                    ");
          Console.Write("  {0}", year);
          Console.Write("  {0,8:####0.00}", subtotals[0]);
          for (int i=1 ; i < subtotals.Length ; i++)
            if (i == subtotals.Length - 1)
              Console.Write("  {0,8:####0.00}", subtotals[i]);
            else
              Console.Write("  {0,7:###0.00}", subtotals[i]);
          firstYear = false;
          Console.WriteLine();
        }
      }
    }
    Console.WriteLine();

    // (string, (string, double, double, double)[])[] TopGrossingProductsByCategory(long);
    foreach (var (categoryName, productsRevenues) in northwind.TopGrossingProductsByCategory(90)) {
      Console.WriteLine("{0,-14}", categoryName);
      foreach (var (productName, revenues, percentage, cumulativePercentage) in productsRevenues)
        Console.WriteLine("  {0,-32}  {1,9:#####0.00}  {2:0.00}  {3:0.00}", productName, revenues, percentage, cumulativePercentage);
    }
    Console.WriteLine();

    // (string, (string, (DateTime, long, double)[])[])[] LastOrdersForDiscontinuedProducts();
    foreach (var (productName, lastCustomers) in northwind.LastOrdersForDiscontinuedProducts()) {
      Console.WriteLine("{0,-29}", productName);
      foreach (var (customerName, lastOrders) in lastCustomers) {
        bool first = true;
        foreach (var (date, quantity, discount) in lastOrders) {
          if (first)
            Console.Write("  {0,-28}", customerName);
          else
            Console.Write("                              ");
          Console.WriteLine("  {0:yyyy-MM-dd}  {1,3}  {2:0.00}", date, quantity, discount);
          first = false;
        }
      }
    }
    Console.WriteLine();

    // (string, string, double, double)[] EmployeesSales();
    foreach (var (firstName, lastName, sales, groupSales) in northwind.EmployeesSales())
      Console.WriteLine("{0,-8}  {1,-9}  {2,9:#####0.00}  {3,10:######0.00}", firstName, lastName, sales, groupSales);
    Console.WriteLine();

    // SalesTree[] SalesTrees();
    foreach (var salesTree in northwind.SalesTrees())
      PrintSalesTree(salesTree, 0);
    Console.WriteLine();

    // (string, string, string)[] PhoneNumbers();
    foreach (var (companyName, phone, contactName) in northwind.PhoneNumbers())
      Console.WriteLine("{0,-38}  {1,-17}  {2,-27}", companyName, phone, contactName);
    Console.WriteLine();

    // string[] PhoneOwners(string);
    // (string, string) BasicInfo(string);
    foreach (var (_, phone, _) in northwind.PhoneNumbers()) {
      var id = northwind.PhoneOwners(phone)[0];
      var (desc, code) = northwind.BasicInfo(id);
      Console.WriteLine("{0,-38}  {1}  {2,-17}", desc, code, phone);
    }
    Console.WriteLine();

    foreach (var (_, phone, _) in northwind.PhoneNumbers())
      foreach (var (desc, _, type) in northwind.PhoneInfo(phone))
        Console.WriteLine("{0,-38}  {1,-17}  {2,-5}", desc, phone, type);
    Console.WriteLine();
  }

  private static void PrintSalesTree(SalesTree salesTree, int indent) {
    string firstName = salesTree.Item1;
    string lastName = salesTree.Item2;
    double sales = salesTree.Item3;
    double? groupSales = salesTree.Item4;
    SalesTree[] subtrees = salesTree.Item5;

    for (int i=0 ; i < indent ; i++)
      Console.Write("  ");
    if (indent == 0)
      Console.Write("{0,-6}  {1,-6}  {2,9:#####0.00}", firstName, lastName, sales);
    else if (indent == 1)
      Console.Write("{0,-8}  {1,-9}  {2,9:#####0.00}", firstName, lastName, sales);
    else
      Console.Write("{0,-7}  {1,-9}  {2,9:#####0.00}", firstName, lastName, sales);

    if (groupSales != null) {
      if (indent == 0)
        Console.WriteLine("  {0,10:######0.00}", groupSales);
      else
        Console.WriteLine("  {0,9:#####0.00}", groupSales);
      foreach (SalesTree subtree in subtrees)
        PrintSalesTree(subtree, indent + 1);
    }
    else
      Console.WriteLine();
  }
}
