using Mohaymen.DataAccess;
using Mohaymen.DTOs;
using Mohaymen.Enums;
using Mohaymen.Exceptions;
using Mohaymen.Manager;
using Mohaymen.repositories;
using Mohaymen.Services;
using System.Collections.Generic;
using System.IO;

AppDbContext appDbContext = new AppDbContext();
UserRepository userRepository = new UserRepository(appDbContext);
MessageRepository messageRepository = new MessageRepository(appDbContext);
Service service = new Service(userRepository,messageRepository);

while (true)
{
    if (LocalStorage.LoginUser == null)
    {
        Console.WriteLine("please enter command:");
        string command = Console.ReadLine();
        string[] commandParts = command.Split(' ');
        try
        {
            InstructionEnum instructionEnum = (InstructionEnum)Enum.Parse(typeof(InstructionEnum), commandParts[0]);
            switch (instructionEnum)
            {
                case InstructionEnum.Register:

                    AuthenticationDto authentication = ParseAuthCommand(commandParts);
                    if (authentication.Username == null || authentication.Password == null)
                    {
                        Console.WriteLine("The username or password is invalid or empty.");
                    }
                    else
                    {
                        try
                        {
                            service.Register(authentication.Username, authentication.Password);
                            Console.WriteLine("register is done");
                        }
                        catch (UserAlreadyExistException e)
                        {
                            Console.WriteLine(e.Message);
                        }

                    }
                    break;
                case InstructionEnum.Login:

                    authentication = ParseAuthCommand(commandParts);
                    if (authentication.Username == null || authentication.Password == null)
                    {
                        Console.WriteLine("The username or password is invalid or empty.");
                    }
                    else
                    {
                        try
                        {
                            service.Login(authentication.Username, authentication.Password);
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
            Console.WriteLine("invalid command");
        }
    }

    else
    {
        try
        {
            ShowMenu();
            int option = int.Parse(Console.ReadLine());
            switch (option)
            {
                case 1:
                    Console.WriteLine("please enter command for Change status:");
                    string commandChangeStatus = Console.ReadLine();
                    string[] commandChangeStatusArray = commandChangeStatus.Split(" ");
                    try
                    {
                        InstructionEnum instructionEnum = (InstructionEnum)Enum.Parse(typeof(InstructionEnum), commandChangeStatusArray[0]);
                        if (InstructionEnum.Change == instructionEnum)
                        {
                            string status = ParseStatusCommand(commandChangeStatusArray);
                            if (status == null)
                            {
                                Console.WriteLine("Status not entered");
                            }
                            else
                            {
                                try
                                {
                                    StatusEnum statusEnum = (StatusEnum)Enum.Parse(typeof(StatusEnum), status);
                                    string result = service.ChangeStatus(statusEnum);
                                    Console.WriteLine($"status change to {result}");
                                }
                                catch (ArgumentException)
                                {
                                    Console.WriteLine("invalid status");
                                }
                                catch (NotLogInException e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("command for change status is wrong");
                        }
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("invalid command for change status");
                    }




                    break;
                case 2:
                    Console.WriteLine("please enter command for search");
                    string commandSearch = Console.ReadLine();
                    string[] commandSearchArray = commandSearch.Split(" ");
                    try
                    {

                        InstructionEnum instructionEnum = (InstructionEnum)Enum.Parse(typeof(InstructionEnum), commandSearchArray[0]);
                        if (InstructionEnum.Search == instructionEnum)
                        {
                            string? username = ParseSearchCommand(commandSearchArray);
                            if (username == null)
                            {
                                Console.WriteLine("You did not enter a username.");
                            }
                            else
                            {
                                List<User> users = service.Search(username);
                                ShowListUsers(users);
                            }
                        }
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("invalid instructon for search ");
                    }


                    break;
                case 3:
                    ShowUsers();
                    Console.WriteLine("please enter command for SendMessage");
                    string commandSendMessage = Console.ReadLine();
                    string[] commandSendMessageArray = commandSendMessage.Split(" ");
                    try 
                    {
                        SendMessageDto sendMessageDto = ParseSendMessageCommand(commandSendMessageArray);
                        if (sendMessageDto.Text ==null|| sendMessageDto.SenderUsername==null)
                        {
                            Console.WriteLine("text or Sender not entered");
                        }
                        InstructionEnum instructionEnum = (InstructionEnum)Enum.Parse(typeof(InstructionEnum), commandSendMessageArray[0]);
                        if (InstructionEnum.SendMessage == instructionEnum)
                        {
                            service.SendMessage(sendMessageDto.SenderUsername, sendMessageDto.Text);
                            Console.WriteLine("sent message");
                        }
                    }
                    catch (ArgumentException) 
                    {
                        Console.WriteLine("invalid command for SendMessage");
                    }
                    catch (UserNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("invalid option Please select an option from the list.");
        }

    }

}
void ShowMenu()
{
    Console.WriteLine("please enter option");
    Console.WriteLine("1.Change Status");
    Console.WriteLine("2.Search");
    Console.WriteLine("3.Send Message");

}

void ShowListUsers(List<User> users)
{
    foreach (User user in users)
    {
        Console.WriteLine($"{user.Username}|status:{user.Status}");
    }
}
AuthenticationDto ParseAuthCommand(string[] instructionParts)
{
    string username = null;
    string password = null;
    for (int i = 0; i < instructionParts.Length; i++)
    {
        if (instructionParts[i] == "--username" && i + 1 < instructionParts.Length && !instructionParts[i + 1].StartsWith("--"))
        {
            username = instructionParts[i + 1];
        }
        if (instructionParts[i] == "--password" && i + 1 < instructionParts.Length && !instructionParts[i + 1].StartsWith("--"))
        {
            password = instructionParts[i + 1];
        }
    }
    AuthenticationDto authenticationDto = new AuthenticationDto
    {
        Username = username,
        Password = password
    };
    return authenticationDto;
}

string ParseStatusCommand(string[] commandChangeStatusArray)
{

    string status = null;
    for (int i = 0; i < commandChangeStatusArray.Length; i++)
    {
        if (commandChangeStatusArray[i] == "--status" && i + 1 < commandChangeStatusArray.Length && !commandChangeStatusArray[i + 1].StartsWith("--"))
        {
            status = commandChangeStatusArray[i + 1];
        }
    }
    return status;
}

string? ParseSearchCommand(string[] commandSearchArray)
{
    string username = null;
    for (int i = 0; i < commandSearchArray.Length; i++)
    {
        if (commandSearchArray[i] == "--username" && i + 1 < commandSearchArray.Length && !commandSearchArray[i + 1].StartsWith("--"))
        {
            username = commandSearchArray[i + 1];
        }
    }
    return username;
}

SendMessageDto ParseSendMessageCommand(string[] commandSendMessageArray) 
{
    string senderUsername = null;
    string text = null;
    for (int i = 1; i < commandSendMessageArray.Length; i++)
    {
        if (commandSendMessageArray[i] == "--to" && i + 1 < commandSendMessageArray.Length && !commandSendMessageArray[i + 1].StartsWith("--"))
        {
            senderUsername = commandSendMessageArray[i + 1];
        }
        if (commandSendMessageArray[i] == "--text" && i + 1 < commandSendMessageArray.Length && !commandSendMessageArray[i + 1].StartsWith("--"))
        {
            text = commandSendMessageArray[i + 1];
        }
    }
    SendMessageDto sendMessageDto = new SendMessageDto 
    {
        SenderUsername = senderUsername,
        Text = text
    };
    return sendMessageDto;

}

void ShowUsers() 
{
    List<User> users=service.GetUsers();
    foreach (User user in users) 
    {
       
        Console.WriteLine($"Id:{user.Id}|{user.Username}");
    }

}