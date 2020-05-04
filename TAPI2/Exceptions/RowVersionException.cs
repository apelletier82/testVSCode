using System;
namespace TAPI2.Exceptions
{
    public class RowVersionException : EntityNotFoundException
    {
        const string _msg = "{0} already updated by another user";
        public RowVersionException(Type entityType) : base(string.Format(_msg, entityType.Name)) { }
        public RowVersionException(Type entityType, Exception innerException) : base(string.Format(_msg, entityType.Name), innerException) { }
    }
}