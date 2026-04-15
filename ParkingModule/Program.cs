// See https://aka.ms/new-console-template for more information


using ParkingModule;

Client unRegClient = new Client("O007EA197");
RegisterClient regClient = new RegisterClient("O008EA197", "ivanov", "qwerty");

// интерфейсы (сервисы)
INotificationService notificationService = new NotificationService();
IPaymentService cashPayment = new PaymentServiceCash();
IPaymentService cardPayment = new PaymentServiceCard();

Parking parking = new Parking(
    notificationService, 1, 
    "Ул. Б.Покровская, 61",
    "Нижний Новгород",
    2
    );

// оплата  мест достаточно
Console.WriteLine("Въезд и оплата (мест достаточно)");
parking.AddAuto(unRegClient, 200, cardPayment);

Console.WriteLine("Въезд и оплата (мест достаточно)");
parking.AddAuto(regClient, 200, cashPayment);


// оплата картой мест достаточно
parking.AddAuto(regClient, 200, cardPayment);
Console.WriteLine("Попытка въезда на парковку зарегистированного клиента - СМС");

parking.AddAuto(unRegClient, 200, cardPayment);
Console.WriteLine("Попытка въезда на парковку зарегистированного клиента - Сообщение в терминале");

// Выезд с парковки 
parking.DelAuto(regClient);
Console.WriteLine("Выезд зарегистированного клиента - Сообщение в СМС");

parking.DelAuto(unRegClient);
Console.WriteLine("Выезд незарегистированного клиента - Сообщение в терминале");
    
    