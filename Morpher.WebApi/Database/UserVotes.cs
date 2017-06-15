namespace Morpher.WebService.V3.Database
{
    using System;

    public class UserVotes
    {
        public Guid UserID { get; set; }

        public Guid NameId { get; set; }
    }
}