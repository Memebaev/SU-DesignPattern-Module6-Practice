//1
public interface ICostCalculationStrategy
{
    double Calculate(double distance, int passengerCount, Klass klass, bool hasDiscount);
}
public enum Klass { Economy, Business }
public class AirplaneStrategy : ICostCalculationStrategy
{
    public double Calculate(double distance, int passengerCount, Klass klass, bool hasDiscount)
    {
        double price = distance * 0.5;
        if (klass == Klass.Business)
        {
            price = price * 2;
        }
        double total = price * passengerCount;
        if (hasDiscount)
        {
            total = total * 0.75;
        }
        return total;
    }
}
public class TrainStrategy : ICostCalculationStrategy
{
    public double Calculate(double distance, int passengerCount, Klass klass, bool hasDiscount)
    {
        double price = distance * 0.25;

        if (klass == Klass.Business)
        {
            price = price * 2;
        }
        double total = price * passengerCount;
        if (hasDiscount)
        {
            total = total * 0.75;
        }

        return total;
    }
}
public class BusStrategy : ICostCalculationStrategy
{
    public double Calculate(double distance, int passengerCount, Klass klass, bool hasDiscount)
    {
        double total = distance * 0.05 * passengerCount;
        if (hasDiscount)
        {
            total = total * 0.9;
        }
        return total;
    }
}
public class TravelBookingContext
{
    ICostCalculationStrategy strategy;
    public void SetStrategy(ICostCalculationStrategy strategy)
    {
        this.strategy = strategy;
    }
    public double CalculateTravelCost(double distance, int passengers, Klass klass, bool discount)
    {
        return strategy.Calculate(distance, passengers, klass, discount);
    }
}

//2
public interface IObserver
{
    void Update();
}
public interface ISubject
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify();
}
public class StockExchange : ISubject
{
    private List<IObserver> observers = new List<IObserver>();
    private decimal price;
    public decimal GetPrice()
    {
        return price;
    }
    public void SetPrice(decimal price)
    {
        this.price = price;
        Notify();
    }
    public void Attach(IObserver observer)
    {
        observers.Add(observer);
    }
    public void Detach(IObserver observer)
    {
        observers.Remove(observer);
    }
    public void Notify()
    {
        foreach (IObserver observer in observers)
        {
            observer.Update();
        }
    }
}
public class Trader : IObserver
{
    private StockExchange stockExchange;
    public Trader(StockExchange stockExchange)
    {
        this.stockExchange = stockExchange;
    }
    public void Update()
    {
        Console.WriteLine($"trader: {stockExchange.GetPrice()}");
    }
}
public class Robot : IObserver
{
    private StockExchange stockExchange;
    public Robot(StockExchange stockExchange)
    {
        this.stockExchange = stockExchange;
    }
    public void Update()
    {
        decimal price = stockExchange.GetPrice();

        if (price > 500)
            Console.WriteLine("Robot: sell");
        else if (price < 500)
            Console.WriteLine("Robot: buy");
        else
            Console.WriteLine("Robot: tormozi");
    }
}

class Program
{
    static void Main()
    {
        TravelBookingContext context = new TravelBookingContext();
        Console.WriteLine("1-samolet, 2-poezd, 3-bus");
        string c = Console.ReadLine();
        switch (c)
        {
            case "1":
                context.SetStrategy(new AirplaneStrategy());
                break;
            case "2":
                context.SetStrategy(new TrainStrategy());
                break;
            case "3":
                context.SetStrategy(new BusStrategy());
                break;
            default:
                Console.WriteLine("eto che");
                break;
        }
        Console.WriteLine("km:");
        double km = double.Parse(Console.ReadLine());
        Console.WriteLine("people:");
        int people = int.Parse(Console.ReadLine());
        double cost = context.CalculateTravelCost(km, people, Klass.Business, true);
        Console.WriteLine(cost);

        StockExchange stockExchange = new StockExchange();
        Trader trader = new Trader(stockExchange);
        Robot robot = new Robot(stockExchange);
        stockExchange.Attach(trader);
        stockExchange.Attach(robot);
        stockExchange.SetPrice(10000);
        stockExchange.SetPrice(100);
        stockExchange.SetPrice(500);
    }
}