using System.Threading.Tasks;

namespace AsyncAwaitConsoleApp
{
    public class Message
    {
        public static async Task<Message> CreateMessageAsync(string text)
        {
            await Task.Delay(500);
            return new Message { Text = $"Text: {text}\n" };
        }


        public string Text { get; set; }
    }
}