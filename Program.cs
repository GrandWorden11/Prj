using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImporterCars
{
    public enum CAR_EQUIPMENT
    {
        E_SEAT_HEATER,
        E_PANORAMIC_ROOF,
        E_ELECTRIC_TAILGATE,
        E_ENTERTAINMENT_SYSTEM,
        E_FORWARD_ELECTRIC_SEATS,
        E_DIGITAL_DRIVER_DISPLAY,
        E_REAR_AC
    }
    public abstract class Car
    {
        public string Color { get; protected set; }
        public readonly string Vendor { get; protected set; }
        public readonly string Model { get; protected set; }
        public string Engine { get; protected set; }
        public readonly DateTime Date { get; protected set; }
        public string HP { get; protected set; }
        public List<CAR_EQUIPMENT> Arsenal { get; protected set; }
        public DateTime WashDate { get; protected set; }
        public bool NeedsWash { get; set; }

        public Car(string color, string vendor, string model, string engion, DateTime date, string horsePower, List<CAR_EQUIPMENT> arsenal)
        {
            Color = color;
            Vendor = vendor;
            Model = model;
            Engine = engion;
            Date = date;
            HP = horsePower;
            Arsenal = arsenal;
            ProlongWashTimeIn30Days();
            if (WashDate < DateTime.Now)
            {
                NeedsWash = true;
            }
            else { NeedsWash = false; }
        }

        public abstract string GetInternalCartypeString();
        public void ProlongWashTimeIn30Days()
        {
            WashDate.AddDays(30);
        }

        public override string ToString()
        {
            return
                " Vendor: " + Vendor +
                " Model: " + Model +
                " CarType: " + GetInternalCartypeString() +
                " Engine: " + Engine +
                " HorsePower: " + HP +
                " WashDate " + WashDate;
        }

        public override bool Equals(object obj)
        {
            return obj is Car car &&
                   Color == car.Color &&
                   Vendor == car.Vendor &&
                   Model == car.Model &&
                   Engine == car.Engine &&
                   Date == car.Date &&
                   HP == car.HP &&
                   EqualityComparer<List<CAR_EQUIPMENT>>.Default.Equals(Arsenal, car.Arsenal);
        }

        public override int GetHashCode()
        {
            int hashCode = -1210936446;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Color);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Vendor);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Model);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Engine);
            hashCode = hashCode * -1521134295 + Date.GetHashCode();
            hashCode = hashCode * -1521134295 + HP.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<CAR_EQUIPMENT>>.Default.GetHashCode(Arsenal);
            return hashCode;
        }
    }
    public class Person
    {
        public string Name { get; set; }

        public Person(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return "Name: " + Name;
        }

    }
    public class ShowCar : Car
    {
        public DateTime VendorArrival { get; private set; }
        public double Km { get; private set; }


        public ShowCar(string color, string vendor, string model, string engion,
            DateTime date, string horsePower, List<CAR_EQUIPMENT> arsenal
            , DateTime vendoeArrival, double km)
            : base(color, vendor, model, engion, date, horsePower, arsenal)
        {
            VendorArrival = vendoeArrival;
            Km = km;
        }


        public override string GetInternalCartypeString()
        {
            return "Show car";
        }

        public override bool Equals(object obj)
        {
            return obj is ShowCar car &&
                   VendorArrival == car.VendorArrival &&
                   Km == car.Km;
        }

        public override int GetHashCode()
        {
            int hashCode = 1531679703;
            hashCode = hashCode * -1521134295 + VendorArrival.GetHashCode();
            hashCode = hashCode * -1521134295 + Km.GetHashCode();
            return hashCode;
        }
        public class SoldCar : Car
        {
            public Person _costumer { get; private set; }
            public double _paidSum { get; private set; }
            public Person _serviceGiver { get; private set; }

            public SoldCar(string color, string vendor, string model, string engion, DateTime date,
                string horsePower, List<CAR_EQUIPMENT> arsenal,
                Person costumer, double paidSum, Person serviceGiver)
                : base(color, vendor, model, engion, date, horsePower, arsenal)
            {
                _costumer = costumer;
                _paidSum = paidSum;
                _serviceGiver = serviceGiver;
            }
            public override string GetInternalCartypeString()
            {
                return "Sold Car";
            }
            public override bool Equals(object obj)
            {
                return obj is SoldCar car &&
                        EqualityComparer<Person>.Default.Equals(_costumer, car._costumer) &&
                        _paidSum == car._paidSum &&
                        EqualityComparer<Person>.Default.Equals(_serviceGiver, car._serviceGiver);
            }

            public override int GetHashCode()
            {
                int hashCode = 1406259131;
                hashCode = hashCode * -1521134295 + EqualityComparer<Person>.Default.GetHashCode(_costumer);
                hashCode = hashCode * -1521134295 + _paidSum.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<Person>.Default.GetHashCode(_serviceGiver);
                return hashCode;
            }
        }
        public class Importer : Person
        {
            public List<Car> Cars { get; protected set; }

            public Importer(string name, List<Car> cars) : base(name)
            {
                Cars = cars;
            }
            public void addCar(Car car)
            {
                Cars.Add(car);
            }
            public void removeCar(Car car)
            {
                Cars.Remove(car);
            }
            public void RemoveFordAndAddTwo(Car c1, Car c2)
            {
                List<Car> copy = new List<Car>(Cars);
                copy.Add(c1);
                copy.Add(c2);
                for (int i = 0; i < copy.Count; i++)
                {
                    if (copy[i].Vendor == "Ford")
                        copy.Remove(copy[i]);
                }
                string str = "";
                for (int i = 0; i < copy.Count; i++)
                {
                    str += copy[i].ToString();
                }
                Console.WriteLine(str);
            }
            public bool IsContainSubList<T>(List<T> mainList, List<T> subList)
            {
                foreach (T item in subList)
                {
                    if (!(mainList.Contains(item)))
                    {
                        return false;
                    }
                }
                return true;
            }
            public void PrintCarsWithArsenal(List<CAR_EQUIPMENT> arsenal)
            {
                for (int i = 0; i < Cars.Count; i++)
                {
                    if (IsContainSubList<CAR_EQUIPMENT>(Cars[i].Arsenal, arsenal))
                        Console.WriteLine(Cars[i].ToString());
                }
            }
            public void PrintCarsNeedWash()
            {
                foreach (Car car in Cars)
                {
                    if (car.WashDate < DateTime.Now)
                        Console.WriteLine(car.ToString());
                }
            }
            public void PrintCarsReadyForSale()
            {
                foreach (Car car in Cars)
                {
                    if (car is SoldCar && car.NeedsWash == false)
                    {
                        Console.WriteLine(car.ToString());
                    }
                }
            }
            public void CarsToWash()
            {
                foreach (Car car in Cars)
                {
                    if (car is SoldCar && car.NeedsWash)
                    {
                        Console.WriteLine(car.ToString());
                    }
                }
            }
            public override string ToString()
            {
                string str = "";
                for (int i = 0; i < Cars.Count; i++)
                {
                    str += Cars[i].ToString();
                }
                return str + Name;
            }
        }
        public class Employee : Importer
        {
            public Employee(string name, List<Car> cars) : base(name, cars)
            {
            }

            private void AlertNeedWash(int CarNumber)
            {
                Cars[CarNumber].NeedsWash = true;
            }
            private void AlertDoesntNeedWash(int CarNumber)
            {
                Cars[CarNumber].NeedsWash = false;
            }
        }
        public class Program
        {
            static void Main(string[] args)
            {
                SoldCar tesla = new SoldCar("Black", "Tesla", "Model 3", "Electric Engine", DateTime.Now.AddDays(30),
                 "204", new List<CAR_EQUIPMENT>() { CAR_EQUIPMENT.E_SEAT_HEATER, CAR_EQUIPMENT.E_PANORAMIC_ROOF, CAR_EQUIPMENT.E_ELECTRIC_TAILGATE, CAR_EQUIPMENT.E_ENTERTAINMENT_SYSTEM, CAR_EQUIPMENT.E_FORWARD_ELECTRIC_SEATS }, new Person("Yuval"), 200000, new Person("Yossi"));
                Console.WriteLine(tesla);
                Console.WriteLine();

                ShowCar skoda = new ShowCar("Red", "Skoda", "Octavia", "Petrol Engine", DateTime.Now.AddDays(30),
                 "158", new List<CAR_EQUIPMENT>() { CAR_EQUIPMENT.E_ELECTRIC_TAILGATE, CAR_EQUIPMENT.E_FORWARD_ELECTRIC_SEATS, CAR_EQUIPMENT.E_DIGITAL_DRIVER_DISPLAY, CAR_EQUIPMENT.E_REAR_AC }, DateTime.Now.AddDays(-30), 50000);
                Console.WriteLine(skoda);
                Console.WriteLine();

                SoldCar ford = new SoldCar("Black", "Ford", "Focus", "Hybrid Engine", DateTime.Now.AddDays(-999),
                 "143", new List<CAR_EQUIPMENT>() { CAR_EQUIPMENT.E_SEAT_HEATER, CAR_EQUIPMENT.E_PANORAMIC_ROOF, CAR_EQUIPMENT.E_ELECTRIC_TAILGATE, CAR_EQUIPMENT.E_FORWARD_ELECTRIC_SEATS, CAR_EQUIPMENT.E_DIGITAL_DRIVER_DISPLAY, CAR_EQUIPMENT.E_REAR_AC }, new Person("Yuval"), 200000, new Person("Yossi"));
                Console.WriteLine(ford);
                Console.WriteLine();

                ShowCar Mitzi = new ShowCar("White", "Mitzibushi", "very new", "Petrol Engine", DateTime.Now.AddDays(-80),
                 "158", new List<CAR_EQUIPMENT>(), DateTime.Now.AddDays(-30), 5090000);

                ShowCar Ferrari = new ShowCar("Red", "Ferrari", "555", "Petrol Engine", DateTime.Now.AddDays(-30),
                 "158", new List<CAR_EQUIPMENT>(), DateTime.Now.AddDays(-30), 50000);

                List<Car> carsList = new List<Car>() { tesla, skoda, ford };

                Importer importer = new Importer("John", carsList);

                importer.RemoveFordAndAddTwo(Mitzi, Ferrari);

                List<CAR_EQUIPMENT> cAR_EQUIPMENT = new List<CAR_EQUIPMENT>() { CAR_EQUIPMENT.E_ENTERTAINMENT_SYSTEM, CAR_EQUIPMENT.E_PANORAMIC_ROOF, CAR_EQUIPMENT.E_ELECTRIC_TAILGATE };

                importer.PrintCarsWithArsenal(cAR_EQUIPMENT);

                importer.PrintCarsNeedWash();

                Employee employee = new Employee("Bob", importer.Cars);

                employee.AlertNeedWash(1);

                Console.WriteLine(importer.Cars[1].NeedsWash);

                importer.PrintCarsReadyForSale();

                importer.CarsToWash();

            }
        }
    }
}
