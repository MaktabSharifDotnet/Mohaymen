using Mohaymen.Enums;

while (true)
{
    Console.WriteLine("please enter command");

    string command = Console.ReadLine();
    string[] instructionParts = command.Split(' ');
    Enum instruction = (Enum)Enum.Parse(typeof(InstructionEnum), instructionParts[0]);
    switch (instruction)
    {
        case InstructionEnum.Register:
            break;
        case InstructionEnum.Login:
            break;
    }


}
