import java.util.List;
import java.util.ArrayList;
import java.util.Set;
import java.util.Map;
import java.util.HashMap;
import java.time.LocalDate;
import java.io.IOException;
import java.io.Reader;
import java.io.Writer;
import java.io.FileReader;
import java.io.FileWriter;
import java.nio.file.Paths;
import java.nio.file.Files;

import net.cell_lang.*;


public class Tester {
  public static void main(String[] args) throws IOException {
    if (args.length != 1 && args.length != 2) {
      System.out.println("Usage: northwind <input dataset> [<saved dataset copy>]");
      return;
    }

    // Creating the automaton
    Northwind northwind = new Northwind();

    String path = args[0];

    if (Files.isRegularFile(Paths.get(path))) {
      // Loading the initial state
      try (Reader reader = new FileReader(path)) {
        northwind.load(reader);
      }
    }
    else if (Files.isDirectory(Paths.get(path))) {
      // Importing the data from the CSV files
      importCsvData(northwind, path);
    }
    else {
      System.out.printf("No such file or directory: %s\n", path);
      return;
    }

    RunQueries(northwind);

    if (args.length == 2) {
      // Saving the final state to the indicated file
      try (Writer writer = new FileWriter(args[1])) {
        northwind.save(writer);
      }
    }
  }

  //////////////////////////////////////////////////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////

  private static LocalDate parseDate(String str) {
    // 1996-07-16 00:00:00
    if (str.length() == 0)
      return null;
    return LocalDate.parse(str.substring(0, 10));
  }

  //////////////////////////////////////////////////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////

  private static void importCsvData(Northwind northwind, String path) throws IOException {
    readCategories(northwind, path);
    readSuppliers(northwind, path);
    readRegions(northwind, path);
    readTerritories(northwind, path);
    readShippers(northwind, path);
    readCustomers(northwind, path);
    readEmployees(northwind, path);
    readEmployeeTerritories(northwind, path);
    readProducts(northwind, path);
    readOrders(northwind, path);
    readOrderDetails(northwind, path);
  }


  private static void readCategories(Northwind northwind, String path) throws IOException {
    CsvReader reader = new CsvReader(Files.readAllBytes(Paths.get(path + "/Categories.csv")));

    reader.skipLine();
    while (!reader.eof()) {
      long id = reader.readLong();
      reader.skip(';');
      String name = reader.readString();
      reader.skip(';');
      String description = reader.readString();
      reader.skipLine();

      northwind.addCategory( id, name, description);
    }
  }


  private static void readSuppliers(Northwind northwind, String path) throws IOException {
    CsvReader reader = new CsvReader(Files.readAllBytes(Paths.get(path + "/Suppliers.csv")));

    reader.skipLine();
    while (!reader.eof()) {
      long id = reader.readLong();
      reader.skip(';');
      String companyName = reader.readString();
      reader.skip(';');
      String contactName = reader.readString();
      reader.skip(';');
      String contactTitle = reader.readString();
      reader.skip(';');
      String address = reader.readString();
      reader.skip(';');
      String city = reader.readString();
      reader.skip(';');
      String regionName = reader.readString();
      reader.skip(';');
      String postalCode = reader.readString();
      reader.skip(';');
      String country = reader.readString();
      reader.skip(';');
      String phone = reader.readString();
      reader.skip(';');
      String fax = reader.readString();
      reader.skip(';');
      String homePage = reader.readString();
      reader.skipLine();

      northwind.addSupplier(
        id,            // id
        companyName,   // companyName
        contactName,   // contactName
        contactTitle,  // contactTitle
        address,       // address
        city,          // city
        regionName,    // regionName
        postalCode,    // postalCode
        country,       // country
        phone,         // phone
        fax,           // fax
        homePage       // homePage
      );
    }
  }

  private static void readRegions(Northwind northwind, String path) throws IOException {
    CsvReader reader = new CsvReader(Files.readAllBytes(Paths.get(path + "/Region.csv")));

    reader.skipLine();
    while (!reader.eof()) {
      long id = reader.readLong();
      reader.skip(';');
      String description = reader.readString().trim();
      reader.skipLine();

      northwind.addRegion( id, description);
    }
  }

  private static void readTerritories(Northwind northwind, String path) throws IOException {
    CsvReader reader = new CsvReader(Files.readAllBytes(Paths.get(path + "/Territories.csv")));

    reader.skipLine();
    while (!reader.eof()) {
      String idStr = reader.readString();
      reader.skip(';');
      String description = reader.readString().trim();
      reader.skip(';');
      long region = reader.readLong();
      reader.skipLine();

      int id = Integer.parseInt(idStr);

      northwind.addTerritory(id, description, region);
    }
  }

  private static void readShippers(Northwind northwind, String path) throws IOException {
    CsvReader reader = new CsvReader(Files.readAllBytes(Paths.get(path + "/Shippers.csv")));

    reader.skipLine();
    while (!reader.eof()) {
      long id = reader.readLong();
      reader.skip(';');
      String companyName = reader.readString();
      reader.skip(';');
      String phone = reader.readString();
      reader.skipLine();

      northwind.addShipper(id, companyName, phone);
    }
  }

  private static void readCustomers(Northwind northwind, String path) throws IOException {
    CsvReader reader = new CsvReader(Files.readAllBytes(Paths.get(path + "/Customers.csv")));

    reader.skipLine();
    while (!reader.eof()) {
      String id = reader.readString();
      reader.skip(';');
      String companyName = reader.readString();
      reader.skip(';');
      String contactName = reader.readString();
      reader.skip(';');
      String contactTitle = reader.readString();
      reader.skip(';');
      String address = reader.readString();
      reader.skip(';');
      String city = reader.readString();
      reader.skip(';');
      String regionName = reader.readString();
      reader.skip(';');
      String postalCode = reader.readString();
      reader.skip(';');
      String country = reader.readString();
      reader.skip(';');
      String phone = reader.readString();
      reader.skip(';');
      String fax = reader.readString();
      reader.skipLine();

      northwind.addCustomer(
        id,            // id
        companyName,   // companyName
        contactName,   // contactName
        contactTitle,  // contactTitle
        address,       // address
        city,          // city
        regionName,    // regionName
        postalCode,    // postalCode
        country,       // country
        phone,         // phone
        fax            // fax
      );
    }
  }


  private static void readEmployees(Northwind northwind, String path) throws IOException {
    CsvReader reader = new CsvReader(Files.readAllBytes(Paths.get(path + "/Employees.csv")));

    HashMap<Long, Long> reportHierarchy = new HashMap<Long, Long>();

    reader.skipLine();
    while (!reader.eof()) {
      long id = reader.readLong();
      reader.skip(';');
      String lastName = reader.readString();
      reader.skip(';');
      String firstName = reader.readString();
      reader.skip(';');
      String title = reader.readString();
      reader.skip(';');
      String titleOfCourtesy = reader.readString();
      reader.skip(';');
      LocalDate birthDate = parseDate(reader.readString());
      reader.skip(';');
      LocalDate hireDate = parseDate(reader.readString());
      reader.skip(';');
      String address = reader.readString();
      reader.skip(';');
      String city = reader.readString();
      reader.skip(';');
      String regionName = reader.readString();
      reader.skip(';');
      String postalCode = reader.readString();
      reader.skip(';');
      String country = reader.readString();
      reader.skip(';');
      String homePhone = reader.readString();
      reader.skip(';');
      String extension = reader.readString();
      reader.skip(';');
      String __ = reader.readString();
      reader.skip(';');
      String notes = reader.readString();
      reader.skip(';');
      long reportsTo = reader.readLong();
      reader.skip(';');
      String photoPath = reader.readString();
      reader.skip(';');
      double salary = reader.readDouble();
      reader.skipLine();

      if (reportsTo != 0)
        reportHierarchy.put(id, reportsTo);

      northwind.addEmployee(
        id,                // id
        lastName,          // lastName
        firstName,         // firstName
        title,             // title
        titleOfCourtesy,   // titleOfCourtesy
        birthDate,         // birthDate
        hireDate,          // hireDate
        address,           // address
        city,              // city
        regionName,        // regionName
        postalCode,        // postalCode
        country,           // country
        homePhone,         // homePhone
        extension,         // extension
        notes,             // notes
        photoPath,         // photoPath
        salary,            // salary
        null               // reportsTo
      );
    }

    northwind.setReportsTo(reportHierarchy);
  }

  private static void readEmployeeTerritories(Northwind northwind, String path) throws IOException {
    CsvReader reader = new CsvReader(Files.readAllBytes(Paths.get(path + "/EmployeeTerritories.csv")));

    reader.skipLine();
    while (!reader.eof()) {
      long employeeId = reader.readLong();
      reader.skip(';');
      int territoryId = Integer.parseInt(reader.readString());
      reader.skipLine();

      northwind.assignTerritory(employeeId, territoryId);
    }
  }

  private static void readProducts(Northwind northwind, String path) throws IOException {
    CsvReader reader = new CsvReader(Files.readAllBytes(Paths.get(path + "/Products.csv")));

    reader.skipLine();
    while (!reader.eof()) {
      long id = reader.readLong();
      reader.skip(';');
      String productName = reader.readString();
      reader.skip(';');
      long supplierId = reader.readLong();
      reader.skip(';');
      long categoryId = reader.readLong();
      reader.skip(';');
      String quantityPerUnit = reader.readString();
      reader.skip(';');
      double unitPrice = reader.readDouble();
      reader.skip(';');
      long unitsInStock = reader.readLong();
      reader.skip(';');
      long unitsOnOrder = reader.readLong();
      reader.skip(';');
      long reorderLevel = reader.readLong();
      reader.skip(';');
      long discontinued = reader.readLong();
      reader.skipLine();

      assert discontinued == 0 | discontinued == 1;

      northwind.addProduct(
        id,               // id
        productName,      // productName
        supplierId,       // supplierId
        categoryId,       // categoryId
        quantityPerUnit,  // quantityPerUnit
        unitPrice,        // unitPrice
        unitsInStock,     // unitsInStock
        unitsOnOrder,     // unitsOnOrder
        reorderLevel,     // reorderLevel
        discontinued != 0 // discontinued
      );
    }
  }

  private static void readOrders(Northwind northwind, String path) throws IOException {
    CsvReader reader = new CsvReader(Files.readAllBytes(Paths.get(path + "/Orders.csv")));

    reader.skipLine();
    while (!reader.eof()) {
      long id = reader.readLong();
      reader.skip(';');
      String customerId = reader.readString();
      reader.skip(';');
      long employeeId = reader.readLong();
      reader.skip(';');
      LocalDate orderDate = parseDate(reader.readString());
      reader.skip(';');
      LocalDate requiredDate = parseDate(reader.readString());
      reader.skip(';');
      LocalDate shippedDate = parseDate(reader.readString());
      reader.skip(';');
      long shipVia = reader.readLong();
      reader.skip(';');
      double freight = reader.readDouble();
      reader.skip(';');
      String shipName = reader.readString();
      reader.skip(';');
      String shipAddress = reader.readString();
      reader.skip(';');
      String shipCity = reader.readString();
      reader.skip(';');
      String shipRegion = reader.readString();
      reader.skip(';');
      String shipPostalCode = reader.readString();
      reader.skip(';');
      String shipCountry = reader.readString();
      reader.skipLine();

      northwind.addOrder(
        id,              // id
        customerId,      // customerId
        employeeId,      // employeeId
        orderDate,       // orderDate
        requiredDate,    // requiredDate
        shipVia,         // shipVia
        freight,         // freight
        shipName,        // shipName
        shipAddress,     // shipAddress
        shipCity,        // shipCity
        shipRegion,      // shipRegion
        shipPostalCode,  // shipPostalCode
        shipCountry,     // shipCountry
        shippedDate      // shippedDate
      );
    }
  }

  private static void readOrderDetails(Northwind northwind, String path) throws IOException {
    CsvReader reader = new CsvReader(Files.readAllBytes(Paths.get(path + "/OrderDetails.csv")));

    reader.skipLine();
    while (!reader.eof()) {
      long orderId = reader.readLong();
      reader.skip(';');
      long productId = reader.readLong();
      reader.skip(';');
      double itemUnitPrice = reader.readDouble();
      reader.skip(';');
      long quantity = reader.readLong();
      reader.skip(';');
      double discount = reader.readDouble();
      reader.skipLine();

      northwind.addOrderDetail(
        orderId,        // orderId
        productId,      // productId
        itemUnitPrice,  // itemUnitPrice
        quantity,       // quantity
        discount        // discount
      );
    }
  }

  //////////////////////////////////////////////////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////

  private static void RunQueries(Northwind northwind) throws IOException {
    // (long, String)[] Regions();
    for (var row : northwind.regions())
      System.out.printf("%s  %-8s\n", row.item1, row.item2);
    System.out.println();

    // (String, String)[] SortedEmployeesNames();
    for (var row : northwind.sortedEmployeesNames())
      System.out.printf("%-8s  %-9s\n", row.item1, row.item2);
    System.out.println();

    // (long, double)[] OrdersSubtotals();
    for (var row : northwind.ordersSubtotals())
      System.out.printf("%s  %8.2f\n", row.item1, row.item2);
    System.out.println();

    // (LocalDate, long, double)[] ShippedOrdersSubtotal();
    for (var row : northwind.shippedOrdersSubtotal())
      System.out.printf("%tY-%<tm-%<td  %s  %8.2f\n", row.item1, row.item2, row.item3);
    System.out.println();

    // (long, String, String, double)[] TotalSalesPerProductWithCategory();
    for (var row : northwind.totalSalesPerProductWithCategory())
      System.out.printf("%s  %-14s  %-32s  %9.2f\n", row.item1, row.item2, row.item3, row.item4);
    System.out.println();

    // (long, String, String, double)[] TotalSalesPerProductWithCategory(LocalDate, LocalDate);
    for (var row : northwind.totalSalesPerProductWithCategory(LocalDate.of(1997, 1, 1), LocalDate.of(1997, 12, 31)))
      System.out.printf("%s  %-14s  %-32s  %8.2f\n", row.item1, row.item2, row.item3, row.item4);
    System.out.println();

    // (long, String, (String, double)[])[] TotalSalesPerProductByCategory();
    for (var categoryRow : northwind.totalSalesPerProductByCategory()) {
      System.out.printf("%s  %-14s\n", categoryRow.item1, categoryRow.item2);
      for (var salesRow : categoryRow.item3)
        System.out.printf("  %-32s  %9.2f\n", salesRow.item1, salesRow.item2);
    }
    System.out.println();

    // (String, double)[] AboveAveragePriceProducts();
    for (var row : northwind.aboveAveragePriceProducts())
      System.out.printf("%-31s  %6.2f\n", row.item1, row.item2);
    System.out.println();

    // (String, String, long, double)[] SalesByProductAndQuarter(long);
    for (var row : northwind.salesByProductAndQuarter(1997))
      System.out.printf("%-14s  %-32s  %s  %8.2f\n", row.item1, row.item2, row.item3, row.item4);
    System.out.println();

    // (String, (String, double[])[])[] SalesByCategoryProductAndQuarter(long);
    for (var categoryRow : northwind.salesByCategoryProductAndQuarter(1997)) {
      System.out.printf("%-14s\n", categoryRow.item1);
      for (var productRow : categoryRow.item2) {
        System.out.printf("  %-32s", productRow.item1);
        for (int i=0 ; i < productRow.item2.length ; i++)
          System.out.printf("  %8.2f", productRow.item2[i]);
        System.out.println();
      }
    }
    System.out.println();

    // (String, double)[] SalesByCategory(LocalDate, LocalDate);
    for (var row : northwind.salesByCategory(LocalDate.of(1997, 1, 1), LocalDate.of(1997, 12, 31)))
      System.out.printf("%-14s  %9.2f\n", row.item1, row.item2);
    System.out.println();


    // (String, String, long, double, double, double, double)[] QuarterlyOrdersByProductCustomerYear(LocalDate, LocalDate);
    for (var row : northwind.quarterlyOrdersByProductCustomerYear(LocalDate.of(1997, 1, 1), LocalDate.of(1997, 12, 31)))
      System.out.printf("%-32s  %-34s  %s  %8.2f  %7.2f  %7.2f  %7.2f\n", row.item1, row.item2, row.item3, row.item4, row.item5, row.item6, row.item7);
    System.out.println();

    // (String, (String, (long, double[])[])[])[] QuarterlyOrdersByProductCustomerYear();
    for (var productEntry : northwind.quarterlyOrdersByProductCustomerYear()) {
      System.out.printf("%-32s\n", productEntry.item1);
      for (var customerEntry : productEntry.item2) {
        boolean firstYear = true;
        for (var yearRow : customerEntry.item2) {
          if (firstYear)
            System.out.printf("  %-34s", customerEntry.item1);
          else
            System.out.printf("                                    ");
          System.out.printf("  %s", yearRow.item1);
          System.out.printf("  %8.2f", yearRow.item2[0]);
          for (int i=1 ; i < yearRow.item2.length ; i++)
            if (i == yearRow.item2.length - 1)
              System.out.printf("  %8.2f", yearRow.item2[i]);
            else
              System.out.printf("  %7.2f", yearRow.item2[i]);
          firstYear = false;
          System.out.println();
        }
      }
    }
    System.out.println();

    // (String, (String, double, double, double)[])[] TopGrossingProductsByCategory(long);
    for (var categoryEntry : northwind.topGrossingProductsByCategory(90)) {
      System.out.printf("%-14s\n", categoryEntry.item1);
      for (var productEntry : categoryEntry.item2)
        System.out.printf("  %-32s  %9.2f  %.2f  %.2f\n", productEntry.item1, productEntry.item2, productEntry.item3, productEntry.item4);
    }
    System.out.println();

    // (String, (String, (LocalDate, long, double)[])[])[] LastOrdersForDiscontinuedProducts();
    for (var productEntry : northwind.lastOrdersForDiscontinuedProducts()) {
      System.out.printf("%-29s\n", productEntry.item1);
      for (var customerEntry : productEntry.item2) {
        boolean first = true;
        for (var orderRow : customerEntry.item2) {
          if (first)
            System.out.printf("  %-28s", customerEntry.item1);
          else
            System.out.printf("                              ");
          System.out.printf("  %tY-%<tm-%<td  %3d  %.2f\n", orderRow.item1, orderRow.item2, orderRow.item3);
          first = false;
        }
      }
    }
    System.out.println();

    // (String, String, double, double)[] EmployeesSales();
    for (var row : northwind.employeesSales())
      System.out.printf("%-8s  %-9s  %9.2f  %10.2f\n", row.item1, row.item2, row.item3, row.item4);
    System.out.println();

    // SalesTree[] SalesTrees();
    for (var salesTree : northwind.salesTrees())
      PrintSalesTree(salesTree, 0);
    System.out.println();

    // (String, String, String)[] PhoneNumbers();
    for (var row : northwind.phoneNumbers())
      System.out.printf("%-38s  %-17s  %-27s\n", row.item1, row.item2, row.item3);
    System.out.println();

    // String[] PhoneOwners(String);
    // (String, String) BasicInfo(String);
    for (var phoneRow : northwind.phoneNumbers()) {
      var id = northwind.phoneOwners(phoneRow.item2)[0];
      var infoRow = northwind.basicInfo((ShipperId_CustomerId_EmployeeId_SupplierId) id);
      System.out.printf("%-38s  %s  %-17s\n", infoRow.item1, infoRow.item2, phoneRow.item2);
    }
    System.out.println();

    for (var phoneRow : northwind.phoneNumbers())
      for (var infoRow : northwind.phoneInfo(phoneRow.item2))
        System.out.printf("%-38s  %-17s  %-5s\n", infoRow.item1, phoneRow.item2, infoRow.item3);
    System.out.println();
  }

  private static void PrintSalesTree(SalesTree salesTree, int indent) {
    String firstName = salesTree.item1;
    String lastName = salesTree.item2;
    double sales = salesTree.item3;
    Double groupSales = salesTree.item4;
    SalesTree[] subtrees = salesTree.item5;

    for (int i=0 ; i < indent ; i++)
      System.out.printf("  ");
    if (indent == 0)
      System.out.printf("%-6s  %-6s  %9.2f", firstName, lastName, sales);
    else if (indent == 1)
      System.out.printf("%-8s  %-9s  %9.2f", firstName, lastName, sales);
    else
      System.out.printf("%-7s  %-9s  %9.2f", firstName, lastName, sales);

    if (groupSales != null) {
      if (indent == 0)
        System.out.printf("  %10.2f\n", groupSales);
      else
        System.out.printf("  %9.2f\n", groupSales);
      for (SalesTree subtree : subtrees)
        PrintSalesTree(subtree, indent + 1);
    }
    else
      System.out.println();
  }
}
