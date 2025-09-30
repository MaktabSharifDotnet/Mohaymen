using Mohaymen.DataAccess;
using Mohaymen.DTOs;
using Mohaymen.Enums;
using Mohaymen.Exceptions;
using Mohaymen.Manager;
using Mohaymen.repositories;
using Mohaymen.Services;
using System.Collections.Generic;

AppDbContext context = new AppDbContext();
UserRepository userRepository = new UserRepository(context);
MessageRepository messageRepository = new MessageRepository(context);
Service service = new Service(userRepository , messageRepository);

while (true)
{
    if (LocalStorage.LoginUser == null)
    {
        Console.WriteLine("please enter command");
        string command = Console.ReadLine();
        string[] instructionParts = command.Split(' ');
        try
        {
            Enum instruction = (Enum)Enum.Parse(typeof(InstructionEnum), instructionParts[0]);
            switch (instruction)
            {
                case InstructionEnum.Register:

                    AuthenticationDto registerCommandDto = ParseAuthCommand(instructionParts);
                    if (registerCommandDto.Username == null || registerCommandDto.Password == null)
                    {
                        Console.WriteLine("invalid command");
                    }
                    else
                    {
                        try
                        {
                            service.Register(registerCommandDto.Username, registerCommandDto.Password);
                            Console.WriteLine("register is done");
                        }
                        catch (UserAlreadyExistException e)
                        {
                            Console.WriteLine(e.Message);
                        }

                    }

                    break;
                case InstructionEnum.Login:
                    registerCommandDto = ParseAuthCommand(instructionParts);
                    if (registerCommandDto.Username == null || registerCommandDto.Password == null)
                    {
                        Console.WriteLine("invalid command");
                    }
                    else
                    {
                        try
                        {
                            service.Login(registerCommandDto.Username, registerCommandDto.Password);
                            Console.WriteLine("Login is done");
                        }
                        catch (UserNotFoundException e)
                        {
                            Console.WriteLine(e.Message);
                        }


                    }

                    break;
            }
        }
        catch (ArgumentException)
        {
            Console.WriteLine("The input is invalid.  ");
        }
    }
    else
    {
        ShowMenu();
        try
        {
            int option = int.Parse(Console.ReadLine());
            switch (option)
            {
                case 1:
                    Console.WriteLine("please enter command for ChangeStatus:");
                    string command = Console.ReadLine();
                    string[] instructionParts = command.Split(' ');
                    try
                    {
                        InstructionEnum instruction = (InstructionEnum)Enum.Parse(typeof(InstructionEnum), instructionParts[0]);
                        string? status = ParseStatusCommand(instructionParts);
                        if (status == null)
                        {
                            Console.WriteLine("Status not entered");
                        }
                        try
                        {
                            StatusEnum statusEnum = (StatusEnum)Enum.Parse(typeof(StatusEnum), status);
                            string result = service.ChangeStatus(statusEnum);
                            Console.WriteLine(result);
                        }
                        catch (ArgumentException)
                        {
                            Console.WriteLine("invalid status . status is available or notavailable");
                        }
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("The input is invalid.");
                    }
                    break;
                case 2:
                    Console.WriteLine("please enter command for Search:");
                    string commandSearch = Console.ReadLine();
                    instructionParts = commandSearch.Split(' ');
                    try
                    {
                        InstructionEnum instructionEnum = (InstructionEnum)Enum.Parse(typeof(InstructionEnum), instructionParts[0]);
                        string? username = ParseSearchCommand(instructionParts);
                        if (username == null)
                        {
                            Console.WriteLine("username not entered");
                        }
                        List<User> users = service.SearchUsername(username);
                        DisplayUsers(users);
                    }
                    catch (NotLogInException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("invalid Search . instruction search is Search");
                    }
                    break;
                case 3:
                    ShowUsers();
                    Console.WriteLine("please enter command for Send message:");
                    string commandSend = Console.ReadLine();
                    instructionParts = commandSend.Split(' ');
                    try
                    {
                        InstructionEnum instructionSend = (InstructionEnum)Enum.Parse(typeof(InstructionEnum), instructionParts[0]);
                        SendMessageDto messageDto = ParseSendMessageCommand(instructionParts);
                        if (messageDto.Username == null || messageDto.Text == null)
                        {
                            Console.WriteLine("invalid command , username or text is null");
                        }
                        else
                        {
                            service.SendMessage(messageDto.Username, messageDto.Text);
                            Console.WriteLine($"message sent to {messageDto.Username}");
                        }
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("invalid Instruction SendMessage ");
                    }
                    catch (NotLogInException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (UserNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;
                case 4:
                    Console.WriteLine("please enter command for Inbox:");
                    string commandSent = Console.ReadLine();
                    try 
                    {
                        InstructionEnum instruction = (InstructionEnum)Enum.Parse(typeof(InstructionEnum), commandSent);
                        if (InstructionEnum.Inbox==instruction) 
                        {
                            List<Message> messages = service.Inbox();
                            ShowMessage(messages);
                        }
                        else 
                        {
                            Console.WriteLine("command is wrong ");
                        }
                    }
                    catch (ArgumentException) 
                    {
                        Console.WriteLine("invalid commandSent please enter Sent");
                    }
                   
                    break;

            }
        }
        catch (FormatException)
        {
            Console.WriteLine("invalid option Please select an option from the list provided.");
        }
    }

}
AuthenticationDto ParseAuthCommand(string[] instructionParts)
{
    string username = null;
    string password = null;
    for (int i = 0; i < instructionParts.Length; i++)
    {
        if (instructionParts[i] == "--username")
        {

            if (i + 1 < instructionParts.Length && !instructionParts[i + 1].StartsWith("--"))
            {
                username = instructionParts[i + 1];
            }
        }
        else if (instructionParts[i] == "--password")
        {

            if (i + 1 < instructionParts.Length && !instructionParts[i + 1].StartsWith("--"))
            {
                password = instructionParts[i + 1];
            }
        }
    }
    AuthenticationDto registerCommandDto = new AuthenticationDto
    {
        Username = username,
        Password = password
    };
    return registerCommandDto;
}

string? ParseStatusCommand(string[] instructionParts)
{
    string status = null;
    for (int i = 0; i < instructionParts.Length; i++)
    {
        if (instructionParts[i] == "--status" && i + 1 < instructionParts.Length)
        {
            status = instructionParts[i + 1];
        }
    }
    return status;
}

string? ParseSearchCommand(string[] instructionParts)
{
    string username = null;
    for (int i = 0; i < instructionParts.Length; i++)
    {
        if (instructionParts[i] == "--username" && i + i < instructionParts.Length)
        {
            username = instructionParts[i + 1];
        }
    }
    return username;
}

SendMessageDto ParseSendMessageCommand(string[] instructionParts)
{
    string username = null;
    string text = null;
    for (int i = 0; i < instructionParts.Length; i++)
    {
        if (instructionParts[i] == "--to" && i + 1 < instructionParts.Length  && !instructionParts[i + 1].StartsWith("--"))
        {
            username = instructionParts[i + 1];
        }
        if (instructionParts[i] == "--text" && i + 1 < instructionParts.Length  && !instructionParts[i + 1].StartsWith("--"))
        {
            text = instructionParts[i + 1];
        }
    }
    var send = new SendMessageDto
    {
        Text = text,
        Username = username
    };
    return send;

}
void ShowMenu()
{
    Console.WriteLine("Select the desired option.");
    Console.WriteLine("1.ChangeStatus");
    Console.WriteLine("2.Search");
    Console.WriteLine("3.send message");
    Console.WriteLine("4.Inbox");
}
void ShowMessage(List<Message> messages) 
{
    foreach (var message in messages)
    {
        Console.WriteLine($"From:{message.Sender.Username}|{message.Text}");
    }

}
void ShowUsers()
{
    List<User> users = service.GetUsers();
    foreach (User user in users)
    {
        if (LocalStorage.LoginUser.Id!=user.Id)
        {
            Console.WriteLine($"userId:{user.Id} , username:{user.Username}");
        }     
    }
}
void DisplayUsers(List<User> users)
{
    foreach (var user in users)
    {
        Console.WriteLine($"{user.Username} | status:{user.Status}");
    }
}
