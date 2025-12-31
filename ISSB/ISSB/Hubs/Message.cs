using System;
namespace ISSB.Hubs
{
	public interface IMessage
	{
		string Send(string message);
	}

	public class Message : IMessage
    {
        public string Send(string message)
        {
            return message;
        }
    }
}
