﻿namespace Morpher.WebService.V3.General.Data
{
    public interface IUserDictionaryLookup
    {
        /// <summary>
        /// Получает пользовательское исправление из источника
        /// </summary>
        /// <param name="nominativeSingular">именительная форма, регистр не учитывается</param>
        /// <remarks>Вызвать toUpperCase/toLowerCase для nominativeSingular</remarks>
        /// <returns>Пользовательское исправление</returns>
        object Lookup(string nominativeSingular);
    }
}