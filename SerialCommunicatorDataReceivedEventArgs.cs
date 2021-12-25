namespace raspai_blazor
{
    public class SerialCommunicatorDataReceivedEventArgs
    {
        public string RecieveData { get; }

        public SerialCommunicatorDataReceivedEventArgs(string recieveData)
        {
            this.RecieveData = recieveData;
        }
    }
}
