namespace DatingApp.Enums
{
    /// <summary>
    /// Numbers of database engine events and errors.
    /// <see href="https://learn.microsoft.com/en-us/sql/relational-databases/errors-events/database-engine-events-and-errors?view=sql-server-ver16"/>
    /// </summary>
    public enum SqlExceptionNumbers
    {
        /// <summary>
        /// Cannot insert duplicate key row in object '%.*ls' with unique index '%.*ls'. The duplicate key value is %ls.
        /// </summary>
        DuplicatedKeyRowInObject = 2601,

        /// <summary>
        /// Violation of %ls constraint '%.*ls'. Cannot insert duplicate key in object '%.*ls'. The duplicate key value is %ls.
        /// </summary>
        DuplicatedKeyInObject = 2627,

        /// <summary>
        /// The %ls statement conflicted with the %ls constraint "%.*ls". The conflict occurred in database "%.*ls", table "%.*ls"%ls%.*ls%ls.
        /// </summary>
        DependentObjectExists = 547,

        /// <summary>
        /// Invalid object name '%.*ls'.
        /// </summary>
        InvalidObjectName = 208,

        /// <summary>
        /// The target database, '%.*ls', is participating in an availability group and is currently not accessible for queries. Either data movement is suspended or the availability replica is not enabled for read access. To allow read-only access to this and other databases in the availability group, enable read access to one or more secondary availability replicas in the group. For more information, see the ALTER AVAILABILITY GROUP statement in SQL Server Books Online.
        /// </summary>
        TargetDatabaseNotAccessible = 976,

        /// <summary>
        /// Unable to access availability database '%.*ls' because the database replica is not in the PRIMARY or SECONDARY role. Connections to an availability database is permitted only when the database replica is in the PRIMARY or SECONDARY role. Try the operation again later.
        /// </summary>
        DatabaseNotAllowedConnections = 983
    }
}
