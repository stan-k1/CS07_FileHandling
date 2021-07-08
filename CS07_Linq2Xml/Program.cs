using System;
using System.Linq;
using System.Xml.Linq;

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
        }
    }
}
