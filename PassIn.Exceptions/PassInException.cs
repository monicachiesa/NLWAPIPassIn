namespace PassIn.Exceptions
{
    public class PassInException : Exception //classe vai herdar as exceptions
    {
        public PassInException(string message) : base(message) //pega a mensagem que eu recebi e repassa para o construtor
            //do system.exception
        {
            
        }
    }
}
