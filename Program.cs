using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company
{
    public class Address
    {
        private string country;
        private string city;
        private int number;
        private int floor;
        private int apartmentNumber;
        private bool isPrivate;

        public Address(string country, string city, int number, int floor, int apartmentNumber, bool isPrivate)
        {
            this.country = country;
            this.city = city;
            this.number = number;
            this.isPrivate = isPrivate;
            if (!isPrivate)
            {
                this.floor = floor;
                this.apartmentNumber = apartmentNumber;
            }
        }

        public string GetCountry() { return this.country; }
    }
    public class Client
    {
        private string firstName;
        private string lastName;
        private int idNumber;
        private Address address;

        public Client(string firstName, string lastName, int idNumber, Address address)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.idNumber = idNumber;
            this.address = address;
        }

        public Address GetAddress() { return this.address; }
        public int GetIdNumber() { return this.idNumber; }
        public string GetFirstName() { return this.firstName; }
        public string GetLastName() { return this.lastName; }
    }
    public class Parcel
    {
        private Product product;
        private double volume;
        private Address sourceAddress;
        private Client client;
        private readonly int id;
        private static int counter;

        public Parcel(Product product, double volume, Address sourceAddress, Client client)
        {
            this.product = product;
            this.volume = volume;
            this.sourceAddress = sourceAddress;
            this.client = client;
            counter++;
            this.id = counter;

            // Update client if is member
            if (this.client is Member)
            {
                ((Member)this.client).UpdateTotalPaid(this.GetPrice());
            }
        }

        public static int GetCounter() { return counter; }

        public double ChooseBox()
        {
            double[] boxSizes = new double[] { 10, 25, 100 };
            for (int i = 0; i < boxSizes.Length; i++)
            {
                if (this.volume < boxSizes[i])
                    return boxSizes[i];
            }
            return this.volume;
        }

        public int ChooseZone()
        {
            string country = this.client.GetAddress().GetCountry();
            if (country != null && country != "")
            {
                return char.ToLower(country[0]) - 96;
            }
            return -1;
        }
        public Address GetAddress()
        {
            return this.sourceAddress;
        }
        public double GetPrice()
        {
            double pricePerCm = 0.1;
            double boxVolume = this.ChooseBox();
            double originalPrice = this.product.GetPrice();
            double priceBump = pricePerCm * boxVolume;
            double finalPrice = originalPrice + priceBump;
            if (boxVolume > 100)
            {
                finalPrice += 5;
            }


            // Discount calculation is client is member
            double discountPercent = 0;
            if (this.client is Member)
            {
                discountPercent = ((Member)this.client).GetDiscount();
            }
            else if (this.client is VIPMember)
            {
                discountPercent = VIPMember.discountPercent;
            }

            finalPrice -= finalPrice * (discountPercent / 100);
            return finalPrice;
        }

        public override string ToString()
        {
            return string.Format("Product name: {0}, Source Country: {1}, Destination Country: {2}, Final Price: {3}$",
                this.product.GetName(),
                this.sourceAddress.GetCountry(),
                this.client.GetAddress().GetCountry(),
                this.GetPrice());
        }
    }
    public class Product
    {
        private string name;
        private string companyName;
        private double price;

        public Product(string name, string companyName, double price)
        {
            this.name = name;
            this.companyName = companyName;
            this.price = price;
        }

        public string GetName() { return this.name; }
        public double GetPrice() { return this.price; }
    }
    public class Member : Client
    {
        private double totalPaid;

        public Member(string firstName, string lastName, int idNumber, Address address) : base(firstName, lastName, idNumber, address)
        {
            this.totalPaid = 0;
        }

        public Member(Client client) : base(client.GetFirstName(), client.GetLastName(), client.GetIdNumber(), client.GetAddress())
        {
            this.totalPaid = 0;
        }

        public void UpdateTotalPaid(double add)
        {
            this.totalPaid += add;
        }

        public double GetDiscount()
        {
            double[] totalPaid = new double[] { 1000, 5000, 20000 };
            double[] discountPercent = new double[] { 2.5, 5, 8 };

            for (int i = totalPaid.Length - 1; i >= 0; i--)
            {
                if (this.totalPaid > totalPaid[i])
                    return discountPercent[i];
            }
            return 0;
        }
    }
    public class VIPMember : Client
    {
        public const double discountPercent = 10;
        public const double pricePerMonth = 200;

        public VIPMember(string firstName, string lastName, int idNumber, Address address) : base(firstName, lastName, idNumber, address) { }
        public VIPMember(Client client) : base(client.GetFirstName(), client.GetLastName(), client.GetIdNumber(), client.GetAddress()) { }
    }
    public abstract class Delivery
    {
        protected Parcel parcel;
        protected string sentDate;
        protected int status;

        public Delivery(Parcel parcel)
        {
            this.parcel = parcel;
            this.sentDate = "";
            this.status = 1;
        }

        public void UpdateStatus()
        {
            if (this.status < 3)
                this.status += 1;
        }

        public void Send()
        {
            this.UpdateStatus();
            this.sentDate = DateTime.Now.ToString("d");
        }

        public abstract double GetPrice();
        public abstract int GetEstimateDaysToArrive();
    }
    public class RegularDelivery : Delivery
    {
        protected const double inChinaPrice = 2;
        protected const double outChinaPrice = 5;
        protected const int inChinaEstimateDays = 5;
        protected const int outChinaEstimateDays = 10;

        public RegularDelivery(Parcel parcel) : base(parcel) { }

        public override double GetPrice()
        {
            if (this.parcel.GetClient().GetAddress().GetCountry() == "China")
                return inChinaPrice;
            return outChinaPrice;
        }

        public override int GetEstimateDaysToArrive()
        {
            if (this.parcel.GetClient().GetAddress().GetCountry() == "China")
                return inChinaEstimateDays;
            return outChinaEstimateDays;
        }
    }
    public class FastDelivery : RegularDelivery
    {
        public FastDelivery(Parcel parcel) : base(parcel) { }

        public override double GetPrice()
        {
            return this.parcel.GetPrice() * 0.1;
        }
    }
    public class DeliveryCenter
    {
        private Zone[] zones;

        public DeliveryCenter()
        {
            this.zones = new Zone[26];
            for (int i = 0; i < this.zones.Length; i++)
            {
                this.zones[i] = new Zone();
            }
        }

        public void AddParcel(Parcel parcel)
        {
            int zoneIndex = parcel.ChooseZone() - 1;
            if (zoneIndex > -1)
                this.zones[zoneIndex].AddParcel(parcel);
        }

        public override string ToString()
        {
            string finalStr = "";
            for (int i = 0; i < this.zones.Length; i++)
            {
                finalStr += string.Format("Zone #{0} has {1} parcels that worth {2}$\n", i + 1, this.zones[i].GetNumParcels(), this.zones[i].GetTotalPriceOfParcels());
            }
            return finalStr;
        }
    }
    public class Zone
    {
        private Parcel[] parcels;
        private int numParcels;

        public Zone()
        {
            this.parcels = new Parcel[10000];
            this.numParcels = 0;
        }

        public int GetNumParcels() { return this.numParcels; }
        public void AddParcel(Parcel parcel)
        {
            if (this.numParcels < this.parcels.Length)
            {
                this.parcels[this.numParcels] = parcel;
                this.numParcels++;
            }
        }

        public double GetTotalPriceOfParcels()
        {
            double totalPrice = 0;
            for (int i = 0; i < this.numParcels; i++)
            {
                totalPrice += this.parcels[i].GetPrice();
            }
            return totalPrice;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
