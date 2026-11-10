namespace Speurzoekers.Common.Domain
{
    public class ActionResult
    {
        public bool Succeeded { get; set; }
        public ActionResultError[] Errors { get; set; }
    }

    public class ActionResultError
    {
        public const string DuplicateUserNameKey = "DuplicateUserName";
        public const string UserNamePendingKey = "UserNamePending";

        public string Code { get; set; }
        public string Description { get; set; }
    }
}
