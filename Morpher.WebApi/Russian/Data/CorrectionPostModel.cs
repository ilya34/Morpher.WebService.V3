namespace Morpher.WebService.V3.Russian.Data
{
    /// <summary>
    /// Microsoft: "At most one parameter is allowed to read from the message body."
    /// Этот класс нужен что бы получать пользовательские исправление как x-form-www-urlencoded
    /// </summary>
    /// <remarks>Да я знаю про IModelBinder, но Сергей С. опять будет говорить про reflection и что можно проще)</remarks>
    public class CorrectionPostModel
    {
        public string И { get; set; }
        public string Р { get; set; }
        public string Д { get; set; }
        public string В { get; set; }
        public string Т { get; set; }
        public string П { get; set; }
        public string П_о { get; set; }
        public string М_И { get; set; }
        public string М_Р { get; set; }
        public string М_Д { get; set; }
        public string М_В { get; set; }
        public string М_Т { get; set; }
        public string М_П { get; set; }
        public string М_П_о { get; set; }

        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(И)
                   || string.IsNullOrWhiteSpace(Р)
                   || string.IsNullOrWhiteSpace(Д)
                   || string.IsNullOrWhiteSpace(В)
                   || string.IsNullOrWhiteSpace(Т)
                   || string.IsNullOrWhiteSpace(П)
                   || string.IsNullOrWhiteSpace(П_о)
                   || string.IsNullOrWhiteSpace(М_И)
                   || string.IsNullOrWhiteSpace(М_Р)
                   || string.IsNullOrWhiteSpace(М_Д)
                   || string.IsNullOrWhiteSpace(М_В)
                   || string.IsNullOrWhiteSpace(М_Т)
                   || string.IsNullOrWhiteSpace(М_П)
                   || string.IsNullOrWhiteSpace(М_П_о);
        }
    }
}