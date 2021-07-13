using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CS07_Linq2Xml
{
    class Program
    {
        static void Main(string[] args)
        {
            //Load Documents

            XElement customersElem = XElement.Load(@"C:/CS07/customers.xml");
            XDocument customerDoc = XDocument.Load(@"C:/CS07/customers.xml");

            //1. Basic Query: Load All Emails In Four Different Ways

            //1A. Retrieve Customer Emails from the XElement
            var customers = customersElem.Elements("Customer");
            var customerIds = customers.Attributes("Id");

            Console.WriteLine("--Retrieve Customer Emails from the XElement--");
            foreach (var cust in customers)
            {
                Console.WriteLine(cust.Element("Email").Value);
            }

            Console.WriteLine();

            //1B. Retrieve Customer Emails from the XElement With Query Syntax
            var custNamesQ = 
                (from cust in customersElem.Elements("Customer")
                select cust);

            Console.WriteLine("--Retrieve Customer Emails from the XElement With Query Syntax--");
            foreach (var cust in custNamesQ)
            {
                Console.WriteLine(cust.Element("Email").Value);
            }

            Console.WriteLine();

            //1C. Retrieve Customer Emails from the XDocument
            var custNamesD = customerDoc.Descendants("Customer");

            Console.WriteLine("--Retrieve Customer Emails from the XDocument--");
            foreach (var cust in custNamesD)
            {
                Console.WriteLine(cust.Element("Email").Value);
            }

            Console.WriteLine();

            //1D. Using Chained Descendants Operators
            var custNamesD2 = customerDoc.Descendants("Customer").Descendants("Email");

            Console.WriteLine("--Using Chained Descendants Operators--");
            foreach (var email in custNamesD2)
            {
                Console.WriteLine(email.Value);
            }

            Console.WriteLine();

            //2A Accessing Attributes and Using the Where Clause

            //From now on Customers collection from line 19 will be used.
            var Cust5 = customers.Where(c => c.Attribute("Id").Value == "5");

            var Cust5Q = (from cust in customers
                where cust.Attribute("Id").Value == "5"
                select cust);
                

            //2B Casting the Attribute to an Int to Use a Numeric Condition
            var CustUpTo5=customers.Where(c => int.Parse(c.Attribute("Id").Value) <= 5);

            var CustUpTo5Q = (from cust in customers
                where int.Parse(cust.Attribute("Id").Value) <= 5
                select cust);

            //2C Ordering Results with the OrderBy clause

            var CustUpTo5Ord = customers
                .Where(c => int.Parse(c.Attribute("Id").Value) <= 5)
                .OrderBy(c => c.Element("Age").Value).ToList();

            var CustUpTo5OrdQ = (from cust in customers
                where int.Parse(cust.Attribute("Id").Value) <= 5
                orderby cust.Element("Age").Value
                select cust).ToList();

            //3 Joining XML Files
            XElement salesElem = XElement.Load(@"C:/CS07/Sales.xml");

            XElement CustSale = 
                new XElement("CustomerAndSale",
                from c in customersElem.Elements("Customer")

                join s in salesElem.Elements("Sale")
                    on (string)c.Attribute("Id") equals
                    (string)s.Element("CustomerId")

                select new XElement("Order",
                    new XElement("CustomerID", (string)c.Attribute("Id")),
                    new XElement("CustomerName", (string)c.Element("LastName")),
                    new XElement("SaleId", (string)s.Attribute("Id")),
                    new XElement("Amount", (string)s.Element("Amount"))
                    )
                );

            Console.WriteLine(CustSale.ToString());

            //4A Aggregating Data
            var avg = customers.Average(c => Int32.Parse(c.Element("Age").Value));
            var sum = customers.Sum(c => Int32.Parse(c.Element("Age").Value));
            var min = customers.Min(c => Int32.Parse(c.Element("Age").Value));
            var max = customers.Max(c => Int32.Parse(c.Element("Age").Value));
            var count = customers.Count();
            var countAttribute = customerIds.Count();

            //4B Aggregating Data To a New Type
            XElement aggregateGroup = 
                new XElement("CountriesAndAverageAges",
                from c in customers
                group c by c.Element("Country").Value
                into cByCountry
                select new XElement
                (
                    "AverageAgeByCountry",
                    new XElement("Country", cByCountry.Elements("Country")).FirstNode,
                    new XElement("AverageAge", cByCountry.Average(cByCountry => Int32.Parse(cByCountry.Element("Age").Value)))
                ));

            Console.WriteLine(aggregateGroup.ToString());

            //5 Using XPath
            XDocument customerDocCopy = XDocument.Load(@"C:/CS07/customers.xml");
            var customersCopy = customerDocCopy.Descendants("Customer"); //Linq to XML
            var customersX = customerDocCopy.XPathSelectElements("Customers/Customer"); //XPath Equivalent

            var customerIdsCopy = customerDocCopy.Descendants("Customer").Attributes("Id");
            var customerIdsX = customerDocCopy.XPathSelectElements("Customers/Customer/@Id");

            var UKCustomersX = customerDocCopy.XPathSelectElements("Customers/Customer[Country='UK']/LastName");

            Console.WriteLine("Last Names of Customers in the UK retrieved using XPath:");
            foreach (var c in UKCustomersX)
            {
                Console.WriteLine(c.Value);
            }

            //Equivalent alternative Where the Whole Customer is retrieved instead:
            var UKCustomersXWhole = customerDocCopy.XPathSelectElements("Customers/Customer[Country='UK']");
            Console.WriteLine("Last Names of Customers in the UK retrieved using XPath (2):");
            foreach (var c in UKCustomersXWhole)
            {
                Console.WriteLine(c.Element("LastName").Value);
            }

            //Selecting a single element using XPath

            XElement customer2 = customerDocCopy.XPathSelectElement("Customers/Customer[@Id = '2']");
            Console.WriteLine("Last Name of Customer with Id 2: " + customer2.Element("LastName").Value);

            //Selecting a single element using XPath (XElement root)
            XElement customer2E = customersElem.XPathSelectElement("Customer[@Id = '2']");
            Console.WriteLine("Last Name of Customer with Id 2: " + customer2E.Element("LastName").Value);

            //Adding an element using XElement
            XElement newCustomer = new XElement("Customer",
                new XAttribute("Id", "11"),
                new XElement("FirstName", "Leo"),
                new XElement("LastName", "Tolstoy"),
                new XElement("Gender", "M"),
                new XElement("Age", "81"),
                new XElement("Phone", "478-909-7865"),
                new XElement("Email", "leot@example.com"),
                new XElement("Country", "Russia")
                );

            customersElem.Add(newCustomer);
            customersElem.Save(@"C:/CS07/Customers.xml");

            //Deleting an element with XElement and Linq
            XElement node2delete = customersElem.Elements("Customer")
                .SingleOrDefault(c => c.Attribute("Id").Value == "11");

            if(node2delete!=null) node2delete.Remove();
            customersElem.Save(@"C:/CS07/Customers.xml");

            //Upading an element with XElement and Linq
            XElement node2update = customersElem.Elements("Customer")
                .SingleOrDefault(c => c.Attribute("Id").Value == "1");

            if (node2delete != null) node2update.Element("FirstName").Value="Annika";
            customersElem.Save(@"C:/CS07/Customers.xml");

        }
    }
}
