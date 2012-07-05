namespace Marina.Store.Web.MailService
{
    /// <summary>
    /// Сервис, отвечающий за взаимодействие с пользователями по емайлу
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// Отправка подтверждения реггистрации
        /// </summary>
        void SendUserConfirmatiom(string email, string registrationRequestId);
    }
}