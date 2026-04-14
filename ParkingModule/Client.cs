namespace ParkingModule;

public class Client
{
    public int Id { get; set; }
    public string NumberAuto { get; set; }
    
}

public class RegisterClient: Client
{
    public string Login { get; set; }
    public string Password { get; set; }
}


public class Parking
{
    public int Id { get; set; }
    public string Adress { get; set; }
    public string City { get; set; }
    public bool IsFill { get; set; }
    public int maxCountState { get; set; }
    
    public int currentCountState { get; set; }

    public void AddAuto()
    {
        if (!IsFill)
        {
            currentCountState++;
            if (currentCountState == maxCountState)
            {
                IsFill = true;
            }
        }
        else
        {
            Console.WriteLine("Нет свободных мест");
        }
    }
    
    public void DelAuto()
    {
        if (currentCountState != 0)
        {
            currentCountState--;
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