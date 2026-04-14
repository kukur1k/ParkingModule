namespace ParkingModule;

// Open-Closed
public class Client
{
    public int Id { get; set; }
    public string NumberAuto { get; set; }
    
    // Liskov
    public virtual string GetClientInfo()
    {
        return $"ID: {Id}, Авто: {NumberAuto} (Гость)";
    }
    
}

public class RegisterClient: Client
{
    public string Login { get; set; }
    public string Password { get; set; }
    
    // Liskov, переопределяем метод
    public override string GetClientInfo()
    {
        return $"ID: {Id}, Авто: {NumberAuto}, Username: {Login} (Зарегистрирован)";
    }
}


public interface IPaymentService
{
    void SellPayment(double amount, Client client);
}

public interface INotificationService
{
    void SendNotification(string message, Client client);
}







public class Parking
{
    private readonly IPaymentService _paymentService;
    private readonly INotificationService _notificationService;
    
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



    public class PaymentService: IPaymentService
    {
        public void SellPayment(double amount, Client client)
        {
            Console.WriteLine($"Клиент -- {client.NumberAuto} платил {amount} руб.");
        }
    }
    
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
    
    
}