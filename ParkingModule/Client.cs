namespace ParkingModule;

public class Client
{
    public Client(int id, string numberAuto)
    {
        Id = id;
        NumberAuto = numberAuto;
    }

    public int Id { get; set; }
    public string NumberAuto { get; set; }
    
    // Liskov - у родителя стандартный метод, переопределяемый особым образом для ребенка (зарег. клиента)
    public virtual string GetClientInfo()
    {
        return $"ID: {Id}, Авто: {NumberAuto} (Гость)";
    }
    
}

public class RegisterClient: Client
{
    public RegisterClient(int id, string numberAuto, string login, string password) : base(id, numberAuto)
    {
        Login = login;
        Password = password;
    }

    public string Login { get; set; }
    public string Password { get; set; }
    
    // Liskov, переопределяем метод
    public override string GetClientInfo()
    {
        return $"ID: {Id}, Авто: {NumberAuto}, Username: {Login} (Зарегистрирован)";
    }
}


// OpenClosed - интерфейсы для операция (далее для них создаются отдельные классы)
// ISP - идет разделение обязанностей сервисов по интерфейсам для вывода сообщений и проведения оплаты
public interface IPaymentService
{
    void SellPayment(double amount, Client client);
}

// Dependency Inversion Principle - создаем интерфейс, а потом создадим класс для вывода сообщений
// *(разных, в зависимости от типа клиента - моделируем варианты сообщения на терминале или в смс)

public interface INotificationService
{
    void SendNotification(string message, Client client);
}


public class PaymentService: IPaymentService
{
    public void SellPayment(double amount, Client client)
    {
        Console.WriteLine($"Клиент -- {client.NumberAuto} платил {amount} руб.");
    }
}
    
// Single Responsibility Principle - оплата и вывод сообщения о выезде разделены


// Класс для вывода сообщений в зависимости от типа клиента
public class NotificationService: INotificationService
{
    public void SendNotification(string message, Client client)
    {
        if (client is RegisterClient registerClient)
        {
            Console.WriteLine($"SMS-Сообщение для клиента {registerClient.Login}: {message}");
        }
        else
        {
            Console.WriteLine($"Сообщение на терминале для клиента Рег.Номер: {client.NumberAuto}: {message}");
        }
            
    }
}





// Реализовываем класс верхнего уровня
public class Parking
{
    private readonly IPaymentService _paymentService;
    private readonly INotificationService _notificationService;

    public Parking(IPaymentService paymentService, INotificationService notificationService, int id, string adress, string city, bool isFill, int maxCountState, int currentCountState)
    {
        _paymentService = paymentService;
        _notificationService = notificationService;
        Id = id;
        Adress = adress;
        City = city;
        IsFill = isFill;
        this.maxCountState = maxCountState;
        this.currentCountState = currentCountState;
    }

    public int Id { get; set; }
    public string Adress { get; set; }
    public string City { get; set; }
    public bool IsFill { get; set; }
    public int maxCountState { get; set; }
    public int currentCountState { get; set; }

    
    public void AddAuto(Client client, double amount)
    {
        if (!IsFill)
        {
            _paymentService.SellPayment(amount, client);
            currentCountState++;
            Console.WriteLine("Парковка совершена: \n"+ client.GetClientInfo());
            Console.WriteLine("Мест занято - " + currentCountState);
            if (currentCountState == maxCountState)
            {
                IsFill = true;
            }
        }
        else
        {
            _notificationService.SendNotification("Отказано в парковке", client);
            Console.WriteLine("Нет свободных мест");
        }
    }
    
    public void DelAuto(Client client)
    {
        if (currentCountState > 0)
        {
            currentCountState--;
            Console.WriteLine("Парковку завершил: \n"+ client.GetClientInfo());
            Console.WriteLine("Мест занято - " + currentCountState);
            _notificationService.SendNotification($"Вы покинули парковку {Adress} -- {City}. Хорошей дороги", client);
            if (currentCountState < maxCountState)
            {
                IsFill = false;
            }
        }
        else
        {
            Console.WriteLine("Автомобили отсутствуют");
        }
        
    }
}