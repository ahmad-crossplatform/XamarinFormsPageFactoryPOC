using System;

namespace ComplaintAppPageFactory.Attributes
{
    public class CommandAttribute : Attribute
    {
        public string Command { get; }


        public CommandAttribute(string command)
        {
            Command = command;
        }
    }
}