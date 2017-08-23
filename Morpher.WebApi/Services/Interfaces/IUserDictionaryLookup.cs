namespace Morpher.WebService.V3.Services.Interfaces
{
    using System;
    using Models;

    public interface IUserDictionaryLookup
    {
        /// <summary>
        /// Получает пользовательское исправление из источника
        /// </summary>
        /// <param name="nominativeSingular">именительная форма, регистр не учитывается</param>
        /// <param name="token">токен пользователя, null в случае local сервиса</param>
        /// <remarks>Вызвать toUpperCase/toLowerCase для nominativeSingular</remarks>
        /// <returns>Пользовательское исправление</returns>
        RussianEntry Lookup(string nominativeSingular, Guid? token);
    }
}